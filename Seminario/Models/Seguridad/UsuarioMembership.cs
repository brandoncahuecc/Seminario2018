using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using System.Web.Security;

namespace Seminario.Models.Seguridad
{
    public class UsuarioMembership : MembershipUser
    {
        public UsuarioMembership(Usuario usuario)
        {
            Id = usuario.Id;
            Nombre = usuario.Nombre;
            Apellido = usuario.Apellido;
            Telefono = usuario.Telefono;
            Correo = usuario.Correo;
            User = usuario.User;
            Password = usuario.Password;
            Municipio = usuario.Municipio;
            TipoUsuario = usuario.TipoUsuario;
            Estado = usuario.Estado;
        }
        
        [Key]
        public int Id { get; set; }

        [StringLength(40)]
        [Display(Name = "Nombres")]
        public string Nombre { get; set; }

        [StringLength(40)]
        [Display(Name = "Apellidos")]
        public string Apellido { get; set; }
        
        [DataType(DataType.PhoneNumber)]
        [Display(Name = "Numero de Teléfono")]
        public int Telefono { get; set; }

        [StringLength(30)]
        [DataType(DataType.EmailAddress)]
        [Display(Name = "Correo Electrónico")]
        public string Correo { get; set; }

        [StringLength(20)]
        [Display(Name = "Usuario")]
        public string User { get; set; }

        [StringLength(20)]
        [DataType(DataType.Password)]
        [Display(Name = "Contraseña")]
        public string Password { get; set; }

        [Display(Name = "Municipio")]
        [ForeignKey("Municipio")]
        public int MunicipioId { get; set; }

        [Display(Name = "Tipo de Usuario")]
        public TipoUsuario TipoUsuario { get; set; }
        
        public EstadoUsuario Estado { get; set; }

        public Municipio Municipio { get; set; }
    }
}