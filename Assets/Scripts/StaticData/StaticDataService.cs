using System.Collections.Generic;
using System.Linq;
using StaticData.Windows;
using UI.Services.Windows;
using UnityEngine;

namespace StaticData
{
    public class StaticDataService : IStaticDataService
    {
        private const string StaticDataWindowsPath = "StaticData/UI/WindowsStaticData";

        private Dictionary<WindowType, WindowConfig> _windowConfigs;

        public void Initialize()
        {
            _windowConfigs = Resources
                .Load<WindowsStaticData>(StaticDataWindowsPath)
                .Configs
                .ToDictionary(x => x.WindowType, x => x);
        }

        public WindowConfig ForWindow(WindowType windowType) =>
            _windowConfigs.TryGetValue(windowType, out WindowConfig windowConfig) ? windowConfig : null;
    }
}