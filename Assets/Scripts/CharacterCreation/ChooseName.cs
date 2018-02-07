using UnityEngine;
using UnityEngine.UI;

public class ChooseName : MonoBehaviour {

    public delegate void PanelToggle();
    public delegate void ChangeTextEvent(string textToShow);

    public static PanelToggle OnPanelToggle;
    public static ChangeTextEvent OnChangeText;

    [SerializeField] private GameObject _panelToToggle;
    [SerializeField] private Text _nameButtonText;
    [SerializeField] private GameObject _onScreenKeyBoard;

    void OnEnable () {
        OnPanelToggle += TogglePanel;
        OnChangeText += SetButtonText;
	}

    public void TogglePanel()
    {
        if (_panelToToggle.activeSelf)
        {
            _panelToToggle.SetActive(false);
            _onScreenKeyBoard.SetActive(true);
            CharacterCreationInput.OnSwitchState(3);
        }
        else
        {
            _panelToToggle.SetActive(true);
            _onScreenKeyBoard.SetActive(false);
            CharacterCreationInput.OnSwitchState(2);
        }
    }

    void SetButtonText(string textToShow)
    {
        _nameButtonText.text = textToShow;
    }

    private void OnDisable()
    {
        OnPanelToggle -= TogglePanel;
        OnChangeText -= SetButtonText;
    }
}