Imports System.Configuration

Public Class DatabaseConfig
    Public Shared Function GetConnectionString() As String
        Return ConfigurationManager.ConnectionStrings("GestorInventariosConnection").ConnectionString
    End Function
End Class