/*
	TurnManager.cs
	Created 12/4/2017 2:14:15 PM
	Project Boardgame by Base Games
*/
using PlayerCharacters;
using System.Collections.Generic;
using UI;
using UnityEngine;
using Inventory;
using Combat;
using System.Collections;
using UnityEngine.EventSystems;
using Serialization;
using UnityEngine.UI;
using DG.Tweening;

namespace Utility
{
    public class TurnManager : MonoBehaviour
    {
        public delegate void ResetTurnAction();
        public static event ResetTurnAction OnResetTurn;

        [SerializeField] private RotateSpinner _spinner;
        [SerializeField] private GameObject _KOPopUp;

        private List<PlayerMovement> _players = new List<PlayerMovement>();
        [HideInInspector] public PlayerMovement ActivePlayer;
        private List<PlayerCharacter> _playerCharacters = new List<PlayerCharacter>();
        [HideInInspector] public PlayerCharacter ActivePlayerCharacter;
        [SerializeField] private Transform _turnTextBackground;
        [SerializeField] private Text _turnText;

        private int _playerTurnIndex = 0;

        public static int TurnMutation = 0;

        [SerializeField] private Button _accessInventoryButton;

        public List<PlayerMovement> Players
        {
            get { return _players; }
        }

        private void OnEnable()
        {
            GameStateManager.CurrentGameState = GameState.WORLD;
            BoardTile.OnTileEventDone += ChangePlayerTurn;
            ConfirmMoveMenu.OnConfirmMove += ExecuteTileEvent;
            PlayerMovement.OnFinishedPathMove += ExecuteTileEvent;
        }

        private void Start()
        {
            BattleSceneTransition.TransitionOut();
            _playerTurnIndex = PlayerPrefs.GetInt(InlineStrings.PLAYERTURNINDEX, 0);
            foreach (GameObject playerGO in GameObject.FindGameObjectsWithTag(InlineStrings.PLAYERTAG))
            {
                _players.Add(playerGO.GetComponent<PlayerMovement>());
                _playerCharacters.Add(playerGO.GetComponent<PlayerCharacter>());
            }
            ResetPlayerSprites();
            ChangePlayerTurn(TurnMutation);
            OverWorldStatUI.OnShowStats(ActivePlayerCharacter);
            TurnMutation = 0;
        }

        private void OnDisable()
        {
            BoardTile.OnTileEventDone -= ChangePlayerTurn;
            ConfirmMoveMenu.OnConfirmMove -= ExecuteTileEvent;
            PlayerMovement.OnFinishedPathMove -= ExecuteTileEvent;
        }

        public void StopSpin()
        {
            _spinner.StopRotation();
            ActivePlayer.NumberOfMoves += _spinner.NumberOfMoves;
            ActivePlayer.PlayerHasMoves = true;
        }

        public void ExecuteTileEvent()
        {
            ActivePlayer.PlayerHasMoves = false;
            ActivePlayer.CanMoveAnyPlace = false;
            ActivePlayer.MapScript.BoardTiles[ActivePlayer.PositionOnMap.y, ActivePlayer.PositionOnMap.x].PlayerLandsOnTile(ActivePlayerCharacter);
            ActivePlayer.CanMove = false;
        }

        public void ChangePlayerTurn(int turnMutation)
        {
            
            if(ActivePlayer != null)
                ActivePlayer.CanMoveAnyPlace = false;

            _playerTurnIndex += turnMutation;
            if (_playerTurnIndex > _players.Count - 1)
                _playerTurnIndex = 0;

            PlayerPrefs.SetInt(InlineStrings.PLAYERTURNINDEX, _playerTurnIndex);

            for (int i = 0; i < _players.Count; i++)
            {
                _players[i].IsPlayerTurn = false;
                _players[i].ChangeSortingOrder(2);
            }

            ActivePlayerCharacter = _playerCharacters[_playerTurnIndex];

            ActivePlayer = _players[_playerTurnIndex];
            ShowTurnText(ActivePlayerCharacter.CharacterName);
            if (CameraFollowPlayer.OnChangeTarget != null)
                CameraFollowPlayer.OnChangeTarget(ActivePlayer.transform);

            ActivePlayer.IsPlayerTurn = true;
            ActivePlayer.ChangeSortingOrder(5);
            MoveCameraTo.OnMoveToPlayer(ActivePlayer.transform);
            OverWorldStatUI.OnShowStats(ActivePlayerCharacter);

            //ActivePlayerCharacter._PlayerInventory.AddInventoryItem("Lock Down");

            if (OnResetTurn != null)
            {
                OnResetTurn();
            }
            
            StartCoroutine(InstantBattleRoutine());
            StatusEffectCountdown(ActivePlayerCharacter);
        }

