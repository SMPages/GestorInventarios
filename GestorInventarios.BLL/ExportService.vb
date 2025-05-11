Imports System.IO
Imports System.Data
Imports ClosedXML.Excel
Imports iTextSharp.text
Imports iTextSharp.text.pdf

Public Class ExportService
    Public Sub ExportToExcel(data As DataTable, filePath As String)
        Using workbook As New XLWorkbook()
            Dim worksheet = workbook.Worksheets.Add("Datos")

            ' Agregar encabezados
            For i As Integer = 0 To data.Columns.Count - 1
                worksheet.Cell(1, i + 1).Value = data.Columns(i).ColumnName
            Next

            ' Agregar datos
            For i As Integer = 0 To data.Rows.Count - 1
                For j As Integer = 0 To data.Columns.Count - 1
                    worksheet.Cell(i + 2, j + 1).Value = data.Rows(i)(j).ToString()
                Next
            Next

            ' Ajustar ancho de columnas
            worksheet.Columns().AdjustToContents()

            ' Guardar archivo
            workbook.SaveAs(filePath)
        End Using
    End Sub

    Public Sub ExportToPDF(data As DataTable, filePath As String, title As String)
        Using document As New Document(PageSize.A4.Rotate())
            PdfWriter.GetInstance(document, New FileStream(filePath, FileMode.Create))
            document.Open()

            ' Agregar título
            Dim titleFont = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 16)
            Dim titleParagraph = New Paragraph(title, titleFont)
            titleParagraph.Alignment = Element.ALIGN_CENTER
            titleParagraph.SpacingAfter = 20
            document.Add(titleParagraph)

            ' Crear tabla
            Dim table = New PdfPTable(data.Columns.Count)
            table.WidthPercentage = 100

            ' Agregar encabezados
            Dim headerFont = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 10)
            For Each column As DataColumn In data.Columns
                table.AddCell(New PdfPCell(New Phrase(column.ColumnName, headerFont)))
            Next

            ' Agregar datos
            Dim dataFont = FontFactory.GetFont(FontFactory.HELVETICA, 9)
            For Each row As DataRow In data.Rows
                For Each column As DataColumn In data.Columns
                    table.AddCell(New PdfPCell(New Phrase(row(column).ToString(), dataFont)))
                Next
            Next

            document.Add(table)
            document.Close()
        End Using
    End Sub
End Class