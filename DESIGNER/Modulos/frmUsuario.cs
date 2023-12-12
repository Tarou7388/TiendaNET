using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DESIGNER.Tool;
using CryptSharp;
using System.Windows.Forms;
using BOL;
using ENTITIES;


namespace DESIGNER.Modulos
{
    public partial class frmUsuario : Form
    {

        //Contiene los funciones logicas CRUDS
        Usuario usuario = new Usuario();

        //Datos de la tabla
        EUsuario entUsuario = new EUsuario();

        string nivelAcceso = "INV";

        //Revservar un espacio para el objeto
        DataView dv;

        public frmUsuario()
        {
            InitializeComponent();
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void btnCerrar_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnGuardar_Click(object sender, EventArgs e)
        {
            if (Aviso.Preguntar("¿Desea Guardar el registro?") == DialogResult.Yes)
            {
                entUsuario.apellidos = txtApellidos.Text;
                entUsuario.nombres = txtNombres.Text;
                entUsuario.email = txtEmail.Text;
                entUsuario.claveacceso = Crypter.Blowfish.Crypt(txtClave.Text.Trim());
                entUsuario.nivelAcceso = nivelAcceso;

                if (usuario.Registrar(entUsuario) > 0)
                {
                    Aviso.Informar("Nuevo usuario registrado");
                    reiniciarInterfaz();
                    actualizarDatos();
                }
                else
                {
                    Aviso.Advertir("No hemos podido terminar el registro");
                }
            }
        }

        private void actualizarDatos()
        {
            dv = new DataView(usuario.Listar());
            gridUsuarios.DataSource = dv;
            gridUsuarios.Refresh();

            gridUsuarios.Columns[0].Visible = false;
            gridUsuarios.Columns[4].Visible = false;

            gridUsuarios.Columns[1].Width = 180;
            gridUsuarios.Columns[2].Width = 180;
            gridUsuarios.Columns[3].Width = 270;
            gridUsuarios.Columns[5].Width = 150;
            gridUsuarios.AlternatingRowsDefaultCellStyle.BackColor = Color.AliceBlue;
        } 

        private void reiniciarInterfaz()
        {
            txtApellidos.Clear();
            txtNombres.Clear();
            txtEmail.Clear();
            txtClave.Clear();
            optInvitado.Checked = true;
            nivelAcceso = "INV";
            txtApellidos.Focus();
        }

        private void optAdministrador_CheckedChanged(object sender, EventArgs e)
        {
            nivelAcceso = "ADM";
        }

        private void optInvitado_CheckedChanged(object sender, EventArgs e)
        {
            nivelAcceso = "INV";
        }

        private void frmUsuario_Load(object sender, EventArgs e)
        {
            actualizarDatos();
        }

        private void gridUsuarios_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            gridUsuarios.ClearSelection();
        }

        private void txtBuscar_KeyUp(object sender, KeyEventArgs e)
        {
            dv.RowFilter = "nombres LIKE '%" + txtBuscar.Text + "%' OR apellidos LIKE '%" + txtBuscar.Text + "%'";
        }
    }
}
