namespace PracticeClassLibrary
{
    public static class CustomMath
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