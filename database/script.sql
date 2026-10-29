/*
    UTN Hardware — Script de la base de datos (TP Programación 3, UTN FRGP)

    Crea la base UtnHardware desde cero: tablas, procedimientos almacenados y datos de prueba.
    ATENCIÓN: si la base ya existe, la BORRA y la vuelve a crear.

    Diagrama entidad-relación (DER) y su código DBML: https://dbdiagram.io/d/6ae36b49aac33014105af07d
    Si cambia el modelo, actualizar también el diagrama y exportar de nuevo database/DER.png.

    Usuarios de prueba:
        admin    / admin123     (Administrador)
        vendedor / vendedor123  (Vendedor)
*/

USE master;
GO

IF DB_ID('UtnHardware') IS NOT NULL
BEGIN
    ALTER DATABASE UtnHardware SET SINGLE_USER WITH ROLLBACK IMMEDIATE;
    DROP DATABASE UtnHardware;
END
GO

CREATE DATABASE UtnHardware;
GO

USE UtnHardware;
GO

/* =========================================================
   TABLAS
   ========================================================= */

CREATE TABLE Marcas (
    Id          INT IDENTITY(1,1) PRIMARY KEY,
    Descripcion NVARCHAR(50) NOT NULL,
    PaisOrigen  NVARCHAR(50) NULL,
    Activo      BIT NOT NULL DEFAULT 1,
    CONSTRAINT UQ_Marcas_Descripcion UNIQUE (Descripcion)
);

CREATE TABLE Categorias (
    Id          INT IDENTITY(1,1) PRIMARY KEY,
    Descripcion NVARCHAR(50) NOT NULL,
    Activo      BIT NOT NULL DEFAULT 1,
    CONSTRAINT UQ_Categorias_Descripcion UNIQUE (Descripcion)
);

CREATE TABLE Proveedores (
    Id          INT IDENTITY(1,1) PRIMARY KEY,
    Cuit        VARCHAR(11) NOT NULL,
    RazonSocial NVARCHAR(100) NOT NULL,
    Telefono    VARCHAR(30) NULL,
    Email       NVARCHAR(100) NULL,
    Direccion   NVARCHAR(150) NULL,
    Activo      BIT NOT NULL DEFAULT 1,
    CONSTRAINT UQ_Proveedores_Cuit UNIQUE (Cuit)
);

CREATE TABLE Clientes (
    Id         INT IDENTITY(1,1) PRIMARY KEY,
    Documento  VARCHAR(11) NOT NULL,
    Nombre     NVARCHAR(50) NOT NULL,
    Apellido   NVARCHAR(50) NOT NULL,
    Telefono   VARCHAR(30) NULL,
    Email      NVARCHAR(100) NOT NULL,
    Direccion  NVARCHAR(150) NULL,
    FechaAlta  DATE NOT NULL DEFAULT GETDATE(),
    Activo     BIT NOT NULL DEFAULT 1,
    CONSTRAINT UQ_Clientes_Documento UNIQUE (Documento),
    CONSTRAINT UQ_Clientes_Email UNIQUE (Email)
);

CREATE TABLE Productos (
    Id                 INT IDENTITY(1,1) PRIMARY KEY,
    Codigo             VARCHAR(20) NOT NULL,
    Nombre             NVARCHAR(100) NOT NULL,
    Descripcion        NVARCHAR(500) NULL,
    UrlImagen          NVARCHAR(500) NULL,
    IdMarca            INT NOT NULL,
    IdCategoria        INT NOT NULL,
    StockActual        INT NOT NULL DEFAULT 0,
    StockMinimo        INT NOT NULL DEFAULT 0,
    PorcentajeGanancia DECIMAL(5,2) NOT NULL,
    Activo             BIT NOT NULL DEFAULT 1,
    CONSTRAINT UQ_Productos_Codigo UNIQUE (Codigo),
    CONSTRAINT FK_Productos_Marcas FOREIGN KEY (IdMarca) REFERENCES Marcas(Id),
    CONSTRAINT FK_Productos_Categorias FOREIGN KEY (IdCategoria) REFERENCES Categorias(Id),
    CONSTRAINT CK_Productos_StockActual CHECK (StockActual >= 0),
    CONSTRAINT CK_Productos_StockMinimo CHECK (StockMinimo >= 0),
    CONSTRAINT CK_Productos_PorcentajeGanancia CHECK (PorcentajeGanancia >= 0)
);

