using Dominio;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Negocio;

namespace TPFinalNivel2_MuñozSalas
{
    public partial class VerDetalle : Form
    {
        private Articulos articulo = null;
        public VerDetalle()
        {
            InitializeComponent();
        }

        public VerDetalle(Articulos articulo)
        {
            InitializeComponent();
            this.articulo = articulo;
            Text = "Ver Detalle";
        }

        private void cargarImagen(string imagen)
        {
            try
            {
                pbArticulo.SizeMode = PictureBoxSizeMode.Zoom;
                pbArticulo.Load(imagen);
            }
            catch (Exception)
            {
                //MessageBox.Show("No se pudo encontrar la imagen");
                pbArticulo.Load("https://cdn.icon-icons.com/icons2/3001/PNG/512/default_filetype_file_empty_document_icon_187718.png");
            }
        }

        private void VerDetalle_Load(object sender, EventArgs e)
        {
            negocioMarca aux = new negocioMarca();
            negocioCategoria aux2 = new negocioCategoria();

            try
            {
                cbMarca.DataSource = aux.listar();
                cbMarca.ValueMember = "Id";
                cbMarca.DisplayMember = "DescripcionMarca";
                cbCategoria.DataSource = aux2.listar();
                cbCategoria.ValueMember = "Id";
                cbCategoria.DisplayMember = "DescripcionCategoria";

                if (articulo != null)
                {
                    txtCodigo.Text = articulo.Codigo;
                    txtCodigo.Enabled = false;
                    txtNombre.Text = articulo.Nombre;
                    txtNombre.Enabled = false;
                    txtDescripcion.Text = articulo.Descripcion;
                    txtDescripcion.Enabled = false;
                    txtImagen.Text = articulo.UrlImagen;
                    txtImagen.Enabled = false;
                    cargarImagen(articulo.UrlImagen);
                    txtPrecio.Text = articulo.Precio.ToString("0.00");
                    txtPrecio.Enabled = false;
                    cbMarca.SelectedValue = articulo.Marca.Id;
                    cbMarca.Enabled = false;
                    cbCategoria.SelectedValue = articulo.Categoria.Id;
                    cbCategoria.Enabled = false;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void btnCancelar_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
