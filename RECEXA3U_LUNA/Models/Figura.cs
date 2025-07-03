using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace RECEXA3U_LUNA.Models
{
    [Table("figura")]
    public class Figura
    {
        [Key]
        [Column("figura_id")]
        public int FiguraId { get; set; }

        [Required]
        [Column("nombre")]
        [StringLength(30)]
        public string Nombre { get; set; }

        [Column("formula")]
        [StringLength(100)]
        public string Formula { get; set; }

        [Required]
        [Column("parametros_requeridos")]
        public byte ParametrosRequeridos { get; set; } // TINYINT se mapea a byte

        [Required]
        [Column("estado")]
        [StringLength(1)]
        public string Estado { get; set; }

        // Navegación
        public virtual ICollection<AreaFigura> AreaFiguras { get; set; }
    }
}