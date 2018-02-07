using UnityEngine;
using UnityEngine.UI;

public class StatsUI : MonoBehaviour {

    public delegate void ShowStatsEvent(Character character);
    public delegate void ShowStatChangeEvent(Character character, int statIndex, float statIncrease);
    public delegate void TextColorChangeEvent(int statToCheck, int textToChange);
    public static TextColorChangeEvent OnStatChange;
    public static ShowStatsEvent OnShowInitialIncreases;
    public static ShowStatsEvent OnShowStats;
    public static ShowStatsEvent OnShowGeneralStats;
    public static ShowStatChangeEvent OnStatIncrease;
    public static ShowStatsEvent OnStatReset;

    [Header("Character general stats")]
    [SerializeField] private Text _characterName;
    [SerializeField] private Text _characterLevel;

    [Header("Character Stat Values")]
    [SerializeField] private Text _attackValue;
    [SerializeField] private Text _defenseValue;
    [SerializeField] private Text _techValue;
    [SerializeField] private Text _speedValue;
    [SerializeField] private Text _maxHPValue;

    [Header("Stat Points Up Values")]
    [SerializeField] private Text _attackUpPoints;
    [SerializeField] private Text _defenseUpPoints;
    [SerializeField] private Text _techUpPoints;
    [SerializeField] private Text _speedUpPoints;
    [SerializeField] private Text _maxHPUpPoints;

    [Header("Stat Up Arrows")]
    [SerializeField] private GameObject _attackUpArrow;
    [SerializeField] private GameObject _defenseUpArrow;
    [SerializeField] private GameObject _techUpArrow;
    [SerializeField] private GameObject _speedUpArrow;
    [SerializeField] private GameObject _maxHPUpArrow;

    private Color _increasedStatColor = Color.green;
    private Color _defaultStatColor = Color.white;

    private void OnEnable()
    {
        OnShowStats += ShowStats;
        OnShowGeneralStats += ShowGeneralStats;
        OnStatIncrease += ShowStatIncrease;
        OnStatReset += ResetStatIncrease;
        OnStatChange += ChangeColorText;
        OnShowInitialIncreases += CheckGainedStats;
    }

    void ShowStats(Character character)
    {
        character.CalculateTotalStats();
        _attackValue.text = character.TotalAttack.ToString();
        _defenseValue.text = character.TotalDefense.ToString();
        _techValue.text = character.TotalTech.ToString();
        _speedValue.text = character.TotalSpeed.ToString();
        _maxHPValue.text = character.TotalMaxHP.ToString();
    }

    void ShowGeneralStats(Character character)
    {
        _characterName.text = character.CharacterName;
        _characterLevel.text = "Lv. " + character.Level.ToString();
    }

    void ShowStatIncrease(Character character, int statIndex,float statIncrease)
    {
        if (statIncrease > 0)
        {
            switch (statIndex)
            {
                case 0:
                    _attackUpArrow.SetActive(true);
                    _attackUpPoints.text = statIncrease.ToString();
                    break;
                case 1:
                    _defenseUpArrow.SetActive(true);
                    _defenseUpPoints.text = statIncrease.ToString();
                    break;
                case 2:
                    _techUpArrow.SetActive(true);
                    _techUpPoints.text = statIncrease.ToString();
                    break;
                case 3:
                    _speedUpArrow.SetActive(true);
                    _speedUpPoints.text = statIncrease.ToString();
                    break;
                case 4:
                    _maxHPUpArrow.SetActive(true);
                    _maxHPUpPoints.text = statIncrease.ToString();
                    break;
            }
        }
    }

    void ResetStatIncrease(Character character)
    {
        _attackUpArrow.SetActive(false);
        _defenseUpArrow.SetActive(false);
        _techUpArrow.SetActive(false);
        _speedUpArrow.SetActive(false);
        _maxHPUpArrow.SetActive(false);

        _attackUpPoints.text = "";
        _defenseUpPoints.text = "";
        _techUpPoints.text = "";
        _speedUpPoints.text = "";
        _maxHPUpPoints.text = "";

        CheckGainedStats(character);

        OnStatIncrease(character, 0, character.AttackToGain);
        OnStatIncrease(character, 1, character.DefenseToGain);
        OnStatIncrease(character, 2, character.TechToGain);
        OnStatIncrease(character, 3, character.SpeedToGain);
        OnStatIncrease(character, 4, character.MaxHPToGain);
    }

    void CheckStatIncreases(float statToCheck, Text textToChange)
    {
        if (statToCheck > 0)
        {
            textToChange.color = _increasedStatColor;
        }
        else
        {
            textToChange.color = _defaultStatColor;
        }
    }

    void ChangeColorText(int statChange,int textIndex)
    {//This function gets called in StatPointAllocation to check which stat is being increased
        switch (textIndex)
        {
            case 0:
                CheckStatIncreases(statChange, _attackValue);
                break;
            case 1:
                CheckStatIncreases(statChange, _defenseValue);
                break;
            case 2:
                CheckStatIncreases(statChange, _techValue);
                break;
            case 3:
                CheckStatIncreases(statChange, _speedValue);
                break;
            case 4:
                CheckStatIncreases(statChange, _maxHPValue);
                break;
        }
    }

    void CheckGainedStats(Character character)
    {
        CheckStatIncreases(character.AttackToGain, _attackValue);
        CheckStatIncreases(character.DefenseToGain, _defenseValue);
        CheckStatIncreases(character.TechToGain, _techValue);
        CheckStatIncreases(character.SpeedToGain, _speedValue);
        CheckStatIncreases(character.MaxHPToGain, _maxHPValue);
    }

    private void OnDisable()
    {
        OnShowStats -= ShowStats;
        OnShowGeneralStats -= ShowGeneralStats;
        OnStatIncrease -= ShowStatIncrease;
        OnStatReset -= ResetStatIncrease;
        OnStatChange -= ChangeColorText;
        OnShowInitialIncreases -= CheckGainedStats;
    }
}