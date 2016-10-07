using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Web;

namespace MovieApp.Models
{
    public class Arvostelu
    {
        public int movieid { get; set; }
        public int review { get; set; }

        public Arvostelu( int movieid, int review)
        {
            this.movieid = movieid;
            this.review = review;

        }
    }

}