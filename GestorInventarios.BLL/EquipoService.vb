Imports GestorInventarios.DAL
Imports GestorInventarios.Entities

Public Class EquipoService
    Private ReadOnly _equipoRepository As New EquipoRepository()
    Private ReadOnly _departamentoRepository As New DepartamentoRepository()

    Public Function GetAllEquipos(Optional incluirInactivos As Boolean = False) As List(Of Equipo)
        Return _equipoRepository.GetAll(incluirInactivos)
    End Function

    Public Function GetEquipoById(id As Integer) As Equipo
        Return _equipoRepository.GetById(id)
    End Function

    Public Function CreateEquipo(equipo As Equipo) As Integer
        ' Antes de crear un equipo, validamos que los datos sean correctos y cumplan las reglas de negocio.
        ValidateEquipo(equipo)
        Return _equipoRepository.Create(equipo)
    End Function

    Public Function UpdateEquipo(equipo As Equipo) As Boolean
        ' Antes de actualizar un equipo, validamos que los datos sean correctos y cumplan las reglas de negocio.
        ValidateEquipo(equipo)
        Return _equipoRepository.Update(equipo)
    End Function

    Public Function DeleteEquipo(id As Integer) As Boolean
        Return _equipoRepository.Delete(id)
    End Function

    Public Function GetEquiposByDepartamento(departamentoId As Integer) As List(Of Equipo)
        Return _departamentoRepository.GetEquiposByDepartamento(departamentoId)
    End Function

    Public Function GetEquiposByUsuario(usuario As String) As List(Of Equipo)
        Return _equipoRepository.GetAll().Where(Function(e) e.UsuarioCargo = usuario).ToList()
    End Function

    Public Function GetEquiposByTipo(tipo As String) As List(Of Equipo)
        Return _equipoRepository.GetAll().Where(Function(e) e.Tipo = tipo).ToList()
    End Function

    Public Function GetEquiposByEstado(activo As Boolean) As List(Of Equipo)
        Return _equipoRepository.GetAll().Where(Function(e) e.Activo = activo).ToList()
    End Function

    Private Sub ValidateEquipo(equipo As Equipo)
        ' Validamos que el nombre del equipo no esté vacío antes de guardar.
        If String.IsNullOrEmpty(equipo.Nombre) Then
            Throw New ArgumentException("El nombre del equipo es requerido")
        End If

        ' Validamos que el tipo de equipo esté seleccionado.
        If String.IsNullOrEmpty(equipo.Tipo) Then
            Throw New ArgumentException("El tipo de equipo es requerido")
        End If

        ' Validamos que el serial no esté vacío.
        If String.IsNullOrEmpty(equipo.Serial) Then
            Throw New ArgumentException("El serial del equipo es requerido")
        End If

        ' Validamos que el serial sea único. Si ya existe otro equipo con el mismo serial, no permitimos guardar.
        Dim equiposExistentes = _equipoRepository.GetAll(True)
        If equiposExistentes.Any(Function(e) e.Serial = equipo.Serial AndAlso e.EquipoId <> equipo.EquipoId) Then
            Throw New ArgumentException("Ya existe un equipo con ese serial")
        End If

        ' Validamos que el usuario no tenga más de dos equipos activos asignados.
        If Not String.IsNullOrEmpty(equipo.UsuarioCargo) Then
            Dim equiposUsuario = equiposExistentes.Where(Function(e) e.UsuarioCargo = equipo.UsuarioCargo AndAlso e.Activo AndAlso e.EquipoId <> equipo.EquipoId).ToList()
            ' Si es un equipo nuevo o se está activando, sumamos el actual
            If equipo.Activo AndAlso equiposUsuario.Count >= 2 Then
                Throw New InvalidOperationException("El usuario ya tiene asignados dos equipos activos.")
            End If
        End If
    End Sub
End Class