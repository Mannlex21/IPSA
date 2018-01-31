using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AplicacionWebMVC.Models;
using System.Web.Script.Serialization;
using System.Data.SqlClient;
using System.Configuration;
using System.Net;
using System.IO;
using System.Web.Services;

namespace AplicacionWebMVC.Controllers
{
    public class RequisicionController : Controller
    {

        public static string carpetaAnexosSol = "/WebSolicitudesAnexos/";
        //public static string carpetaAnexosSol = @"E:\Documentos\SolicitudesAnexos\";

        // GET: Requisicion
        public ActionResult Index()
        {
            return View();
        }
        
        public JsonResult setRequisicion(Solicitud solicitud, List<Partidas> partidas, Checks checks) //Gets the todo Lists.  
        {
            try
            {
                string s;
                AlmacenEntities db = new AlmacenEntities();
                var context = new AlmacenEntities();
                var connection = context.Database.Connection;
                int preReq = db.Solicitud_Requisiciones.Where(s2=>s2.departamento==solicitud.departamento).Count();

               
                Solicitud_Requisiciones soli= new Solicitud_Requisiciones();
                soli.preRequisicion = preReq + 1;
                soli.preRequisicionAnt = 0;
                soli.requisicion = solicitud.idRequisicion;
                soli.fechaNecesitar = Convert.ToDateTime(solicitud.fechaNecesitar);
                soli.fechaRequisicion = Convert.ToDateTime(solicitud.fechaRequisicion);
                soli.uso = solicitud.uso;
                soli.departamento = (Int16)solicitud.departamento;
                soli.ciclo = solicitud.ciclo.ToString();
                soli.area = solicitud.area.ToString();
                soli.fechaRecepcion = Convert.ToDateTime(solicitud.fechaRecepcion);
                soli.ejercicio = solicitud.ejercicio;
                soli.solicitante = solicitud.solicitante;
                soli.observaciones = (solicitud.observaciones == null) ? "---" :solicitud.observaciones;
                soli.liberaElectrico = (checks.electrico == false) ? true : false;
                soli.liberaCapitalHumano = (checks.trabajoSindicato == false && checks.retencionImpuesto == false) ? true : false;
                soli.liberaSeguridad = (checks.soldadura == false && checks.altura == false
                            && checks.espaciosConfinados == false && checks.izajes == false && checks.montacarga == false) ? true : false;
                soli.liberaLocal = false;
                soli.liberaAlmacen = false;
                db.Solicitud_Requisiciones.Add(soli);
                
                int cont = 0;
                string r = "";
                foreach (var value in partidas)
                {
                    DetalleRequisicion detR = new DetalleRequisicion();
                    detR.preRequisicion = preReq + 1;
                    detR.requisicion = solicitud.idRequisicion;
                    detR.partida = (Int16)cont++;
                    detR.material = Int32.Parse(value.Clave);
                    detR.cantidad = Decimal.Parse(value.Cantidad);
                    detR.detalle = value.Descripcion;
                    detR.descripcion =  value.Detalle;
                    detR.ejercicio = solicitud.ejercicio;
                    detR.costoU = Decimal.Parse(value.PrecioU);
                    detR.costoTotal = Decimal.Parse(value.PrecioU) * Decimal.Parse(value.Cantidad);
                    detR.existencia = Decimal.Parse(value.Existencia);
                    detR.FechaUltimaEntrada = Convert.ToDateTime("01/01/2017");
                    detR.departamento = Int16.Parse(solicitud.departamento.ToString());
                    db.DetalleRequisicion.Add(detR);
                }
                DetalleRequisicion2 detR2 = new DetalleRequisicion2();
                detR2.preRequisicion = preReq + 1;
                detR2.departamento = (Int16)solicitud.departamento;
                detR2.ejercicio = (Int16)solicitud.ejercicio;
                detR2.trabajoSindicato = checks.trabajoSindicato;
                detR2.retencionImpuesto = checks.retencionImpuesto;
                detR2.altura = checks.altura;
                detR2.espaciosConfinados = checks.espaciosConfinados;
                detR2.electrico = checks.electrico;
                detR2.corte = checks.corte;
                detR2.corte = checks.corte;
                detR2.soldadura = checks.soldadura;
                detR2.operacionMontacargas = checks.montacarga;
                detR2.izajesCarga = checks.izajes;
                db.DetalleRequisicion2.Add(detR2);
                db.SaveChanges();
                
                var jsonData = new { code = "OK", preRequisicion= preReq + 1,departamento= (Int16)solicitud.departamento,
                    ejercicio= solicitud.ejercicio
                };
                return Json(jsonData, JsonRequestBehavior.AllowGet);
            }
            catch (SqlException odbcEx)
            {
                var jsonData = new { code = odbcEx };
                return Json(jsonData, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex){
                Response.StatusCode = (int)HttpStatusCode.BadRequest;
                List<string> errors = new List<string>();
                errors.Add(ex.Message);
                return Json(errors);
                //var jsonData = new{code = ex};
                //return Json(jsonData, JsonRequestBehavior.AllowGet);
            }
        }
        public JsonResult GetReq()
        {
            var json = new
            {
                code = "ok"
            };
            return Json(json,JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public ActionResult UploadFiles()
        {

            try
            {
                var files = Request.Files;
                var req = Request;
                var id = Request.Form[0];
                var departamento= Request.Form[1];
                var ejercicio = Request.Form[2];
                var url="";
                if (files.Count>0)
                {
                    var carpeta = "SolicitudReq-" + id+"-"+departamento+"-"+ejercicio;
                    url = RutasGenerales.root + carpeta;
                    crearCarpetaAdjunto(url);

                    var context = new AlmacenEntities();
                    var connection = context.Database.Connection;
                    using (SqlConnection con = new SqlConnection(connection.ConnectionString))
                    {
                        string query = "UPDATE Solicitud_Requisiciones SET anexo = '" + carpeta + "' " +
                            "WHERE departamento =" + departamento + " and ejercicio=" + ejercicio + " and preRequisicion=" + id;
                        using (SqlCommand cmd = new SqlCommand(query))
                        {
                            cmd.Connection = con;
                            con.Open();
                            cmd.ExecuteScalar();
                            con.Close();
                        }
                    }
                }
                else
                {
                    var context = new AlmacenEntities();
                    var connection = context.Database.Connection;
                    using (SqlConnection con = new SqlConnection(connection.ConnectionString))
                    {
                        string query = "UPDATE Solicitud_Requisiciones SET anexo = '---' " +
                            "WHERE departamento =" + departamento + " and ejercicio=" + ejercicio + " and preRequisicion=" + id;
                        using (SqlCommand cmd = new SqlCommand(query))
                        {
                            cmd.Connection = con;
                            con.Open();
                            cmd.ExecuteScalar();
                            con.Close();
                        }
                    }
                }
                
                for (int i = 0; i < files.Count; i++)
                {
                    var file = Request.Files[i];
                    string fileName = Path.GetFileName(file.FileName);
                    string[] ext = fileName.Split('.');
                    string[] archivos = Directory.GetFiles(url);
                    int fc = archivos.Length + 1;

                    var nuevoNombre = "S-" + fc + "." + ext[1];
                    var path = Path.Combine(url, nuevoNombre);
                    file.SaveAs(path);
                }
                return Json(new { IsSucccess = true, ServerMessage = "Se subio correctamente"}, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex) {
                return Json(new { IsSucccess = false, ServerMessage = "Error: "+ex.Message}, JsonRequestBehavior.AllowGet);
            }
            
        }
        /*public String crearImagen(String url, String primero, String id)
        {
            try
            {
                url = url.ToLower();

                String ext = (url.EndsWith(".png")) ? ".png" : ".jpg";
                try
                {
                    String nuevo = carpetaImagen + primero + "-" + id + ext;
                    File.Copy(url, nuevo, true);
                    return nuevo;
                }
                catch (FileNotFoundException ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
            catch (IOException e)
            {
                Console.WriteLine(e);
            }
            return "";
        }*/
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
        /*public void crearImagenes(string url, string primero, string id, string urlN)
        {
            try
            {
                url = url.ToLower();
                String[] urlA = url.Split(',');
                for (int i = 0; i < urlA.Length; i++)
                {
                    string nuevo = "";
                    if (urlA[i] != "")
                    {
                        string[] parts = urlA[i].Split('\\');
                        string[] ext = parts[parts.Length - 1].Split('.');
                        string[] files = Directory.GetFiles(urlN);
                        int fc = files.Length + 1;

                        nuevo = "M-" + fc + "." + ext[1];
                        File.Copy(urlA[i], urlN + "\\" + nuevo, true);
                    }
                }
            }
            catch (IOException e)
            {
                Console.WriteLine(e);
            }
        }*/
    }
}