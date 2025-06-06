using System;
using UnityEngine;
using UnityEngine.UIElements;
using Utils;
using Mvp.Presenters;

namespace Mvp.Views.Screens
{
    [RequireComponent(typeof(UIDocument))]
    public class MainMenuScreen : MonoBehaviour, IMainMenuView
    {
        // UI Elements
        private UIDocument _uiDocument;
        private Label _versionLabel;
        private Label _clickedButtonInfo;
        private Button _newGameButton;
        private Button _loadGameButton;
        private Button _settingsButton;
        private Button _quitButton;

        // Events
        public event Action NewGameClicked;
        public event Action LoadGameClicked;
        public event Action SettingsClicked;
        public event Action QuitClicked;

        // Presenter reference
        private IMainMenuPresenter _presenter;

        private void Awake()
        {
            _uiDocument = GetComponent<UIDocument>();
            
            // Initialize presenter (could be injected via DI in a real scenario)
            _presenter = new MainMenuPresenter(this);
        }

        private void Start()
        {
            if (!_uiDocument || _uiDocument.rootVisualElement == null)
            {
                FhLog.E("UIDocument or rootVisualElement is not set.", this);
                return;
            }
            
            Initialize(_uiDocument.rootVisualElement);
            _presenter.Initialize();
        }

        private void OnDestroy()
        {
            if (_presenter is IDisposable disposable)
            {
                disposable.Dispose();
            }
        }

        public void Initialize(VisualElement root)
        {
            if (root == null)
            {
                FhLog.E("Root VisualElement is null.", this);
                return;
            }

            // Query UI elements
            _versionLabel = root.Q<Label>("version-label");
            _clickedButtonInfo = root.Q<Label>("clicked-button-info");
            _newGameButton = root.Q<Button>("new-game-button");
            _loadGameButton = root.Q<Button>("load-game-button");
            _settingsButton = root.Q<Button>("settings-button");
            _quitButton = root.Q<Button>("quit-button");

            // Register button click events
            RegisterButton(_newGameButton, OnNewGameClicked);
            RegisterButton(_loadGameButton, OnLoadGameClicked);
            RegisterButton(_settingsButton, OnSettingsClicked);
            RegisterButton(_quitButton, OnQuitClicked);

            // Initial UI state
            UpdateClickedButtonInfo("None");
            UpdateVersionText();
            
            FhLog.I($"{nameof(MainMenuScreen)} initialized");
        }

        private void RegisterButton(Button button, Action<ClickEvent> clickHandler)
        {
            if (button != null)
            {
                button.clicked += () => clickHandler?.Invoke(null);
                FhLog.I($"Button '{button.name}' registered.");
            }
            else
            {
                FhLog.E("Button not found in UXML.", this);
            }
        }

        private void UpdateVersionText()
        {
            if (_versionLabel == null)
                return;
                
            const string version = "0.1.0";
            string buildTimestamp = DateTime.Now.ToString("yyyyMMdd.HHmm");
            string versionText = $"v{version}-{buildTimestamp}";
            
            _versionLabel.text = versionText;
            FhLog.I($"Version: {versionText}");
        }

        #region IMainMenuView Implementation

        public void UpdateClickedButtonInfo(string buttonId)
        {
            if (_clickedButtonInfo != null)
            {
                _clickedButtonInfo.text = $"Last clicked: {buttonId}";
            }
        }

        public void ShowMessage(string message)
        {
            // In a real app, you might show this in a toast or dialog
            FhLog.I($"UI Message: {message}");
        }

        #endregion

        #region Button Click Handlers

        private void OnNewGameClicked(ClickEvent evt)
        {
            UpdateClickedButtonInfo("new-game-button");
            NewGameClicked?.Invoke();
        }

        private void OnLoadGameClicked(ClickEvent evt)
        {
            UpdateClickedButtonInfo("load-game-button");
            LoadGameClicked?.Invoke();
        }

        private void OnSettingsClicked(ClickEvent evt)
        {
            UpdateClickedButtonInfo("settings-button");
            SettingsClicked?.Invoke();
        }

        private void OnQuitClicked(ClickEvent evt)
        {
            UpdateClickedButtonInfo("quit-button");
            QuitClicked?.Invoke();
        }

        #endregion
    }
}
