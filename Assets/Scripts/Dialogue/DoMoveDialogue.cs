using DG.Tweening;
using UnityEngine;

namespace Dialogue
{
	public class DoMoveDialogue : MonoBehaviour 
	{
        [SerializeField] private Vector2 _startPos;
        private Vector2 _endPos;

        private RectTransform _rt;

        private void OnEnable()
        {
            _rt = GetComponent<RectTransform>();
            _endPos = _rt.localPosition;
            MoveDialogueItem();
        }

        private void MoveDialogueItem()
        {
            _rt.localPosition = _startPos;
            _rt.DOLocalMove(_endPos, 1.5f).SetEase(Ease.OutBack, overshoot: 1.2f);
        }
    }
}