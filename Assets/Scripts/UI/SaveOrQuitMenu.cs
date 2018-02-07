using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SaveOrQuitMenu : MonoBehaviour
{
    [SerializeField] private EventSystem _saveOrQuitEventSystem;
    [SerializeField] private GameObject _okButtonGameObject, _confirmMenuGameObject;
    [SerializeField] private Button _saveButton, _quitButton = null;

    public void SaveButtonClick()
    {
        _saveOrQuitEventSystem.SetSelectedGameObject(_okButtonGameObject);
        _saveButton.interactable = false;
        _quitButton.interactable = false;
    }

    public void OkButtonClick()
    {
        _saveOrQuitEventSystem.SetSelectedGameObject(_saveButton.gameObject);
        _saveButton.interactable = true;
        _quitButton.interactable = true;
    }

    public void CloseMenu()
    {
        if (_confirmMenuGameObject.activeSelf)
            _confirmMenuGameObject.SetActive(false);

        gameObject.SetActive(false);
    }
}