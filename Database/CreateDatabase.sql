-- Crear la base de datos
CREATE DATABASE GestorInventarios;
GO

USE GestorInventarios;
GO

-- Crear tabla de Departamentos
CREATE TABLE Departamentos (
    DepartamentoId INT IDENTITY(1,1) PRIMARY KEY,
    Nombre NVARCHAR(100) NOT NULL,
    Descripcion NVARCHAR(500),
    FechaCreacion DATETIME DEFAULT GETDATE(),
    Activo BIT DEFAULT 1
);
GO

-- Crear tabla de Equipos
CREATE TABLE Equipos (
    EquipoId INT IDENTITY(1,1) PRIMARY KEY,
    Nombre NVARCHAR(100) NOT NULL,
    Tipo NVARCHAR(50) NOT NULL,
    Serial NVARCHAR(100) NOT NULL UNIQUE,
    DepartamentoId INT NOT NULL,
    FechaIngreso DATETIME NOT NULL,
    Activo BIT DEFAULT 1,
    UsuarioCargo NVARCHAR(100) NOT NULL,
    FechaCreacion DATETIME DEFAULT GETDATE(),
    FOREIGN KEY (DepartamentoId) REFERENCES Departamentos(DepartamentoId)
);
GO

-- Insertar datos de ejemplo para Departamentos
INSERT INTO Departamentos (Nombre, Descripcion) VALUES
('Sistemas', 'Departamento de Tecnologías de la Información'),
('Recursos Humanos', 'Gestión de personal'),
('Contabilidad', 'Gestión financiera'),
('Ventas', 'Departamento comercial');
GO

-- Insertar datos de ejemplo para Equipos
INSERT INTO Equipos (Nombre, Tipo, Serial, DepartamentoId, FechaIngreso, UsuarioCargo) VALUES
('PC-001', 'PC', 'PC2024001', 1, GETDATE(), 'Juan Pérez'),
('Laptop-001', 'Laptop', 'LT2024001', 1, GETDATE(), 'María García'),
('Impresora-001', 'Impresora', 'IMP2024001', 2, GETDATE(), 'Ana López');
GO 