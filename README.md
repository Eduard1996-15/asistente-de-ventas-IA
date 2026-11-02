# Asistente de Ventas IA

Sistema de gestión de ventas para una tienda de indumentaria, con clasificación de
prendas por imagen y recomendaciones personalizadas usando **ML.NET**.

Proyecto **ASP.NET Core MVC (.NET 6)** con autenticación por roles, catálogo de
stock, ventas y dos modelos de visión que etiquetan automáticamente una foto de
producto (tipo de prenda y textura del material).

## Funcionalidades

- **Login con roles** — administrador y vendedor, con cookies de autenticación y
  sesión de 6 horas.
- **Catálogo de stock** — alta, edición, baja y detalle de productos con código de
  color, modelo y stock ampliado.
- **Búsqueda de productos** — búsqueda por texto y carga de imágenes.
- **Clasificación de imágenes con ML.NET**
  - `ClasificarImgTipoPrenda` → identifica el tipo de prenda (camisa, pantalón, etc.).
  - `ClasificarImgTextura` → identifica la textura del material.
  - Ambos modelos se entrenan con *Model Builder* y se consumen desde el
    controlador de vendedores.
- **Recomendaciones** — motor de recomendación de ML.NET que sugiere productos a
  partir del historial de compras del cliente, con *fallback* a los productos más
  vendidos cuando no hay datos suficientes.
- **Encuestas post-venta por correo** — envío automático de un mail con link a la
  encuesta a los clientes que compraron en las últimas 24 horas (MailKit).

## Stack

| Capa | Tecnología |
|---|---|
| Framework | ASP.NET Core MVC 6.0 |
| Acceso a datos | Entity Framework Core 6 + SQL Server |
| Machine Learning | ML.NET 2.0 (`Microsoft.ML`, `.Vision`, `.Recommender`) |
| Correo | MailKit / MimeKit |
| Frontend | Razor Views + Bootstrap 5 |
| Imágenes | AForge / AForge.Imaging |

## Estructura

```
Controllers/            Login, stock, clientes, administración y envío de mails
Models/                 Entidades del dominio (Stock, Client, Facloc, Usuario...)
data/                   DbContext de EF Core
Views/                  Vistas Razor
wwwroot/                Estáticos (Bootstrap, CSS, JS)
*.mbconfig              Configuración de cada modelo en Model Builder
*.training.cs           Pipeline de entrenamiento generado por Model Builder
*.consumption.cs        Carga y predicción del modelo entrenado
Recomendaciones.zip     Modelo de recomendación entrenado (~1,2 MB, sí versionado)
```

## Puesta en marcha

### 1. Requisitos

- .NET SDK 6.0 o superior
- SQL Server (local, Express o Azure SQL)

### 2. Restaurar dependencias

```bash
dotnet restore
```

### 3. Configurar la cadena de conexión

No hay credenciales en el repositorio. Definí `ConnectionStrings:conexion` con
variables de entorno o con *user-secrets*:

```bash
dotnet user-secrets init
dotnet user-secrets set "ConnectionStrings:conexion" "Server=localhost;Database=asistenteventas;Trusted_Connection=True;TrustServerCertificate=True"
```

o bien:

```bash
export ConnectionStrings__conexion="Server=localhost;Database=asistenteventas;User Id=sa;Password=tu_password;TrustServerCertificate=True"
```

### 4. Configurar el correo (opcional)

El envío de encuestas necesita un servidor SMTP:

```bash
dotnet user-secrets set "Smtp:Host" "smtp.example.com"
dotnet user-secrets set "Smtp:Port" "587"
dotnet user-secrets set "Smtp:User" "no-reply@example.com"
dotnet user-secrets set "Smtp:Password" "tu_password"
```

Si no está configurado, la pantalla de administración muestra un aviso y no
intenta enviar nada.

### 5. Ejecutar

```bash
dotnet run
```

La aplicación queda en `http://localhost:5214` y arranca en la pantalla de login.

## Modelos de ML.NET

`Recomendaciones.zip` sí está versionado porque pesa ~1,2 MB. Los dos modelos de
visión **no** se versionan: cada `.mlnet` pesa unos 91 MB y supera lo razonable
para un repositorio.

Para regenerarlos:

1. Abrí el proyecto en Visual Studio.
2. Hacé doble clic en `ClasificarImgTipoPrenda.mbconfig` o
   `ClasificarImgTextura.mbconfig` (abre *Model Builder*).
3. Entrená el modelo y guardá el `.mlnet` en la raíz del proyecto. El
   `.csproj` ya lo copia al directorio de salida (`CopyToOutputDirectory`).

Sin esos archivos, el resto de la aplicación funciona normalmente; sólo fallan los
endpoints de clasificación por imagen, que cargan el modelo de forma perezosa.

## Notas de seguridad

- Este proyecto nació como trabajo académico. El manejo de contraseñas de usuario
  es básico (comparación directa en la base) y **no** está pensado para producción:
  para un despliegue real habría que usar ASP.NET Core Identity o un hash con
  `PasswordHasher`.
- No se versionan cadenas de conexión, credenciales SMTP, ni perfiles de
  publicación de Azure.

## Licencia

Uso educativo y de portfolio.
