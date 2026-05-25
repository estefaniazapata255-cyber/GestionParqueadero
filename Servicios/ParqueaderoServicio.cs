using System;
using System.Collections.Generic;
using GestionParqueadero.Modelo;

namespace GestionParqueadero.Servicios
{
    public class ParqueaderoServicio 
    {
        private List<Vehiculo> _vehiculos = new List<Vehiculo>();

        // MATRIZ: 9 filas en total (0-4 Carros, 5-8 Motos) y 5 columnas
        private string[,] _puestos = new string[9, 5];

        public ParqueaderoServicio()
        {
            // Inicializamos la matriz de 9x5 vacía
            for (int f = 0; f < 9; f++)
            {
                for (int c = 0; c < 5; c++)
                {
                    _puestos[f, c] = "";
                }
            }
        }

        // Dibuja el mapa con los nuevos 45 puestos
        public void DibujarMapaPuestos()
        {
            Console.WriteLine("\n=== 🗺️ MAPA DE PUESTOS DEL PARQUEADERO ===");
            Console.WriteLine("         Col 0     Col 1     Col 2     Col 3     Col 4");
            for (int f = 0; f < 9; f++)
            {
                // Filas 0 a 4 son Carros (25 puestos). Filas 5 a 8 son Motos (20 puestos)
                string tipoFila = f < 5 ? $"🚗 F{f}" : $"🏍️ F{f}";
                Console.Write($"{tipoFila}  ");

                for (int c = 0; c < 5; c++)
                {
                    if (_puestos[f, c] == "")
                    {
                        Console.Write("[ Libre ] ");
                    }
                    else
                    {
                        Console.Write($"[{_puestos[f, c]}] ");
                    }
                }
                Console.WriteLine();
            }
            Console.WriteLine("==========================================");
        }

        public string RegistrarIngreso(Vehiculo nuevoVehiculo, int fila, int columna)
        {
            // Validar límites de la nueva matriz (9 filas, 5 columnas)
            if (fila < 0 || fila >= 9 || columna < 0 || columna >= 5)
            {
                return "❌ Error: Las coordenadas del puesto no existen.";
            }

            // Nueva validación de zonas: Carros van de 0 a 4, Motos de 5 a 8
            if (nuevoVehiculo is Carro && fila >= 5)
            {
                return "❌ Error: Las filas de la 5 a la 8 son exclusivas para Motocicletas.";
            }
            if (nuevoVehiculo is Motocicleta && fila < 5)
            {
                return "❌ Error: Las filas de la 0 a la 4 son exclusivas para Carros.";
            }

            if (_puestos[fila, columna] != "")
            {
                return $"❌ Error: El puesto en la Fila {fila}, Columna {columna} ya está ocupado.";
            }

            if (BuscarPorPlaca(nuevoVehiculo.Placa) != null)
            {
                return $"❌ Error: El vehículo con placa {nuevoVehiculo.Placa} ya está adentro.";
            }

            _vehiculos.Add(nuevoVehiculo);
            _puestos[fila, columna] = nuevoVehiculo.Placa.ToUpper();

            return $"✅ Registrado con éxito en el puesto [{fila},{columna}]. Placa: {nuevoVehiculo.Placa}.";
        }

        public Vehiculo BuscarPorPlaca(string placa)
        {
            foreach (var v in _vehiculos)
            {
                if (v.Placa.Equals(placa, StringComparison.OrdinalIgnoreCase))
                {
                    return v;
                }
            }
            return null;
        }

        public string RegistrarSalida(string placa)
        {
            Vehiculo vehiculo = BuscarPorPlaca(placa);
            if (vehiculo == null)
            {
                return $"❌ Error: No se encontró ningún vehículo activo con la placa {placa}.";
            }

            if (vehiculo is Motocicleta moto) moto.RegistrarSalida();
            else if (vehiculo is Carro carro) carro.RegistrarSalida();

            decimal totalACobrar = vehiculo.CalcularCobro();

            // Limpiar el puesto en la matriz de 9x5
            for (int f = 0; f < 9; f++)
            {
                for (int c = 0; c < 5; c++)
                {
                    if (_puestos[f, c].Equals(placa, StringComparison.OrdinalIgnoreCase))
                    {
                        _puestos[f, c] = "";
                    }
                }
            }

            _vehiculos.Remove(vehiculo);

            return $"🚪 Salida Registrada.\nPlaca: {vehiculo.Placa}\nHora Ingreso: {vehiculo.HoraIngreso}\nHora Salida: {vehiculo.HoraSalida}\n💰 TOTAL A PAGAR: ${totalACobrar}";
        }

        public List<Vehiculo> ObtenerVehiculosActivos()
        {
            return _vehiculos;
        }
    }
}