        IEnumerator InstantBattleRoutine()
        {
            yield return new WaitForEndOfFrame();
            GameStateManager.TimeGameState = GameState.PAUSED;
            if (IsInBattle(ActivePlayerCharacter))
            {
                if (PlayerOptionsMenu.OnEnableOrDisableMenu != null)
                    PlayerOptionsMenu.OnEnableOrDisableMenu(false);

                if(EventSystem.current != null)
                    EventSystem.current.gameObject.SetActive(false);

                yield return new WaitForSeconds(2f);
                ExecuteTileEvent();
            }
            else
            {
                if (PlayerOptionsMenu.OnEnableOrDisableMenu != null)
                    PlayerOptionsMenu.OnEnableOrDisableMenu(true);

                ActivePlayer.CanMove = true;
            }
        }

        void ShowTurnText(string charactersName)
        {
            _turnText.text = charactersName + "'s " + "turn";
            Sequence scaleSequence = DOTween.Sequence();
            scaleSequence.Append(_turnTextBackground.DOScale(1, 0.75f));
            scaleSequence.AppendInterval(0.8f);
            scaleSequence.Append(_turnTextBackground.DOScale(0, 0.4f));
        }

        /// <summary>
        /// Checks if character has any status effects (buff , debuff or movement impairment)
        ///If character does have status, reduce turn count of status lasting by 1
        ///If lasting turns hits 0 , remove status
        /// </summary>
        /// <param name="pCharacter"></param>
        private void StatusEffectCountdown(PlayerCharacter pCharacter)
        {
            if (pCharacter.TurnsBuffed > 0)
            {
                pCharacter.TurnsBuffed--;
            }
            else
            {
                pCharacter.RemoveBonusStats();
            }

            if (pCharacter.TurnsDebuffed > 0)
            {
                pCharacter.TurnsDebuffed--;
            }
            else
            {
                pCharacter.RemoveBonusStats();
            }

            if (pCharacter.TurnsMovementImpaired > 0)
            {
                pCharacter.TurnsMovementImpaired--;
            }
            
            if (pCharacter.TurnsInventoryLocked > 0)
            {
                pCharacter.TurnsInventoryLocked--;
            }

            if (pCharacter.TurnsDead > 0)
            {
                _KOPopUp.SetActive(true);
                KOPopUp.OnRecoveryTextEvent(pCharacter);
                pCharacter.TurnsDead--;
                StartCoroutine(DisableOptionsMenuDelay());
            }
            else
            {
                pCharacter.Revive(); //This function checks if the player was dead as a failsafe
                OverWorldStatUI.OnShowStats(pCharacter);
            }
        }

        private IEnumerator DisableOptionsMenuDelay()
        {
            yield return new WaitForEndOfFrame();
            if (PlayerOptionsMenu.OnEnableOrDisableMenu != null)
                    PlayerOptionsMenu.OnEnableOrDisableMenu(false);
        }

        public bool IsInBattle(PlayerCharacter player)
        {
            GameObject ongoingBattlesObject = GameObject.Find(InlineStrings.ONGOINGBATTLESTAG);

            OngoingBattles ongoingBattles = ongoingBattlesObject.GetComponent<OngoingBattles>();

            foreach (var key in ongoingBattles.ongoingBattles)
            {
                if (player.gameObject == key.Value.Character1)
                    return true;
                else if (player.gameObject == key.Value.Character2)
                    return true;
            }

            return false;
        }

        private void ResetPlayerSprites()
        {
            for (int i = 0; i < _playerCharacters.Count; i++)
            {
                _playerCharacters[i].ResetSpriteFlip();
            }
        }
    }
}