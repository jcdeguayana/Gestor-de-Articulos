using Dominio;
using Negocio;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TPFinalNivel2_MuñozSalas
{
    public partial class ListadoDeArticulos : Form
    {
        private List<Articulos> articulos;
        public ListadoDeArticulos(string Usuario)
        {
            InitializeComponent();
            label1.Text = "Usuario: " + Usuario;
        }

        private void ListadoDeArticulos_Load(object sender, EventArgs e)
        {
            Cargar();
            cbCampo.Items.Add("Categoria");
            cbCampo.Items.Add("Precio");
        }

        private void Cargar()
        {
            try
            {
                negocioArticulo obj = new negocioArticulo();
                articulos = obj.Listar();

                dgvArticulos.DataSource = articulos;
                OcultarColumnas();
                cargarImagen(articulos[0].UrlImagen);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void OcultarColumnas()
        {
            dgvArticulos.Columns["Id"].Visible = false;
            dgvArticulos.Columns["UrlImagen"].Visible = false;
        }

        private void cargarImagen(string imagen)
        {
            try
            {
                pbArticulos.SizeMode = PictureBoxSizeMode.Zoom;
                pbArticulos.Load(imagen);
            }
            catch (Exception)
            {
                //MessageBox.Show("No se pudo encontrar la imagen");
                pbArticulos.Load("https://cdn.icon-icons.com/icons2/3001/PNG/512/default_filetype_file_empty_document_icon_187718.png");
            }
        }

        private void btnAgregar_Click(object sender, EventArgs e)
        {
            AltaArticulo alta = new AltaArticulo();
            alta.ShowDialog();
            Cargar();
        }

        private void dgvArticulos_SelectionChanged(object sender, EventArgs e)
        {
            if (dgvArticulos.CurrentRow != null)
            {
                Articulos seleccionado = (Articulos)dgvArticulos.CurrentRow.DataBoundItem;
                cargarImagen(seleccionado.UrlImagen);
            }
        }

        private void btnModificar_Click(object sender, EventArgs e)
        {
            if (dgvArticulos.Rows.Count == 0)
            {
                MessageBox.Show("No hay artículos disponibles para modificar.");
                return;
            }

            if (dgvArticulos.CurrentRow == null || dgvArticulos.CurrentRow.DataBoundItem == null)
            {
                MessageBox.Show("Seleccione un artículo para modificar.");
                return;
            }
            Articulos seleccionado;
            seleccionado = (Articulos)dgvArticulos.CurrentRow.DataBoundItem;
            AltaArticulo modificar = new AltaArticulo(seleccionado);
            modificar.ShowDialog();
            Cargar();
        }

        private void btnEliminar_Click(object sender, EventArgs e)
        {
            if (dgvArticulos.Rows.Count == 0)
            {
                MessageBox.Show("No hay artículos disponibles para eliminar.");
                return;
            }

            if (dgvArticulos.CurrentRow == null || dgvArticulos.CurrentRow.DataBoundItem == null)
            {
                MessageBox.Show("Seleccione un artículo para eliminar.");
                return;
            }
            negocioArticulo negocio = new negocioArticulo();
            Articulos articulo = new Articulos();
            try
            {
                DialogResult result = MessageBox.Show("Confirmar","Eliminando",MessageBoxButtons.YesNo,MessageBoxIcon.Warning);
                if (result == DialogResult.Yes)
                {
                    articulo = (Articulos)dgvArticulos.CurrentRow.DataBoundItem;
                    negocio.EliminarArticulo(articulo.Id);
                    Cargar();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private bool ValidarFiltro()
        {
            if (cbCampo.SelectedIndex < 0 || cbCriterio.SelectedIndex < 0)
            {
                MessageBox.Show("Seleccionar una opción");
                return true;
            }
            if (cbCampo.SelectedItem.ToString() == "Precio")
            {
               if (string.IsNullOrEmpty(txtFiltro2.Text))
               {
                    MessageBox.Show("Completar campo númerico");
                    return true;
                }
               if (!(ValidarNumero(txtFiltro2.Text)))
               {
                    MessageBox.Show("Solo números por favor");
                    return true;
               }
               
            }
            return false;
        }

        private bool ValidarNumero(string cadena)
        {
            foreach (char caracter in cadena)
            {
                if (!char.IsNumber(caracter))
                    return false;
            }
            return true;
        }

        private void btnFiltrar_Click(object sender, EventArgs e)
        {
            negocioArticulo articulo = new negocioArticulo();
            try
            {
                if (ValidarFiltro())
                    return;
                string Campo = cbCampo.SelectedItem.ToString();
                string Criterio = cbCriterio.SelectedItem.ToString();
                string Filtro = txtFiltro2.Text;
                dgvArticulos.DataSource = articulo.filtrar(Campo,Criterio,Filtro);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }

        }

        private void txtFiltro_KeyPress(object sender, KeyPressEventArgs e)
        {

        }

        private void txtFiltro_TextChanged(object sender, EventArgs e)
        {
            List<Articulos> Filtro;
            string filtro = txtFiltro.Text;

            if (filtro.Length > 2)
            {
                Filtro = articulos.FindAll(x => x.Nombre.ToLower().Contains(filtro.ToLower()) || x.Marca.ToString().ToLower().Contains(filtro.ToLower()));
            }
            else
            {
                Filtro = articulos;
            }
            dgvArticulos.DataSource = null;
            dgvArticulos.DataSource = Filtro;
            OcultarColumnas();
        }

        private void cbCampo_SelectedIndexChanged(object sender, EventArgs e)
        {
            string Opcion = cbCampo.SelectedItem.ToString();

            if (Opcion == "Precio")
            {
                cbCriterio.Items.Clear();
                cbCriterio.Items.Add("Mayor a:");
                cbCriterio.Items.Add("Menor a:");
                cbCriterio.Items.Add("Igual a:");
            }
            else
            {
                cbCriterio.Items.Clear();
                cbCriterio.Items.Add("Comienza con");
                cbCriterio.Items.Add("Termina con");
                cbCriterio.Items.Add("Contiene");
            }
        }

        private void btnDetalle_Click(object sender, EventArgs e)
        {
            if (dgvArticulos.Rows.Count == 0)
            {
                MessageBox.Show("No hay artículos disponibles para ver.");
                return;
            }

            if (dgvArticulos.CurrentRow == null || dgvArticulos.CurrentRow.DataBoundItem == null)
            {
                MessageBox.Show("Seleccione un artículo para ver.");
                return;
            }
            Articulos seleccionado;
            seleccionado = (Articulos)dgvArticulos.CurrentRow.DataBoundItem;
            VerDetalle VerDetalle = new VerDetalle(seleccionado);
            VerDetalle.ShowDialog();
            Cargar();
        }
    }
}
