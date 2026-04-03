using System.Threading.Tasks;

namespace UI.Services.Factory
{
    public interface IUIFactoryService
    {
        public void CreateMainMenu();
        public void CreateGamePlayWindow();
        public Task CreateGameCanvas();
        public void CreateLowScoreWindow();
        public void CreateShowMainMenuWindow();
    }
}