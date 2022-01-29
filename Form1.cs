using System;
using System.Drawing;
using System.Windows.Forms;

namespace vycislovac_rovnic
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        int a = 0;

        private void napovedaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Zde se zobrazí nápověda k programu.", "nápověda", MessageBoxButtons.OK, MessageBoxIcon.Information);
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
                MessageBox.Show(ch.Message, "chyba", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception)
            {
                MessageBox.Show("Neočekávaná chyba!!!", "chyba", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
