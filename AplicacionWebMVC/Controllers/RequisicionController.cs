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
                    detR.detalle = value.Detalle;
                    detR.descripcion = value.Descripcion;
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
                
                var jsonData = new { code = "OK",resultSol=0,resultPar=r, partidas=partidas,solicitud=solicitud, preRequisicion= preReq + 1 };
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