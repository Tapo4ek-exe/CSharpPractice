namespace WordsInTextCounting.TextProvider
{
    internal class ConsoleTextProvider : ITextProvider
    {
        public void ExportText(string text)
        {
            Console.WriteLine(text);
        }


        public string GetText()
        {
            Console.WriteLine("Введите текст:");
            string? text = Console.ReadLine();
            return text ?? string.Empty;
        }


        public override string ToString()
        {
            return "Консоль";
        }
    }
}
