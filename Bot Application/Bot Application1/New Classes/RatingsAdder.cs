using Microsoft.Office.Interop.Excel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using Microsoft.Office.Interop.Excel;
using Excel = Microsoft.Office.Interop.Excel;
using System.Runtime.InteropServices;

namespace Bot_Application1.New_Classes
{
    public class RatingsAdder
    {
        public static void initializeWorkbook()
        {
            Excel.Application excelApplication = new Excel.Application();

            Workbook workbook =
               excelApplication.Workbooks.Open(@"F:\DSA Project Stuff\Lists of American Films\List Of American Films 2014.xlsx");
            getExcelFile(workbook, excelApplication);
        }

        private static void getExcelFile(Workbook workbook, Excel.Application application)
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

            List<string> movieTitles = new List<string>();
            for (int i = 2; i <= rowCount; i++)
            {
                string title = xlRange.Cells[i, 1].Value;
                movieTitles.Add(title);
            }

            getSourceCode(movieTitles, xlWorksheet, xlRange, rowCount);

           
        }
     

        public static async void getSourceCode(List<string> movieTitles, Excel.Worksheet worksheet, Excel.Range range, int rCount)
        {
            int cnt = 0;
            List<string> ratings = new List<string>();
            foreach (string movieTitle in movieTitles)
            {
                using (var client = new System.Net.Http.HttpClient())
                {
                    client.DefaultRequestHeaders.TryAddWithoutValidation("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/54.0.2840.99 Safari/537.36");
                    var html = client.GetStringAsync("https://www.google.com/search?q=" + movieTitle + " movie").Result;
                    var doc = new HtmlAgilityPack.HtmlDocument();
                    doc.LoadHtml(html);
                   
                    var nodes = doc.DocumentNode 
                                .SelectNodes("//span[@class='_tvg']");
                    List<string> result = new List<string>();
                    
                    if (nodes != null)
                    {
                        result = nodes.Select(p => p.InnerText)
                       .ToList();

                        string selectedRating = result[0].Substring(0, 3).ToString();
                        ratings.Add(selectedRating);
                       
                    }
                    else
                    {
                        ratings.Add("N/A");
   
                    }

                    cnt++;
                }
            }
            insertIntoExcel(ratings, worksheet, range, rCount);

        }


        private static void insertIntoExcel(List<string> ratings, Excel.Worksheet worksheet, Excel.Range range, int rCount)
        {
            range.Cells[1, 6] = "IMDB Rating";
            for (int i = 2, j = 0; i <= rCount; i++, j++)
            {
                range.Cells[i, 6] = ratings[j];
            }

        }
    }
}