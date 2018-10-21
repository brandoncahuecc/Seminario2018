using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Seminario.Models
{
    public class Delito
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(50)]
        [Index(IsUnique = true)]
        [Display(Name = "Nombre del Delito")]
        public string Nombre { get; set; }

        [Display(Name = "Tipo de Delito")]
        public TipoDelito TipoDelito { get; set; }

        public List<HechoDelictivo> HechoDelictivo { get; set; }
    }

    public enum TipoDelito
    {
        [Display(Name = "Contra la Vida")]
        ContraLaVida = 1,
        [Display(Name = "Contra el Patrimonio")]
        ContraElPatrimonio = 2
    }
}