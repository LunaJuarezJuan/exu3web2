using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RECEXA3U_LUNA.Models
{
	public class AreaConsultaDTO
	{
        public int AreaId { get; set; }
        public string Figura { get; set; }
        public decimal Parametro1 { get; set; }
        public decimal? Parametro2 { get; set; }
        public decimal? Parametro3 { get; set; }
        public decimal Resultado { get; set; }
        public DateTime FechaHora { get; set; }
        public string Observaciones { get; set; }
    }
}