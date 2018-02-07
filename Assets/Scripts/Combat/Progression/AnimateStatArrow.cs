using UnityEngine;
using DG.Tweening;

public class AnimateStatArrow : MonoBehaviour {

    private Transform _arrow;

    void Start () {
        _arrow = this.transform;
        Arrow();
	}

    void Arrow()
    {
        Sequence arrowHopSequence = DOTween.Sequence();
        arrowHopSequence.Append(_arrow.DOLocalMoveY(-35, 0.75f));
        arrowHopSequence.Append(_arrow.DOLocalMoveY(-39, 0.75f));
        arrowHopSequence.OnComplete(Arrow);
    }
}