-- A qué proveedores se les PUEDE comprar cada producto.
CREATE TABLE ProductoProveedor (
    IdProducto  INT NOT NULL,
    IdProveedor INT NOT NULL,
    CONSTRAINT PK_ProductoProveedor PRIMARY KEY (IdProducto, IdProveedor),
    CONSTRAINT FK_ProductoProveedor_Productos FOREIGN KEY (IdProducto) REFERENCES Productos(Id),
    CONSTRAINT FK_ProductoProveedor_Proveedores FOREIGN KEY (IdProveedor) REFERENCES Proveedores(Id)
);

CREATE TABLE Usuarios (
    Id            INT IDENTITY(1,1) PRIMARY KEY,
    NombreUsuario VARCHAR(30) NOT NULL,
    Nombre        NVARCHAR(50) NOT NULL,
    Apellido      NVARCHAR(50) NOT NULL,
    Email         NVARCHAR(100) NOT NULL,
    PasswordHash  CHAR(64) NOT NULL,      -- SHA-256 en hexadecimal (minúsculas)
    Perfil        INT NOT NULL,           -- 1 = Administrador, 2 = Vendedor (enum TipoPerfil)
    Activo        BIT NOT NULL DEFAULT 1,
    CONSTRAINT UQ_Usuarios_NombreUsuario UNIQUE (NombreUsuario),
    CONSTRAINT UQ_Usuarios_Email UNIQUE (Email),
    CONSTRAINT CK_Usuarios_Perfil CHECK (Perfil IN (1, 2))
);

CREATE TABLE Compras (
    Id            INT IDENTITY(1,1) PRIMARY KEY,
    IdProveedor   INT NOT NULL,
    IdUsuario     INT NOT NULL,           -- quién cargó la compra
    Fecha         DATE NOT NULL,          -- fecha de la factura del proveedor
    NumeroFactura VARCHAR(30) NOT NULL,   -- número de la factura del proveedor
    CONSTRAINT FK_Compras_Proveedores FOREIGN KEY (IdProveedor) REFERENCES Proveedores(Id),
    CONSTRAINT FK_Compras_Usuarios FOREIGN KEY (IdUsuario) REFERENCES Usuarios(Id),
    CONSTRAINT UQ_Compras_Proveedor_Factura UNIQUE (IdProveedor, NumeroFactura)
);

CREATE TABLE DetalleCompra (
    Id             INT IDENTITY(1,1) PRIMARY KEY,
    IdCompra       INT NOT NULL,
    IdProducto     INT NOT NULL,
    Cantidad       INT NOT NULL,
    PrecioUnitario DECIMAL(18,2) NOT NULL,  -- costo unitario de esta compra
    CONSTRAINT FK_DetalleCompra_Compras FOREIGN KEY (IdCompra) REFERENCES Compras(Id),
    CONSTRAINT FK_DetalleCompra_Productos FOREIGN KEY (IdProducto) REFERENCES Productos(Id),
    CONSTRAINT CK_DetalleCompra_Cantidad CHECK (Cantidad > 0),
    CONSTRAINT CK_DetalleCompra_PrecioUnitario CHECK (PrecioUnitario > 0)
);

CREATE TABLE Ventas (
    Id            INT IDENTITY(1,1) PRIMARY KEY,
    IdCliente     INT NOT NULL,
    IdUsuario     INT NOT NULL,           -- vendedor que registró la venta
    Fecha         DATETIME NOT NULL DEFAULT GETDATE(),
    NumeroFactura INT NOT NULL,           -- correlativo; se muestra como 0001-00000006
    CONSTRAINT FK_Ventas_Clientes FOREIGN KEY (IdCliente) REFERENCES Clientes(Id),
    CONSTRAINT FK_Ventas_Usuarios FOREIGN KEY (IdUsuario) REFERENCES Usuarios(Id),
    CONSTRAINT UQ_Ventas_NumeroFactura UNIQUE (NumeroFactura)
);

CREATE TABLE DetalleVenta (
    Id             INT IDENTITY(1,1) PRIMARY KEY,
    IdVenta        INT NOT NULL,
    IdProducto     INT NOT NULL,
    Cantidad       INT NOT NULL,
    PrecioUnitario DECIMAL(18,2) NOT NULL,  -- precio de venta del momento
    CONSTRAINT FK_DetalleVenta_Ventas FOREIGN KEY (IdVenta) REFERENCES Ventas(Id),
    CONSTRAINT FK_DetalleVenta_Productos FOREIGN KEY (IdProducto) REFERENCES Productos(Id),
    CONSTRAINT CK_DetalleVenta_Cantidad CHECK (Cantidad > 0),
    CONSTRAINT CK_DetalleVenta_PrecioUnitario CHECK (PrecioUnitario > 0)
);

