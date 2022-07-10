using System.Text;

namespace WordsInTextCounting.TextProvider
{
    class FileTextProvider : ITextProvider
    {
        const string DefaultFileName = "output.txt";

        public void ExportText(string text)
        {
            Console.WriteLine("Введите название (путь) файла:");
            string? filename = Console.ReadLine();
            File.WriteAllText(filename ?? DefaultFileName, text);
        }


        public string GetText()
        {
            bool fileRead = false;
            while (!fileRead)
            {
                Console.WriteLine("Введите путь к файлу:");
                string? path = Console.ReadLine();

                if (path == null) continue;
                fileRead = File.Exists(path);
                if (fileRead)
                {
                    return File.ReadAllText(path, Encoding.UTF8);
                }
                else
                {
                    Console.WriteLine("Файл не найден! Проверьте путь.");
                }
            }
            return string.Empty;
        }


        public override string ToString()
        {
            return "Файл";
        }
    }
}
