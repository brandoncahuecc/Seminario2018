using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Seminario.Models
{
    public class Usuario
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(40)]
        [Display(Name = "Nombres")]
        public string Nombre { get; set; }

        [Required]
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

        [Index(IsUnique = true)]
        [Required]
        [StringLength(20)]
        [Display(Name = "Usuario")]
        public string User { get; set; }

        [Required]
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

        public List<HechoDelictivo> HechoDelictivo { get; set; }
    }

    public enum EstadoUsuario
    {
        Inactivo,
        Activo
    }

    public enum TipoUsuario
    {
        Administrador = 1,
        Oficinista,
        [Display(Name = "Control Mapa")]
        ControlMapa
    }

}