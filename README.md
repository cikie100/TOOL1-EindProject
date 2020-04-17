# TOOL1-EindProject

ProgramRapport bevat de Main om een rapport te maken en opslaan naar de map "Rapport", 
  en om gegevens bestanden weg te schrijven naar de map "Weg_geschreven_data".

De map "Model" heeft een hoop klasses die ik aangemaakt heb, onderandere:
*Provincie
*Gemeente
*Straat
*Graaf
*Segment
*Knoop
*Punt 

-Inlezen Bron bestand gebeurt in  DataBeh map------------------------------------------------------------------------------

  *getprovincieIdsVlaanderen() geeft List<String> terug met Provincie id's die moeten gebruikt worden.
  
  *getprovinciesList() geeft List <Provincie> terug met provincies en hun ( gemeenteId; provincieId; taalCodeProvincieNaam; provincieNaam)
          deze methonde bekijkt of de id in de lijst van getprovincieIdsVlaanderen() zit.
  
  *getGemeente() maakt gemeentes aan en geeft List<Gemeente> terug, 
        bevat de methode getStraatNaamId_gemeenteID(List<Gemeente> gemeente), die geeft List<String> stratenNaamId toe per gemeente.
        Hierdoor weet je welke staat(Id) tot welke gemeente hoort.
  
  *GeefStratenWegSegment(List<Straat> stratenLijst, List<Provincie> provincies) geeft al de straten die in provincie>gemeente>straat
        zitten een wegsegment terug. Wegsegmenten werden gemaakt door MaakSegmenten_vanDocument().
  
  *getStraatNamen(List<Provincie> provincies) geeft een List<Straat> terug, checkt eerst of een gemeente die stratenId bezit voor de
        straat word aangemaakt.
  
  *getStraatNaamId_gemeenteID(List<Gemeente> gemeentes), voegt straatnaamId's toe aan gepaste gemeente.
        wordt gebruikt door getGemeente();
  
  *MaakSegmenten_vanDocument() maakt segmenten lijst aan, wordt gebruikt door GeefStratenWegSegment(...)
