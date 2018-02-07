using UnityEngine;
using System.Collections;
using DG.Tweening;

public class TweenStick : MonoBehaviour
{
    private RectTransform _rt;

    private Sequence _stickSequence;

    private void OnEnable()
    {
        _rt = GetComponent<RectTransform>();

        _stickSequence = DOTween.Sequence();
        _stickSequence.Append(_rt.DOLocalMoveY(-200f, 1f));
        _stickSequence.AppendCallback(() => ResetPosition());
        _stickSequence.SetLoops(-1);
    }

    private void ResetPosition()
    {
        _rt.DOLocalMoveY(-250f, 0f);
    }

    private void OnDisable()
    {
        _stickSequence.Kill();
        ResetPosition();
    }
}