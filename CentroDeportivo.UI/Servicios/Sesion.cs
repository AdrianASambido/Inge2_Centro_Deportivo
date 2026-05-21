using CentroDeportivo.Aplicacion.Entidades;
using CentroDeportivo.UI.Servicios;
namespace CentroDeportivo.UI.Servicios;

public class Sesion
{
    public Usuario? UsuarioActual { get; private set; }

    public void IniciarSesion(Usuario u)
        => UsuarioActual = u;

    public bool esCliente()
        => UsuarioActual != null && UsuarioActual.Rol == Rol.Cliente;
    public bool esAdministrador()
        => UsuarioActual != null && UsuarioActual.Rol == Rol.Administrador;
    public bool esEmpleado()
        => UsuarioActual != null && UsuarioActual.Rol == Rol.Empleado;
    public void CerrarSesion()
        => UsuarioActual = null;

    public bool EstaAutenticado()
        => UsuarioActual != null;

    public event Action OnChange;

    public void NotificarCambio() => OnChange?.Invoke();



}