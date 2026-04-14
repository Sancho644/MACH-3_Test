using System.Collections.Generic;
using System.Linq;
using Core.Match3;
using StaticData.Windows;
using UI.Services.Windows;
using UnityEngine;

namespace StaticData
{
    public class StaticDataService : IStaticDataService
    {
        private const string StaticDataWindowsPath = "StaticData/UI/WindowsStaticData";
        private const string StaticDataBoardPath = "StaticData/UI/BoardStaticData";

        private Dictionary<WindowType, WindowConfig> _windowConfigs;
        private BoardStaticData _boardConfigs;

        public void Initialize()
        {
            _windowConfigs = Resources
                .Load<WindowsStaticData>(StaticDataWindowsPath)
                .Configs
                .ToDictionary(x => x.WindowType, x => x);

            _boardConfigs = Resources.Load<BoardStaticData>(StaticDataBoardPath);
        }

        public WindowConfig GetWindowConfig(WindowType windowType) =>
            _windowConfigs.TryGetValue(windowType, out WindowConfig windowConfig) ? windowConfig : null;

        public BoardStaticData GetBoardConfig() => _boardConfigs;
    }
}