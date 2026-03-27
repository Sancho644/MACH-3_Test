using System;
using System.Collections;
using UnityEngine.SceneManagement;

namespace Scenes
{
    public class SceneLoaderService : ISceneLoaderService
    {
        private readonly ICoroutineRunner _coroutineRunner;

        public SceneLoaderService(ICoroutineRunner coroutineRunner)
        {
            _coroutineRunner = coroutineRunner;
        }

        public void Load(SceneName sceneName, Action onLoadComplete = null)
        {
            _coroutineRunner.StartCoroutine(LoadScene(sceneName, onLoadComplete));
        }

        private IEnumerator LoadScene(SceneName sceneName, Action onLoadComplete = null)
        {
            var strSceneName = sceneName.ToString();

            if (SceneManager.GetActiveScene().name == strSceneName)
            {
                onLoadComplete?.Invoke();
                yield break;
            }

            var waitNextScene = SceneManager.LoadSceneAsync(strSceneName);

            while (waitNextScene is { isDone: false })
                yield return null;

            onLoadComplete?.Invoke();
        }
    }
}