using UnityEngine;
using UnityEngine.UI;

public class OverWorldStatUI : MonoBehaviour {

    public delegate void ShowStatsEvent(Character character);
    public static ShowStatsEvent OnShowStats;
    //GeneralStats
    [SerializeField] private Text _nameValue;
    [SerializeField] private Text _goldValue;
    [SerializeField] private Text _levelValue;
    //Combat stats
    [SerializeField] private Text _healthValue;
    [SerializeField] private Text _attackValue;
    [SerializeField] private Text _defenseValue;
    [SerializeField] private Text _techValue;
    [SerializeField] private Text _speedValue;
    //Healthbar
    [SerializeField] private Image _healthBar;

    [SerializeField] private Color _debuffedColor;
    [SerializeField] private Color _buffedColor;

	void Awake () {
        OnShowStats += ShowCombatStats;
        OnShowStats += ShowGeneralStats;
	}

    void ShowGeneralStats(Character character)
    {
        _nameValue.text = character.CharacterName;
        _goldValue.text = character.Gold.ToString();
        _levelValue.text = "Lv. " + character.Level;
    }

    void ShowCombatStats(Character character)
    {
        character.CalculateTotalStats();
        if (character.TurnsDebuffed > 0)
        {
            SetDebuffedTextColor();
        }

        if (character.TurnsBuffed > 0)
        {
            SetBuffedTextColor();
        }

        _healthBar.fillAmount = character.CurrentHP / character.TotalMaxHP;
        _healthValue.text = "HP : " + character.CurrentHP + "/" + character.TotalMaxHP;
        _attackValue.text = "AT : " + character.TotalAttack;
        _defenseValue.text = "DF : " + character.TotalDefense;
        _techValue.text = "TC : " + character.TotalTech;
        _speedValue.text = "SP : " + character.TotalSpeed;
    }

    void SetDebuffedTextColor()
    {
        _attackValue.color = _debuffedColor;
        _defenseValue.color = _debuffedColor;
        _techValue.color = _debuffedColor;
        _speedValue.color = _debuffedColor;
    }

    void SetBuffedTextColor()
    {
        _attackValue.color = _buffedColor;
        _defenseValue.color = _buffedColor;
        _techValue.color = _buffedColor;
        _speedValue.color = _buffedColor;
    }

    private void OnDestroy()
    {
        OnShowStats -= ShowCombatStats;
        OnShowStats -= ShowGeneralStats;
    }
}