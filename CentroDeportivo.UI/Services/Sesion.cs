using CentroDeportivo.Aplicacion.Entidades;

namespace CentroDeportivo.UI.Services;

public class Sesion
{
    public Usuario? UsuarioActual { get; private set; }

    public void IniciarSesion(Usuario usuario)
    {
        UsuarioActual = usuario;
    }

    public void CerrarSesion()
    {
        UsuarioActual = null;
    }

    public bool EstaAutenticado() => UsuarioActual != null;
}
