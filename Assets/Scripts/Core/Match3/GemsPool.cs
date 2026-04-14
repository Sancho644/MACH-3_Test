using System.Collections.Generic;
using UnityEngine;

namespace Core.Match3
{
    public class GemsPool : MonoBehaviour
    {
        [SerializeField] private bool prewarmPool = true;
        [SerializeField] private RectTransform poolRoot;
        
        private readonly Stack<GemView> _pool = new();
        
        private bool _poolPrewarmed;
        private BoardModel _model;
        private BoardStaticData _staticData;

        public void Init(BoardModel model, BoardStaticData staticData)
        {
            _model = model;
            _staticData = staticData;

            if (prewarmPool)
            {
                PrewarmPool();
            }
        }
        
        public void ReleaseView(GemView view)
        {
            if (view == null)
                return;

            view.ResetVisuals();
            view.SetGem(null);
            view.gameObject.SetActive(false);
            view.transform.SetParent(poolRoot, false);
            _pool.Push(view);
        }
        
        public GemView GetView()
        {
            GemView view = _pool.Count > 0 ? _pool.Pop() : Instantiate(_staticData.GemPrefab, poolRoot);
            view.transform.SetParent(poolRoot, false);
            view.gameObject.SetActive(true);
            view.ResetVisuals();
            
            return view;
        }

        private void PrewarmPool()
        {
            if (_poolPrewarmed || _staticData == null || _staticData.GemPrefab == null || _model == null)
            {
                return;
            }

            var total = _model.Width * _model.Height;
            for (var i = 0; i < total; i++)
            {
                var view = Instantiate(_staticData.GemPrefab, poolRoot);
                view.gameObject.SetActive(false);
                _pool.Push(view);
            }

            _poolPrewarmed = true;
        }
    }
}