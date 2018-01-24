using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AplicacionWebMVC.Controllers
{
    public class ProveedorController : Controller
    {
        // GET: Proveedor
        [Authorize(Roles = "Proveedor")]
        public ActionResult Proveedor()
        {
            return View();
        }
    }
}