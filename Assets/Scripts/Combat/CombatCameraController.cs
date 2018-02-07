using UnityEngine;
using DG.Tweening;

public class CombatCameraController : MonoBehaviour {

    public delegate void CombatCameraEvent();
    public static CombatCameraEvent OnCombatCameraEvent;

    [SerializeField] private Camera _camera;
    //private Vector3 _cameraDefaultPos;
    private Transform _attackingCharacterPosition;
    //private Transform _defendingCharacterPosition;

    private void OnEnable()
    {
        OnCombatCameraEvent += InitiateCombatActionsCamera;
        BattleStateMachine.OnBattleWon += MoveToWinner;
    }

    void Start () {
        //_cameraDefaultPos = _camera.transform.position;
	}

    void InitiateCombatActionsCamera()
    {
        _attackingCharacterPosition = BattleStateMachine.AttackingCharacter.transform;
        //_defendingCharacterPosition = BattleStateMachine.DefendingCharacter.transform;
        Sequence cameraSequence = DOTween.Sequence();
        cameraSequence.Append(_camera.transform.DOLocalMove(_attackingCharacterPosition.position,1));

    }

    private void OnDisable()
    {
        OnCombatCameraEvent -= InitiateCombatActionsCamera;
        BattleStateMachine.OnBattleWon -= MoveToWinner;
    }

    void MoveToWinner()
    {
        Transform winner = BattleStateMachine.CharacterThatWon.transform;

        _camera.transform.DOMove(new Vector3(Mathf.Clamp(winner.position.x, -4.25f, 4.25f), Mathf.Clamp(winner.position.y, -2f, 2.4f), 0), 1f);
        _camera.DOOrthoSize(3, 1);
    }
}