using System;
using System.Collections.Generic;
using System.Text;

namespace CineStreamCR.BLL.DTO
{
    public class Respuesta <T>
    {
        public bool EsCorrecto { get; set; }
        public string mensaje { get; set; } = string.Empty;
        public T Dato { get; set; }
        public int codigo { get; set; }

        public Respuesta()
        {
            EsCorrecto = true;
            mensaje = "Operación realizada correctamente.";
            codigo = 200;
        }
    }
}
