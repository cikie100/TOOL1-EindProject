using OpgaveLabo;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using Tool1.Databeh;
using Tool1.Model;

namespace Tool1
{
    public class ProgramRapport
    {
        private static Databeheer d = new Databeheer();

        private static void Main(string[] args)
        {
      
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();
           
            #region data en lists aanmaken

            List<Provincie> provincies = d.getprovinciesList(); //duurt 1 second

            //--Maakt lijst straten aan die zich in bestaande gemeente lijst bevinden
            List<Straat> gimmeStreets = d.getStraatNamen(provincies); //duurt 73 seconden

            //--Vult gemeente lijst met juiste <Straat>
            provincies.ForEach(p => p.gemeenteLijst.ForEach(g => g.Dezegemeente_stratenObjecten_gevenVanTekstLijst(gimmeStreets))); // duurt 54 seconden

            //Duurt heeeeeel erg lang, maakt graaf objecten aan voor de straten
            d.GeefStratenWegSegment(gimmeStreets, provincies);  //--

            #endregion data en lists aanmaken

            #region testen (mag verwijderd worden ljena)

            ////TRY
            //List<Provincie> provincies = new List<Provincie>();
            //provincies.Add(new Provincie("4", "nl", "Oost-Vlaanderen"));
            //provincies[0].gemeenteLijst.Add(new Gemeente("0", "1", "nl", "Aalst"));
            //provincies[0].gemeenteLijst[0].stratenLijst.Add(new Straat(1, "straat1"));
            //provincies[0].gemeenteLijst[0].stratenLijst.Add(new Straat(2, "straat2"));
            //provincies[0].gemeenteLijst[0].stratenLijst.Add(new Straat(3, "straat3"));
            //provincies[0].gemeenteLijst[0].stratenLijst.Add(new Straat(4, "straat4"));
            //provincies[0].gemeenteLijst[0].stratenLijst.Add(new Straat(5, "straat5"));
            //provincies[0].gemeenteLijst[0].stratenLijst.Add(new Straat(6, "straat6"));

            //provincies[0].gemeenteLijst[0].stratenLijst[0].Length = 0.0;
            //provincies[0].gemeenteLijst[0].stratenLijst[1].Length = 500;
            //provincies[0].gemeenteLijst[0].stratenLijst[2].Length = 100;
            //provincies[0].gemeenteLijst[0].stratenLijst[4].graaf = new Graaf(1, new List<Segment>(0));

            //Console.WriteLine(provincies[0].gemeenteLijst[0].Getkortste_straat());
            //STOP

            //Console.WriteLine(provincies[0].gemeenteLijst[0].Getkortste_straat());

            #endregion testen (mag verwijderd worden ljena)

            #region rapport aanmaken LUKT v

            //string fileName = @"..\..\..\Rapport\RapportBestand.txt";
            //FileInfo fileLoc = new FileInfo(fileName);
            //try
            //{
            //    // Check if file already exists. If yes, delete it.
            //    if (fileLoc.Exists)
            //    {
            //        fileLoc.Delete();
            //    }

            //    // Create a new file
            //    using (StreamWriter sw = File.CreateText(fileName))
            //    {
            //        sw.WriteLine(StratenIntBerekenen_enAfdrukken(provincies) + "\n");
            //        sw.WriteLine("Aantal Straten per provincie : \n");
            //        Provafdrukken(provincies).ForEach(str => sw.WriteLine(str));

            //        foreach (Provincie prov in provincies)
            //        {
            //            sw.WriteLine("\nStraatInfo voor provincie " + prov.provincieNaam + " :\n");
            //            foreach (Gemeente gem in prov.gemeenteLijst)
            //            {
            //                if (!gem.TotaleLengte().Equals(0.ToString()))
            //                {
            //                    sw.WriteLine("\t> " + gem.gemeenteNaam + " :" + gem.stratenLijst.Count.ToString() + " ,totale lengte:" + gem.TotaleLengte());

            //                    sw.WriteLine(gem.Getkortste_straat());
            //                    sw.WriteLine(gem.Getlangste_straat());
            //                }
            //            }
            //        }
            //    }
            //}
            //catch (Exception Ex)
            //{
            //    Console.WriteLine(Ex.ToString());
            //}

            #endregion rapport aanmaken

            #region databestand aanmaken vr databank

            //Ik wil er 4 maken = provincie, gemeente, straat, graaf bestand.
            #region werkend
            ////DEZE HIERONDER WERKT
            ////gemeente: GemeenteId, Gemeentenaam, straatId
            //string fileNameGemeenteB = @"..\..\..\Weg_geschreven_data\GemeenteBestand.txt";
            //FileInfo fileLocGemeenteB = new FileInfo(fileNameGemeenteB);
            //try
            //{
            //    // Check if file already exists. If yes, delete it.
            //    if (fileLocGemeenteB.Exists)
            //    {
            //        fileLocGemeenteB.Delete();
            //    }

            //    // Create a new file
            //    using (StreamWriter sw = File.CreateText(fileNameGemeenteB))
            //    {
            //        #region Goedgekeurde Code

            //        sw.WriteLine("StraatId;Gemeente_naam;(StraatId)");

            //        #endregion Goedgekeurde Code

            //        foreach (Provincie prov in provincies)
            //        {
            //            foreach (Gemeente gem in prov.gemeenteLijst)
            //            {
            //                sw.WriteLine(gem.gemeenteId + ";" + gem.gemeenteNaam + ";(" + gem.returnAlleStraatIds() + ")");
            //            }
            //        }
            //    }
            //}
            //catch (Exception Ex)
            //{
            //    Console.WriteLine(Ex.ToString());
            //}


            ////DEZE HIERONDER WERKT
            ////provincie: provincieID, provnaam, taalcode, gemeenteId
            //string fileNameProvB = @"..\..\..\Weg_geschreven_data\ProvincieBestand.txt";
            //FileInfo fileLocProvB = new FileInfo(fileNameProvB);
            //try
            //{
            //    // Check if file already exists. If yes, delete it.
            //    if (fileLocProvB.Exists)
            //    {
            //        fileLocProvB.Delete();
            //    }

            //    // Create a new file
            //    using (StreamWriter sw = File.CreateText(fileNameProvB))
            //    {
            //        sw.WriteLine("provincieID;Provnaam;taalcode;(gemeenteId)");

            //        foreach (Provincie prov in provincies)
            //        {
            //            sw.WriteLine(prov.provincieId + ";" + prov.provincieNaam + ";" + prov.taalCodeProvincieNaam + ";(" + prov.GetGemeenteIds_voorDatabestand() + ")");
            //        }
            //    }
            //}
            //catch (Exception Ex)
            //{
            //    Console.WriteLine(Ex.ToString());
            //}

            ////straat: straatId, naam, lengte, graafID
            //string fileNameStrB = @"..\..\..\Weg_geschreven_data\StraatBestand.txt";
            //FileInfo fileLocStrB = new FileInfo(fileNameStrB);
            //try
            //{
            //    // Check if file already exists. If yes, delete it.
            //    if (fileLocStrB.Exists)
            //    {
            //        fileLocStrB.Delete();
            //    }

            //    // Create a new file //waar lenght niet zero veranderen!
            //    using (StreamWriter sw = File.CreateText(fileNameStrB))
            //    {
            //        sw.WriteLine("straatId;Straatnaam;lengte;graafID");
            //        foreach (Straat s in gimmeStreets)
            //        {
            //            if (s.Length != 0)
            //            {
            //                sw.WriteLine(s.GetStraat_voorStraatDataBestand());
            //            }

            //        }
            //    }




            //}
            //catch (Exception Ex)
            //{
            //    Console.WriteLine(Ex.ToString());
            //}
            #endregion



            //graaf: graafId, knoop, segmenten
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
                    

                    sw.WriteLine("GraafId; KnoopId; knoop x punt; knoop y punt");

                   

                    provincies.ForEach(p =>
                             p.gemeenteLijst.ForEach(g =>
                                 g.stratenLijst.ForEach(straat =>
                                 {
                                     if(straat.GetGraaF_voorDataBestand().Length > 1) {
                                         sw.WriteLine(straat.GetGraaF_voorDataBestand());
                                     }
                                 }

                                 )));
                }
            }
            catch (Exception Ex)
            {
                Console.WriteLine(Ex.ToString());
            }



