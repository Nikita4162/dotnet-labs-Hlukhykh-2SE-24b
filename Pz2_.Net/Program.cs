using System;
using System.Collections.Generic;

namespace Pz2_.Net
{
    public class InvalidEmployeeDataException : Exception
    {
        public InvalidEmployeeDataException() { }
        public InvalidEmployeeDataException(string message) : base(message) { }
        public InvalidEmployeeDataException(string message, Exception inner) : base(message, inner) { }
    }

    public abstract class Employee : IComparable<Employee>
    {
        private string name = string.Empty;
        private decimal baseSalary;

        public string Name
        {
            get => name;
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new InvalidEmployeeDataException("Ім'я співробітника не може бути порожнім!");
                name = value;
            }
        }

        public decimal BaseSalary
        {
            get => baseSalary;
            set
            {
                if (value < 0)
                    throw new InvalidEmployeeDataException("Базова зарплата не може бути від'ємною!");
                baseSalary = value;
            }
        }

        public Employee(string name, decimal baseSalary)
        {
            Name = name;
            BaseSalary = baseSalary;
        }

        public abstract decimal CalculateSalary();

        public virtual void PrintInfo()
        {
            Console.Write($"[{this.GetType().Name}] {Name} | Базова ставка: {BaseSalary:C} | До сплати: {CalculateSalary():C}");
        }

        public int CompareTo(Employee? other)
        {
            if (other == null) return 1;
            return this.CalculateSalary().CompareTo(other.CalculateSalary());
        }
    }

    public class Developer : Employee
    {
        private int linesOfCode;

        public int LinesOfCode
        {
            get => linesOfCode;
            set
            {
                if (value < 0)
                    throw new InvalidEmployeeDataException("Кількість рядків коду не може бути від'ємною!");
                linesOfCode = value;
            }
        }

        public Developer(string name, decimal baseSalary, int linesOfCode) : base(name, baseSalary)
        {
            LinesOfCode = linesOfCode;
        }

        public override decimal CalculateSalary()
        {
            return BaseSalary + (LinesOfCode * 0.5m);
        }

        public override void PrintInfo()
        {
            base.PrintInfo();
            Console.WriteLine($" | Рядків коду: {LinesOfCode}");
        }
    }

    public class Manager : Employee
    {
        private decimal bonus;

        public decimal Bonus
        {
            get => bonus;
            set
            {
                if (value < 0)
                    throw new InvalidEmployeeDataException("Бонус не може бути від'ємним!");
                bonus = value;
            }
        }

        public Manager(string name, decimal baseSalary, decimal bonus) : base(name, baseSalary)
        {
            Bonus = bonus;
        }

        public override decimal CalculateSalary()
        {
            return BaseSalary + Bonus;
        }

        public override void PrintInfo()
        {
            base.PrintInfo();
            Console.WriteLine($" | Бонус: {Bonus:C}");
        }
    }

    class Program
    {
        static void Main()
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;
            Console.InputEncoding = System.Text.Encoding.UTF8;

            List<Employee> employees = new List<Employee>
            {
                new Developer("Олександр", 25000, 1500),
                new Manager("Марія", 30000, 5000),
                new Developer("Іван", 20000, 800)
            };

            bool isRunning = true;
            while (isRunning)
            {
                Console.WriteLine("\n--- Управління співробітниками ---");
                Console.WriteLine("1. Показати всіх співробітників");
                Console.WriteLine("2. Відсортувати за зарплатою (зростання)");
                Console.WriteLine("3. Додати розробника");
                Console.WriteLine("4. Додати менеджера");
                Console.WriteLine("0. Вийти");
                Console.Write("Ваш вибір: ");

                string? choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        PrintEmployees(employees);
                        break;
                    case "2":
                        employees.Sort();
                        Console.WriteLine("Співробітників відсортовано за зарплатою.");
                        PrintEmployees(employees);
                        break;
                    case "3":
                        AddDeveloper(employees);
                        break;
                    case "4":
                        AddManager(employees);
                        break;
                    case "0":
                        isRunning = false;
                        break;
                    default:
                        Console.WriteLine("Некоректний вибір. Спробуйте ще раз.");
                        break;
                }
            }
        }

        static void PrintEmployees(List<Employee> employees)
        {
            Console.WriteLine("\nСписок співробітників:");
            foreach (var emp in employees)
            {
                emp.PrintInfo();
            }
        }

        static void AddDeveloper(List<Employee> employees)
        {
            try
            {
                Console.Write("Введіть ім'я: ");
                string name = Console.ReadLine() ?? "";

                Console.Write("Введіть базову ставку: ");
                if (!decimal.TryParse(Console.ReadLine(), out decimal salary))
                    throw new InvalidEmployeeDataException("Ставка має бути числом!");

                Console.Write("Введіть кількість написаних рядків коду: ");
                if (!int.TryParse(Console.ReadLine(), out int lines))
                    throw new InvalidEmployeeDataException("Кількість рядків має бути цілим числом!");

                Developer dev = new Developer(name, salary, lines);
                employees.Add(dev);
                Console.WriteLine("Розробника успішно додано!");
            }
            catch (InvalidEmployeeDataException ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"Помилка даних: {ex.Message}");
                Console.ResetColor();
            }
            catch (Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"Непередбачена помилка: {ex.Message}");
                Console.ResetColor();
            }
        }

        static void AddManager(List<Employee> employees)
        {
            try
            {
                Console.Write("Введіть ім'я: ");
                string name = Console.ReadLine() ?? "";

                Console.Write("Введіть базову ставку: ");
                if (!decimal.TryParse(Console.ReadLine(), out decimal salary))
                    throw new InvalidEmployeeDataException("Ставка має бути числом!");

                Console.Write("Введіть суму бонусу: ");
                if (!decimal.TryParse(Console.ReadLine(), out decimal bonus))
                    throw new InvalidEmployeeDataException("Бонус має бути числом!");

                Manager mgr = new Manager(name, salary, bonus);
                employees.Add(mgr);
                Console.WriteLine("Менеджера успішно додано!");
            }
            catch (InvalidEmployeeDataException ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"Помилка даних: {ex.Message}");
                Console.ResetColor();
            }
        }
    }
}