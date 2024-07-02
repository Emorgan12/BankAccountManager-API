using System;
using System.ComponentModel.DataAnnotations;

namespace BankAccountManager
{
    public record AccountDto(Guid Id, [Required] string Username, [Required] string Password, int balance, DateTimeOffset CreatedDate);
    public record CreateAccountDto([Required] string Username, string Password);
    public record UpdateAccountDto([Required] string Username, string Password);
    public record LoginDto([Required] string Username, [Required] string Password);
}