using asistenteventas.Data;
using asistenteventas.Models;
using Asistenteventas;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Text;
using System.Drawing;
using System.Drawing.Imaging;
using AForge.Imaging.Filters;
using Microsoft.AspNetCore.Authorization;


namespace asistenteventas.Controllers
{
   
    public class VendedorsController : Controller
    {
        private readonly ASISTENTE_DE_VENTASContext _context;

#pragma warning disable CS8618 // El elemento campo "_stocks" que no acepta valores NULL debe contener un valor distinto de NULL al salir del constructor. Considere la posibilidad de declarar el elemento campo como que admite un valor NULL.
        public VendedorsController(ASISTENTE_DE_VENTASContext context)
#pragma warning restore CS8618 // El elemento campo "_stocks" que no acepta valores NULL debe contener un valor distinto de NULL al salir del constructor. Considere la posibilidad de declarar el elemento campo como que admite un valor NULL.
        {
            _context = context;
        }

        private IEnumerable<Stock> _stocks;
        //busqueda
        [Authorize(Roles = "1,2")]
        public ActionResult Busqueda()
        {
             
                return View();

        }
        [Authorize(Roles = "1,2")]
        [HttpPost]
        public ActionResult Busqueda(string imagen)
        {
            _stocks = null;
            try
            {
                if (string.IsNullOrEmpty(imagen))
                {
                    ViewBag.mje = "No se ha proporcionado ninguna imagen.";
                    return View();
                }

                byte[] bytes ;
                try
                {
                    bytes = Convert.FromBase64String(imagen);
                }
                catch (Exception ex)
                {
                    ViewBag.mje = "El formato de la imagen proporcionada no es correcto.";
                    return View();
                }
              
                string[] busqueda = RealizarBusqueda(bytes);

                IEnumerable<StockAmpliado> stocks = ObtenerStocks(busqueda);

                if (stocks != null && stocks.Any())
                {
                    ViewBag.mje = "Modelos encontrados de: " + busqueda;
                    return View("Stock", stocks);
                }
                else
                {
                    ViewBag.mje = "No se encontraron modelos con esa foto.";
                    return View();
                }
            }
            catch (Exception ex)
            {
                ViewBag.mje = "Error al buscar por foto: " + ex.Message;
                return View();
            }
        }
        [Authorize(Roles = "1,2")]
        public ActionResult BusquedaSubir()
        {
            
                return View();

        }
        [Authorize(Roles = "1,2")]
        [HttpPost]
        public ActionResult BusquedaSubir(IFormFile imagenFile)
        {
            _stocks = null;
            try
            {
                if (imagenFile == null || imagenFile.Length == 0)
                {
                    ViewBag.mje = "No se ha proporcionado ninguna imagen.";
                    return View();
                }

                using (var memoryStream = new MemoryStream())
                {
                    imagenFile.CopyTo(memoryStream);
                    byte[] bytes = memoryStream.ToArray();

                    string[] busqueda = RealizarBusqueda(bytes);

                    IEnumerable<StockAmpliado> stocks = ObtenerStocks(busqueda);

                    if (stocks != null && stocks.Any())
                    {
                        ViewBag.mje = "Modelos encontrados de: " + busqueda;
                        return View("Stock", stocks);
                    }
                    else
                    {
                        ViewBag.mje = "No se encontraron modelos con esa foto.";
                        return View();
                    }
                }
            }
            catch (Exception ex)
            {
                ViewBag.mje = "Error al buscar por foto: " + ex.Message;
                return View();
            }
        }
        [Authorize(Roles = "1,2")]
        private string[] RealizarBusqueda(byte[] imagenBytes)
        {
            string tipoPrenda = CargarTipoPrenda(imagenBytes);
            string color = DetectarColor(imagenBytes);
            string textura = CargarTextura(imagenBytes);
            string suma = $"{tipoPrenda}_{color}_{textura}";
            string[] ret = suma.Split('_', StringSplitOptions.RemoveEmptyEntries);
            return ret;
        }

        [Authorize(Roles = "1,2")]

