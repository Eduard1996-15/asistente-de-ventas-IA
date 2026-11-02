using asistenteventas.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;

namespace asistenteventas.Controllers
{
   
    public class UsuariosController : Controller
    {
        private readonly ASISTENTE_DE_VENTASContext _context;
        public UsuariosController(ASISTENTE_DE_VENTASContext context)
        {
            _context = context;
        }
        public IActionResult Login()
        {

            if (User.IsInRole("1"))
            {
                return RedirectToAction("Index", "Administradors");
            }
            else if (User.IsInRole("2"))
            {
                return RedirectToAction("Index", "Vendedors");
            }
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Login(string n, string c)
        {

            if (User.HasClaim("Logged", "true"))
            {
                ViewBag.mje = "Ya hay una sesión activa para este usuario.";
                if (User.IsInRole("1"))
                {
                    return RedirectToAction("Index", "Administradors");
                }
                else if (User.IsInRole("2"))
                {
                    return RedirectToAction("Index", "Vendedors");
                }
            }
            try
            {
                var usu = _context.Usuarios.FirstOrDefault(
                u => u.nombre == n && u.passsword == c);

                if (usu == null)
                {
                    ViewBag.mje = "login incorrecto";
                    return View();
                }
                else
                {
                    var claims = new List<Claim>
                    {
                        new Claim(ClaimTypes.Name, usu.nombre),
                        new Claim("password", usu.passsword),
                         new Claim("Logged", "true")
                    };
                    claims.Add(new Claim(ClaimTypes.Role, usu.rol.ToString()));
                    var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                    await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity));

                    if (usu.rol == 1)
                    {
                        HttpContext.Session.SetString("Admi", usu.nombre);
                        return RedirectToAction("Index", "Administradors");
                    }
                    if (usu.rol == 2)
                    {
                        HttpContext.Session.SetString("Vend", usu.nombre);
                        return RedirectToAction("Index", "Vendedors");
                    }
                    return View();
                }

            }
            catch (Exception ex)
            {
                ViewBag.mje = "login incorrecto";
                return View();
            }
        }
        // GET: Vendedors
        [Authorize(Roles = "1,2")]
        public IActionResult Logout()
        {

            return View();

        }


        [HttpPost]
        [Authorize(Roles = "1,2")]
        public async Task<IActionResult> Logout(string n)
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

            return RedirectToAction("Login");
        }
    }
}
