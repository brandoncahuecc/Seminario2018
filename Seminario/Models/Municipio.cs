using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Seminario.Models
{
    public class Municipio
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(25)]
        [Index(IsUnique = true)]
        [Display(Name = "Nombre del Municipio")]
        public string Nombre { get; set; }

        [Display(Name = "Estación")]
        public Comisaria Comisaria { get; set; }

        [Display(Name = "Sub Estación")]
        public SubEstacion SubEstacion { get; set; }

        public List<Lugar> Lugar { get; set; }
    }

    public enum Comisaria
    {
        [Display(Name = "52-1")]
        C52_1,
        [Display(Name = "52-2")]
        C52_2
    }

    public enum SubEstacion
    {
        Seleccione,
        Uno,
        Dos,
        Tres,
        Cuatro
    }
}