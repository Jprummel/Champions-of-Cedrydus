/*
	ConfirmMoveMenu.cs
	Created 12/4/2017 3:17:10 PM
	Project Boardgame by Base Games
*/
using PlayerCharacters;
using System.Collections;
using UnityEngine;

namespace UI
{
	public class ConfirmMoveMenu : MonoBehaviour 
	{
        public delegate void ConfirmMoveAction();
        public static event ConfirmMoveAction OnConfirmMove;

        [SerializeField] private GameObject _confirmMoveMenu;

        private void OnEnable()
        {
            PlayerMovement.OnMovesGone += ShowMenu;
        }

        private void OnDisable()
        {
            PlayerMovement.OnMovesGone -= ShowMenu;
        }

        private void ShowMenu()
        {
            _confirmMoveMenu.SetActive(true);
        }

        public void ConfirmMove()
        {
            _confirmMoveMenu.SetActive(false);
            if (OnConfirmMove != null)
                OnConfirmMove();
        }

        public void HideMenu()
        {
            PlayerTarget.ClearTargets();
            StartCoroutine(HideMenuDelay());
        }

        private IEnumerator HideMenuDelay()
        {
            yield return new WaitForEndOfFrame();
            _confirmMoveMenu.SetActive(false);
        }
    }
}