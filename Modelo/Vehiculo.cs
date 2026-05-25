using System;

namespace GestionParqueadero.Modelo // Dice a donde pertenece el archivo
{
    public abstract class Vehiculo // Clase padre no deja crear vehiculos genericos 
    {
        // Atributos encapsulados con Propiedades (get y set)
        // 100% Inmutables: Solo se asignan en el constructor y nadie más los puede cambiar
        public string Placa { get; } 
        public DateTime HoraIngreso { get; }

        // Controlados: Cualquiera los puede LEER, pero SOLO esta clase o sus hijos 
        // pueden MODIFICARLOS cuando el vehículo vaya a salir.
        public DateTime? HoraSalida { get; protected set; } // El "?" significa que puede ser nulo (porque cuando entra, aún no ha salido)
        public decimal ValorPagado { get; protected set; }

        // Constructor: Se ejecuta automáticamente cuando un vehículo ingresa
        public Vehiculo(string placa)
        {
            Placa = placa;
            HoraIngreso = DateTime.Now; // Captura la fecha y hora exacta del sistema en ese instante y ya no se cambia
            ValorPagado = 0;
        }

        // Método Polimórfico (abstract): Cada clase hija obligatoriamente 
        // tendrá que implementar su propia forma de calcular el cobro es la pantilla para que los hijos calculen el cobro
        public abstract decimal CalcularCobro();
    }
}