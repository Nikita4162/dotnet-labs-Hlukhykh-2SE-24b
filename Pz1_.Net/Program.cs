using System;

class Program
{
   
    private const double KmToMileCoef = 1.609344;
    private const double KgToPoundCoef = 0.45359237;
    private const double AbsoluteZeroCelsius = -273.15;
    private const double AbsoluteZeroFahrenheit = -459.67;

    static void Main()
    {
        
        Console.OutputEncoding = System.Text.Encoding.UTF8;
        Console.InputEncoding = System.Text.Encoding.UTF8;

        bool isRunning = true;
        while (isRunning)
        {
            Console.WriteLine("\nКонвертація величин");
            Console.WriteLine("1. км -> милі");
            Console.WriteLine("2. милі -> км");
            Console.WriteLine("3. кг -> фунти");
            Console.WriteLine("4. фунти -> кг");
            Console.WriteLine("5. °C -> °F");
            Console.WriteLine("6. °F -> °C");
            Console.WriteLine("0. Вийти");

            int choice = ReadInt("Ваш вибір: ", 0, 6);

            
            isRunning = choice switch
            {
                1 => HandleConversion("км", "милі", KmToMiles, x => x >= 0, "Відстань не може бути від'ємною!"),
                2 => HandleConversion("милі", "км", MilesToKm, x => x >= 0, "Відстань не може бути від'ємною!"),
                3 => HandleConversion("кг", "фунти", KgToPounds, x => x >= 0, "Маса не може бути від'ємною!"),
                4 => HandleConversion("фунти", "кг", PoundsToKg, x => x >= 0, "Маса не може бути від'ємною!"),
                5 => HandleConversion("°C", "°F", CelsiusToFahrenheit, x => x >= AbsoluteZeroCelsius, $"Температура не може бути нижчою за абсолютний нуль ({AbsoluteZeroCelsius} °C)!"),
                6 => HandleConversion("°F", "°C", FahrenheitToCelsius, x => x >= AbsoluteZeroFahrenheit, $"Температура не може бути нижчою за абсолютний нуль ({AbsoluteZeroFahrenheit} °F)!"),
                0 => false,
                _ => true
            };
        }

        Console.WriteLine("\nSuccess, easy peasy lemmon squezea beash bash bosh!");
    }

    static bool HandleConversion(string fromUnit, string toUnit, Func<double, double> conversionFormula, Predicate<double> validator, string errorMessage)
    {
        double input = ReadDouble($"Введіть значення в [{fromUnit}]: ");

        
        if (!validator(input))
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"Помилка: {errorMessage}");
            Console.ResetColor();
            return true;
        }

        double result = conversionFormula(input);
        
        
        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine($"Результат: {input} {fromUnit} = {result:F2} {toUnit}");
        Console.ResetColor();
        
        return true;
    }
    
    static int ReadInt(string prompt, int min, int max)
    {
        while (true)
        {
            Console.Write(prompt);
            string? input = Console.ReadLine();
            if (int.TryParse(input, out int value) && value >= min && value <= max)
            {
                return value;
            }
            Console.WriteLine($"Некоректний вибір. Введіть число від {min} до {max}.");
        }
    }
    static double ReadDouble(string prompt)
    {
        while (true)
        {
            Console.Write(prompt);
            string? input = Console.ReadLine();
            
            if (input != null && double.TryParse(input.Replace('.', ','), out double value))
            {
                return value;
            }
            Console.WriteLine("Некоректне введення. Будь ласка, введіть числове значення.");
        }
    }
    static double KmToMiles(double km) => km / KmToMileCoef;
    static double MilesToKm(double miles) => miles * KmToMileCoef;
    static double KgToPounds(double kg) => kg / KgToPoundCoef;
    static double PoundsToKg(double pounds) => pounds * KgToPoundCoef;
    static double CelsiusToFahrenheit(double c) => c * 9 / 5 + 32;
    static double FahrenheitToCelsius(double f) => (f - 32) * 5 / 9;
}
