using System;
using UI.Services.Windows;
using UI.Windows;

namespace StaticData.Windows
{
    [Serializable]
    public class WindowConfig
    {
        public WindowType WindowType;
        public AbstractWindow Prefab;
    }
}