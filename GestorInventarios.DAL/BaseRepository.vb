Imports System.Data.SqlClient

Public Class BaseRepository(Of T As New)
    Protected ReadOnly ConnectionString As String

    Public Sub New()
        ConnectionString = DatabaseConfig.GetConnectionString()
    End Sub

    Protected Function ExecuteQuery(query As String, parameters As Dictionary(Of String, Object)) As DataTable
        Using connection As New SqlConnection(ConnectionString)
            Using command As New SqlCommand(query, connection)
                For Each param In parameters
                    command.Parameters.AddWithValue(param.Key, param.Value)
                Next

                Dim dataTable As New DataTable()
                connection.Open()
                dataTable.Load(command.ExecuteReader())
                Return dataTable
            End Using
        End Using
    End Function

    Protected Function ExecuteNonQuery(query As String, parameters As Dictionary(Of String, Object)) As Integer
        Using connection As New SqlConnection(ConnectionString)
            Using command As New SqlCommand(query, connection)
                For Each param In parameters
                    command.Parameters.AddWithValue(param.Key, param.Value)
                Next

                connection.Open()
                Return command.ExecuteNonQuery()
            End Using
        End Using
    End Function
End Class