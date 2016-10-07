using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Newtonsoft.Json.Linq;

namespace MovieApp.Models
{
    public class User
    {
        public int Userid;
        public List<Arvostelu> Userarvostelut;
        public double cossim;
        
        public User()
        {
            Userarvostelut = new List<Arvostelu>();
        }
        public void Printuser()
        {
            System.Diagnostics.Debug.WriteLine("==============USERID==============");
            System.Diagnostics.Debug.WriteLine(Userid);
            for (int i = 0; i < Userarvostelut.Count; i++)
            {
                System.Diagnostics.Debug.WriteLine("=====Userin Movieid========");
                System.Diagnostics.Debug.WriteLine(Userarvostelut[i].movieid);
                System.Diagnostics.Debug.WriteLine("======Movien arvostelu========");
                System.Diagnostics.Debug.WriteLine(Userarvostelut[i].review);
            }
        }

        public void SortArvostelut()
        {
            Userarvostelut = Userarvostelut.OrderBy(o => o.movieid).ToList();
        }

        public void SortDescenting()
        {
            Userarvostelut = Userarvostelut.OrderByDescending(o => o.review).ToList();
        }
        public double LaskeCossim(User toinenUser)
        {
            int pistetulo = 0;
            double omavektoripituus = 0;
            double toinenvektoripituus = 0;
            double palautus = 0;
            int i = 0;
            int j = 0;
            while (i < Userarvostelut.Count && j < toinenUser.Userarvostelut.Count)
            {
                
                //Jos osuu sama elokuva lisätään pistetuloon ja liikutetaan molempia yhdellä indexillä
             
                if (Userarvostelut[i].movieid == toinenUser.Userarvostelut[j].movieid)
                {
                    
                    
                    pistetulo += Userarvostelut[i].review*toinenUser.Userarvostelut[j].review;
                    i++;
                    j++;
                }
                else
                { //Jos ei löydy liikutetaan pienmpää indexiä yhdellä
                    if (Userarvostelut[i].movieid < toinenUser.Userarvostelut[j].movieid)
                    {
                        i++;
                    }
                    else
                    {
                        j++;
                    }
                }
            }
            //Jos ei yhtään osumaa niin automaattisesti Cosine similarity 0
            if (pistetulo == 0)
            {
                return 0;
            }
            //lasketaan molempien arvosteluiden vektorien pituus
            foreach (var var in Userarvostelut)
            {
                omavektoripituus += var.review*var.review;
            }
            foreach (var vari in toinenUser.Userarvostelut.ToList())
            {
                toinenvektoripituus += vari.review * vari.review;
            }
            //Sijoitetaan kaikki lopulliseen kaavaan ja palautetaan
            palautus = pistetulo/Math.Sqrt(omavektoripituus*toinenvektoripituus);
            toinenUser.cossim = palautus;
            return palautus;
        }

    }
}