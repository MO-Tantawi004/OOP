using System;

namespace BankSystem
{
    class Program
    {
        static void Main()
        {
            Bank bank = new Bank("National Bank", "B01");

            while (true)
            {
                Console.WriteLine("\n--- Bank Menu ---");
                Console.WriteLine("1. Add Customer");
                Console.WriteLine("2. Open Savings Account");
                Console.WriteLine("3. Open Current Account");
                Console.WriteLine("4. Deposit");
                Console.WriteLine("5. Withdraw");
                Console.WriteLine("6. Transfer");
                Console.WriteLine("7. Bank Report");
                Console.WriteLine("0. Exit");
                Console.Write("Choose: ");
                var choice = Console.ReadLine();

                try
                {
                    switch (choice)
                    {
                        case "1":
                            Console.Write("Name: ");
                            string name = Console.ReadLine();
                            Console.Write("National ID: ");
                            string nid = Console.ReadLine();
                            Console.Write("DOB (yyyy-mm-dd): ");
                            DateTime dob = DateTime.Parse(Console.ReadLine());
                            var c = bank.AddCustomer(name, nid, dob);
                            Console.WriteLine($"Customer added: {c}");
                            break;

                        case "2":
                            Console.Write("Customer ID: ");
                            int cust1 = int.Parse(Console.ReadLine());
                            Console.Write("Interest Rate (e.g. 0.05): ");
                            decimal rate = decimal.Parse(Console.ReadLine());
                            var sAcc = bank.OpenSavings(cust1, rate);
                            Console.WriteLine($"Savings Account created: {sAcc}");
                            break;

                        case "3":
                            Console.Write("Customer ID: ");
                            int cust2 = int.Parse(Console.ReadLine());
                            Console.Write("Overdraft Limit: ");
                            decimal od = decimal.Parse(Console.ReadLine());
                            var cAcc = bank.OpenCurrent(cust2, od);
                            Console.WriteLine($"Current Account created: {cAcc}");
                            break;

                        case "4":
                            Console.Write("Account Number: ");
                            string accNumD = Console.ReadLine();
                            Console.Write("Amount: ");
                            decimal amtD = decimal.Parse(Console.ReadLine());
                            var accD = bank.GetAccount(accNumD);
                            accD?.Deposit(amtD);
                            Console.WriteLine("Deposit done.");
                            break;

                        case "5":
                            Console.Write("Account Number: ");
                            string accNumW = Console.ReadLine();
                            Console.Write("Amount: ");
                            decimal amtW = decimal.Parse(Console.ReadLine());
                            var accW = bank.GetAccount(accNumW);
                            accW?.Withdraw(amtW);
                            Console.WriteLine("Withdraw done.");
                            break;

                        case "6":
                            Console.Write("From Account: ");
                            string from = Console.ReadLine();
                            Console.Write("To Account: ");
                            string to = Console.ReadLine();
                            Console.Write("Amount: ");
                            decimal amtT = decimal.Parse(Console.ReadLine());
                            bank.Transfer(from, to, amtT);
                            Console.WriteLine("Transfer done.");
                            break;

                        case "7":
                            bank.PrintReport();
                            break;

                        case "0":
                            return;
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error: " + ex.Message);
                }
            }
        }
    }
}