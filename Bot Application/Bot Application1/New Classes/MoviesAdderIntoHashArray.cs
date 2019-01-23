using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Office.Interop.Excel;
using Excel = Microsoft.Office.Interop.Excel;
using System.Runtime.InteropServices;


namespace Bot_Application1
{
    class MoviesAdderIntoHashArray
    {
        private static HashTable moviesHashTable = new HashTable(27);
        private static HashTable ratingsHashTable = new HashTable(10);

        public static List<Movie> getSpecificListFromMoviesHashTable(int index)
        {
            return moviesHashTable.getSpecificListFromHashArray(index);
        }

        public static List<Movie> getSpecificListFromRatingsHashTable(int index)
        {
            return ratingsHashTable.getSpecificListFromHashArray(index);
        }

       public static HashTable getMoviesHashTable()
        {
            return moviesHashTable;
        }


        public static void initializeMovieListWorkbooks()
        {
            Excel.Application excelApplication = new Excel.Application();

            Workbook workbook2010 =
                excelApplication.Workbooks.Open(@"F:\DSA Project Stuff\Lists of American Films\List Of American Films 2010.xlsx");
            getExcelFile(workbook2010, excelApplication);
            Workbook workbook2011 =
            excelApplication.Workbooks.Open(@"F:\DSA Project Stuff\Lists of American Films\List Of American Films 2011.xlsx");
            getExcelFile(workbook2011, excelApplication);
            Workbook workbook2012 =
                excelApplication.Workbooks.Open(@"F:\DSA Project Stuff\Lists of American Films\List Of American Films 2012.xlsx");
            getExcelFile(workbook2012, excelApplication);
            Workbook workbook2013 =
                excelApplication.Workbooks.Open(@"F:\DSA Project Stuff\Lists of American Films\List Of American Films 2013.xlsx");
            getExcelFile(workbook2013, excelApplication);
            //Workbook workbook2014 =
            //    excelApplication.Workbooks.Open(@"F:\DSA Project Stuff\Lists of American Films\List Of American Films 2014.xlsx");
            //getExcelFile(workbook2014, excelApplication);
            //Workbook workbook2015 =
            //    excelApplication.Workbooks.Open(@"F:\DSA Project Stuff\Lists of American Films\List Of American Films 2015.xlsx");
            //getExcelFile(workbook2015, excelApplication);
            //Workbook workbook2016 =
            //    excelApplication.Workbooks.Open(@"F:\DSA Project Stuff\Lists of American Films\List Of American Films 2016.xlsx");
            //getExcelFile(workbook2016, excelApplication);

        }

        private static void getExcelFile(Workbook workbook, Application excelApplication)
        {

            Excel.Worksheet xlWorksheet = workbook.Sheets[1];
            Excel.Range xlRange = xlWorksheet.UsedRange;

            int rowCount;
            int colCount;

            rowCount = xlWorksheet.UsedRange.Rows.Count;
            colCount = xlWorksheet.UsedRange.Columns.Count;

            xlWorksheet.Columns.ClearFormats();
            xlWorksheet.Rows.ClearFormats();

            rowCount = xlWorksheet.UsedRange.Rows.Count;
            colCount = xlWorksheet.UsedRange.Columns.Count;

            string[] movieInfoExtracted = new string[colCount];

            for (int i = 2; i <= rowCount; i++)
            {
                for (int j = 1, index = 0; j <= colCount; j++, index++)
                {
                    if (xlRange.Cells[i, j] != null && xlRange.Cells[i, j].Value != null)
                    {
                        movieInfoExtracted[index] = xlRange.Cells[i, j].Value.ToString();
                    }
                }
                insertMovieAlphabetically(movieInfoExtracted[0], movieInfoExtracted[1], movieInfoExtracted[2], movieInfoExtracted[3], movieInfoExtracted[4], movieInfoExtracted[5]);
                insertMovieRatingWise(movieInfoExtracted[0], movieInfoExtracted[1], movieInfoExtracted[2], movieInfoExtracted[3], movieInfoExtracted[4], movieInfoExtracted[5]);

            }
        }

        private static void insertMovieAlphabetically(string title, string director, string casts, string genres, string yearOfRelease, string rating)
        {

            int ascii;
            if (title[0] == 'T' && title[1] == 'h' && title[2] == 'e' && title[3] == ' ')
            {
                ascii = (int)title[4];
            }
            else
                ascii = (int)title[0];

            if (ascii < 65 || ascii > 90)
            {
                moviesHashTable.addToList(new Movie(title, director, casts, genres, yearOfRelease, rating), 26);
            }

            int i = 65;
            for (int index = 0; i <= 90; i++, index++)
            {
                if (i == ascii)
                {
                    moviesHashTable.addToList(new Movie(title, director, casts, genres, yearOfRelease, rating), index);
                    break;
                }

            }
        }

        private static void insertMovieRatingWise(string title, string director, string casts, string genres, string yearOfRelease, string rating)
        {
            if (rating == "N/A")
            {
                ratingsHashTable.addToList(new Movie(title, director, casts, genres, yearOfRelease, rating), 9);
            }
            else
            {
                int intRating = Int32.Parse(rating.Substring(0, 1));
                if (intRating >= 1 && intRating < 10)
                {

                    ratingsHashTable.addToList(new Movie(title, director, casts, genres, yearOfRelease, rating), intRating - 1);
                }

            }
        }



    }
}