        private IEnumerable<StockAmpliado> ObtenerStocks(string[] busqueda)
        {
            IEnumerable<StockAmpliado> stocks = _context.Stocks
                .Select(s => new StockAmpliado
                {
                    Stock = s,
                    Color = _context.CodigosColor
                        .Where(cc => cc.CodCol == s.CodColPrv)
                        .Select(cc => cc.Nombre)
                        .FirstOrDefault() ?? ""
                })
                .AsEnumerable();

            stocks = FiltrarYOrdenarStocks(stocks, busqueda);
            return AsignarImagenesAStocks(stocks);
        }
		private IEnumerable<StockAmpliado> AsignarImagenesAStocks(IEnumerable<StockAmpliado> stocks)
		{
			var stocksConImagenes = stocks.Select(stock =>
			{
				var imagen = ObtenerImagen(stock.Stock.CodArt);
				stock.Imagen = imagen;
				return stock;
			});

			return stocksConImagenes;
		}

		private byte[] ObtenerImagen(string codArt)
		{
			var imagen = _context.Images
				.Where(i => i.CodArt == codArt)
				.Select(i => i.Imagen)
				.FirstOrDefault();

			return imagen;
		}


		[Authorize(Roles = "1,2")]
        private string CargarTipoPrenda(byte[] bytes)
        {
            ClasificarImgTipoPrenda.ModelInput sampleData = new ClasificarImgTipoPrenda.ModelInput()
            {
                ImageSource = bytes,
            };

            return ClasificarImgTipoPrenda.Predict(sampleData).PredictedLabel;
        }

        [Authorize(Roles = "1,2")]
        private string CargarTextura(byte[] bytes)
        {
            ClasificarImgTextura.ModelInput sampleData2 = new ClasificarImgTextura.ModelInput()
            {
                ImageSource = bytes,
            };

            return ClasificarImgTextura.Predict(sampleData2).PredictedLabel;
        }

        [Authorize(Roles = "1,2")]
        private IEnumerable<StockAmpliado> FiltrarYOrdenarStocks(IEnumerable<StockAmpliado> stocks, string[] palabras)
        {
            return stocks
                .Where(m => CalcularPuntaje(m, palabras) > 0)
                .OrderByDescending(m => CalcularPuntaje(m, palabras))
                .Take(12)
                .ToList();
        }

        [Authorize(Roles = "1,2")]
        public ActionResult Stock()
        {
            
                if (_stocks != null && _stocks.Any())
                {
                    ViewBag.mje = "Modelos encontrados:";
                    return View(_stocks);
                }
                else
                {
                    ViewBag.mje = "No se encontraron modelos.";
                    return View();
                }
           

        }

        private string DetectarColor(byte[] archivo)
        {
            if (archivo == null || archivo.Length == 0)
            {
                return "No se seleccionó ninguna imagen.";
            }

            try
            {
                using (MemoryStream stream = new MemoryStream(archivo))
                using (Bitmap imagenOriginal = new Bitmap(stream))
                {
                    // Redimensionar la imagen a un tamaño más pequeño (por ejemplo, 100x100 píxeles)
                    int nuevoAncho = 100;
                    int nuevoAlto = 100;
                    Bitmap imagenRedimensionada = RedimensionarImagen(imagenOriginal, nuevoAncho, nuevoAlto);

                    ResizeBicubic filter = new ResizeBicubic(100, 100);

                    Color color = ObtenerColorPredominante(imagenOriginal, 30);
                    string nombreColor = GetNombreColor(color);

                    return nombreColor;
                }
            }
            catch (Exception ex)
            {
                return "Error al procesar la imagen: " + ex.Message;
            }
        }
        private Bitmap RedimensionarImagen(Bitmap imagenOriginal, int nuevoAncho, int nuevoAlto)
        {
            Bitmap imagenRedimensionada = new Bitmap(nuevoAncho, nuevoAlto);

            using (Graphics graficos = Graphics.FromImage(imagenRedimensionada))
            {
                // Configurar opciones de calidad de interpolación
                graficos.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
                graficos.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
                graficos.PixelOffsetMode = System.Drawing.Drawing2D.PixelOffsetMode.HighQuality;
                graficos.CompositingQuality = System.Drawing.Drawing2D.CompositingQuality.HighQuality;

                // Redimensionar la imagen original al nuevo tamaño
                graficos.DrawImage(imagenOriginal, new Rectangle(0, 0, nuevoAncho, nuevoAlto));
            }

            return imagenRedimensionada;
        }

