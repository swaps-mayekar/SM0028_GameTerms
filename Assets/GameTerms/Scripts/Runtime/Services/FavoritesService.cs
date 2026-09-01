using System;
using System.Collections.Generic;
using System.Linq;
using GameTerms.Persistence;

namespace GameTerms
{
    public sealed class FavoritesService
    {
        private readonly HashSet<string> favoriteIds = new(StringComparer.Ordinal);
        private readonly IUserDataStore dataStore;
        private UserDataSnapshot snapshot;

        public event Action Changed;

        public FavoritesService(IUserDataStore dataStore)
        {
            this.dataStore = dataStore;
            snapshot = dataStore.Load();
            foreach (var id in snapshot.FavoriteTermIds)
            {
                favoriteIds.Add(id);
            }
        }

        public bool IsFavorite(string termId) => favoriteIds.Contains(termId);

        public IReadOnlyList<string> GetFavoriteIds() => snapshot.FavoriteTermIds.ToList();

        public void ToggleFavorite(string termId)
        {
            if (string.IsNullOrWhiteSpace(termId))
            {
                return;
            }

            if (favoriteIds.Contains(termId))
            {
                favoriteIds.Remove(termId);
                snapshot.FavoriteTermIds.Remove(termId);
            }
            else
            {
                favoriteIds.Add(termId);
                snapshot.FavoriteTermIds.Insert(0, termId);
            }

            Persist();
            Changed?.Invoke();
        }

        private void Persist()
        {
            dataStore.Save(snapshot);
        }
    }
}
