using System;
using System.IO;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Dominio;
using Negocio;
using System.Configuration;
namespace TPFinalNivel2_MuñozSalas
{
    public partial class AltaArticulo : Form
    {
        private Articulos articulo = null;
        private OpenFileDialog archivo = null;
        public AltaArticulo()
        {
            InitializeComponent();
        }

        public AltaArticulo(Articulos articulo)
        {
            InitializeComponent();
            this.articulo = articulo;
            Text = "Modificar Articulo";
        }
        private void btnCancelar_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private bool ValidarCampos()
        {
            if (txtCodigo.Text == "" || txtNombre.Text == "" || txtDescripcion.Text == "" || txtImagen.Text == "" || txtPrecio.Text == "")
            {
                MessageBox.Show("Debe completar los campos para agregar el articulo");
                return true;
            }
                return false;
        }

        private void btnAceptar_Click(object sender, EventArgs e)
        {
            negocioArticulo negocio = new negocioArticulo();
            //Articulos articulo = new Articulos();
            try
            {
                if (ValidarCampos())
                    return;
                if (articulo == null)
                articulo = new Articulos();
                articulo.Codigo = txtCodigo.Text;
                articulo.Nombre = txtNombre.Text;
                articulo.Descripcion = txtDescripcion.Text;
                articulo.Precio = Convert.ToDecimal(txtPrecio.Text);
                articulo.Marca = (Marcas)cbMarca.SelectedItem;
                articulo.Categoria = (Categorias)cbCategoria.SelectedItem;
                articulo.UrlImagen = txtImagen.Text;

                if(articulo.Id != 0)
                {
                    negocio.ModificarArticulo(articulo);
                    MessageBox.Show("Modificado con éxito");
                }
                else
                {
                    negocio.AgregarArticulo(articulo);
                    MessageBox.Show("Agregado con éxito");
                }

                if(archivo!=null && !(txtImagen.Text.ToUpper().Contains("HTTP")))
                    File.Copy(archivo.FileName, ConfigurationManager.AppSettings["Imagenes"] + archivo.SafeFileName);

                    this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void AltaArticulo_Load(object sender, EventArgs e)
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
                    txtNombre.Text = articulo.Nombre;
                    txtDescripcion.Text = articulo.Descripcion;
                    txtImagen.Text = articulo.UrlImagen;
                    cargarImagen(articulo.UrlImagen);
                    txtPrecio.Text = articulo.Precio.ToString("0.00");
                    cbMarca.SelectedValue = articulo.Marca.Id;
                    cbCategoria.SelectedValue = articulo.Categoria.Id;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void txtImagen_Leave(object sender, EventArgs e)
        {
            cargarImagen(txtImagen.Text);
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

        private void btnAgregarImagen_Click(object sender, EventArgs e)
        {
            archivo = new OpenFileDialog();
            archivo.Filter = "jpg| * .jpg|png| * .png";
            if (archivo.ShowDialog() == DialogResult.OK)
            {
                txtImagen.Text = archivo.FileName;
                cargarImagen(archivo.FileName);

                //File.Copy(archivo.FileName, ConfigurationManager.AppSettings["Imagenes"] + archivo.SafeFileName);
            }
        }
    }
}
