using System.Threading.Tasks;
using UI.Windows.MainMenu;

namespace UI.Services.Factory
{
    public interface IUIFactoryService
    {
        public void CreateMainMenu();
        public void CreateGamePlayWindow();
        public Task CreateGameCanvas();
    }
}