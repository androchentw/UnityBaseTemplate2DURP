using UnityEngine;
namespace Resources.Config
{
    [CreateAssetMenu(fileName = "EnvironmentConfig", menuName = "Config/Environment Config")]
    public class EnvironmentConfig : ScriptableObject
    {
        [Header("Version Information")]
        [SerializeField] private string version = "1.2.0";
        [SerializeField] private string buildNumber = "";
        
        [Header("Environment")]
        [SerializeField] private bool isDevelopment = true;
        [SerializeField] private string environmentName = "Development";
        
        public string Version => version;
        public string BuildNumber => string.IsNullOrEmpty(buildNumber) 
            ? System.DateTime.Now.ToString("yyyyMMdd.HHmm") 
            : buildNumber;
        public string FullVersion => $"v{Version}-{BuildNumber}";
        public bool IsDevelopment => isDevelopment;
        public string EnvironmentName => environmentName;

        // Editor-only method to set build number for builds
#if UNITY_EDITOR
        public void SetBuildNumber(string newBuildNumber)
        {
            buildNumber = newBuildNumber;
            UnityEditor.EditorUtility.SetDirty(this);
            UnityEditor.AssetDatabase.SaveAssets();
        }
#endif
    }
}
