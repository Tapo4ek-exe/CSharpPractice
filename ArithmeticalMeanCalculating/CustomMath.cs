using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArithmeticalMeanCalculating
{
    static class CustomMath
    {
        // Функция для подсчета среднего арифметического чисел
        public static double Average(params double[] numbers)
        {
            // Подсчет суммы чисел
            double sum = 0;
            foreach (double number in numbers)
            {
                sum += number;
            }

            return sum / numbers.Length;
        }
    }
}
