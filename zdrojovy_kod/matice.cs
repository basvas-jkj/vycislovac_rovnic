using System.Linq;

using static System.Math;
using static vycislovac_rovnic.CHYBA;

namespace vycislovac_rovnic
{
    /// <summary>
    ///     Třída MATICE umožňuje ukládání a zpracování matic.
    ///     Hlavní operací je eliminace, která upraví pro potřeby
    ///     tohoto projektu matici tak, aby z ní bylo možno zjistit,
    ///     zda a jak je možné chemickou rovnici vyčíslit.
    /// </summary>
    class MATICE
    {
        //  ----------------------------
        //  |       konstruktory       |
        //  ----------------------------

        /// <summary>
        ///     Vytvoří matici, která je totožná s maticí <paramref name="zdroj">. 
        /// </summary>
        /// <param name="zdroj">Vzor, podle kterého se vytváří nová matice.</param>
        /// <exception cref="CHYBA">
        ///     Chyba Prazdna_matice (č. 0): parametr <paramref name="zdroj"> nabývá hodnoty null.
        /// </exception>
        public MATICE (MATICE zdroj) : this(zdroj?.matice)
        {

        }

        /// <summary>
        ///     Vytvoří matici, jejíž rozložení prvků udává parametr <paramref name="zdroj">.
        /// </summary>
        /// <param name="zdroj">Vzor, podle kterého se vytváří nová matice.</param>
        /// <exception cref="CHYBA">
        ///     Chyba Prazdna_matice (č. 0): parametr <paramref name="zdroj"> nabývá hodnoty null nebo se jedná o pole s nulovou délkou.
        /// </exception>
        public MATICE (int[,] zdroj)
        {
            if (zdroj == null || zdroj.Length == 0)
            {
                throw new CHYBA(Prazdna_matice);
            }
            pocet_radku = (uint) zdroj.GetLength(0);
            pocet_sloupcu = (uint) zdroj.GetLength(1);
            matice = (int[,])zdroj.Clone();
        }

        /// <summary>
        ///     Vytvoří nulovou matici o velikosti <paramref name="pocet_radku">×<paramref name="pocet_sloupcu">.
        /// </summary>
        /// <param name="pocet_radku">Počet řádků vytvářené matice.</param>
        /// <param name="pocet_sloupcu">Počet sloupců vytvářené matice.</param>
        /// <exception cref="CHYBA">
        ///     Chyba Prazdna_matice (č. 0): parametr <paramref name="pocet_radku"> nebo <paramref name="pocet_sloupcu"> nabývá hodnoty menší než 1.
        /// </exception>
        public MATICE(int pocet_radku, int pocet_sloupcu)
        {
            if (pocet_radku < 1 || pocet_sloupcu < 1)
            {
                throw new CHYBA(Prazdna_matice);
            }
            this.pocet_radku = (uint) pocet_radku;
            this.pocet_sloupcu = (uint) pocet_sloupcu;
            matice = new int[pocet_radku, pocet_sloupcu];
        }

        //  ------------------------------
        //  |       matice samotná       |
        //  ------------------------------
        
        /// <summary>
        ///     Vrací počet řádků matice.
        /// </summary>
        public uint pocet_radku {get;}

        /// <summary>
        ///     Vrací počet sloupců matice.
        /// </summary>
        public uint pocet_sloupcu {get;}
        
        /// <summary>
        ///     Uchovává hodnoty všech prvků matice.
        /// </summary>
        private readonly int[,] matice;  // řádky × sloupce (sločeniny × prvky)

        /// <summary>
        ///     Umožňuje získat prvek matice na pozici [<paramref name="radek">; <paramref name="sloupec">].
        /// </summary>
        /// <param name="radek">Udává číslo řádku, ve kterém se prvek nachází.</param>
        /// <param name="sloupec">Udává číslo sloupce, ve kterém se prvek nachází.</param>
        /// <exception cref="CHYBA">
        ///     Chyba Prilis_vysoky_index (č. 1): prvek s tak vysokými souřadnicemi není v této matici obsažen.
        /// </exception>
        public int this[uint radek, uint sloupec]
        {
            get
            {
                if (radek > pocet_radku || sloupec > pocet_sloupcu)
                {
                    throw new CHYBA(Prilis_vysoky_index);
                }
                return matice[radek, sloupec];
            }
        }

