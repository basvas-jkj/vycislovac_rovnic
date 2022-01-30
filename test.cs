using System;
using System.Windows.Forms;
using System.Collections.Generic;

//#error "Je potřeba opravit výpis rovnice - není možné vypsat rovnici s maticí s nulovými řádky."
#warning "Je nutné přidat centrální zachytávač vnitřních chyb."

namespace vycislovac_rovnic
{
    class TEST
    {
        static readonly List<ROVNICE> rovnice = new();

        public static void testuj (TextBox t, Label l)
        {
            priprav_test(t);

            foreach (ROVNICE r in rovnice)
            {
                try
                {
                    t.Text = r.ToString();
                    r.vycisli_rovnici();
                    l.Text = r.ToString();
                    MessageBox.Show("ano", "správně", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (CHYBA ch)
                {
                    MessageBox.Show(ch.Message, "chyba", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                catch (Exception e)
                {
                    MessageBox.Show("Neočekávaná chyba!!!: " + e.Message, "chyba", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

            }
        }

        static void priprav_test (TextBox t)
        {
            List<string> data = new();
            data.Add("H2+O2>H2O");
            data.Add("Hs>Lf>Hs");
            data.Add("Hs>");
            data.Add(">LO");
            data.Add("Po++Gh>LO");
            data.Add("Hss>Hss");
            data.Add("H>O");
            data.Add("O2>O3");
            data.Add("NO2>N2O3");
            data.Add("N2O+N2O3>NO");
            data.Add("N2O+N2O5>NO+NO2");
            data.Add("H2+O2>H2O");
            data.Add("H2+O2>H2O2");
            data.Add("O2+O3>H2");
            data.Add("Na+H2O>NaOH");
            data.Add("Fe2O3+C>FeO+CO");
            data.Add("Fe3O4+CO>FeO+CO2");
            data.Add("H2SO4 + Na > H2 + Na2SO4");
            data.Add("HCl+K2Cr2O7>Cl2+CrCl3+KCl+H2O");
            data.Add("Na+KNO3>Na2O+K2O+N2");
            data.Add("N2O4+H2O>HNO2+HNO3");
            data.Add("C2H4+O2>C6H12O6");
            data.Add("CH4+O2>C6H12O6+H2O");

            foreach (string s in data)
            {
                try
                {
                    rovnice.Add(new ROVNICE(s));
                }
                catch (CHYBA ch)
                {
                    t.Text = s;
                    MessageBox.Show(ch.Message, "chyba", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                catch (Exception e)
                {
                    t.Text = s;
                    MessageBox.Show("Neočekávaná chyba!!!: " + e.Message, "chyba", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
    }
}
