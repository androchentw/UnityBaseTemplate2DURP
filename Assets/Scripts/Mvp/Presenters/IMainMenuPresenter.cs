namespace Mvp.Presenters
{
    public interface IMainMenuPresenter
    {
        void OnViewReady();
        void OnNewGameClicked();
        void OnLoadGameClicked();
        void OnSettingsClicked();
        void OnQuitClicked();
    }
}
