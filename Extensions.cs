namespace BankAccountManager
{
    public static class Extensions
    {
        public static AccountDto AsDto(this Account account)
        {
            return new AccountDto(account.Id, account.Username, account.Password, account.Balance, account.CreatedDate);
        }
    }
}