        /// <summary>
        ///     Zjišťuje, kolik nenulových řádků matice obsahuje.
        /// </summary>
        /// <remarks>
        ///     V případě, že je matice již eliminovaná, je vrácená hodnota zároveň hodností matice.
        /// </remarks>
        /// <returns>počet nenulových řádků matice</returns>
        public uint pocet_nenulovych_radku()
        {
            for (uint fa = pocet_radku - 1; fa >= 0; fa--)
            {
                for (int fb = 0; fb < pocet_sloupcu; fb++)
                {
                    if (matice[fa,fb] != 0)
                    {
                        return fa + 1;
                    }
                }
            }
            return 0;
        }

        //  -------------------------
        //  |       eliminace       |
        //  -------------------------

        /// <summary>
        ///     Funkce, která počítá největší společný dělitel dvou čísel.
        /// </summary>
        /// <param name="a">Kladné celé číslo.</param>
        /// <param name="b">Kladné celé číslo.</param>
        /// <returns>
        ///     Největší společný dělitel parametrů <paramref name="a"> a <paramref name="b">.
        /// </returns>
        /// <exception cref="CHYBA">
        ///     Chyba Nekladny_vstup (č. 2): některý z parametrů není kladný.
        /// </exception>
        private static int D (int a, int b)
        {

            if (a <= 0 || b <= 0)
            {
                throw new CHYBA(Nekladny_vstup);
            }

            while (b != 0)
            {
                int c = a % b;
                a = b;
                b = c;
            }
            return a;
        }

        /// <summary>
        ///     Funkce, která počítá největší společný dělitel několika čísel.
        /// </summary>
        /// <remarks>
        ///     Znaménko návratové hodnoty je shodné se znaménem prvního nenulového čísla z parametru <paramref name="p">.
        ///     Pokud se v seznamu čísel objeví 0, není při výpočtu z ohledněna, největší společný dělitel se počítá pouze z nenulových čísel.
        ///     Pokud jsou všechny prvky parametru <paramref name="p"> rovny 0, je návratová hodnota 1.
        /// </remarks>
        /// <param name="p">Seznam celých čísel, ze kterých se počítá největší společný dělitel.</param>
        /// <returns>
        ///     Největší společný dělitel čísel z parametru <paramref name="p">.
        /// </returns>
        private static int D (params int[] p)
        {
            int znamenko = 1;
            int nejvetsi_spolecny_delitel = 0;

            for (int f = 0; f < p.Length; f += 1)
            {
                if (p[f] > 0)
                {
                    if (nejvetsi_spolecny_delitel == 0)
                    {
                        nejvetsi_spolecny_delitel = p[f];
                    }
                    nejvetsi_spolecny_delitel = D(nejvetsi_spolecny_delitel, p[f]);
                }
                else if (p[f] < 0)
                {
                    if (nejvetsi_spolecny_delitel == 0)
                    {
                        znamenko = -1;
                        nejvetsi_spolecny_delitel = -p[f];
                    }
                    else
                    {
                        nejvetsi_spolecny_delitel = D(nejvetsi_spolecny_delitel, -p[f]);
                    }
                }
                else
                {
                    continue;
                }
            }
            if (nejvetsi_spolecny_delitel == 0)
            {
                return 1;
            }
            return nejvetsi_spolecny_delitel * znamenko;
        }

        /// <summary>
        ///     Funkce, která počítá nejmenší společný násobek dvou čísel.
        /// </summary>
        /// <param name="a">Kladné celé číslo.</param>
        /// <param name="b">Kladné celé číslo.</param>
        /// <returns>
        ///     Nejmenší společný násobek parametrů <paramref name="a"> a <paramref name="b">.
        /// </returns>
        /// <exception cref="CHYBA">
        ///     Chyba Nekladny_vstup (č. 2): některý z parametrů není kladný.
        /// </exception>
        private static int n (int a, int b)
        {
            return a * b / D(a, b);
        }

        /// <summary>
        ///     Funkce, která počítá nejmenší společný násobek několika čísel.
        /// </summary>
        /// <remarks>
        ///     Návratová hodnota je vždy kladná, bez ohledu na znaménka čísel z parametru <paramref name="p">.
        ///     Pokud se v seznamu čísel objeví 0, není při výpočtu z ohledněna, nejmenší společný násobek se počítá pouze z nenulových čísel.
        ///     Pokud jsou všechny prvky parametru <paramref name="p"> rovny 0, je návratová hodnota 1.
        /// </remarks>
        /// <param name="p">Seznam celých čísel, ze kterých se počítá největší společný dělitel.</param>
        /// <returns>
        ///     Nejmenší společný násobek čísel z parametru <paramref name="p">.
        /// </returns>
        private static int n (params int[] p)
        {
            int nejmensi_spolecny_nasobek = 1;

            for (int f = 0; f < p.Length; f += 1)
            {
                if (p[f] > 0)
                {
                    nejmensi_spolecny_nasobek = n(nejmensi_spolecny_nasobek, p[f]);
                }
                else if (p[f] < 0)
                {
                    nejmensi_spolecny_nasobek = n(nejmensi_spolecny_nasobek, -p[f]);
                }
                else
                {
                    continue;
                }
            }
            return nejmensi_spolecny_nasobek;
        }

