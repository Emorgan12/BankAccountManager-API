using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BankAccountManager
{
    public interface AccountsRepository
    {
        Task<Account> GetItemAsync(Guid id);
        Task<IEnumerable<Account>> GetItemsAsync();
        Task CreateItemAsync(Account account);
        Task UpdateItemAsync(Account account);
        Task DeleteItemAsync(Guid id);
        Task<AccountDto> LoginAsync(string username, string password);
    }
}