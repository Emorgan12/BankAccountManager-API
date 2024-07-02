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

        [HttpGet ("{Username}, {Password}")]
        public async Task<AccountDto> Login(string Username = null, string Password = null)
        {
            var accounts = await repository.LoginAsync(Username, Password);
    
            logger.LogInformation($"{DateTime.UtcNow:hh:mm:ss}: Hello {Username}! You have successfully logged in! Your account balance is {accounts.balance}!");
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
    
            return CreatedAtAction(nameof(repository.GetItemsAsync), new { id = account.Id }, account.AsDto());
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteItemAsync(Guid id)
        {
            var existingAccount = await repository.GetItemAsync(id);

            if (existingAccount is null)
            {
                return NotFound();
            }

            logger.LogInformation($"{DateTime.UtcNow:hh:mm:ss}: Deleting account with username {existingAccount.Username} and id {id}");
            await repository.DeleteItemAsync(id);


            return NoContent();
        }

        [HttpGet]
        public async Task<IEnumerable<AccountDto>> GetItemsAsync()
        {
            var accounts = (await repository.GetItemsAsync())
                            .Select(account => account.AsDto());
            return accounts;
        }

    }
}