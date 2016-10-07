using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MovieApp.Models
{
    public class AllRatings
    {
        public int[] Userids  { get; set; }
        public int[] Movieids { get; set; }
        public int[] Userratings { get; set; }

        public AllRatings()
        {
            Userids = new int[100000];
            Movieids = new int[100000];
            Userratings = new int[100000];
        }
    }
}