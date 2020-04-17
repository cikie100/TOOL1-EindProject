using System;
using System.Collections.Generic;
using System.Linq;

namespace OpgaveLabo
{
    public class Graaf
    {
        #region Properties

        private static int nrOfInstances = 0;
        public int graafID;
        public List<Segment> segmenten { get; set; }
        public Dictionary<Knoop, List<Segment>> map { get; set; }

        #endregion Properties

        public Graaf(int iD, List<Segment> segmenten)
        {
            this.graafID = iD;
            this.segmenten = segmenten;

            this.map = new Dictionary<Knoop, List<Segment>>();

            MaakMapAan();
        }

        public void MaakMapAan()
        {
            List<Segment> x;
            List<Segment> y;
            //zie kladpapier hiervoor.
            //als er geen segmenten zijn gevonden voor de straat
            if (segmenten.Count == 0)
            {
            }
            //als er maar 1 segment is voor de straat
            else if (segmenten.Count == 1)
            {
                x = new List<Segment>();
                x.Add(segmenten[0]);
                map.Add(segmenten[0].beginknoop, x);
                map.Add(segmenten[0].eindknoop, x);
            }
            // als er meerdere segmenten zijn voor de straat
            else
            {
                foreach (Segment seg in segmenten)
                {
                    x = new List<Segment>();
                    y = new List<Segment>();

                    #region als een begin punt

                    //de eerste segment (begin van de straat) dus er bestaat geen segment met deze eindknoop voor deze straat in segmenten.
                    if (!segmenten.Exists(s => s.eindknoop.Equals(seg.beginknoop))
                        || segmenten.Count(s => s.beginknoop.Equals(seg.beginknoop)) > 1
                        )
                    {
                        if (segmenten.Count(s => s.beginknoop.Equals(seg.beginknoop)) == 1)
                        {
                            //de beginknoop toevoegen
                            x.Add(seg);
                            if (!map.ContainsKey(seg.beginknoop))
                            {
                                map.Add(seg.beginknoop, x);
                            }
                        }
                        else
                        {
                            if (!map.ContainsKey(seg.beginknoop))
                            {
                                List<Segment> metBK_Lis = segmenten.Where(s => s.beginknoop.Equals(seg.beginknoop)).ToList();
                                metBK_Lis.ForEach(obj => x.Add(obj));

                                //als er een segment is waar de eindknoop = seg.beginknoop, moet die segment er ook bij
                                List<Segment> metBK_List = segmenten.Where(s => s.eindknoop.Equals(seg.beginknoop)).ToList();
                                metBK_List.ForEach(obj => x.Add(obj));

                                map.Add(seg.beginknoop, x);
                            }
                        }
                    }

                    #endregion als een begin punt

                    #region als een eind punt

                    else if (!segmenten.Exists(s => s.beginknoop.Equals(seg.eindknoop)))
                    {
                        //als maar 1 elements deze eindknoop in segmenten
                        if (segmenten.Count(s => s.eindknoop.Equals(seg.eindknoop)) == 1)
                        {
                            {
                                x.Add(seg);
                                //eindknoop toevoegen van laatste segment
                                map.Add(seg.eindknoop, x);

                                //beginknoop toevoegen ( die heeft 2 segmenten)
                                y.Add(seg);
                                Segment sX = segmenten.Find(s => s.eindknoop.Equals(seg.beginknoop));
                                y.Add(sX);
                                map.Add(seg.beginknoop, y);
                            }
                        }
                        // als meerdere elementen aan deze eindknoop
                        else
                        {
                            if (!map.ContainsKey(seg.eindknoop))
                            {
                                List<Segment> metEK_Lis = segmenten.Where(s => s.eindknoop.Equals(seg.eindknoop)).ToList();
                                metEK_Lis.ForEach(obj => x.Add(obj));
                                map.Add(seg.eindknoop, x);

                                List<Segment> metEK_List = segmenten.Where(s => s.eindknoop.Equals(seg.eindknoop)).ToList();
                                metEK_List.ForEach(segg =>
                                {
                                    y = new List<Segment>();
                                    y.Add(segg);
                                    Segment VorigeSeg = segmenten.Where(s => s.eindknoop.Equals(segg.beginknoop)).FirstOrDefault();
                                    y.Add(VorigeSeg);

                                    if (!map.ContainsKey(segg.beginknoop))
                                    {
                                        map.Add(segg.beginknoop, y);
                                    }
                                }

                                    );
                            }
                        }
                    }

                    #endregion als een eind punt

                    #region knopen die geen begin of eindes zijn

                    //knopen tussen de eerste segment en laatste segment van de straat
                    else
                    {
                        Segment sX = segmenten.Find(s => s.eindknoop.Equals(seg.beginknoop));
                        x.Add(seg);
                        x.Add(sX);
                        map.Add(seg.beginknoop, x);
                    }

                    #endregion knopen die geen begin of eindes zijn
                }
            }
        }

        public static int _graafID { get; set; }

        public static Graaf buildGraaf(List<Segment> segmenten)
        {
            _graafID = Graaf.nrOfInstances;
            Graaf.nrOfInstances++;
            return new Graaf(_graafID, segmenten);
        }

        public List<Knoop> getKnopen()
        {
            List<Knoop> knopen = null;

            foreach (KeyValuePair<Knoop, List<Segment>> entry in map)
            {
                knopen.Add(entry.Key);
            }

            return knopen;
        }

        public void showGraaf()
        {
         //  String x = "("+graafID.ToString()
        }




    }


}