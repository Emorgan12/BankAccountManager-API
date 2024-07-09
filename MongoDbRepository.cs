using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Amazon.Runtime.Internal.Util;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.Extensions.Http.Logging;
using Microsoft.Extensions.Logging;
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

        private readonly ILogger<MongoDbRepository> logger;

    
        public async Task<Account> GetItemAsync(string Username, string Password)
        {
            var filter = filterBuilder.Eq(item => item.Password, Password) & filterBuilder.Eq(item => item.Username, Username);
            var Currentaccount = await accountsCollection.Find(filter).SingleOrDefaultAsync();
            return Currentaccount;
        }

        public async Task CreateItemAsync(Account account)
        {
        
            await accountsCollection.InsertOneAsync(account);
        }

        public async Task UpdateItemAsync(string Username, string Password, Account account)
        {
            var filter = filterBuilder.Eq(existingItem => existingItem.Username, Username) & filterBuilder.Eq(existingItem => existingItem.Password, Password);
            await accountsCollection.ReplaceOneAsync(filter, account);
        }

        public async Task DeleteItemAsync(string username, string password)
        {
            var filter = filterBuilder.Eq(item => item.Username, username) & filterBuilder.Eq(item => item.Password, password);
            await accountsCollection.DeleteOneAsync(filter);
        }

        public async Task<IEnumerable<Account>> GetItemsAsync()
        {
            return await accountsCollection.Find(new BsonDocument()).ToListAsync();
        }

        public async Task<AccountDto> GetItemAsyncNoPass(string Username)
        {
            var filter = filterBuilder.Eq(item => item.Username, Username);
            var Currentaccount = await accountsCollection.Find(filter).SingleOrDefaultAsync();
            return Currentaccount.AsDto();
        }
    }
}