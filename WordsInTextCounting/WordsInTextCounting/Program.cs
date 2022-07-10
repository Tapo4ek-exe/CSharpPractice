using WordsInTextCounting.TextProcessing;
using WordsInTextCounting.TextProvider;

class Program
{
    static void Main(string[] args)
    {
        ITextProvider[] textProviders = { new ConsoleTextProvider(), new FileTextProvider() };
        Console.WriteLine("ПОДСЧЕТ СЛОВ В ТЕКСТЕ");
        Console.WriteLine();
        
        // Ввод данных
        int inputMethod = -1;
        do
        {
            Console.WriteLine("Выберите способ ввода:");
            for (int i = 0; i < textProviders.Length; i++)
            {
                Console.WriteLine($"{i} - {textProviders[i]}");
            }

            string? input = Console.ReadLine();
            bool isConverted = int.TryParse(input, out inputMethod);

            // Проверка успеха конвертации
            if (!isConverted)
            {
                Console.WriteLine($"Ошибка: {input} не является числом!");
                inputMethod = -1;
                continue;
            }

            // Проверка на соответстиве доступному диапозону
            if (inputMethod < 0 || inputMethod > textProviders.Length - 1)
            {
                Console.WriteLine($"Ошибка: {input} не входит в доступный диапозон 0-{textProviders.Length - 1}!");
                inputMethod = -1;
            }
        }
        while (inputMethod == -1);
        string text = textProviders[inputMethod].GetText();

        int wordCount = TextProcessor.CountWords(text, true);

        // Вывод результата
        int outputMethod = -1;
        do
        {
            Console.WriteLine("Выберите способ вывода:");
            for (int i = 0; i < textProviders.Length; i++)
            {
                Console.WriteLine($"{i} - {textProviders[i]}");
            }

            string? input = Console.ReadLine();
            bool isConverted = int.TryParse(input, out outputMethod);

            // Проверка успеха конвертации
            if (!isConverted)
            {
                Console.WriteLine($"Ошибка: {input} не является числом!");
                outputMethod = -1;
                continue;
            }

            // Проверка на соответстиве доступному диапозону
            if (outputMethod < 0 || outputMethod > textProviders.Length - 1)
            {
                Console.WriteLine($"Ошибка: {input} не входит в доступный диапозон 0-{textProviders.Length - 1}!");
                outputMethod = -1;
            }
        }
        while (outputMethod == -1);
        textProviders[outputMethod].ExportText($"Кол-во слов: {wordCount}");

    }
}