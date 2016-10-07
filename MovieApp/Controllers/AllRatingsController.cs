using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using MovieApp.Models;


namespace MovieApp.Controllers
{

    public class AllRatingsController : ApiController
    {
        AllRatings allratings = new AllRatings();
       static List<User> allusers = new List<User>(); 

        public AllRatingsController()
        {
           

            using (StreamReader reader = new StreamReader(@"C:\Matin\MovieApp\MovieApp\MovieApp\u.data"))
            {
                var lineCount = File.ReadLines(@"C:\Matin\MovieApp\MovieApp\MovieApp\u.data").Count();

                string line;

                for (int i = 0; i < lineCount; i++)
                {
                    if ((line = reader.ReadLine()) != null)
                    {
                        
                        Dictionary<string, string> singlereview = new Dictionary<string, string>();
                        singlereview = ParseMovieData(line);
                        int userid = int.Parse(singlereview["Userids"]);
                        int movieid = int.Parse(singlereview["Movieids"]);
                        int userrating = int.Parse(singlereview["Userratings"]);
                        bool loytyi = false;
                      
                        for (int j = 0; j < allusers.Count; j++)
                        {
                            if (userid == allusers[j].Userid)
                            {
                            
                                Arvostelu yksiarvostelu = new Arvostelu(movieid, userrating);
                                allusers[j].Userarvostelut.Add(yksiarvostelu);
                                loytyi = true;
                                break;
                            }
                           
                        }
                        if (loytyi != true)
                        {
                            User user = new User();
                            Arvostelu yksiarvostelu = new Arvostelu( movieid, userrating);
                            user.Userid = userid;
                            user.Userarvostelut.Add(yksiarvostelu);
                            allusers.Add(user);
                        }
   
    
                    }

                }
         

            }

        }


       
        

            [NonAction]
        private static Dictionary<string, string> ParseMovieData(string raw)
        {
            Dictionary<string, string> singlereview = new Dictionary<string, string>();
            string sep = "\t";
            string[] split = raw.Split(sep.ToCharArray());
            singlereview["Userids"] = split[0];
            singlereview["Movieids"] = split[1];
            singlereview["Userratings"] = split[2];

            return singlereview;
        }
        [NonAction]
        public static List<User> Kaikkiarviot()
        {
           
            return allusers;
        }
    }
}
