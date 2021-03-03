using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.OleDb;
using A_todo_trapo.Clases;

namespace A_todo_trapo
{
    public partial class frmArticulos : Form
    {
        //iniciamos la conexion
        OleDbConnection con = new OleDbConnection("Provider=Microsoft.ACE.OLEDB.12.0;Data Source=DB.accdb");
        List<Articulos> articulos = new List<Articulos>(); //almacenará una lista de objetos articulos

        public frmArticulos()
        {
            InitializeComponent();
        }

        private void frmArticulos_Load(object sender, EventArgs e)
        {
            bool estaConectado = false;

            try
            {
                con.Open();
                //MessageBox.Show(cn.State.ToString());
                estaConectado = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show("No se pudo conectar: \n" + ex);
                estaConectado = false;
            }
            // Si logramos conectarnos entonces vamos a leer la DB
            if (estaConectado == true)
            {
                // Leemos todos los datos de la tabla clientes
                OleDbCommand cmd = new OleDbCommand("select * from Articulos", con);

                // Creamos un reader, que nos permitirá ejecutar la consulta
                OleDbDataReader r = cmd.ExecuteReader();

                int columnas = r.FieldCount; // Obtenemos la cantidad de columnas

                if (r.HasRows)//Tenemos una o mas filas
                {
                    while (r.Read()) // Recorremos todas las filas
                    {
                        Articulos ar = new Articulos();
                        ar.Codigo = r.GetInt32(0); // valor de la celda correspondiente a la fila actual y primera columna
                        ar.Nombre = r.GetString(1); // valor de la segunda columna
                        ar.Cantidad = r.GetInt32(2);
                        ar.PrecioCosto = r.GetInt32(3);
                        ar.PrecioVenta = r.GetInt32(4);

                        articulos.Add(ar);
                    }

                    inicializarTablaArticulos();
                    //Mostramos los datos en la grilla
                    for (int i = 0; i < articulos.Count(); i++)
                    {
                        tablaArticulos.Rows.Add(articulos[i].Codigo, articulos[i].Nombre, articulos[i].Cantidad,
                            articulos[i].PrecioCosto, articulos[i].PrecioVenta);
                    }

                    articulos.Clear(); //limpio la lista de clientes para utilizarla en otros lugares

                    //tablaArticulos_CellClick(this, null);
                    updateColor();

                }
                else // No hay filas para leer
                {
                    MessageBox.Show("No tenemos filas");
                }
                r.Close();
            }
        }

        private void inicializarTablaArticulos()
        {
            //Inicializamos la grilla
            tablaArticulos.AllowUserToAddRows = false;
            tablaArticulos.AllowUserToDeleteRows = false;
            tablaArticulos.AllowUserToOrderColumns = false;
            tablaArticulos.ReadOnly = true;
            tablaArticulos.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            tablaArticulos.EditMode = DataGridViewEditMode.EditProgrammatically;
            tablaArticulos.MultiSelect = false;
            tablaArticulos.AutoResizeColumns();
            //tablaArticulos.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;            

            tablaArticulos.Columns.Add("codigo", "Codigo");
            tablaArticulos.Columns.Add("nombre", "Nombre");
            tablaArticulos.Columns.Add("cantidad", "Cantidad");
            tablaArticulos.Columns.Add("precioCosto", "Costo");
            tablaArticulos.Columns.Add("precioVenta", "Venta");

            tablaArticulos.Columns[0].Width = 50;
            tablaArticulos.Columns[1].Width = 340;
            tablaArticulos.Columns[2].Width = 60;
            tablaArticulos.Columns[3].Width = 50;
            tablaArticulos.Columns[4].Width = 50;

            tablaArticulos.Columns[0].HeaderCell.Style.Alignment = DataGridViewContentAlignment.BottomCenter;
            tablaArticulos.Columns[1].HeaderCell.Style.Alignment = DataGridViewContentAlignment.BottomCenter;
            tablaArticulos.Columns[2].HeaderCell.Style.Alignment = DataGridViewContentAlignment.BottomCenter;
            tablaArticulos.Columns[3].HeaderCell.Style.Alignment = DataGridViewContentAlignment.BottomCenter;
            tablaArticulos.Columns[4].HeaderCell.Style.Alignment = DataGridViewContentAlignment.BottomCenter;
        }

