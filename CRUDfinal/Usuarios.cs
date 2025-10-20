using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRUDfinal
{
    internal class Usuarios
    {
        public int Id_usuario { get; set; }
        public string Nombre { get; set; }=string.Empty;
        public string Apellido { get; set; }= string.Empty;
        public int DNI { get; set; }
        public int Telefono { get; set; }
        public string Direccion {  get; set; }
        //public bool baja { get; set; }
    }
}
