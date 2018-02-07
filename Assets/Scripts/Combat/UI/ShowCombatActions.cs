using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class ShowCombatActions : MonoBehaviour {

    public delegate void ShowActions(bool InitiatorGoesFirst = true, bool secondHalf = false);
    public static ShowActions OnShowCombatUI;

    public delegate void ActionsConfirm(string side);
    public static ActionsConfirm OnActionConfirm;

    private bool _lastValue;

    [SerializeField] private List<GameObject> _actions = new List<GameObject>();
    [SerializeField] private List<RectTransform> _actionPositions = new List<RectTransform>();

    //private Sequence _sqn;

    private void Awake()
    {
        OnShowCombatUI += ShowUI;
        OnActionConfirm += GreyOutActions;
        //_sqn = DOTween.Sequence();
    }

    void ShowUI(bool FirstAttackingCharacter = true, bool secondHalf = false)
    {
        if (!BattleStateMachine.CombatEnd)
        {
            if (secondHalf)
            {
                KeyButton.AllowInput = true;
                BattleStateMachine.CombatEnd = true;
                SetPositions(!_lastValue);
            }
            else
            {
                SetPositions(FirstAttackingCharacter);
                _lastValue = FirstAttackingCharacter;
            }         

            for (int i = 0; i < _actions.Count; i++)
            {                
                _actions[i].SetActive(true);
                _actions[i].transform.DOScale(Vector3.one, 0.15f).SetEase(Ease.OutBack);
            }
        }
        else
        {
            BattleStateMachine.OnBattleComplete();
        }
    }

    void SetPositions(bool InitiatorGoesFirst)
    {
        GreyOutActions("Defender");
        if (InitiatorGoesFirst)
        {
            _actions[0].GetComponent<RectTransform>().anchoredPosition = _actionPositions[0].anchoredPosition;
            _actions[1].GetComponent<RectTransform>().anchoredPosition = _actionPositions[1].anchoredPosition;
            BattleStateMachine.SetAttacker(BattleStateMachine.BattleCharacters[0]);
            BattleStateMachine.SetDefender(BattleStateMachine.BattleCharacters[1]);
        }
        else
        {
            _actions[0].GetComponent<RectTransform>().anchoredPosition = _actionPositions[1].anchoredPosition;
            _actions[1].GetComponent<RectTransform>().anchoredPosition = _actionPositions[0].anchoredPosition;
            BattleStateMachine.SetAttacker(BattleStateMachine.BattleCharacters[1]);
            BattleStateMachine.SetDefender(BattleStateMachine.BattleCharacters[0]);
        }
    }

    void GreyOutActions(string side)
    {
        if(side == "Attacker")
        {
            foreach (Transform action in _actions[0].transform)
            {
                action.GetComponent<Image>().DOFade(0.5f, 0.25f);
            }
            foreach (Transform action in _actions[1].transform)
            {
                action.GetComponent<Image>().DOFade(1f, 0.25f);
            }
        }
        else if(side == "Defender")
        {
            foreach (Transform action in _actions[1].transform)
            {
                action.GetComponent<Image>().DOFade(0.5f, 0.05f);
            }
        }
        else if(side == "Reset")
        {
            for (int i = 0; i < _actions.Count; i++)
            {
                foreach (Transform action in _actions[i].transform)
                {
                    action.GetComponent<Image>().DOFade(1f, 0.05f);
                }
            }
        }
    }

    private void OnDisable()
    {
        OnShowCombatUI -= ShowUI;
        OnActionConfirm -= GreyOutActions;
    }
}