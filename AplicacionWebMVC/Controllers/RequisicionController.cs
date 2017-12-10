using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AplicacionWebMVC.Models;
using System.Web.Script.Serialization;
using System.Data.SqlClient;
using System.Configuration;

namespace AplicacionWebMVC.Controllers
{
    public class RequisicionController : Controller
    {
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
                var context = new AlmacenEntities();
                var connection = context.Database.Connection;
                int preReq = 0;
                using (SqlConnection con = new SqlConnection(connection.ConnectionString))
                {
                    string query = "SELECT COUNT(*) FROM Solicitud_Requisiciones WHERE departamento=" + solicitud.departamento;
                    using (SqlCommand cmd = new SqlCommand(query))
                    {
                        cmd.Connection = con;
                        con.Open();
                        preReq = Convert.ToInt32(cmd.ExecuteScalar());
                        con.Close();
                    }
                    

                }
                using (SqlConnection con = new SqlConnection(connection.ConnectionString))
                {
                    string query = "INSERT INTO Solicitud_Requisiciones(preRequisicion,preRequisicionAnt,requisicion,fechaRequisicion,fechaNecesitar,uso,departamento,ciclo,area,fechaRecepcion,ejercicio,solicitante,observaciones,liberaLocal,liberaSeguridad,liberaElectrico,liberaCapitalHumano) " +
                        "VALUES(@preRequisicion,@preRequisicionAnt,@requisicion,@fechaRequisicion,@fechaNecesitar,@uso,@departamento,@ciclo,@area,@fechaRecepcion,@ejercicio,@solicitante,@observaciones,@liberaLocal,@liberaSeguridad,@liberaElectrico,@liberaCapitalHumano)";
                    query += " SELECT SCOPE_IDENTITY()";
                    using (SqlCommand cmd = new SqlCommand(query))
                    {
                        cmd.Connection = con;
                        con.Open();
                        cmd.Parameters.AddWithValue("@preRequisicion", preReq+1);
                        cmd.Parameters.AddWithValue("@preRequisicionAnt", 0);
                        cmd.Parameters.AddWithValue("@requisicion", solicitud.idRequisicion);
                        cmd.Parameters.AddWithValue("@fechaNecesitar", Convert.ToDateTime(solicitud.fechaNecesitar));
                        cmd.Parameters.AddWithValue("@fechaRequisicion", Convert.ToDateTime(solicitud.fechaRequisicion));
                        cmd.Parameters.AddWithValue("@uso", solicitud.uso);
                        cmd.Parameters.AddWithValue("@departamento", solicitud.departamento);
                        cmd.Parameters.AddWithValue("@ciclo", solicitud.ciclo);
                        cmd.Parameters.AddWithValue("@area", solicitud.area);
                        cmd.Parameters.AddWithValue("@fechaRecepcion", Convert.ToDateTime(solicitud.fechaRecepcion));
                        cmd.Parameters.AddWithValue("@ejercicio", solicitud.ejercicio);
                        cmd.Parameters.AddWithValue("@solicitante", solicitud.solicitante);
                        if (solicitud.observaciones == null)
                        {
                            cmd.Parameters.AddWithValue("@observaciones", "---");
                        }
                        else
                        {
                            cmd.Parameters.AddWithValue("@observaciones", solicitud.observaciones);
                        }
                        if ( checks.electrico == false ) {
                            cmd.Parameters.AddWithValue("@liberaElectrico", true);
                        }
                        else
                        {
                            cmd.Parameters.AddWithValue("@liberaElectrico", false);
                        }
                        if (checks.soldadura == false && checks.altura==false 
                            && checks.espaciosConfinados == false && checks.izajes == false && checks.montacarga == false)
                        {
                            cmd.Parameters.AddWithValue("@liberaSeguridad", true);
                        }
                        else
                        {
                            cmd.Parameters.AddWithValue("@liberaSeguridad", false);
                        }
                        if (checks.trabajoSindicato == false && checks.retencionImpuesto== false)
                        {
                            cmd.Parameters.AddWithValue("@liberaCapitalHumano", true);
                        }
                        else
                        {
                            cmd.Parameters.AddWithValue("@liberaCapitalHumano", false);
                        }
                        cmd.Parameters.AddWithValue("@liberaLocal", false);
                        s = cmd.ExecuteScalar().ToString();
                        con.Close();
                    }
                }
                int cont=0;
                string r="";
                foreach (var value in partidas)
                {

                    
                    using (SqlConnection con = new SqlConnection(connection.ConnectionString))
                    {
                        string query = "INSERT INTO DetalleRequisicion (preRequisicion,requisicion,partida,material,cantidad,detalle,ejercicio,costoU,costoTotal,existencia,FechaUltimaEntrada,departamento) " +
                            "VALUES(@preRequisicion, @requisicion, @partida, @material, @cantidad, @detalle, @ejercicio, @costoU, @costoTotal, @existencia, @FechaUltimaEntrada,@departamento)";
                        query += " SELECT SCOPE_IDENTITY()";
                        using (SqlCommand cmd = new SqlCommand(query))
                        {
                            cmd.Connection = con;
                            con.Open();
                            cmd.Parameters.AddWithValue("@preRequisicion", preReq+1);
                            cmd.Parameters.AddWithValue("@requisicion", solicitud.idRequisicion);
                            cmd.Parameters.AddWithValue("@partida", cont++);
                            cmd.Parameters.AddWithValue("@material", Int32.Parse(value.Clave));
                            cmd.Parameters.AddWithValue("@cantidad", float.Parse(value.Cantidad));
                            cmd.Parameters.AddWithValue("@detalle", value.Detalle);
                            cmd.Parameters.AddWithValue("@ejercicio", solicitud.ejercicio);
                            cmd.Parameters.AddWithValue("@costoU", float.Parse(value.PrecioU));
                            cmd.Parameters.AddWithValue("@costoTotal", float.Parse(value.PrecioU)*float.Parse(value.Cantidad));
                            cmd.Parameters.AddWithValue("@existencia", float.Parse(value.Existencia));
                            cmd.Parameters.AddWithValue("@FechaUltimaEntrada", Convert.ToDateTime("01/01/2017"));
                            cmd.Parameters.AddWithValue("@departamento", solicitud.departamento);
                            r =r+cmd.ExecuteScalar().ToString()+",";
                            con.Close();
                        }
                    }
                }
                using (SqlConnection con = new SqlConnection(connection.ConnectionString))
                {
                    string query = "INSERT INTO DetalleRequisicion2 (trabajoSindicato,retencionImpuesto,altura,espaciosConfinados,electrico,corte,soldadura,operacionMontacargas,izajesCarga,preRequisicion,departamento,ejercicio) " +
                        "VALUES(@trabajoSindicato, @retencionImpuesto, @altura, @espaciosConfinados, @electrico, @corte, @soldadura, @operacionMontacargas, @izajesCarga, @preRequisicion, @departamento, @ejercicio)";
                    query += " SELECT SCOPE_IDENTITY()";
                    using (SqlCommand cmd = new SqlCommand(query))
                    {
                        cmd.Connection = con;
                        con.Open();
                        cmd.Parameters.AddWithValue("@preRequisicion", preReq + 1);
                        cmd.Parameters.AddWithValue("@departamento", solicitud.departamento);
                        cmd.Parameters.AddWithValue("@ejercicio", solicitud.ejercicio);
                        cmd.Parameters.AddWithValue("@trabajoSindicato", checks.trabajoSindicato);
                        cmd.Parameters.AddWithValue("@retencionImpuesto", checks.retencionImpuesto);
                        cmd.Parameters.AddWithValue("@altura", checks.altura);
                        cmd.Parameters.AddWithValue("@espaciosConfinados", checks.espaciosConfinados);
                        cmd.Parameters.AddWithValue("@electrico", checks.electrico);
                        cmd.Parameters.AddWithValue("@corte", checks.corte);
                        cmd.Parameters.AddWithValue("@soldadura", checks.soldadura);
                        cmd.Parameters.AddWithValue("@operacionMontacargas", checks.montacarga);
                        cmd.Parameters.AddWithValue("@izajesCarga", checks.izajes);
                        s = cmd.ExecuteScalar().ToString();
                        con.Close();
                    }
                }
                var jsonData = new { code = "OK",resultSol=s,resultPar=r, partidas=partidas,solicitud=solicitud, preRequisicion= preReq + 1 };
                return Json(jsonData, JsonRequestBehavior.AllowGet);
            }
            catch (SqlException odbcEx)
            {
                var jsonData = new { code = odbcEx };
                return Json(jsonData, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex){
                var jsonData = new{code = ex};
                return Json(jsonData, JsonRequestBehavior.AllowGet);
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
    }
}