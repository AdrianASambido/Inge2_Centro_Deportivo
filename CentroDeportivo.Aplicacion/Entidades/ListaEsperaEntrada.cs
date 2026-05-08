namespace CentroDeportivo.Aplicacion.Entidades;

/// <summary>Entrada de lista de espera ligada a un turno concreto (día + hora + actividad).</summary>
public class ListaEsperaEntrada
{
    public int Id { get; private set; }
    public int Id_Turno { get; set; }
    public int Id_Usuario { get; set; }
    public DateTime FechaAltaUtc { get; set; }
    public EstadoListaEspera Estado { get; set; }
    /// <summary>Fecha límite (UTC) para aceptar la oferta de cupo liberado.</summary>
    public DateTime? OfertaExpiraUtc { get; set; }

    public Turno? Turno { get; set; }
    public Usuario? Usuario { get; set; }

    public ListaEsperaEntrada() { }
}
