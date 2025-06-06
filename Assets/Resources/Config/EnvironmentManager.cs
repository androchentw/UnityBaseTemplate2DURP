using System;
using UnityEngine;

namespace Resources.Config
{
  /// <summary>
  /// Manages the environment configuration for the application.
  /// Provides thread-safe access to the EnvironmentConfig asset.
  /// </summary>
  public static class EnvironmentManager
  {
    private const string ConfigPath = "Config/EnvironmentConfig";
    private static EnvironmentConfig _config;
    private static readonly object Lock = new object();
    private static bool _isInitialized;

    /// <summary>
    /// Gets the current environment configuration.
    /// </summary>
    /// <exception cref="System.InvalidOperationException">Thrown when configuration fails to load.</exception>
    public static EnvironmentConfig Config
    {
      get
      {
        EnsureInitialized();
        return _config ?? throw new InvalidOperationException("Failed to load environment configuration.");
      }
    }

    /// <summary>
    /// Safely gets the environment configuration if available.
    /// </summary>
    /// <param name="config">The loaded configuration, or null if not available.</param>
    /// <returns>True if the configuration was loaded successfully, false otherwise.</returns>
    public static bool TryGetConfig(out EnvironmentConfig config)
    {
      EnsureInitialized();
      config = _config;
      return config is not null;
    }

    /// <summary>
    /// Forces a reload of the environment configuration.
    /// </summary>
    public static void ReloadConfig()
    {
      lock (Lock)
      {
        _isInitialized = false;
        _config = null;
      }
      EnsureInitialized();
    }

    private static void EnsureInitialized()
    {
      if (_isInitialized) return;

      lock (Lock)
      {
        if (_isInitialized) return;

        try
        {
          _config = UnityEngine.Resources.Load<EnvironmentConfig>(ConfigPath);

          if (!_config)
          {
            Debug.LogWarning($"EnvironmentConfig not found at path: {ConfigPath}");
#if UNITY_EDITOR
            CreateDefaultConfig();
            _config = UnityEngine.Resources.Load<EnvironmentConfig>(ConfigPath);
#endif
          }

          if (_config)
          {
            Debug.Log($"Environment config loaded. Version: {_config.Version}, Environment: {_config.EnvironmentName}");
          }
        }
        catch (Exception ex)
        {
          Debug.LogError($"Failed to load environment configuration: {ex.Message}");
          throw;
        }
        finally
        {
          _isInitialized = true;
        }
      }
    }

#if UNITY_EDITOR
    /// <summary>
    /// Creates a default environment configuration file if it doesn't exist.
    /// </summary>
    [UnityEditor.MenuItem("Tools/Config/Create Environment Config")]
    public static void CreateDefaultConfig()
    {
      try
      {
        // Ensure Resources directory exists
        if (!UnityEditor.AssetDatabase.IsValidFolder("Assets/Resources"))
        {
          UnityEditor.AssetDatabase.CreateFolder("Assets", "Resources");
        }

        // Ensure Config directory exists
        if (!UnityEditor.AssetDatabase.IsValidFolder("Assets/Resources/Config"))
        {
          UnityEditor.AssetDatabase.CreateFolder("Assets/Resources", "Config");
        }

        string configPath = "Assets/Resources/Config/EnvironmentConfig.asset";

        // Only create if it doesn't exist
        if (!UnityEditor.AssetDatabase.LoadAssetAtPath<EnvironmentConfig>(configPath))
        {
          var config = ScriptableObject.CreateInstance<EnvironmentConfig>();
          UnityEditor.AssetDatabase.CreateAsset(config, configPath);
          UnityEditor.AssetDatabase.SaveAssets();
          Debug.Log($"Created default EnvironmentConfig at: {configPath}");
        }
        else
        {
          Debug.Log($"EnvironmentConfig already exists at: {configPath}");
        }
      }
      catch (Exception ex)
      {
        Debug.LogError($"Failed to create default environment config: {ex.Message}");
        throw;
      }
    }
#endif
  }
}
