using RECEXA3U_LUNA.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;

namespace RECEXA3U_LUNA.Controllers
{
    public class AreaFiguraController : Controller
    {
        private bdfigurasEntities db = new bdfigurasEntities();
        

        // GET: AreaFigura
        public ActionResult Index()
        {
            var areas = db.AreaFigura.Include(a => a.Figura).Where(a => a.Estado == "A");
            return View(areas.ToList());
        }

        // GET: AreaFigura/Create
        public ActionResult Create()
        {
            ViewBag.FiguraId = ObtenerSelectListFiguras();
            return View(new AreaFigura());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "FiguraId,Parametro1,Parametro2,Parametro3,Observaciones")] AreaFigura areaFigura)
        {
            ViewBag.FiguraId = ObtenerSelectListFiguras(areaFigura.FiguraId);

            try
            {
                var figura = db.Figura.Find(areaFigura.FiguraId);
                if (figura == null)
                {
                    ModelState.AddModelError("FiguraId", "La figura seleccionada no existe");
                    return View(areaFigura);
                }

                int parametrosRequeridos = Convert.ToInt32(figura.ParametrosRequeridos);

                if (!ValidarParametros(parametrosRequeridos, areaFigura.Parametro1, areaFigura.Parametro2, areaFigura.Parametro3))
                {
                    ModelState.AddModelError("", "Parámetros inválidos para la figura seleccionada");
                    return View(areaFigura);
                }

                decimal resultado = CalcularArea(parametrosRequeridos, areaFigura.Parametro1, areaFigura.Parametro2, areaFigura.Parametro3);
                if (resultado <= 0)
                {
                    ModelState.AddModelError("", "Error al calcular el área. Verifica los parámetros.");
                    return View(areaFigura);
                }

                string connectionString = ConfigurationManager.ConnectionStrings["ConexionBD"].ConnectionString;

                using (var con = new System.Data.SqlClient.SqlConnection(connectionString))
                {
                    string query = @"
INSERT INTO AreaFigura 
(figura_id, Parametro1, Parametro2, Parametro3, Observaciones, Resultado, FechaHora, Estado)
VALUES 
(@FiguraId, @Parametro1, @Parametro2, @Parametro3, @Observaciones, @Resultado, @FechaHora, @Estado)";

                    using (var cmd = new System.Data.SqlClient.SqlCommand(query, con))
                    {
                        cmd.Parameters.AddWithValue("@FiguraId", areaFigura.FiguraId);
                        cmd.Parameters.AddWithValue("@Parametro1", areaFigura.Parametro1);
                        cmd.Parameters.AddWithValue("@Parametro2", (object)areaFigura.Parametro2 ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("@Parametro3", (object)areaFigura.Parametro3 ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("@Observaciones", (object)areaFigura.Observaciones ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("@Resultado", resultado);
                        cmd.Parameters.AddWithValue("@FechaHora", DateTime.Now);
                        cmd.Parameters.AddWithValue("@Estado", "A");

                        con.Open();
                        cmd.ExecuteNonQuery();
                    }
                }

                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "Error al insertar: " + ex.Message);
                return View(areaFigura);
            }
        }


        // 🔄 Método auxiliar para centralizar la carga del ViewBag
        private SelectList ObtenerSelectListFiguras(object selected = null)
        {
            return new SelectList(db.Figura.Where(f => f.Estado == "A"), "FiguraId", "Nombre", selected);
        }




        // GET: AreaFigura/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            AreaFigura areaFigura = db.AreaFigura.Find(id);
            if (areaFigura == null)
            {
                return HttpNotFound();
            }

            ViewBag.FiguraId = ObtenerSelectListFiguras(areaFigura.FiguraId);
            return View(areaFigura);
        }


        // POST: AreaFigura/Edit
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "AreaId,FiguraId,Parametro1,Parametro2,Parametro3,Observaciones")] AreaFigura areaFigura)
        {
            ViewBag.FiguraId = ObtenerSelectListFiguras(areaFigura.FiguraId);

            try
            {
                var figura = db.Figura.Find(areaFigura.FiguraId);
                if (figura == null)
                {
                    ModelState.AddModelError("FiguraId", "La figura seleccionada no existe");
                    return View(areaFigura);
                }

                int parametrosRequeridos = Convert.ToInt32(figura.ParametrosRequeridos);

                if (!ValidarParametros(parametrosRequeridos, areaFigura.Parametro1, areaFigura.Parametro2, areaFigura.Parametro3))
                {
                    ModelState.AddModelError("", "Parámetros inválidos para la figura seleccionada");
                    return View(areaFigura);
                }

                decimal resultado = CalcularArea(parametrosRequeridos, areaFigura.Parametro1, areaFigura.Parametro2, areaFigura.Parametro3);
                if (resultado <= 0)
                {
                    ModelState.AddModelError("", "Error al calcular el área. Verifica los parámetros.");
                    return View(areaFigura);
                }

                string connectionString = ConfigurationManager.ConnectionStrings["ConexionBD"].ConnectionString;

                using (var con = new System.Data.SqlClient.SqlConnection(connectionString))
                {
                    string query = @"
UPDATE AreaFigura SET 
    figura_id = @FiguraId,
    Parametro1 = @Parametro1,
    Parametro2 = @Parametro2,
    Parametro3 = @Parametro3,
    Observaciones = @Observaciones,
    Resultado = @Resultado,
    FechaHora = @FechaHora,
    Estado = @Estado
WHERE area_id = @AreaId";

                    using (var cmd = new System.Data.SqlClient.SqlCommand(query, con))
                    {
                        cmd.Parameters.AddWithValue("@AreaId", areaFigura.AreaId);
                        cmd.Parameters.AddWithValue("@FiguraId", areaFigura.FiguraId);
                        cmd.Parameters.AddWithValue("@Parametro1", areaFigura.Parametro1);
                        cmd.Parameters.AddWithValue("@Parametro2", (object)areaFigura.Parametro2 ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("@Parametro3", (object)areaFigura.Parametro3 ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("@Observaciones", (object)areaFigura.Observaciones ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("@Resultado", resultado);
                        cmd.Parameters.AddWithValue("@FechaHora", DateTime.Now);
                        cmd.Parameters.AddWithValue("@Estado", "A");

                        con.Open();
                        cmd.ExecuteNonQuery();
                    }
                }

                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "Error al actualizar: " + ex.Message);
                return View(areaFigura);
            }
        }




        // GET: AreaFigura/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            var areaFigura = db.AreaFigura.Include(a => a.Figura).FirstOrDefault(a => a.AreaId == id && a.Estado == "A");
            if (areaFigura == null)
                return HttpNotFound();

            return View(areaFigura);
        }

        // POST: AreaFigura/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            var areaFigura = db.AreaFigura.Find(id);
            if (areaFigura != null)
            {
                areaFigura.Estado = "I"; // Inactivo
                db.Entry(areaFigura).State = EntityState.Modified;
                db.SaveChanges();
            }
            return RedirectToAction("Index");
        }

        // Validación de parámetros
        private bool ValidarParametros(int parametrosRequeridos, decimal param1, decimal? param2, decimal? param3)
        {
            // Validar que param1 siempre sea positivo
            if (param1 <= 0)
                return false;

            switch (parametrosRequeridos)
            {
                case 1:
                    return true; // Solo necesita param1
                case 2:
                    return param2.HasValue && param2.Value > 0;
                case 3:
                    return param2.HasValue && param2.Value > 0 && param3.HasValue && param3.Value > 0;
                default:
                    return false;
            }
        }

        // Cálculo del área
        private decimal CalcularArea(int parametrosRequeridos, decimal param1, decimal? param2, decimal? param3)
        {
            try
            {
                switch (parametrosRequeridos)
                {
                    case 1:
                        // Cuadrado: lado^2
                        return param1 * param1;

                    case 2:
                        // Rectángulo: base * altura
                        if (param2.HasValue)
                            return param1 * param2.Value;
                        return 0;

                    case 3:
                        // Triángulo usando fórmula de Herón
                        if (param2.HasValue && param3.HasValue)
                        {
                            decimal a = param1;
                            decimal b = param2.Value;
                            decimal c = param3.Value;

                            // Verificar que los lados formen un triángulo válido
                            if (a + b <= c || a + c <= b || b + c <= a)
                                return 0;

                            decimal s = (a + b + c) / 2;
                            decimal areaSquared = s * (s - a) * (s - b) * (s - c);

                            if (areaSquared <= 0)
                                return 0;

                            // Convertir a double para la raíz cuadrada
                            double area = Math.Sqrt((double)areaSquared);
                            return (decimal)area;
                        }
                        return 0;

                    default:
                        return 0;
                }
            }
            catch (Exception)
            {
                return 0;
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}