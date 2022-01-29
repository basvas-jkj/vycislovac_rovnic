using System.IO;
using System.Collections.Generic;

namespace vycislovac_rovnic
{
    /// <summary>
    ///     Umožňuje získat seznam prvků ve sloučenině nebo jejich
    ///     skupinách, nez ohledu na jejich počet nebo umístění.
    /// </summary>
    class SEZNAM_PRVKU
    {
        /// <summary>
        ///     Ukládá seznam všech prvků ve sloučenině, bez ohledu na jejich počet a polohu ve sloučenině.
        /// </summary>
        readonly HashSet<string> prvky;

        /// <summary>
        ///     Vytvoří novou instanci třídy SEZNAM_PRVKU.
        /// </summary>
        public SEZNAM_PRVKU()
        {
            prvky = new HashSet<string>();
        }

        /// <summary>
        ///     Vytvoří novou instanci třídy SEZNAM_PRVKU a naplní
        ///     ji všemi prvky ze sloučeniny <paramref name="s">.
        /// </summary>
        /// <remarks>
        ///     Pokud sloučenina obsahuje stejný prvek na více různých místech, bude tento prvek uložen pouze jednou.
        /// </remarks>
        /// <example>
        ///     Sloučenina NH4NO3 má seznam prvků {N, H, O}.
        ///     Sloučenina CH3CH2CH3 má seznam prvků {C, H}.
        /// </example>
        /// <param name="s">Sloučenina, jejíž seznam prvků se vytváří.</param>
        public SEZNAM_PRVKU (SLOUCENINA s): this()
        {
            foreach (PRVEK p in s.prvky())
            {
                pridej_prvek(p);
            }
        }

        /// <summary>
        ///     Vytvoří novou instanci třídy SEZNAM_PRVKU a naplní ji všemi
        ///     prvky ze všech sloučenin z paramteru <paramref name="s">.
        /// <remarks>
        ///     Pokud více sloučenin obsahuje stejný prvek, bude tento prvek uložen pouze jednou.
        /// </remarks>
        /// <example>
        ///     Skupina sloučenin Fe2O3, CO2, Fe3C
        ///     má seznam prvků {Fe, O, C}.
        /// </example>
        /// </summary>
        /// <param name="slouceniny">Pole sloučenin, jejichž seznam prvků se vytváří.</param>
        public SEZNAM_PRVKU(SLOUCENINA[] slouceniny): this()
        {
            foreach (SLOUCENINA s in slouceniny)
            {
                foreach (string p in new SEZNAM_PRVKU(s).prvky)
                {
                    prvky.Add(p);
                }
            }
        }

        /// <summary>
        ///     Pokud prvek <paramref name="p"> není součástí
        ///     tohoto seznamu, bude do něj přidán.
        /// </summary>
        /// <param name="p">Prvek přidávaný do seznamu.</param>
        public void pridej_prvek (PRVEK p)
        {
            prvky.Add(p.znacka);
        }
        
        /// <summary>
        ///     Zjišťuje, jestli je prvek <paramref name="p"> součastí tohoto seznamu prvků.
        /// </summary>
        /// <param name="p">Prvek, jehož přítomnost se v seznamu zjišťuje.</param>
        /// <returns>True, pokud je <paramref name="p"> součástí seznamu, jinak false.</returns>
        public bool obsahuje_prvek (PRVEK p)
        {
            return prvky.Contains(p.znacka);
        }
        
        /// <summary>
        ///     Umožňuje získat počet prvků v tomto seznamu prvků.
        /// </summary>
        /// <returns>Počet prvků tohoto seznamu.</returns>
        public int pocet_prvku()
        {
            return prvky.Count;
        }

        /// <summary>
        ///     Převede tento seznam prvků (reprezentovaný množinou) na pole.
        /// </summary>
        /// <returns>Pole prvků, odpovídající seznamu.</returns>
        public PRVEK[] pole_prvku()
        {
            int f = 0;
            PRVEK[] pole = new PRVEK[prvky.Count];
            foreach (string p in prvky)
            {
                pole[f] = new PRVEK(p, 1);
                f += 1;
            }
            return pole;
        }

        /// <summary>
        ///     Převede seznam prvků na jeho textovou reprezentaci.
        /// </summary>
        /// <returns>Textovou reprezentaci seznamu prvků.</returns>
        public override string ToString()
        {
            StringWriter sw = new();

            foreach (string s in prvky)
            {
                sw.Write(s + ",");
            }

            sw.WriteLine();
            return sw.ToString();
        }
    }
}
