using System.Linq;
using System.Collections.Generic;

using static vycislovac_rovnic.CHYBA;

namespace vycislovac_rovnic
{
    /// <summary>
    ///     Reprezentuje jednu chemickou veličinu z rovnice.
    /// </summary>
    class SLOUCENINA
    {
        /// <summary>
        ///     Vytváří instanci typu sloučenina.
        /// </summary>
        /// <remarks>
        ///     Parametr <paramref name="pocet"> má jako svoji implicitní hodnotu
        ///     null, protože se nepředpokládá, že by již při vytváření sloučeniny
        ///     byla správná hodnota tohoto parametru známa.
        /// </remarks>
        /// <param name="vzorec">Chemický vzorec sloučeniny.</param>
        /// <param name="pocet">Počet molekul této sloučeniny ve vzorci.</param>
        /// <exception cref="CHYBA">
        ///     Chyba Neplatny_znak (č. 5): rovnice obsahuje znak, který v chemickém vzorci není povolen.
        ///     Chyba Nula_polozek (č. 9): parametr <paramref name="pocet"> nabývá hodnoty 0.
        ///     Chyba Nulovy_pocet (č. 9): počet některého prvku nebo závorky je roven 0.
        /// </exception>
        public SLOUCENINA(string vzorec, int? pocet = null)
        {
            if (pocet == 0)
            {
                throw new CHYBA(Nulovy_pocet);
            }
            pocet_molekul = pocet;
            polozky = parse(vzorec);
        }

        /// <summary>
        ///     Seznam všech prvků a závorek této sloučeniny.
        /// </summary>
        public i_POLOZKA[] polozky;
        
        /// <summary>
        ///     Udává počet molekul této sloučeniny v chemické rovnici.
        /// </summary>
        /// <remarks>
        ///     Při vytváření sloučeniny není počet molekul znám, je
        ///     nutné ho spočítat podle ostatních sloučenin v rovnici,
        ///     proto je tato proměnná označena jako nullable.
        /// </remarks>
        public int? pocet_molekul;

        /// <summary>
        ///     Umožňuje převod chemického vzorce sloučeniny na pole položek.
        /// </summary>
        /// <param name="vzorec">Textová reprezentace sloučeniny.</param>
        /// <returns>Pole položek.</returns>
        /// <exception cref="CHYBA">
        ///     Chyba Neplatny_znak (č. 5): vzorec obsahuje znak, který v chemickém vzorci není povolen.
        ///     Chyba Nulovy_pocet (č. 9): počet některého prvku nebo závorky je roven 0.
        /// </exception>
        private static i_POLOZKA[] parse(string vzorec)
        {
            LinkedList<i_POLOZKA> polozky = new();
            for (int f = 0; f < vzorec.Length; f++)
            {
                if (char.IsUpper(vzorec[f]))
                {
                    string znacka_prvku;
                    uint pocet_prvku;

                    if (f + 1 < vzorec.Length && char.IsLower(vzorec[f + 1]))
                    {
                        znacka_prvku = string.Format("{0}{1}", vzorec[f], vzorec[f + 1]);
                        f++;
                    }
                    else
                    {
                        znacka_prvku = vzorec[f].ToString();
                    }

                    if (f + 1 >= vzorec.Length || !char.IsDigit(vzorec[f + 1]))
                    {
                        pocet_prvku = 1;
                    }
                    else
                    {
                        int p = 1;
                        while (f + p < vzorec.Length && char.IsDigit(vzorec[f + p]))
                        {
                            p += 1;
                        }
                        
                        pocet_prvku = uint.Parse(vzorec.Substring(f + 1, p - 1));
                        f += p - 1;
                    }

                    polozky.AddLast(new PRVEK(znacka_prvku, pocet_prvku));
                }
                // zatím fungují pouze kulaté závorky
                else if (vzorec[f] == '(')
                {
                    int p = 1;
                    uint pocet_prvku; //= 1;
                    string zavorka;
                    i_POLOZKA[] prvky_zavorky;
                    
                    while (f + p < vzorec.Length && vzorec[f + p] != ')')
                    {
                        p += 1;
                    }
                    zavorka = vzorec.Substring(f + 1, p - 1);
                    
                    f += p + 1;
                    if (f >= vzorec.Length || !char.IsDigit(vzorec[f]))
                    {
                        pocet_prvku = 1;
                    }
                    else
                    {
                        p = 1;
                        while (f + p < vzorec.Length && char.IsDigit(vzorec[f + p]))
                        {
                            p += 1;
                        }
                        
                        pocet_prvku = uint.Parse(vzorec.Substring(f, p));
                        f += p;
                    }
                    f--;

                    prvky_zavorky = parse(zavorka);
                    polozky.AddLast(new ZAVORKA(prvky_zavorky, pocet_prvku));
                }
                else
                {
                    throw new CHYBA(Neplatny_znak);
                }
            }
            return polozky.ToArray();
        }

        /// <summary>
        ///     Zjišťuje, kolik atomů prvku <paramref name="p">
        ///     tato sloučenina obsahuje.
        /// </summary>
        /// <param name="p">Prvek, jehož počet se zjišťuje.</param>
        /// <returns>Počet atomů prvku <paramref name="p">.</returns>
        public uint pocet_atomu(PRVEK p)
        {
            uint pocet_atomu_prvku = 0;

            foreach (i_POLOZKA pp in polozky)
            {
                if (pp is PRVEK ppp)
                {
                    if (p == ppp)
                    {
                        pocet_atomu_prvku += ppp.pocet;
                    }
                }
                else
                {
                    ZAVORKA z = (ZAVORKA)pp;
                    pocet_atomu_prvku += z.pocet * z.pocet_atomu(p);
                }
            }
            return pocet_atomu_prvku;
        }

        /// <summary>
        ///   Umožňuje postupně vrátit všechny prvky ve sloučenině.
        /// </summary>
        /// <returns>Všechny prvky sloučeniny.</returns>
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
                    foreach (PRVEK pb in ((ZAVORKA) pa).prvky())
                    {
                        yield return pb;
                    }
                }
            }
            yield break;
        }

        /// <summary>
        ///     Převede sloučeninu na její textovou reprezentaci.
        /// </summary>
        /// <returns>Textovou reprezentaci sloučeniny.</returns>
        /// <exception cref="CHYBA">
        ///     Chyba Nulovy_pocet (č. 9): sloučenina se nemůže vyskytovat v počtu 0 molekul.
        /// </exception>
        public override string ToString()
        {
            if (pocet_molekul == 0)
            {
                throw new CHYBA(Neplatna_rovnice);
            }

            string sloucenina = (pocet_molekul != null && pocet_molekul > 1) ? pocet_molekul.ToString() : "";

            foreach (i_POLOZKA p in polozky)
            {
                sloucenina += p;
            }
            return sloucenina;
        }
    }
}
 