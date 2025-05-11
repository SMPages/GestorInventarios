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
-- Insertar 50 equipos de ejemplo
INSERT INTO Equipos (Nombre, Tipo, Serial, DepartamentoId, FechaIngreso, UsuarioCargo, Activo, FechaCreacion) VALUES
('PC-001', 'PC', 'PC2024001', 1, GETDATE(), 'Juan Pérez', 1, GETDATE()),
('Laptop-001', 'Laptop', 'LT2024001', 1, GETDATE(), 'María García', 1, GETDATE()),
('Impresora-001', 'Impresora', 'IMP2024001', 2, GETDATE(), 'Ana López', 1, GETDATE()),
('PC-002', 'PC', 'PC2024002', 2, GETDATE(), 'Carlos Ruiz', 1, GETDATE()),
('Laptop-002', 'Laptop', 'LT2024002', 2, GETDATE(), 'Laura Torres', 1, GETDATE()),
('Impresora-002', 'Impresora', 'IMP2024002', 3, GETDATE(), 'Pedro Gómez', 1, GETDATE()),
('PC-003', 'PC', 'PC2024003', 3, GETDATE(), 'Sofía Martínez', 1, GETDATE()),
('Laptop-003', 'Laptop', 'LT2024003', 3, GETDATE(), 'Andrés Díaz', 1, GETDATE()),
('Impresora-003', 'Impresora', 'IMP2024003', 4, GETDATE(), 'Mónica Herrera', 1, GETDATE()),
('PC-004', 'PC', 'PC2024004', 4, GETDATE(), 'Jorge Castro', 1, GETDATE()),
('Laptop-004', 'Laptop', 'LT2024004', 1, GETDATE(), 'Paula Ríos', 1, GETDATE()),
('Impresora-004', 'Impresora', 'IMP2024004', 2, GETDATE(), 'Luis Ramírez', 1, GETDATE()),
('PC-005', 'PC', 'PC2024005', 2, GETDATE(), 'Camila Suárez', 1, GETDATE()),
('Laptop-005', 'Laptop', 'LT2024005', 3, GETDATE(), 'Felipe Vargas', 1, GETDATE()),
('Impresora-005', 'Impresora', 'IMP2024005', 4, GETDATE(), 'Valentina León', 1, GETDATE()),
('PC-006', 'PC', 'PC2024006', 1, GETDATE(), 'Ricardo Peña', 1, GETDATE()),
('Laptop-006', 'Laptop', 'LT2024006', 1, GETDATE(), 'Natalia Cárdenas', 1, GETDATE()),
('Impresora-006', 'Impresora', 'IMP2024006', 2, GETDATE(), 'Esteban Gil', 1, GETDATE()),
('PC-007', 'PC', 'PC2024007', 2, GETDATE(), 'Daniela Pardo', 1, GETDATE()),
('Laptop-007', 'Laptop', 'LT2024007', 3, GETDATE(), 'Santiago Mora', 1, GETDATE()),
('Impresora-007', 'Impresora', 'IMP2024007', 3, GETDATE(), 'Lucía Rueda', 1, GETDATE()),
('PC-008', 'PC', 'PC2024008', 4, GETDATE(), 'Tomás Acosta', 1, GETDATE()),
('Laptop-008', 'Laptop', 'LT2024008', 4, GETDATE(), 'Gabriela Niño', 1, GETDATE()),
('Impresora-008', 'Impresora', 'IMP2024008', 1, GETDATE(), 'Samuel Torres', 1, GETDATE()),
('PC-009', 'PC', 'PC2024009', 1, GETDATE(), 'Martina Rojas', 1, GETDATE()),
('Laptop-009', 'Laptop', 'LT2024009', 2, GETDATE(), 'Emilio Salazar', 1, GETDATE()),
('Impresora-009', 'Impresora', 'IMP2024009', 2, GETDATE(), 'Isabella Peña', 1, GETDATE()),
('PC-010', 'PC', 'PC2024010', 3, GETDATE(), 'David Romero', 1, GETDATE()),
('Laptop-010', 'Laptop', 'LT2024010', 3, GETDATE(), 'Sara Medina', 1, GETDATE()),
('Impresora-010', 'Impresora', 'IMP2024010', 4, GETDATE(), 'Julián Vargas', 1, GETDATE()),
('PC-011', 'PC', 'PC2024011', 4, GETDATE(), 'Mariana López', 1, GETDATE()),
('Laptop-011', 'Laptop', 'LT2024011', 1, GETDATE(), 'Sebastián Cruz', 1, GETDATE()),
('Impresora-011', 'Impresora', 'IMP2024011', 2, GETDATE(), 'Valeria Ruiz', 1, GETDATE()),
('PC-012', 'PC', 'PC2024012', 2, GETDATE(), 'Alejandro Torres', 1, GETDATE()),
('Laptop-012', 'Laptop', 'LT2024012', 3, GETDATE(), 'Daniel Gómez', 1, GETDATE()),
('Impresora-012', 'Impresora', 'IMP2024012', 4, GETDATE(), 'Manuela Díaz', 1, GETDATE()),
('PC-013', 'PC', 'PC2024013', 1, GETDATE(), 'Juanita Herrera', 1, GETDATE()),
('Laptop-013', 'Laptop', 'LT2024013', 1, GETDATE(), 'Camilo Ríos', 1, GETDATE()),
('Impresora-013', 'Impresora', 'IMP2024013', 2, GETDATE(), 'Santiago León', 1, GETDATE()),
('PC-014', 'PC', 'PC2024014', 2, GETDATE(), 'Valentina Gil', 1, GETDATE()),
('Laptop-014', 'Laptop', 'LT2024014', 3, GETDATE(), 'Andrés Peña', 1, GETDATE()),
('Impresora-014', 'Impresora', 'IMP2024014', 3, GETDATE(), 'Laura Acosta', 1, GETDATE()),
('PC-015', 'PC', 'PC2024015', 4, GETDATE(), 'Felipe Niño', 1, GETDATE()),
('Laptop-015', 'Laptop', 'LT2024015', 4, GETDATE(), 'Lucía Salazar', 1, GETDATE()),
('Impresora-015', 'Impresora', 'IMP2024015', 1, GETDATE(), 'Tomás Rueda', 1, GETDATE()),
('PC-016', 'PC', 'PC2024016', 1, GETDATE(), 'Gabriela Romero', 1, GETDATE()),
('Laptop-016', 'Laptop', 'LT2024016', 2, GETDATE(), 'Samuel Medina', 1, GETDATE()),
('Impresora-016', 'Impresora', 'IMP2024016', 2, GETDATE(), 'Martina Vargas', 1, GETDATE()),
('PC-017', 'PC', 'PC2024017', 3, GETDATE(), 'Emilio López', 1, GETDATE()),
('Laptop-017', 'Laptop', 'LT2024017', 3, GETDATE(), 'Isabella Cruz', 1, GETDATE()),
('Impresora-017', 'Impresora', 'IMP2024017', 4, GETDATE(), 'David Ruiz', 1, GETDATE()),
('PC-018', 'PC', 'PC2024018', 4, GETDATE(), 'Sara Torres', 1, GETDATE()),
('Laptop-018', 'Laptop', 'LT2024018', 1, GETDATE(), 'Julián Gómez', 1, GETDATE());