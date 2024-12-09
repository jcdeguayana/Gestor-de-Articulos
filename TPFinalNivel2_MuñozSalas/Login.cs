using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics.Eventing.Reader;
using System.Drawing;
using System.Drawing.Text;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TPFinalNivel2_MuñozSalas
{
    public partial class Login : Form
    {
        public Login()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(txtContraseña.Text) && !string.IsNullOrEmpty(txtUsuario.Text))
            {
                string Usuario = txtUsuario.Text;

                // Pasar el valor de 'Usuario' al segundo formulario
                ListadoDeArticulos form2 = new ListadoDeArticulos(Usuario);
                form2.Show(); // Mostrar el segundo formulario
            }
            else
            {
                MessageBox.Show("Debe completar los campos");
            }
        }

        private void lblUsuario_Click(object sender, EventArgs e)
        {

        }
    }
}
