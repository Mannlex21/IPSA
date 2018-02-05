using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AplicacionWebMVC.Models;
using System.Web.Script.Serialization;
using System.Data.SqlClient;
using System.Linq.Expressions;

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
        public JsonResult GetDepartamentos(string sidx, string sord, int page, int rows, string username) //Gets the todo Lists.  
        {
            int pageIndex = Convert.ToInt32(page) - 1;
            int pageSize = rows;

            var usuario = DB.Usuarios.Where(s => s.nombreUsuario.ToUpper().Equals(username.ToUpper())).FirstOrDefault();
            
            var usuarioDeptos = DB.UsuarioDepartamento.Where(s=>s.idUsuario==usuario.idUsuario);
            
            if (usuarioDeptos.Count()==1 && usuarioDeptos.FirstOrDefault().idDepartamento=="T")
            {
                IQueryable<Departamentos> Results = DB.Departamentos;
                //Results= Results
                int totalRecords = Results.Count();
                var totalPages = (int)Math.Ceiling((float)totalRecords / (float)rows);

                if (sord.ToUpper() == "DESC")
                {
                    Results = Results.OrderByDescending(s => s.idDepartamento).Skip(pageIndex * pageSize).Take(pageSize);
                }
                else
                {
                    Results = Results.OrderBy(s => s.idDepartamento).Skip(pageIndex * pageSize).Take(pageSize);
                }
                var jsonData = new
                {
                    total = totalPages,
                    page = page,
                    records = totalRecords,
                    rows = Results.Select(
                            a => new
                            {
                                a.idDepartamento,
                                a.descripcion,
                                a.area
                            })
                };
                return Json(jsonData, JsonRequestBehavior.AllowGet);
            }
            else
            {
                List<Departamentos> d = new List<Departamentos>();
                foreach (var x in usuarioDeptos)
                {
                    var idDep = Int16.Parse(x.idDepartamento);
                    var p = DB.Departamentos.AsNoTracking().Where(s => s.idDepartamento == idDep).FirstOrDefault();
                    d.Add(p);
                }
                int totalRecords = d.Count();
                var totalPages = (int)Math.Ceiling((float)totalRecords / (float)rows);
                var jsonData = new
                {
                    total = totalPages,
                    page = page,
                    records = totalRecords,
                    rows = d.Select( a => new{a.idDepartamento,a.descripcion,a.area})
                };
                return Json(jsonData, JsonRequestBehavior.AllowGet);
            }
            
            
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
                var usuario = DB.Usuarios.Where(s => s.nombreUsuario.ToUpper().Equals(username.ToUpper())).FirstOrDefault();
                var usuarioDeptos = DB.UsuarioDepartamento.Where(s => s.idUsuario == usuario.idUsuario).FirstOrDefault();
                if (usuarioDeptos.idDepartamento.Equals("T"))
                {
                    var r = DB.Departamentos.Where(s=>s.idDepartamento==usuario.departamento).Select(
                    a => new
                    {
                        a.idDepartamento,
                        a.area,
                        a.descripcion
                    });
                    return Json(r, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    var r = DB.Departamentos.Where(s => s.idDepartamento == usuario.departamento).Select(
                    a => new
                    {
                        a.idDepartamento,
                        a.area,
                        a.descripcion
                    });
                    return Json(r, JsonRequestBehavior.AllowGet);

                }
                /*var user = from usuarios in DB.Usuarios
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
                return Json(r, JsonRequestBehavior.AllowGet);*/
            }
            catch (SqlException odbcEx)
            {
                var r = new { code = "error", message=odbcEx.Message };
                return Json(r, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                var r = new { code="error", message = ex.Message };
                return Json(r, JsonRequestBehavior.AllowGet);
            }
            
            
        }
    }
}