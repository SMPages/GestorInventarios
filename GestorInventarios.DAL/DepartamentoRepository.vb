Imports System.Data.SqlClient
Imports GestorInventarios.Entities

Public Class DepartamentoRepository
    Inherits BaseRepository(Of Departamento)

    Public Function GetAll(Optional incluirInactivos As Boolean = False) As List(Of Departamento)
        ' Este método trae todos los departamentos. Si se indica, también incluye los inactivos.
        Dim query As String
        If incluirInactivos Then
            query = "SELECT * FROM Departamentos"
        Else
            query = "SELECT * FROM Departamentos WHERE Activo = 1"
        End If
        Dim dt = ExecuteQuery(query, New Dictionary(Of String, Object))
        Return MapDataTableToDepartamentos(dt)
    End Function

    Public Function GetById(id As Integer) As Departamento
        ' Busca un departamento por su ID, sin importar si está activo o inactivo.
        Dim query = "SELECT * FROM Departamentos WHERE DepartamentoId = @Id"
        Dim parameters As New Dictionary(Of String, Object)
        parameters.Add("@Id", id)
        Dim dt = ExecuteQuery(query, parameters)
        Return If(dt.Rows.Count > 0, MapDataRowToDepartamento(dt.Rows(0)), Nothing)
    End Function

    Public Function Create(departamento As Departamento) As Integer
        Dim query = "INSERT INTO Departamentos (Nombre, Descripcion, Activo, FechaCreacion) " &
                   "VALUES (@Nombre, @Descripcion, @Activo, @FechaCreacion); " &
                   "SELECT SCOPE_IDENTITY()"

        Dim parameters As New Dictionary(Of String, Object)
        parameters.Add("@Nombre", departamento.Nombre)
        parameters.Add("@Descripcion", departamento.Descripcion)
        parameters.Add("@Activo", departamento.Activo)
        parameters.Add("@FechaCreacion", DateTime.Now)

        Return ExecuteNonQuery(query, parameters)
    End Function

    Public Function Update(departamento As Departamento) As Boolean
        ' Actualiza los datos de un departamento existente en la base de datos.
        Dim query = "UPDATE Departamentos SET " &
                   "Nombre = @Nombre, " &
                   "Descripcion = @Descripcion, " &
                   "Activo = @Activo " &
                   "WHERE DepartamentoId = @DepartamentoId"

        Dim parameters As New Dictionary(Of String, Object)
        parameters.Add("@DepartamentoId", departamento.DepartamentoId)
        parameters.Add("@Nombre", departamento.Nombre)
        parameters.Add("@Descripcion", departamento.Descripcion)
        parameters.Add("@Activo", departamento.Activo)

        Return ExecuteNonQuery(query, parameters) > 0
    End Function

    Public Function Delete(id As Integer) As Boolean
        ' Marca un departamento como inactivo en la base de datos (borrado lógico).
        Dim query = "UPDATE Departamentos SET Activo = 0 WHERE DepartamentoId = @Id"
        Dim parameters As New Dictionary(Of String, Object)
        parameters.Add("@Id", id)
        Return ExecuteNonQuery(query, parameters) > 0
    End Function

    Public Function GetEquiposByDepartamento(departamentoId As Integer) As List(Of Equipo)
        ' Devuelve la lista de equipos activos asociados a un departamento.
        Dim query = "SELECT * FROM Equipos WHERE DepartamentoId = @DepartamentoId AND Activo = 1"
        Dim parameters As New Dictionary(Of String, Object)
        parameters.Add("@DepartamentoId", departamentoId)
        Dim dt = ExecuteQuery(query, parameters)
        Return MapDataTableToEquipos(dt)
    End Function

    Private Function MapDataTableToDepartamentos(dt As DataTable) As List(Of Departamento)
        Dim departamentos As New List(Of Departamento)
        For Each row As DataRow In dt.Rows
            departamentos.Add(MapDataRowToDepartamento(row))
        Next
        Return departamentos
    End Function

    Private Function MapDataRowToDepartamento(row As DataRow) As Departamento
        Return New Departamento With {
            .DepartamentoId = Convert.ToInt32(row("DepartamentoId")),
            .Nombre = row("Nombre").ToString(),
            .Descripcion = row("Descripcion").ToString(),
            .Activo = Convert.ToBoolean(row("Activo")),
            .FechaCreacion = Convert.ToDateTime(row("FechaCreacion"))
        }
    End Function

    Private Function MapDataTableToEquipos(dt As DataTable) As List(Of Equipo)
        Dim equipos As New List(Of Equipo)
        For Each row As DataRow In dt.Rows
            equipos.Add(New Equipo With {
                .EquipoId = Convert.ToInt32(row("EquipoId")),
                .Nombre = row("Nombre").ToString(),
                .Tipo = row("Tipo").ToString(),
                .Serial = row("Serial").ToString(),
                .DepartamentoId = Convert.ToInt32(row("DepartamentoId")),
                .FechaIngreso = Convert.ToDateTime(row("FechaIngreso")),
                .UsuarioCargo = row("UsuarioCargo").ToString(),
                .Activo = Convert.ToBoolean(row("Activo")),
                .FechaCreacion = Convert.ToDateTime(row("FechaCreacion"))
            })
        Next
        Return equipos
    End Function
End Class