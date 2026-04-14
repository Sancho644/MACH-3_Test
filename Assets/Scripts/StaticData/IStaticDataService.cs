using System.Threading.Tasks;
using Core.Match3;
using Core.Match3.Board;
using StaticData.Windows;
using UI.Services.Windows;

namespace StaticData
{
    public interface IStaticDataService
    {
        public Task Initialize();
        public WindowConfig GetWindowConfig(WindowType windowType);
        public BoardStaticData GetBoardConfig();
    }
}