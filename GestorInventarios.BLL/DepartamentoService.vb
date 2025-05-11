Imports GestorInventarios.DAL
Imports GestorInventarios.Entities

Public Class DepartamentoService
    Private ReadOnly _departamentoRepository As New DepartamentoRepository()
    Private ReadOnly _equipoRepository As New EquipoRepository()

    Public Function GetAllDepartamentos(Optional incluirInactivos As Boolean = False) As List(Of Departamento)
        Return _departamentoRepository.GetAll(incluirInactivos)
    End Function

    Public Function GetDepartamentoById(id As Integer) As Departamento
        Return _departamentoRepository.GetById(id)
    End Function

    Public Function CreateDepartamento(departamento As Departamento) As Integer
        ValidateDepartamento(departamento)
        Return _departamentoRepository.Create(departamento)
    End Function

    Public Function UpdateDepartamento(departamento As Departamento) As Boolean
        ' Validamos que los datos del departamento sean correctos antes de actualizar.
        ValidateDepartamento(departamento)
        ' Antes de inactivar un departamento, revisamos si tiene equipos activos.
        ' Si hay al menos uno, no permitimos el cambio y mostramos un mensaje claro al usuario.
        If departamento.Activo = False Then
            Dim equiposActivos = _equipoRepository.GetAll().Where(Function(e) e.DepartamentoId = departamento.DepartamentoId AndAlso e.Activo).ToList()
            If equiposActivos.Any() Then
                Throw New InvalidOperationException("No se puede inactivar el departamento porque tiene equipos activos asignados.")
            End If
        End If
        Return _departamentoRepository.Update(departamento)
    End Function

    Public Function DeleteDepartamento(id As Integer) As Boolean
        ' Verificar si hay equipos asociados al departamento
        Dim equipos = _departamentoRepository.GetEquiposByDepartamento(id)
        If equipos.Any() Then
            Throw New InvalidOperationException("No se puede eliminar el departamento porque tiene equipos asociados")
        End If
        Return _departamentoRepository.Delete(id)
    End Function

    Public Function GetEquiposByDepartamento(departamentoId As Integer) As List(Of Equipo)
        Return _departamentoRepository.GetEquiposByDepartamento(departamentoId)
    End Function

    Public Function GetResumenEquiposPorDepartamento() As Dictionary(Of String, Integer)
        Dim departamentos = GetAllDepartamentos()
        Dim resumen As New Dictionary(Of String, Integer)

        For Each departamento In departamentos
            Dim cantidadEquipos = GetEquiposByDepartamento(departamento.DepartamentoId).Count
            resumen.Add(departamento.Nombre, cantidadEquipos)
        Next

        Return resumen
    End Function

    Private Sub ValidateDepartamento(departamento As Departamento)
        ' Validamos que el nombre del departamento no esté vacío antes de guardar.
        If String.IsNullOrEmpty(departamento.Nombre) Then
            Throw New ArgumentException("El nombre del departamento es requerido")
        End If

        ' Validar que no exista un departamento con el mismo nombre (evitar duplicados).
        Dim departamentosExistentes = _departamentoRepository.GetAll()
        If departamentosExistentes.Any(Function(d) d.Nombre = departamento.Nombre AndAlso d.DepartamentoId <> departamento.DepartamentoId) Then
            Throw New ArgumentException("Ya existe un departamento con ese nombre")
        End If
    End Sub
End Class