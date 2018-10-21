using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Seminario.Models
{
    public class Patrimonio
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

        [StringLength(30)]
        [Display(Name = "Tipo de Patrimonio")]
        public string Tipo { get; set; }

        [StringLength(20)]
        public string Placas { get; set; }

        [StringLength(30)]
        public string Marca { get; set; }

        [StringLength(20)]
        public string Color { get; set; }

        [StringLength(10)]
        public string Modelo { get; set; }

        [StringLength(20)]
        public string Motor { get; set; }

        [StringLength(20)]
        public string Chasis { get; set; }

        [Display(Name = "Numero de Telefono")]
        [DataType(DataType.PhoneNumber)]
        public int Movil { get; set; }

        [StringLength(15)]
        public string Registro { get; set; }

        [StringLength(10)]
        public string Calibre { get; set; }

        [Display(Name = "Tipo de Delito")]
        public int DelitoId { get; set; }

        [Required]
        [Display(Name = "Numero de Oficio")]
        public int Oficio { get; set; }

        [Display(Name = "Nombre del Denunciante")]
        [StringLength(60)]
        public string Denunciante { get; set; }

        [StringLength(100)]
        public string Observaciones { get; set; }

        public Usuario Usuario { get; set; }
        public Lugar Lugar { get; set; }
        public Delito Delito { get; set; }
    }
}