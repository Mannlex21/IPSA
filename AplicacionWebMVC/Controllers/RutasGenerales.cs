using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Net;
using System.IO;
namespace AplicacionWebMVC.Controllers
{
    public class RutasGenerales
    {
        public static string carpetaImagen = "\\\\172.16.0.5\\Materiales\\Principal\\";
        public static string carpetaAdjunto = "\\\\172.16.0.5\\Materiales\\Adjuntos\\";
        public static string ipServidor = "172.16.0.5";
        //public static string root = HttpContext.Current.Server.MapPath("/WebSolicitudesAnexos/");

        //public static string carpetaImagen = @"E:\Documentos\Programacion\MatImg\Material\";
        //public static string carpetaAdjunto = @"E:\Documentos\Programacion\MatImg\Adjuntos\";
        //public static string carpetaAdjuntoSoli = @"E:\Documentos\SolicitudesAnexos\";
        //public static string ipServidor = "192.168.0.2";
        //public static string root = @"E:\Documentos\SolicitudesAnexos\";
    }
}