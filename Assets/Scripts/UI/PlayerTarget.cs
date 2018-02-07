/*
	PlayerTarget.cs
	Created 12/19/2017 11:20:28 AM
	Project Boardgame by Base Games
*/
using Inventory;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Utility;

namespace UI
{
    public enum TargetingMode
    {
        Players,
        Towns,
        AnyTile,
        Nothing,
        Disabled
    }

    public class PlayerTarget : MonoBehaviour
    {
        public delegate void ItemUsageCanceledAction();
        public static event ItemUsageCanceledAction OnItemUsageCanceled;

        public delegate void BoardTargetSelected();
        public static event BoardTargetSelected OnBoardTargetSelected;

        public static UsableItem _UsableItem;

        public static Character Target;
        public static TownTile TownTarget;
        public static BoardTile BoardTarget;

        private TurnManager _turnManager;

        [SerializeField] private GameObject _buttonParent;
        [SerializeField] private Button[] _confirmTargetButtons;

        public static bool CanSelectAnyone = true;
        public static bool IsSelectingTarget = false;
        private bool _canTarget;
        private bool _confirmingTarget;
        private bool _isTargetSelectionOpen;

        public static TargetingMode _TargetingMode = TargetingMode.Players;

        private List<Character> _potentialTargets;
        [SerializeField] private GameObject _confirmMenu;
        [SerializeField] private Text _confirmMenuText;

        private void OnEnable()
        {
            _canTarget = false;
            IsSelectingTarget = true;
            InputManager.OnAButton += SelectTarget;
            InputManager.OnBButton += CloseTargetSelection;
            InputManager.OnBButton += CancelItemUsage;

            _turnManager = GameObject.FindWithTag(InlineStrings.TURNMANAGERTAG).GetComponent<TurnManager>();
            StartCoroutine(UnpauseDelay());
        }

        private IEnumerator UnpauseDelay()
        {
            yield return new WaitForEndOfFrame();
            _canTarget = true;
        }

        private void OnDisable()
        {
            IsSelectingTarget = false;
            InputManager.OnAButton -= SelectTarget;
            InputManager.OnBButton -= CloseTargetSelection;
            InputManager.OnBButton -= CancelItemUsage;
        }

        private void SelectTarget()
        {
            if (GameStateManager.TimeGameState == GameState.PLAYING && _canTarget)
            {
                RaycastHit2D[] hits = Physics2D.RaycastAll(transform.position, Vector2.zero);

                if (hits.Length > 0)
                {
                    if (_TargetingMode == TargetingMode.Players)
                    {
                        List<Character> targets = new List<Character>();
                        _potentialTargets = new List<Character>();
                        for (int i = 0; i < hits.Length; i++)
                        {
                            if (hits[i].transform.tag == InlineStrings.PLAYERTAG)
                                targets.Add(hits[i].transform.GetComponent<Character>());
                        }

                        if (!CanSelectAnyone)
                        {
                            CanSelectAnyone = true;
                            for (int i = 0; i < targets.Count; i++)
                            {
                                if (!(_turnManager.IsInBattle((PlayerCharacter)targets[i])))
                                {
                                    _potentialTargets.Add(targets[i]);
                                }
                            }
                        }
                        else
                        {
                            _potentialTargets = targets;
                            
                        }

                        if (_potentialTargets.Count == 1)
                        {
                            Target = _potentialTargets[0];
                            OpenConfirmMenu();
                        }
                        else
                        {
                            for (int i = 0; i < _potentialTargets.Count; i++)
                            {
                                _confirmTargetButtons[i].gameObject.SetActive(true);
                                int temp = i;
                                _confirmTargetButtons[i].onClick.AddListener(() => SetTarget(temp));
                                _confirmTargetButtons[i].GetComponentInChildren<Text>().text = _potentialTargets[i].CharacterName;
                                _buttonParent.SetActive(true);
                            }
                            _isTargetSelectionOpen = true;
                        }
                    }
                    else if (_TargetingMode == TargetingMode.Towns)
                    {
                        for (int i = 0; i < hits.Length; i++)
                        {
                            if (hits[i].transform.tag == InlineStrings.TOWNTILETAG)
                            {
                                TownTarget = hits[i].transform.GetComponent<TownTile>();
                                OpenConfirmMenu();
                                break;
                            }
                        }
                    }
                    else if (_TargetingMode == TargetingMode.AnyTile)
                    {
                        for (int i = 0; i < hits.Length; i++)
                        {
                            if (hits[i].transform.tag != InlineStrings.PLAYERTAG && hits[i].transform.GetComponent<BoardTile>().IsHightlighted)
                            {
                                BoardTarget = hits[i].transform.GetComponent<BoardTile>();
                                if(OnBoardTargetSelected != null)
                                    OnBoardTargetSelected();
                                break;
                            }
                        }
                    }
                }
            }
        }

        private void OpenConfirmMenu()
        {
            _confirmingTarget = true;
            if (_TargetingMode == TargetingMode.Players)
            {
                _confirmMenuText.text = "Do you want to use " + _UsableItem.ItemName + " on " + Target.CharacterName + "?";
            }
            else if (_TargetingMode == TargetingMode.Towns)
            {
                _confirmMenuText.text = "Do you want to teleport to this town?";
            }
            _confirmMenu.SetActive(true);
        }

        public void ConfirmItemUsage()
        {
            _confirmingTarget = true;
            _confirmMenu.SetActive(false);
            _UsableItem.ExecuteUsableEffect();
        }

        public void DenyItemUsage()
        {
            _confirmMenu.SetActive(false);
            ClearTargets();
            _confirmingTarget = false;
        }

        private void CancelItemUsage()
        {
            if(_UsableItem != null && !_buttonParent.gameObject.activeSelf && !_confirmingTarget && !_isTargetSelectionOpen)
            {
                _UsableItem.transform.SetParent(_UsableItem.FormerParent, false);
                _UsableItem.transform.localPosition = Vector2.zero;
                _UsableItem.transform.localScale = Vector2.one;
                if (OnItemUsageCanceled != null)
                    OnItemUsageCanceled();
            }
        }

        private void CloseTargetSelection()
        {
            if (_buttonParent.gameObject.activeSelf)
            {
                for (int i = 0; i < _confirmTargetButtons.Length; i++)
                {
                    _confirmTargetButtons[i].gameObject.SetActive(false);
                }
                _buttonParent.SetActive(false);
                _isTargetSelectionOpen = false;
            }
        }

        public void SetTarget(int characterIndex)
        {
            Target = _potentialTargets[characterIndex];
            CloseTargetSelection();
            OpenConfirmMenu();
        }

        public static void ClearTargets()
        {
            Target = null;
            TownTarget = null;
            BoardTarget = null;
        }
    }
}