namespace Practice
{
    class Program
    {
        static void Main(string[] args)
        {
            // Ввод данных пользователем
            Console.WriteLine("Введите число для извлечения корня: ");
            double number;
            string? input = Console.ReadLine();
            bool isConverted = double.TryParse(input, out number);

            // Проверка успеха конвертации
            if (!isConverted)
            {
                Console.WriteLine($"Ошибка: {input} не является числом!");
                return;
            }

            // Проверка числа на отрицательность
            if (number < 0)
            {
                Console.WriteLine("Ошибка: корень отрицательного числа вычислить невозможно!");
            }

            double result = Math.Sqrt(number);

            Console.WriteLine($"Результат: {result}");
        }
    }
}