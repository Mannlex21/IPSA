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
    
    public class HomeController : Controller
    {
        
        AlmacenEntities DB = new AlmacenEntities();

        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Login(Usuarios usuario, string returnUrl)
        {
            var Result = DB.Usuarios.Where(u => u.nombreUsuario == usuario.nombreUsuario && u.contrasena == usuario.contrasena).FirstOrDefault();
            if (Result != null)
            {
                FormsAuthentication.SetAuthCookie(Result.nombreUsuario, false);
                if (Url.IsLocalUrl(returnUrl) && returnUrl.Length > 1 && returnUrl.StartsWith("/")
                         && !returnUrl.StartsWith("//") && !returnUrl.StartsWith("/\\"))
                {
                    return Redirect(returnUrl); 
                }
                else
                {
                   return RedirectToAction("Index");  
                }
            }
            else
            {
                ModelState.AddModelError("", "");
                return View();
            }
        }
        [Authorize(Roles = "Admin")]
        public ActionResult Registro()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Registro(Usuarios usuario, string user, FormCollection frm)
        {
            String f = frm["tipo"];
            usuario.registradoPor = user;
            int idUsuario=0;
            System.Console.WriteLine(usuario);
            if (ModelState.IsValid)
            {
                try { 
                    using (AlmacenEntities db = new AlmacenEntities())
                    {
                        var count = DB.Usuarios.Count(u => u.nombreUsuario== usuario.nombreUsuario);
                        if (count == 0)
                        {
                            db.Usuarios.Add(usuario);
                            db.SaveChanges();
                            idUsuario= usuario.idUsuario;
                            String s;
                            var context = new AlmacenEntities();
                            var connection = context.Database.Connection;
                            if (usuario.Role.Equals("Revisor2"))
                            {
                                using (SqlConnection con = new SqlConnection(connection.ConnectionString))
                                {
                                    string query = "INSERT INTO DetallesUsuarios(idUsuario,electricos,seguridad,capitalHumano) " +
                                        "VALUES(@idUsuario,@electricos,@seguridad,@capitalHumano)";
                                    query += " SELECT SCOPE_IDENTITY()";
                                    using (SqlCommand cmd = new SqlCommand(query))
                                    {
                                        cmd.Connection = con;
                                        con.Open();
                                        cmd.Parameters.AddWithValue("@idUsuario", idUsuario);
                                        if (frm["tipo"].Equals("Seguridad"))
                                        {
                                            cmd.Parameters.AddWithValue("@electricos", false);
                                            cmd.Parameters.AddWithValue("@seguridad", true);
                                            cmd.Parameters.AddWithValue("@capitalHumano", false);
                                        }
                                        else if (frm["tipo"].Equals("CapitalHumano"))
                                        {
                                            cmd.Parameters.AddWithValue("@seguridad", false);
                                            cmd.Parameters.AddWithValue("@electricos", false);
                                            cmd.Parameters.AddWithValue("@capitalHumano", true);
                                        }
                                        else if (frm["tipo"].Equals("Electrico"))
                                        {
                                            cmd.Parameters.AddWithValue("@seguridad", false);
                                            cmd.Parameters.AddWithValue("@electricos", true);
                                            cmd.Parameters.AddWithValue("@capitalHumano", false);
                                        }
                                        s = cmd.ExecuteScalar().ToString();
                                        con.Close();
                                    }
                                }
                            }
                            else
                            {
                                using (SqlConnection con = new SqlConnection(connection.ConnectionString))
                                {
                                    string query = "INSERT INTO DetallesUsuarios(idUsuario,electricos,seguridad,capitalHumano) " +
                                        "VALUES(@idUsuario,@electricos,@seguridad,@capitalHumano)";
                                    query += " SELECT SCOPE_IDENTITY()";
                                    using (SqlCommand cmd = new SqlCommand(query))
                                    {
                                        cmd.Connection = con;
                                        con.Open();
                                        cmd.Parameters.AddWithValue("@idUsuario", idUsuario);
                                        cmd.Parameters.AddWithValue("@seguridad", false);
                                        cmd.Parameters.AddWithValue("@electricos", false);
                                        cmd.Parameters.AddWithValue("@capitalHumano", false);
                                        s = cmd.ExecuteScalar().ToString();
                                        con.Close();
                                    }
                                }
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
                        string query = "DELETE FROM Usuarios WHERE idUsuario"+idUsuario
                            + "DELETE FROM DetallesUsuarios WHERE idUsuario"+idUsuario; ;
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
        [Authorize(Roles = "Admin")]
        public ActionResult ListaUsuarios()
        {
            return View(DB.Usuarios.ToList());
        }
        public ActionResult UpdateUsuarios(int id)
        {
            var Result = DB.Usuarios.Find(id);

            return View(Result);
        }
        [HttpPost]
        public ActionResult UpdateUsuarios(Usuarios usuario, string username, string usernameA, int idUser)
        {
            //var id=DB.Usuarios.Find(usuario.idUsuario);

            if (ModelState.IsValid)
            {
                try
                {
                    var us = from u in DB.Usuarios select u ;
                    us = us.Where(u => u.nombreUsuario == User.Identity.Name);
                    DB.Entry(usuario).State = System.Data.Entity.EntityState.Modified;
                    
                    DB.SaveChanges();
                    if (usuario.idUsuario == us.FirstOrDefault().idUsuario)
                    {
                        return RedirectToAction("SignOut");
                    }
                    else
                    {
                        return RedirectToAction("ListaUsuarios");
                    }

                }
                catch (DbUpdateException /* ex */)
                {
                    //Log the error (uncomment ex variable name and write a log.)
                    ModelState.AddModelError("", "Unable to save changes. " +
                        "Try again, and if the problem persists, " +
                        "see your system administrator.");

                }
            }
            return RedirectToAction("ListaUsuarios");
        }
        public ActionResult DeleteUsuarios(int id, string username)
        {
            try
            {
                var context = new AlmacenEntities();
                var connection = context.Database.Connection;
                if (DB.Usuarios.Find(id).nombreUsuario.ToString() == username)
                {
                    using (SqlConnection con = new SqlConnection(connection.ConnectionString))
                    {
                        string query = "DELETE FROM DetallesUsuarios WHERE idUsuario=" + id;
                        string s = "";
                        using (SqlCommand cmd = new SqlCommand(query))
                        {
                            cmd.Connection = con;
                            con.Open();
                            s += cmd.ExecuteScalar();
                            con.Close();
                        }
                    }
                    Usuarios student = DB.Usuarios.Find(id);
                    DB.Usuarios.Remove(student);
                    DB.SaveChanges();
                    return RedirectToAction("SignOut");
                }
                else
                {

                    using (SqlConnection con = new SqlConnection(connection.ConnectionString))
                    {
                        string query = "DELETE FROM DetallesUsuarios WHERE idUsuario="+id;
                        string s="";
                        using (SqlCommand cmd = new SqlCommand(query))
                        {
                            cmd.Connection = con;
                            con.Open();
                            s += cmd.ExecuteScalar();
                            con.Close();
                        }
                    }
                    Usuarios student = DB.Usuarios.Find(id);
                    DB.Usuarios.Remove(student);
                    DB.SaveChanges();
                    return RedirectToAction("ListaUsuarios");
                }
            }
            catch (DataException/* dex */)
            {
                //Log the error (uncomment dex variable name and add a line here to write a log.
                return RedirectToAction("Delete", new { id = id, saveChangesError = true });
            }

        }
        [Authorize]
        public ActionResult SignOut()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Index", "Home");
        }
        [Authorize(Roles = "Admin, Empleado, Revisor1, Revisor2")]
        public ActionResult Solicitud()
        {
            return View();
        }
        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }
        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
        [Authorize(Roles = "Admin, Revisor1, Revisor2")]
        public ActionResult Revision(string sortOrder, string currentFilter, string searchString, int? page)
        {
            var usuario = from u in DB.Usuarios select u;
            usuario = usuario.Where(u => u.nombreUsuario == User.Identity.Name);
            var solicitud = from s in DB.Solicitud_Requisiciones select s;
            if (usuario.FirstOrDefault().Role.Equals("Admin"))
            {
                solicitud = solicitud.Where(s => s.liberaLocal == true);
            }
            else
            {
                solicitud = solicitud.Where(s => s.liberaLocal == false && s.departamento == usuario.FirstOrDefault().departamento);
            }
           //solicitud = solicitud.Where(s => s.libera == false && s.departamento==usuario.FirstOrDefault().departamento);

            int pageSize = 10;
            int pageNumber = (page ?? 1);
            
            return View(solicitud.OrderBy(i => i.preRequisicion).ToPagedList(page ?? 1, pageSize));
        }
        [Authorize(Roles = "Admin, Revisor2")]
        public ActionResult RevisionExterna(string sortOrder, string currentFilter, string searchString, int? page)
        {
            var usuario = from u in DB.Usuarios select u;
            usuario = usuario.Where(u => u.nombreUsuario == User.Identity.Name);
            var solicitud = from s in DB.Solicitud_Requisiciones select s;
            if (usuario.FirstOrDefault().Role.Equals("Admin"))
            {
                solicitud = solicitud.Where(s => s.liberaLocal == false);
            }
            else
            {
                var detUsuario = from u in DB.DetallesUsuarios select u;
                detUsuario = detUsuario.Where(u => u.idUsuario == usuario.FirstOrDefault().idUsuario);
                if (detUsuario.FirstOrDefault().seguridad==true)
                {
                    solicitud = solicitud.Where(s => s.liberaLocal == true && s.liberaSeguridad == false);
                }
                if (detUsuario.FirstOrDefault().electricos == true)
                {
                    solicitud = solicitud.Where(s => s.liberaLocal == true && s.liberaElectrico == false);
                }
                if (detUsuario.FirstOrDefault().capitalHumano == true)
                {
                    solicitud = solicitud.Where(s => s.liberaLocal == true &&  s.liberaCapitalHumano == false);
                }
                
            }

            int pageSize = 10;
            int pageNumber = (page ?? 1);

            return View(solicitud.OrderBy(i => i.preRequisicion).ToPagedList(page ?? 1, pageSize));
        }
        [Authorize(Roles = "Admin, Revisor1, Revisor2")]
        public ActionResult RevisionTotal(string sortOrder, string currentFilter, string searchString, int? page, string fechaInicial, string fechaFinal)
        {
            var usuario = from u in DB.Usuarios select u;
            usuario = usuario.Where(u => u.nombreUsuario == User.Identity.Name);
            var solicitud = from s in DB.Solicitud_Requisiciones select s;
            if (usuario.FirstOrDefault().Role.Equals("Admin"))
            {
                solicitud = solicitud.Where(s => s.liberaLocal == true);
            }
            else
            {
                solicitud = solicitud.Where(s => s.departamento == usuario.FirstOrDefault().departamento && s.liberaLocal == true);
            }
            
            /*if (usuario.FirstOrDefault().Role!="Admin")
            {
                solicitud = solicitud.Where(s => s.departamento == usuario.FirstOrDefault().departamento);
            }*/
            if (!String.IsNullOrEmpty(fechaInicial) && !String.IsNullOrEmpty(fechaFinal))
            {
                DateTime fi = Convert.ToDateTime(Convert.ToDateTime(fechaInicial).ToString("yyyy-MM-dd HH:mm:ss.fff"));
                DateTime ff = Convert.ToDateTime(Convert.ToDateTime(fechaFinal).ToString("yyyy-MM-dd HH:mm:ss.fff"));
                solicitud = solicitud.Where(s => s.fechaRequisicion >=fi && s.fechaRequisicion <= ff);
            }

            int pageSize = 10;
            int pageNumber = (page ?? 1);

            return View(solicitud.OrderBy(i => i.preRequisicion).ToPagedList(page ?? 1, pageSize));
        }
        public ActionResult LiberarSolicitud(int preRequisicion)
        {
            try
            {
                var context = new AlmacenEntities();
                var connection = context.Database.Connection;
                string s = "";
                using (SqlConnection con = new SqlConnection(connection.ConnectionString))
                {
                    string query = "UPDATE Solicitud_Requisiciones SET liberaLocal = 1 WHERE preRequisicion =" + preRequisicion;
                    using (SqlCommand cmd = new SqlCommand(query))
                    {
                        cmd.Connection = con;
                        con.Open();
                        s += cmd.ExecuteScalar();
                        con.Close();
                    }
                }
                return RedirectToAction("Revision");
            }
            catch (SqlException odbcEx)
            {
                return RedirectToAction("Revision");
            }
            catch (Exception ex)
            {
                return RedirectToAction("Revision");
            }
            
        }
        public ActionResult LiberarSolicitudExterna(int preRequisicion)
        {
            try
            {
                
                var context = new AlmacenEntities();
                var connection = context.Database.Connection;
                string s = "";
                using (SqlConnection con = new SqlConnection(connection.ConnectionString))
                {
                    string query="";
                    var usuario = from u in DB.Usuarios select u;
                    usuario = usuario.Where(u => u.nombreUsuario == User.Identity.Name);
                    var detUsuario = from d in DB.DetallesUsuarios select d;
                    detUsuario = detUsuario.Where(u => u.idUsuario == usuario.FirstOrDefault().idUsuario);
                    if (usuario.FirstOrDefault().Role.Equals("Admin"))
                    {
                        query = "UPDATE Solicitud_Requisiciones SET liberaSeguridad = 1,liberaElectrico = 1,liberaCapitalHumano = 1  WHERE preRequisicion =" + preRequisicion;
                    }
                    else {
                        if (detUsuario.FirstOrDefault().seguridad == true)
                        {
                            query = "UPDATE Solicitud_Requisiciones SET liberaSeguridad = 1 WHERE preRequisicion =" + preRequisicion;
                        }
                        if (detUsuario.FirstOrDefault().electricos == true)
                        {
                            query = "UPDATE Solicitud_Requisiciones SET liberaElectrico = 1 WHERE preRequisicion =" + preRequisicion;
                        }
                        if (detUsuario.FirstOrDefault().capitalHumano == true)
                        {
                            query = "UPDATE Solicitud_Requisiciones SET liberaCapitalHumano = 1 WHERE preRequisicion =" + preRequisicion;
                        }

                        
                    }
                    using (SqlCommand cmd = new SqlCommand(query))
                    {
                        cmd.Connection = con;
                        con.Open();
                        s += cmd.ExecuteScalar();
                        con.Close();
                    }
                }
                    
                return RedirectToAction("RevisionExterna");
            }
            catch (SqlException odbcEx)
            {
                return RedirectToAction("RevisionExterna");
            }
            catch (Exception ex)
            {
                return RedirectToAction("RevisionExterna");
            }

        }
        public JsonResult GetSolicitud(string sidx, string sord, int page, int rows,int preRequisicion,int departamento,int ejercicio)
        {
            int pageIndex = Convert.ToInt32(page) - 1;
            int pageSize = rows;
            var Results = from d in DB.DetalleRequisicion select d;
            Results = Results.Where(d => d.departamento == departamento && d.ejercicio == ejercicio && d.preRequisicion == preRequisicion);

            int totalRecords = Results.Count();
            var totalPages = (int)Math.Ceiling((float)totalRecords / (float)rows);

            if (sord.ToUpper() == "DESC")
            {
                Results = Results.OrderByDescending(s => s.detalle);
                Results = Results.Skip(pageIndex * pageSize).Take(pageSize);
            }
            else
            {
                Results = Results.OrderBy(s => s.detalle);
                Results = Results.Skip(pageIndex * pageSize).Take(pageSize);
            }
            var r= Results.Select(
                a => new
                {
                    a.idDReq,
                    a.detalle,
                    a.partida,
                    a.cantidad,
                    a.costoU,
                    a.departamento,
                    descripcion=a.Materiales.descripcion,
                    idMaterial=a.Materiales.idMaterial
                });
            var jsonData = new
            {
                total = totalPages,
                page = page,
                records = totalRecords,
                rows = r
            };
            return Json(jsonData, JsonRequestBehavior.AllowGet);
            
            
        }
    }
}