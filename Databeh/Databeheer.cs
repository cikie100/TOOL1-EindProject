using OpgaveLabo;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Tool1.Model;

namespace Tool1.Databeh
{
    public class Databeheer
    {
        //Hierin lees ik al de data bestanden in en verwerk ik hun naar klassen en etc
        //

        private List<Gemeente> gemeentes = getGemeente();
        public List<Segment> wegSegmentenLijstVoorDatabestand;

        #region inlees methodes

        //
        // geeft List terug met Provincie id's die moeten gebruikt worden
        // ProvincieIDsVlaanderen.csv geeft 1,2,4,5,8
        public List<String> getprovincieIdsVlaanderen()
        {
            List<String> provincieIdsVlaanderen = new List<string>();
            FileInfo providtxt = new FileInfo(@"..\..\..\..\WRData\ProvincieIDsVlaanderen.csv");
            using (StreamReader sreader = providtxt.OpenText())
            {
                string cijfers = sreader.ReadLine();

                string[] words = cijfers.Split(',');
                for
                    (int i = 0; i < words.Count(); i++) //loops through each element of the array
                {
                    provincieIdsVlaanderen.Add(words[i]); //Add each int on the array on to provincieIdsVlaanderen list
                }
            }
            return provincieIdsVlaanderen;
        }

        //
        // geeft List terug met provincies en hun ( gemeenteId; provincieId; taalCodeProvincieNaam; provincieNaam)
        // deze methonde bekijkt of de id in de lijst van getprovincieIdsVlaanderen() zit.
        // ProvincieInfo.csv geeft gemeenteId; provincieId; taalCodeProvincieNaam; provincieNaam
        public List<Provincie> getprovinciesList()
        {
            List<String> ProvInVla = getprovincieIdsVlaanderen();
            List<Provincie> provinciesLijst = new List<Provincie>();
            List<string> ProvInfo = new List<string>();

            FileInfo provtxt = new FileInfo(@"..\..\..\..\WRData\ProvincieInfo.csv");
            using (StreamReader sreader = provtxt.OpenText())
            {
                string input = null;
                while ((input = sreader.ReadLine()) != null)
                {
                    if (input.Contains("nl"))
                    {
                        string woord = input;
                        string[] words = woord.Split(';');
                        if (words.Contains("nl"))
                        {
                            for
                            (int i = 0; i < words.Count(); i++) //loops through each element of the array
                            {
                                ProvInfo.Add(words[i]); //Add each word on the array on to woorden list
                            }
                        }
                    }
                }
            }
            //overloopt heel de lijst en neemt per keer 4x objecten eruit
            for (int i = 0; i < ProvInfo.Count(); i += 4)
            {   //kijkt of provincie al bestaat
                if (!provinciesLijst.Any(p => p.provincieId.Equals(ProvInfo[i + 1])))
                {
                    if (ProvInVla.Contains(ProvInfo[i + 1]))
                    {
                        provinciesLijst.Add(new Provincie(ProvInfo[i + 1], ProvInfo[i + 2], ProvInfo[i + 3]));
                    }
                }
                //voegt gemeenteIds toe aan huidige provincie
                if (provinciesLijst.Any(p => p.provincieId.Equals(ProvInfo[i + 1])))
                {
                    Provincie x = provinciesLijst.Where(p => p.provincieId.Equals(ProvInfo[i + 1])).First();
                    x.addGemeenteIds_toProvin(ProvInfo[i], ProvInfo[i + 1]);
                }
            }
            provinciesLijst.ForEach(p => p.Prov_gemeente_geven(gemeentes));
            return provinciesLijst;
        }

        //
        // maakt gemeentes aan en geeft List terug, bevat de methode getStraatNaamId_gemeenteID(List gemeente),
        // die geeft List stratenNaamId toe per gemeente. Hierdoor weet je welke staat(Id) tot welke gemeente hoort.
        // WRGemeentenaam.csv geeft gemeenteNaamId; gemeenteId; taalCodeGemeenteNaam; gemeenteNaam
        public static List<Gemeente> getGemeente()
        {
            List<Gemeente> Gemeentes = new List<Gemeente>();
            List<string> ProvInfo = new List<string>();

            FileInfo Gemtxt = new FileInfo(@"..\..\..\..\WRData\WRGemeentenaam.csv");
            using (StreamReader sreader = Gemtxt.OpenText())
            {
                string input = null;
                while ((input = sreader.ReadLine()) != null)
                {
                    if (input.Contains("nl"))
                    {
                        string woord = input;
                        string[] words = woord.Split(';');

                        ////filteren  want  we  gaan enkel de Nederlandse naam gebruiken (code : nl).
                        if (words.Contains("nl"))
                        {
                            for
                            (int i = 0; i < words.Count(); i++) //loops through each element of the array
                            {
                                ProvInfo.Add(words[i]); //Add each word on the array on to woorden list
                            }
                        }
                    }
                }
            }
            for (int i = 0; i < ProvInfo.Count(); i += 4)
            {
                Gemeentes.Add(new Gemeente(ProvInfo[i], ProvInfo[i + 1], ProvInfo[i + 2], ProvInfo[i + 3]));
            }
            getStraatNaamId_gemeenteID(Gemeentes);
            return Gemeentes;
        }

