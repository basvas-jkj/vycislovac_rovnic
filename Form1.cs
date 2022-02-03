using System;
using System.IO;
using System.Windows.Forms;

namespace vycislovac_rovnic
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                ROVNICE r = new(textBox1.Text);
                r.vycisli_rovnici();
                label1.Text = r.ToString();
            }
            catch (CHYBA ch)
            {
                label1.Text = ch.Message;
                MessageBox.Show(ch.Message, "chyba", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception)
            {
                label1.Text = "Neočekávaná chyba!!!";
                MessageBox.Show("Neočekávaná chyba!!!", "chyba", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void napovedaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            StringWriter sw = new();
            sw.Write("Vstup má podobu chemické rovnice bez stechiometrických koeficientů. ");
            sw.Write("Znaménko '+' odděluje jednotlivé sloučeniny, '>' odděluje strany rovnice. ");
            sw.Write("Sloučenina musí být napsaná bez mezer, nikde jinde na mezerách nezáleží. ");
            sw.Write("Značka prvku musí začínat velkým písmenem a může mít maximáně dvě písmena. ");

            MessageBox.Show(sw.ToString(), "nápověda", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void vzorovyVstupToolStripMenuItem_Click(object sender, EventArgs e)
        {
            textBox1.Text = "Fe2O3+C>FeO+CO";
            button1_Click(sender, e);
        }

        private void omezeniToolStripMenuItem_Click(object sender, EventArgs e)
        {
            StringWriter sw = new();
            sw.Write("Program neumí vyčíslit sloučeniny, ve kterých je počet sloučenin na obou stranách rovnice jiný než právě o jedna vyšší než počet prvků. ");
            sw.Write("Závorky jsou ve vzorcích povoleny, ale pouze kulaté a nelze je zanořovat. Není proto možné vyčíslovat reakce s komplexními sloučeninami.");
            sw.Write("Program neumí zpracovat prvky, jejichž značka je delší než dvě písmena. ");

            MessageBox.Show(sw.ToString(), "nápověda", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
    }
}
