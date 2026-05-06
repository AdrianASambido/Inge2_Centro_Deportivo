using CentroDeportivo.Aplicacion.Entidades;
using CentroDeportivo.UI.Servicios;
namespace CentroDeportivo.UI.Servicios;

public class Sesion
{
    public Usuario? UsuarioActual { get; private set; }

    public void IniciarSesion(Usuario u)
        => UsuarioActual = u;

    public void CerrarSesion()
        => UsuarioActual = null;

    public bool EstaAutenticado()
        => UsuarioActual != null;

}