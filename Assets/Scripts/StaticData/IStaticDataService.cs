using StaticData.Windows;
using UI.Services.Windows;

namespace StaticData
{
    public interface IStaticDataService
    {
        public void Initialize();
        public WindowConfig ForWindow(WindowType windowType);
    }
}