using System;
using System.CodeDom;
using System.CodeDom.Compiler;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Runtime.Remoting.Messaging;
using System.Web.Http;
using MovieApp.Models;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http.Description;
using System.Web.UI.WebControls;


namespace MovieApp.Controllers
{
    public class MovieController : ApiController
    {
        Random rnd = new Random();
        private static  readonly object synclock = new object();
        Movie[] movies = new Movie[1682];
        Movie[] selectedmovies = new Movie[20];
        Movie[] suositellutleffat = new Movie[30];
        List<User> suositellut = new List<User>();
        List<int> suositellutleffaid = new List<int>();


        public MovieController ()
        {
            //suositellut.Clear();
            //suositellutleffaid.Clear();
            using (StreamReader reader = new StreamReader(@"C:\Matin\MovieApp\MovieApp\MovieApp\u.item"))
            {
                var lineCount = File.ReadLines(@"C:\Matin\MovieApp\MovieApp\MovieApp\u.item").Count();
                
                string line;
               
               for(int i = 0; i<lineCount; i++)
                {//Luetaan data ja parsetetaan muuttujiin
                    if((line = reader.ReadLine()) != null) { 
                    Dictionary<string, string> singlemovie = new Dictionary<string, string>();
                    singlemovie = ParseMovieData(line);
                    int movieid = int.Parse(singlemovie["Id"]);
                    string moviename = singlemovie["Name"];
                    string movieyear = singlemovie["Year"];
            
                        movies[i] = new Movie {Id = movieid, Name = moviename, Year = movieyear};
                       
                    }
                }
               
               for(int j= 0; j<20; j++)
                {
                    lock (synclock)
                    {
                        selectedmovies[j] = movies[rnd.Next(0, 1682)];
                    }
                }

              
            }

        }


        public IEnumerable<Movie> GetAllMovies()
        {
     
            return selectedmovies;
        }
        
    
        [ResponseType(typeof(UserRatings))]
        public HttpResponseMessage PostMovie(UserRatings Omaarvostelu)
        {
            
   
            List<User> kaikkiarviot = new List<User>();
            
            User self = new User();
            self.Userid = -1;
            
           //Lisätään käyttäjän arvostelut
           for(int i= 0; i<20; i++)
            {

                Arvostelu uusiArvostelu = new Arvostelu(Omaarvostelu.Movieids[i], Omaarvostelu.MovieRatings[i]);
                self.Userarvostelut.Add(uusiArvostelu);
            }
            //Sortataan arvostelut movied:n mukaan
            
           self.SortArvostelut();

            double cossim = 0;

            kaikkiarviot = AllRatingsController.Kaikkiarviot();
            
           
            for (int j=0; j < kaikkiarviot.Count; j++)
            {
                //sortataan arviot
                kaikkiarviot[j].SortArvostelut();
                //Itse Cosine similarityn lasku
                cossim =self.LaskeCossim(kaikkiarviot[j]);
 //Jos vähämmä kuin kolme suositelluissa käyttäjissä niin lisätään suoraan
                if (suositellut.Count < 3)
                {
                    suositellut.Add(kaikkiarviot[j]);
                }
                else
                {
 //järjestellään niin että 0 indexi suurin 
                        suositellut = suositellut.OrderByDescending(x => x.cossim).ToList();
                        if (suositellut[2].cossim < cossim)
                        {
                            //poistetaan pienin

                            suositellut.RemoveAt(2);
                            kaikkiarviot[j].cossim = cossim;
                            suositellut.Add(kaikkiarviot[j]);
                           
                        }         
                }
            }
            
   
            suositellut = suositellut.OrderByDescending(x => x.cossim).ToList();
            
            System.Diagnostics.Debug.WriteLine("========top 3========");
            for (int i = 0; i < suositellut.Count; i++)
            {
             
                System.Diagnostics.Debug.WriteLine(suositellut[i].cossim);
   
            }
            System.Diagnostics.Debug.WriteLine("==========top 3========");

            for (int j = 0; j < 3; j++)
            {
                suositellut[j].SortDescenting();
               
                for (int i = 0; i < 10; i++)
                {
                    
                    suositellutleffaid.Add(suositellut[j].Userarvostelut[i].movieid);
                    
                }
            }
            //tallennetaan Kolmekymmentä elokuvaa
            for(int i = 0; i<30; i++)
            {
                
                for (int j = 0; j < movies.Length; j++)
                {
                    if (movies[j].Id == suositellutleffaid[i])
                    {
                        suositellutleffat[i] = movies[j];
                    }
                } 

            }
           
           
            //response viestin luonti
            HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.OK, suositellutleffat);
            return response;
        }

    // hae yksittäinen leffa id:nn perusteella Api/movie/{id}
        public IHttpActionResult GetMovie(int id)
        {
            var selectedmovies = movies.FirstOrDefault((p) => p.Id == id);
            if (selectedmovies == null)
            {
                return NotFound();
            }
            return Ok(selectedmovies);
        }
        //parsetus metode
        [NonAction]
        private static Dictionary<string, string> ParseMovieData(string raw)
        {
            Dictionary<string, string> singlemovie = new Dictionary<string, string>();
            string[] split = raw.Split('|');
            singlemovie["Id"] = split[0];
            singlemovie["Name"] = split[1];
            singlemovie["Year"] = split[2];

            return singlemovie;
        }

        


    }

}
