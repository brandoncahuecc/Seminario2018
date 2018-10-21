using System;
using System.ComponentModel.DataAnnotations;

namespace Seminario.Models
{
    public class Filtro
    {
        [Display(Name = "Municipio")]
        [Required]
        public int MunicipioId { get; set; }

        [Display(Name = "Tipo de Delito")]
        public TipoDelito TipoDelito { get; set; }

        [Display(Name = "Año")]
        [Required]
        public Anio Anio { get; set; }

        [Required]
        public Mes Mes { get; set; }

        public Semana Semana { get; set; }

        [Range(0, 31)]
        [Display(Name = "Día")]
        public int Dia { get; set; }

        [Range(0, 24)]
        public int Hora { get; set; }

    }

    public enum Anio
    {
        [Display(Name = "2018")]
        a2018 = 2018,
        [Display(Name = "2019")]
        a2019 = 2019,
        [Display(Name = "2020")]
        a2020 = 2020
    }

    public enum Mes
    {
        Enero,
        Febrero,
        Marzo,
        Abril,
        Mayo,
        Junio,
        Julio,
        Agosto,
        Septiembre,
        Octubre,
        Noviembre,
        Diciembre
    }

    public enum Semana
    {
        [Display(Name = "Selección Opcional")]
        SeleccionOpcional,
        [Display(Name = "Semana Uno")]
        SemanaUno,
        [Display(Name = "Semana Dos")]
        SemanaDos,
        [Display(Name = "Semana Tres")]
        SemanaTres,
        [Display(Name = "Semana Cuatro")]
        SemanaCuatro,
        [Display(Name = "Semana Cinco")]
        SemanaCinco
    }

}