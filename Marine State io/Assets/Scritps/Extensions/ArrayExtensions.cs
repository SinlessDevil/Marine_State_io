using System;

namespace Extensions
{
    public static class ArrayExtensions
    {
        private static Random random = new Random();

        public static void Shuffle<T>(this T[] array)
        {
            int n = array.Length;
            while (n > 1)
            {
                n--;
                int k = random.Next(n + 1);
                T temp = array[k];
                array[k] = array[n];
                array[n] = temp;
            }
        }
    }

}