using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Servidor.Models
{
    public class Usuario
    {
        public Usuario()
        {
            FechaCreacion = DateTime.Now;
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }
        public string Email { get; set; }
        public string NombreUsuario { get; set; }
        public string Contrasena { get; set; }
        public bool Estatus { get; set; }
        public string Sexo { get; set; }
        public DateTime FechaCreacion { get; set; }
        public string VCode { get; set; }
    }
}