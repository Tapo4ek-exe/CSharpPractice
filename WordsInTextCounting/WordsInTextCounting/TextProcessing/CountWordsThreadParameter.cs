

namespace WordsInTextCounting.TextProcessing
{
    public class CountWordsThreadParameter
    {
        public string text { get; set; }
        public int start { get; set; }
        public int end { get; set; }

        public CountWordsThreadParameter(string text, int start, int end)
        {
            this.text = text;
            this.start = start;
            this.end = end;
        }
    }
}
