using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
public enum OrientacjaPolaczenia
{
    pozioma,
    pionowa
}
public class Dok
{
    public bool hierarchia1;
    public bool hierarchia2;
    public Dok()
    {
        hierarchia1 = false;
        hierarchia2 = false;
    }
}
public class Polaczenia
{
    public Dok gorny;
    public Dok dolny;
    public Dok lewy;
    public Dok prawy;
    public int gora()
    {
        int suma = 0;
        if (gorny.hierarchia1)
            suma++;
        if (gorny.hierarchia2)
            suma++;
        return suma;
    }
    public int dol()
    {
        int suma = 0;
        if (dolny.hierarchia1)
            suma++;
        if (dolny.hierarchia2)
            suma++;
        return suma;
    }
    public int lewo()
    {
        int suma = 0;
        if (lewy.hierarchia1)
            suma++;
        if (lewy.hierarchia2)
            suma++;
        return suma;
    }
    public int prawo()
    {
        int suma = 0;
        if (prawy.hierarchia1)
            suma++;
        if (prawy.hierarchia2)
            suma++;
        return suma;
    }
    public Polaczenia(Dok _gorny, Dok _dolny, Dok _lewy, Dok _prawy)
    {
        gorny = _gorny;
        dolny = _dolny;
        lewy = _lewy;
        prawy = _prawy;
    }
}
public class Kolko
{
    public int numer;
    public int stopienMaksymalny;
    public int wiersz;
    public int kolumna;
    public Polaczenia polaczenia;
    public int stopienAktualny()
    {
        return polaczenia.gora() + polaczenia.dol() + polaczenia.lewo() + polaczenia.prawo();
    }
    public Kolko(int _numer, int _stopienMaksymalny, int _wiersz, int _kolumna, Polaczenia _polaczenia)
    {
        numer = _numer;
        stopienMaksymalny = _stopienMaksymalny;
        wiersz = _wiersz;
        kolumna = _kolumna;
        polaczenia = _polaczenia;
    }
}
public class Linia
{
    // nazwa w formacie
    // linia_*_* (*x*)
    // gdzie gwiazdki to kolejno: 
    // pionowa/pozioma, 1/2, numer kolka1, numer kolka2
    // numery w nazwie liczone od 1
    public string nazwa;
    public int kolko1Numer;
    public int kolko2Numer;
    public int hierarchia;  // 1 lub 2
    public void wyluskajNumeryKolekZNazwy()
    {
        string razem = nazwa.Substring(nazwa.IndexOf('(') + 1, nazwa.IndexOf(')') - nazwa.IndexOf('(') - 1);
        kolko1Numer = int.Parse(razem.Substring(0, razem.IndexOf('x'))) - 1;
        kolko2Numer = int.Parse(razem.Substring(razem.IndexOf('x') + 1)) - 1;
    }
    public void wyluskajHierarchieZNazwy()
    {
        hierarchia = int.Parse(nazwa.Substring(nazwa.LastIndexOf('_') + 1, 1));
    }
    public OrientacjaPolaczenia orientacja()
    {
        if (kolko2Numer == kolko1Numer + 1)
            return OrientacjaPolaczenia.pozioma;
        else if (kolko2Numer == kolko1Numer + 4)
            return OrientacjaPolaczenia.pionowa;
        else
            throw new Exception();
    }
    public Linia(string _nazwa)
    {
        nazwa = _nazwa;
        wyluskajHierarchieZNazwy();
        wyluskajNumeryKolekZNazwy();
    }
}
public static class Narzedzia
{
    public enum Kolor
    {
        BIALY,
        WYBRANY,
        POMARANCZOWY1,
        POMARANCZOWY2,
        POMARANCZOWY3,
        POMARANCZOWY4,
        POMARANCZOWY5,
        POMARANCZOWY6,
        POMARANCZOWY7,
        POMARANCZOWY8
    }
    public static Dictionary<int, Kolor> StopnieNaKolory = new Dictionary<int, Kolor>()
    {
        { 0, Kolor.BIALY },
        { 1, Kolor.POMARANCZOWY1 },
        { 2, Kolor.POMARANCZOWY2 },
        { 3, Kolor.POMARANCZOWY3 },
        { 4, Kolor.POMARANCZOWY4 },
        { 5, Kolor.POMARANCZOWY5 },
        { 6, Kolor.POMARANCZOWY6 },
        { 7, Kolor.POMARANCZOWY7 },
        { 8, Kolor.POMARANCZOWY8 }
    };
    public static Dictionary<Kolor, Color32> KoloryNaColor32 = new Dictionary<Kolor, Color32>()
    {
        { Kolor.BIALY, new Color32(255,255,255,255) },
        { Kolor.WYBRANY, new Color32(78, 145, 82, 255) },
        { Kolor.POMARANCZOWY1, new Color32(235, 200, 140, 255) },
        { Kolor.POMARANCZOWY2, new Color32(230, 180, 125, 255) },
        { Kolor.POMARANCZOWY3, new Color32(225, 160, 110, 255) },
        { Kolor.POMARANCZOWY4, new Color32(220, 130, 95, 255) },
        { Kolor.POMARANCZOWY5, new Color32(215, 120, 80, 255) },
        { Kolor.POMARANCZOWY6, new Color32(210, 100, 65, 255) },
        { Kolor.POMARANCZOWY7, new Color32(205, 80, 50, 255) },
        { Kolor.POMARANCZOWY8, new Color32(200, 60, 35, 255) }
    };
}
public class KierownikGry : MonoBehaviour
{
    public List<GameObject> kolkaGame;
    public List<GameObject> linieGame;
    public List<Kolko> kolkaObiekty;
    public List<Linia> linieObiekty;
    public int numerAktywnegoPoziomu;
    public List<int[,]> poziomy;
    public int[,] aktywnyPoziom;
    public Kolko wybraneKolko;
    public int docelowaLiczbaPolaczen;
    public int aktualnaLiczbaPolaczen;
    // Start is called before the first frame update
    void Start()
    {
        // Zalkaduj poziom - zawartosci kolek
        numerAktywnegoPoziomu = 0;
        poziomy = zaladujListePoziomow();
        aktywnyPoziom = poziomy[numerAktywnegoPoziomu];
        //
        wybraneKolko = null;
        docelowaLiczbaPolaczen = 0;
        aktualnaLiczbaPolaczen = 0;
        // Utworz liste kolek-obiektow
        kolkaObiekty = new List<Kolko>();
        for (int i = 0; i < 6; i++)
        {
            for (int j = 0; j < 4; j++)
            {
                int indeks_kolka = 4 * i + j;
                kolkaObiekty.Add(new Kolko(indeks_kolka, 0, i, j, new Polaczenia(new Dok(), new Dok(), new Dok(), new Dok())));
                kolkaGame[indeks_kolka].GetComponent<SpriteRenderer>().color = Narzedzia.KoloryNaColor32[Narzedzia.StopnieNaKolory[kolkaObiekty[indeks_kolka].stopienMaksymalny]];
                var textObject = kolkaGame[indeks_kolka].transform.GetChild(1).gameObject;
                textObject.GetComponent<TextMeshPro>().text = "0";
                kolkaGame[indeks_kolka].SetActive(false);
            }
        }
        // Utworz liste linii-obiektow
        linieObiekty = new List<Linia>();
        foreach (var i in linieGame)
        {
            linieObiekty.Add(new Linia(i.name));
            i.SetActive(false);
        }
        zaladujAktywnyPoziom();
    }
    // Update is called once per frame
    void Update()
    {
        //Debug.Log(docelowaLiczbaPolaczen + " " + aktualnaLiczbaPolaczen);
    }
    List<int[,]> zaladujListePoziomow()
    {
        List<int[,]> poziomy = new List<int[,]>();
        int[,] poziom0 = { { 0, 0, 0, 0 }, { 0, 3, 0, 2 }, { 0, 0, 0, 0 }, { 0, 2, 1, 0 }, { 0, 0, 0, 0 }, { 0, 0, 0, 0 } };
        int[,] poziom1 = { { 0, 0, 0, 0 }, { 3, 2, 0, 0 }, { 2, 0, 0, 1 }, { 0, 2, 0, 2 }, { 0, 0, 0, 0 }, { 0, 0, 0, 0 } };
        int[,] poziom2 = { { 0, 3, 0, 4 }, { 1, 4, 0, 3 }, { 0, 0, 0, 0 }, { 0, 6, 4, 4 }, { 1, 4, 0, 3 }, { 0, 0, 1, 2 } };
        //poziomy.Add(poziom0);
        poziomy.Add(poziom1);
        //poziomy.Add(poziom2);
        return poziomy;
    }
    public void kliknietoKolko(string kolkoGame_nazwa)
    {
        int indeks = kolkaGame.FindIndex(x => x.name == kolkoGame_nazwa);
        Kolko kliknieteKolkoObiekt = kolkaObiekty[indeks];
        //Debug.Log(kliknieteKolkoObiekt.numer + " " + kliknieteKolkoObiekt.polaczenia.gora() + " " + kliknieteKolkoObiekt.polaczenia.dol() + " " + kliknieteKolkoObiekt.polaczenia.lewo() + " " + kliknieteKolkoObiekt.polaczenia.prawo());
        if (wybraneKolko is null)
            ustawWybraneKolko(kliknieteKolkoObiekt);
        else if (wybraneKolko == kliknieteKolkoObiekt)
            zresetujWybraneKolko();
        else
            sprobojPolaczycKolka(kliknieteKolkoObiekt);
    }
    void ustawWybraneKolko(Kolko k)
    {
        wybraneKolko = k;
        kolkaGame[k.numer].GetComponent<SpriteRenderer>().color = Narzedzia.KoloryNaColor32[Narzedzia.Kolor.WYBRANY];
    }
    void zresetujWybraneKolko()
    {
        kolkaGame[wybraneKolko.numer].GetComponent<SpriteRenderer>().color = Narzedzia.KoloryNaColor32[Narzedzia.StopnieNaKolory[wybraneKolko.stopienMaksymalny]];
        wybraneKolko = null;
    }
    void sprobojPolaczycKolka(Kolko k)
    {
        Kolko kolko1 = wybraneKolko;
        Kolko kolko2 = k;
        if (kolko2.numer < kolko1.numer)
        {
            kolko1 = k;
            kolko2 = wybraneKolko;
        }
        bool mozna_polaczyc = false;
        if (kolko1.stopienAktualny() < kolko1.stopienMaksymalny && kolko2.stopienAktualny() < kolko2.stopienMaksymalny)
        {
            if (kolko1.wiersz == kolko2.wiersz && NieMaKolekPomiedzy(kolko1,kolko2)) // poziomo i nic pomiedzy
            {
                if (kolko1.polaczenia.prawo() < 2 && kolko2.polaczenia.lewo() < 2)
                {
                    mozna_polaczyc = true;
                    polaczKolka(kolko1,kolko2,OrientacjaPolaczenia.pozioma);
                }
            }
            else if (kolko1.kolumna == kolko2.kolumna && NieMaKolekPomiedzy(kolko1, kolko2))
            {
                if (kolko1.polaczenia.dol() < 2 && kolko2.polaczenia.gora() < 2)
                {
                    mozna_polaczyc = true;
                    polaczKolka(kolko1, kolko2,OrientacjaPolaczenia.pionowa);
                }
            }
        }
        if (!mozna_polaczyc)
        {
            zresetujWybraneKolko();
            // odtworzDzwiekBledu();
        }
    }
    bool NieMaKolekPomiedzy(Kolko kolko1, Kolko kolko2)
    {
        bool cos_pomiedzy = false;
        if (kolko1.wiersz == kolko2.wiersz) // poziomo
        {
            for (int i=kolko1.kolumna+1; i<kolko2.kolumna; i++)
            {
                int numer = 4 * kolko1.wiersz + i;
                //Debug.Log("poziom" + numer + " " + kolkaObiekty[numer].stopienMaksymalny+ " "+ kolkaObiekty[numer].polaczenia.gora);
                if (!(kolkaObiekty[numer].stopienMaksymalny == 0 && kolkaObiekty[numer].polaczenia.gora()==0))
                {
                    cos_pomiedzy = true;
                    break;
                }
            }
        }
        else if (kolko1.kolumna == kolko2.kolumna) // pionowo
        {
            for (int i = kolko1.wiersz + 1; i < kolko2.wiersz; i++)
            {
                int numer = 4 * i + kolko1.kolumna;
                //Debug.Log("pion "+ numer + " " + kolkaObiekty[numer].stopienMaksymalny+ " "+ kolkaObiekty[numer].polaczenia.lewo);
                if (!(kolkaObiekty[numer].stopienMaksymalny == 0 && kolkaObiekty[numer].polaczenia.lewo() == 0))
                {
                    cos_pomiedzy = true;
                    break;
                }
            }
        }
        return !cos_pomiedzy;
    }
    void polaczKolka(Kolko kolko1, Kolko kolko2, OrientacjaPolaczenia orientacja)
    {
        var hierarchia = wyznaczHierarchie(kolko1, orientacja);
        aktywujLinie(hierarchia, kolko1.numer, kolko2.numer, orientacja);
        zwiekszLiczbePolaczen(kolko1, kolko2, orientacja, hierarchia);
        zwiekszAktualnaLiczbePolaczen();
        zresetujWybraneKolko();
    }
    void zwiekszLiczbePolaczen(Kolko k1, Kolko k2, OrientacjaPolaczenia o, int hierarchia_polaczenia)
    {
        if (o == OrientacjaPolaczenia.pozioma)
        {
            if (hierarchia_polaczenia == 1)
            {
                k1.polaczenia.prawy.hierarchia1 = true;
                k2.polaczenia.lewy.hierarchia1 = true;
            }
            else if (hierarchia_polaczenia == 2)
            {
                k1.polaczenia.prawy.hierarchia2 = true;
                k2.polaczenia.lewy.hierarchia2 = true;
            }
        }
        else    //if (o == OrientacjaPolaczenia.pionowa)
        {
            if (hierarchia_polaczenia == 1)
            {
                k1.polaczenia.dolny.hierarchia1 = true;
                k2.polaczenia.gorny.hierarchia1 = true;
            }
            else if (hierarchia_polaczenia == 2)
            {
                k1.polaczenia.dolny.hierarchia2 = true;
                k2.polaczenia.gorny.hierarchia2 = true;
            }
        }
    }
    IEnumerator zakonczPoziom()
    {
        Debug.Log("plansza rozwiazana");
        this.GetComponent<AudioSource>().Play();
        yield return new WaitForSecondsRealtime(0.5f);
        numerAktywnegoPoziomu++;
        if (numerAktywnegoPoziomu < poziomy.Count)
        {
            zresetujPoziom();
            aktywnyPoziom = poziomy[numerAktywnegoPoziomu];
            zaladujAktywnyPoziom();
        }
        else
        {
            //zresetujPoziom();
            wypelnijPoziomGenerowanym();
        }
    }
    void zresetujPoziom()
    {
        wybraneKolko = null;
        docelowaLiczbaPolaczen = 0;
        aktualnaLiczbaPolaczen = 0;
        foreach (var k in kolkaObiekty)
        {
            k.stopienMaksymalny = 0;
            k.polaczenia = new Polaczenia(new Dok(), new Dok(), new Dok(), new Dok());
            kolkaGame[k.numer].GetComponent<SpriteRenderer>().color = Narzedzia.KoloryNaColor32[Narzedzia.StopnieNaKolory[kolkaObiekty[k.numer].stopienMaksymalny]];
        }
        foreach (var k in kolkaGame)
        {
            k.SetActive(false);
            var textObject = k.transform.GetChild(1).gameObject;
            textObject.GetComponent<TextMeshPro>().text = "0";
        }
        foreach (var l in linieGame.FindAll(x => x.activeInHierarchy))
        {
            l.SetActive(false);
        }
    }
    void zaladujAktywnyPoziom()
    {
        // zakladamy ze wszystko jest na null/0/nieaktywne
        wybraneKolko = null;
        docelowaLiczbaPolaczen = 0;
        aktualnaLiczbaPolaczen = 0;
        for (int i = 0; i < 6; i++)
        {
            for (int j = 0; j < 4; j++)
            {
                int indeks_kolka = 4 * i + j;
                int zawartosc_kolka = aktywnyPoziom[i, j];
                docelowaLiczbaPolaczen += zawartosc_kolka;
                kolkaObiekty[indeks_kolka].stopienMaksymalny = zawartosc_kolka;
                kolkaGame[indeks_kolka].GetComponent<SpriteRenderer>().color = Narzedzia.KoloryNaColor32[Narzedzia.StopnieNaKolory[kolkaObiekty[indeks_kolka].stopienMaksymalny]];
                if (zawartosc_kolka != 0)
                {
                    kolkaGame[indeks_kolka].SetActive(true);
                    var textObject = kolkaGame[indeks_kolka].transform.GetChild(1).gameObject;
                    textObject.GetComponent<TextMeshPro>().text = zawartosc_kolka.ToString();
                }
            }
        }
    }
    void zwiekszAktualnaLiczbePolaczen()
    {
        aktualnaLiczbaPolaczen += 2;
        if (aktualnaLiczbaPolaczen == docelowaLiczbaPolaczen)
            StartCoroutine(zakonczPoziom());
    }
    void aktywujLinie(int hierarchia, int kolko1_numer, int kolko2_numer, OrientacjaPolaczenia orientacja)
    {
        if (orientacja == OrientacjaPolaczenia.pozioma)
            aktywujLiniePoziomo(hierarchia, kolko1_numer, kolko2_numer);
        else //if (orientacja == OrientacjaPolaczenia.pionowa)
            aktywujLiniePionowo(hierarchia, kolko1_numer, kolko2_numer);
    }
    void aktywujLiniePoziomo(int hierarchia, int kolko1_numer, int kolko2_numer)
    {
        for (int i = kolko1_numer; i < kolko2_numer; i++)
        {
            Linia liniaObiekt = linieObiekty.Find(x => x.hierarchia == hierarchia && x.kolko1Numer == i && x.kolko2Numer == i + 1);
            GameObject liniaGame = linieGame.Find(x => x.name == liniaObiekt.nazwa);
            liniaGame.SetActive(true);
            //Debug.Log("dodalem "+ liniaGame.name);
            // 
            if (i>kolko1_numer)     // (!kolkaGame[i].activeInHierarchy)
            {
                if (hierarchia == 1)
                {
                    kolkaObiekty[i].polaczenia.prawy.hierarchia1 = true;
                    kolkaObiekty[i].polaczenia.lewy.hierarchia1 = true;
                }
                else if (hierarchia == 2)
                {
                    kolkaObiekty[i].polaczenia.prawy.hierarchia2 = true;
                    kolkaObiekty[i].polaczenia.lewy.hierarchia2 = true;
                }
            }
        }
    }
    void aktywujLiniePionowo(int hierarchia, int kolko1_numer, int kolko2_numer)
    {
        for (int i = kolko1_numer; i < kolko2_numer; i+=4)
        {
            Linia liniaObiekt = linieObiekty.Find(x => x.hierarchia == hierarchia && x.kolko1Numer == i && x.kolko2Numer == i + 4);
            GameObject liniaGame = linieGame.Find(x => x.name == liniaObiekt.nazwa);
            liniaGame.SetActive(true);
            //Debug.Log("dodalem " + liniaGame.name);
            //
            if (i>kolko1_numer)
            {
                if (hierarchia == 1)
                {
                    kolkaObiekty[i].polaczenia.gorny.hierarchia1 = true;
                    kolkaObiekty[i].polaczenia.dolny.hierarchia1 = true;
                }
                else if (hierarchia == 2)
                {
                    kolkaObiekty[i].polaczenia.gorny.hierarchia2 = true;
                    kolkaObiekty[i].polaczenia.dolny.hierarchia2 = true;
                }
            }
        }
    }
    int wyznaczHierarchie(Kolko k, OrientacjaPolaczenia o)
    {
        // zakladamy ze polaczen jest <2
        if (o == OrientacjaPolaczenia.pozioma)
        {
            if (k.polaczenia.prawo() == 0)
                return 1;
            else    // jest jedno polaczenie
            {
                if (k.polaczenia.prawy.hierarchia1)
                    return 2;
                else
                    return 1;
            }
        }
        else    // pionowo
        {
            if (k.polaczenia.dol() == 0)
                return 1;
            else
            {
                if (k.polaczenia.dolny.hierarchia1)
                    return 2;
                else
                    return 1;
            }
        }
    }
    public void kliknietoLinie(string liniaGame_nazwa)
    {
        Linia kliknietaLiniaObiekt = linieObiekty.Find(x => x.nazwa == liniaGame_nazwa);
        dezaktywujLinieOrazZmniejszLiczbePolaczen(kliknietaLiniaObiekt);
        zmniejszAktualnaLiczbePolaczen();
    }
    void dezaktywujLinieOrazZmniejszLiczbePolaczen(Linia linia)
    {
        if (linia.orientacja() == OrientacjaPolaczenia.pozioma)
            dezaktywujLiniePoziomoOrazZmniejszLiczbePolaczen(linia);
        else
            dezaktywujLiniePionowoOrazZmniejszLiczbePolaczen(linia);
    }
    void dezaktywujLiniePoziomoOrazZmniejszLiczbePolaczen(Linia linia)
    {
        linieGame.Find(x => x.name == linia.nazwa).SetActive(false);
        int i = linia.kolko1Numer;
        Kolko kolko_lewe = kolkaObiekty[i];
        while (kolko_lewe.stopienMaksymalny<=0)
        {
            Linia pom1 = linieObiekty.Find(x => x.hierarchia == linia.hierarchia && x.kolko1Numer == i - 1 && x.kolko2Numer == i);
            linieGame.Find(x => x.name == pom1.nazwa).SetActive(false);
            //
            if (linia.hierarchia == 1)
            {
                kolko_lewe.polaczenia.prawy.hierarchia1 = false;
                kolko_lewe.polaczenia.lewy.hierarchia1 = false;
            }
            else if (linia.hierarchia == 2)
            {
                kolko_lewe.polaczenia.prawy.hierarchia2 = false;
                kolko_lewe.polaczenia.lewy.hierarchia2 = false;
            }
            //
            i--;
            kolko_lewe = kolkaObiekty[i];
        }
        //
        i = linia.kolko2Numer;
        Kolko kolko_prawe = kolkaObiekty[i];
        while (kolko_prawe.stopienMaksymalny <= 0)
        {
            Linia pom1 = linieObiekty.Find(x => x.hierarchia == linia.hierarchia && x.kolko1Numer == i && x.kolko2Numer == i+1);
            linieGame.Find(x => x.name == pom1.nazwa).SetActive(false);
            //
            if (linia.hierarchia == 1)
            {
                kolko_prawe.polaczenia.prawy.hierarchia1 = false;
                kolko_prawe.polaczenia.lewy.hierarchia1 = false;
            }
            else if (linia.hierarchia == 2)
            {
                kolko_prawe.polaczenia.prawy.hierarchia2 = false;
                kolko_prawe.polaczenia.lewy.hierarchia2 = false;
            }
            //
            i++;
            kolko_prawe = kolkaObiekty[i];
        }
        //
        zmniejszLiczbePolaczen(kolko_lewe, kolko_prawe, linia.orientacja(), linia.hierarchia);
    }
    void dezaktywujLiniePionowoOrazZmniejszLiczbePolaczen(Linia linia)
    {
        linieGame.Find(x => x.name == linia.nazwa).SetActive(false);
        int i = linia.kolko1Numer;
        Kolko kolko_gorne = kolkaObiekty[i];
        while (kolko_gorne.stopienMaksymalny <= 0)
        {
            Linia pom1 = linieObiekty.Find(x => x.hierarchia == linia.hierarchia && x.kolko1Numer == i - 4 && x.kolko2Numer == i);
            linieGame.Find(x => x.name == pom1.nazwa).SetActive(false);
            //
            if (linia.hierarchia == 1)
            {
                kolko_gorne.polaczenia.dolny.hierarchia1 = false;
                kolko_gorne.polaczenia.gorny.hierarchia1 = false;
            }
            else if (linia.hierarchia == 2)
            {
                kolko_gorne.polaczenia.dolny.hierarchia2 = false;
                kolko_gorne.polaczenia.gorny.hierarchia2 = false;
            }
            //
            i -=4;
            kolko_gorne = kolkaObiekty[i];
        }
        //
        i = linia.kolko2Numer;
        Kolko kolko_dolne = kolkaObiekty[i];
        while (kolko_dolne.stopienMaksymalny <= 0)
        {
            Linia pom1 = linieObiekty.Find(x => x.hierarchia == linia.hierarchia && x.kolko1Numer == i && x.kolko2Numer == i + 4);
            linieGame.Find(x => x.name == pom1.nazwa).SetActive(false);
            //
            if (linia.hierarchia == 1)
            {
                kolko_dolne.polaczenia.dolny.hierarchia1 = false;
                kolko_dolne.polaczenia.gorny.hierarchia1 = false;
            }
            else if (linia.hierarchia == 2)
            {
                kolko_dolne.polaczenia.dolny.hierarchia2 = false;
                kolko_dolne.polaczenia.gorny.hierarchia2 = false;
            }
            //
            i +=4;
            kolko_dolne = kolkaObiekty[i];
        }
        //
        zmniejszLiczbePolaczen(kolko_gorne, kolko_dolne, linia.orientacja(), linia.hierarchia);
    }
    void zmniejszLiczbePolaczen(Kolko k1, Kolko k2, OrientacjaPolaczenia o, int hierarchia_polaczenia)
    {
        if (o == OrientacjaPolaczenia.pozioma)
        {
            if (hierarchia_polaczenia == 1)
            {
                k1.polaczenia.prawy.hierarchia1 = false;
                k2.polaczenia.lewy.hierarchia1 = false;
            }
            else if (hierarchia_polaczenia == 2)
            {
                k1.polaczenia.prawy.hierarchia2 = false;
                k2.polaczenia.lewy.hierarchia2 = false;
            }
        }
        else    //if (o == OrientacjaPolaczenia.pionowa)
        {
            if (hierarchia_polaczenia == 1)
            {
                k1.polaczenia.dolny.hierarchia1 = false;
                k2.polaczenia.gorny.hierarchia1 = false;
            }
            else if (hierarchia_polaczenia == 2)
            {
                k1.polaczenia.dolny.hierarchia2 = false;
                k2.polaczenia.gorny.hierarchia2 = false;
            }
        }
    }
    void zmniejszAktualnaLiczbePolaczen()
    {
        aktualnaLiczbaPolaczen -= 2;
    }
    //
    void wypelnijPoziomGenerowanym()
    {
        wybraneKolko = null;
        docelowaLiczbaPolaczen = -1;
        aktualnaLiczbaPolaczen = 0;
        foreach (var k in kolkaObiekty)
        {
            k.stopienMaksymalny = 8;
            k.polaczenia = new Polaczenia(new Dok(), new Dok(), new Dok(), new Dok());
            kolkaGame[k.numer].GetComponent<SpriteRenderer>().color = Narzedzia.KoloryNaColor32[Narzedzia.StopnieNaKolory[kolkaObiekty[k.numer].stopienMaksymalny]];
        }
        foreach (var k in kolkaGame)
        {
            k.SetActive(true);
        }
        foreach (var l in linieGame)
        {
            l.SetActive(false);
        }
        int iteracje = 0;
        int udane = 0;
        var rnd = new System.Random();
        while (udane <= 5 && iteracje <= 30)
        {
            int orientacja = rnd.Next(2);
            if (orientacja == 0)
            {
                int wiersz = rnd.Next(6);
                List<int> zbior = new List<int>(){ 0, 1, 2, 3 };
                int kolumna1 = zbior[rnd.Next(zbior.Count)];
                zbior.Remove(kolumna1);
                int kolumna2 = zbior[rnd.Next(zbior.Count)];
                int numer1 = 4 * wiersz + kolumna1;
                int numer2 = 4 * wiersz + kolumna2;
                if (numer2<numer1)
                {
                    int pom = numer2;
                    numer2 = numer1;
                    numer1 = pom;
                }
                if (sprobojPolaczycKolkaDoGenerowania(kolkaObiekty[numer1], kolkaObiekty[numer2], rnd.Next(1,3)))
                {
                    udane++;
                    //Debug.Log(orientacja);
                }
                Debug.Log(numer1 + " " + numer2);
            }
            else
            {
                int kolumna = rnd.Next(4);
                List<int> zbior = new List<int>() { 0, 1, 2, 3 };
                int wiersz1 = zbior[rnd.Next(zbior.Count)];
                zbior.Remove(wiersz1);
                int wiersz2 = zbior[rnd.Next(zbior.Count)];
                //Debug.Log("wiersze "+wiersz1 + " " + wiersz2);
                //Debug.Log(zbior.Count);
                int numer1 = 4 * wiersz1 + kolumna;
                int numer2 = 4 * wiersz2 + kolumna;
                if (numer2 < numer1)
                {
                    int pom = numer2;
                    numer2 = numer1;
                    numer1 = pom;
                }
                //Debug.Log("numery " + numer1 + " " + numer2);
                if (sprobojPolaczycKolkaDoGenerowania(kolkaObiekty[numer1], kolkaObiekty[numer2], rnd.Next(1, 3)))
                {
                    udane++;
                    //Debug.Log(orientacja);
                }
                Debug.Log(numer1 + " " + numer2);
            }
            //Debug.Log(udane);
            iteracje++;
        }
        docelowaLiczbaPolaczen = 0;
        aktualnaLiczbaPolaczen = 0;
        foreach (var k in kolkaObiekty)
        {
            //Debug.Log(k.stopienMaksymalny);
            k.stopienMaksymalny = k.stopienAktualny();
            docelowaLiczbaPolaczen += k.stopienMaksymalny;
            k.polaczenia = new Polaczenia(new Dok(), new Dok(), new Dok(), new Dok());
            kolkaGame[k.numer].GetComponent<SpriteRenderer>().color = Narzedzia.KoloryNaColor32[Narzedzia.StopnieNaKolory[kolkaObiekty[k.numer].stopienMaksymalny]];
            var textObject = kolkaGame[k.numer].transform.GetChild(1).gameObject;
            textObject.GetComponent<TextMeshPro>().text = k.stopienMaksymalny.ToString();
            if (k.stopienMaksymalny <= 0)
                kolkaGame[k.numer].SetActive(false);
        }
        foreach (var l in linieGame.FindAll(x => x.activeInHierarchy))
        {
            l.SetActive(false);
        }
    }
    bool sprobojPolaczycKolkaDoGenerowania(Kolko kolko1, Kolko kolko2, int ilosc_razy)
    {
        bool mozna_polaczyc = false;
        if (kolko1.stopienAktualny() < kolko1.stopienMaksymalny && kolko2.stopienAktualny() < kolko2.stopienMaksymalny)
        {
            if (kolko1.wiersz == kolko2.wiersz && NieMaKolekPomiedzyDoGenerowania(kolko1, kolko2)) // poziomo i nic pomiedzy
            {
                if (kolko1.polaczenia.prawo() == 0 && kolko2.polaczenia.lewo() == 0)
                {
                    mozna_polaczyc = true;
                    polaczKolkaDoGenerowania(kolko1, kolko2, OrientacjaPolaczenia.pozioma);
                    if (ilosc_razy>1)
                        polaczKolkaDoGenerowania(kolko1, kolko2, OrientacjaPolaczenia.pozioma);
                }
            }
            else if (kolko1.kolumna == kolko2.kolumna && NieMaKolekPomiedzyDoGenerowania(kolko1, kolko2))
            {
                if (kolko1.polaczenia.dol() == 0 && kolko2.polaczenia.gora() == 0)
                {
                    mozna_polaczyc = true;
                    polaczKolkaDoGenerowania(kolko1, kolko2, OrientacjaPolaczenia.pionowa);
                    if (ilosc_razy > 1)
                        polaczKolkaDoGenerowania(kolko1, kolko2, OrientacjaPolaczenia.pionowa);
                }
            }
        }
        return mozna_polaczyc;
    }
    void polaczKolkaDoGenerowania(Kolko kolko1, Kolko kolko2, OrientacjaPolaczenia orientacja)
    {
        var hierarchia = wyznaczHierarchie(kolko1, orientacja);
        aktywujLinie(hierarchia, kolko1.numer, kolko2.numer, orientacja);
        zwiekszLiczbePolaczen(kolko1, kolko2, orientacja, hierarchia);
        zwiekszAktualnaLiczbePolaczen();
    }
    bool NieMaKolekPomiedzyDoGenerowania(Kolko kolko1, Kolko kolko2)
    {
        bool cos_pomiedzy = false;
        if (kolko1.wiersz == kolko2.wiersz) // poziomo
        {
            for (int i = kolko1.kolumna + 1; i < kolko2.kolumna; i++)
            {
                int numer = 4 * kolko1.wiersz + i;
                Debug.Log("poziom" + numer + " " + kolkaObiekty[numer].stopienAktualny());
                if (kolkaObiekty[numer].stopienAktualny()>0)
                {
                    cos_pomiedzy = true;
                    break;
                }
            }
        }
        else if (kolko1.kolumna == kolko2.kolumna) // pionowo
        {
            for (int i = kolko1.wiersz + 1; i < kolko2.wiersz; i++)
            {
                int numer = 4 * i + kolko1.kolumna;
                Debug.Log("pion "+ numer + " " + kolkaObiekty[numer].stopienAktualny());
                if (kolkaObiekty[numer].stopienAktualny() > 0)
                {
                    cos_pomiedzy = true;
                    break;
                }
            }
        }
        return !cos_pomiedzy;
    }
}