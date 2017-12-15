using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AplicacionWebMVC.Models;
using System.Web.Script.Serialization;

namespace AplicacionWebMVC.Controllers
{
    public class MaterialesController : Controller
    {
        // GET: Materiales
        public ActionResult Index()
        {
            return View();
        }
        AlmacenEntities DB = new AlmacenEntities();
        public JsonResult GetMateriales(string sidx, string sord, int page, int rows, string descripcion, string marca, string idMaterial, string consecutivo) //Gets the todo Lists.  
        {
            int pageIndex = Convert.ToInt32(page) - 1;
            int pageSize = rows;
            var Results = DB.Materiales.Select(
                a => new
                {
                    a.idMaterial,
                    a.descripcion,
                    a.marca,
                    a.uMedida,
                    a.costoProm,
                    a.existencia
                });
            if (!string.IsNullOrEmpty(marca))
            {
                Results = Results.Where(s => s.marca.Contains(marca));
            }
            if (!string.IsNullOrEmpty(descripcion))
            {
                Results = Results.Where(s => s.descripcion.Contains(descripcion));
            }
            if (!string.IsNullOrEmpty(idMaterial))
            {
                Results = Results.Where(s => s.idMaterial.ToString().Contains(idMaterial));
            }
            int totalRecords = Results.Count();
            var totalPages = (int)Math.Ceiling((float)totalRecords / (float)rows);
            if (sord.ToUpper() == "DESC")
            {
                Results = Results.OrderByDescending(s => s.idMaterial);
                Results = Results.Skip(pageIndex * pageSize).Take(pageSize);
            }
            else
            {
                Results = Results.OrderBy(s => s.idMaterial);
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
    }
}