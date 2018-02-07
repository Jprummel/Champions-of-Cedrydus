/*
	EnableCrosshair.cs
	Created 12/20/2017 11:57:59 AM
	Project Boardgame by Base Games
*/
using Inventory;
using UnityEngine;
using Utility;

namespace UI
{
    public class EnableCrosshair : MonoBehaviour
    {
        public delegate void UnhighlightAction(bool active);
        public static event UnhighlightAction OnUnighlight;

        [SerializeField] private GameObject _crosshair;
        private TurnManager _turnManager;

        private void OnEnable()
        {
            UsableItem.OnUseItem += EnableTheCrosshair;
            UsableItem.OnExecuteEffect += DisableCrosshair;
            InputManager.OnXButton += EnableCrossHairIfNotPaused;
            ConfirmPathMove.OnPathConfirmed += DisableCrosshair;
            InputManager.OnBButton += DisableCrossHairIfActive;
            PlayerTarget.OnItemUsageCanceled += DisableCrosshair;
            PlayerOptionsMenu.OnViewMap += EnableTheCrosshair;
            DecryptionTool.OnDecryptionTool += EnableCrossHairIfNotPaused;
        }

        private void Start()
        {
            _turnManager = GameObject.FindWithTag(InlineStrings.TURNMANAGERTAG).GetComponent<TurnManager>();
        }

        private void OnDisable()
        {
            UsableItem.OnUseItem -= EnableTheCrosshair;
            UsableItem.OnExecuteEffect -= DisableCrosshair;
            InputManager.OnXButton -= EnableCrossHairIfNotPaused;
            ConfirmPathMove.OnPathConfirmed -= DisableCrosshair;
            InputManager.OnBButton -= DisableCrossHairIfActive;
            PlayerTarget.OnItemUsageCanceled -= DisableCrosshair;
            PlayerOptionsMenu.OnViewMap -= EnableTheCrosshair;
            DecryptionTool.OnDecryptionTool -= EnableCrossHairIfNotPaused;
        }

        private void EnableCrossHairIfNotPaused()
        {
            if (GameStateManager.TimeGameState == GameState.PLAYING && _turnManager.ActivePlayer._previousTiles.Count == 0)
            {
                EnableTheCrosshair();
            }   
        }

        private void DisableCrossHairIfActive()
        {
            if (!ConfirmPathMove.IsConfirming)
            {
                if (GameStateManager.TimeGameState == GameState.PLAYING && _crosshair.activeSelf)
                {
                    if (CameraFollowPlayer.OnChangeTarget != null)
                        CameraFollowPlayer.OnChangeTarget(_turnManager.ActivePlayer.transform);
                    DisableCrosshair();
                }

                if (PlayerTarget._TargetingMode == TargetingMode.Nothing)
                {
                    PlayerTarget._TargetingMode = TargetingMode.Disabled;
                    if (PlayerOptionsMenu.OnEnableOrDisableMenu != null)
                        PlayerOptionsMenu.OnEnableOrDisableMenu(true);
                }

                if (OnUnighlight != null)
                    OnUnighlight(false);
            }
        }

        private void EnableTheCrosshair()
        {
            _crosshair.SetActive(true);
        }

        private void DisableCrosshair()
        {
            _crosshair.SetActive(false);
        }
    }
}