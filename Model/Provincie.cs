using System;
using System.Collections.Generic;

namespace Tool1.Model
{
    public class Provincie
    {
        #region prop

        public List<String> gemeenteIds { get; set; }

        //alle gemeentes van deze provincie
        public List<Gemeente> gemeenteLijst { get; set; }

        public String provincieId { get; set; }
        public String taalCodeProvincieNaam { get; set; }
        public String provincieNaam { get; set; }

        #endregion prop

        public Provincie(String provincieId, string taalCodeProvincieNaam, string provincieNaam)
        {
            this.provincieId = provincieId;
            this.taalCodeProvincieNaam = taalCodeProvincieNaam;
            this.provincieNaam = provincieNaam;
            this.gemeenteIds = new List<string>();
            this.gemeenteLijst = new List<Gemeente>();
        }

        //gebruikt voor het vullen van gemeenteIds van een provincie ( in Databeheer.cs)
        public void addGemeenteIds_toProvin(string GemID, string ProvID)
        {
            if (ProvID.Equals(provincieId))
            {
                gemeenteIds.Add(GemID);
            }
        }

        //gebruikt voor het vullen van List<gemeente> (gebeurd in Databeheer.cs)
        public void Prov_gemeente_geven(List<Gemeente> gemeentes)
        {
            for (int i = 0; i < gemeentes.Count; i++)
            {
                if (gemeenteIds.Contains(gemeentes[i].gemeenteId))
                {
                    gemeenteLijst.Add(gemeentes[i]);
                }
            }
        }

        //gebruikt voor data tekst van Provincie weg te schrijven (gebruikt in ProgramRapport.cs)
        public String GetGemeenteIds_voorDatabestand()
        {
            String x = "";
            if (gemeenteIds.Count != 0)
            {
                for (int i = 0; i < gemeenteIds.Count; i++)
                {
                    x += gemeenteIds[i] + ";";
                }
            }
            return x;
        }
    }
}