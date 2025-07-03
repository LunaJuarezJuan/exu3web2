using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;

namespace RECEXA3U_LUNA.Models
{
    public class bdfigurasEntities : DbContext
    {
        // Esto se asegura de que use la cadena de conexión "ConexionBD" del Web.config
        public bdfigurasEntities() : base("name=ConexionBD")
        {
        }

        public DbSet<Figura> Figura { get; set; }
        public DbSet<AreaFigura> AreaFigura { get; set; }
    }
}