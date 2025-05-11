Imports System.Windows.Forms
Imports GestorInventarios.Entities

Public Class SelectDepartamentoForm
    Inherits Form

    Private _departamentos As List(Of Departamento)
    Public Property DepartamentoSeleccionado As Departamento

    Public Sub New(departamentos As List(Of Departamento))
        _departamentos = departamentos
        InitializeComponent()
    End Sub

    Private Sub InitializeComponent()
        Me.Text = "Seleccionar Departamento"
        Me.Size = New Size(400, 400)
        Me.StartPosition = FormStartPosition.CenterParent
        Me.FormBorderStyle = FormBorderStyle.FixedDialog
        Me.MaximizeBox = False
        Me.MinimizeBox = False

        Dim dgv As New DataGridView()
        dgv.Dock = DockStyle.Top
        dgv.Height = 300
        dgv.DataSource = _departamentos
        dgv.SelectionMode = DataGridViewSelectionMode.FullRowSelect
        dgv.MultiSelect = False
        dgv.ReadOnly = True
        dgv.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill
        dgv.RowHeadersVisible = False
        AddHandler dgv.CellDoubleClick, AddressOf SeleccionarDepartamento
        Me.Controls.Add(dgv)

        Dim btnSeleccionar As New Button() With {.Text = "Seleccionar", .Location = New Point(100, 320), .Width = 80}
        AddHandler btnSeleccionar.Click, Sub(sender, e) SeleccionarDepartamento(dgv, Nothing)
        Dim btnCancelar As New Button() With {.Text = "Cancelar", .Location = New Point(200, 320), .Width = 80}
        AddHandler btnCancelar.Click, Sub(sender, e)
            Me.DialogResult = DialogResult.Cancel
            Me.Close()
        End Sub
        Me.Controls.Add(btnSeleccionar)
        Me.Controls.Add(btnCancelar)
    End Sub

    Private Sub SeleccionarDepartamento(sender As Object, e As EventArgs)
        Dim dgv = Me.Controls.OfType(Of DataGridView)().FirstOrDefault()
        If dgv IsNot Nothing AndAlso dgv.CurrentRow IsNot Nothing Then
            DepartamentoSeleccionado = CType(dgv.CurrentRow.DataBoundItem, Departamento)
            Me.DialogResult = DialogResult.OK
            Me.Close()
        End If
    End Sub
End Class 