        /// <summary>
        ///     Zkrátí řádek <paramref name="cislo_radku"> na nesoudělný tvar.
        /// </summary>
        /// <param name="cislo_radku">Udává pořadí řádku v matici.</param>
        /// <exception cref="CHYBA">
        ///     Chyba Prilis_vysoky_index (č. 1): řádek s tímto číslem tato matice neobsahuje.
        /// </exception>
        private void zkrat_radek (uint cislo_radku)
        {
            if (cislo_radku >= pocet_radku)
            {
                throw new CHYBA(Prilis_vysoky_index);
            }
            int[] radek = vrat_radek(cislo_radku);
            int nejvetsi_spolecny_delitel = D(radek);

            for (int f = 0; f < pocet_sloupcu; f += 1)
            {
                matice[cislo_radku, f] /= nejvetsi_spolecny_delitel;
            }
        }

        /// <summary>
        ///     Vynásobí všechny prvky řádku <paramref name="cislo_radku"> parametrem <paramref name="koeficient">.
        /// </summary>
        /// <param name="cislo_radku">Udává pořadí řádku v matici.</param>
        /// <param name="koeficient">Udává činitel, kterým bude řádek vynásoben.</param>
        /// <exception cref="CHYBA">
        ///     Chyba Prilis_vysoky_index (č. 1): řádek s tímto číslem tato matice neobsahuje.
        /// </exception>
        private void rozsir_radek (uint cislo_radku, int koeficient)
        {
            if (cislo_radku >= pocet_radku)
            {
                throw new CHYBA(Prilis_vysoky_index);
            }
            
            for (int f = 0; f < pocet_sloupcu; f += 1)
            {
                matice[cislo_radku, f] *= koeficient;
            }
        }

        /// <summary>
        ///     Vynásobí všechny řádky matice tak, aby v sloupci s číslem <paramref name="cislo_sloupce">
        ///     Měly všechny řádky stejnou hodnotu nebo hodnotu 0.
        /// </summary>
        /// <param name="cislo_sloupce">Udává pořadí sloupce v matici.</param>
        /// <exception cref="CHYBA">
        ///     Chyba Prilis_vysoky_index (č. 1): sloupec s tímto číslem tato matice neobsahuje.
        /// </exception>
        private void rozsir_matici (uint cislo_sloupce)
        {
            if (cislo_sloupce >= pocet_sloupcu)
            {
                throw new CHYBA(Prilis_vysoky_index);
            }
            else if (matice[cislo_sloupce, cislo_sloupce] == 0)
            {
                return;
            }
            int[] sloupec = vrat_sloupec(cislo_sloupce);
            int nasobek_sloupce = n(sloupec);

            for (uint f = 0; f < pocet_radku; f += 1)
            {
                if (matice[f, cislo_sloupce] == 0) {continue;}
                int koeficient = nasobek_sloupce / matice[f, cislo_sloupce];
                rozsir_radek(f, koeficient);
            }
        }

        /// <summary>
        ///     Zajistí, že všechny řádky kromě <paramref name="cislo_radku"> mají ve sloupci
        ///     <paramref name="cislo_radku"> hodnotu 0.
        /// </summary>
        /// <param name="cislo_radku">Udává pořadí řádku v matici.</param>
        /// <exception cref="CHYBA">
        ///     Chyba Prilis_vysoky_index (č. 1): řádek s tímto číslem tato matice neobsahuje.
        /// </exception>
        private void odecti_radek(uint cislo_radku)
        {
            if (cislo_radku >= pocet_radku)
            {
                throw new CHYBA(Prilis_vysoky_index);
            }
            rozsir_matici(cislo_radku);

            for (uint fa = 0; fa < pocet_radku; fa += 1)
            {
                if (fa == cislo_radku || matice[fa, cislo_radku] == 0)
                {
                    continue;
                }
                else if (matice[fa, cislo_radku] < 0)
                {
                    for (int fb = 0; fb < pocet_sloupcu; fb += 1)
                    {
                        matice[fa, fb] += matice[cislo_radku, fb];
                    }
                    zkrat_radek(fa);
                }
                else
                {
                    for (int fb = 0; fb < pocet_sloupcu; fb += 1)
                    {
                        matice[fa, fb] -= matice[cislo_radku, fb];
                    }
                    zkrat_radek(fa);
                }
            }
            zkrat_radek(cislo_radku);
        }

