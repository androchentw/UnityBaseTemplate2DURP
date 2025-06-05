using Mvp.Views.Screens;
using Utils;


namespace Mvp.Presenters
{
    public class MainMenuPresenter : IMainMenuPresenter
    {
        private readonly IMainMenuView _view;
        private bool _isViewReady;

        public MainMenuPresenter(IMainMenuView view)
        {
            _view = view;
            FhLog.I("MainMenuPresenter initialized");
        }

        public void OnViewReady()
        {
            _isViewReady = true;
            FhLog.I("MainMenuView is ready");
        }

        public void OnNewGameClicked()
        {
            if (!_isViewReady) return;
            
            FhLog.I("Starting new game...");
            _view.ShowMessage("Starting new game...");
            
            // TODO: Implement new game logic here
            // Example: SceneManager.LoadScene("GameScene");
        }


        public void OnLoadGameClicked()
        {
            if (!_isViewReady) return;
            
            FhLog.I("Loading saved game...");
            _view.ShowMessage("Loading saved game...");
            
            // TODO: Implement load game logic here
        }

        public void OnSettingsClicked()
        {
            if (!_isViewReady) return;
            
            FhLog.I("Opening settings...");
            _view.ShowMessage("Opening settings...");
            
            // TODO: Implement settings logic here
        }

        public void OnQuitClicked()
        {
            if (!_isViewReady) return;
            
            FhLog.I("Quitting application...");
            _view.ShowMessage("Quitting application...");
        }
    }
}
