using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Bot_Application1.New_Classes
{
    public class MoviesSorter
    {
        private static HashTable moviesHashTable = MoviesAdderIntoHashArray.getMoviesHashTable();

        public static void sortMoviesHashTableAlphabetically()
        {
            foreach (List<Movie> list in moviesHashTable.getHashArray())
            {
                quickSort(list, 0, list.Count - 1);
            }
        }

        private static void quickSort(List<Movie> list, int startIndex, int endIndex)
        {
            if (startIndex < endIndex)
            {
                int pIndex = partition(list, startIndex, endIndex);
                quickSort(list, startIndex, pIndex - 1);
                quickSort(list, pIndex + 1, endIndex);
            }
        }

        private static int partition(List<Movie> list, int startIndex, int endIndex)
        {
            string pivot = list[list.Count - 1].getTitle();
            int pIndex = startIndex;

            for (int i = 0; i < endIndex; i++)
            {

                for (int j = 0; ; j++)
                {
                    int charToMatch = (int)list[i].getTitle()[j];
                    int pivotToMatch = (int)pivot[j];
                    if (charToMatch < pivotToMatch)
                    {
                        swap(list[i], list[pIndex]);
                        pIndex++;
                        break;
                    }
                    else if (charToMatch > pivotToMatch)
                        break;
                }
            }
            swap(list[pIndex], list[endIndex]);
            return pIndex;

        }

        private static void swap(Movie movie1, Movie movie2)
        {
            Movie temp = movie1;
            movie1 = movie2;
            movie2 = temp;

        }
    }
}