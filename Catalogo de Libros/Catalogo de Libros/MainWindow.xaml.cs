using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Data.OleDb;//Agregamos libreria OleDB
using System.Data; //Agregamos System.Data

namespace Catalogo_de_Libros
{
    /// <summary>
    /// Lógica de interacción para MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        OleDbConnection con; //Agregamos la conexión
        DataTable dt;   //Agregamos la tabla
        public MainWindow()
        {
            InitializeComponent();
            //Conectamos la base de datos a nuestro archivo Access
            con = new OleDbConnection();
            con.ConnectionString = "Provider=Microsoft.Jet.Oledb.4.0; Data Source=" + AppDomain.CurrentDomain.BaseDirectory + "\\GADIVA.mdb";
            MostrarDatos();
        }

        private void MostrarDatos()
        {
           OleDbCommand cmd = new OleDbCommand();
            if (con.State != ConnectionState.Open)
                con.Open();
            cmd.Connection = con;
            cmd.CommandText = "select * from PedidosGadiva";
            OleDbDataAdapter da = new OleDbDataAdapter(cmd);
            dt = new DataTable();
            da.Fill(dt);
            gvDatos.ItemsSource = dt.AsDataView();

            if (dt.Rows.Count > 0)
            {
                lbContenido.Visibility = System.Windows.Visibility.Hidden;
                gvDatos.Visibility = System.Windows.Visibility.Visible;
            }
            else
            {
                lbContenido.Visibility = System.Windows.Visibility.Visible;
                gvDatos.Visibility = System.Windows.Visibility.Hidden;
            }
        }
    

        private void BtnNuevo_Click(object sender, RoutedEventArgs e)
        {
            OleDbCommand cmd = new OleDbCommand();
            if (con.State != ConnectionState.Open)
                con.Open();
            cmd.Connection = con;

            if (txtCuenta.Text != "")
            {
                if (txtCuenta.IsEnabled == true)
                {
                    cmd.CommandText = "insert into PedidosGadiva(Cuenta,Semilla,Color,Cantidad,Comprador,Telefono,Frase) " +
                        "Values(" + txtCuenta.Text + ",'" + cbSemilla.Text + "','" + cbColor.Text + "','"
                        + txtCantidad.Text + "','" + txtComprador.Text + "','" + txtTelefono.Text + "','"
                        + txtFrase.Text + "')";
                    cmd.ExecuteNonQuery();
                    MostrarDatos();
                    MessageBox.Show("Nuevo pedido agregado correctamente...");
                    LimpiaTodo();
                }
                else
                {
                    cmd.CommandText = "update PedidosGadiva set Cuenta='" + txtCuenta.Text + "',Semilla='" + cbSemilla.Text +
                         "',Color='" + cbColor.Text + "',Cantidad='" + txtCantidad.Text + "',Comprador='" + txtComprador.Text + "',Telefono='" + txtTelefono.Text +
                        "',Frase='" + txtFrase.Text + "' where Cuenta=" + txtCuenta.Text;
                    cmd.ExecuteNonQuery();
                    MostrarDatos();
                    MessageBox.Show("Datos del pedido Actualizados...");
                    LimpiaTodo();
                }
            }
            else
            {
                MessageBox.Show("Favor de poner la Id del pedido");
            }  
        }
    

        private void BtnEditar_Click(object sender, RoutedEventArgs e)
        {
            if (gvDatos.SelectedItems.Count > 0)
            {
                DataRowView row = (DataRowView)gvDatos.SelectedItems[0];
                txtCuenta.Text = row["Cuenta"].ToString();
                cbSemilla.Text = row["Semilla"].ToString();
                cbColor.Text = row["Color"].ToString();
                txtCantidad.Text = row["Cantidad"].ToString();
                txtComprador.Text = row["Comprador"].ToString();
                txtTelefono.Text = row["Telefono"].ToString();
                txtFrase.Text = row["Frase"].ToString();
                txtCuenta.IsEnabled = false;
                btnNuevo.Content = "Actualizar";
            }
            else
            {
                MessageBox.Show("Favor de seleccionar el numero de cuenta que desea editar");
            }
        }

        private void BtnEliminar_Click(object sender, RoutedEventArgs e)
        {
            if (gvDatos.SelectedItems.Count > 0)
            {
                DataRowView row = (DataRowView)gvDatos.SelectedItems[0];

                OleDbCommand cmd = new OleDbCommand();
                if (con.State != ConnectionState.Open)
                    con.Open();
                cmd.Connection = con;
               cmd.CommandText = "delete from PedidosGadiva where Id=" + row["Cuenta"].ToString();
                cmd.ExecuteNonQuery();
                MostrarDatos();
                MessageBox.Show("Pedido cancelado");
                LimpiaTodo();
            }
            else
            {
                MessageBox.Show("Favor de elegir el pedido que desea eliminar");
            }
        }

        private void BtnLimpiar_Click(object sender, RoutedEventArgs e)
        {
            LimpiaTodo();
        }

        private void BtnSalir_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void LimpiaTodo()
        {
            txtCuenta.Text = "";
            cbSemilla.SelectedIndex = 0;
            cbColor.SelectedIndex = 0;
            txtCantidad.Text = "";
            txtComprador.Text = "";
            txtTelefono.Text = "";
            txtFrase.Text = "";
            btnNuevo.Content = "Nuevo";
            txtCuenta.IsEnabled = true;
        }
    }
}