        //
        // geeft al de straten die in provincie>gemeente>straat zitten een wegsegment terug.
        // Wegsegmenten werden gemaakt door MaakSegmenten_vanDocument().
        public void GeefStratenWegSegment(List<Straat> stratenLijst, List<Provincie> provincies)
        {
            List<Segment> wegSegmentenLijst = new List<Segment>();

            //duurt 20 sec
            wegSegmentenLijst = MaakSegmenten_vanDocument().OrderBy(o => o.linksStraatnaamID).ToList();
            //  v  idk yet, neemt veel in
            //int x = wegSegmentenLijst.RemoveAll(wsg => !provincies.Any(p => p.gemeenteLijst.Any
            //        (g => g.stratenNaamId.Contains(wsg.linksStraatnaamID.ToString())
            //        || g.stratenNaamId.Contains(wsg.rechtsStraatnaamID.ToString()))));

            List<Segment> wegSegmentenLijst_VoorHuidige_straat;

            for (int i = 0; i < stratenLijst.Count(); i++)
            {
                wegSegmentenLijst_VoorHuidige_straat = new List<Segment>();

                foreach (Segment se in wegSegmentenLijst.Where(ws => ws.linksStraatnaamID <= stratenLijst[i].straatID + 1
                                                                 && ws.linksStraatnaamID >= stratenLijst[i].straatID - 1
                ))
                {
                    if (se.linksStraatnaamID.Equals(stratenLijst[i].straatID))
                    {
                        wegSegmentenLijst_VoorHuidige_straat.Add(se);
                    }
                }
                if (wegSegmentenLijst_VoorHuidige_straat.Count > 0)
                {
                    Graaf graafx = Graaf.buildGraaf(wegSegmentenLijst_VoorHuidige_straat);
                    stratenLijst[i].graaf = graafx;

                    stratenLijst[i].StraatLengthBerekenen();

                    if (stratenLijst[i].Length == 0)
                    {
                        stratenLijst.Remove(stratenLijst[i]);
                    }

                    List<Segment> toRemove = new List<Segment>();
                    toRemove = wegSegmentenLijst_VoorHuidige_straat;
                    foreach (Segment item in toRemove)
                    {
                        wegSegmentenLijst.Remove(item);
                    }
                }
            }
        }

        // geeft een List terug, checkt eerst of een gemeente die stratenId bezit voor de straat word aangemaakt.
        // WRstraatnamen.csv geeft straatNaamId;straatNaam
        public List<Straat> getStraatNamen(List<Provincie> provincies)
        {
            #region lees in van bestand

            List<Straat> straten = new List<Straat>();
            List<String> StratenAllesInString = new List<string>();

            FileInfo providtxt = new FileInfo(@"..\..\..\..\WRData\WRstraatnamen.csv");
            using (StreamReader sreader = providtxt.OpenText())
            {
                string stratenLijn = sreader.ReadLine();
                string input = null;
                while ((input = sreader.ReadLine()) != null)
                {
                    if (!input.Contains("NULL"))
                    {
                        string woord = input;
                        string[] words = woord.Split(';');
                        for
                            (int i = 0; i < words.Count(); i++) //loops through each element of the array
                        {
                            StratenAllesInString.Add(words[i]); //Add each int on the array on to provincieIdsVlaanderen list
                        }
                    }
                }
            }

            #endregion lees in van bestand

            //LJENA: zet i terug op 0
            for (int i = 0; i < StratenAllesInString.Count(); i += 2)
            {
                if (provincies.Any(p => p.gemeenteLijst.Any(g => g.stratenNaamId.Contains(StratenAllesInString[i]))))
                {
                    straten.Add(new Straat(Int32.Parse(StratenAllesInString[i]), StratenAllesInString[i + 1]));
                }
            }
            return straten;
        }

