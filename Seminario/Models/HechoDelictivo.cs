using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Seminario.Models
{
    public class HechoDelictivo
    {
        [Key]
        public int Id { get; set; }

        [ForeignKey("Usuario")]
        [Display(Name = "Oficinista")]
        public int UsuarioId { get; set; }

        public DateTime Fecha { get; set; }

        [ForeignKey("Lugar")]
        [Display(Name = "Lugar")]
        public int LugarId { get; set; }

        [Display(Name = "Dirección")]
        [Required]
        [StringLength(100)]
        public string Direccion { get; set; }

        [Display(Name = "Nombre de la Victima")]
        [Required]
        [StringLength(60)]
        public string NombreVictima { get; set; }

        public Genero Genero { get; set; }

        [Range(6, 99)]
        public int Edad { get; set; }

        [StringLength(30)]
        [Display(Name = "Tipo de Patrimonio")]
        public string Tipo { get; set; }

        [StringLength(20)]
        public string Placas { get; set; }

        [StringLength(30)]
        public string Marca { get; set; }

        [StringLength(20)]
        public string Color { get; set; }

        [StringLength(20)]
        public string Modelo { get; set; }

        [StringLength(20)]
        public string Motor { get; set; }

        [StringLength(20)]
        public string Chasis { get; set; }

        [DataType(DataType.PhoneNumber)]
        [StringLength(15)]
        public string Movil { get; set; }

        [StringLength(15)]
        public string Registro { get; set; }

        [StringLength(10)]
        public string Calibre { get; set; }

        [ForeignKey("Delito")]
        [Display(Name = "Tipo de Delito")]
        public int DelitoId { get; set; }

        [StringLength(50)]
        public string Causa { get; set; }

        [Display(Name = "Numero de Oficio")]
        [StringLength(5)]
        public string Oficio { get; set; }

        [Display(Name = "Nombre del Denunciante")]
        [StringLength(60)]
        public string Denunciante { get; set; }

        [StringLength(100)]
        public string Observaciones { get; set; }

        public DateTime FechaIngreso { get; set; }

        public Usuario Usuario { get; set; }
        public Lugar Lugar { get; set; }
        public Delito Delito { get; set; }
    }
}