-- "Líneas de stock": cada compra y cada venta deja un movimiento por producto.
CREATE TABLE MovimientosStock (
    Id         INT IDENTITY(1,1) PRIMARY KEY,
    IdProducto INT NOT NULL,
    Fecha      DATETIME NOT NULL DEFAULT GETDATE(),
    Tipo       INT NOT NULL,              -- 1 = Compra, 2 = Venta (enum TipoMovimiento)
    Cantidad   INT NOT NULL,
    IdCompra   INT NULL,
    IdVenta    INT NULL,
    CONSTRAINT FK_MovimientosStock_Productos FOREIGN KEY (IdProducto) REFERENCES Productos(Id),
    CONSTRAINT FK_MovimientosStock_Compras FOREIGN KEY (IdCompra) REFERENCES Compras(Id),
    CONSTRAINT FK_MovimientosStock_Ventas FOREIGN KEY (IdVenta) REFERENCES Ventas(Id),
    CONSTRAINT CK_MovimientosStock_Tipo CHECK (Tipo IN (1, 2)),
    CONSTRAINT CK_MovimientosStock_Cantidad CHECK (Cantidad > 0)
);
GO

/* =========================================================
   PROCEDIMIENTOS ALMACENADOS
   ========================================================= */

-- Marcas
CREATE PROCEDURE SP_ListarMarcas
    @SoloActivas BIT = 1
AS
BEGIN
    SELECT Id, Descripcion, PaisOrigen, Activo
    FROM Marcas
    WHERE @SoloActivas = 0 OR Activo = 1
    ORDER BY Descripcion;
END
GO

CREATE PROCEDURE SP_AgregarMarca
    @Descripcion NVARCHAR(50),
    @PaisOrigen  NVARCHAR(50)
AS
BEGIN
    INSERT INTO Marcas (Descripcion, PaisOrigen)
    VALUES (@Descripcion, @PaisOrigen);

    SELECT CAST(SCOPE_IDENTITY() AS INT) AS Id;
END
GO

CREATE PROCEDURE SP_ModificarMarca
    @Id          INT,
    @Descripcion NVARCHAR(50),
    @PaisOrigen  NVARCHAR(50)
AS
BEGIN
    UPDATE Marcas
    SET Descripcion = @Descripcion,
        PaisOrigen  = @PaisOrigen
    WHERE Id = @Id;
END
GO

CREATE PROCEDURE SP_BajaMarca
    @Id INT
AS
BEGIN
    UPDATE Marcas SET Activo = 0 WHERE Id = @Id;
END
GO

/* =========================================================
   DATOS DE PRUEBA
   ========================================================= */

INSERT INTO Marcas (Descripcion, PaisOrigen) VALUES
    (N'AMD',      N'Estados Unidos'),   -- 1
    (N'ASUS',     N'Taiwán'),           -- 2
    (N'Corsair',  N'Estados Unidos'),   -- 3
    (N'Intel',    N'Estados Unidos'),   -- 4
    (N'Kingston', N'Estados Unidos'),   -- 5
    (N'MSI',      N'Taiwán'),           -- 6
    (N'Samsung',  N'Corea del Sur');    -- 7

INSERT INTO Categorias (Descripcion) VALUES
    (N'Procesadores'),      -- 1
    (N'Memorias RAM'),      -- 2
    (N'Almacenamiento'),    -- 3
    (N'Motherboards'),      -- 4
    (N'Fuentes'),           -- 5
    (N'Placas de video'),   -- 6
    (N'Gabinetes');         -- 7

-- Proveedores y clientes inventados (los emails usan el dominio reservado .example).
INSERT INTO Proveedores (Cuit, RazonSocial, Telefono, Email, Direccion) VALUES
    ('30712345671', N'Componentes del Norte S.A.', '011 4555-1020', N'ventas@componentesnorte.example',   NULL),  -- 1
    ('30709876543', N'Distribuidora Sur S.R.L.',   '011 4321-8800', N'pedidos@distrisur.example',          NULL),  -- 2
    ('33711223349', N'Mayorista Pacheco S.A.',     '011 4740-3311', N'compras@mayoristapacheco.example',   NULL),  -- 3
    ('30705556662', N'TecnoImport S.A.',           '011 5263-4400', N'comercial@tecnoimport.example',      NULL);  -- 4

