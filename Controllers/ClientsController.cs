using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using asistenteventas.Data;
using asistenteventas.Models;
using Microsoft.AspNetCore.Authorization;

namespace asistenteventas.Controllers
{
    
    public class ClientsController : Controller
    {
        private readonly ASISTENTE_DE_VENTASContext _context;

        public ClientsController(ASISTENTE_DE_VENTASContext context)
        {
            _context = context;
        }

        // GET: Clients


        // GET: Clients/Details/5
        [Authorize(Roles = "1,2")]
        public async Task<IActionResult> Details(decimal? id)
        {
            if (id == null || _context.Clients == null)
            {
                return NotFound();
            }

            var client = await _context.Clients
                .FirstOrDefaultAsync(m => m.CodCli == id);
            if (client == null)
            {
                return NotFound();
            }

            return View(client);
        }

        [Authorize(Roles = "1,2")]
        public IActionResult CreateCliente()
        {
           
                return View();
           

        }

        // POST: Clients/Create

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "1,2")]
        public async Task<IActionResult> CreateCliente([Bind("CodCli,MailCli,PriNomCli,NroDocCli,PriApeCli,SegApeCli")] Client client)
        {
            try { 
            if (ModelState.IsValid)
            {
                if (client == null)
                {
                    ViewBag.mje = "No se ha creado el cliente con éxito";
                    return View();
                }

                _context.Add(client);
                await _context.SaveChangesAsync();

                // Establece un mensaje de éxito en ViewBag
                ViewBag.mje = "Cliente creado con éxito";

                    // Devuelve la vista "Create" con el mensaje de éxito
                    return RedirectToAction("Details", new { id = client.CodCli });
                } 
                
                return View(client);
            }catch (Exception ex)
            {
                ViewBag.mje = "error en crear cliente, favor  inetente nuevamente";
                return View(client);
            }

          
        }




        public ActionResult Encuesta()
        {
            ViewBag.mje = "";
            return View();
        }
        [HttpPost]
        public ActionResult Encuesta(string cedula, string calificacion, string comentario)
        {
            try
            {
                var facloc = _context.Faclocs.Where(f => f.CodCli == decimal.Parse(cedula)).OrderByDescending(f => f.FecFacLoc).FirstOrDefault();
                var facloc1 = _context.Facloc1s.Where(f1 => f1.Facloc == facloc).FirstOrDefault();
                if (facloc1 == null)
                {
                    ViewBag.mje = "no se pudo hacer la encuesta correctamente";
                    return View();
                }
                //creo un nuevo regustro de  la tabla datoscompras
                DatosCompras dt = new DatosCompras(decimal.Parse(cedula), facloc1.CodArt, int.Parse(calificacion), comentario);

                _context.Add(dt);
                _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                ViewBag.mje = ex.Message;
                return View();
            }
            ViewBag.mje = "Encuesta Completa muchas gracias !!";
            return View();
        }



    }
}
