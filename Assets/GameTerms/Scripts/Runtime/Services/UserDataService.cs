using System;
using GameTerms.Persistence;

namespace GameTerms
{
    public sealed class UserDataService
    {
        private readonly IUserDataStore dataStore;

        public UserDataSnapshot Snapshot { get; }

        public event Action Changed;

        public UserDataService(IUserDataStore dataStore)
        {
            this.dataStore = dataStore;
            Snapshot = UserDataMigrator.Normalize(dataStore.Load());
        }

        public void Save()
        {
            dataStore.Save(Snapshot);
            Changed?.Invoke();
        }
    }
}
