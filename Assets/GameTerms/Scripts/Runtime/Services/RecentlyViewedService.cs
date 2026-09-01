using System;
using System.Collections.Generic;
using System.Linq;
using GameTerms.Persistence;

namespace GameTerms
{
    public sealed class RecentlyViewedService
    {
        public const int MaxRecentCount = 20;

        private readonly IUserDataStore dataStore;
        private UserDataSnapshot snapshot;

        public event Action Changed;

        public RecentlyViewedService(IUserDataStore dataStore)
        {
            this.dataStore = dataStore;
            snapshot = dataStore.Load();
            snapshot.RecentlyViewedTermIds ??= new List<string>();
        }

        public IReadOnlyList<string> GetRecentIds() => snapshot.RecentlyViewedTermIds.ToList();

        public void RecordView(string termId)
        {
            if (string.IsNullOrWhiteSpace(termId))
            {
                return;
            }

            snapshot.RecentlyViewedTermIds.Remove(termId);
            snapshot.RecentlyViewedTermIds.Insert(0, termId);

            if (snapshot.RecentlyViewedTermIds.Count > MaxRecentCount)
            {
                snapshot.RecentlyViewedTermIds = snapshot.RecentlyViewedTermIds
                    .Take(MaxRecentCount)
                    .ToList();
            }

            dataStore.Save(snapshot);
            Changed?.Invoke();
        }
    }
}
