using System.Collections.Generic;

using static vycislovac_rovnic.CHYBA;

namespace vycislovac_rovnic
{
    /// <summary>
    ///     Umožňuje jednotné používání jednoho prvku a skupiny prvků,
    ///     která je v chemickém vzorci znázorněna závorkou.
    /// </summary>
    /// <remarks>
    ///     Implementováno těmito datovými typy:
    ///         class PRVEK
    ///         class ZAVORKA
    /// </remarks>
    interface i_POLOZKA
    {
    }

    /// <summary>
    ///     Reprezentuje jeden prvek, který je v chemickém vzorci
    ///     znázorněn svojí chemickou značkou a číslem, které udává
    ///     počet tohoto prvku ve sloučenině.
    /// </summary>
    struct PRVEK: i_POLOZKA
    {
        /// <summary>
        ///     Představuje chemickou značku prvku.
        /// </summary>
        /// <remarks>
        ///     Vždy by se mělo jednat o jedno velké a nejvýše jedno malé písmeno.
        /// </remarks>
        public readonly string znacka;
        
        /// <summary>
        ///     Představuje počet atomů tohoto prvku ve sloučenině.
        /// </summary>
        public readonly uint pocet;

        /// <summary>
        ///     Vytvoří instanci prvku.
        /// </summary>
        /// <param name="znacka">Chemická značka prvku.</param>
        /// <param name="pocet">Počet atomů prvku ve sloučenině.</param>
        /// <exception cref="CHYBA">
        ///     Chyba Nulovy_pocet (č. 9): parametr <paramref name="pocet"> nabývá hodnoty 0.
        /// </exception>
        public PRVEK (string znacka, uint pocet)
        {
            if (pocet == 0)
            {
                throw new CHYBA(Nulovy_pocet);
            }

            this.znacka = znacka;
            this.pocet = pocet;
        }

        /// <summary>
        ///     Převede prvek na jeho textovou reprezentaci.
        /// </summary>
        /// <returns>Textovou reprezentaci prvku.</returns>
        public override string ToString()
        {
            if (pocet == 1)
            {
                return znacka;
            }
            else
            {
                return znacka + pocet;
            }
        }

        /// <summary>
        ///     Porovnává dvě instance třídy PRVEK podle jejich proměnné znacka.
        /// </summary>
        /// <param name="a">první prvek</param>
        /// <param name="b">druhý prvek</param>
        /// <returns>True, pokud obě instance mají stejnou značku, jinak false.</returns>
        public static bool operator == (PRVEK a, PRVEK b)
        {
            return a.znacka == b.znacka;
        }
        
        /// <summary>
        ///     Porovnává dvě instance třídy PRVEK podle jejich proměnné znacka.
        /// </summary>
        /// <param name="a">první prvek</param>
        /// <param name="b">druhý prvek</param>
        /// <returns>False, pokud obě instance mají stejnou značku, jinak true.</returns>
        public static bool operator != (PRVEK a, PRVEK b)
        {
            return a.znacka != b.znacka;
        }
    }

    /// <summary>
    ///     Reprezentuje skupinu prvků, která je v chemickém vzorci označena závorkou.
    /// </summary>
    struct ZAVORKA: i_POLOZKA
    {
        /// <summary>
        ///     Určuje typ závorky.
        /// </summary>
        /// <remarks>
        ///     1 - kulaté, 2 - hranaté, 3 - složené (zatím fungují pouze kulaté závorky)
        /// </remarks>
        public readonly int typ_zavorky;
        
        /// <summary>
        ///     Seznam všech prvků (případně vniřních závorek) této závorky.
        /// </summary>
        public readonly i_POLOZKA[] polozky;

        /// <summary>
        ///     Představuje počet těchto závorek ve sloučenině.
        /// </summary>
        public readonly uint pocet;

        /// <summary>
        ///     Vytvoří instanci závorky.
        /// </summary>
        /// <param name="polozky">Seznam prvků (případně závorek) tvořících tupo závorku.</param>
        /// <param name="pocet">Počet skupin atomů tohoto typu ve sloučenině.</param>
        /// <exception cref="CHYBA">
        ///     Chyba Nulovy_pocet (č. 9): parametr <paramref name="pocet"> nabývá hodnoty 0.
        ///     Chyba Prazdna_zavorka (č. 10): pole <paramref name="pocet"> nabývá hodnoty null nebo má nulovou délku.
        /// </exception>
        public ZAVORKA (i_POLOZKA[] polozky, uint pocet)
        {
            if (pocet == 0)
            {
                throw new CHYBA(Nulovy_pocet);
            }
            else if (polozky.Length == 0 || polozky == null)
            {
                throw new CHYBA(Prazdna_zavorka);
            }

            typ_zavorky = 1; // zatím lze použít pouze kulaté
            this.polozky = polozky;
            this.pocet = pocet;
        }

        /// <summary>
        ///     Převede závorku na její textovou reprezentaci.
        /// </summary>
        /// <returns>Textovou reprezentaci závorky.</returns>
        public override string ToString()
        {
            string zapis = "(";
            foreach (i_POLOZKA p in polozky)
            {
                zapis += p;
            }
            if (pocet == 1)
            {
                return zapis + ")";
            }
            else
            {
                return zapis + ")" + pocet;
            }
        }

        /// <summary>
        ///     Zjišťuje, kolik atomů prvku <paramref name="p">
        ///     tato závorka obsahuje.
        /// </summary>
        /// <param name="p">Prvek, jehož porčet se zjišťuje.</param>
        /// <returns>Počet atomů prvku <paramref name="p">.</returns>
        public uint pocet_atomu (PRVEK p)
        {
            uint pocet_atomu_prvku = 0;

            foreach (i_POLOZKA pp in polozky)
            {
                if (pp is PRVEK ppp && p == ppp)
                {
                    pocet_atomu_prvku += ppp.pocet;
                }
                else if (pp is ZAVORKA z)
                {
                    pocet_atomu_prvku += z.pocet + z.pocet_atomu(p);
                }
            }
            return pocet_atomu_prvku;
        }

        /// <summary>
        ///   Umožňuje postupně vrátit všechny prvky v závorce.
        /// </summary>
        /// <returns>Všechny prvky závorky.</returns>
        public IEnumerable<PRVEK> prvky()
        {
            foreach (i_POLOZKA pa in polozky)
            {
                if (pa is PRVEK p)
                {
                    yield return p;
                }
                else
                {
                    foreach (PRVEK pb in ((ZAVORKA)pa).prvky())
                    {
                        yield return pb;
                    }
                }
            }
        }
    }
}
 