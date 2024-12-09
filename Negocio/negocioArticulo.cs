using Dominio;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Negocio
{
    public class negocioArticulo
    {
        public List<Articulos> Listar()
        {
            List<Articulos> lista = new List<Articulos>();
            SqlConnection conexion = new SqlConnection();
            SqlCommand comando = new SqlCommand();
            SqlDataReader lector;

            try
            {
                conexion.ConnectionString = "server=.\\SQLEXPRESS; database=CATALOGO_DB; integrated security=true";
                comando.CommandType = System.Data.CommandType.Text;
                comando.CommandText = "Select A.Id, Codigo, Nombre, A.Descripcion, IdMarca, IdCategoria, ImagenUrl, Precio, C.Descripcion Categoria, M.Descripcion Marca, C.Id as IdCategoria, M.Id as IdMarca from ARTICULOS A, CATEGORIAS C, MARCAS M WHERE C.Id = A.IdCategoria AND M.Id = A.IdMarca;";
                comando.Connection = conexion;

                conexion.Open();
                lector = comando.ExecuteReader();

                while (lector.Read())
                {
                    Articulos aux = new Articulos();
                    aux.Id = lector.GetInt32(0);
                    aux.Codigo = (string)lector["Codigo"];
                    aux.Nombre = (string)lector["Nombre"];
                    aux.Descripcion = (string)lector["Descripcion"];
                    if (!(lector["ImagenUrl"] is DBNull))
                    aux.UrlImagen = (string)lector["ImagenUrl"];
                    aux.Precio = (decimal)lector["Precio"];
                    aux.Marca = new Marcas();
                    aux.Marca.Id = (int)lector["IdMarca"];
                    aux.Marca.DescripcionMarca = (string)lector["Marca"];
                    //aux.IdMarca.Id = lector.GetInt32(0);
                    aux.Categoria = new Categorias();
                    aux.Categoria.Id = (int)lector["IdCategoria"];
                    aux.Categoria.DescripcionCategoria = (string)lector["Categoria"];
                    //aux.IdCategoria.Id = lector.GetInt32(0);

                    lista.Add(aux);
                }

                conexion.Close();
                return lista;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void AgregarArticulo(Articulos articulo)
        {
            AccesoDatos datos = new AccesoDatos();
            try
            {
                datos.setearConsulta("Insert into ARTICULOS (Codigo,Nombre,Descripcion,Precio,IdMarca,IdCategoria,ImagenUrl) Values ('"+articulo.Codigo+"','"+articulo.Nombre+"','"+articulo.Descripcion+"',"+articulo.Precio+", @IdMarca, @IdCategoria, @Imagen);");
                datos.setearParametros("@IdMarca",articulo.Categoria.Id);
                datos.setearParametros("@IdCategoria",articulo.Marca.Id);
                datos.setearParametros("@Imagen",articulo.UrlImagen);
                datos.EjecutarAccion();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                datos.cerrarConexion();
            }
        }

        public void ModificarArticulo(Articulos articulo)
        {
            AccesoDatos datos = new AccesoDatos();
            try
            {
                datos.setearConsulta("UPDATE ARTICULOS SET Codigo = @CODIGO, Nombre = @NOMBRE, Descripcion = @DESCRIPCION, IdMarca = @IDMARCA, IdCategoria = @IDCATEGORIA, ImagenUrl = @ImagenUrl, Precio = @Precio WHERE Id = @Id;");
                datos.setearParametros("@CODIGO",articulo.Codigo);
                datos.setearParametros("@NOMBRE",articulo.Nombre);
                datos.setearParametros("@DESCRIPCION",articulo.Descripcion);
                datos.setearParametros("@IDMARCA",articulo.Marca.Id);
                datos.setearParametros("@IDCATEGORIA", articulo.Categoria.Id);
                datos.setearParametros("@ImagenUrl", articulo.UrlImagen);
                datos.setearParametros("@Precio", articulo.Precio);
                datos.setearParametros("@Id",articulo.Id);
                datos.EjecutarAccion();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                datos.cerrarConexion();
            }
        }

        public void EliminarArticulo(int Id)
        {
            try
            {
                AccesoDatos datos = new AccesoDatos();
                datos.setearConsulta("DELETE FROM ARTICULOS WHERE Id = @Id");
                datos.setearParametros("@Id",Id);
                datos.EjecutarAccion();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<Articulos> filtrar(string campo, string criterio, string filtro)
        {
            List<Articulos> articulos = new List<Articulos>();
            AccesoDatos datos = new AccesoDatos();
            try
            {
                string consulta = "Select A.Id, Codigo, Nombre, A.Descripcion, IdMarca, IdCategoria, ImagenUrl, Precio, C.Descripcion Categoria, M.Descripcion Marca, C.Id as IdCategoria, M.Id as IdMarca from ARTICULOS A, CATEGORIAS C, MARCAS M WHERE C.Id = A.IdCategoria AND M.Id = A.IdMarca AND ";
                if (campo == "Precio") 
                {
                    switch (criterio)
                    {
                        case "Mayor a:":
                            consulta += "Precio >" + filtro;
                            break;
                        case "Menor a:":
                            consulta += "Precio <" + filtro;
                            break;
                        default:
                            consulta += "Precio =" + filtro;
                            break;
                    }
                }
                else
                {
                    switch (criterio)
                    {
                        case "Comienza con":
                            consulta += "C.Descripcion LIKE '" + filtro + "%'";
                            break;
                        case "Termina con":
                            consulta += "C.Descripcion LIKE '%" + filtro +"'";
                            break;
                        default:
                            consulta += "C.Descripcion LIKE '%" + filtro + "%'";
                            break;
                    }
                }
                datos.setearConsulta(consulta);
                datos.ejecutarLectura();
                while (datos.Lector.Read())
                {
                    Articulos aux = new Articulos();
                    aux.Id = datos.Lector.GetInt32(0);
                    aux.Codigo = (string)datos.Lector["Codigo"];
                    aux.Nombre = (string)datos.Lector["Nombre"];
                    aux.Descripcion = (string)datos.Lector["Descripcion"];
                    if (!(datos.Lector["ImagenUrl"] is DBNull))
                        aux.UrlImagen = (string)datos.Lector["ImagenUrl"];
                    aux.Precio = (decimal)datos.Lector["Precio"];
                    aux.Marca = new Marcas();
                    aux.Marca.Id = (int)datos.Lector["IdMarca"];
                    aux.Marca.DescripcionMarca = (string)datos.Lector["Marca"];
                    //aux.IdMarca.Id = lector.GetInt32(0);
                    aux.Categoria = new Categorias();
                    aux.Categoria.Id = (int)datos.Lector["IdCategoria"];
                    aux.Categoria.DescripcionCategoria = (string)datos.Lector["Categoria"];
                    //aux.IdCategoria.Id = lector.GetInt32(0);
                    articulos.Add(aux);
                }
                return articulos;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
