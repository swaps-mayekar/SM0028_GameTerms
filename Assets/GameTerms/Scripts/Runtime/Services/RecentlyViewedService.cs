using System;
using System.Collections.Generic;
using System.Linq;

namespace GameTerms
{
    public sealed class RecentlyViewedService
    {
        public const int MaxRecentCount = 20;

        private readonly UserDataService userData;

        public event Action Changed;

        public RecentlyViewedService(UserDataService userData)
        {
            this.userData = userData;
            userData.Snapshot.RecentlyViewedTermIds ??= new List<string>();
        }

        public IReadOnlyList<string> GetRecentIds() => userData.Snapshot.RecentlyViewedTermIds.ToList();

        public void RecordView(string termId)
        {
            if (string.IsNullOrWhiteSpace(termId))
            {
                return;
            }

            var ids = userData.Snapshot.RecentlyViewedTermIds;
            var index = ids.IndexOf(termId);
            if (index == 0)
            {
                return;
            }

            if (index > 0)
            {
                ids.RemoveAt(index);
            }

            ids.Insert(0, termId);

            if (ids.Count > MaxRecentCount)
            {
                userData.Snapshot.RecentlyViewedTermIds = ids.Take(MaxRecentCount).ToList();
            }

            userData.Save();
            Changed?.Invoke();
        }
    }
}
