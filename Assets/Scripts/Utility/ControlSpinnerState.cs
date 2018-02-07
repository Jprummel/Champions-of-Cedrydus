using UnityEngine;
using System.Collections;
using Utility;
using Inventory;

public class ControlSpinnerState : MonoBehaviour
{
    [SerializeField] private GameObject _spinnerGameObject;
    private RotateSpinner _spinner;

    private void OnEnable()
    {
        _spinner = GetComponentInChildren<RotateSpinner>(true);
        InputManager.OnBButton += ReturnToMenu;
    }

    private void OnDisable()
    {
        InputManager.OnBButton -= ReturnToMenu;
    }

    private void ShowOrHide()
    {
        if (_spinnerGameObject.activeSelf)
            _spinnerGameObject.SetActive(false);
        else
            _spinnerGameObject.SetActive(true);
    }

    private void ReturnToMenu()
    {
        if (SpinnerStateManager.CurrentSpinnerState == SpinnerState.MOVEMENTSPINNER)
        {
            if (_spinnerGameObject.activeSelf && !_spinner.IsSpinning)
            {
                ShowOrHide();
                if (PlayerOptionsMenu.OnEnableOrDisableMenu != null)
                    PlayerOptionsMenu.OnEnableOrDisableMenu(true);
            }
        }
    }
}