using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using BOL;
using ENTITIES;
using DESIGNER.Tool;

namespace DESIGNER.Modulos
{
    public partial class frmProductos : Form
    {
        Producto producto = new Producto();
        Categoria categoria = new Categoria();
        Subcategoria subcategoria = new Subcategoria();
        Marca marca = new Marca();
        EProducto entProducto = new EProducto();
        int Correcto = 0;

        bool categoriasListas = false;
        public frmProductos()
        {
            InitializeComponent();
        }

        private void actualizarMarcas()
        {
            cboMarca.DataSource = marca.listar();
            cboMarca.DisplayMember = "marca";
            cboMarca.ValueMember = "idmarca";
            cboMarca.Refresh();
            cboMarca.Text = "";
        }

        private void actualizarCategorias()
        {
            cboCategoria.DataSource = categoria.listar();
            cboCategoria.DisplayMember = "categoria";
            cboCategoria.ValueMember = "idcategoria";
            cboCategoria.Refresh();
            cboCategoria.Text = "";
        }


        private void actualizarDatos()
        {
            GridProductos.DataSource = producto.listar();
            GridProductos.Refresh();
        }

        private void frmProductos_Load(object sender, EventArgs e)
        {
            actualizarDatos();
            actualizarMarcas();
            actualizarCategorias();
            categoriasListas = true;
        }

        private void cboCategoria_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (categoriasListas)
            {
                int idcategoria = Convert.ToInt32(cboCategoria.SelectedValue.ToString());
                cboSubcategoria.DataSource = subcategoria.listar(idcategoria);
                cboSubcategoria.DisplayMember = "subcategoria";
                cboSubcategoria.ValueMember = "idsubcategoria";
                cboSubcategoria.Refresh();
                cboSubcategoria.Text = "";
            }
            
        }
        private void reiniciarInterfaz()
        {
            cboSubcategoria.Text = "";
            cboCategoria.Text = "";
            cboMarca.Text = "";
            txtDescripcion.Text = "";
            txtGarantia.Text = "";
            txtPrecio.Text = "";
            txtStock.Text = "";
        }
        private void bntGuardar_Click(object sender, EventArgs e)
        {
            errorProductos.Clear();
            if (txtDescripcion.Text.Trim() == string.Empty)
            {
                errorProductos.SetError(txtDescripcion, "Ingrese la Descripción");
                Correcto = 1;
                txtDescripcion.Focus();
            }

            if (cboSubcategoria.Text.Trim() == string.Empty)
            {
                errorProductos.SetError(cboSubcategoria, "Ingrese la Sub Categoria");
                Correcto = 1;
                cboSubcategoria.Focus();
            }
            if (cboCategoria.Text.Trim() == string.Empty)
            {
                errorProductos.SetError(cboCategoria, "Ingrese la Categoria");
                Correcto = 1;
                cboCategoria.Focus();
            }
            if (txtGarantia.Text.Trim() == string.Empty)
            {
                errorProductos.SetError(txtGarantia, "Ingrese la Garantia");
                Correcto = 1;
                txtGarantia.Focus();
            }
            if (txtPrecio.Text.Trim() == string.Empty)
            {
                errorProductos.SetError(txtPrecio, "Ingrese el Precio");
                Correcto = 1;
                txtPrecio.Focus();
            }
            if (txtStock.Text.Trim() == string.Empty)
            {
                errorProductos.SetError(txtStock, "Ingrese el Stock");
                Correcto = 1;
                txtStock.Focus();
            }
            if (cboMarca.Text.Trim() == string.Empty)
            {
                errorProductos.SetError(cboMarca, "Ingrese la Marca");
                Correcto = 1;
                cboMarca.Focus();
            }
            if (Correcto ==0)
            {
                errorProductos.Clear();
                if (Aviso.Preguntar("¿Desea Guardar la empresa?") == DialogResult.Yes)
                {
                    entProducto.idmarca = Convert.ToInt32(cboMarca.SelectedValue.ToString());
                    entProducto.idsubcategoria = Convert.ToInt32(cboSubcategoria.SelectedValue.ToString());
                    entProducto.descripcion = txtDescripcion.Text;
                    entProducto.garantia = Convert.ToInt32(txtGarantia.Text);
                    entProducto.precio = Convert.ToDouble(txtPrecio.Text);
                    entProducto.stock = Convert.ToInt32(txtStock.Text);
                    if (producto.registrarProducto(entProducto) > 0)
                    {
                        Aviso.Informar("Nuevo Producto registrado");
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

        private void bntImprimir_Click(object sender, EventArgs e)
        {
            Reportes.CrystalReport1 reporte = new Reportes.CrystalReport1();
            reporte.SetDataSource(producto.listar());
            reporte.Refresh();
            Reportes.frmVisorReportes formulario = new Reportes.frmVisorReportes();

            formulario.Visor.ReportSource = reporte;
            formulario.Visor.Refresh();
            formulario.ShowDialog();
        }

        private void bntExportarXLS_Click(object sender, EventArgs e)
        {
            Exportardatos("XLSX");
        }
        private void bntExportarPDF_Click_1(object sender, EventArgs e)
        {
            Exportardatos("PDF");
        }
        private void Exportardatos(string extension)
        {
            SaveFileDialog sd = new SaveFileDialog();
            sd.Title = "Reporte de productos";
            sd.Filter = $"Archivos en formato {extension.ToUpper()}|*.{extension.ToLower()}";
            if (sd.ShowDialog() == DialogResult.OK)
            {
                Reportes.CrystalReport1 report = new Reportes.CrystalReport1();
                report.SetDataSource(producto.listar());
                report.Refresh();
                if (extension.ToUpper() == "PDF")
                {
                    report.ExportToDisk(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat, sd.FileName);
                }
                else if(extension.ToUpper() == "XLSX"){
                    report.ExportToDisk(CrystalDecisions.Shared.ExportFormatType.ExcelWorkbook, sd.FileName);
                }
                

                Aviso.Informar("Se ha creado el reporte correctamente");
            }
        }

        
    }
}
