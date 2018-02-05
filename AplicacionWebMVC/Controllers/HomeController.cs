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
            try
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
            catch (EntityException ex)
            {
                return View();
            }
        }
        [Authorize(Roles = "Admin")]
        public ActionResult Registro()
        {
           
            return View();
        }
        /*[HttpPost]
        public ActionResult Registro(Usuarios usuario, string user, FormCollection frm)
        {
            String f = frm["tipo"];
            usuario.registradoPor = user;
            int idUsuario = 0;
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
        }*/
        [HttpPost]
        public JsonResult Registro(usuario usuario ,List<dep> departamentos)
        {
            Usuarios us = new Usuarios();
            us.nombreUsuario = usuario.nombreUsuario;
            us.contrasena = usuario.pass;
            us.idEmpleado = usuario.idEmpleado;
            us.registradoPor = usuario.registradoPor;
            us.departamento = Int16.Parse(usuario.departamento);
            us.Role = usuario.Role;

            int idUsuario = 0;
            try
            {
                using (AlmacenEntities db = new AlmacenEntities())
                {

                    var count = db.Usuarios.Count(u => u.nombreUsuario == usuario.nombreUsuario);
                    if (count == 0)
                    {
                        db.Usuarios.Add(us);
                        db.SaveChanges();
                        idUsuario = us.idUsuario;
                        if (usuario.Role.Equals("Revisor2"))
                        {
                            DetallesUsuarios detUs = new DetallesUsuarios();
                            detUs.idUsuario = idUsuario;
                            detUs.seguridad = (usuario.tipo.Equals("Seguridad"))?true:false;
                            detUs.electricos = (usuario.tipo.Equals("Electrico")) ? true : false;
                            detUs.capitalHumano = (usuario.tipo.Equals("CapitalHumano")) ? true : false;
                            db.DetallesUsuarios.Add(detUs);
                            db.SaveChanges();

                        }
                        else
                        {
                            DetallesUsuarios detUs = new DetallesUsuarios();
                            detUs.idUsuario = idUsuario;
                            detUs.seguridad = false;
                            detUs.electricos =false;
                            detUs.capitalHumano = false;
                            db.DetallesUsuarios.Add(detUs);
                            db.SaveChanges();
                        }

                        if (departamentos!=null)
                        {
                            foreach (var d in departamentos)
                            {
                                UsuarioDepartamento ud = new UsuarioDepartamento();
                                ud.idDepartamento = d.id;
                                ud.idUsuario = idUsuario;
                                db.UsuarioDepartamento.Add(ud);
                                
                            }
                            db.SaveChanges();
                        }
                        var jsonData = new
                        {
                            code = "OK"
                        };
                        return Json(jsonData, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        var jsonData = new
                        {
                            code = "Error",
                            message= "Ya existe este usuario"
                        };
                        return Json(jsonData, JsonRequestBehavior.AllowGet);
                        //ModelState.AddModelError("", "Ya existe este usuario");
                    }
                }
            }
            catch (Exception ex)
            {
                using (AlmacenEntities db = new AlmacenEntities())
                {
                    var detU = db.DetallesUsuarios.Find(idUsuario);
                    if (detU!=null)
                    {
                        db.DetallesUsuarios.Attach(detU);
                        db.DetallesUsuarios.Remove(detU);
                        db.SaveChanges();
                    }
                    var depU = db.UsuarioDepartamento.Find(idUsuario);
                    if (depU!=null)
                    {
                        db.UsuarioDepartamento.Attach(depU);
                        db.UsuarioDepartamento.Remove(depU);
                        db.SaveChanges();
                    }
                    
                    var u = db.Usuarios.Find(idUsuario);
                    if (u!=null)
                    {
                        db.Usuarios.Attach(u);
                        db.Usuarios.Remove(u);
                        db.SaveChanges();
                    }
                }
                var jsonData = new
                {
                    code = "Error",
                    message=ex.Message
                };
                return Json(jsonData, JsonRequestBehavior.AllowGet);
            }
            
        }
        public class usuario
        {
            public string nombreUsuario { get; set; }
            public string pass { get; set; }
            public string idEmpleado { get; set; }
            public string registradoPor { get; set; }
            public string departamento { get; set; }
            public string Role { get; set; }
            public string tipo { get; set; }
        }
        public class dep
        {
            public string id { get; set; }
            public string text { get; set; }
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
                        string query = "DELETE FROM DetallesUsuarios WHERE idUsuario=" + id+ " DELETE FROM UsuarioDepartamento WHERE idUsuario="+id;
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
                        string query = "DELETE FROM DetallesUsuarios WHERE idUsuario="+id + " DELETE FROM UsuarioDepartamento WHERE idUsuario=" + id;
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
            try
            {
                var usuario = DB.Usuarios.Where(u => u.nombreUsuario == User.Identity.Name);
                var solicitud = from s in DB.Solicitud_Requisiciones select s;
                if (usuario.FirstOrDefault().Role.Equals("Admin"))
                {
                    solicitud = solicitud.Where(s => s.liberaLocal=="P");
                }
                else
                {
                    solicitud = solicitud.Where(s => s.liberaLocal == "P" && s.departamento == usuario.FirstOrDefault().departamento);
                }
                //solicitud = solicitud.Where(s => s.libera == false && s.departamento==usuario.FirstOrDefault().departamento);

                int pageSize = 10;
                int pageNumber = (page ?? 1);

                return View(solicitud.OrderBy(i => i.preRequisicion).ToPagedList(page ?? 1, pageSize));
            }
            catch (Exception ex)
            {
                var e=ex;
                return View(e.Message);
            }
            
        }
        [Authorize(Roles = "Admin, Revisor2")]
        public ActionResult RevisionExterna(string sortOrder, string currentFilter, string searchString, int? page)
        {
            try
            {
                var usuario = DB.Usuarios.Where(u => u.nombreUsuario == User.Identity.Name);
                var solicitud = from s in DB.Solicitud_Requisiciones select s;
                if (usuario.FirstOrDefault().Role.Equals("Admin"))
                {
                    solicitud = solicitud.Where(s => s.liberaLocal == "A" && (s.liberaCapitalHumano != "A" || s.liberaElectrico != "A" ||
                                                s.liberaSeguridad != "A"));
                }
                else
                {
                    var detUsuario = from u in DB.DetallesUsuarios select u;
                    detUsuario = detUsuario.Where(u => u.idUsuario == usuario.FirstOrDefault().idUsuario);
                    if (detUsuario.FirstOrDefault().seguridad == true)
                    {
                        solicitud = solicitud.Where(s => s.liberaLocal == "A" && s.liberaSeguridad != "A");
                    }
                    if (detUsuario.FirstOrDefault().electricos == true)
                    {
                        solicitud = solicitud.Where(s => s.liberaLocal == "A" && s.liberaElectrico != "A");
                    }
                    if (detUsuario.FirstOrDefault().capitalHumano == true)
                    {
                        solicitud = solicitud.Where(s => s.liberaLocal == "A" && s.liberaCapitalHumano != "A");
                    }

                }

                int pageSize = 10;
                int pageNumber = (page ?? 1);

                return View(solicitud.OrderBy(i => i.preRequisicion).ToPagedList(page ?? 1, pageSize));
            }
            catch (Exception ex)
            {
                var e = ex;
                return View(e.Message);
            }
            
        }
        [Authorize(Roles = "Admin, Revisor1, Revisor2")]
        public ActionResult RevisionTotal(string sortOrder, string currentFilter, string searchString, int? page, string fechaInicial, string fechaFinal,string departamentoS,string cicloS,string ejercicioS)
        {
            try
            {
                var usuario = from u in DB.Usuarios select u;
                usuario = usuario.Where(u => u.nombreUsuario == User.Identity.Name);
                var solicitud = from s in DB.Solicitud_Requisiciones select s;
                if (usuario.FirstOrDefault().Role.Equals("Admin"))
                {
                    solicitud = solicitud.Where(s => s.liberaLocal == "A");
                }
                else
                {
                    solicitud = solicitud.Where(s => s.departamento == usuario.FirstOrDefault().departamento && s.liberaLocal == "A");
                }
                if (!String.IsNullOrEmpty(fechaInicial) && !String.IsNullOrEmpty(fechaFinal))
                {
                    DateTime fi = Convert.ToDateTime(Convert.ToDateTime(fechaInicial).ToString("yyyy-MM-dd HH:mm:ss.fff"));
                    DateTime ff = Convert.ToDateTime(Convert.ToDateTime(fechaFinal).ToString("yyyy-MM-dd HH:mm:ss.fff"));
                    solicitud = solicitud.Where(s => s.fechaRequisicion >= fi && s.fechaRequisicion <= ff);
                }
                if (!string.IsNullOrEmpty(departamentoS))
                {
                    int dep = Int32.Parse(departamentoS);
                    solicitud = solicitud.Where(s => s.departamento == dep);
                }
                if (!string.IsNullOrEmpty(cicloS))
                {
                    solicitud = solicitud.Where(s => s.ciclo.Equals(cicloS));
                }
                if (!string.IsNullOrEmpty(ejercicioS))
                {
                    int ej = Int32.Parse(ejercicioS);
                    solicitud = solicitud.Where(s => s.ejercicio == ej);
                }
                int pageSize = 10;
                int pageNumber = (page ?? 1);

                return View(solicitud.OrderBy(i => i.preRequisicion).ToPagedList(page ?? 1, pageSize));
            }
            catch (Exception ex)
            {
                var e = ex;
                return View(e.Message);
            }
            
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
                    string query = "UPDATE Solicitud_Requisiciones SET liberaLocal = 'A' WHERE preRequisicion =" + preRequisicion;
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
                        query = "UPDATE Solicitud_Requisiciones SET liberaSeguridad = 'A',liberaElectrico = 'A',liberaCapitalHumano = 'A'  WHERE preRequisicion =" + preRequisicion;
                    }
                    else {
                        if (detUsuario.FirstOrDefault().seguridad == true)
                        {
                            query = "UPDATE Solicitud_Requisiciones SET liberaSeguridad = 'A' WHERE preRequisicion =" + preRequisicion;
                        }
                        if (detUsuario.FirstOrDefault().electricos == true)
                        {
                            query = "UPDATE Solicitud_Requisiciones SET liberaElectrico = 'A' WHERE preRequisicion =" + preRequisicion;
                        }
                        if (detUsuario.FirstOrDefault().capitalHumano == true)
                        {
                            query = "UPDATE Solicitud_Requisiciones SET liberaCapitalHumano = 'A' WHERE preRequisicion =" + preRequisicion;
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
        public ActionResult RechazarSolicitud(int preRequisicion)
        {
            try
            {
                var context = new AlmacenEntities();
                var connection = context.Database.Connection;
                string s = "";
                using (SqlConnection con = new SqlConnection(connection.ConnectionString))
                {
                    string query = "UPDATE Solicitud_Requisiciones SET liberaLocal = 'N' WHERE preRequisicion =" + preRequisicion;
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
        public ActionResult RechazarSolicitudExterna(int preRequisicion)
        {
            try
            {

                var context = new AlmacenEntities();
                var connection = context.Database.Connection;
                string s = "";
                using (SqlConnection con = new SqlConnection(connection.ConnectionString))
                {
                    string query = "";
                    var usuario = from u in DB.Usuarios select u;
                    usuario = usuario.Where(u => u.nombreUsuario == User.Identity.Name);
                    var detUsuario = from d in DB.DetallesUsuarios select d;
                    detUsuario = detUsuario.Where(u => u.idUsuario == usuario.FirstOrDefault().idUsuario);
                    if (usuario.FirstOrDefault().Role.Equals("Admin"))
                    {
                        query = "UPDATE Solicitud_Requisiciones SET liberaSeguridad = 'N',liberaElectrico = 'N',liberaCapitalHumano = 'N'  WHERE preRequisicion =" + preRequisicion;
                    }
                    else
                    {
                        if (detUsuario.FirstOrDefault().seguridad == true)
                        {
                            query = "UPDATE Solicitud_Requisiciones SET liberaSeguridad = 'N' WHERE preRequisicion =" + preRequisicion;
                        }
                        if (detUsuario.FirstOrDefault().electricos == true)
                        {
                            query = "UPDATE Solicitud_Requisiciones SET liberaElectrico = 'N' WHERE preRequisicion =" + preRequisicion;
                        }
                        if (detUsuario.FirstOrDefault().capitalHumano == true)
                        {
                            query = "UPDATE Solicitud_Requisiciones SET liberaCapitalHumano = 'N' WHERE preRequisicion =" + preRequisicion;
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
            try
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
                var r = Results.Select(
                    a => new
                    {
                        a.idDReq,
                        a.detalle,
                        a.partida,
                        a.cantidad,
                        a.costoU,
                        a.departamento,
                        descripcion = (DB.Materiales.Where(s => s.idMaterial == a.material).FirstOrDefault().descripcion == null) ? a.descripcion : DB.Materiales.Where(s => s.idMaterial == a.material).FirstOrDefault().descripcion,
                        idMaterial = (DB.Materiales.Where(s => s.idMaterial == a.material).FirstOrDefault().idMaterial == null) ? a.material : DB.Materiales.Where(s => s.idMaterial == a.material).FirstOrDefault().idMaterial
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
            catch (Exception ex)
            {
                var jsonData = new
                {
                    error=ex.Message
                };
                return Json(jsonData, JsonRequestBehavior.AllowGet);
            }
        }
        [Authorize(Roles = "Admin, Revisor1, Revisor2,Empleado")]
        public ActionResult ListaSolicitudes(string sortOrder, string currentFilter, string searchString, int? page)
        {
            try
            {
                var ud = User.Identity.Name;
                var usuario = DB.Usuarios.Where(u => u.nombreUsuario == ud).FirstOrDefault();
                
                if (usuario.Role.Equals("Admin"))
                {
                    int pageSize = 10;
                    int pageNumber = (page ?? 1);

                    return View(DB.Solicitud_Requisiciones.OrderBy(i => i.preRequisicion).ToPagedList(page ?? 1, pageSize));
                }
                else
                {
                   
                    var idE = Int32.Parse(usuario.idEmpleado);
                    int pageSize = 10;
                    int pageNumber = (page ?? 1);
                    var Result = DB.Solicitud_Requisiciones.Where(s => s.solicitante == idE).OrderBy(i => i.preRequisicion).ToPagedList(page ?? 1, pageSize);
                    return View(Result);
                }
                //solicitud = solicitud.Where(s => s.libera == false && s.departamento==usuario.FirstOrDefault().departamento);

                
            }
            catch (Exception ex)
            {
                var e = ex;
                return View(e.Message);
            }

        }
        public JsonResult GetSolicitudImpimir(string preRequisicion, string departamento, string ejercicio)
        {
            try
            {
                int preR = Int32.Parse(preRequisicion);
                int dep = Int32.Parse(departamento);
                int ejer = Int32.Parse(ejercicio);
                var solicitud = DB.Solicitud_Requisiciones.Where(s=>s.preRequisicion==preR && s.departamento==dep  
                                && s.ejercicio==ejer );
                List<DetalleRequisicion> partidas = DB.DetalleRequisicion.Where(s => s.preRequisicion == preR && s.departamento == dep
                                && s.ejercicio == ejer).ToList();
                var jsonData = new
                {
                    solicitud = solicitud.Select(
                            a => new
                            {
                                a.anexo,
                                a.area,
                                a.ciclo,
                                a.departamento,
                                a.departamentoSolicitante,
                                a.ejercicio,
                                a.estatus,
                                a.fechaNecesitar,
                                a.fechaRequisicion,
                                a.liberaAlmacen,
                                a.liberaCapitalHumano,
                                a.liberaElectrico,
                                a.liberaLocal,
                                a.liberaSeguridad,
                                a.observaciones,
                                a.preRequisicion,
                                a.preRequisicionAnt,
                                a.requisicion,
                                a.solicitante,
                                a.uso
                            }).FirstOrDefault(),
                    partidas=partidas.Select(
                        a=> new {
                        unidad= DB.Materiales.Where(s => s.idMaterial == a.material).FirstOrDefault().uMedida ?? "---",
                        a.cantidad,
                        a.costoTotal,
                        a.costoU,
                        a.departamento,
                        a.descripcion,
                        a.detalle,
                        a.ejercicio,
                        a.existencia,
                        a.FechaUltimaEntrada,
                        a.idDReq,
                        a.material,
                        a.partida,
                        a.preRequisicion,
                        a.requisicion
                    })
                };
                //var json = new JavaScriptSerializer().Serialize(jsonData);
                return Json(jsonData, JsonRequestBehavior.AllowGet);

            }
            catch (Exception ex)
            {
                var jsonData = new
                {
                    code="Error",
                    message = ex.Message
                };
                return Json(jsonData, JsonRequestBehavior.AllowGet);
            }
        }
    }
}