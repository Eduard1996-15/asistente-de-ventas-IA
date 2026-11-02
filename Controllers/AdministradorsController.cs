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
        private readonly IConfiguration _configuration;

        public AdministradorsController(ASISTENTE_DE_VENTASContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
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

            // Configuracion SMTP: se toma de variables de entorno o user-secrets.
            // No se versionan credenciales (ver README).
            var smtpUser = _configuration["Smtp:User"];
            var smtpPassword = _configuration["Smtp:Password"];
            var smtpHost = _configuration["Smtp:Host"] ?? "smtp.example.com";
            var smtpPort = int.TryParse(_configuration["Smtp:Port"], out var parsedPort) ? parsedPort : 587;

            if (string.IsNullOrWhiteSpace(smtpUser) || string.IsNullOrWhiteSpace(smtpPassword))
            {
                ViewBag.men = "Falta configurar Smtp:User y Smtp:Password para enviar correos.";
                return View("Index", _context.Usuarios.Where(u => u.rol == 2));
            }

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
                    mensaje.From.Add(new MailboxAddress("Administrador", smtpUser));
                    mensaje.To.Add(new MailboxAddress(usu.PriNomCli, usu.MailCli));
                    mensaje.Subject = "Encuesta sobre su compra";

                    var bodyBuilder = new BodyBuilder();
                    bodyBuilder.HtmlBody = "<h1>Encuesta sobre su compra</h1><p>Por favor, complete la siguiente encuesta:</p><a href=\"http://localhost:5214/Clients/Encuesta\">Ir a la encuesta</a>";
                    mensaje.Body = bodyBuilder.ToMessageBody();
                    //me conecto co autenticacion 
                    using (var client = new MailKit.Net.Smtp.SmtpClient())
                    {
                        client.Connect(smtpHost, smtpPort, false);
                        client.Authenticate(smtpUser, smtpPassword);
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
