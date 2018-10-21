using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Seminario.Models
{
    public class Excel
    {
        [Display(Name = "Municipio")]
        [Required]
        public int MunicipioId { get; set; }

        [Display(Name = "Año")]
        [Required]
        public Anio Anio { get; set; }

        [Required]
        public Mes Mes { get; set; }
    }
}