        private void actualizarTablaArticulos()
        {
            //Limpiamos la grilla
            tablaArticulos.Rows.Clear();
            tablaArticulos.Refresh();

            // Leemos todos los datos de la tabla clientes
            OleDbCommand cmd = new OleDbCommand("select * from Articulos", con);     //consulta SQL

            // Creamos un reader, que nos permitirá ejecutar la consulta
            OleDbDataReader r = cmd.ExecuteReader();

            if (r.HasRows) //Tenemos una o mas filas
            {
                while (r.Read()) // Recorremos todas las filas
                {
                    Articulos ar = new Articulos();
                    ar.Codigo = r.GetInt32(0); // valor de la celda correspondiente a la fila actual y primera columna
                    ar.Nombre = r.GetString(1); // valor de la segunda columna
                    ar.Cantidad = r.GetInt32(2);
                    ar.PrecioCosto = r.GetInt32(3);
                    ar.PrecioVenta = r.GetInt32(4);

                    articulos.Add(ar);
                }

                //Mostramos los datos en la grilla
                for (int i = 0; i < articulos.Count(); i++)
                {
                    tablaArticulos.Rows.Add(articulos[i].Codigo, articulos[i].Nombre, articulos[i].Cantidad, articulos[i].PrecioCosto,
                        articulos[i].PrecioVenta);
                }

                updateColor();
                articulos.Clear(); //limpio la lista de articulos para utilizarla en otros lugares
            }
            r.Close();
        }

        private void tablaArticulos_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            txtCodigo.Text = tablaArticulos.CurrentRow.Cells[0].Value.ToString();
            txtNombre.Text = tablaArticulos.CurrentRow.Cells[1].Value.ToString();
            txtCantidad.Text = tablaArticulos.CurrentRow.Cells[2].Value.ToString();
            txtPrecioCosto.Text = tablaArticulos.CurrentRow.Cells[3].Value.ToString();
            txtPrecioVenta.Text = tablaArticulos.CurrentRow.Cells[4].Value.ToString();
        }

        private void btnLimpiar_Click(object sender, EventArgs e)
        {
            limpiarCampos();
        }

        private void btnNuevo_Click(object sender, EventArgs e)
        {
            if (con.State == ConnectionState.Open)
            {
                if (String.IsNullOrEmpty(txtNombre.Text) || String.IsNullOrEmpty(txtPrecioCosto.Text)
                    || String.IsNullOrEmpty(txtPrecioVenta.Text) || String.IsNullOrEmpty(txtCantidad.Text))
                {
                    MessageBox.Show("Ningun campo puede estar vacio");
                }
                else
                {

                    string nombre = txtNombre.Text;
                    int cantidad = Convert.ToInt32(txtCantidad.Text);
                    float precioCosto = Convert.ToSingle(txtPrecioCosto.Text);
                    float precioVenta = Convert.ToSingle(txtPrecioVenta.Text);

                    //Escribimos el comando de inserción
                    //El ID de la persona es autonumérico por lo cual no necesitamos establecerlo

                    string strinsert = "INSERT into Articulos (Nombre, Cantidad, PrecioCosto, PrecioVenta) " +
                                        "Values(@nom, @cant, @preC, @preV)";
                    OleDbCommand cmd = new OleDbCommand(strinsert, con);

                    //Establecemos los parámetros que se utilizarán en el comando Insert
                    cmd.Parameters.AddWithValue("nom", nombre);
                    cmd.Parameters.AddWithValue("cant", cantidad);
                    cmd.Parameters.AddWithValue("preC", precioCosto);
                    cmd.Parameters.AddWithValue("preV", precioVenta);

                    try
                    {
                        cmd.ExecuteNonQuery(); //Ejecutamos el comando
                        MessageBox.Show("Registro agregado");
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Error al ingresar el articulo " + ex);
                        throw;
                    }

                    //Actualizamos la grilla y limpiamos los campos
                    actualizarTablaArticulos();
                    limpiarCampos();
                }
            }
        }

