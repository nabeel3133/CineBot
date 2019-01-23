using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Bot_Application1
{
    class SimilarMoviesFinder
    {
        private static string entityTitle = "";

        public static List<Movie> returnSimilarMoviesList(string eTitle)
        {
            List<Movie> similarMovies = new List<Movie>();
            entityTitle = eTitle;

            int genreMatchCount = 0;

            Movie movieFromTable = returnEntityMovieFromMoviesHashTable();

            if (movieFromTable == null)
            {
                return null;
                
            }

            string genreString = movieFromTable.getGenres();
            if (genreString == null)
                return null;

            string[] genreArray = Regex.Split(genreString, ", ");

            string rating = movieFromTable.getRating();
            List<Movie> list = MoviesAdderIntoHashArray.getSpecificListFromRatingsHashTable(movieFromTable.getIntegerRating() - 1);

            foreach (Movie movie in list)
            {
                string[] tempGenreArray = Regex.Split(movie.getGenres(), ", ");

                foreach (string f in tempGenreArray)
                    Console.Write(f + ", ");

                foreach (string genre in genreArray)
                {
                   
                    if (tempGenreArray.Contains(genre))
                        genreMatchCount++;
                }
                if (genreMatchCount > 0)
                    similarMovies.Add(movie);
            }
            Console.WriteLine();

            return similarMovies;
        }


        private static Movie returnEntityMovieFromMoviesHashTable()
        {
            Movie movie;
            int ascii;

            if (entityTitle.Substring(0, 4).Equals("the ", StringComparison.InvariantCultureIgnoreCase))
                ascii = (int)Char.ToUpper(entityTitle[4]);

            else
            {
                ascii = Char.ToUpper(entityTitle[0]);
            }

            if (ascii < 65 || ascii > 90)
            {
                movie = findEntityMovieFromHashTable(26);
                if (movie != null)
                {
                    return movie;
                }

                return null;
            }

            else
            {
                movie = findEntityMovieFromHashTable(ascii - 65);
                if (movie != null)
                {
                    return movie;
                }

                return null;
            }
        }

        private static Movie findEntityMovieFromHashTable(int index)
        {
            List<Movie> list = MoviesAdderIntoHashArray.getSpecificListFromMoviesHashTable(index);
            if (list != null)
            {

                foreach (Movie movie in list)
                {
                    if (movie.getTitle().Trim().Equals(entityTitle.Trim(), StringComparison.InvariantCultureIgnoreCase)) // ERROR!!
                    {
                        return movie;
                    }
                }

                return null;
            }

            else
            {
                return null;
            }

        }
    }
}
