using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using DESIGNER.Tool;
using CryptSharp;
using BOL;
using ENTITIES;

namespace DESIGNER.Modulos
{
    public partial class frmEmpresas : Form
    {
        Empresa empresa = new Empresa();

        EEmpresa entEmpresa = new EEmpresa();

        DataView dv;

        int Correcto = 0;


        public frmEmpresas()
        {
            InitializeComponent();
        }

        private void btnEnviar_Click(object sender, EventArgs e)
        {
            errorRegistroEmp.Clear();
            if(txtRazon.Text.Trim() == string.Empty)
            {
                errorRegistroEmp.SetError(txtRazon, "Ingrese la Razon");
                Correcto = 1;
                txtRazon.Focus();
            }
            if(txtRuc.Text.Trim() == string.Empty)
            {
                errorRegistroEmp.SetError(txtRuc, "Ingrese El R.U.C");
                Correcto = 1;
                txtRazon.Focus();
            }
            if (Correcto == 0)
            {
                errorRegistroEmp.Clear();
                if (Aviso.Preguntar("¿Desea Guardar la empresa?") == DialogResult.Yes)
                {
                    entEmpresa.razonsocial = txtRazon.Text;
                    entEmpresa.ruc = txtRuc.Text;
                    entEmpresa.direccion = txtDireccion.Text;
                    entEmpresa.telefono = txtTelefono.Text;
                    entEmpresa.email = txtEmail.Text;
                    entEmpresa.website = txtWeb.Text;

                    //int num = empresa.registrarEmpresa(entEmpresa);
                    //System.Windows.Forms.MessageBox.Show(num.ToString());

                    if (empresa.registrarEmpresa(entEmpresa) > 0)
                    {
                        Aviso.Informar("Nueva empresa registrada");
                        actualizarDatos();
                        reiniciarInterfaz();
                    }
                    else
                    {
                        Aviso.Advertir("No hemos podido terminar el registro");
                    }
                }
            }
        }

        private void actualizarDatos()
        {
            dv = new DataView(empresa.Listar());
            gridEmpresasview.DataSource = dv;
            gridEmpresasview.Refresh();

            gridEmpresasview.Columns[0].Visible = false;

            gridEmpresasview.Columns[1].Width = 270;
            gridEmpresasview.Columns[2].Width = 100;
            gridEmpresasview.Columns[3].Width = 200;
            gridEmpresasview.Columns[4].Width = 100;
            gridEmpresasview.Columns[5].Width = 180;
            gridEmpresasview.Columns[6].Width = 180;
            gridEmpresasview.AlternatingRowsDefaultCellStyle.BackColor = Color.AliceBlue;
        }

        private void reiniciarInterfaz()
        {
            txtRazon.Clear();
            txtRuc.Clear();
            txtDireccion.Clear();
            txtTelefono.Clear();
            txtEmail.Clear();
            txtWeb.Clear();
            txtRazon.Focus();
        }

        private void txtCerrar_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void frmEmpresas_Load(object sender, EventArgs e)
        {
            actualizarDatos();
        }

        private void txtBuscaremp_TextChanged(object sender, EventArgs e)
        {
            dv.RowFilter = "razonsocial LIKE '%" + txtBuscaremp.Text + "%'";
        }
    }
}
