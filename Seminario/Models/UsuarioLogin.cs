using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Seminario.Models
{
    public class UsuarioLogin
    {
        [Required(ErrorMessage = "Ingrese el Usuario.")]
        [StringLength(20)]
        [Display(Name = "Usuario")]
        public string User { get; set; }

        [Required(ErrorMessage = "Ingrese la Contraseña.")]
        [StringLength(20)]
        [DataType(DataType.Password)]
        [Display(Name = "Contraseña")]
        public string Password { get; set; }
    }
}