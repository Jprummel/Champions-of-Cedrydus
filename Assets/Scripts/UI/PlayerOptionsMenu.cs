using Dialogue;
using Inventory;
using UI;
using UnityEngine;
using UnityEngine.EventSystems;
using Utility;

public class PlayerOptionsMenu : MonoBehaviour
{
    public delegate void AccessInventoryAction();
    public static event AccessInventoryAction OnAccessInventory;

    public delegate void SlowHackedAction();
    public static event SlowHackedAction OnSlowHacked;

    public delegate void ViewMapAction();
    public static event ViewMapAction OnViewMap;

    public delegate void EnableOrDisableMenuAction(bool active);
    public static EnableOrDisableMenuAction OnEnableOrDisableMenu;

    [SerializeField] private GameObject _optionsMenu;
    [SerializeField] private GameObject _spinnerGameObject;
    [SerializeField] private GameObject _saveOrQuitMenu;
    [SerializeField] private DisplayDialogue _displayDialogue;

    private TurnManager _turnManager;

    private GameObject _lastSelectedButton = null;

    [SerializeField] private EventSystem _optionsEventSystem;

    private void Awake()
    {
        _turnManager = GameObject.Find(InlineStrings.TURNMANAGERTAG).GetComponent<TurnManager>();
    }

    private void OnEnable()
    {
        GameStateManager.TimeGameState = GameState.PAUSED;
        UsableItem.OnShowSpinner += StartSpinner;
        OnEnableOrDisableMenu += EnableOrDisableMenu;
        InputManager.OnBButton += CloseSaveOrQuitMenu;
    }

    private void OnDisable()
    {
        UsableItem.OnShowSpinner -= StartSpinner;
        OnEnableOrDisableMenu -= EnableOrDisableMenu;
        InputManager.OnBButton -= CloseSaveOrQuitMenu;
    }

    public void StartSpinner()
    {
        _lastSelectedButton = _optionsEventSystem.currentSelectedGameObject;
        EnableOrDisableMenu(false);

        if (_turnManager.ActivePlayerCharacter.TurnsMovementImpaired <= 0)
        {
            RotateSpinner.IsNewTurn = true;
            SpinnerStateManager.CurrentSpinnerState = SpinnerState.MOVEMENTSPINNER;
            _spinnerGameObject.SetActive(true);
        }
        else
        {
            _turnManager.ActivePlayer.PlayerHasMoves = true;
            _turnManager.ActivePlayer.NumberOfMoves = 1; //If player is slowed , give only 1 tile to move
            if (OnSlowHacked != null)
                OnSlowHacked();
        }
    }

    public void AccessInventory()
    {
        if(_turnManager.ActivePlayerCharacter.TurnsInventoryLocked <= 0)
        {
            _lastSelectedButton = _optionsEventSystem.currentSelectedGameObject;
            if (OnAccessInventory != null)
                OnAccessInventory();

            EnableOrDisableMenu(false);
        }
    }

    private void EnableOrDisableMenu(bool active)
    {
        if(PlayerPrefs.GetInt(InlineStrings.ISNEWGAME, 0) == 1)
        {
            _displayDialogue.StartDialogueFromBeginning();
            PlayerPrefs.SetInt(InlineStrings.ISNEWGAME, 0);
        }
        else
        {
            _optionsMenu.SetActive(active);
            if (active && _lastSelectedButton != null)
                _optionsEventSystem.firstSelectedGameObject = _lastSelectedButton;
        }
    }

    public void ViewMap()
    {
        _lastSelectedButton = _optionsEventSystem.currentSelectedGameObject;
        PlayerTarget._TargetingMode = TargetingMode.Nothing;
        EnableOrDisableMenu(false);
        if (OnViewMap != null)
            OnViewMap();
    }

    public void OpenMenu()
    {
        _lastSelectedButton = _optionsEventSystem.currentSelectedGameObject;
        EnableOrDisableMenu(false);
        _saveOrQuitMenu.SetActive(true);
    }

    private void CloseSaveOrQuitMenu()
    {
        if(_saveOrQuitMenu.activeSelf)
        {
            _saveOrQuitMenu.GetComponent<SaveOrQuitMenu>().CloseMenu();
            EnableOrDisableMenu(true);
        }
    }
}