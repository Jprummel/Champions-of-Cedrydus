/*
	DoTweenBounce.cs
	Created 10/1/2017 8:14:43 PM
	Project Resource Collector by Base Games
*/
using DG.Tweening;
using UnityEngine;

namespace UI
{
	public class DoTweenBounce : MonoBehaviour 
	{
        [SerializeField] private float _timeToEnlarge = 0.3f, _timeToBounceBack = 0.15f;

        [Tooltip("-1 = infinite loops 0 = doesn't play")]
        [SerializeField] private int _numberOfLoops = 1;

        [SerializeField] private bool _playOnEnable;

        [SerializeField] private Vector3 _beginScale = new Vector3(0.1f, 0.1f, 0.1f), _bouncedScale = new Vector3(1.1f, 1.1f, 1.1f);

        private Sequence _bounceSequence;

        private void OnEnable()
        {
            if (_playOnEnable)
                Bounce();
        }

        private void OnDisable()
        {
            _bounceSequence.Kill();
        }

        public void Bounce()
        {
            _bounceSequence = DOTween.Sequence();
            transform.localScale = _beginScale;
            _bounceSequence.AppendInterval(.1f);
            _bounceSequence.Append(transform.DOScale(_bouncedScale, _timeToEnlarge));
            _bounceSequence.Append(transform.DOScale(Vector3.one, _timeToBounceBack));
            _bounceSequence.SetLoops(_numberOfLoops);
        }
    }
}