        private Color ObtenerColorPredominante(Bitmap imagen, int radio)
        {
            int centroX = imagen.Width / 2;
            int centroY = imagen.Height / 2;

            int startX = Math.Max(0, centroX - radio);
            int startY = Math.Max(0, centroY - radio);
            int endX = Math.Min(imagen.Width - 1, centroX + radio);
            int endY = Math.Min(imagen.Height - 1, centroY + radio);

            Dictionary<Color, int> colorCounts = new Dictionary<Color, int>();

            for (int y = startY; y <= endY; y++)
            {
                for (int x = startX; x <= endX; x++)
                {
                    Color pixelColor = imagen.GetPixel(x, y);

                    if (colorCounts.ContainsKey(pixelColor))
                    {
                        colorCounts[pixelColor]++;
                    }
                    else
                    {
                        colorCounts.Add(pixelColor, 1);
                    }
                }
            }

            KeyValuePair<Color, int> maxCountColor = colorCounts.OrderByDescending(c => c.Value).First();

            // Crear una nueva instancia de Color solo con el valor RGB
            Color rgbColor = Color.FromArgb(maxCountColor.Key.R, maxCountColor.Key.G, maxCountColor.Key.B);

            return rgbColor;
        }

        private Bitmap ElegirCuadrante(Bitmap imagen, PixelFormat formato)
        {
            using (Bitmap imagenClonada = imagen.Clone(new Rectangle(5, 5, imagen.Width, imagen.Height), formato))
            {
                return new Bitmap(imagenClonada);
            }
        }

        private string GetNombreColor(Color colorOriginal)

        {
            Dictionary<Color, string> colorTable = new Dictionary<Color, string>()

            {
            { Color.FromArgb(255, 255, 255, 255), "Blanco" },
            { Color.FromArgb(255, 169, 169, 169), "Gris Plomo" },
            { Color.FromArgb(255, 255, 206, 0), "Amarillo" },
            { Color.FromArgb(255, 255, 235, 205), "Blanco Off White" },
            { Color.FromArgb(255, 237, 237, 237), "Crudo" },
            { Color.FromArgb(255, 0, 191, 255), "Azul Celeste" },
            { Color.FromArgb(255, 0, 255, 255), "Azul Aqua" },
            { Color.FromArgb(255, 255, 255, 240), "Blanco Marfil" },
            { Color.FromArgb(255, 0, 0, 0), "Negro" },
            { Color.FromArgb(255, 64, 224, 208), "Turquesa" },
            { Color.FromArgb(255, 85, 107, 47), "Verde Olivo" },
            { Color.FromArgb(255, 184, 115, 51), "Marron Cobre" },
            { Color.FromArgb(255, 192, 192, 192), "Gris Plata" },
            { Color.FromArgb(255, 255, 0, 255), "Magenta" },
            { Color.FromArgb(255, 123, 17, 19), "Rojo Borgoña" },
            { Color.FromArgb(255, 0, 0, 255), "Azul" },
            { Color.FromArgb(255, 245, 245, 220), "Beige" },
            { Color.FromArgb(255, 193, 154, 107), "Marron Camel" },
            { Color.FromArgb(255, 210, 105, 30), "Marron Chocolate" },
            { Color.FromArgb(255, 238, 130, 238), "Violeta" },
            { Color.FromArgb(255, 128, 128, 0), "Vison" },
            { Color.FromArgb(255, 255, 0, 0), "Rojo" },
            { Color.FromArgb(255, 200, 162, 200), "Violeta Lila" },
            { Color.FromArgb(255, 255, 231, 186), "Blanco Arena" },
            { Color.FromArgb(255, 240, 240, 0), "Amarillo" },
            { Color.FromArgb(255, 218, 165, 32), "Amarillo Dorado" },
            { Color.FromArgb(255, 75, 83, 32), "Verde Militar" },
            { Color.FromArgb(255, 195, 176, 145), "Verde Kaki" },
            { Color.FromArgb(255, 0, 128, 0), "Verde" },
            { Color.FromArgb(255, 255, 215, 0), "Oro" },
            { Color.FromArgb(255, 0, 128, 128), "Verde Petroleo" },
            { Color.FromArgb(255, 255, 165, 0), "Naranja" },
            { Color.FromArgb(255, 0, 0, 128), "Azul Marino" },
            { Color.FromArgb(255, 128, 0, 128), "Purpura" },
            { Color.FromArgb(255, 139, 69, 19), "Marron" },
            { Color.FromArgb(255, 255, 255, 0), "Amarillo Lima" },
            { Color.FromArgb(255, 255, 63, 150), "Fucxia" }
        };
            int minDistancia = int.MaxValue;
            string colorCercano = "Desconocido";
            string color = VerificarParticulares(colorOriginal);

            if (color != "") return color;

            foreach (var valor in colorTable)
            {
                if (Color.Equals(colorOriginal, valor.Key))
                {
                    colorCercano = valor.Value;
                    break;
                }
                int distancia = CalcularDistancia(colorOriginal, valor.Key);
                if (distancia < minDistancia)
                {
                    minDistancia = distancia;
                    colorCercano = valor.Value;
                    if (minDistancia > 30)
                    {
                        colorCercano = colorCercano.Split(' ')[0];
                    }
                }
            }
            return colorCercano;
        }

