using System.Threading.Tasks;

namespace UI.Services.Factory
{
    public interface IUIFactoryService
    {
        public void CreateMainMenuWindow();
        public void CreateGamePlayWindow();
        public Task CreateGameCanvas();
        public void CreateLowScoreWindow();
        public void CreateShowMainMenuWindow();
        public void CreateRecordsWindow();
    }
}