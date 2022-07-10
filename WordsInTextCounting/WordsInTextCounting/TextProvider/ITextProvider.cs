using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WordsInTextCounting.TextProvider
{
    interface ITextProvider
    {
        // Метод для получения данных
        string GetText();


        // Метод для вывода (экспорта) данных
        void ExportText(string text);
    }
}
