using PracticeClassLibrary;

class Program
{
    static void Main(string[] args)
    {
        // Ввод данных пользователем
        Console.WriteLine("Введите 3 числа: ");
        double[] numbers = new double[3];
        for (int index = 0; index < numbers.Length; index++)
        {
            string? input = Console.ReadLine();
            bool isConverted = double.TryParse(input, out numbers[index]);
            
            // Проверка успеха конвертации
            if (!isConverted)
            {
                Console.WriteLine($"Ошибка: {input} не является числом!");
                return;
            }
        }

        double result = CustomMath.Average(numbers[0], numbers[1], numbers[2]);
        Console.WriteLine($"Среднее арифметическое: {result}");
    }
}