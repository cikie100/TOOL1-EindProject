using System;
using System.Collections.Generic;
using System.Linq;

namespace OpgaveLabo
{
    public class Segment
    //Een wegsegment wordt begrensd door een begin-en eindknoop
    //en verwijst naar een lijst van punten die het segment beschrijven.
    {
        #region Properties

        public Knoop beginknoop { get; set; }
        public Knoop eindknoop { get; set; }
        public int segmentID { get; set; }
        public List<Punt> punten_verticles { get; set; }

        public int linksStraatnaamID { get; set; }
        public int rechtsStraatnaamID { get; set; }
        public double Length { get; set; }

        #endregion Properties

        public Segment(int segmentID, Knoop beginknoop, Knoop eindknoop, List<Punt> lijstPunten, int linksStraatnaamID, int rechtsStraatnaamID)
        {
            this.segmentID = segmentID;
            this.beginknoop = beginknoop;
            this.eindknoop = eindknoop;
            this.punten_verticles = lijstPunten;
            this.linksStraatnaamID = linksStraatnaamID;
            this.rechtsStraatnaamID = rechtsStraatnaamID;

            if (punten_verticles.Count.Equals(0) || !punten_verticles.Any() || punten_verticles == null)
            {
                this.Length = 0;
            }
            else
            {
                this.Length = Math.Round(SegLengthBerekenen());
            }
        }

        //gebruikt bij testen & debug
        public override string ToString()
        {
            List<string> pntLijst = null;

            punten_verticles.ForEach(p => pntLijst.Add(p.ToString()));

            return ("Segment {0}, heeft als beginknoop x,y: ({1},{2}), heeft als eindknoop x,y: ({3},{4})",
                segmentID,
                beginknoop.punt.x,
                 beginknoop.punt.y,
                  eindknoop.punt.x,
                   eindknoop.punt.y
                          )

                +
                 "/n en als verticles: " + pntLijst;
        }

        #region equals & getHashCode

        public override bool Equals(object obj)
        {
            return obj is Segment segment &&
                   EqualityComparer<Knoop>.Default.Equals(beginknoop, segment.beginknoop) &&
                   EqualityComparer<Knoop>.Default.Equals(eindknoop, segment.eindknoop) &&
                   segmentID == segment.segmentID &&
                   EqualityComparer<List<Punt>>.Default.Equals(punten_verticles, segment.punten_verticles);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(beginknoop, eindknoop, segmentID, punten_verticles);
        }

        #endregion equals & getHashCode

        //Methodes
        //berekent de lengte van de segment, door de afstand van punt naar punt te berekenen
        public double SegLengthBerekenen()
        {
            double l = 0.0;

            for (int i = 1; i < punten_verticles.Count; i++)
            {
                l += Math.Sqrt(Math.Pow(punten_verticles[i].x - punten_verticles[i - 1].x, 2) + Math.Pow(punten_verticles[i].y - punten_verticles[i - 1].y, 2));
            }
            return l;
        }
    }
}