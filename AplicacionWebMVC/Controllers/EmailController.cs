using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AplicacionWebMVC.Models;
using System.Web.Script.Serialization;
using System.Net.Mail;
using System.Net;

namespace AplicacionWebMVC.Controllers
{
    public class EmailController : Controller
    {
        // GET: Email
        AlmacenEntities DB = new AlmacenEntities();
        
        public ActionResult Index()
        {
            return View();
        }
        public JsonResult sendEmailAsync(Email email)
        {
            try
            {
                var Results = DB.Configuracion.FirstOrDefault();
                string Host = Results.host;//"mail.ingeniopuga.com.mx";
                int Puerto = Int32.Parse(Results.puerto.ToString());//26
                string fromCorreo = Results.correoEnvia;
                var toCorreo = Results.correoRecibe;
                var passCorreo = Results.contrasenaCorreoE;
                System.Diagnostics.Debug.WriteLine(Results);
                
                var body = 
                    "<div class='col-md-6' style='margin:auto;'>" +
                        "<div style='background-color: #fff;  border: 1px solid transparent; border-radius: 4px; box-shadow: 0 1px 1px rgba(0, 0, 0, 0.05); margin-bottom: 20px; border-color: #428bca;' class='preferred-selection panel panel-primary'>" +
                        "<div style='text-align:center;border-top-left-radius: 3px; border-top-right-radius: 3px; padding: 10px 15px; background-color: #428bca; border-color: #428bca; color: #fff;'>Se registro una pre-requisicion!</div>" +
                            "<table class='table manual_settings_table' style='border-collapse: collapse; border-spacing: 0; margin-bottom: 0; width: 100%; background-color: transparent; max-width: 100%;'>" +
                                "<tbody>" +
                                    "<tr>" +
                                        "<td style='border-top: 1px solid #ddd; padding: 8px;width: 240px;'>Departamento: </td>" +
                                        "<td style='border-top: 1px solid #ddd; padding: 8px;text-align:center;'>{0}</td>" +
                                    "</tr>" +
                                    "<tr>" +
                                        "<td style='border-top: 1px solid #ddd; padding: 8px;width: 240px;' >Uso: </td>" +
                                        "<td style='border-top: 1px solid #ddd; padding: 8px;text-align:center;' > {1}</td>" +
                                    "</tr>" +
                                    "<tr>" +
                                        "<td style='border-top: 1px solid #ddd; padding: 8px;width: 240px;' >Fecha a necesitar: </td>" +
                                        "<td style='border-top: 1px solid #ddd; padding: 8px;text-align:center;' > {2}</td>" +
                                    "</tr>" +
                                    "<tr>" +
                                        "<td style='border-top: 1px solid #ddd; padding: 8px;width: 240px;' >Fecha de requisicion: </td>" +
                                        "<td style='border-top: 1px solid #ddd; padding: 8px;text-align:center;' > {3}</td>" +
                                    "</tr>" +
                                    "<tr>" +
                                        "<td style='border-top: 1px solid #ddd; padding: 8px;width: 240px;' >Observaciones: </td>" +
                                        "<td style='border-top: 1px solid #ddd; padding: 8px;text-align:center;' > {4}</td>" +
                                    "</tr>" +
                                "</tbody>" +
                            "</table>" +
                    "</div>";

                var smtp = new SmtpClient
                {
                    Host = Host,
                    //Host = "smtp.gmail.com",
                    //Host = "pop.gmail.com",
                    Port = Puerto,
                    EnableSsl = true,
                    DeliveryMethod = SmtpDeliveryMethod.Network,
                    Credentials = new NetworkCredential(fromCorreo, passCorreo),
                    Timeout = 20000
                };
                using (var message = new MailMessage(new MailAddress(fromCorreo, "Sistema pre-requisicion"), new MailAddress(toCorreo, "Almacen"))
                {
                    Subject = email.Subject,
                    Body =  string.Format(body, email.departamento, email.uso, email.fechaNecesitar,email.fechaNecesitar,email.observaciones),
                    IsBodyHtml = true

            })
                    smtp.Send(message);
                var msj = new
                {
                    code = "ok"
                };
                return Json(msj, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                var msj = new
                {
                    code = "error", descripcion = "No se pudo enviar el correo",result=ex
                };
                return Json(msj, JsonRequestBehavior.AllowGet);
            }
        }
        
    }
}