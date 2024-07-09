using System;
using System.ComponentModel.DataAnnotations;

namespace BankAccountManager
{
    public record AccountDto(Guid Id, [Required] string Username, [Required] string Password, int Balance, DateTimeOffset CreatedDate);
    public record CreateAccountDto([Required][Length(3,20)] string Username,[Required][Length(8,20)] string Password);
    public record UpdateAccountDto([Required] string Username, [Required][Length(8,20)] string Password);
}