            //ALTERNATIEF
            //graaf: graafId, knoop, segmenten
            string fileNameGraaffB = @"..\..\..\Weg_geschreven_data\GraafKnoopSegmentBestand.txt";
            FileInfo fileLocGraaffrB = new FileInfo(fileNameGraaffB);
            try
            {
                // Check if file already exists. If yes, delete it.
                if (fileLocGraaffrB.Exists)
                {
                    fileLocGraaffrB.Delete();
                }

                // Create a new file
                using (StreamWriter sw = File.CreateText(fileNameGraaffB))
                {


                    sw.WriteLine("GraafId; KnoopId; knoop x punt; knoop y punt");
                    provincies.ForEach(p =>
                             p.gemeenteLijst.ForEach(g =>
                                 g.stratenLijst.ForEach(straat =>
                                 {
                                     if (straat.GetGraaF_voorDataBestand().Length > 1)
                                     {
                                         sw.WriteLine(straat.GetGraaF_voorDataBestandd());
                                     }
                                 }

                                 )));

                }


                    
                
            }
            catch (Exception Ex)
            {
                Console.WriteLine(Ex.ToString());
            }



            #endregion databestand aanmaken vr databank

            stopWatch.Stop();
            long duration = stopWatch.ElapsedMilliseconds / 1000;
              Console.WriteLine("\nRunTime " + duration + " Elapsed seconds");

            Console.ReadLine(); //4121 sec =
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
                //geeft gemeenteLijst count terug
                //int x = pr.gemeenteLijst.Count(g => g.stratenNaamId.Any());

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