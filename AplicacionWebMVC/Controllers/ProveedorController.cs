using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AplicacionWebMVC.Models;
using System.Web.Script.Serialization;
using System.Web.Security;
using System.Data;
using System.Data.Entity.Infrastructure;
using PagedList;
using System.Data.SqlClient;
using System.Data.Entity.Core;
namespace AplicacionWebMVC.Controllers
{
    public class ProveedorController : Controller
    {
        AlmacenEntities DB = new AlmacenEntities();
        // GET: Proveedor
        [Authorize(Roles = "Admin, Proveedor")]
        public ActionResult Cotizacion()
        {
            return View();
        }
        [Authorize(Roles = "Admin, Proveedor")]
        public ActionResult Perfil()
        {
            return View();
        }
        [Authorize(Roles = "Admin, Proveedor")]
        public ActionResult Invitacion()
        {
            return View();
        }
        [Authorize(Roles = "Admin")]
        public ActionResult ListaProveedor()
        {
            return View(DB.Usuarios.ToList());
        }
        [Authorize(Roles = "Admin")]
        public ActionResult RegistroProveedor()
        {
            return View();
        }
        [HttpPost]
        public ActionResult RegistroProveedor(Usuarios usuario, string user, FormCollection frm)
        {
            String f = frm["tipo"];
            usuario.registradoPor = user;
            int idUsuario = 0;
            System.Console.WriteLine(usuario);
            if (ModelState.IsValid)
            {
                try
                {
                    using (AlmacenEntities db = new AlmacenEntities())
                    {
                        var count = DB.Usuarios.Count(u => u.nombreUsuario == usuario.nombreUsuario);
                        if (count == 0)
                        {
                            db.Usuarios.Add(usuario);
                            db.SaveChanges();
                            idUsuario = usuario.idUsuario;
                            String s;
                            
                            ModelState.Clear();
                            ViewBag.Message = "";
                            return RedirectToAction("ListaUsuarios");
                        }
                        else
                        {
                            ModelState.AddModelError("", "Ya existe este usuario");
                        }
                    }
                }
                catch (EntityException ex)
                {
                    var context = new AlmacenEntities();
                    var connection = context.Database.Connection;
                    String s;
                    using (SqlConnection con = new SqlConnection(connection.ConnectionString))
                    {
                        string query = "DELETE FROM Usuarios WHERE idUsuario" + idUsuario
                            + "DELETE FROM DetallesUsuarios WHERE idUsuario" + idUsuario; ;
                        using (SqlCommand cmd = new SqlCommand(query))
                        {
                            cmd.Connection = con;
                            con.Open();
                            s = cmd.ExecuteScalar().ToString();
                            con.Close();
                        }
                    }
                }

            }
            return View();
        }
    }
}