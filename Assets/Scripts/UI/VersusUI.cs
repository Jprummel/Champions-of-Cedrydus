using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class VersusUI : MonoBehaviour {

    public delegate void SetVersusStats(Character attacker, Character defender, float enemyHP = 0f);
    public static SetVersusStats OnSetVersusStats;

    [Header("Attacker's Stats")]
    [SerializeField] private Text _aName;
    [SerializeField] private Text _aLevel;
    [SerializeField] private Text _aHealthValues;
    [SerializeField] private Image _aHealthbar;
    [SerializeField] private RectTransform _aParent;

    [Header("Defender's Stats")]
    [SerializeField] private Text _dName;
    [SerializeField] private Text _dLevel;
    [SerializeField] private Text _dHealthValues;
    [SerializeField] private Image _dHealthbar;
    [SerializeField] private RectTransform _dParent;

    [Header("Versus Objects")]
    [SerializeField] private GameObject _versusTexts;
    [SerializeField] private GameObject _VersusContainer;

    private void OnEnable()
    {
        OnSetVersusStats += ShowStats;
    }

    void ShowVersusPopUp()
    {
        Sequence versusSequence = DOTween.Sequence();

        versusSequence.OnComplete(RotateText);

        versusSequence.Append(_aParent.DOAnchorPosX(-250f, 0.5f).SetEase(Ease.InQuart));
        versusSequence.Join(_dParent.DOAnchorPosX(250f, 0.5f).SetEase(Ease.InQuart));
        versusSequence.AppendInterval(0.2f);
        versusSequence.AppendCallback(()=>_versusTexts.SetActive(true));
    }

    void RotateText()
    {
        Sequence rotateVSText = DOTween.Sequence();
        rotateVSText.OnComplete(() => BattleSceneTransition.TransitionIn());

        rotateVSText.Append(_versusTexts.transform.DOScale(Vector3.one, 0.2f));
        rotateVSText.Join(_versusTexts.transform.DORotate(new Vector3(0, 0, -3240), 0.5f, RotateMode.FastBeyond360));
        rotateVSText.AppendInterval(0.5f);
    }

    void ShowStats(Character attacker, Character defender, float enemyHP = 0f)
    {
        _aName.text = attacker.CharacterName;
        _aLevel.text = "Lv: " + attacker.Level;
        _aHealthValues.text = attacker.CurrentHP + "/" + attacker.MaxHP;
        _aHealthbar.fillAmount = attacker.CurrentHP / attacker.MaxHP;

        _dName.text = defender.CharacterName;
        _dLevel.text = "Lv: " + defender.Level;
        if (enemyHP > 0)
        {
            _dHealthValues.text = enemyHP + "/" + defender.MaxHP;
            _dHealthbar.fillAmount = enemyHP / defender.MaxHP;
            Debug.Log(enemyHP / defender.MaxHP);
        }
        else
        {
            _dHealthValues.text = defender.CurrentHP + "/" + defender.MaxHP;
            _dHealthbar.fillAmount = defender.CurrentHP / defender.MaxHP;
        }

        _VersusContainer.SetActive(true);
        ShowVersusPopUp();
    }

    private void OnDisable()
    {
        OnSetVersusStats -= ShowStats;
    }
}
