using System;
namespace Mvp.Presenters
{
  public interface IMainMenuPresenter : IDisposable
  {
    // Initialization
    void Initialize();

    // Command Methods
    void StartNewGame();
    void LoadGame();
    void OpenSettings();
    void QuitGame();
  }
}
