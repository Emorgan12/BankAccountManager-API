namespace BankAccountManager
{
    public class Account{
        public Guid Id {get; set;}
        public string Username {get; set; }
        public string Password {get; set; }
        public int Balance {get; set; }
        public DateTimeOffset DateCreated {get; set; }
    }
}