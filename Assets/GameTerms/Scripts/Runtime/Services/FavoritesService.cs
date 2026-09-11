using System;
using System.Collections.Generic;
using System.Linq;

namespace GameTerms
{
    public sealed class FavoritesService
    {
        private readonly HashSet<string> favoriteIds = new(StringComparer.Ordinal);
        private readonly UserDataService userData;

        public event Action Changed;

        public FavoritesService(UserDataService userData)
        {
            this.userData = userData;
            foreach (var id in userData.Snapshot.FavoriteTermIds)
            {
                favoriteIds.Add(id);
            }
        }

        public bool IsFavorite(string termId) => favoriteIds.Contains(termId);

        public IReadOnlyList<string> GetFavoriteIds() => userData.Snapshot.FavoriteTermIds.ToList();

        public void ToggleFavorite(string termId)
        {
            if (string.IsNullOrWhiteSpace(termId))
            {
                return;
            }

            if (favoriteIds.Contains(termId))
            {
                favoriteIds.Remove(termId);
                userData.Snapshot.FavoriteTermIds.Remove(termId);
            }
            else
            {
                favoriteIds.Add(termId);
                userData.Snapshot.FavoriteTermIds.Insert(0, termId);
            }

            userData.Save();
            Changed?.Invoke();
        }
    }
}
