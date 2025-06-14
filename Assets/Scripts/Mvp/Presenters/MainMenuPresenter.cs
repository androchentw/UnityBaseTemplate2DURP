using System;
using System.Linq;
using Mvp.Views.Screens;
using Utils;
using Managers.Gameplay;

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

      // Ensure SaveManager exists
      SaveManager.EnsureExists();

      // Generate a simple ID for the player
      string playerId = Guid.NewGuid().ToString().Substring(0, 8);
      string playerName = $"Player_{playerId}";

      if (SaveManager.Instance && SaveManager.Instance.SaveGame(playerName, playerId))
      {
        string message = $"New Player: {playerName}\nID: {playerId}";
        _view.ShowMessage(message);
        _view.UpdateClickedButtonInfo(message);

        // Here you would typically load the game scene
        // SceneManager.LoadScene("GameScene");
      }
      else
      {
        const string error = "Failed to create new game save";
        _view.ShowMessage(error);
        FhLog.E(error);
      }
    }

    public void LoadGame()
    {
      if (!_isInitialized) return;

      // Ensure SaveManager exists
      SaveManager.EnsureExists();

      if (!SaveManager.Instance)
      {
        _view.ShowMessage("Failed to initialize save system");
        return;
      }

      var saveFiles = SaveManager.Instance.GetAllSaveFiles();
      if (saveFiles.Count == 0)
      {
        string message = "No save files found";
        _view.ShowMessage(message);
        _view.UpdateClickedButtonInfo("No Saves Available");
        FhLog.I(message);
        return;
      }

      // Try to load the most recent save
      string latestSave = saveFiles.FirstOrDefault();
      if (SaveManager.Instance.LoadGame(latestSave))
      {
        var saveData = SaveManager.Instance.CurrentSaveData;
        string message = $"Game loaded!\nPlayer: {saveData.playerName}\nSaved: {saveData.SaveTime}";
        _view.ShowMessage(message);
        _view.UpdateClickedButtonInfo($"Loaded: {saveData.playerName}");
        FhLog.I(message);

        // Here you would typically load the game scene with the loaded data
        // SceneManager.LoadScene("GameScene");
      }
      else
      {
        string error = "Failed to load game save";
        _view.ShowMessage(error);
        _view.UpdateClickedButtonInfo("Load Failed");
        FhLog.E(error);
      }
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
