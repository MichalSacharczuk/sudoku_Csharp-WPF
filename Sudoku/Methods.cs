using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sudoku
{
    public static class Methods
    {
        public static int Factorial(int n)
        {
            if (n == 0 || n == 1)
                return 1;
            else
                return n * Factorial(n - 1);
        }

        public static List<List<string>> AllPermutationsOfTheList(List<string> list)
        {
            List<List<string>> allPermutationsOfTheList = new List<List<string>>();
            if (list.Count == 3)
            {
                for (int i = 0; i < 6; i++)
                {
                    allPermutationsOfTheList.Add(new List<string>());
                }
                for (int i = 0; i < 3; i++)
                {
                    allPermutationsOfTheList[0].Add(list[i]);
                    allPermutationsOfTheList[1].Add(list[list.Count - 1 - i]);
                    allPermutationsOfTheList[2].Add(list[(2 * i) % list.Count]);
                    allPermutationsOfTheList[3].Add(list[list.Count - 1 - ((2 * i) % list.Count)]);
                    allPermutationsOfTheList[4].Add(list[(i + 1) % list.Count]);
                    allPermutationsOfTheList[5].Add(list[(2 * i + 1) % list.Count]);
                }
            }
            else if (list.Count == 2)
            {
                for (int i = 0; i < 2; i++)
                {
                    allPermutationsOfTheList.Add(new List<string>());
                } 
                for (int i = 0; i < 2; i++)
                {
                    allPermutationsOfTheList[0].Add(list[i]);
                    allPermutationsOfTheList[1].Add(list[list.Count - 1 - i]);
                }
            }
            else if (list.Count == 1)
            {
                allPermutationsOfTheList.Add(new List<string>()); 
                allPermutationsOfTheList[0].Add(list[0]);
            }
            return allPermutationsOfTheList;
        }

        public static void RandomizeList(List<string> list)
        {
            List<string> tempElementList = new List<string>();
            Random rnd = new Random();
            int liscCount = list.Count;
            int randomIndexNr;
            while (list.Count > 0)
            {
                randomIndexNr = rnd.Next(0, list.Count);
                tempElementList.Add(list[randomIndexNr]);
                list.RemoveAt(randomIndexNr);
            }
            for (int i = 0; i < liscCount; i++)
            {
                list.Add(tempElementList[i]);
            }
        }

    }
}
