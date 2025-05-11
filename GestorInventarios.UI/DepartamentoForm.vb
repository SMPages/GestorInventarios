Imports System.Windows.Forms
Imports GestorInventarios.BLL
Imports GestorInventarios.Entities

Public Class DepartamentoForm
    Inherits Form

    Private ReadOnly _departamentoService As New DepartamentoService()
    Private _departamento As Departamento
    Private _isNew As Boolean

    Public Sub New(departamento As Departamento, isNew As Boolean)
        _departamento = If(departamento, New Departamento())
        _isNew = isNew
        InitializeComponent()
        If Not isNew Then
            LoadDepartamentoData()
        End If
    End Sub

    Private Sub InitializeComponent()
        Me.Text = If(_isNew, "Nuevo Departamento", "Editar Departamento")
        Me.Size = New Size(400, 300)
        Me.StartPosition = FormStartPosition.CenterParent
        Me.FormBorderStyle = FormBorderStyle.FixedDialog
        Me.MaximizeBox = False
        Me.MinimizeBox = False

        ' Crear controles
        CreateControls()
    End Sub

    Private Sub CreateControls()
        ' Nombre
        Dim lblNombre As New Label() With {
            .Text = "Nombre:",
            .Location = New Point(20, 20),
            .AutoSize = True
        }
        Dim txtNombre As New TextBox() With {
            .Name = "txtNombre",
            .Location = New Point(120, 17),
            .Width = 250
        }

        ' Descripción
        Dim lblDescripcion As New Label() With {
            .Text = "Descripción:",
            .Location = New Point(20, 50),
            .AutoSize = True
        }
        Dim txtDescripcion As New TextBox() With {
            .Name = "txtDescripcion",
            .Location = New Point(120, 47),
            .Width = 250,
            .Multiline = True,
            .Height = 100
        }

        ' Activo
        Dim chkActivo As New CheckBox() With {
            .Name = "chkActivo",
            .Text = "Activo",
            .Location = New Point(120, 160),
            .AutoSize = True,
            .Checked = True
        }

        ' Botones
        Dim btnGuardar As New Button() With {
            .Text = "Guardar",
            .Location = New Point(120, 200),
            .Width = 100
        }
        AddHandler btnGuardar.Click, AddressOf GuardarDepartamento

        Dim btnCancelar As New Button() With {
            .Text = "Cancelar",
            .Location = New Point(230, 200),
            .Width = 100
        }
        AddHandler btnCancelar.Click, AddressOf Cancelar

        ' Agregar controles al formulario
        Me.Controls.AddRange({
            lblNombre, txtNombre,
            lblDescripcion, txtDescripcion,
            chkActivo,
            btnGuardar, btnCancelar
        })
    End Sub

    Private Sub LoadDepartamentoData()
        Dim txtNombre = DirectCast(Me.Controls.Find("txtNombre", True)(0), TextBox)
        Dim txtDescripcion = DirectCast(Me.Controls.Find("txtDescripcion", True)(0), TextBox)
        Dim chkActivo = DirectCast(Me.Controls.Find("chkActivo", True)(0), CheckBox)

        txtNombre.Text = _departamento.Nombre
        txtDescripcion.Text = _departamento.Descripcion
        chkActivo.Checked = _departamento.Activo
    End Sub

    Private Sub GuardarDepartamento(sender As Object, e As EventArgs)
        Try
            Dim txtNombre = DirectCast(Me.Controls.Find("txtNombre", True)(0), TextBox)
            Dim txtDescripcion = DirectCast(Me.Controls.Find("txtDescripcion", True)(0), TextBox)
            Dim chkActivo = DirectCast(Me.Controls.Find("chkActivo", True)(0), CheckBox)

            _departamento.Nombre = txtNombre.Text
            _departamento.Descripcion = txtDescripcion.Text
            _departamento.Activo = chkActivo.Checked

            If _isNew Then
                _departamentoService.CreateDepartamento(_departamento)
            Else
                _departamentoService.UpdateDepartamento(_departamento)
            End If

            Me.DialogResult = DialogResult.OK
            Me.Close()
        Catch ex As Exception
            MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub Cancelar(sender As Object, e As EventArgs)
        Me.DialogResult = DialogResult.Cancel
        Me.Close()
    End Sub
End Class