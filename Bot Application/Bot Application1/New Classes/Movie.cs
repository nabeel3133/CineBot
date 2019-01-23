using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bot_Application1
{
    public class Movie
    {
        private string title;
        private string director;
        private string casts;
        private string genres;
        private string yearOfRelease;
        private string rating;

        public Movie(string title, string director, string casts, string genres, string yearOfRelease, string rating)
        {
            this.title = title;
            this.director = director;
            this.casts = casts;
            this.genres = genres;
            this.yearOfRelease = yearOfRelease;
            this.rating = rating;
        }

        public string getTitle()
        {
            return this.title;
        }

        public string getRating()
        {
            return this.rating;
        }

        public double getDoubleRating()
        {
            return Double.Parse(this.rating);
        }

        public int getIntegerRating()
        {
            return Int32.Parse(this.rating.Substring(0, 1));
        }
        public string getGenres()
        {
            return this.genres;
        }

        public string getCast()
        {
            return this.casts;
        }

        public string getDirector()
        {
            return this.director;
        }

        public string getYearOfRelease()
        {
            return this.yearOfRelease;
        }
    }


}
