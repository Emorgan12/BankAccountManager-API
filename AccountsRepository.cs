using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BankAccountManager
{
    public interface AccountsRepository
    {
        Task<Account> GetItemAsync(string username, string password);
        Task<IEnumerable<Account>> GetItemsAsync();
        Task CreateItemAsync(Account account);
        Task UpdateItemAsync(string Username, string Password, Account account);
        Task DeleteItemAsync(string username, string password);
        Task<AccountDto> GetItemAsyncNoPass(string recipientUsername);
    }
}