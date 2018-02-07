using UnityEngine;
using DG.Tweening;

public class BattleStartUITweens : MonoBehaviour {

    [SerializeField] private Transform _endPosition;

	void Start () {
        transform.DOMove(_endPosition.position, 1);
	}
}