using OpgaveLabo;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Tool1.Model
{
    public class Gemeente
    {
        public string gemeenteNaamId { get; set; }
        public string gemeenteId { get; set; }

        ////filteren  want  we  gaan enkel de Nederlandse naam gebruiken (code : nl).
        ///getGemeente() in Databeheer filtert het voor ons.
        public string taalCodeGemeenteNaam { get; set; }

        public string gemeenteNaam { get; set; }

        public List<Straat> stratenLijst { get; set; }
        public List<String> stratenNaamId { get; set; }

        public Gemeente(string gemeenteNaamId, string gemeenteId, string taalCodeGemeenteNaam, string gemeenteNaam)
        {
            this.gemeenteNaamId = gemeenteNaamId;
            this.gemeenteId = gemeenteId;
            this.taalCodeGemeenteNaam = taalCodeGemeenteNaam;
            this.gemeenteNaam = gemeenteNaam;

            this.stratenLijst = new List<Straat>();
            stratenNaamId = new List<string>();
        }

        //vult de stratenNaamId lijst op in Databeheer
        public void addStratenNaamId_toGemeente(string StraatID, string gemID)
        {
            if (gemID.Equals(gemeenteId))
            {
                stratenNaamId.Add(StraatID);
            }
        }

        //vraagt  veel
        public void Dezegemeente_stratenObjecten_gevenVanTekstLijst(List<Straat> straten)
        {
            for (int i = 0; i < straten.Count; i++)
            {
                if (stratenNaamId.Contains(straten[i].straatID.ToString()))
                {
                   
                    stratenLijst.Add(straten[i]);
                }
            }
        }

        public String TotaleLengte()
        {
            double l = 0.0;
            foreach (Straat str in stratenLijst)
            {
                l += str.Length;
            }
            double x = Math.Round(l);
            return x.ToString();
        }

        public String Getkortste_straat() {

            //  Straat kort = stratenLijst.Where(s => s.Length != 0.0 && s.Length != 0).Min();
            

            double x = stratenLijst.Where(s => s.Length !=0 ).Select(s => s.Length).Min();
            Straat kort = stratenLijst.Where(s => s.Length.Equals(x)).FirstOrDefault();

            return ("\t\t >> Id: " + kort.straatID.ToString() + " , Naam: " +kort.straatnaam.ToString().Trim() + " , Lengte: "+kort.Length.ToString()+" m");
        }
        public String Getlangste_straat() {
            double x = stratenLijst.Max(s => s.Length);
            Straat lang = stratenLijst.Where(s => s.Length.Equals(x)).FirstOrDefault();

            return ("\t\t >> Id: " + lang.straatID.ToString() + " , Naam: " + lang.straatnaam.ToString().Trim() + " , Lengte: " + lang.Length.ToString() + " m");
        }
        //
        public String returnAlleStraatIds() {

            String x = "";
            if (stratenLijst.Count !=0)
            {
                for (int i = 0; i < stratenLijst.Count; i++)
                {
                    x += stratenLijst[i].straatID.ToString() + ";";
                }
            }
            return x;
        
        }

        public String returnAlleStraatGraaf_vGemeente_GraafBestand() {
            String x = "";
            stratenLijst.ForEach(straat =>
            {
                if (straat.GetGraaF_voorDataBestand().Length > 1)
                {
                    x += straat.GetGraaF_voorDataBestand();
                }
            });

            return x;
        
        }
    }
}