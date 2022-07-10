using Xunit;
using WordsInTextCounting.TextProcessing;

namespace WordsInTextCounting.Tests
{
    public class CountWordsSingleThreadedTests
    {
        [Fact]
        public void CountWords_Null_ReturnException()
        {
            Assert.Throws<ArgumentNullException>(() => TextProcessor.CountWords(null));
        }

        [Fact]
        public void CountWords_EmptyString_Return0()
        {
            string text = String.Empty;
            int expexted = 0;

            int actual = TextProcessor.CountWords(text);

            Assert.Equal(expexted, actual);
        }

        [Fact]
        public void CountWords_SimpleSentence_Return3()
        {
            string text = "����� ������� �����������";
            int expexted = 3;

            int actual = TextProcessor.CountWords(text);

            Assert.Equal(expexted, actual);
        }

        [Fact]
        public void CountWords_SimpleSentenceWithSpaceEnd_Return3()
        {
            string text = "����� ������� ����������� ";
            int expexted = 3;

            int actual = TextProcessor.CountWords(text);

            Assert.Equal(expexted, actual);
        }

        [Fact]
        public void CountWords_SentenceWithPunctuation_Return19()
        {
            string text = "����� ������� �����������, � ������� ������������ ��������� ����� ����������: " +
                "(������� ������) () ( ), {�������� ������} {} { }, <����� �����������> <> < > " +
                "� ������ ������ ? ! @ \" # $ % ^ & * - + = [] [ ] [�����].";
            int expexted = 19;

            int actual = TextProcessor.CountWords(text);

            Assert.Equal(expexted, actual);
        }

        [Fact]
        public void CountWords_SentenceWithNumbers_Return14()
        {
            string text = "����� ������� �������� �� ��������� �� ����� 123 " +
                "� ����� ����������� � ����� ��������� 2-��";
            int expexted = 14;

            int actual = TextProcessor.CountWords(text);

            Assert.Equal(expexted, actual);
        }

        [Fact]
        public void CountWords_MixedSentence_Return14()
        {
            string text = "� ���� ����������� ���������� ����, ������ ���������� � ����: " +
                "'����_�����' <7-�> ���� �� 7 (1 ��������� 1)!";
            int expexted = 14;

            int actual = TextProcessor.CountWords(text);

            Assert.Equal(expexted, actual);
        }
    }
}