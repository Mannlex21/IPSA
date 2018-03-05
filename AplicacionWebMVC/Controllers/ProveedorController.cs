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
using System.IO;


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
            var u = User.Identity.Name;
            var idU=DB.Usuarios.Where(s=>s.nombreUsuario.ToUpper().Equals(u.ToString().ToUpper())).FirstOrDefault().idUsuario;
            var idP = DBC.Usuario.Where(s=>s.idUsuarios==idU).FirstOrDefault().Proveedor;
            var p = DBC.Proveedores.Where(s=>s.consecutivos==idP).FirstOrDefault();
            return View(p);
        }
        [Authorize(Roles = "Admin, Proveedor")]
        public ActionResult Invitacion(string sortOrder, string currentFilter, string searchString, int? page)
        {
            int pageSize = 10;
            int pageNumber = (page ?? 1);

            return View(DB.Solicitud_Requisiciones.OrderBy(i => i.fechaNecesitar).ToPagedList(page ?? 1, pageSize));
            //return View(DB.Solicitud_Requisiciones.ToList());
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
                            return RedirectToAction("ListaProveedor");
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
                            + "DELETE FROM DetallesUsuarios WHERE idUsuario" + idUsuario;
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
            return RedirectToAction("ListaProveedor");
        }
        public ActionResult DeleteUsuarioP(int id)
        {
            try
            {
                var usuarioP = DBC.Usuario.Where(s => s.idUsuarios == id).FirstOrDefault();
                if (usuarioP != null)
                {
                    DBC.Usuario.Remove(usuarioP);
                    DBC.SaveChanges();
                }
                var usuario = DB.Usuarios.Where(s => s.idUsuario == id).FirstOrDefault();
                if (usuarioP != null)
                {
                    DB.Usuarios.Remove(usuario);
                    DB.SaveChanges();
                }
                return RedirectToAction("ListaProveedor");
            }
            catch (Exception ex)
            {
                return RedirectToAction("ListaProveedor");
            }

        }
        [HttpPost]
        public string ActualizarProveedor(string razSoc2, string RFC, string direccion, string telefono,
            string representante, string colonia, string ciudad, string codigoPostal, string fax, string tipoProveedor) //Gets the todo Lists.  
        {
            try
            {
                var u = User.Identity.Name;
                var usuario = DB.Usuarios.Where(s => s.nombreUsuario.ToUpper().Equals(u.ToUpper())).FirstOrDefault();
                var ud = DBC.Usuario.Where(s => s.idUsuarios == usuario.idUsuario).FirstOrDefault();
                var proveedor = DBC.Proveedores.Where(s => s.consecutivos == ud.Proveedor).FirstOrDefault();
                if (proveedor != null)
                {
                    proveedor.razSoc2 = razSoc2;
                    proveedor.direccion = direccion;
                    proveedor.telefono = telefono;
                    proveedor.representante = representante;
                    proveedor.colonia = colonia;
                    proveedor.ciudad = ciudad;
                    proveedor.codigoPostal = Int32.Parse(codigoPostal);
                    proveedor.fax = fax;
                    proveedor.tipoProveedor = Int16.Parse(tipoProveedor);

                    DBC.Entry(proveedor).State = System.Data.Entity.EntityState.Modified;
                    DBC.SaveChanges();
                    var d = new
                    {
                        code = "ok",
                        message = "Se edito correctamento"
                    };
                    return "ok";
                    //return Json(proveedor, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    var d = new
                    {
                        code = "error",
                        message = "No se encontro proveedor"
                    };
                    return "No se encontro proveedor";
                    //return Json(d, JsonRequestBehavior.AllowGet);
                }
            }
            catch (SqlException odbcEx)
            {
                var r = new { code = "error", message = odbcEx.Message };
                return odbcEx.Message;
                //return Json(r, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                var r = new { code = "error", message = ex.Message };
                return ex.Message;
                //return Json(r, JsonRequestBehavior.AllowGet);
            }
        }
        public JsonResult GetProveedores(string sidx, string sord, int page, int rows, string idProveedor, string razsoc, string rfc) //Gets the todo Lists.  
        {
            int pageIndex = Convert.ToInt32(page) - 1;
            int pageSize = rows;
            var Results = DBC.Proveedores.Select(
                a => new
                {
                    a.consecutivos,
                    a.razSoc,
                    a.razSoc2,
                    a.RFC
                });

            if (!string.IsNullOrEmpty(idProveedor))
            {
                int idp = Int32.Parse(idProveedor);
                Results = Results.Where(s => s.consecutivos == idp);
            }
            if (!string.IsNullOrEmpty(razsoc))
            {
                //int idp = Int32.Parse(idProveedor);
                Results = Results.Where(s => s.razSoc.Contains(razsoc) || s.razSoc2.Contains(razsoc));
            }
            if (!string.IsNullOrEmpty(rfc))
            {
                //int idp = Int32.Parse(idProveedor);
                Results = Results.Where(s => s.RFC.Contains(rfc));
            }
            int totalRecords = Results.Count();
            var totalPages = (int)Math.Ceiling((float)totalRecords / (float)rows);
            if (sord.ToUpper() == "DESC")
            {
                Results = Results.OrderByDescending(s => s.consecutivos);
                Results = Results.Skip(pageIndex * pageSize).Take(pageSize);
            }
            else
            {
                Results = Results.OrderBy(s => s.consecutivos);
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
        public JsonResult GetProveedor() //Gets the todo Lists.  
        {
            try
            {
                var u = User.Identity.Name;
                var usuario = DB.Usuarios.Where(s => s.nombreUsuario.ToUpper().Equals(u.ToUpper())).FirstOrDefault();
                var ud = DBC.Usuario.Where(s => s.idUsuarios == usuario.idUsuario).FirstOrDefault();
                var proveedor = DBC.Proveedores.Where(s => s.consecutivos == ud.Proveedor).FirstOrDefault();
                if (proveedor != null)
                {

                    return Json(proveedor, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    var d = new
                    {
                        code = "error",
                        message = "No se encontro proveedor"
                    };
                    return Json(d, JsonRequestBehavior.AllowGet);
                }
            }
            catch (SqlException odbcEx)
            {
                var r = new { code = "error", message = odbcEx.Message };
                return Json(r, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                var r = new { code = "error", message = ex.Message };
                return Json(r, JsonRequestBehavior.AllowGet);
            }


        }
        [HttpPost]
        public ActionResult UploadFilesInfoProveedor()
        {
            string root = Server.MapPath("/Proveedores/");
            //string root= "\\\\172.16.0.5\\WebSolicitudesAnexos\\";
            try
            {
                var files = Request.Files;
                var req = Request;
                var url = "";
                if (files.Count > 0)
                {
                    if (req.ContentLength < 150000000)
                    {
                        DateTime fecha = DateTime.Today;
                        var u = User.Identity.Name;
                        var usuario = DB.Usuarios.Where(s => s.nombreUsuario.ToUpper().Equals(u.ToUpper())).FirstOrDefault();
                        var usuarioProv = DBC.Usuario.Where(s => s.idUsuarios == usuario.idUsuario).FirstOrDefault();
                        var detUsuario = DBC.DetallesProveedor.Where(s => s.proveedor == usuarioProv.Proveedor).FirstOrDefault();
                        if (detUsuario != null)
                        {
                            var carpetaRoot = "Proveedor-" + usuarioProv.Proveedor + "\\";
                            var carpeta = "InfoPersonal";
                            url = root + carpetaRoot + carpeta;
                            crearCarpetaAdjunto(url);

                            var idDet = DBC.DetallesProveedor.Where(s => s.proveedor == detUsuario.proveedor).FirstOrDefault();
                            var context = new ComprasEntities();
                            var connection = context.Database.Connection;
                            using (SqlConnection con = new SqlConnection(connection.ConnectionString))
                            {
                                string query = "UPDATE DetallesProveedor SET perfil = '" + carpetaRoot + carpeta + "' " +
                                    "WHERE idDetalles=" + idDet.idDetalles;
                                using (SqlCommand cmd = new SqlCommand(query))
                                {
                                    cmd.Connection = con;
                                    con.Open();
                                    cmd.ExecuteScalar();
                                    con.Close();
                                }
                            }

                            for (int i = 0; i < files.Count; i++)
                            {
                                var file = Request.Files[i];
                                string fileName = Path.GetFileName(file.FileName);
                                string[] ext = fileName.Split('.');
                                string[] archivos = Directory.GetFiles(url);
                                int fc = archivos.Length + 1;

                                var nuevoNombre = "Proveedor(" + fecha.Day + "-" + fecha.Month + "-" + fecha.Year + ")-" + fc + "." + ext[1];
                                var path = Path.Combine(url, nuevoNombre);
                                file.SaveAs(path);
                            }
                        }
                        else
                        {
                            var carpetaRoot = "Proveedor-" + usuarioProv.Proveedor + "\\";
                            var carpeta = "InfoPersonal";
                            url = root + carpetaRoot + carpeta;
                            crearCarpetaAdjunto(url);

                            DetallesProveedor dp = new DetallesProveedor();
                            dp.perfil = carpetaRoot + carpeta;
                            dp.facturas = "---";
                            dp.cotizaciones = "---";
                            dp.pedidos = "---";
                            dp.proveedor = usuarioProv.Proveedor;

                            DBC.DetallesProveedor.Add(dp);
                            DBC.SaveChanges();
                            for (int i = 0; i < files.Count; i++)
                            {
                                var file = Request.Files[i];
                                string fileName = Path.GetFileName(file.FileName);
                                string[] ext = fileName.Split('.');
                                string[] archivos = Directory.GetFiles(url);
                                int fc = archivos.Length + 1;

                                var nuevoNombre = "Proveedor(" + fecha.Day + "-" + fecha.Month + "-" + fecha.Year + ")-" + fc + "." + ext[1];
                                var path = Path.Combine(url, nuevoNombre);
                                file.SaveAs(path);
                            }

                        }
                    }
                    else
                    {
                        var d2 = new
                        {
                            code = "error",
                            message = "El archivo excede el limite permitido. Limite: 150 mb"
                        };
                        return Json(d2, JsonRequestBehavior.AllowGet);
                    }
                }

                var d = new
                {
                    code = "ok",
                    message = "Se subio el archivo"
                };
                return Json(d, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                var d = new
                {
                    code = "error",
                    message = ex.Message
                };
                return Json(d, JsonRequestBehavior.AllowGet);
            }

        }
        public string crearCarpetaAdjunto(string dir)
        {
            try
            {
                if (Directory.Exists(dir))
                {
                    Console.WriteLine("That path exists already.");
                    return dir;
                }
                else
                {
                    DirectoryInfo di = Directory.CreateDirectory(dir);
                    return dir;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("The process failed: {0}", e.ToString());
                return "";
            }
        }
        public ActionResult CambiarPassword(string passwordA, string passwordN, string passwordN2)
        {
            try
            {
                var u = User.Identity.Name;
                var usuario = DB.Usuarios.Where(s => s.nombreUsuario.ToUpper().Equals(u.ToUpper())).FirstOrDefault();
                if (passwordA == usuario.contrasena)
                {
                    if (passwordN == passwordN2)
                    {
                        if (passwordN != passwordA)
                        {
                            var context = new AlmacenEntities();
                            var connection = context.Database.Connection;
                            using (SqlConnection con = new SqlConnection(connection.ConnectionString))
                            {
                                string query = "UPDATE Usuarios SET contrasena = '" + passwordN + "' " +
                                    "WHERE idUsuario=" + usuario.idUsuario;
                                using (SqlCommand cmd = new SqlCommand(query))
                                {
                                    cmd.Connection = con;
                                    con.Open();
                                    cmd.ExecuteScalar();
                                    con.Close();
                                }
                            }
                            return Json(new {code="ok",message="Se cambio correctamente" }, JsonRequestBehavior.AllowGet);
                        }
                        else
                        {
                            return Json(new { code = "error", message = "La contraseña nueva debe ser diferente de la anterior" }, JsonRequestBehavior.AllowGet);
                        }
                    }
                    else
                    {
                        return Json(new { code = "error", message = "La contraseña nueva debe repetirse correctamente" }, JsonRequestBehavior.AllowGet);
                    }
                }
                else
                {
                    return Json(new { code = "error", message = "La contraseña anterior es incorrecta" }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                var d = new
                {
                    code = "error",
                    message = ex.Message
                };
                return Json(d, JsonRequestBehavior.AllowGet);
            }

        }
        public class Result{
            public string code { get; set; }
            public string message { get; set; }
        }
    }
}