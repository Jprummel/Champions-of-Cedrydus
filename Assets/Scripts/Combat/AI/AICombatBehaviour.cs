using UnityEngine;

public class AICombatBehaviour : MonoBehaviour {

    public delegate void BattleActionEvent();
    public static BattleActionEvent OnChooseOffensive;
    public static BattleActionEvent OnChooseDefensive;
    private Character _character;
    private BattleActions _battleActions;
    [SerializeField] private int _maxDefensiveOptions;
    [SerializeField] private int _maxOffensiveOptions;

    void Awake () {
        _character = GetComponent<Character>();
        _battleActions = GameObject.FindGameObjectWithTag(InlineStrings.BATTLEACTIONSTAG).GetComponent<BattleActions>();
        OnChooseDefensive += ChooseDefensiveOption;
        OnChooseOffensive += ChooseOffensiveOption;
    }

    void ChooseAction()
    {
        if (BattleStateMachine.AttackingCharacter == _character)
        {
            ChooseOffensiveOption();
        }
        else if (BattleStateMachine.DefendingCharacter == _character)
        {
            ChooseDefensiveOption();
        }
    }

    void ChooseDefensiveOption()
    {
        int randomActionNumber = Random.Range(0, _maxDefensiveOptions);
        _battleActions.DefensiveAction(randomActionNumber);
        BattleStateMachine.EndDefendersTurn();
    }

    void ChooseOffensiveOption()
    {
        int randomActionNumber = Random.Range(0, _maxOffensiveOptions);
        _battleActions.OffensiveAction(randomActionNumber);
        BattleStateMachine.EndAttackersTurn();
    }

    private void OnDestroy()
    {
        OnChooseDefensive -= ChooseDefensiveOption;
        OnChooseOffensive -= ChooseOffensiveOption;
    }
}