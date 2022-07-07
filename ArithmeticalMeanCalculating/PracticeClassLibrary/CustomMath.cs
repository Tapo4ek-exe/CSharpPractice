namespace PracticeClassLibrary
{
    public static class CustomMath
    {
        // Функция для подсчета среднего арифметического чисел
        public static double Average(in double number1, in double number2, params double[] otherNumbers)
        {
            // Подсчет суммы чисел
            double sum = number1 + number2;
            foreach (double number in otherNumbers)
            {
                sum += number;
            }

            return sum / (otherNumbers.Length + 2);
        }
    }
}