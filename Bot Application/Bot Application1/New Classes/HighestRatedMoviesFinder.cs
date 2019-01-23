using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Bot_Application1.New_Classes
{

    class HighestRatedMoviesFinder
    {
        public static List<Movie> findMovies(string specifiedGenre)
        {
            List<Movie> eightPlusList = MoviesAdderIntoHashArray.getSpecificListFromRatingsHashTable(7);
            List<Movie> ninePlusList = MoviesAdderIntoHashArray.getSpecificListFromRatingsHashTable(8);
            List<Movie> selectedMovies = new List<Movie>();

            string upperCaseGenre = Char.ToUpper(specifiedGenre[0]) + specifiedGenre.Substring(1);
            string lowerCaseGenre = Char.ToLower(specifiedGenre[0]) + specifiedGenre.Substring(1);

            if (eightPlusList != null)
            {
                foreach (Movie movie in eightPlusList)
                {
                    string genres = movie.getGenres();
                    if (genres != null)
                    {
                        if (genres.Contains(upperCaseGenre) || genres.Contains(lowerCaseGenre))
                        {
                            selectedMovies.Add(movie);
                        }
                    }
                    
                }
            }

            if (ninePlusList != null)
            {
                foreach (Movie movie in ninePlusList)
                {
                    string genres = movie.getGenres();
                    if (genres != null)
                    {
                        if (genres.Contains(upperCaseGenre) || genres.Contains(lowerCaseGenre))
                        {
                            selectedMovies.Add(movie);
                        }
                    }
                }
            }


            return selectedMovies;
        }
    }
}