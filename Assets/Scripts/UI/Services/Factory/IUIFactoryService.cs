using System.Threading.Tasks;

namespace UI.Services.Factory
{
    public interface IUIFactoryService
    {
        public void CreateMainMenuWindow();
        public void CreateGameplayWindow();
        public Task CreateGameCanvas();
        public void CreateLowScoreWindow();
        public void CreateShowMainMenuWindow();
        public void CreateRecordsWindow();
    }
}