        private string VerificarParticulares(Color color)
        {
            if (color.R > 50 && color.G > 50 && color.B > 50 && (Math.Abs(color.R - color.G) <= 10) && (Math.Abs(color.B - color.G) < 10) && (Math.Abs(color.R - color.B) <= 10))
                return "Gris";

            if (color.G - color.R > 30 && color.G - color.B > 20)
                return "Verde";

            if (color.R > 180 && color.R - color.B > 20 && color.R - color.G > 20)
                return "Rosa";

            else
                return "";
        }

        private int CalcularDistancia(Color color1, Color color2)
        {
            return Math.Abs(color1.R - color2.R) +
                   Math.Abs(color1.G - color2.G) +
                   Math.Abs(color1.B - color2.B);
        }

        [Authorize(Roles = "2,2")]
        public ActionResult Index()
        {
                return View();
        }
        [Authorize(Roles = "2,2")]
        [HttpPost]
        public ActionResult Index(string txtbuscar)
        {

            try
            {
#pragma warning disable CS8602 // Desreferencia de una referencia posiblemente NULL.
                var cliente = _context.Clients.FirstOrDefault(x => x.CodCli.ToString().Contains(txtbuscar));
#pragma warning restore CS8602 // Desreferencia de una referencia posiblemente NULL.
                if (cliente != null)
                {
                    // ViewBag.mod= modelos;
                    ViewBag.mje = " encontrado : ";
                    ViewBag.cli = cliente;
                    return View(cliente);
                }
                else
                {
                    ViewBag.mje = " No se encontro con esa cedula ";
                    return View();
                }

            }
            catch (Exception ex)
            {
                ViewBag.mje = "No se encontro con ese texto ";
                return View();
            }
        }
        [Authorize(Roles = "1,1")]
        public async Task<IActionResult> Details(int id)
        {

            if (id.ToString() == null || _context.Usuarios == null)
            {
                return NotFound();
            }

            var vendedores = await _context.Usuarios
                .FirstOrDefaultAsync(m => m.id == id);
            if (vendedores == null)
            {
                return NotFound();
            }

            return View(vendedores);
        }

        // GET: Administradors1/Create
        [Authorize(Roles = "1,1")]
        public IActionResult Create()
        {
           
                return View();

           

        }

        // POST: Administradors1/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize(Roles = "1,1")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("passsword,nombre")] Usuario vendedor)
        {
            if (ModelState.IsValid)
            {
                vendedor.rol = 2;
                _context.Add(vendedor);
                await _context.SaveChangesAsync();
                var ven = _context.Usuarios.Where(u => u.rol == 2);
                
                return RedirectToAction("Index", "Administradors", new { Usuario = vendedor });
            }

            return View(vendedor);
        }

