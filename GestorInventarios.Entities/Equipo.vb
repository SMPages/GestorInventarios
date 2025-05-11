Public Class Equipo
    Inherits BaseEntity

    Public Property EquipoId As Integer
    Public Property Nombre As String
    Public Property Tipo As String
    Public Property Serial As String
    Public Property DepartamentoId As Integer
    Public Property FechaIngreso As DateTime
    Public Property UsuarioCargo As String
    Public Property Departamento As Departamento
End Class