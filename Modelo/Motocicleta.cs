using System;

namespace GestionParqueadero.Modelo // Dice a donde pertenece el archivo
{
    public class Motocicleta : Vehiculo // Hacemos herencia motocicleta es hija de vehiculo
    {
        // Propiedad (Atributo) solo de la motocicleta
        public bool DejaCasco { get; }

        // CONSTRUCTOR : Recibe placa y el estado del casco con (:base(placa)) le pasamos la placa al constructor Vehiculo 
        public Motocicleta(string placa, bool dejaCasco) : base(placa)
        {
            // Asignamos el parámetro en minúscula a la propiedad en mayúscula
            this.DejaCasco = dejaCasco; 
        }

        // POLIMORFISMO: Aquí reescribimos el método CalcularCobro
        // La palabra "override" es obligatoria para cambiar el comportamiento del papá
        public override decimal CalcularCobro()
        {
            // Si el vehículo aún no ha salido, el cobro es 0
            if (HoraSalida == null) return 0;

            // Lógica de control y cálculos
            // Calculamos la diferencia de tiempo entre entrada y salida
            TimeSpan tiempoEstadia = HoraSalida.Value - HoraIngreso;
            // Redondeamos las horas hacia arriba (ej: si estuvo 1.1 horas, cobramos 2)
            int horasCobrar = (int)Math.Ceiling(tiempoEstadia.TotalHours);

            decimal tarifaHora = 2000;
            decimal total = horasCobrar * tarifaHora;

            // // Aplicamos un condicional: Si dejó casco, sumamos $1.000 extra al total
            if (DejaCasco)
            {
                total += 1000;
            }

            // Guardamos el resultado en la variable protegida que heredamos del papá
            ValorPagado = total;
            return total;
        }
        
        // Este método nos servirá para "Setear" la hora de salida desde afuera
        public void RegistrarSalida()
        {
            HoraSalida = DateTime.Now;
        }
    }
}