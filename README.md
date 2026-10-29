# tp-cuatrimestral-equipo-v
TP Cuatrimestral – Sistema de compras y ventas (ASP.NET Web Forms): Comercio.

**UTN Hardware**: aplicación web para administrar las compras y ventas de una tienda de componentes de PC.
Programación 3 — UTN FRGP. Equipo: Mateo Brena, Tomás Mosgidis e Ignacio Federico Rodriguez.

## Cómo correrlo

1. Ejecutar `database/script.sql` en SQL Server (crea la base `UtnHardware` con datos de prueba).
   Si la base ya existe, el script la borra y la vuelve a crear.
2. Abrir `tp-cuatrimestral-equipo-v.slnx` en Visual Studio, establecer `TPCuatrimestralEquipoV` como proyecto de inicio y ejecutar.

Por defecto la aplicación se conecta a **SQL Server Express** (`.\SQLEXPRESS`) con autenticación de Windows.

Usuarios de prueba:

| Usuario | Contraseña | Perfil |
|---|---|---|
| `admin` | `admin123` | Administrador |
| `vendedor` | `vendedor123` | Vendedor |

### Usar otro SQL Server (por ejemplo, en Docker)

1. En la carpeta `TPCuatrimestralEquipoV`, copiar `appSettings.local.example.config` como `appSettings.local.config`.
2. Poner ahí la cadena de conexión propia.

`appSettings.local.config` está en el `.gitignore` y pisa el valor del `Web.config`. Si no existe, se usa el del `Web.config` sin error.

## Estructura

| Proyecto | Contenido |
|---|---|
| `Dominio` | Clases del modelo (Producto, Marca, Venta, etc.) |
| `Negocio` | Reglas de negocio y acceso a datos (`AccesoDatos`, una clase `...Negocio` por entidad) |
| `TPCuatrimestralEquipoV` | Aplicación web: páginas `.aspx` |
| `database` | Script de la base de datos y diagrama entidad-relación (`DER.png`) |

Requisitos: Visual Studio 2022 actualizado (o 2026) con la carga de trabajo de ASP.NET, .NET Framework 4.8 y SQL Server.
