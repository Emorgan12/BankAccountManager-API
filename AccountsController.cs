using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Amazon.SecurityToken.Model;
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
            var accounts = await repository.GetItemAsync(Username, Password);
        
            logger.LogInformation($"{DateTime.UtcNow:hh:mm:ss}: Hello {Username}! You have successfully logged in! Your account balance is {accounts.Balance}!");
            return accounts.AsDto();
        }
    
        [HttpPost]
        public async Task<ActionResult<AccountDto>> CreateAccountAsync(CreateAccountDto accountDto)
        {

            if(GetItemsAsync().Result.Any(account => account.Username == accountDto.Username))
            {
                return BadRequest("Username already exists");
            }

            if(!validation.ContainsNumber(accountDto.Password))
                return BadRequest("Password must contain at least one number");
            Account account = new()
            {
                Id = Guid.NewGuid(),
                Username = accountDto.Username,
                Password = accountDto.Password,
                CreatedDate = DateTimeOffset.UtcNow
            };
    
            await repository.CreateItemAsync(account);
    
            return CreatedAtAction(nameof(repository.GetItemsAsync), new { id = account.Id }, account.AsDto());
        }

        [HttpDelete("{Username}, {Password}/delete")]
        public async Task<ActionResult> DeleteItemAsync(string Username, string Password)
        {
            var existingAccount = await repository.GetItemAsync(Username, Password);
            if (existingAccount is null)
            {
                logger.LogInformation($"{DateTime.UtcNow:hh:mm:ss}: Account with username {Username} not found");
                return NotFound();
            }

            logger.LogInformation($"{DateTime.UtcNow:hh:mm:ss}: Deleting account with username {Username}");
            await repository.DeleteItemAsync(Username, Password);

            return NoContent();
        }

        [HttpGet]
        public async Task<IEnumerable<Account>> GetItemsAsync()
        {
            IEnumerable<Account> accounts = (await repository.GetItemsAsync())
                                                .Select(account => account);
            return accounts;
        }

        [HttpPut("{Username}, {Password}, {DepositAmount}/deposit")]
        public async Task<ActionResult<AccountDto>> DepositMoney(string Username, string Password, int DepositAmount)
        {
            var existingAccount = await repository.GetItemAsync(Username, Password);

            if (existingAccount is null)
            {
                return NotFound();
            }

            if (DepositAmount < 0)
            {
                return BadRequest("Deposit amount must be greater than 0");
            }

            Account updatedAccount = new Account
            {
                Id = existingAccount.Id,
                Username = existingAccount.Username,
                Password = existingAccount.Password,
                CreatedDate = existingAccount.CreatedDate,
                Balance = existingAccount.Balance + DepositAmount
            };

            await repository.UpdateItemAsync(Username, Password, updatedAccount);

            return updatedAccount.AsDto();
        }

        [HttpPut("{Username}, {Password}, {WidthdrawAmount}/widthdraw")]
        public async Task<ActionResult<AccountDto>> WidthdrawMoney(string Username, string Password, int WidthdrawAmount)
        {
            var existingAccount = await repository.GetItemAsync(Username, Password);

            if (existingAccount is null)
            {
                return NotFound();
            }

            if (WidthdrawAmount < 0)
            {
                return BadRequest("Widthdraw amount must be greater than 0");
            }

            if (existingAccount.Balance < WidthdrawAmount)
            {
                return BadRequest("Insufficient funds");
            }

            Account updatedAccount = new Account
            {
                Id = existingAccount.Id,
                Username = existingAccount.Username,
                Password = existingAccount.Password,
                CreatedDate = existingAccount.CreatedDate,
                Balance = existingAccount.Balance - WidthdrawAmount
            };

            await repository.UpdateItemAsync(Username, Password, updatedAccount);

            return updatedAccount.AsDto();
        }

        [HttpPut("{Username}, {Password}, {TransferAmount}, {RecipientUsername}/transfer")]
        
        public async Task<ActionResult<AccountDto>> TransferMoney(string Username, string Password, int TransferAmount, string RecipientUsername)
        {
            var existingAccount = await repository.GetItemAsync(Username, Password);
            var recipientAccount = await repository.GetItemAsyncNoPass(RecipientUsername);

            if (existingAccount is null)
            {
                return NotFound();
            }

            if (recipientAccount is null)
            {
                return NotFound();
            }

            if (TransferAmount < 0)
            {
                return BadRequest("Transfer amount must be greater than 0");
            }

            if (existingAccount.Balance < TransferAmount)
            {
                return BadRequest("Insufficient funds");
            }

            Account updatedAccount = new Account
            {
                Id = existingAccount.Id,
                Username = existingAccount.Username,
                Password = existingAccount.Password,
                CreatedDate = existingAccount.CreatedDate,
                Balance = existingAccount.Balance - TransferAmount
            };

            Account updatedRecipientAccount = new Account
            {
                Id = recipientAccount.Id,
                Username = recipientAccount.Username,
                Password = recipientAccount.Password,
                CreatedDate = recipientAccount.CreatedDate,
                Balance = recipientAccount.Balance + TransferAmount
            };

            await repository.UpdateItemAsync(Username, Password, updatedAccount);
            await repository.UpdateItemAsync(RecipientUsername, recipientAccount.Password, updatedRecipientAccount);

            logger.LogInformation($"{DateTime.UtcNow:hh:mm:ss}: {TransferAmount} has been transferred from {Username} to {RecipientUsername}");
            logger.LogInformation($"{DateTime.UtcNow:hh:mm:ss}: {Username} has a balance of {updatedAccount.Balance}");
            logger.LogInformation($"{DateTime.UtcNow:hh:mm:ss}: {RecipientUsername} has a balance of {updatedRecipientAccount.Balance}");
                 
            return updatedAccount.AsDto();
        }

        [HttpPut("{Username}, {Password}, {NewPassword}/changepassword")]
        public async Task<ActionResult<AccountDto>> ChangePassword(string Username, string Password, string NewPassword)
        {
            var existingAccount = await repository.GetItemAsync(Username, Password);

            if (existingAccount is null)
            {
                return NotFound();
            }

            Account updatedAccount = new Account
            {
                Id = existingAccount.Id,
                Username = existingAccount.Username,
                Password = NewPassword,
                CreatedDate = existingAccount.CreatedDate,
                Balance = existingAccount.Balance
            };

            await repository.UpdateItemAsync(Username, Password, updatedAccount);

            return updatedAccount.AsDto();
        }

    }
}