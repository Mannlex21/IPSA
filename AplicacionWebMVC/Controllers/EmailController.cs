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
                var Results = DB.Configuracion.Select(
                a => new
                {
                    a.contrasenaCorreoE,
                    a.correoEnvia,
                    a.correoRecibe,
                    a.host,
                    a.puerto
                }).FirstOrDefault();
                string Host = "mail.ingeniopuga.com.mx";
                int Puerto = 465;
                string fromCorreo = Results.correoEnvia;
                var toCorreo = Results.correoRecibe;
                var passCorreo = Results.contrasenaCorreoE;
                System.Diagnostics.Debug.WriteLine(Results);
                var body = 
                    "<h1 style='color: #5e9ca0; text-align: center;'>Se registro una pre-requisicion!</h1>" +
                    "<h2><span style = 'color: #000000;'> Departamento: {0}</span></h2>" +
                    "<h2><span style = 'color: #000000;'> Uso: {1}</span></h2>" +
                    "<h2><span style = 'color: #000000;'> Fecha a necesitar: {2}</span></h2>" +
                    "<h2><span style = 'color: #000000;'> Fecha de requisicion: {3}</span></h2>" +
                    "<h2><span style = 'color: #000000;'> Observaciones: {4}</span></h2>";

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