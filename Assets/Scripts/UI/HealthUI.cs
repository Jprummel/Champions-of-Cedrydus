using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class HealthUI : MonoBehaviour {

    public delegate void HealthEvent();
    public static HealthEvent OnHealthEvent;
    
    private Character _character;
    public Character Character
    {
        get { return _character; }
        set { _character = value; }
    }

    [SerializeField]private Image _healthBar;
    [SerializeField] private Image _healthBarGradient;
    [SerializeField] private Text _healthValue;

    void Awake()
    {
        OnHealthEvent += UpdateHealthUI;
    }

    void UpdateHealthUI()
    {
        Color damageColor = new Color(0.95f, 0.67f, 0f);
        Color healColor = new Color(0.35f, 0.7f, 0f);

        if(_character.CurrentHP / _character.MaxHP < _healthBar.fillAmount)
        {
            _healthBarGradient.color = damageColor;
            _healthBar.fillAmount = _character.CurrentHP / _character.MaxHP;
            _healthBarGradient.DOFillAmount(_character.CurrentHP / _character.MaxHP, 0.5f).SetEase(Ease.OutCirc);
        }
        else if(_character.CurrentHP / _character.MaxHP > _healthBar.fillAmount)
        {
            _healthBarGradient.color = healColor;
            _healthBarGradient.fillAmount = _character.CurrentHP / _character.MaxHP;
            _healthBar.DOFillAmount(_character.CurrentHP / _character.MaxHP, 0.3f).SetEase(Ease.InCirc);
        }
        _healthValue.text = "HP : " + _character.CurrentHP + " / " + _character.MaxHP;
    }

    private void OnDisable()
    {
        OnHealthEvent -= UpdateHealthUI;
    }
}