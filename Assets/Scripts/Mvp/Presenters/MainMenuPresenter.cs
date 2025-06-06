using System;
using Mvp.Views.Screens;
using Utils;

namespace Mvp.Presenters
{
  public class MainMenuPresenter : IMainMenuPresenter
  {
    private readonly IMainMenuView _view;
    private bool _isInitialized;

    public MainMenuPresenter(IMainMenuView view)
    {
      _view = view ?? throw new ArgumentNullException(nameof(view));
      FhLog.I($"{nameof(MainMenuPresenter)} initialized");
    }

    public void Initialize()
    {
      if (_isInitialized) return;

      // Subscribe to view events
      _view.NewGameClicked += OnNewGameClicked;
      _view.LoadGameClicked += OnLoadGameClicked;
      _view.SettingsClicked += OnSettingsClicked;
      _view.QuitClicked += OnQuitClicked;

      _isInitialized = true;
      FhLog.I($"{nameof(MainMenuPresenter)} initialized and ready");
    }

    public void StartNewGame()
    {
      if (!_isInitialized) return;

      FhLog.I("Starting new game...");
      _view.ShowMessage("Starting new game...");
      // SceneManager.LoadScene("GameScene");
    }

    public void LoadGame()
    {
      if (!_isInitialized) return;

      FhLog.I("Loading saved game...");
      _view.ShowMessage("Loading saved game...");
    }

    public void OpenSettings()
    {
      if (!_isInitialized) return;

      FhLog.I("Opening settings...");
      _view.ShowMessage("Opening settings...");
    }

    public void QuitGame()
    {
      if (!_isInitialized) return;

      FhLog.I("Quitting application...");
      _view.ShowMessage("Quitting application...");

            #if UNITY_EDITOR
      UnityEditor.EditorApplication.isPlaying = false;
            #else
                Application.Quit();
            #endif
    }

    public void Dispose()
    {
      if (!_isInitialized) return;

      // Unsubscribe from view events
      _view.NewGameClicked -= OnNewGameClicked;
      _view.LoadGameClicked -= OnLoadGameClicked;
      _view.SettingsClicked -= OnSettingsClicked;
      _view.QuitClicked -= OnQuitClicked;

      _isInitialized = false;
      FhLog.I($"{nameof(MainMenuPresenter)} disposed");
    }

        #region Event Handlers

    private void OnNewGameClicked() => StartNewGame();
    private void OnLoadGameClicked() => LoadGame();
    private void OnSettingsClicked() => OpenSettings();
    private void OnQuitClicked() => QuitGame();

        #endregion
  }
}
