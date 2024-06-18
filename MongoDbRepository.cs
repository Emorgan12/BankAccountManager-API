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

        public Task<Account> GetItemAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Account>> GetItemsAsync()
        {
            throw new NotImplementedException();
        }

        public Task CreateItemAsync(Account account)
        {
            throw new NotImplementedException();
        }

        public Task UpdateItemAsync(Account account)
        {
            throw new NotImplementedException();
        }

        public Task DeleteItemAsync(Guid id)
        {
            throw new NotImplementedException();
        }
    }
}