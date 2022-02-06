using System;
using System.Linq;

using static System.Math;
using static vycislovac_rovnic.CHYBA;

namespace vycislovac_rovnic
{
    /// <summary>
    ///     Reprezentuje rovnici chemické reakce.
    /// </summary>
    public class ROVNICE
    {
        /// <summary>
        ///     Vytvoří instanci typu ROVNICE na základě textové podoby
        ///     podle parametru <paramref name="rovnice">.
        /// </summary>
        /// <param name="rovnice">Textová reprezentace chemické rovnice.</param>
        /// <exception>
        ///     Chyba Prilis_mnoho_stran_rovnice (č. 4): <paramref name="rovnice"> neobsahuje levou i pravou stranu.
        ///     Chyba Malo_sloucenin (č. 5): alespoň jedna strana rovnice neobsahuje ani jednu sloučeninu.
        ///     Chyba Prazdna_sloucenina (č. 7): <paramref name="rovnice"> obsahuje dvě + za sebou.
        /// </exception>
        public ROVNICE(string rovnice)
        {
            rovnicova_matice = null;
            string[] strany_rovnice = rovnice.Split('>');
            if (strany_rovnice.Length != 2)
            {
                throw new CHYBA(Chybny_pocet_stran_rovnice);
            }

            string[] leva_strana = strany_rovnice[0].Split('+', StringSplitOptions.TrimEntries);
            string[] prava_strana = strany_rovnice[1].Split('+', StringSplitOptions.TrimEntries);

            reaktanty = new SLOUCENINA[leva_strana.Length];
            produkty = new SLOUCENINA[prava_strana.Length];

            for (int f = 0; f < leva_strana.Length; f += 1)
            {
                if (leva_strana[f] == "")
                {
                    throw new CHYBA(Prazdna_sloucenina);
                }
                reaktanty[f] = new SLOUCENINA(leva_strana[f]);
            }

            for (int f = 0; f < prava_strana.Length; f += 1)
            {
                if (prava_strana[f] == "")
                {
                    throw new CHYBA(Prazdna_sloucenina);
                }
                produkty[f] = new SLOUCENINA(prava_strana[f]);
            }
        }

        /// <summary>
        ///     Ukládá chemické sloučeniny, které do reakce vstupují jako reaktanty.
        /// </summary>
        private readonly SLOUCENINA[] reaktanty;
        
        /// <summary>
        ///     Ukládá chemické sloučeniny, které z reakce vystupují jako proddukty.    
        /// </summary>
        private readonly SLOUCENINA[] produkty;

        /// <summary>
        ///     Ukládá matici, která představuje rozložení prvků sloučenině.
        /// </summary>
        /// <remarks>
        ///     Více o struktuře a významu této matice viz dokumentace.
        /// </remarks>
        private MATICE rovnicova_matice = null;

        /// <summary>
        ///     Vytvoří matici chemické rovnice, kterou uloží do proměnné rovnicova_matice.
        /// </summary>
        /// <remarks>
        ///     Více o vytváření této matice viz dokumentace.
        /// </remarks>
        private void ziskej_matici_rovnice()
        {
            SLOUCENINA[] slouceniny = reaktanty.Concat(produkty).ToArray();
            SEZNAM_PRVKU prvky_rovnice = new(slouceniny);
            int pocet_prvku = prvky_rovnice.pocet_prvku();
            int pocet_sloucenin = reaktanty.Length + produkty.Length;

            int[,] matice = new int[pocet_prvku, pocet_sloucenin];
            PRVEK[] pole_prvku = prvky_rovnice.pole_prvku();

            for (int fa = 0; fa < pocet_sloucenin; fa++)
            {
                for (int fb = 0; fb < pocet_prvku; fb++)
                {
                    if (fa < reaktanty.Length)
                    {
                        matice[fb, fa] = (int) slouceniny[fa].pocet_atomu(pole_prvku[fb]);
                    }
                    else
                    {
                        matice[fb, fa] = (int) -slouceniny[fa].pocet_atomu(pole_prvku[fb]);
                    }
                }
            }

            rovnicova_matice = new(matice);
        }

        /// <summary>
        ///     Uloží stechiometrické koeficienty všech sloučenin 
        ///     do instanční proměnné SLOUCENINA.pocet_molekul 
        ///     příslušných sloučenin v polích reaktanty a produkty.
        /// </summary>
        /// <exception cref="CHYBA">
        ///     Chyba Nevyplnena_matice (č. 7): dosud nebyla zavolána funkce ROVNICE.ziskej_matici_rovnice().
        /// </exception>
        private void preved_matici_na_rovnici()
        {
            if (rovnicova_matice == null)
            {
                throw new CHYBA(Nevyplnena_matice);
            }
            uint fa;
            uint fb;
            for (fa = 0; fa < reaktanty.Length; fa++)
            {
                reaktanty[fa].pocet_molekul = Abs(rovnicova_matice[fa, rovnicova_matice.pocet_sloupcu - 1]);
            }
            for (fb = 0; fb < produkty.Length; fb++)
            {
                if (fa + fb < rovnicova_matice.pocet_nenulovych_radku())
                {
                    produkty[fb].pocet_molekul = Abs(rovnicova_matice[fa + fb, rovnicova_matice.pocet_sloupcu - 1]);
                }
                else
                {
                    produkty[fb].pocet_molekul = rovnicova_matice[0, 0];
                }
            }
        }

        /// <summary>
        ///     Provede vyčíslení chemické rovnice.
        /// </summary>
        /// <exception cref="CHYBA">
        ///     Chyba Neocekavana_situace (č. 3): s touto situací se program nedokázal vypořádat.
        /// </exception>
        public void vycisli_rovnici()
        {
            ziskej_matici_rovnice();
            rovnicova_matice.eliminuj();

            if (rovnicova_matice.pocet_sloupcu > rovnicova_matice.pocet_nenulovych_radku() + 1)
            {
                throw new CHYBA(Neocekavana_situace);
            }

            preved_matici_na_rovnici();
        }

        /// <summary>
        ///     Převede chemickou rovnici na její textovou reprezentaci.
        /// </summary>
        /// <returns>Textovou reprezentaci chemické rovnice.</returns>
        /// <exception cref="CHYBA">
        ///     Chyba Nulovy_pocet (č. 9): některá sloučenina se vyskytuje v počtu 0 molekul.
        /// </exception>
        public override string ToString()
        {
            string r = null;
            foreach (SLOUCENINA s in reaktanty)
            {
                r += s + " + ";
            }

            r = r.Remove(r.Length - 2);
            r += "> ";

            foreach (SLOUCENINA s in produkty)
            {
                r += s + " + ";
            }

            r = r.Remove(r.Length - 2);
            return r;
        }
    }
}
