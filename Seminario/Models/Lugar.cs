using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Seminario.Models
{
    public class Lugar
    {
        [Key]
        public int Id { get; set; }

        [Display(Name = "Municipio")]
        [ForeignKey("Municipio")]
        public int MunicipioId { get; set; }

        [Required]
        [Index(IsUnique = true)]
        [StringLength(100)]
        public string Nombre { get; set; }

        public Municipio Municipio { get; set; }

        public List<HechoDelictivo> HechoDelictivo { get; set; }
    }
}