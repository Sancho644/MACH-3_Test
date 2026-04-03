using DG.Tweening;
using UnityEngine;

namespace UI.Animations
{
    public class MoveTween : MonoBehaviour
    {
        [SerializeField] private float duration = 0.2f;
        [SerializeField] private Ease ease = Ease.OutQuad;

        private Sequence _sequence;

        public Sequence Play(Vector3 targetPosition)
        {
            _sequence?.Kill(true);

            _sequence = DOTween.Sequence();
            _sequence.SetLink(gameObject, LinkBehaviour.KillOnDestroy);
            _sequence.Append(transform.DOMove(targetPosition, duration).SetEase(ease));
            _sequence.Play();

            return _sequence;
        }

        public Tween GetTween(Vector3 targetPosition)
        {
            return transform.DOMove(targetPosition, duration)
                .SetEase(ease)
                .SetLink(gameObject, LinkBehaviour.KillOnDestroy);
        }

        public Tween GetTween(Vector3 targetPosition, float tweenDuration, Ease tweenEase)
        {
            return transform.DOMove(targetPosition, tweenDuration)
                .SetEase(tweenEase)
                .SetLink(gameObject, LinkBehaviour.KillOnDestroy);
        }

        private void OnDestroy()
        {
            _sequence?.Kill();
        }
    }
}