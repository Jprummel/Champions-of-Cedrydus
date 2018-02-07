using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
public class ChoosePlayerAmount : MonoBehaviour {

    public delegate void ChoosePlayerAmountEvent();
    public static ChoosePlayerAmountEvent OnCancelCreation;

    public static int PlayersToCreate;
    [SerializeField] private GameObject _eventSystem;
    [SerializeField] private Button _cancelButton;
    [SerializeField] private GameObject _returnToMenuPanel;
    [SerializeField] private List<Button> _playerAmountButtons = new List<Button>();

    private void OnEnable()
    {
        OnCancelCreation += InvokeCancel;
    }

    public void SetPlayerAmount(int playersToCreate)
    {
        PlayersToCreate = playersToCreate;
        CharacterCreation.OnCharacterCreationEvent();
        CharacterCreationInput.OnSwitchState(2);
    }

    void DisableButtons()
    {
        _eventSystem.SetActive(false);
        for (int i = 0; i < _playerAmountButtons.Count; i++)
        {
            _playerAmountButtons[i].interactable = false;
        }
    }

    public void EnableButtons()
    {
        for (int i = 0; i < _playerAmountButtons.Count; i++)
        {
            _playerAmountButtons[i].interactable = true;
        }
        _eventSystem.SetActive(true);
    }

    void InvokeCancel()
    {
        _cancelButton.onClick.Invoke();
    }

    void CancelCreation()
    {
        DisableButtons();
        _returnToMenuPanel.SetActive(true);
    }

    private void OnDisable()
    {
        OnCancelCreation -= InvokeCancel;
    }
}