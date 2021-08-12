﻿using LogicaNegocios;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace Ventas
{
    public partial class Form1 : Form
    {
        public static string NombreUsuario = "";
        public Form1()
        {
            
            InitializeComponent();
            cmbBaseDeDatos.DataSource = Enum.GetValues(typeof(SeleccionBaseDeDatos.TipoBaseDeDatos));
        }

        public void button1_Click(object sender, EventArgs e)
        {
            try
            {
                if (cmbBaseDeDatos.SelectedValue.ToString()== "SQL")
                {
                    Global.TipoBaseDeDatos = SeleccionBaseDeDatos.TipoBaseDeDatos.SQL;
                    Global.FuenteDeDatos = "Server=localhost;Database=VENTAS_DB;Trusted_Connection=True;";
                    //MessageBox.Show("Se conecto a sql");
                }
                else
                {
                    Global.TipoBaseDeDatos = SeleccionBaseDeDatos.TipoBaseDeDatos.Excel;
                    Global.FuenteDeDatos = Path.Combine(Environment.CurrentDirectory, "ExcelDB", "Database.xlsx");
                }

                Empleado empleado = new Empleado(Global.TipoBaseDeDatos, Global.FuenteDeDatos);

                string usuario = textBoxUser.Text;

                string contraseña = textBoxPassword.Text;

                empleado.Login(usuario, contraseña);

                Global.Usuarioid = empleado.Id;
                Global.NombreUsuario = empleado.Nombre;


                //if (empleado.Login(usuario, contraseña))
                //{
                //    //vaciamos el contenido del usuario a una variable a fin de llamarlo en el menu
                //    NombreUsuario = usuario;

                this.Hide();
                new Menu().ShowDialog();
                this.Show();

                //    //MessageBox.Show("Conexion Satisfactoria");
                //}
                //else
                //{
                //    throw new Exception("Error de Conexión");
                //}
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
