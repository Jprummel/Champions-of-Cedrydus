using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class StatPointAllocation : MonoBehaviour
{
    public delegate void StatAllocationEvent(Character character);
    public static StatAllocationEvent OnStatAllocation;
    
    [SerializeField]private Character _character;
    [SerializeField] private GameObject _eventSystem;
    [SerializeField] private GameObject _confirmPanel;
    [SerializeField] private Text _availablePointsText;
    private int[] _pointsToAllocateInts = new int[4]; //Points to put in stats chosen by the player
    private int[] _baseStatPointsInts = new int[4]; //Starting stat values before applying allocated points
    private float[] _pointsToAllocateFloats = new float[1];
    private float[] _baseStatPointsFloats = new float[1];
    private int _usedStatPoints;
    [SerializeField] private List<Button> _statButtons = new List<Button>();

    private void Awake()
    {
        OnStatAllocation += SetCharacter;
        //StatsUI.OnShowInitialIncreases(_character);
    }


    void SetCharacter(Character character)
    {
        _character = character;
        this.gameObject.SetActive(true);
        StatsUI.OnShowInitialIncreases(_character);
    }

    void Start()
    {
        RetrieveStatBaseStatPoints(_character); //Gets the new base values after adding the stats on level up that can't be changed
        RetrievePointsToAllocate(_character);
        ShowAvailablePoints();        
        ShowInitialStatIncrease();
        StatsUI.OnShowStats(_character); //Shows the characters stats at the start
        StatsUI.OnShowGeneralStats(_character);
    }

    void ShowInitialStatIncrease()
    {
        StatsUI.OnStatIncrease(_character, 0, _character.AttackToGain);
        StatsUI.OnStatIncrease(_character, 1, _character.DefenseToGain);
        StatsUI.OnStatIncrease(_character, 2, _character.TechToGain);
        StatsUI.OnStatIncrease(_character, 3, _character.SpeedToGain);
        StatsUI.OnStatIncrease(_character, 4, _character.MaxHPToGain);
    }

    void ShowAvailablePoints()
    {
        //Shows the available stat points for the current character
        if (_character.StatPoints > 0)
        {
            _availablePointsText.text = "Available points : " + _character.StatPoints;
            EnableButtons();
        }
        else
        {
            _availablePointsText.text = "";
            _eventSystem.SetActive(false);
            _confirmPanel.SetActive(true);
            DisableButtons();
        }
    }

    public void AddStatPoint(int statIndex)
    {
        if (_character.StatPoints > 0)
        {
            //Set this function to a buttons onclick to add a point in a stat
            switch (statIndex)
            {
                case 0:
                    _character.Attack++;
                    StatsUI.OnStatIncrease(_character, statIndex,_character.AttackToGain + _character.Attack - _baseStatPointsInts[0]);
                    
                    break;
                case 1:
                    _character.Defense++;
                    StatsUI.OnStatIncrease(_character, statIndex,_character.DefenseToGain + _character.Defense - _baseStatPointsInts[1]);
                    break;
                case 2:
                    _character.Tech++;
                    StatsUI.OnStatIncrease(_character, statIndex,_character.TechToGain + _character.Tech - _baseStatPointsInts[2]);
                    break;
                case 3:
                    _character.Speed++;
                    StatsUI.OnStatIncrease(_character, statIndex,_character.SpeedToGain + _character.Speed - _baseStatPointsInts[3]);
                    break;
                case 4:
                    _character.MaxHP += 10;
                    StatsUI.OnStatIncrease(_character, statIndex,_character.MaxHPToGain + _character.MaxHP - _baseStatPointsFloats[0]);
                    _character.CurrentHP = _character.MaxHP;
                    break;
            }
            _character.StatPoints--; //Removes a stat point to use
            StatsUI.OnStatChange(1, statIndex);
            _usedStatPoints++; //Increases the used stat points by 1 (Used if player resets his current decision)
            ShowAvailablePoints();
            RetrievePointsToAllocate(_character);
            StatsUI.OnShowStats(_character);
        }
    }

    public void ConfirmStats()
    {
        _usedStatPoints = 0; //Sets used points to 0 if player confirms
        //onCheckBaseStats(_character); //Sets the new base stats so only the proper buttons show
        gameObject.SetActive(false);
        if (BattleStateMachine.OnExit != null && BattleStateMachine.PVPWon())
        {
            SetPunishmentUI.SetPunishmentActive(true);
        }
        else if(BattleStateMachine.OnExit != null)
        {
            BattleStateMachine.OnExit();
        }
    }

    public void ResetToBaseStats()
    {
        //Resets the stats to what they were before allocating points
        _character.Attack = _baseStatPointsInts[0];
        _character.Defense = _baseStatPointsInts[1];
        _character.Tech = _baseStatPointsInts[2];
        _character.Speed = _baseStatPointsInts[3];
        _character.MaxHP = _baseStatPointsFloats[0];
        _character.StatPoints += _usedStatPoints; //Returns the players used points
        _usedStatPoints = 0; // Resets used points to 0
        ShowAvailablePoints();
        RetrievePointsToAllocate(_character);
        _confirmPanel.SetActive(false);
        _eventSystem.SetActive(true);
        StatsUI.OnShowStats(_character);
        StatsUI.OnStatReset(_character);
    }

    void RetrieveStatBaseStatPoints(Character character)
    {
        //This is used at the start of point allocation to check if there are any changes to any stat
        _baseStatPointsInts[0] = character.Attack;
        _baseStatPointsInts[1] = character.Defense;
        _baseStatPointsInts[2] = character.Tech;
        _baseStatPointsInts[3] = character.Speed;
        _baseStatPointsFloats[0] = character.MaxHP;
        _usedStatPoints = 0; //Makes sure the the player cant return points after switching characters
    }

    void RetrievePointsToAllocate(Character character)
    {
        //This is used to check if there is any change to the stat , compares to the base stats
        _pointsToAllocateInts[0] = character.Attack;
        _pointsToAllocateInts[1] = character.Defense;
        _pointsToAllocateInts[2] = character.Tech;
        _pointsToAllocateInts[3] = character.Speed;
        _pointsToAllocateFloats[0] = character.MaxHP;
    }

    void DisableButtons()
    {
        for (int i = 0; i < _statButtons.Count; i++)
        {
            _statButtons[i].interactable = false;
        }
    }

    public void EnableButtons()
    {
        for (int i = 0; i < _statButtons.Count; i++)
        {
            _statButtons[i].interactable = true;
        }
    }

    private void OnDisable()
    {
        OnStatAllocation -= SetCharacter;
    }
}