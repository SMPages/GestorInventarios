Public Class Departamento
    Inherits BaseEntity

    Public Property DepartamentoId As Integer
    Public Property Nombre As String
    Public Property Descripcion As String
    Public Property Equipos As List(Of Equipo)
End Class