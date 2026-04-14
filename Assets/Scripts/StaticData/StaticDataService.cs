using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core.AssetManagement;
using Core.Match3.Board;
using StaticData.Windows;
using UI.Services.Windows;

namespace StaticData
{
    public class StaticDataService : IStaticDataService
    {
        private const string StaticDataWindowsPath = "WindowsStaticData";
        private const string StaticDataBoardPath = "BoardStaticData";

        private Dictionary<WindowType, WindowConfig> _windowConfigs;
        private BoardStaticData _boardConfigs;
        
        private readonly IAssets _assets;

        public StaticDataService(IAssets assets)
        {
            _assets = assets;
        }
        
        public async Task Initialize()
        {
            var windowsStaticData = await _assets.Load<WindowsStaticData>(StaticDataWindowsPath);
            _windowConfigs = windowsStaticData.Configs.ToDictionary(x => x.WindowType, x => x);

            _boardConfigs = await _assets.Load<BoardStaticData>(StaticDataBoardPath);
        }

        public WindowConfig GetWindowConfig(WindowType windowType)
        {
            return _windowConfigs.GetValueOrDefault(windowType);
        }

        public BoardStaticData GetBoardConfig() => _boardConfigs;
    }
}