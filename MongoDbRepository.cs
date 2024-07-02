using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Driver;

namespace BankAccountManager{

    public class MongoDbRepository : AccountsRepository
    {
        public MongoDbRepository(IMongoClient mongoClient)
        {
            IMongoDatabase database = mongoClient.GetDatabase(databaseName);
            accountsCollection = database.GetCollection<Account>(collectionName);
        }
        private const string databaseName = "bankAccounts";
        private const string collectionName = "accounts";
        private readonly IMongoCollection<Account> accountsCollection;
        private readonly FilterDefinitionBuilder<Account> filterBuilder = Builders<Account>.Filter;

        public async Task<Account> GetItemAsync(Guid id)
        {
            var filter = filterBuilder.Eq(item => item.Id, id);
            return await accountsCollection.Find(filter).SingleOrDefaultAsync();
        }

        public async Task<AccountDto> LoginAsync(string Username, string Password)
        {
            var filter = filterBuilder.Eq(item => item.Password, Password) & filterBuilder.Eq(item => item.Username, Username);
            var Currentaccount = await accountsCollection.Find(filter).SingleOrDefaultAsync();
            return Currentaccount.AsDto();
        }

        public async Task CreateItemAsync(Account account)
        {
            await accountsCollection.InsertOneAsync(account);
        }

        public Task UpdateItemAsync(Account account)
        {
            throw new NotImplementedException();
        }

        public async Task DeleteItemAsync(Guid id)
        {
            var filter = filterBuilder.Eq(item => item.Id, id);
            await accountsCollection.DeleteOneAsync(filter);
        }

        public async Task<IEnumerable<Account>> GetItemsAsync()
        {
            return await accountsCollection.Find(new BsonDocument()).ToListAsync();
        }
    }
}