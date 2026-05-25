using System;

namespace GestionParqueadero.Modelo
{
    // 1. HERENCIA: Conectamos Carro como hijo de Vehiculo
    public class Carro : Vehiculo
    {
        // 2. ENCAPSULAMIENTO: Propiedad exclusiva del carro (Solo lectura)
        public bool EsCamioneta { get; }

        // 3. CONSTRUCTOR: Recibe la placa y si es camioneta, y chuta la placa al papá
        public Carro(string placa, bool esCamioneta) : base(placa)
        {
            this.EsCamioneta = esCamioneta;
        }

        // 4. POLIMORFISMO: Reescribimos el cálculo según las reglas del carro
        public override decimal CalcularCobro()
        {
            // Validación de seguridad por si no ha salido
            if (HoraSalida == null) return 0;

            // Calculamos la estadía usando TimeSpan
            TimeSpan tiempoEstadia = HoraSalida.Value - HoraIngreso;
            int horasCobrar = (int)Math.Ceiling(tiempoEstadia.TotalHours);

            // Tarifa base por hora para un carro común: $5.000
            decimal tarifaHora = 5000;

            // LÓGICA DE CONTROL (Unidad 1): Si es camioneta, la hora vale $7.000
            if (EsCamioneta)
            {
                tarifaHora = 7000;
            }

            // Operación matemática final
            decimal total = horasCobrar * tarifaHora;

            // Guardamos en la variable del papá
            ValorPagado = total;

            return total;
        }

        // Modificador de salida para el carro
        public void RegistrarSalida()
        {
            HoraSalida = DateTime.Now;
        }
    }
}