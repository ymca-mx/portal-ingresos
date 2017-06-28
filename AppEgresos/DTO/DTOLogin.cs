using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AppEgresos
{
    public class DTOLogin
    {
        public int usuarioId { get; set; }
        public string nombre { get; set; }
        public string email { get; set; }
        public int sucursalCajaId { get; set; }
        public Acceso TipoAcceso { get; set; }
    }

    public class DTOCredenciales
    {
        public int username { get; set; }
        public string password { get; set; }
    }

    public enum Acceso
    {
        Ingresos = 1,
        General = 2
    }

    public class DTOUsuarioImagen
    {
        public byte[] imagen { get; set; }
        public string imagenBase64 { get; set; }
        public string extensionImagen { get; set; }
        public string nombre { get; set; }
    }

    public class DTOMenu
    {
        public int MenuId { get; set; }
        public string Descripcion { get; set; }
        public virtual List<DTOSubMenu> SubMenu { get; set; }
    }

    public class DTOSubMenu
    {
        public int SubmenuId { get; set; }
        public int MenuId { get; set; }
        public string Descripcion { get; set; }
        public string Direccion { get; set; }
    }
}