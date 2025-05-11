Imports System.Data.SqlClient
Imports GestorInventarios.Entities

Public Class EquipoRepository
    Inherits BaseRepository(Of Equipo)

    Public Function GetAll(Optional incluirInactivos As Boolean = False) As List(Of Equipo)
        ' Este método trae todos los equipos. Si se indica, también incluye los inactivos.
        Dim query As String
        If incluirInactivos Then
            query = "SELECT * FROM Equipos"
        Else
            query = "SELECT * FROM Equipos WHERE Activo = 1"
        End If
        Dim dt = ExecuteQuery(query, New Dictionary(Of String, Object))
        Return MapDataTableToEquipos(dt)
    End Function

    Public Function GetById(id As Integer) As Equipo
        ' Busca un equipo por su ID, pero solo si está activo.
        Dim query = "SELECT * FROM Equipos WHERE EquipoId = @Id AND Activo = 1"
        Dim parameters As New Dictionary(Of String, Object)
        parameters.Add("@Id", id)
        Dim dt = ExecuteQuery(query, parameters)
        Return If(dt.Rows.Count > 0, MapDataRowToEquipo(dt.Rows(0)), Nothing)
    End Function

    Public Function Create(equipo As Equipo) As Integer
        Dim query = "INSERT INTO Equipos (Nombre, Tipo, Serial, DepartamentoId, FechaIngreso, UsuarioCargo, Activo, FechaCreacion) " &
                   "VALUES (@Nombre, @Tipo, @Serial, @DepartamentoId, @FechaIngreso, @UsuarioCargo, @Activo, @FechaCreacion); " &
                   "SELECT SCOPE_IDENTITY()"

        Dim parameters As New Dictionary(Of String, Object)
        parameters.Add("@Nombre", equipo.Nombre)
        parameters.Add("@Tipo", equipo.Tipo)
        parameters.Add("@Serial", equipo.Serial)
        parameters.Add("@DepartamentoId", equipo.DepartamentoId)
        parameters.Add("@FechaIngreso", equipo.FechaIngreso)
        parameters.Add("@UsuarioCargo", equipo.UsuarioCargo)
        parameters.Add("@Activo", equipo.Activo)
        parameters.Add("@FechaCreacion", DateTime.Now)

        Return ExecuteNonQuery(query, parameters)
    End Function

    Public Function Update(equipo As Equipo) As Boolean
        ' Actualiza los datos de un equipo existente en la base de datos.
        Dim query = "UPDATE Equipos SET " &
                   "Nombre = @Nombre, " &
                   "Tipo = @Tipo, " &
                   "Serial = @Serial, " &
                   "DepartamentoId = @DepartamentoId, " &
                   "FechaIngreso = @FechaIngreso, " &
                   "UsuarioCargo = @UsuarioCargo, " &
                   "Activo = @Activo " &
                   "WHERE EquipoId = @EquipoId"

        Dim parameters As New Dictionary(Of String, Object)
        parameters.Add("@EquipoId", equipo.EquipoId)
        parameters.Add("@Nombre", equipo.Nombre)
        parameters.Add("@Tipo", equipo.Tipo)
        parameters.Add("@Serial", equipo.Serial)
        parameters.Add("@DepartamentoId", equipo.DepartamentoId)
        parameters.Add("@FechaIngreso", equipo.FechaIngreso)
        parameters.Add("@UsuarioCargo", equipo.UsuarioCargo)
        parameters.Add("@Activo", equipo.Activo)

        Return ExecuteNonQuery(query, parameters) > 0
    End Function

    Public Function Delete(id As Integer) As Boolean
        ' Marca un equipo como inactivo en la base de datos (borrado lógico).
        Dim query = "UPDATE Equipos SET Activo = 0 WHERE EquipoId = @Id"
        Dim parameters As New Dictionary(Of String, Object)
        parameters.Add("@Id", id)
        Return ExecuteNonQuery(query, parameters) > 0
    End Function

    Private Function MapDataTableToEquipos(dt As DataTable) As List(Of Equipo)
        Dim equipos As New List(Of Equipo)
        For Each row As DataRow In dt.Rows
            equipos.Add(MapDataRowToEquipo(row))
        Next
        Return equipos
    End Function

    Private Function MapDataRowToEquipo(row As DataRow) As Equipo
        Return New Equipo With {
            .EquipoId = Convert.ToInt32(row("EquipoId")),
            .Nombre = row("Nombre").ToString(),
            .Tipo = row("Tipo").ToString(),
            .Serial = row("Serial").ToString(),
            .DepartamentoId = Convert.ToInt32(row("DepartamentoId")),
            .FechaIngreso = Convert.ToDateTime(row("FechaIngreso")),
            .UsuarioCargo = row("UsuarioCargo").ToString(),
            .Activo = Convert.ToBoolean(row("Activo")),
            .FechaCreacion = Convert.ToDateTime(row("FechaCreacion"))
        }
    End Function
End Class