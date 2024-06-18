using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace BankAccountManager{
    [ApiController]
    [Route("accounts")]
    public class AccountsController : ControllerBase
    {
        private readonly AccountsRepository repository;
        private readonly ILogger<AccountsController> logger;

        public AccountsController(AccountsRepository repository, ILogger<AccountsController> logger)
        {
            this.repository = repository;
            this.logger = logger;
        }

        [HttpGet]
        public async Task<IEnumerable<AccountDto>> GetItemsAsync(string Username = "")
        {
            var accounts = (await repository.GetItemsAsync())
                        .Select(account => account.AsDto());
    
    

            if (!string.IsNullOrWhiteSpace(Username))
            {
                accounts = accounts.Where(account => account.Username.Contains(Username, StringComparison.OrdinalIgnoreCase));
            }
    
            logger.LogInformation($"{DateTime.UtcNow.ToString("hh:mm:ss")}: Retrieved {accounts.Count()} accounts");
            return accounts;
        }
    
        [HttpPost]
        public async Task<ActionResult<AccountDto>> CreateAccountAsync(CreateAccountDto accountDto)
        {
            Account account = new()
            {
                Id = Guid.NewGuid(),
                Username = accountDto.Username,
                Password = accountDto.Password,
                DateCreated = DateTimeOffset.UtcNow
            };
    
            await repository.CreateItemAsync(account);
    
            return CreatedAtAction(nameof(GetItemsAsync), new { id = account.Id }, account.AsDto());
        }
    }
}