INSERT INTO Clientes (Documento, Nombre, Apellido, Telefono, Email, FechaAlta) VALUES
    ('30123456', N'Laura',  N'Fernández', '11 5555-0101', N'laura.fernandez@mail.example', '2026-09-02'),
    ('35987321', N'Martín', N'Sosa',      '11 5555-0102', N'msosa@mail.example',           '2026-09-15'),
    ('28456789', N'Carla',  N'Ríos',      '11 5555-0103', N'carla.rios@mail.example',      '2026-10-03'),
    ('40112233', N'Diego',  N'Acosta',    '11 5555-0104', N'dacosta@mail.example',         '2026-10-20'),
    ('33221100', N'Sofía',  N'Medina',    '11 5555-0105', N'sofia.medina@mail.example',    '2026-10-27');

-- Contraseñas: SHA-256 de 'admin123' y de 'vendedor123'.
INSERT INTO Usuarios (NombreUsuario, Nombre, Apellido, Email, PasswordHash, Perfil) VALUES
    ('admin',    N'Administrador', N'General', N'admin@utnhardware.example',
     '240be518fabd2724ddb6f04eeb1da5967448d7e831c08c8fa822809f74c720a9', 1),   -- 1
    ('vendedor', N'Juan',          N'Pérez',   N'vendedor@utnhardware.example',
     '56976bf24998ca63e35fe4f1e2469b5751d1856003e8d16fef0aafef496ed044', 2);   -- 2

-- StockActual coincide con las compras de prueba de más abajo.
-- El producto 7 no tiene compras: no tiene precio y no se puede vender.
INSERT INTO Productos (Codigo, Nombre, IdMarca, IdCategoria, StockActual, StockMinimo, PorcentajeGanancia) VALUES
    ('AMD-R5-7600',   N'Ryzen 5 7600',               1, 1, 14,  5, 25),   -- 1
    ('INT-I5-14400F', N'Core i5-14400F',             4, 1,  3,  5, 25),   -- 2
    ('KIN-FB-16D5',   N'Fury Beast 16 GB DDR5',      5, 2, 24, 10, 30),   -- 3
    ('SAM-990E-1TB',  N'990 EVO 1 TB',               7, 3,  8,  6, 28),   -- 4
    ('ASU-B650M-A',   N'Prime B650M-A',              2, 4,  3,  3, 22),   -- 5
    ('COR-RM750E',    N'RM750e',                     3, 5,  4,  4, 20),   -- 6
    ('MSI-4060-V2X',  N'GeForce RTX 4060 Ventus 2X', 6, 6,  0,  2, 18);   -- 7

INSERT INTO ProductoProveedor (IdProducto, IdProveedor) VALUES
    (1, 1), (1, 3),
    (2, 1), (2, 3),
    (3, 1), (3, 2),
    (4, 1), (4, 2),
    (5, 2), (5, 4),
    (6, 1),
    (7, 4);

-- Compras de prueba (cargadas por el usuario admin).
-- El Ryzen 5 7600 tiene dos compras con distinto costo: el precio de venta usa la más reciente.
INSERT INTO Compras (IdProveedor, IdUsuario, Fecha, NumeroFactura) VALUES
    (3, 1, '2026-09-20', 'A-0002-00000850'),   -- 1
    (1, 1, '2026-10-01', 'A-0003-00012801'),   -- 2
    (2, 1, '2026-10-05', 'B-0001-00004512'),   -- 3
    (3, 1, '2026-10-10', 'A-0002-00000877');   -- 4

INSERT INTO DetalleCompra (IdCompra, IdProducto, Cantidad, PrecioUnitario) VALUES
    (1, 1,  2, 220000.00),
    (2, 1, 12, 230000.00),
    (2, 3, 24,  60000.00),
    (2, 6,  4, 110000.00),
    (3, 4,  8,  90000.00),
    (3, 5,  3, 170000.00),
    (4, 2,  3, 211000.00);

-- Un movimiento de tipo Compra (1) por cada línea de compra.
INSERT INTO MovimientosStock (IdProducto, Fecha, Tipo, Cantidad, IdCompra) VALUES
    (1, '2026-09-20', 1,  2, 1),
    (1, '2026-10-01', 1, 12, 2),
    (3, '2026-10-01', 1, 24, 2),
    (6, '2026-10-01', 1,  4, 2),
    (4, '2026-10-05', 1,  8, 3),
    (5, '2026-10-05', 1,  3, 3),
    (2, '2026-10-10', 1,  3, 4);
GO
