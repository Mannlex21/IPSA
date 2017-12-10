using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AplicacionWebMVC.Models;
using System.Web.Script.Serialization;
using System.Data.SqlClient;

namespace AplicacionWebMVC.Controllers
{
    public class DepartamentosController : Controller
    {
        // GET: Departamentos
        public ActionResult Index()
        {
            return View();
        }
        AlmacenEntities DB = new AlmacenEntities();
        public JsonResult GetDepartamentos(string sidx, string sord, int page, int rows) //Gets the todo Lists.  
        {
            int pageIndex = Convert.ToInt32(page) - 1;
            int pageSize = rows;
            var Results = DB.Departamentos.Select(
                a => new
                {
                    a.idDepartamento,
                    a.descripcion,
                    a.area
                });
            int totalRecords = Results.Count();
            var totalPages = (int)Math.Ceiling((float)totalRecords / (float)rows);
           
            if (sord.ToUpper() == "DESC")
            {
                Results = Results.OrderByDescending(s => s.idDepartamento);
                Results = Results.Skip(pageIndex * pageSize).Take(pageSize);
            }
            else
            {
                Results = Results.OrderBy(s => s.idDepartamento);
                Results = Results.Skip(pageIndex * pageSize).Take(pageSize);
            }
            var jsonData = new
            {
                total = totalPages,
                page=page,
                records = totalRecords,
                rows = Results
            };
            return Json(jsonData, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetDepartamentos2() //Gets the todo Lists.  
        {
            var Results = DB.Departamentos.Select(
                a => new
                {
                    a.idDepartamento,
                    a.descripcion,
                    a.area
                });
            return Json(Results, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetDepartamento(string username) //Gets the todo Lists.  
        {
            try
            {
                var user = from usuarios in DB.Usuarios
                           join departamentos in DB.Departamentos 
                           on usuarios.departamento equals departamentos.idDepartamento
                           select new { usuarios,departamentos};
                user = user.Where(u => u.usuarios.nombreUsuario== username);
                var r = user.Select(
                    a => new
                    {
                        a.departamentos.idDepartamento,
                        a.departamentos.area,
                        a.departamentos.descripcion
                    });
                return Json(r, JsonRequestBehavior.AllowGet);
            }
            catch (SqlException odbcEx)
            {
                var r = new { code = "error" };
                return Json(r, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                var r = new { code="error"};
                return Json(r, JsonRequestBehavior.AllowGet);
            }
            
            
        }
    }
}