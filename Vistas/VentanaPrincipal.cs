using System;
using System.Drawing;
using System.Windows.Forms;
using GestionParqueadero.Servicios;

namespace GestionParqueadero
{
    public partial class VentanaPrincipal : Form
    {
        private readonly ParqueaderoServicio _servicio = new ParqueaderoServicio();
        private Button[,] _botonesPuestos = new Button[9, 5];

        // Componentes de la interfaz
        private TextBox txtPlaca;
        private ComboBox cmbTipoVehiculo;
        private NumericUpDown numFila;
        private NumericUpDown numColumna;
        private CheckBox chkPropiedadEspecial;
        private Button btnRegistrar;
        private Button btnDarSalida;
        private Button btnBuscar;
        private Label lblTotalActivos;

        public VentanaPrincipal()
        {
            ConfigurarVentana();
            InicializarComponentesControl();
            InicializarMatrizVisual();
            ActualizarEstadisticas();
        }

        private void ConfigurarVentana()
        {
            this.Text = "🚗 🏍️ Sistema de Gestión de Parqueadero Inteligente";
            this.Size = new Size(1020, 520);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
        }

        private void InicializarComponentesControl()
        {
            Panel panelControl = new Panel
            {
                Size = new Size(300, 460),
                Location = new Point(10, 10),
                BackColor = Color.FromArgb(240, 240, 240)
            };

            Label lblTitulo = new Label
            {
                Text = "PANEL DE CONTROL",
                Font = new Font("Arial", 12, FontStyle.Bold),
                Location = new Point(15, 15),
                Size = new Size(250, 25)
            };

            Label lblPlaca = new Label
            {
                Text = "Placa del Vehículo:",
                Location = new Point(15, 55),
                Size = new Size(120, 20)
            };

            txtPlaca = new TextBox
            {
                Location = new Point(15, 75),
                Size = new Size(170, 25),
                CharacterCasing = CharacterCasing.Upper
            };

            btnBuscar = new Button
            {
                Text = "🔍 Buscar",
                Location = new Point(190, 74),
                Size = new Size(65, 24),
                BackColor = Color.LightSkyBlue,
                Font = new Font("Arial", 8, FontStyle.Bold)
            };

            btnBuscar.Click += BtnBuscar_Click;

            Label lblTipo = new Label
            {
                Text = "Tipo de Vehículo:",
                Location = new Point(15, 115),
                Size = new Size(120, 20)
            };

            cmbTipoVehiculo = new ComboBox
            {
                Location = new Point(15, 135),
                Size = new Size(240, 25),
                DropDownStyle = ComboBoxStyle.DropDownList
            };

            cmbTipoVehiculo.Items.AddRange(new string[]
            {
                "🚗 Automóvil (Carro)",
                "🏍️ Motocicleta"
            });

            cmbTipoVehiculo.SelectedIndex = 0;

            cmbTipoVehiculo.SelectedIndexChanged += (s, e) =>
            {
                ActualizarEtiquetaCheckbox();
                numFila.Value = cmbTipoVehiculo.SelectedIndex == 0 ? 0 : 5;
            };

            chkPropiedadEspecial = new CheckBox
            {
                Text = "¿Es una Camioneta?",
                Location = new Point(15, 175),
                Size = new Size(240, 25)
            };

            Label lblFila = new Label
            {
                Text = "Asignar Fila:",
                Location = new Point(15, 215),
                Size = new Size(100, 20)
            };

            numFila = new NumericUpDown
            {
                Location = new Point(15, 235),
                Size = new Size(100, 25),
                Minimum = 0,
                Maximum = 8
            };

            Label lblCol = new Label
            {
                Text = "Asignar Columna:",
                Location = new Point(145, 215),
                Size = new Size(110, 20)
            };

            numColumna = new NumericUpDown
            {
                Location = new Point(145, 235),
                Size = new Size(110, 25),
                Minimum = 0,
                Maximum = 4
            };

            btnRegistrar = new Button
            {
                Text = "📥 Registrar Ingreso",
                Location = new Point(15, 285),
                Size = new Size(240, 35),
                BackColor = Color.LightGreen,
                Font = new Font("Arial", 9, FontStyle.Bold)
            };

            btnRegistrar.Click += BtnRegistrar_Click;

            btnDarSalida = new Button
            {
                Text = "🚪 Registrar Salida / Cobrar",
                Location = new Point(15, 335),
                Size = new Size(240, 35),
                BackColor = Color.LightCoral,
                Font = new Font("Arial", 9, FontStyle.Bold)
            };

            btnDarSalida.Click += BtnDarSalida_Click;

            lblTotalActivos = new Label
            {
                Text = "Vehículos Estacionados: 0 / 45",
                Font = new Font("Arial", 10, FontStyle.Bold),
                ForeColor = Color.DarkSlateGray,
                Location = new Point(15, 410),
                Size = new Size(250, 30)
            };

            panelControl.Controls.AddRange(new Control[]
            {
                lblTitulo,
                lblPlaca,
                txtPlaca,
                btnBuscar,
                lblTipo,
                cmbTipoVehiculo,
                chkPropiedadEspecial,
                lblFila,
                numFila,
                lblCol,
                numColumna,
                btnRegistrar,
                btnDarSalida,
                lblTotalActivos
            });

            this.Controls.Add(panelControl);
        }

        private void InicializarMatrizVisual()
        {
            GroupBox grupoMapa = new GroupBox
            {
                Text = "🗺️ Mapa Físico de Puestos",
                Size = new Size(670, 460),
                Location = new Point(325, 10),
                Font = new Font("Arial", 10, FontStyle.Bold)
            };

            int anchoBoton = 100;
            int altoBoton = 38;
            int margenFila = 40;
            int margenColumna = 110;

            for (int f = 0; f < 9; f++)
            {
                string textoIcono = f < 5 ? $"🚗 F{f}" : $"🏍️ F{f}";

                Label lblIconoFila = new Label
                {
                    Text = textoIcono,
                    Location = new Point(20, margenFila + (f * (altoBoton + 5)) + 10),
                    Size = new Size(80, 20),
                    Font = new Font("Arial", 9, FontStyle.Bold)
                };

                grupoMapa.Controls.Add(lblIconoFila);

                for (int c = 0; c < 5; c++)
                {
                    Button btn = new Button
                    {
                        Size = new Size(anchoBoton, altoBoton),
                        Location = new Point(
                            margenColumna + (c * (anchoBoton + 8)),
                            margenFila + (f * (altoBoton + 5))
                        ),
                        Text = $"[{f},{c}] Libre",
                        BackColor = Color.White,
                        Font = new Font("Segoe UI", 8, FontStyle.Regular),
                        Tag = new Point(f, c),
                    };

                    btn.Click += (s, e) =>
                    {
                        Point coord = (Point)((Button)s).Tag;
                        numFila.Value = coord.X;
                        numColumna.Value = coord.Y;
                    };

                    _botonesPuestos[f, c] = btn;
                    grupoMapa.Controls.Add(btn);
                }
            }

            this.Controls.Add(grupoMapa);
        }
    }
}