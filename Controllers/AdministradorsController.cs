using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using asistenteventas.Data;
using asistenteventas.Models;
using MimeKit;
using Microsoft.AspNetCore.Authorization;

namespace asistenteventas.Controllers
{
    [Authorize(Roles = "1,1")]
    public class AdministradorsController : Controller
    {
        private readonly ASISTENTE_DE_VENTASContext _context;

        public AdministradorsController(ASISTENTE_DE_VENTASContext context)
        {
            _context = context;
        }

        // GET: Administradors1
        [Authorize(Roles = "1,1")]
        public async Task<IActionResult> Index()
        {
            var aSISTENTE_DE_VENTASContext = _context.Usuarios.Where(u=>u.rol ==2);
            return View(await aSISTENTE_DE_VENTASContext.ToListAsync());
        }
        [Authorize(Roles = "1,1")]
        public IActionResult EnviarCorreo()
        {
            // Obtener la fecha y hora actuales
            DateTime ayer = DateTime.Today;

            //Consultar si hay nuevas compras realizadas a partir de las 00 horas del día de ayer
            var nuevascompras = _context.Faclocs.Where(p => p.FecFacLoc > ayer).ToList();
            if (nuevascompras.Count > 0)
            {


                // Enviar un correo electrónico a cada  usuario con link a la encuesta
                foreach (var compra in nuevascompras)
                {
                    var usu = _context.Clients.Where(c => c.CodCli == compra.CodCli).SingleOrDefault();
                    //creo el mensaje con todas sus partes 
                    var mensaje = new MimeMessage();
                    mensaje.From.Add(new MailboxAddress("Administrador", "edwardjunior49@gmail.com"));
                    mensaje.To.Add(new MailboxAddress(usu.PriNomCli, usu.MailCli));
                    mensaje.Subject = "Encuesta sobre su compra";

                    var bodyBuilder = new BodyBuilder();
                    bodyBuilder.HtmlBody = "<h1>Encuesta sobre su compra</h1><p>Por favor, complete la siguiente encuesta:</p><a href=\"http://localhost:5214/Clients/Encuesta\">Ir a la encuesta</a>";
                    mensaje.Body = bodyBuilder.ToMessageBody();
                    //me conecto co autenticacion 
                    using (var client = new MailKit.Net.Smtp.SmtpClient())
                    {
                        client.Connect("smtp.gmail.com", 587, false);
                        client.Authenticate("edwardjunior49@gmail.com", "bxdsvybobxgxzxli");
                        client.Send(mensaje);
                        //me desconecto
                        client.Disconnect(true);
                    }
                }

            }
            ViewBag.men = "Correos enviados ";
            var ven = _context.Usuarios.Where(u => u.rol == 2);
            return View("Index", ven);
        }

    }
}
