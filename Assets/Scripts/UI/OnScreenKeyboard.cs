using UnityEngine;
using UnityEngine.UI;

public class OnScreenKeyboard : MonoBehaviour {

    public delegate void OnScreenKeyboardEvent();
    public static OnScreenKeyboardEvent OnCancel;
    public static OnScreenKeyboardEvent OnDelete;
    public static OnScreenKeyboardEvent OnFinish;
    [SerializeField] private Text _stringExample;
    [SerializeField] private int _maxCharacterCount;
    private string _stringInput;
    private string _lastString;
    private CharacterCreation _characterCreation;
    [SerializeField] private GameObject _invalidPopUp;
    [SerializeField] private GameObject _eventSystem;


    private void OnEnable()
    {
        _stringInput = "";
        _stringExample.text = _stringInput;
        OnCancel += CancelNaming;
        OnDelete += ReturnToLastString;
        OnFinish += FinishNaming;
    }

    private void Awake()
    {
        // get character creation script
        _characterCreation = GameObject.FindGameObjectWithTag(InlineStrings.CHARACTERCREATIONMANAGER).GetComponent<CharacterCreation>();
    }

    public void AddToString(string tingToAdd)
    {
        if (_stringInput.Length <= _maxCharacterCount)
        {
            _stringInput += tingToAdd;
            _stringExample.text = _stringInput;
        }
    }

    public void ReturnToLastString()
    {
        if (_stringInput.Length > 0)
        {
            _stringInput = _stringInput.Substring(0, _stringInput.Length - 1);
            _stringExample.text = _stringInput;
        }
    }

    public void FinishNaming()
    {
        if (_stringInput == null || _stringInput == string.Empty)
        {
            _eventSystem.SetActive(false);
            NamingErrors.EventSystem = _eventSystem;
            NamingErrors.OnEmptyName();
        }else if (_characterCreation.CreatedCharacterNames.Contains(_stringInput)) //Checks if the name is already in the list
        {
            //If it is, give an error
            _eventSystem.SetActive(false);
            NamingErrors.EventSystem = _eventSystem;
            NamingErrors.OnNameAlreadyExists();
        }
        if (!_characterCreation.CreatedCharacterNames.Contains(_stringInput) && _stringInput != string.Empty) //Checks if the player had any input for the name
        {
            //If they didnt, give an error
            CharacterCreation.OnNameEvent(_stringInput);
            ChooseName.OnChangeText(_stringInput);
            ChooseName.OnPanelToggle();
            _stringInput = null;
        }
        
    }

    public void CancelNaming()
    {
        _stringInput = null;
        _stringExample.text = "Name";
        ChooseName.OnPanelToggle();
    }

    private void OnDisable()
    {
        OnCancel -= CancelNaming;
        OnDelete -= ReturnToLastString;
        OnFinish -= FinishNaming;
    }
}