using Game.Match3;
using StaticData.Windows;
using UI.Services.Windows;

namespace StaticData
{
    public interface IStaticDataService
    {
        public void Initialize();
        public WindowConfig GetWindowConfig(WindowType windowType);
        public BoardStaticData GetBoardConfig();
    }
}