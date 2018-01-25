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
        ComprasEntities DBC = new ComprasEntities();
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
            int idProveedor = Int32.Parse(frm["proveedor"]);
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
                            Usuario up = new Usuario();
                            up.idUsuarios = idUsuario;
                            up.Proveedor = idProveedor;
                            
                            using (ComprasEntities dbc = new ComprasEntities())
                            {
                                dbc.Usuario.Add(up);
                                dbc.SaveChanges();
                            }

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
        public ActionResult DeleteUsuarioP(int id, string username)
        {
            try
            {
                var context = new ComprasEntities();
                var connection = context.Database.Connection;
                if (DB.Usuarios.Find(id).nombreUsuario.ToString() == username)
                {
                    Usuario usuarioPConex = DBC.Usuario.Find(id);
                    if (usuarioPConex != null)
                    {
                        DBC.Usuario.Remove(usuarioPConex);
                        DBC.SaveChanges();
                    }
                    Usuarios usuarioP = DB.Usuarios.Find(id);
                    if (usuarioP != null)
                    {
                        DB.Usuarios.Remove(usuarioP);
                        DB.SaveChanges();
                    }
                    return RedirectToAction("SignOut");
                }
                else
                {
                    Usuario usuarioPConex = DBC.Usuario.Find(id);
                    if (usuarioPConex!=null)
                    {
                        DBC.Usuario.Remove(usuarioPConex);
                        DBC.SaveChanges();
                    }
                    Usuarios usuarioP = DB.Usuarios.Find(id);
                    if (usuarioP != null)
                    {
                        DB.Usuarios.Remove(usuarioP);
                        DB.SaveChanges();
                    }
                    return RedirectToAction("ListaUsuarios");
                }
            }
            catch (DataException/* dex */)
            {
                //Log the error (uncomment dex variable name and add a line here to write a log.
                return RedirectToAction("ListaUsuarios", new { id = id, saveChangesError = true });
            }

        }
        public JsonResult GetProveedores(string sidx, string sord, int page, int rows) //Gets the todo Lists.  
        {
            int pageIndex = Convert.ToInt32(page) - 1;
            int pageSize = rows;
            var Results = DBC.Proveedores.Select(
                a => new
                {
                    a.proveedor,
                    a.razSoc,
                    a.razSoc2
                });
            int totalRecords = Results.Count();
            var totalPages = (int)Math.Ceiling((float)totalRecords / (float)rows);

            if (sord.ToUpper() == "DESC")
            {
                Results = Results.OrderByDescending(s => s.proveedor);
                Results = Results.Skip(pageIndex * pageSize).Take(pageSize);
            }
            else
            {
                Results = Results.OrderBy(s => s.proveedor);
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