using Serialization;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterCreation : MonoBehaviour {

    //Delegates
    public delegate void CharacterCreationEvent();
    public delegate void NamingEvent(string name);
    public static CharacterCreationEvent OnCharacterCreationEvent;
    public static CharacterCreationEvent OnFinishCreation;
    public static CharacterCreationEvent OnCancelCreation;
    public static CharacterCreationEvent OnNamingEvent;
    public static NamingEvent OnNameEvent;

    //UI
    [SerializeField] private GameObject _choosePlayerAmountPanel;
    [SerializeField] private GameObject _characterCreationPanel;
    [SerializeField] private GameObject _eventSystem;
    //Buttons to invoke
    [SerializeField] private Button _backButton;
    [SerializeField] private Button _namingButton;
    [SerializeField] private Button _nextButton;
    public static bool CanUseControllerButtons = true;
    //Character gameobjects and scripts
    private List<GameObject> _createdCharacterObjects = new List<GameObject>(); //Player created characters
    private List<PlayerCharacter> _createdCharacters = new List<PlayerCharacter>();
    [SerializeField]private List<string> _createdCharacterNames = new List<string>();
    private GameObject _characterObject;
    private PlayerCharacter _character;
    [SerializeField] private GameObject _defaultCharacterObject;
    private PlayerCharacter _defaultCharacter;
    //Character that is currently being worked on
    private int _characterID;
    private int _playerNumber;

    public List<string> CreatedCharacterNames
    {
        get { return _createdCharacterNames; }
        set { _createdCharacterNames = value; }
    }

    private void OnEnable()
    {
        GameStateManager.CurrentGameState = GameState.CHARACTERCREATION;
        OnCharacterCreationEvent += StartCharacterCreation;
        OnFinishCreation += InvokeNext;
        OnCancelCreation += InvokeCancel;
        OnNamingEvent += InvokeNaming;
        OnNameEvent += SetName;
    }

    private void Awake()
    {
        ShowCharacterNumber();
        CharacterCreationState.CurrentCreationState = CharacterCreationState.CreationState.CHOOSEPLAYERAMOUNT;
    }

    void ShowCharacterNumber()
    {
        _playerNumber = _characterID + 1;
    }

    void StartCharacterCreation()
    {
        for (int i = 0; i < ChoosePlayerAmount.PlayersToCreate; i++)
        {
            GameObject defaultCharacter = Instantiate(_defaultCharacterObject);
            _createdCharacterObjects.Add(defaultCharacter);
            _defaultCharacter = defaultCharacter.GetComponent<PlayerCharacter>();
            _createdCharacters.Add(_defaultCharacter);
            _createdCharacterNames.Add(null);
        }
        ChooseName.OnChangeText("Player " + _playerNumber);
        CharacterCreationState.CurrentCreationState = CharacterCreationState.CreationState.CHARACTERCREATION;
        _characterID = 0;
        StatsUI.OnShowStats(_createdCharacters[_characterID]);
    }

    public void ChooseClass(GameObject baseCharacter)
    {
        if(_createdCharacterObjects[_characterID] != null)
        {
            Destroy(_createdCharacterObjects[_characterID]); //If there was already a character with the current index, delete it so it can be replaced
        }
        GameObject newCharacter = Instantiate(baseCharacter); //Creates a new instance of the character that was chosen
        _characterObject = newCharacter;
        _character = _characterObject.GetComponent<PlayerCharacter>();
        _createdCharacterObjects[_characterID] = newCharacter;
        _createdCharacters[_characterID] = newCharacter.GetComponent<PlayerCharacter>();
        _createdCharacters[_characterID].Gold += 100;
        _createdCharacters[_characterID].CharacterName = _createdCharacterNames[_characterID];
        StatsUI.OnShowStats(_character);
    }

    public void SetName(string name)
    {
        _createdCharacters[_characterID].CharacterName = name;
        _createdCharacterNames[_characterID] = name;
    }

    public void CreateNextCharacter()
    {
        if (_createdCharacterNames[_characterID] == null || _createdCharacterNames[_characterID] == string.Empty)
        {
            NamingErrors.EventSystem = _eventSystem;
            NamingErrors.OnEmptyName();
        }

        if (_createdCharacterNames[_characterID] != null)
        {
            if (_characterID < ChoosePlayerAmount.PlayersToCreate - 1) // character ID starts at 0 but playerstocreate starts at 1
            {
                _characterID++;
                ShowCharacterNumber();
                ChooseName.OnChangeText("Player " + _playerNumber);
                ShowCharacter.OnCreateNextCharacter();
            }
            else if(GameStateManager.CurrentGameState != GameState.PAUSED)
            {
                SaveCharacters.Instance.ClearSave();
                SaveCharacters.Instance.SavePlayerCharacters();

                if (SceneLoader.OnLoadScene != null)
                {
                    SceneLoader.OnLoadScene(InlineStrings.MAPSCENE);
                }
            }
        }        
    }

    public void Back()
    {
        if(_characterID > 0)
        {
            _createdCharacterNames[_characterID] = "";
            _characterID--;
            ShowCharacterNumber();
            ChooseName.OnChangeText(_createdCharacters[_characterID].CharacterName);
        }
        else if(_characterID <= 0)
        {
            CancelCharacterCreation();
        }
    }

    public void CancelCharacterCreation()
    {
        for (int i = 0; i < _createdCharacterObjects.Count; i++)
        {
            Destroy(_createdCharacterObjects[i]);
            Destroy(_createdCharacters[i]);
        }
        _createdCharacterObjects.Clear();
        _createdCharacterNames.Clear();
        _createdCharacters.Clear();
        ChooseName.OnChangeText("Player " + _playerNumber);
        CharacterCreationState.CurrentCreationState = CharacterCreationState.CreationState.CHOOSEPLAYERAMOUNT;
        _characterCreationPanel.SetActive(false);
        _choosePlayerAmountPanel.SetActive(true);
    }

    void InvokeCancel()
    {
        if (CanUseControllerButtons)
        {
            _backButton.onClick.Invoke();
        }
    }

    void InvokeNaming()
    {
        if (CanUseControllerButtons && CharacterCreationState.CurrentCreationState == CharacterCreationState.CreationState.CHARACTERCREATION)
        {
            _namingButton.onClick.Invoke();
        }
    }

    void InvokeNext()
    {
        if (CanUseControllerButtons && CharacterCreationState.CurrentCreationState == CharacterCreationState.CreationState.CHARACTERCREATION)
        {
            _nextButton.onClick.Invoke();
        }
    }

    private void OnDisable()
    {
        OnCharacterCreationEvent -= StartCharacterCreation;
        OnFinishCreation -= InvokeNext;
        OnCancelCreation -= InvokeCancel;
        OnNamingEvent -= InvokeNaming;
        OnNameEvent -= SetName;
    }
}