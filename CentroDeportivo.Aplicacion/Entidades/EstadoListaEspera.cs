namespace CentroDeportivo.Aplicacion.Entidades;

/// <summary>Estado de una entrada en la lista de espera de un turno (clase).</summary>
public enum EstadoListaEspera
{
    /// <summary>En cola, aún sin oferta de cupo.</summary>
    EnEspera = 0,

    /// <summary>Se le notificó por correo; debe confirmar dentro del plazo.</summary>
    OfertaEnviada = 1,

    /// <summary>No confirmó a tiempo; se pasa al siguiente.</summary>
    OfertaVencida = 2,

    /// <summary>Obtuvo la reserva confirmada en sistema.</summary>
    Cubierta = 3
}
