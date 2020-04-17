using OpgaveLabo;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using Tool1.Databeh;
using Tool1.Model;

namespace Tool1
//leest de data braaf in
//Alles zou moeten lukken, mss niet correct maar het lukt
{
    public class ProgramRapport
    {
        private static Databeheer d = new Databeheer();

        private static void Main(string[] args)
        {
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();

            #region nodige data en lists aanmaken met databeheer.cs

            List<Provincie> provincies = d.getprovinciesList(); //duurt 1 second

            //--Maakt lijst straten aan die zich in bestaande gemeente lijst bevinden
            List<Straat> gimmeStreets = d.getStraatNamen(provincies); //duurt 73 seconden

            //--Vult gemeente lijst met juiste <Straat>
            provincies.ForEach(p => p.gemeenteLijst.ForEach(g => g.Dezegemeente_stratenObjecten_gevenVanTekstLijst(gimmeStreets))); // duurt 54 seconden

            //Duurt heeeeeel erg lang, maakt graaf objecten aan voor de straten
            d.GeefStratenWegSegment(gimmeStreets, provincies);  //--

            #endregion nodige data en lists aanmaken met databeheer.cs

            #region
            // rapport aanmaken, zie vb. in map Rapport

            string fileName = @"..\..\..\Rapport\RapportBestand.txt";
            FileInfo fileLoc = new FileInfo(fileName);
            try
            {
                // Check if file already exists. If yes, delete it.
                if (fileLoc.Exists)
                {
                    fileLoc.Delete();
                }

                // Create a new file
                using (StreamWriter sw = File.CreateText(fileName))
                {
                    sw.WriteLine(StratenIntBerekenen_enAfdrukken(provincies) + "\n");
                    sw.WriteLine("Aantal Straten per provincie : \n");
                    Provafdrukken(provincies).ForEach(str => sw.WriteLine(str));

                    foreach (Provincie prov in provincies)
                    {
                        sw.WriteLine("\nStraatInfo voor provincie " + prov.provincieNaam + " :\n");
                        foreach (Gemeente gem in prov.gemeenteLijst)
                        {
                            if (!gem.TotaleLengte().Equals(0.ToString()))
                            {
                                sw.WriteLine("\t> " + gem.gemeenteNaam + " :" + gem.stratenLijst.Count.ToString() + " ,totale lengte:" + gem.TotaleLengte());

                                sw.WriteLine(gem.Getkortste_straat());
                                sw.WriteLine(gem.Getlangste_straat());
                            }
                        }
                    }
                }
            }
            catch (Exception Ex)
            {
                Console.WriteLine(Ex.ToString());
            }

            #endregion

            #region databestanden aanmaken vr databank, opslaan naar map Weg_geschreven_data

            //Ik wil er 4 maken = provincie, gemeente, straat, graaf bestand.
            // prov en gemeente zullen worden gelinkt met hun id's
            // gemeente en straat zullen worden gelinkt met hun id's
            // straat en graaf zullen worden gelinkt met hun id's

            #region werkend

            ////DEZE HIERONDER WERKT
            //gemeente: GemeenteId, Gemeentenaam, straatId
            string fileNameGemeenteB = @"..\..\..\Weg_geschreven_data\GemeenteBestand.txt";
            FileInfo fileLocGemeenteB = new FileInfo(fileNameGemeenteB);
            try
            {
                // Check if file already exists. If yes, delete it.
                if (fileLocGemeenteB.Exists)
                {
                    fileLocGemeenteB.Delete();
                }

                // Create a new file
                using (StreamWriter sw = File.CreateText(fileNameGemeenteB))
                {
                    #region Goedgekeurde Code

                    sw.WriteLine("GemeenteId;Gemeente_naam;(StraatId)");

                    #endregion Goedgekeurde Code

                    foreach (Provincie prov in provincies)
                    {
                        foreach (Gemeente gem in prov.gemeenteLijst)
                        {
                            sw.WriteLine(gem.gemeenteId + ";" + gem.gemeenteNaam + ";(" + gem.returnAlleStraatIds() + ")");
                        }
                    }
                }
            }
            catch (Exception Ex)
            {
                Console.WriteLine(Ex.ToString());
            }

            //DEZE HIERONDER WERKT
            //provincie: provincieID, provnaam, taalcode, gemeenteId
            string fileNameProvB = @"..\..\..\Weg_geschreven_data\ProvincieBestand.txt";
            FileInfo fileLocProvB = new FileInfo(fileNameProvB);
            try
            {
                // Check if file already exists. If yes, delete it.
                if (fileLocProvB.Exists)
                {
                    fileLocProvB.Delete();
                }

                // Create a new file
                using (StreamWriter sw = File.CreateText(fileNameProvB))
                {
                    sw.WriteLine("provincieID;Provnaam;taalcode;(gemeenteId)");

                    foreach (Provincie prov in provincies)
                    {
                        sw.WriteLine(prov.provincieId + ";" + prov.provincieNaam + ";" + prov.taalCodeProvincieNaam + ";(" + prov.GetGemeenteIds_voorDatabestand() + ")");
                    }
                }
            }
            catch (Exception Ex)
            {
                Console.WriteLine(Ex.ToString());
            }

            //straat: straatId, naam, lengte, graafID
            string fileNameStrB = @"..\..\..\Weg_geschreven_data\StraatBestand.txt";
            FileInfo fileLocStrB = new FileInfo(fileNameStrB);
            try
            {
                // Check if file already exists. If yes, delete it.
                if (fileLocStrB.Exists)
                {
                    fileLocStrB.Delete();
                }

                // Create a new file //waar lenght niet zero veranderen!
                using (StreamWriter sw = File.CreateText(fileNameStrB))
                {
                    sw.WriteLine("straatId;Straatnaam;lengte;graafID");
                    foreach (Straat s in gimmeStreets)
                    {
                        if (s.Length != 0)
                        {
                            sw.WriteLine(s.GetStraat_voorStraatDataBestand());
                        }
                    }
                }
            }
            catch (Exception Ex)
            {
                Console.WriteLine(Ex.ToString());
            }
            //graaf: *GraafId; KnoopId; & meerdere: knoop x punt; knoop y punt; (segmID;segm.beginknoop.knoopID;segm.eindknoop.knoopID)[(punt.x,punt.y)(punt.x,punt.y)...]
            string fileNameGraafB = @"..\..\..\Weg_geschreven_data\GraafBestand.txt";
            FileInfo fileLocGraafrB = new FileInfo(fileNameGraafB);
            try
            {
                // Check if file already exists. If yes, delete it.
                if (fileLocGraafrB.Exists)
                {
                    fileLocGraafrB.Delete();
                }

                // Create a new file
                using (StreamWriter sw = File.CreateText(fileNameGraafB))
                {
                    sw.WriteLine("*GraafId; KnoopId; knoop x punt; knoop y punt; (segmID;segm.beginknoop.knoopID;segm.eindknoop.knoopID)[(punt.x,punt.y)(punt.x,punt.y)...]");

                    provincies.ForEach(p =>
                             p.gemeenteLijst.ForEach(g =>
                                 g.stratenLijst.ForEach(straat =>
                                 {
                                     if (straat.graaf != null && straat.Length != 0)
                                     {
                                         if (straat.GetGraaF_voorDataBestand().Length > 1)
                                         {
                                             sw.WriteLine(straat.GetGraaF_voorDataBestand());
                                         }
                                     }
                                 }

                                 )));
                }
            }
            catch (Exception Ex)
            {
                Console.WriteLine(Ex.ToString());
            }

            #endregion werkend

            #endregion databestanden aanmaken vr databank, opslaan naar map Weg_geschreven_data

            stopWatch.Stop();
            long duration = stopWatch.ElapsedMilliseconds / 1000;
            Console.WriteLine("\nRunTime " + duration + " Elapsed seconds");

            Console.ReadLine(); //4271 sec = 64min
        }

        #region extraMethodes die ik gebruik voor rapport te schrijven

        public static void RapportGenereren(List<Provincie> provincies)
        {
            // StratenIntBerekenen_enAfdrukken(provincies);
            Console.WriteLine("Aantal Straten per provincie : \n");
            Provafdrukken(provincies);
        }

        public static String StratenIntBerekenen_enAfdrukken(List<Provincie> provincies)
        {
            int AantalStratenPro = 0;
            foreach (Provincie pr in provincies)
            {
                int x = 0;
                foreach (Gemeente gem in pr.gemeenteLijst)
                {
                    x += gem.stratenLijst.Count();
                }
                AantalStratenPro += x;
            }

            //653 straten werden niet terug gevonden voor de gemeentes, mss bestaan ze niet meer?
            return ("Totaal aantal straten: " + AantalStratenPro.ToString());
        }

        public static List<String> Provafdrukken(List<Provincie> provincies)
        {
            List<String> lijstje = new List<string>();
            foreach (Provincie pr in provincies)
            {
                //geeft gemeente en zijn aantal straten terug (niet op straatid gebaseerd maar List<straat> van elk gemeente

                int x = 0;
                foreach (Gemeente gem in pr.gemeenteLijst)
                {
                    x += gem.stratenLijst.Count();
                }

                lijstje.Add("\t> " + pr.provincieNaam.ToString() + " : " + x.ToString().ToString());
            }
            return lijstje;
        }

        #endregion extraMethodes die ik gebruik voor rapport te schrijven
    }
}