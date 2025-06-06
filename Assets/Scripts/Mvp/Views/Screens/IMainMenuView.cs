using System;
using UnityEngine.UIElements;

namespace Mvp.Views.Screens
{
  public interface IMainMenuView
  {
    // Events
    event Action NewGameClicked;
    event Action LoadGameClicked;
    event Action SettingsClicked;
    event Action QuitClicked;

    // UI Update Methods
    void UpdateClickedButtonInfo(string buttonId);
    void ShowMessage(string message);

    // Initialization
    void Initialize(VisualElement root);
  }
}
