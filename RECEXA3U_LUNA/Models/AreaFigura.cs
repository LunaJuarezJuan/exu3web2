using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace RECEXA3U_LUNA.Models
{
    [Table("areafigura")]
    public class AreaFigura
    {
        [Key]
        [Column("area_id")]
        public int AreaId { get; set; }

        [Required]
        [Column("figura_id")]
        public int FiguraId { get; set; }

        [Required]
        [Column("parametro1")]
        public decimal Parametro1 { get; set; }

        [Column("parametro2")]
        public decimal? Parametro2 { get; set; } // Nullable decimal válido

        [Column("parametro3")]
        public decimal? Parametro3 { get; set; } // Nullable decimal válido

        [Required]
        [Column("resultado")]
        public decimal Resultado { get; set; }

        [Required]
        [Column("fechahora")]
        public DateTime FechaHora { get; set; }

        [Required]
        [Column("estado")]
        [StringLength(1)]
        public string Estado { get; set; }

        [Column("observaciones")]
        [MaxLength(200)]
        public string Observaciones { get; set; } // Elimina el ?

        [ForeignKey("FiguraId")]
        public virtual Figura Figura { get; set; } // Elimina el ?
    }
}