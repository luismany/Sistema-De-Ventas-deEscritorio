﻿using CapaEntidad;
using CapaNegocio;
using CapaPresentacion.Utilidades;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CapaPresentacion
{
    public partial class frmProveedores : Form
    {
        public frmProveedores()
        {
            InitializeComponent();
        }

        private void frmProveedores_Load(object sender, EventArgs e)
        {
            cboEstado.Items.Add(new OpcionCombobox() { Valor = 1, Texto = "Activo" });
            cboEstado.Items.Add(new OpcionCombobox() { Valor = 0, Texto = "Inactivo" });
            cboEstado.DisplayMember = "Texto";
            cboEstado.ValueMember = "Valor";
            cboEstado.SelectedIndex = 0;

            //Cargar combobox Buscarpor
            foreach (DataGridViewColumn columna in dgvData.Columns)
            {
                if (columna.Visible && columna.Name != "btnSeleccionar") cboBuscar.Items.Add(new OpcionCombobox() { Valor = columna.Name, Texto = columna.HeaderText });

            }
            cboBuscar.DisplayMember = "Texto";
            cboBuscar.ValueMember = "Valor";
            cboBuscar.SelectedIndex = 0;

            //Cargar Datagridview con la data
            List<Proveedor> listaProveedor = new CN_Proveedor().ListaProveedor();

            foreach (Proveedor item in listaProveedor)
            {

                dgvData.Rows.Add(new object[] {"",item.IdProveedor,item.Documento,item.RazonSocial,item.Correo,item.Telefono,
                item.Estado==true ? 1:0,
                item.Estado==true ? "Activo":"Inactivo"
                });
            }
        }

        private void btnGuardar_Click(object sender, EventArgs e)
        {
            string mensaje = string.Empty;

            Proveedor oProveedor = new Proveedor()
            {
                IdProveedor = Convert.ToInt32(txtId.Text),
                Documento = txtDocumento.Text,
                RazonSocial = txtRazonSocial.Text,
                Correo = txtCorreo.Text,
                Telefono = txtTelefono.Text,
                Estado = Convert.ToInt32(((OpcionCombobox)cboEstado.SelectedItem).Valor) == 1 ? true : false
            };

            if (oProveedor.IdProveedor == 0)
            {
                int idGenerado = new CN_Proveedor().AgregarProveedor(oProveedor, out mensaje);

                if (idGenerado != 0)
                {
                    dgvData.Rows.Add(new object[] { "",idGenerado,txtDocumento.Text,txtRazonSocial.Text,txtCorreo.Text,txtTelefono.Text,
            ((OpcionCombobox)cboEstado.SelectedItem).Valor.ToString(),
            ((OpcionCombobox)cboEstado.SelectedItem).Texto.ToString()
            });

                    Limpiar();
                }
                else MessageBox.Show(mensaje);

            }
            else
            {
                bool resultado = new CN_Proveedor().ModificarProveedor(oProveedor, out mensaje);

                if (resultado)
                {
                    DataGridViewRow row = dgvData.Rows[Convert.ToInt32(txtIndice.Text)];
                    row.Cells["IdProveedor"].Value = txtId.Text;
                    row.Cells["Documento"].Value = txtDocumento.Text;
                    row.Cells["RazonSocial"].Value = txtRazonSocial.Text;
                    row.Cells["Correo"].Value = txtCorreo.Text;
                    row.Cells["Telefono"].Value = txtTelefono.Text;
                    row.Cells["EstadoValor"].Value = ((OpcionCombobox)cboEstado.SelectedItem).Valor.ToString();
                    row.Cells["Estado"].Value = ((OpcionCombobox)cboEstado.SelectedItem).Texto.ToString();

                    Limpiar();

                }
                else MessageBox.Show(mensaje);

            }
        }

        private void Limpiar()
        {
            txtIndice.Text = "";
            txtId.Text = "0";
            txtDocumento.Text = "";
            txtRazonSocial.Text = "";
            txtCorreo.Text = "";
            txtTelefono.Text = "";
            cboEstado.SelectedIndex = 0;
        }

        private void dgvData_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (dgvData.Columns[e.ColumnIndex].Name == "btnSeleccionar")
            {
                int indice = e.RowIndex;

                if (indice >= 0)
                {
                    txtIndice.Text = indice.ToString();
                    txtId.Text = dgvData.CurrentRow.Cells["IdProveedor"].Value.ToString();
                    txtDocumento.Text = dgvData.CurrentRow.Cells["Documento"].Value.ToString();
                    txtRazonSocial.Text = dgvData.CurrentRow.Cells["RazonSocial"].Value.ToString();
                    txtCorreo.Text = dgvData.CurrentRow.Cells["Correo"].Value.ToString();
                    txtTelefono.Text = dgvData.CurrentRow.Cells["Telefono"].Value.ToString();


                    foreach (OpcionCombobox oc in cboEstado.Items)
                    {
                        if (Convert.ToInt32(oc.Valor) == Convert.ToInt32(dgvData.Rows[indice].Cells["EstadoValor"].Value))
                        {
                            int indiceCombo = cboEstado.Items.IndexOf(oc);
                            cboEstado.SelectedIndex = indiceCombo;
                            break;
                        }

                    }

                }

            }
        }

        private void btnLimpiar_Click(object sender, EventArgs e)
        {
            Limpiar();
        }

        private void btnEliminar_Click(object sender, EventArgs e)
        {
            if (Convert.ToInt32(txtId.Text) != 0)
            {

                if (MessageBox.Show("¿Seguro que desea eliminar este Proveedor?", "Mensaje", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    string mensaje = string.Empty;
                    int IdProveedor = Convert.ToInt32(txtId.Text);

                    bool resultado = new CN_Proveedor().EliminarProveedor(IdProveedor, out mensaje);

                    if (resultado)
                    {
                        dgvData.Rows.RemoveAt(Convert.ToInt32(txtIndice.Text));
                        Limpiar();
                    }
                    else MessageBox.Show(mensaje, "Mensaje", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                }

            }
        }

        private void btnBuscar_Click(object sender, EventArgs e)
        {
            string columnaFiltro = ((OpcionCombobox)cboBuscar.SelectedItem).Valor.ToString();

            if (dgvData.Rows.Count > 0)
            {
                foreach (DataGridViewRow row in dgvData.Rows)
                {
                    //si el valor de la columnaFiltro contiene el valor  de txtbusqueda
                    if (row.Cells[columnaFiltro].Value.ToString().Trim().ToUpper().Contains(txtbusqueda.Text.Trim().ToUpper()))
                        row.Visible = true;
                    else
                        row.Visible = false;
                }
            }
        }

        private void btnLimpiarBuscador_Click(object sender, EventArgs e)
        {
            txtbusqueda.Text = "";
            foreach (DataGridViewRow row in dgvData.Rows)
            {
                row.Visible = true;
            }
        }
    }
    
}
