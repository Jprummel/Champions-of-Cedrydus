using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class NamingErrors : MonoBehaviour {

    public delegate void NamingErrorEvent();
    public static NamingErrorEvent OnEmptyName;
    public static NamingErrorEvent OnNameAlreadyExists;
    [SerializeField] private GameObject _errorPopUp;
    [SerializeField] private Text _errorMessage;
    public static GameObject EventSystem;

    private void OnEnable()
    {
        OnEmptyName += NameIsEmpty;
        OnNameAlreadyExists += NameAlreadyExists;
    }

    void NameAlreadyExists()
    {
        CharacterCreation.CanUseControllerButtons = false;
        EventSystem.SetActive(false);
        _errorPopUp.SetActive(true);
        _errorMessage.text = "Name already exists";
    }

    void NameIsEmpty()
    {
        CharacterCreation.CanUseControllerButtons = false;
        EventSystem.SetActive(false);
        _errorPopUp.SetActive(true);
        _errorMessage.text = "Please enter a name";
    }

    public void ClosePopUp()
    {
        _errorPopUp.SetActive(false);
        EventSystem.SetActive(true);
        CharacterCreation.CanUseControllerButtons = true;
    }

    private void OnDisable()
    {
        OnEmptyName -= NameIsEmpty;
        OnNameAlreadyExists -= NameAlreadyExists;
    }
}
