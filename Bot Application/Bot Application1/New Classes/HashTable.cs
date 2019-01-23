using System;
using Microsoft.Office.Interop.Excel;
using Excel = Microsoft.Office.Interop.Excel;       
                                                    
using System.Runtime.InteropServices;
using System.Collections.Generic;

namespace Bot_Application1
{
    class HashTable
    {
        private int hashArrayLength;
        private List<Movie>[] hashArray;

        public HashTable(int arrayLength)
        {
            this.hashArrayLength = arrayLength;
            hashArray = new List<Movie>[hashArrayLength];
        }

        public void addToList(Movie movie, int index)
        {
            if (hashArray[index] == null)
                hashArray[index] = new List<Movie>();

            hashArray[index].Add(movie);
        }

        public List<Movie>[] getHashArray()
        {
            return this.hashArray;
        }

        public List<Movie> getSpecificListFromHashArray(int index)
        {
            if (index >= 0 && index < hashArrayLength)
                return hashArray[index];

            else
                return null;
        }

        public int getArrayLength()
        {
            return this.hashArrayLength;
        }

    }
}


