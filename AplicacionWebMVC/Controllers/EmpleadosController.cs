using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AplicacionWebMVC.Models;
using System.Web.Script.Serialization;

namespace AplicacionWebMVC.Controllers
{
    public class EmpleadosController : Controller
    {
        // GET: Empleados
        public ActionResult Index()
        {
            return View();
        }
        EmpleadosEntities DB = new EmpleadosEntities();
        public JsonResult GetEmpleados(string sidx, string sord, int page, int rows, string idEmp, string nombreEmp, string apellidoPEmp, string apellidoMEmp, string rfcEmp) //Gets the todo Lists.  
        {
            int pageIndex = Convert.ToInt32(page) - 1;
            int pageSize = rows;
            var Results = DB.empleados.Select(
                a => new
                {
                    a.empleado,
                    a.nombre,
                    a.apellidoMaterno,
                    a.apellidoPaterno,
                    a.rfc
                });
            if (!string.IsNullOrEmpty(idEmp))
            {
                Results = Results.Where(s => s.empleado.ToString().Contains(idEmp));
            }
            if (!string.IsNullOrEmpty(nombreEmp))
            {
                Results = Results.Where(s => s.nombre.Contains(nombreEmp));
            }
            if (!string.IsNullOrEmpty(apellidoPEmp))
            {
                Results = Results.Where(s => s.apellidoPaterno.ToString().Contains(apellidoPEmp));
            }
            if (!string.IsNullOrEmpty(apellidoMEmp))
            {
                Results = Results.Where(s => s.apellidoMaterno.ToString().Contains(apellidoMEmp));
            }
            if (!string.IsNullOrEmpty(rfcEmp))
            {
                Results = Results.Where(s => s.rfc.ToString().Contains(rfcEmp));
            }
            int totalRecords = Results.Count();
            var totalPages = (int)Math.Ceiling((float)totalRecords / (float)rows);
            if (sord.ToUpper() == "DESC")
            {
                Results = Results.OrderByDescending(s => s.empleado);
                Results = Results.Skip(pageIndex * pageSize).Take(pageSize);
            }
            else
            {
                Results = Results.OrderBy(s => s.empleado);
                Results = Results.Skip(pageIndex * pageSize).Take(pageSize);
            }
            var jsonData = new
            {
                total = totalPages,
                page = page,
                records = totalRecords,
                rows = Results
            };
            return Json(jsonData, JsonRequestBehavior.AllowGet);
        }
    }
}