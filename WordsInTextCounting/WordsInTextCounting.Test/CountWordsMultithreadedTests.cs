using Xunit;
using WordsInTextCounting.TextProcessing;

namespace WordsInTextCounting.Tests
{
    public class CountWordsMultithreadedTests
    {
        [Fact]
        public void CountWords_Null_ReturnException()
        {
            Assert.Throws<ArgumentNullException>(() => TextProcessor.CountWords(null, true));
        }

        [Fact]
        public void CountWords_EmptyString_Return0()
        {
            string text = String.Empty;
            int expexted = 0;

            int actual = TextProcessor.CountWords(text, true);

            Assert.Equal(expexted, actual);
        }

        [Fact]
        public void CountWords_SimpleSentence_Return3()
        {
            string text = "Самое простое предложение";
            int expexted = 3;

            int actual = TextProcessor.CountWords(text, true);

            Assert.Equal(expexted, actual);
        }

        [Fact]
        public void CountWords_SimpleSentenceWithSpaceEnd_Return3()
        {
            string text = "Самое простое предложение ";
            int expexted = 3;

            int actual = TextProcessor.CountWords(text, true);

            Assert.Equal(expexted, actual);
        }

        [Fact]
        public void CountWords_SentenceWithPunctuation_Return19()
        {
            string text = "Самое простое предложение, в котором используются различные знаки препинания: " +
                "(круглые скобки) () ( ), {фигурные скобки} {} { }, <знаки неравенства> <> < > " +
                "и многое другое ? ! @ \" # $ % ^ & * - + = [] [ ] [конец].";
            int expexted = 19;

            int actual = TextProcessor.CountWords(text, true);

            Assert.Equal(expexted, actual);
        }

        [Fact]
        public void CountWords_SentenceWithNumbers_Return14()
        {
            string text = "Цифры стоящие отдельно не считаются за слово 123 " +
                "а цифры относящиеся к слову считаются 2-ой";
            int expexted = 14;

            int actual = TextProcessor.CountWords(text, true);

            Assert.Equal(expexted, actual);
        }

        [Fact]
        public void CountWords_MixedSentence_Return14()
        {
            string text = "В этом предложении комбинации цифр, знаков препинания и букв: " +
                "'одно_слово' <7-й> тест из 7 (1 финальный 1)!";
            int expexted = 14;

            int actual = TextProcessor.CountWords(text, true);

            Assert.Equal(expexted, actual);
        }
    }
}
