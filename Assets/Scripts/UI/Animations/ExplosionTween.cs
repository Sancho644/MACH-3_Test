using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Animations
{
    [RequireComponent(typeof(Image))]
    public class ExplosionTween : MonoBehaviour
    {
        [SerializeField] private float duration = 0.18f;
        [SerializeField] private float scaleMultiplier = 1.25f;
        [SerializeField] private Ease ease = Ease.OutQuad;
        [SerializeField] private Image image;
        
        private Sequence _sequence;

        public Sequence Play()
        {
            _sequence?.Kill(true);
            
            _sequence = DOTween.Sequence();
            var targetScale = transform.localScale * scaleMultiplier;
            
            _sequence.Append(transform.DOScale(targetScale, duration).SetEase(ease));
            _sequence.Append(image.DOFade(0f, duration));
            _sequence.Play();
            
            return _sequence;
        }

        public Tween GetTween()
        {
            var targetScale = transform.localScale * scaleMultiplier;
            return transform.DOScale(targetScale, duration).SetEase(ease);
        }
        
        private void OnDestroy()
        {
            _sequence?.Kill();
        }
    }
}