        //
        // voegt straatnaamId's toe aan gepaste gemeente. wordt gebruikt door getGemeente();
        // WRGemeenteID.csv geeft straatNaamId;gemeenteId
        public static void getStraatNaamId_gemeenteID(List<Gemeente> gemeentes)
        {
            List<String> Txt_naar_eenString = new List<String>();

            FileInfo bestand_txt = new FileInfo(@"..\..\..\..\WRData\WRGemeenteId.csv");
            using (StreamReader sreader = bestand_txt.OpenText())
            {
                string stratenLijn = sreader.ReadLine();
                string input = null;
                while ((input = sreader.ReadLine()) != null)
                {
                    string woord = input;
                    string[] words = woord.Split(';');
                    for
                        (int i = 0; i < words.Count(); i++) //loops through each element of the array
                    {
                        Txt_naar_eenString.Add(words[i]); //Add each int on the array on to provincieIdsVlaanderen list
                    }
                }
            }
            for (int i = 0; i < Txt_naar_eenString.Count(); i += 2)
            {
                if (gemeentes.Any(g => g.gemeenteId.Equals(Txt_naar_eenString[i + 1])))
                {
                    Gemeente x = gemeentes.Where(g => g.gemeenteId.Equals(Txt_naar_eenString[i + 1])).First();
                    x.addStratenNaamId_toGemeente(Txt_naar_eenString[i], Txt_naar_eenString[i + 1]);
                }
            }
        }

        //
        // maakt segmenten lijst aan, wordt gebruikt door GeefStratenWegSegment(...)
        // WRdata.csv geeft wegsegmentID; geo; morfologie; status; beginWegknoopID; eindWegknoopID; linksStraatnaamID; rechtsStraatnaamID;
        public List<Segment> MaakSegmenten_vanDocument()
        {
            List<Segment> segmentsList = new List<Segment>();
            List<String> WS_AllesInString = new List<string>();

            FileInfo providtxt = new FileInfo(@"..\..\..\..\WRData\WRdata.csv");
            using (StreamReader sreader = providtxt.OpenText())
            {
                // string stratenLijn = sreader.ReadLine();
                string input = null;
                while ((input = sreader.ReadLine()) != null)
                {
                    string woord = input;
                    string[] words = woord.Split(';');
                    for
                        (int i = 0; i < words.Count(); i++) //loops through each element of the array
                    {
                        WS_AllesInString.Add(words[i]); //Add each int on the array on to provincieIdsVlaanderen list
                    }
                }
            }
            for (int i = 0; i < WS_AllesInString.Count(); i += 8)
            {
                if (!WS_AllesInString[i].Contains("wegsegmentID"))
                {
                    //Wegsegmenten  die  zowel  een  linksStraatnaamID  als  rechtsStraatnaamID  hebben
                    //  met de waarde –9,gebruiken we niet.
                    if (!WS_AllesInString[i + 6].Equals("-9") &&
                        !WS_AllesInString[i + 7].Equals("-9"))
                    {
                        String puntenString = WS_AllesInString[i + 1];
                        string[] puntenArray = puntenString.Replace("LINESTRING (", "").Replace(")", "").Split(",");

                        //heeft al de punten vast van de geo
                        List<Punt> punten = new List<Punt>();

                        for
                        (int ii = 0; ii < puntenArray.Count(); ii++) //loops through each element of the array
                        {
                            string[] puntenArrayyy = puntenArray[ii].Split(" ");
                            puntenArrayyy = puntenArrayyy.Where(x => !string.IsNullOrEmpty(x)).ToArray();
                            punten.Add(new Punt(Convert.ToDouble(puntenArrayyy[0]), Convert.ToDouble(puntenArrayyy[1])));
                        }

                        Segment x = new Segment(Convert.ToInt32(WS_AllesInString[i]),
                                               new Knoop(Convert.ToInt32(WS_AllesInString[i + 4]), punten.First()),
                                               new Knoop(Convert.ToInt32(WS_AllesInString[i + 5]), punten.Last()),
                                               punten,
                                              Convert.ToInt32(WS_AllesInString[i + 6]),
                                              Convert.ToInt32(WS_AllesInString[i + 7])
                                               );

                        segmentsList.Add(x);
                    }
                }
            }

            return segmentsList;
        }

        #endregion inlees methodes
    }
}