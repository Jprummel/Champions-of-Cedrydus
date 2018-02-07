using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class CombatStatComparisonUI : MonoBehaviour
{
    public delegate void ShowStatEvent(Character character);
    public static ShowStatEvent OnShowInitiatorStats;
    public static ShowStatEvent OnShowInitiatedStats;

    [SerializeField]private GameObject _statDisplay;

    [Header("Initiator's Stats")]
    //Initiators stats
    [SerializeField] private Text _initiatorsAttack;
    [SerializeField] private Text _initiatorsDefense;
    [SerializeField] private Text _initiatorsTech;
    [SerializeField] private Text _initiatorsSpeed;

    [Header("Initiated Stats")]
    //Initiated stats
    [SerializeField] private Text _initiatedAttack;
    [SerializeField] private Text _initiatedDefense;
    [SerializeField] private Text _initiatedTech;
    [SerializeField] private Text _initiatedSpeed;

    private void OnEnable()
    {
        OnShowInitiatorStats += ShowInitiatorStats;
        OnShowInitiatedStats += ShowInitiatedStats;
        BattleStateMachine.OnBattleWon += HideUI;
        BattleStateMachine.OnBattleComplete += HideUI;
        BattleStateMachine.OnBattleActions += SetUI;
    }

    void ShowInitiatorStats(Character character)
    {
        character.CalculateTotalStats();
        _initiatorsAttack.text = character.TotalAttack.ToString();
        _initiatorsDefense.text = character.TotalDefense.ToString();
        _initiatorsTech.text = character.TotalTech.ToString();
        _initiatorsSpeed.text = character.TotalSpeed.ToString();
    }

    void ShowInitiatedStats(Character character)
    {
        character.CalculateTotalStats();
        _initiatedAttack.text = character.TotalAttack.ToString();
        _initiatedDefense.text = character.TotalDefense.ToString();
        _initiatedTech.text = character.TotalTech.ToString();
        _initiatedSpeed.text = character.TotalSpeed.ToString();
    }

    void OnDisable()
    {
        OnShowInitiatorStats -= ShowInitiatorStats;
        OnShowInitiatedStats -= ShowInitiatedStats;
        BattleStateMachine.OnBattleWon -= HideUI;
        BattleStateMachine.OnBattleComplete -= HideUI;
        BattleStateMachine.OnBattleActions -= SetUI;
    }

    void HideUI()
    {
        gameObject.SetActive(false);
    }

    void SetUI(bool active)
    {
        if (active)
            _statDisplay.transform.DOScale(Vector3.one, 0.25f);
        else
            _statDisplay.transform.DOScale(Vector3.zero, 0.25f);
    }
}