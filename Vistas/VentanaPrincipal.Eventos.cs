using System;
using System.Drawing;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using GestionParqueadero.Modelo;

namespace GestionParqueadero
{
    public partial class VentanaPrincipal
    {
        private void ActualizarEtiquetaCheckbox()
        {
            if (cmbTipoVehiculo.SelectedIndex == 0)
                chkPropiedadEspecial.Text = "¿Es una Camioneta?";
            else
                chkPropiedadEspecial.Text = "¿Deja Casco en Custodia?";
        }

        private void ActualizarEstadisticas()
        {
            int activos = _servicio.ObtenerVehiculosActivos().Count;
            lblTotalActivos.Text = $"📊 Vehículos Estacionados: {activos} / 45";
        }

        private void BtnRegistrar_Click(object sender, EventArgs e)
        {
            try
            {
                string placa = txtPlaca.Text.Trim().ToUpper();

                if (string.IsNullOrEmpty(placa))
                    throw new ArgumentException("La placa no puede estar vacía.");

                if (cmbTipoVehiculo.SelectedIndex == 0)
                {
                    if (!Regex.IsMatch(placa, @"^[A-Z]{3}[0-9]{3}$"))
                        throw new ArgumentException("Formato inválido para carro.");
                }
                else
                {
                    if (!Regex.IsMatch(placa, @"^[A-Z]{3}[0-9]{2}[A-Z]{1}$"))
                        throw new ArgumentException("Formato inválido para moto.");
                }

                int fila = (int)numFila.Value;
                int columna = (int)numColumna.Value;

                Vehiculo nuevoVehiculo =
                    cmbTipoVehiculo.SelectedIndex == 0
                    ? new Carro(placa, chkPropiedadEspecial.Checked)
                    : new Motocicleta(placa, chkPropiedadEspecial.Checked);

                string resultado = _servicio.RegistrarIngreso(
                    nuevoVehiculo,
                    fila,
                    columna
                );

                if (resultado.StartsWith("❌"))
                {
                    MessageBox.Show(
                        resultado,
                        "Error",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Warning
                    );
                }
                else
                {
                    MessageBox.Show(
                        resultado,
                        "Éxito",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Information
                    );

                    _botonesPuestos[fila, columna].Text = placa;

                    _botonesPuestos[fila, columna].BackColor =
                        cmbTipoVehiculo.SelectedIndex == 0
                        ? Color.LightCoral
                        : Color.Khaki;

                    ActualizarEstadisticas();
                    LimpiarFormulario();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    ex.Message,
                    "Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );
            }
        }

        private void BtnDarSalida_Click(object sender, EventArgs e)
        {
            string placa = txtPlaca.Text.Trim().ToUpper();

            if (string.IsNullOrEmpty(placa))
            {
                MessageBox.Show(
                    "Ingrese una placa.",
                    "Atención",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning
                );

                return;
            }

            SincronizarControlesPorPlaca(placa);

            string resultadoFactura = _servicio.RegistrarSalida(placa);

            if (resultadoFactura.StartsWith("❌"))
            {
                MessageBox.Show(
                    resultadoFactura,
                    "Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning
                );
            }
            else
            {
                MessageBox.Show(
                    resultadoFactura,
                    "Factura",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information
                );

                LiberarPuestoVisual(placa);

                ActualizarEstadisticas();
                LimpiarFormulario();
            }
        }

        private void LiberarPuestoVisual(string placa)
        {
            for (int f = 0; f < 9; f++)
            {
                for (int c = 0; c < 5; c++)
                {
                    if (_botonesPuestos[f, c].Text.Equals(
                        placa,
                        StringComparison.OrdinalIgnoreCase))
                    {
                        _botonesPuestos[f, c].Text = $"[{f},{c}] Libre";
                        _botonesPuestos[f, c].BackColor = Color.White;
                        return;
                    }
                }
            }
        }

        private void BtnBuscar_Click(object sender, EventArgs e)
        {
            string placa = txtPlaca.Text.Trim().ToUpper();

            Vehiculo encontrado = _servicio.BuscarPorPlaca(placa);

            if (encontrado == null)
            {
                MessageBox.Show(
                    "Vehículo no encontrado.",
                    "Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );

                return;
            }

            SincronizarControlesPorPlaca(placa);

            for (int f = 0; f < 9; f++)
            {
                for (int c = 0; c < 5; c++)
                {
                    if (_botonesPuestos[f, c].Text.Equals(
                        placa,
                        StringComparison.OrdinalIgnoreCase))
                    {
                        MessageBox.Show(
                            $"Vehículo encontrado en Fila {f}, Columna {c}",
                            "Buscar",
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Information
                        );

                        numFila.Value = f;
                        numColumna.Value = c;
                        return;
                    }
                }
            }
        }

        private void SincronizarControlesPorPlaca(string placa)
        {
            for (int f = 0; f < 9; f++)
            {
                for (int c = 0; c < 5; c++)
                {
                    if (_botonesPuestos[f, c].Text.Equals(
                        placa,
                        StringComparison.OrdinalIgnoreCase))
                    {
                        cmbTipoVehiculo.SelectedIndex = f < 5 ? 0 : 1;
                        return;
                    }
                }
            }
        }

        private void LimpiarFormulario()
        {
            txtPlaca.Clear();
            chkPropiedadEspecial.Checked = false;
            txtPlaca.Focus();
        }
    }
}