        // GET: Administradors/Edit/5
        [Authorize(Roles = "1,1")]
        public async Task<IActionResult> Edit(int? id)
        {
           
            if (id.ToString() == null || _context.Usuarios == null)
            {
                return NotFound();
            }

            var vendedor = await _context.Usuarios.FindAsync(id);
            if (vendedor == null)
            {
                return NotFound();
            }

                return View(vendedor);

            

        }

        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize(Roles = "1,1")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("id,passsword,nombre")] Usuario vendedor)
        {
            if (id != vendedor.id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    vendedor.rol = 2;
                    _context.Update(vendedor);
                    await _context.SaveChangesAsync();

                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!VendedorsExists(vendedor.id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction("Index", "Administradors");
            }
            return View();
        }

        // GET: Administradors/Delete/5
        [Authorize(Roles = "1,1")]
        public async Task<IActionResult> Delete(int id)
        {
          
                
            if (id.ToString() == null || _context.Usuarios == null)
            {
                return NotFound();
            }

            var vendedor = await _context.Usuarios

                .FirstOrDefaultAsync(m => m.id == id);
            if (vendedor == null)
            {
                return NotFound();
            }

            return View(vendedor);
        }
        [Authorize(Roles = "1,1")]
        // POST: Administradors/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Usuarios == null)
            {
                return Problem("Entity set 'ASISTENTE_DE_VENTASContext.Vendedors'  is null.");
            }
            var vendedor = await _context.Usuarios.FindAsync(id);
            if (vendedor != null)
            {
                _context.Usuarios.Remove(vendedor);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction("Index","Administradors");
        }
        [Authorize(Roles = "1,1")]
        private bool VendedorsExists(int id)
        {
            return (_context.Usuarios?.Any(e => e.id == id)).GetValueOrDefault();
        }

        [Authorize(Roles = "1,2")]
        public ActionResult BuscarporTexto()
        {
            
            return View();
           
        }
        [Authorize(Roles = "1,2")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult BuscarporTexto(string descripcion)
        {
            try
            {
                string[] palabras = descripcion.Split(' ', StringSplitOptions.RemoveEmptyEntries);

                for (int i = 0; i < palabras.Length; i++)
                {
                    if (!string.IsNullOrWhiteSpace(palabras[i]))
                    {
                        palabras[i] = palabras[i].Substring(0, 1).ToUpper() + palabras[i].Substring(1).ToLower();
                    }
                }

                IEnumerable<StockAmpliado> stocks = ObtenerStocks(palabras);

                if (stocks.Any())
                {
                    ViewBag.mje = "Modelos encontrados:";
                    return View(stocks);
                }

                ViewBag.mje = "No se encontraron modelos con esa descripción.";
                return View();
            }
            catch (Exception ex)
            {
                ViewBag.mje = "Error al buscar por texto: " + ex.Message;
                return View();
            }
        }


        private int CalcularPuntaje(StockAmpliado model, string[] palabras)
        {
            int score = 0;

            string? color = model.Color?.ToLower();
            string? desArt = model.Stock?.DesArt?.ToLower();
            string? desDetArt = model.Stock?.DesDetArt?.ToLower();

            foreach (var palabra in palabras)
            {
                if (!string.IsNullOrEmpty(color) && color.Contains(palabra.ToLower()))
                {
                    score++;
                }

                if (!string.IsNullOrEmpty(desArt) && desArt.Contains(palabra.ToLower()))
                {
                    if(desArt!= "Torso"|| desArt != "Cuerpo" || desArt != "Piernas")
                    {
                        score++;
                    }
                    score++;
                }

                if (!string.IsNullOrEmpty(desDetArt) && desDetArt.Contains(palabra.ToLower()))
                {
                    score++;
                }
            }

            return score;
        }
        [Authorize(Roles = "1,2")]
        public IActionResult RecomendacionesIAAsync(int? id)
        {
            try
            {
                if (id == null)
                {
                    ViewBag.mje = " error al seleccionar usuario";
                    return View();
                }
                // buscar el usuario con ese id
                var usu = _context.Clients.FirstOrDefault(x => x.CodCli == id);

                if (usu != null)
                {
                    //busco las compras del usuario de la tabla datoscompras
                    var compras = _context.DatosCompras.Where(d => d.usuario == usu.CodCli).ToList();

                    List<Stock> stocks = _context.Stocks.ToList();
                    List<DatosCompras> datoscompras = _context.DatosCompras
                .Where(dc => dc.producto != null) // Exclude rows with null 'producto'
                .ToList();


                    //guardar las descripciones de la compras 

                    var descripciones = _context.DatosCompras
                    .Where(d => d.usuario == usu.CodCli)
                    .Join(
                        _context.Stocks,
                        compra => compra.producto,
                        stock => stock.CodArt,
                        (compra, stock) => stock.DesDetArt
                    )
                    .Where(descripcion => !string.IsNullOrEmpty(descripcion))
                    .ToList();



                    if (descripciones.Count < 1)
                    {
                        ViewBag.mje = "No tiene compras suficientes para tener recomendaciones";
                        return View();
                    }
                    else
                    {
                        //separo por palabras 
                        var palabras = descripciones
                            .SelectMany(des => des.Split(" "))
                            .Where(palabra => !string.IsNullOrEmpty(palabra))
                            .ToList();
                        //productos 
                        // Obtener los códigos de artículo de la lista de productos
                        var codigosArticulo = compras.Select(p => p.producto).ToList();

                        var prods = stocks
                         .Where(stock => palabras.Any(palabra => stock.DesDetArt.Contains(palabra)))
                         .Where(stock => !codigosArticulo.Contains(stock.CodArt))
                         .Distinct()
                         .Take(50)
                         .ToList();

                        List<DatosCompras> d = prods
                        .SelectMany(item => datoscompras.Where(dc => item.CodArt.Contains(dc.producto.Trim())).Distinct())
                        .ToList();

                        var listaSinRepetidos = d
                        .GroupBy(elem => elem.producto)
                        .Select(grp => grp.First())
                        .ToList();

                        var recomenda = listaSinRepetidos
                        .Select(item => new Recomendaciones.ModelInput()
                        {
                            Col0 =item.Id,
                            Col1 = (float)id,
                            Col2 = item.producto.Trim(),
                        })
                        .ToList();

                        // Predecir las salidas para los mejores elementos
                        var resultados = recomenda
                            .Select(datosMuestra => Recomendaciones.Predict(datosMuestra))
                            .ToList();

                        if (resultados.All(r => r.Score.Equals(float.NaN)))
                        {
                            var productos = datoscompras
                             .GroupBy(dc => dc.producto)
                             .Select(g => new { Producto = g.Key, TotalCompras = g.Count() })
                             .OrderByDescending(p => p.TotalCompras)
                             .Take(10)
                             .Select(p => p.Producto)
                            .ToList();

                            var st = BuscarStocks(productos, stocks);
                            var rec = ObtenerRecomendaciones(st);
                            ViewBag.mje = "No hay recomendaciones para este usuario, pero aqui le muestro los productos mas comprados";
                            return View(rec);
                        }

                        // Ordenar la lista por puntaje en orden descendente
                        resultados.Sort((a, b) => b.Score.CompareTo(a.Score));

                        // Tomar los 10 mejores elementos
                        var mejoresItems2 = resultados
                            .Take(10)
                            .ToList();

                        List<Stock> s = mejoresItems2
                        .SelectMany(item => stocks.Where(stock => stock.CodArt.Contains(item.Col2.ToString().Trim())))
                        .Distinct()
                        .ToList();


                        s = s.Distinct().ToList();
                        var recomendaciones = ObtenerRecomendaciones(s);

                        return View(recomendaciones);
                    }

                }
                else
                {
                    //no se encontro usuario
                    ViewBag.mje = "Usuario no encontrado";
                    return View();
                }
            }catch (Exception ex)
            {
                ViewBag.mje = "Ha ocurrido un error. Por favor, inténtalo de nuevo más tarde.";
                return  View();
            }

            

        }

         List<StockAmpliado> ObtenerRecomendaciones(List<Stock> s)
        {
            List<StockAmpliado> recomendaciones = new List<StockAmpliado>();

            foreach (var st in s)
            {
                byte[] imagenBytes = ObtenerImagen(st.CodArt);
                string color = _context.CodigosColor
                    .Where(cc => cc.CodCol == st.CodColPrv)
                    .Select(cc => cc.Nombre)
                    .FirstOrDefault() ?? "";

                recomendaciones.Add(new StockAmpliado
                {
                    Stock = st,
                    Color = color,
                    Imagen = imagenBytes
                });
            }

            return recomendaciones;
        }

        private List<Stock> BuscarStocks(List<string> codArticulos, List<Stock> stockss)
        {
            return stockss
                .Where(stock => codArticulos.Any(item => stock.CodArt.Contains(item.Trim())))
                .Distinct()
                .ToList();
        }
    }


}

