using UnityEngine;
using UnityEngine.UIElements;
using Utils;
using Mvp.Presenters;

// Move IMainMenuView interface to a separate file in the Mvp.Interfaces namespace
namespace Mvp.Views.Screens
{
    [RequireComponent(typeof(UIDocument))]
    public class MainMenuScreen : MonoBehaviour, IMainMenuView
    {
        // UI Elements
        private UIDocument _uiDocument;
        private Button _newGameButton;
        private Button _loadGameButton;
        private Button _settingsButton;
        private Button _quitButton;

        // Presenter reference
        private IMainMenuPresenter _presenter;

        private void Awake()
        {
            // Initialize presenter (could be injected via DI in a real scenario)
            _presenter = new MainMenuPresenter(this);
        }

        void OnEnable()
        {
            _uiDocument = GetComponent<UIDocument>();
            if (!_uiDocument)
            {
                FhLog.E("此 GameObject 上找不到 UIDocument 組件。", this);
                return;
            }

            var rootElement = _uiDocument.rootVisualElement;
            if (rootElement == null)
            {
                FhLog.E("UIDocument 中找不到 RootVisualElement。", this);
                return;
            }

            // Query all buttons from UXML
            _newGameButton = rootElement.Q<Button>("new-game-button");
            _loadGameButton = rootElement.Q<Button>("load-game-button");
            _settingsButton = rootElement.Q<Button>("settings-button");
            _quitButton = rootElement.Q<Button>("quit-button");

            // Register button click events
            RegisterButton(_newGameButton, OnNewGameClicked);
            RegisterButton(_loadGameButton, OnLoadGameClicked);
            RegisterButton(_settingsButton, OnSettingsClicked);
            RegisterButton(_quitButton, OnQuitClicked);

            // Notify presenter that view is ready
            _presenter.OnViewReady();
        }

        void OnDisable()
        {
            // Unregister all button click events
            UnregisterButton(_newGameButton, OnNewGameClicked);
            UnregisterButton(_loadGameButton, OnLoadGameClicked);
            UnregisterButton(_settingsButton, OnSettingsClicked);
            UnregisterButton(_quitButton, OnQuitClicked);
        }

        private void RegisterButton(Button button, EventCallback<ClickEvent> callback)
        {
            if (button != null)
            {
                button.RegisterCallback(callback);
                FhLog.I($"Button '{button.name}' event registered.");
            }
            else
            {
                FhLog.E($"Button not found in UXML.", this);
            }
        }

        private void UnregisterButton(Button button, EventCallback<ClickEvent> callback)
        {
            if (button != null)
            {
                button.UnregisterCallback(callback);
                FhLog.I($"Button '{button.name}' event unregistered.");
            }
        }

        #region Button Click Handlers

        private void OnNewGameClicked(ClickEvent evt)
        {
            FhLog.I("New Game button clicked.");
            _presenter.OnNewGameClicked();
        }

        private void OnLoadGameClicked(ClickEvent evt)
        {
            FhLog.I("Load Game button clicked.");
            _presenter.OnLoadGameClicked();
        }

        private void OnSettingsClicked(ClickEvent evt)
        {
            FhLog.I("Settings button clicked.");
            _presenter.OnSettingsClicked();
        }

        private void OnQuitClicked(ClickEvent evt)
        {
            FhLog.I("Quit button clicked.");
            _presenter.OnQuitClicked();
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
        }

        #endregion

        // IMainMenuView implementation
        public void ShowMessage(string message)
        {
            // Implement message display logic here
            FhLog.I($"UI Message: {message}");
        }
    }
}
