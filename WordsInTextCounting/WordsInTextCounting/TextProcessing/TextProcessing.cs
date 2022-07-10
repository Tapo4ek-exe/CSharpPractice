namespace WordsInTextCounting.TextProcessing
{
    public static class TextProcessor
    {
        private static int totalWordCount = 0;
        private static int minLengthPerThread = 100;


        // Подсчет слов в строке
        public static int CountWords(string text, bool isMultithreaded = false)
        {
            // Проверка на null
            if (text == null)
            {
                throw new ArgumentNullException("text");
            }

            // Проверка на пустую строку
            if (text.Length == 0)
            {
                return 0;
            }

            return isMultithreaded ? CountWordsMultithreaded(text) : CountWordsSingleThreaded(text);
        }


        // Подсчет слов в строке в однопоточном режиме
        private static int CountWordsSingleThreaded(string text)
        {
            // Подготовка текста для подсчета
            string preparedText = new(text.Where((c) => (char.IsLetter(c) || char.IsWhiteSpace(c))).ToArray());
            preparedText = ReplaceWhiteSpaces(preparedText, " ");
            preparedText = preparedText.Replace("  ", " ");
            
            return preparedText.Split(" ").Length;
        }


        // Подсчет слов в строке с распараллеливанием
        private static int CountWordsMultithreaded(string text)
        {
            // Подготовка к подсчету
            string preparedText = new(text.Where((c) => (char.IsLetter(c) || char.IsWhiteSpace(c))).ToArray());
            if (!char.IsWhiteSpace(preparedText[^1]))
                preparedText += " ";
            totalWordCount = 0;

            // Создание оптимального кол-ва потоков
            int threadCount = preparedText.Length / minLengthPerThread;
            threadCount += threadCount == 0 ? 2 : 0;
            if (threadCount > Environment.ProcessorCount) threadCount = Environment.ProcessorCount;
            Thread[] threads = new Thread[threadCount];

            // Распределение задач по потокам
            int start = 0, length = preparedText.Length / threads.Length, end = length; // определяют подстроку: индекс начала, базовая длина
            for (int index = 0; index < threads.Length; index++)                        // и индекс конца
            {
                // Корректировка подстроки
                bool startCharIsWhiteSpace = char.IsWhiteSpace(preparedText[start]);    // подстрока начинается с white-space символа
                bool endCharIsWhiteSpace = char.IsWhiteSpace(preparedText[end - 1]);    // подстрока заканчивается  white-space символом
                while (startCharIsWhiteSpace || !endCharIsWhiteSpace)
                {
                    if (startCharIsWhiteSpace && start + 1 < preparedText.Length)       // сдвигаем индекс начала подстроки
                        start++;                                                        // если там находится white-space символ
                    else if (start + 1 >= preparedText.Length)
                        break;

                    if (!endCharIsWhiteSpace && end + 1 < preparedText.Length)          // сдвигаем индекс конца подстроки
                        end++;                                                          // если там не white-space символ
                    else if (end + 1 >= preparedText.Length)
                        break;

                    startCharIsWhiteSpace = char.IsWhiteSpace(preparedText[start]);
                    endCharIsWhiteSpace = char.IsWhiteSpace(preparedText[end - 1]);
                }

                // Запуск потока
                threads[index] = new Thread(new ParameterizedThreadStart(CountWordsThread));
                CountWordsThreadParameter parameter = new(preparedText, start, end);
                threads[index].Start(parameter);

                // Завершение цикла, при достижении конца строки
                if (end >= preparedText.Length)
                    break;

                // Расчет следующей подстроки
                start = end;
                end = (index + 1 < threads.Length - 1) && (end + length <= preparedText.Length) 
                    ? end + length : preparedText.Length;
            }

            // Ожидание заврешения работы потоков
            foreach (Thread thread in threads)
            {
                if (thread != null)
                    thread.Join();
            }

            return totalWordCount;
        }


        // Подсчет слов в потоке
        private static void CountWordsThread(object? obj)
        {
            if (obj is CountWordsThreadParameter parameter)
            {
                int wordCount = char.IsWhiteSpace(parameter.text[parameter.end - 1]) ? 0 : 1;
                for (int index = parameter.start; index < parameter.end; index++)
                {
                    if (char.IsLetter(parameter.text[index]) &&
                        index + 1 < parameter.end &&
                        char.IsWhiteSpace(parameter.text[index + 1]))
                    {
                        wordCount++;
                        index++;
                    }
                }
                Interlocked.Add(ref totalWordCount, wordCount);
            }
        }


        // Заменяет все white-space символы в строке на заданную строку
        private static string ReplaceWhiteSpaces(string text, string newValue)
        {
            char[] delims = new char[] { ' ', '\t', '\r', '\n' };
            return string.Join(newValue, text.Split(delims).Where(s => !string.IsNullOrWhiteSpace(s)));
        }
    }
}
