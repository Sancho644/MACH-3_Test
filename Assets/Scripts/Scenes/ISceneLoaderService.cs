using System;

namespace Scenes
{
    public interface ISceneLoaderService
    {
        public void Load(SceneName sceneName, Action onLoadComplete);
    }
}