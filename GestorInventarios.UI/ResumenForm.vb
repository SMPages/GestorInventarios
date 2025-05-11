Imports System.Windows.Forms
Imports System.Windows.Forms.DataVisualization.Charting
Imports GestorInventarios.BLL

Public Class ResumenForm
    Inherits Form

    Private ReadOnly _departamentoService As New DepartamentoService()

    Public Sub New()
        InitializeComponent()
        LoadResumen()
    End Sub

    Private Sub InitializeComponent()
        Me.Text = "Resumen de Equipos por Departamento"
        Me.Size = New Size(800, 600)
        Me.StartPosition = FormStartPosition.CenterParent
        Me.FormBorderStyle = FormBorderStyle.FixedDialog
        Me.MaximizeBox = False
        Me.MinimizeBox = False

        ' Crear controles
        CreateControls()
    End Sub

    Private Sub CreateControls()
        ' Gráfico
        Dim chart As New Chart()
        chart.Dock = DockStyle.Fill
        chart.ChartAreas.Add(New ChartArea())
        chart.ChartAreas(0).AxisX.Title = "Departamento"
        chart.ChartAreas(0).AxisY.Title = "Cantidad de Equipos"
        chart.ChartAreas(0).AxisX.Interval = 1
        chart.ChartAreas(0).AxisX.LabelStyle.Angle = -45

        ' Serie de datos
        Dim series As New Series()
        series.ChartType = SeriesChartType.Column
        series.Name = "Equipos"
        chart.Series.Add(series)

        ' Botones
        Dim btnExportar As New Button() With {
            .Text = "Exportar a Excel",
            .Location = New Point(10, 10),
            .Width = 120
        }
        AddHandler btnExportar.Click, AddressOf ExportToExcel

        Dim btnCerrar As New Button() With {
            .Text = "Cerrar",
            .Location = New Point(140, 10),
            .Width = 80
        }
        AddHandler btnCerrar.Click, AddressOf Cerrar

        ' Panel para botones
        Dim panel As New Panel()
        panel.Dock = DockStyle.Top
        panel.Height = 50
        panel.Controls.Add(btnExportar)
        panel.Controls.Add(btnCerrar)

        ' Agregar controles al formulario
        Me.Controls.Add(chart)
        Me.Controls.Add(panel)
    End Sub

    Private Sub LoadResumen()
        Try
            Dim resumen = _departamentoService.GetResumenEquiposPorDepartamento()
            Dim chart = DirectCast(Me.Controls.Find("chart", True)(0), Chart)
            Dim series = chart.Series(0)

            series.Points.Clear()
            For Each item In resumen
                series.Points.AddXY(item.Key, item.Value)
            Next

            ' Ajustar el tamaño del gráfico
            chart.ChartAreas(0).AxisX.Maximum = resumen.Count - 1
            chart.ChartAreas(0).AxisY.Maximum = resumen.Values.Max() + 1
        Catch ex As Exception
            MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub ExportToExcel(sender As Object, e As EventArgs)
        Try
            Dim resumen = _departamentoService.GetResumenEquiposPorDepartamento()
            Dim saveFileDialog As New SaveFileDialog()
            saveFileDialog.Filter = "Excel Files|*.xlsx"
            saveFileDialog.Title = "Guardar Resumen"
            saveFileDialog.FileName = "Resumen_Equipos_" & DateTime.Now.ToString("yyyyMMdd")

            If saveFileDialog.ShowDialog() = DialogResult.OK Then
                ' Aquí se implementaría la exportación a Excel
                ' Por ahora solo mostramos un mensaje
                MessageBox.Show("La exportación a Excel se implementará en una versión futura.",
                              "Información", MessageBoxButtons.OK, MessageBoxIcon.Information)
            End If
        Catch ex As Exception
            MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub Cerrar(sender As Object, e As EventArgs)
        Me.Close()
    End Sub
End Class