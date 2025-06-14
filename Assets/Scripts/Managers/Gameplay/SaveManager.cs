using System;
using System.IO;
using System.Text;
using UnityEngine;
using System.Linq;
using System.Collections.Generic;

namespace Managers.Gameplay
{
    [Serializable]
    public class SaveData
    {
        public string playerName;
        public string id;
        public DateTime SaveTime;
    }

    public class SaveManager : MonoBehaviour
    {
        private const string EncryptionKey = "YourSecretKey123"; // Change this to a secure key in production
        private const string SaveFileExtension = ".save";
        
        public static SaveManager Instance { get; private set; }
        
        public SaveData CurrentSaveData { get; private set; }
        public bool HasSaveData => CurrentSaveData != null;
        
        public static void EnsureExists()
        {
            if (!Instance)
            {
                GameObject go = new GameObject("SaveManager");
                Instance = go.AddComponent<SaveManager>();
                DontDestroyOnLoad(go);
            }
        }

        private void Awake()
        {
            if (!Instance)
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else if (Instance != this)
            {
                Destroy(gameObject);
            }
        }

        private static string EncryptDecrypt(string data)
        {
            var input = new StringBuilder(data);
            var output = new StringBuilder(data.Length);

            for (int i = 0; i < data.Length; i++)
            {
                char key = EncryptionKey[i % EncryptionKey.Length];
                output.Append((char)(input[i] ^ key));
            }
            
            return output.ToString();
        }

        public bool SaveGame(string playerName, string id)
        {
            try
            {
                CurrentSaveData = new SaveData
                {
                    playerName = playerName,
                    id = id,
                    SaveTime = DateTime.Now
                };

                string jsonData = JsonUtility.ToJson(CurrentSaveData, true);
                string encryptedData = EncryptDecrypt(jsonData);
                
                string fileName = $"{CurrentSaveData.SaveTime:yyyyMMdd-HHmmss}-{playerName}{SaveFileExtension}";
                string filePath = Path.Combine(Application.persistentDataPath, fileName);
                
                File.WriteAllText(filePath, encryptedData);
                PlayerPrefs.SetString("LastSavePath", filePath);
                PlayerPrefs.Save();
                
                Debug.Log($"Game saved successfully to: {filePath}");
                return true;
            }
            catch (Exception e)
            {
                Debug.LogError($"Error saving game: {e.Message}");
                return false;
            }
        }

        public bool LoadGame(string filePath = null)
        {
            try
            {
                if (string.IsNullOrEmpty(filePath))
                {
                    filePath = PlayerPrefs.GetString("LastSavePath");
                    if (string.IsNullOrEmpty(filePath) || !File.Exists(filePath))
                    {
                        Debug.LogWarning("No save file found");
                        return false;
                    }
                }

                if (!File.Exists(filePath))
                {
                    Debug.LogError($"Save file not found: {filePath}");
                    return false;
                }

                string encryptedData = File.ReadAllText(filePath);
                string jsonData = EncryptDecrypt(encryptedData);
                
                CurrentSaveData = JsonUtility.FromJson<SaveData>(jsonData);
                
                if (CurrentSaveData == null)
                {
                    Debug.LogError("Failed to deserialize save data");
                    return false;
                }
                
                Debug.Log($"Game loaded successfully from: {filePath}");
                return true;
            }
            catch (Exception e)
            {
                Debug.LogError($"Error loading game: {e.Message}");
                return false;
            }
        }

        public bool DeleteSave(string filePath)
        {
            try
            {
                if (!File.Exists(filePath))
                    return false;
                File.Delete(filePath);
                    
                // If we're deleting the current save, clear the reference
                if (CurrentSaveData != null && 
                    filePath.Contains(CurrentSaveData.playerName) && 
                    filePath.Contains(CurrentSaveData.SaveTime.ToString("yyyyMMdd-HHmmss")))
                {
                    CurrentSaveData = null;
                    PlayerPrefs.DeleteKey("LastSavePath");
                    PlayerPrefs.Save();
                }
                    
                Debug.Log($"Save file deleted: {filePath}");
                return true;
            }
            catch (Exception e)
            {
                Debug.LogError($"Error deleting save file: {e.Message}");
                return false;
            }
        }

        public List<string> GetAllSaveFiles()
        {
            try
            {
                if (!Directory.Exists(Application.persistentDataPath))
                {
                    return new List<string>();
                }
                
                return Directory.GetFiles(Application.persistentDataPath, $"*{SaveFileExtension}")
                    .OrderByDescending(f => new FileInfo(f).LastWriteTime)
                    .ToList();
            }
            catch (Exception e)
            {
                Debug.LogError($"Error getting save files: {e.Message}");
                return new List<string>();
            }
        }
    }
}
