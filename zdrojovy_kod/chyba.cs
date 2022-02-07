using System;

namespace vycislovac_rovnic
{
    /// <summary>
    ///     Datový typ CHYBA slouží k předávání výjimek, které
    ///     vznikají ve funkcích projektu VycislovacRovnic.
    /// </summary>
    public class CHYBA: Exception
    {
        /// <summary>
        ///     Oznamuje pokus o vytvoření matice o nulovém počtu řádků nebo sloupců.
        /// </summary>
        /// <remarks>
        ///     Týká se funkcí:
        ///         MATICE.MATICE(MATICE);
        ///         MATICE.MATICE(int[,]);
        ///         MATICE.MATICE(int, int);
        /// </remarks>
        public const int Prazdna_matice = 0;
        
        /// <summary>
        ///     Oznamuje pokus přístup k řádku nebo sloupci, který matice neobsahuje.
        /// </summary>
        /// <remarks>
        ///     Týká se funkcí:
        ///         int MATICE.this[int, int];
        ///         void MATICE.zkrat_radek(uint);
        ///         void MATICE.rozsir_radek(uint);
        ///         void MATICE.rozsir_matici(uint);
        ///         void MATICE.odecti_radek(uint);
        ///         void MATICE.prohod_radky(uint, uint);
        ///         int[] MATICE.vrat_radek(uint);
        ///         int[] MATICE.vrat_sloupec(uint);
        /// </remarks>
        public const int Prilis_vysoky_index = 1;
        
        /// <summary>
        ///     Oznamuje pokus o volání funkce s nekladnou hodnotou
        ///     argumentu, pokud tento argument musí být kladný.
        /// </summary>
        /// <remarks>
        ///     Týká se funkcí:
        ///         int MATICE.D(int, int);
        ///         int MATICE.n(int, int);
        /// </remarks>
        public const int Nekladny_vstup = 2;

        /// <summary>
        ///     Oznamuje, že program není schopen se se zadaným vstupem vypořádat,
        ///     i když se nemusí nutně jednat o chybu.
        /// </summary>
        /// <remarks>
        ///     Týká se:
        ///         void MATICE.eliminuj();
        ///         void ROVNICE.vycisli_rovnici();
        /// </remarks>
        public const int Neocekavana_situace = 3;

        /// <summary>
        ///     Oznamuje pokus o vytvoření rovnice, která nemá právě dvě strany.
        /// </summary>
        /// <remarks>
        ///     Týká se:
        ///         ROVNICE.ROVNICE(string);
        /// </remarks>
        public const int Chybny_pocet_stran_rovnice = 4;

        /// <summary>
        ///     Oznamuje, že parsovaný text obsahuje znak,
        ///     který v dané situaci není platný.
        /// </summary>
        /// <remarks>
        ///     Týká se:
        ///         i_POLOZKA[] SLOUCENINA.parse(string);
        /// </remarks>
        public const int Neplatny_znak = 5;

        /// <summary>
        ///     Oznamuje, že rovnice obsahuje dvě + za sebou, které nejsou odděleny chemickou sloučeninou.
        /// </summary>
        /// <remarks>
        ///     Týká se:
        ///         ROVNICE.ROVNICE(string);
        /// </remarks>
        public const int Prazdna_sloucenina = 6;

        /// <summary>
        ///     Oznamuje pokus o vyčíslení rovnice bez provedeních příslušných výpočtů.
        /// </summary>
        /// <remarks>
        ///     Týká se:
        ///         void ROVNICE.ziskej_matici_rovnice();
        /// </remarks>
        public const int Nevyplnena_matice = 7;

        /// <summary>
        ///     Oznamuje, že některá ze sloučenin má po vyčíslení
        ///     stechiometrický koeficient rovný 0.
        /// </summary>
        /// <remarks>
        ///     Týká se:
        ///         string SLOUCENINA.ToString()
        /// </remarks>
        public const int Neplatna_rovnice = 8;

        /// <summary>
        ///     Oznamuje pokus o vytvoření instance typu PRVEK, ZAVORKA
        ///     nebo SLOUCENINA s nulovým počtem této položky.
        /// </summary>
        /// <remarks>
        ///     Týká se:
        ///         PRVEK.PRVEK(string, uint);
        ///         ZAVORKA.ZAVORKA(i_POLOZKA[], uint);
        ///         SLOUCENINA.SLOUCENINA(string, int?);
        ///         SLOUCENINA.Parse(string);
        /// </remarks>
        public const int Nulovy_pocet = 9;

        /// <summary>
        ///     Oznamuje pokus o vytvoření instance typu ZAVORKA
        ///     bez vnitřních prvků nebo závorek.
        /// </summary>
        /// <remarks>
        ///     Týká se:
        ///         ZAVORKA.ZAVORKA(i_POLOZKA[], uint);
        /// </remarks>
        public const int Prazdna_zavorka = 10;

        /// <summary>
        ///     Uchovává všechny chybové hlášky, které jsou předávány
        ///     v proměnné CHYBA.Message.
        /// </summary>
        /// <remarks>
        ///     Index umístění hlášky v poli odpovídá číslu chyby.
        /// </remarks>
        static readonly string[] prehled_chyb;
        
        /// <summary>
        ///     Připraví pole prehled chyb.
        /// </summary>
        static CHYBA()
        {
            prehled_chyb = new string[11];
            prehled_chyb[0] = "Nelze vytvořit prázdnou matici.";
            prehled_chyb[1] = "Příliš vysoký index - matice tento prvek (řádek, sloupec) neobsahuje.";
            prehled_chyb[2] = "Argumenty této funkce musí být kladné.";
            prehled_chyb[3] = "Rovnici se nepodařilo vyčíslit.";
            prehled_chyb[4] = "Rovnice musí mít právě jedno známénko >.";
            prehled_chyb[5] = "Neplatný vstup.";
            prehled_chyb[6] = "Chybně zadaná sloučenina.";
            prehled_chyb[7] = "Je nutné vyplnit matice chemické rovnice (funkce 'preved_matici_na_rovnici()').";
            prehled_chyb[8] = "Tuto rovnici nelze vyčíslit.";
            prehled_chyb[9] = "Počet atomů prvku ve sloučenině nemůže být číslo 0.";
            prehled_chyb[10] = "Není možné použít prázdnou závorku.";
        }

        /// <summary>
        ///     Upřesňuje druh nastalé chyby.
        /// </summary>
        /// <remarks>
        ///     Odkazuje na text chyby, uložený na indexu cislo_chyby
        ///     v poli prehled_chyb. Smyslem je zejména zjednodušené
        ///     určení konkrétního typu chyby.
        /// </remarks>
        public int cislo_chyby;
        
        /// <summary>
        ///     Vytváří výjimku typu CHYBA.
        /// </summary>
        /// <param name="cislo_chyby">Udává konkrétní druh chyby.</param>
        public CHYBA (int cislo_chyby): base(prehled_chyb[cislo_chyby])
        {
            this.cislo_chyby = cislo_chyby;
        }
    }
}
