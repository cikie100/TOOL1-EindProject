using System;
using System.Collections.Generic;

namespace OpgaveLabo
{
    public class Straat
    {
        #region Properties

        public Graaf graaf { get; set; }
        public int straatID { get; set; }
        public string straatnaam { get; set; }
        public double Length { get; set; }

        #endregion Properties

        // 2 constructors omdat databeheer.cs eerst staten aanmaakt en daarna pas de segmentenlijst eraan koppelt
        // doe ik omdat het de proces van alles aan elkaar te koppelen makkelijker maakte voor mij

        public Straat(int straatID, string straatnaam)
        {
            this.straatID = straatID;
            this.straatnaam = straatnaam;
            this.graaf = null;
        }

        public Straat(int straatID, string straatnaam, Graaf graaf)
        {
            this.straatID = straatID;
            this.straatnaam = straatnaam;
            this.graaf = graaf;
        }

        //geeft de knopen van de graaf terug
        public List<Knoop> getKnopen()
        {
            return graaf.getKnopen();
        }

        //berekent straat lengte met de methode SegLengthBerekenen() in Segment
        public void StraatLengthBerekenen()
        {
            double l = 0.0;
            foreach (Segment seg in graaf.segmenten)
            {
                l += seg.SegLengthBerekenen();
            }

            this.Length = Math.Round(l);
        }

        #region methodes voor data wegschrijven te vermakelijken

        //geeft string met: straatId, straatnaam,graafid
        public String GetStraat_voorStraatDataBestand()
        {
            return straatID.ToString() + ";" + straatnaam.ToString().Trim() + ";" + Length.ToString() + ";" + graaf.graafID.ToString();
        }

        //geeft string terug met graaf info : elk knoop en zijn segmenten
        public String GetGraaF_voorDataBestand()
        {
            String x = "";
            if (graaf.map.Count != 0)
            {
                //GraafId
                x += ("*" + graaf.graafID.ToString() + ";");

                foreach (KeyValuePair<Knoop, List<Segment>> kvp in graaf.map)
                {
                    //KnoopId, knoop x punt, knoop y punt
                    x += (kvp.Key.knoopID.ToString() + ";" + kvp.Key.punt.x.ToString() + ";" + kvp.Key.punt.y.ToString() + ";(");
                    kvp.Value.ForEach(segm =>
                    {
                        if (segm != null)
                        {
                            //segmentId;,beginknoopId,eindknoopId
                            x += (segm.segmentID.ToString() + ";" + segm.beginknoop.knoopID.ToString() + ";" + segm.beginknoop.knoopID.ToString() + ";[");
                            //Alle Punten afdrukken van segment
                            //punt x, punt y
                            segm.punten_verticles.ForEach(punt => x += ("(" + punt.x.ToString() + "," + punt.y.ToString() + ")"));

                            x += ("])");
                        }
                    });
                }
                x += " ";
            }

            return x;
        }

        #endregion methodes voor data wegschrijven te vermakelijken
    }
}