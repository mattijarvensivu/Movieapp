using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MovieApp.Models
{
    public class UserRatings
    {
     
        public int[] Movieids { get; set; }
        public int[] MovieRatings { get; set; }

        public UserRatings()
        {
            Movieids = new int[20];
            MovieRatings = new int[20];
        }
    }
    
    
}