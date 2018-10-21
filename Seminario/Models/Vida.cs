using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Seminario.Models
{
    public class Vida
    {
        public int Id { get; set; }

        [Display(Name = "Oficinista")]
        public int UsuarioId { get; set; }

        [DataType(DataType.Date)]
        public DateTime Fecha { get; set; }

        [DataType(DataType.Time)]
        public DateTime Hora { get; set; }

        [Display(Name = "Lugar")]
        public int LugarId { get; set; }

        [Display(Name = "Dirección")]
        [Required]
        [StringLength(100)]
        public string Direccion { get; set; }

        [Display(Name = "Nombre Completo de la Victima")]
        [Required]
        [StringLength(60)]
        public string NombreVictima { get; set; }

        public Genero Genero { get; set; }

        [Range(6, 99)]
        public int Edad { get; set; }

        [Required]
        [Display(Name = "Tipo de Delito")]
        public int DelitoId { get; set; }
        
        [StringLength(50)]
        public string Causa { get; set; }

        [StringLength(100)]
        public string Observaciones { get; set; }

        public Usuario Usuario { get; set; }
        public Lugar Lugar { get; set; }
        public Delito Delito { get; set; }
    }
}