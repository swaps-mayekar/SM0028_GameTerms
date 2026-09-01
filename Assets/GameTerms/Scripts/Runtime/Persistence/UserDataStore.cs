using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace GameTerms.Persistence
{
    [Serializable]
    public sealed class UserDataSnapshot
    {
        public int Version = 1;
        public List<string> FavoriteTermIds = new();
        public List<string> RecentlyViewedTermIds = new();
        public string LastRandomTermId;
    }

    public interface IUserDataStore
    {
        UserDataSnapshot Load();
        void Save(UserDataSnapshot snapshot);
    }

    public sealed class JsonUserDataStore : IUserDataStore
    {
        private const int CurrentVersion = 1;
        private readonly string filePath;

        public JsonUserDataStore(string fileName = "gameterms_userdata.json")
        {
            filePath = Path.Combine(Application.persistentDataPath, fileName);
        }

        public UserDataSnapshot Load()
        {
            if (!File.Exists(filePath))
            {
                return new UserDataSnapshot { Version = CurrentVersion };
            }

            try
            {
                var json = File.ReadAllText(filePath);
                var snapshot = JsonUtility.FromJson<UserDataSnapshot>(json) ?? new UserDataSnapshot();
                snapshot.FavoriteTermIds ??= new List<string>();
                snapshot.RecentlyViewedTermIds ??= new List<string>();
                snapshot.Version = CurrentVersion;
                return snapshot;
            }
            catch (Exception exception)
            {
                Debug.LogWarning($"Failed to load user data: {exception.Message}");
                return new UserDataSnapshot { Version = CurrentVersion };
            }
        }

        public void Save(UserDataSnapshot snapshot)
        {
            snapshot.Version = CurrentVersion;
            snapshot.FavoriteTermIds ??= new List<string>();
            snapshot.RecentlyViewedTermIds ??= new List<string>();

            try
            {
                var directory = Path.GetDirectoryName(filePath);
                if (!string.IsNullOrEmpty(directory) && !Directory.Exists(directory))
                {
                    Directory.CreateDirectory(directory);
                }

                var json = JsonUtility.ToJson(snapshot, true);
                File.WriteAllText(filePath, json);
            }
            catch (Exception exception)
            {
                Debug.LogWarning($"Failed to save user data: {exception.Message}");
            }
        }
    }
}
