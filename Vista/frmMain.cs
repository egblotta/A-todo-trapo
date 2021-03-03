using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace A_todo_trapo
{
    public partial class frmMain : Form
    {
        public frmMain()
        {
            InitializeComponent();
        }

        private void articulosToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Hide();
            frmArticulos cl = new frmArticulos();
            cl.Show();
        }

        private void ventasToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            this.Hide();
            frmVentas cl = new frmVentas();
            cl.Show();
        }

        private void historialDeVentasToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Hide();
            frmHistorial fr = new frmHistorial();
            fr.Show();
        }

        private void salirToolStripMenuItem_Click(object sender, EventArgs e)   //Salir del programa
        {
            if (MessageBox.Show("¿Desea Salir?", "Salir", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1) == System.Windows.Forms.DialogResult.Yes)
            {
                Application.Exit();
            }
        }
    }
}
