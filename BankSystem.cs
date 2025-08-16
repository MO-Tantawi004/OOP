using System;

namespace BankSystem
{
    abstract class BankAccount
    {
        public string AccountNumber { get; private set; }
        public decimal Balamce { get; protected set; }
        public DateTime DateOpened { get; private set; }

        protected BankAccount(string accountNumber)
        {
            AccountNumber = accountNumber;
            Balamce = 0;
            DateOpened = DateTime.Now;
        }

        public virtual void Deposit(decimal amount)
        {
            if (amount <= 0) return;
            Balamce += amount;
        }

        public abstract void Withdraw(decimal amount);

        public override string ToString()
        {
            return $"{AccountNumber} | Balamce: {Balamce} | Opened: {DateOpened:d}";
        }
    }

    class SavingsAccount : BankAccount
    {
        public decimal InterestRate { get; private set; }

        public SavingsAccount(string accNum, decimal interestRate) : base(accNum)
        {
            InterestRate = interestRate;
        }

        public override void Withdraw(decimal amount)
        {
            if (amount > Balamce) return;
            Balamce -= amount;
        }

        public decimal MonthlyInterest()
        {
            return (Balamce * InterestRate) / 12;
        }

        public override string ToString()
        {
            return base.ToString() + $" | Type: Savings | Interest: {InterestRate * 100}%";
        }
    }

    class CurrentAccount : BankAccount
    {
        public decimal OverdraftLimit { get; private set; }

        public CurrentAccount(string accNum, decimal overdraft) : base(accNum)
        {
            OverdraftLimit = overdraft;
        }

        public override void Withdraw(decimal amount)
        {
            if (Balamce - amount < -OverdraftLimit) return;
            Balamce -= amount;
        }

        public override string ToString()
        {
            return base.ToString() + $" | Type: Current | Overdraft: {OverdraftLimit}";
        }
    }

    class Customer
    {
        private static int CustCounter = 1000;
        public int CustomerID { get; private set; }
        public string FullName { get; private set; }
        public string NationalID { get; private set; }
        public DateTime DOB { get; private set; }

        private BankAccount[] accounts = new BankAccount[5];
        private int accCount = 0;

        public Customer(string name, string nid, DateTime dob)
        {
            CustomerID = ++CustCounter;
            FullName = name;
            NationalID = nid;
            DOB = dob;
        }

        public void Update(string newName, DateTime newDob)
        {
            FullName = newName;
            DOB = newDob;
        }

        public void AddAccount(BankAccount acc)
        {
            if (accCount < accounts.Length)
                accounts[accCount++] = acc;
        }

        public decimal TotalBalamce()
        {
            decimal total = 0;
            for (int i = 0; i < accCount; i++)
                total += accounts[i].Balamce;
            return total;
        }

        public BankAccount? GetAccount(string accNum)
        {
            for (int i = 0; i < accCount; i++)
                if (accounts[i].AccountNumber == accNum)
                    return accounts[i];
            return null;
        }

        public void PrintAccounts()
        {
            for (int i = 0; i < accCount; i++)
                Console.WriteLine("   - " + accounts[i]);
        }

        public override string ToString()
        {
            return $"[{CustomerID}] {FullName} | NID: {NationalID} | DOB: {DOB:d} | Accounts: {accCount} | Total Balamce: {TotalBalamce()}";
        }
    }

    class Bank
    {
        public string Name { get; private set; }
        public string BranchCode { get; private set; }

        private Customer[] customers = new Customer[20];
        private int custCount = 0;
        private int accSeq = 0;

        public Bank(string name, string branch)
        {
            Name = name;
            BranchCode = branch;
        }

        private string NextAccNumber()
        {
            accSeq++;
            return $"{BranchCode}-{accSeq:0000}";
        }

        public Customer AddCustomer(string name, string nid, DateTime dob)
        {
            if (custCount >= customers.Length)
                throw new Exception("Customer limit reached");
            var c = new Customer(name, nid, dob);
            customers[custCount++] = c;
            return c;
        }

        public Customer? FindCustomer(int id)
        {
            for (int i = 0; i < custCount; i++)
                if (customers[i].CustomerID == id) return customers[i];
            return null;
        }

        public void RemoveCustomer(int id)
        {
            for (int i = 0; i < custCount; i++)
            {
                if (customers[i].CustomerID == id)
                {
                    if (customers[i].TotalBalamce() != 0) return;
                    for (int j = i; j < custCount - 1; j++)
                        customers[j] = customers[j + 1];
                    custCount--;
                    return;
                }
            }
        }

        public SavingsAccount OpenSavings(int custId, decimal rate)
        {
            var c = FindCustomer(custId);
            if (c == null) throw new Exception("Customer not found");
            var acc = new SavingsAccount(NextAccNumber(), rate);
            c.AddAccount(acc);
            return acc;
        }

        public CurrentAccount OpenCurrent(int custId, decimal overdraft)
        {
            var c = FindCustomer(custId);
            if (c == null) throw new Exception("Customer not found");
            var acc = new CurrentAccount(NextAccNumber(), overdraft);
            c.AddAccount(acc);
            return acc;
        }

        public BankAccount? GetAccount(string accNum)
        {
            for (int i = 0; i < custCount; i++)
            {
                var acc = customers[i].GetAccount(accNum);
                if (acc != null) return acc;
            }
            return null;
        }

        public void Transfer(string fromAcc, string toAcc, decimal amount)
        {
            var from = GetAccount(fromAcc);
            var to = GetAccount(toAcc);
            if (from == null || to == null) return;
            from.Withdraw(amount);
            to.Deposit(amount);
        }

        public void PrintReport()
        {
            Console.WriteLine($"\n=== Bank Report: {Name} - Branch {BranchCode} ===");
            decimal total = 0;
            for (int i = 0; i < custCount; i++)
            {
                Console.WriteLine(customers[i]);
                customers[i].PrintAccounts();
                total += customers[i].TotalBalamce();
            }
            Console.WriteLine($"Total Bank Balamce: {total}");
        }
    }
}