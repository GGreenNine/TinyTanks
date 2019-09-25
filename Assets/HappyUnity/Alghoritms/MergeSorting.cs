using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
 
namespace HappyUnity.Containers
{
    class Program
    {
        public static void mergeSort(int[] arrayList)
        {
            //уже отсортирован
            if (arrayList.Length < 2)
                return;
            //разбиение на две части
            //если размер массива нечетный, то вторая часть возьмет еще 1 дополнителньый элемент
            int[] firstPart = new int[arrayList.Length / 2];
            int[] secondPart = new int[arrayList.Length - firstPart.Length];
            for (int z = 0; z < firstPart.Length; ++z)
                firstPart[z] = arrayList[z];
            for (int z = 0; z < secondPart.Length; ++z)
                secondPart[z] = arrayList[z + firstPart.Length];
            //рекурсивная сортировка обоих из этих двух частей
            mergeSort(firstPart);
            mergeSort(secondPart);
 
            //слияние частей
            int i = 0;//индекс целого массива
            int j = 0;//индекс первой части
            int k = 0;//индекс второй части
            //добавление элементов к целому массиву пока количество эелментов какой то части не кончится
            while (j != firstPart.Length && k != secondPart.Length)
            {
                if (firstPart[j] < secondPart[k])
                {
                    arrayList[i] = firstPart[j];
                    ++j;
                }
                else
                {
                    arrayList[i] = secondPart[k];
                    ++k;
                }
                ++i;
            }
            //добавление оставшихся элементов
            while (j != firstPart.Length)
            {
                arrayList[i] = firstPart[j];
                ++j;
                 ++i;
            }
            while (k != secondPart.Length)
            {
                arrayList[i] = secondPart[k];
                ++k;
                ++i;
            }
        }
 
        static void Main(string[] args)
        {
            int[] array = { 5, 4, 3, 1, 4 };
            mergeSort(array);
            for (int i = 0; i < array.Length; ++i)
                Console.Write(array[i] + " ");
            Console.ReadKey();
        }
    }
}