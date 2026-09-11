using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace GameTerms.Persistence
{
    [Serializable]
    public sealed class UserDataSnapshot
    {
        public int Version = 2;
        public List<string> FavoriteTermIds = new();
        public List<string> RecentlyViewedTermIds = new();
        public string LastRandomTermId;
        public List<TermProgressRecord> TermProgress = new();
        public int TotalQuizSessions;
        public int BestQuizScore;
        public int LastQuizScore;
        public int CurrentStreakDays;
        public string LastStudyDate;
        public List<string> LastMissedTermIds = new();
    }

    public interface IUserDataStore
    {
        UserDataSnapshot Load();
        void Save(UserDataSnapshot snapshot);
    }

    public static class UserDataMigrator
    {
        public const int CurrentVersion = 2;

        public static UserDataSnapshot Normalize(UserDataSnapshot snapshot)
        {
            snapshot ??= new UserDataSnapshot();
            snapshot.FavoriteTermIds ??= new List<string>();
            snapshot.RecentlyViewedTermIds ??= new List<string>();
            snapshot.TermProgress ??= new List<TermProgressRecord>();
            snapshot.LastMissedTermIds ??= new List<string>();

            if (snapshot.Version < CurrentVersion)
            {
                snapshot.Version = CurrentVersion;
            }

            snapshot.Version = CurrentVersion;
            return snapshot;
        }
    }

    public sealed class JsonUserDataStore : IUserDataStore
    {
        private readonly string filePath;

        public JsonUserDataStore(string fileName = "gameterms_userdata.json")
        {
            filePath = Path.Combine(Application.persistentDataPath, fileName);
        }

        public UserDataSnapshot Load()
        {
            if (!File.Exists(filePath))
            {
                return UserDataMigrator.Normalize(new UserDataSnapshot());
            }

            try
            {
                var json = File.ReadAllText(filePath);
                var snapshot = JsonUtility.FromJson<UserDataSnapshot>(json) ?? new UserDataSnapshot();
                return UserDataMigrator.Normalize(snapshot);
            }
            catch (Exception exception)
            {
                Debug.LogWarning($"Failed to load user data: {exception.Message}");
                return UserDataMigrator.Normalize(new UserDataSnapshot());
            }
        }

        public void Save(UserDataSnapshot snapshot)
        {
            snapshot = UserDataMigrator.Normalize(snapshot);

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