        private void btnModificar_Click(object sender, EventArgs e)
        {
            if (con.State == ConnectionState.Open)
            {
                if (String.IsNullOrEmpty(txtNombre.Text) || String.IsNullOrEmpty(txtPrecioCosto.Text) || String.IsNullOrEmpty(txtCantidad.Text))
                {
                    MessageBox.Show("Ningun campo puede estar vacio");
                }

                else
                {
                    string nombre = txtNombre.Text;
                    float precioCosto = Convert.ToSingle(txtPrecioCosto.Text);
                    float precioVenta = Convert.ToSingle(txtPrecioVenta.Text);
                    int codigo = Convert.ToInt32(txtCodigo.Text);
                    int cantidad = Convert.ToInt32(txtCantidad.Text);

                    //Escribimos el comando de actualización
                    string strupdate = "UPDATE Articulos Set Nombre = @nom, Cantidad = @cant," +
                        " PrecioCosto = @preC, PrecioVenta = @preV WHERE Codigo = @codigo";

                    OleDbCommand cmd = new OleDbCommand(strupdate, con);

                    //Establecemos los parámetros que se utilizarán en el comando Update
                    cmd.Parameters.AddWithValue("nom", nombre);
                    cmd.Parameters.AddWithValue("cant", cantidad);
                    cmd.Parameters.AddWithValue("preC", precioCosto);
                    cmd.Parameters.AddWithValue("preV", precioVenta);
                    cmd.Parameters.AddWithValue("codigo", codigo);

                    try
                    {
                        cmd.ExecuteNonQuery(); //Ejecutamos el comando
                        MessageBox.Show("Registro modificado");
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Error al modificar el registro " + ex);
                    }

                    //Actualizamos la grilla
                    actualizarTablaArticulos();
                }
            }
        }

        private void btnBorrar_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("¿Desea elimar el articulo?", "Eliminar articulo", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1) == System.Windows.Forms.DialogResult.Yes)
            {
                if (con.State == ConnectionState.Open)
                {

                    int cod = Convert.ToInt32(txtCodigo.Text);
                    //Escribimos el comando de eliminacion
                    string strdelete = "DELETE FROM Articulos WHERE codigo = @codigo;";

                    OleDbCommand cmd = new OleDbCommand(strdelete, con);
                    //Establecemos los parámetros que se utilizarán en el comando delete
                    cmd.Parameters.AddWithValue("codigo", cod);

                    cmd.ExecuteNonQuery(); //Ejecutamos el comando

                    MessageBox.Show("Registro eliminado");

                    //Actualizamos la grilla y limpiamos los campos
                    actualizarTablaArticulos();
                    limpiarCampos();
                }

            }
        }

        private void btnVolver_Click(object sender, EventArgs e)
        {
            this.Hide();
            frmMain fr = new frmMain();
            fr.Show();
        }

        private void updateColor()
        {

            foreach (DataGridViewRow row in tablaArticulos.Rows)
            {
                if (Convert.ToInt32(row.Cells["cantidad"].Value) <= 10)
                {
                    //txtCantidad.BackColor = Color.IndianRed;
                    row.DefaultCellStyle.BackColor = Color.IndianRed;
                }

                if (Convert.ToInt32(row.Cells["cantidad"].Value) > 10 && Convert.ToInt32(row.Cells["cantidad"].Value) <= 20)
                {
                    //txtCantidad.BackColor = Color.Orange;
                    row.DefaultCellStyle.BackColor = Color.Orange;
                }

                if (Convert.ToInt32(row.Cells["cantidad"].Value) > 20)
                {
                    //txtCantidad.BackColor = Color.SeaGreen;
                    row.DefaultCellStyle.BackColor = Color.SeaGreen;
                }
            }

        }

        private void limpiarCampos()
        {
            txtNombre.Text = "";
            txtCantidad.Text = "";
            txtPrecioCosto.Text = "";
            txtPrecioVenta.Text = "";
        }        
    }
}