        /// <summary>
        ///     Prohodí hodnoty daných dvou řádků ve všech sloupcích.
        /// </summary>
        /// <param name="prvni_radek">Udává pořadí prvního řádku v matici.</param>
        /// <param name="druhy_radek">Udává pořadí druhého řádku v matici.</param>
        /// <exception cref="CHYBA">
        ///     Chyba Prilis_vysoky_index (č. 1): řádek s tímto číslem tato matice neobsahuje.
        /// </exception>
        private void prohod_radky(uint prvni_radek, uint druhy_radek)
        {
            if (prvni_radek >= pocet_radku || druhy_radek >= pocet_radku)
            {
                throw new CHYBA(Prilis_vysoky_index);
            }

            for (int f = 0; f < pocet_sloupcu; f += 1)
            {
                int p = matice[prvni_radek,f];
                matice[prvni_radek,f] = matice[druhy_radek,f];
                matice[druhy_radek,f] = p;
            }
        }

        /// <summary>
        ///     Provede eliminaci matice na diagonální tvar.
        /// </summary>
        /// <remarks>
        ///     Matice by po provedení této funkce měla mít náležitý tvar,
        ///     více viz dokumentace.
        /// </remarks>
        /// <exception cref="CHYBA">
        ///     Chyba Neocekavana_situace (č. 3): s touto situací se program nedokázal vypořádat.
        /// </exception>
        public void eliminuj()
        {
            for (uint f = 0; f < pocet_radku && f < pocet_sloupcu; f += 1)
            {
                uint posledni_radek = (uint) pocet_radku - 1;
                while (matice[f,f] == 0)
                {
                    if (posledni_radek == f)
                    {
                        break;
                    }

                    prohod_radky(f, posledni_radek);
                    posledni_radek -= 1;
                }
                odecti_radek(f);
            }
            // zbytek kódu má za úkol rozšířit prvky matice podle hlavní diagonály 
            int nasobek_diagonaly = n(vrat_hlavni_diagonalu());
            for (uint f = 0; f < pocet_nenulovych_radku() && f < pocet_sloupcu; f += 1)
            {
                if (matice[f, f] == 0) 
                {
                    throw new CHYBA(Neocekavana_situace);
                }
                int koeficient = nasobek_diagonaly / matice[f, f];
                rozsir_radek(f, koeficient);
            }
        }

        //  -------------------------------
        //  |       součásti matice       |
        //  -------------------------------

        /// <summary>
        ///     Umožňuje získat prvky jednoho řádku matice.
        /// </summary>
        /// <param name="cislo_radku">Udává pořadí řádku v matici.</param>
        /// <returns>Pole s hodnotami prvků řádku.</returns>
        /// <exception cref="CHYBA">
        ///     Chyba Prilis_vysoky_index (č. 1): řádek s tímto číslem tato matice neobsahuje.
        /// </exception>
        private int[] vrat_radek(uint cislo_radku)
        {
            if (cislo_radku >= pocet_radku)
            {
                throw new CHYBA(Prilis_vysoky_index);
            }

            int[] radek = new int[pocet_sloupcu];
            for (int f = 0; f < pocet_sloupcu; f += 1)
            {
                radek[f] = matice[cislo_radku, f];
            }
            return radek;
        }

        /// <summary>
        ///     Umožňuje získat prvky jednoho sloupce matice.
        /// </summary>
        /// <param name="cislo_radku">Udává pořadí sloupce v matici.</param>
        /// <returns>Pole s hodnotami prvků sloupce.</returns>
        /// <exception cref="CHYBA">
        ///     Chyba Prilis_vysoky_index (č. 1): sloupec s tímto číslem tato matice neobsahuje.
        /// </exception>
        private int[] vrat_sloupec(uint cislo_sloupce)
        {
            if (cislo_sloupce >= pocet_sloupcu)
            {
                throw new CHYBA(Prilis_vysoky_index);
            }

            int[] sloupec = new int[pocet_radku];
            for (int f = 0; f < pocet_radku; f += 1)
            {
                sloupec[f] = matice[f, cislo_sloupce];
            }
            return sloupec;
        }

        /// <summary>
        ///     Umožňuje získat prvky z hlavní diagonály matice.
        /// </summary>
        /// <returns>Pole s hodnotami prvků hlavní diahonály.</returns>
        private int[] vrat_hlavni_diagonalu()
        {
            uint delka_diagonaly = Min(pocet_radku, pocet_sloupcu);
            int[] diagonala = new int[delka_diagonaly];

            for (int f = 0; f < delka_diagonaly; f += 1)
            {
                diagonala[f] = matice[f, f];
            }
            return diagonala;
        }
    }
}