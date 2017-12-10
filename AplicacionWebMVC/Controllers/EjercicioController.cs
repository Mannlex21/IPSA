using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AplicacionWebMVC.Models;
using System.Web.Script.Serialization;

namespace AplicacionWebMVC.Controllers
{
    public class EjercicioController : Controller
    {
        // GET: Ejercicio
        public ActionResult Index()
        {
            return View();
        }
        AlmacenEntities DB = new AlmacenEntities();
        public JsonResult GetEjercicio() //Gets the todo Lists.  
        {
            var Results = DB.Configuracion.Select(
                a => new
                {
                    a.ejercicio,
                    a.version,
                    a.identificacion,
                    a.fecha
                });
            var jsonData = new
            {
                ejercicio= Results.First()
        };
            return Json(jsonData, JsonRequestBehavior.AllowGet);
        }
    }
}