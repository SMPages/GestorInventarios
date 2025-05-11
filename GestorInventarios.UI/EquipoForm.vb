Imports System.Windows.Forms
Imports GestorInventarios.BLL
Imports GestorInventarios.Entities

Public Class EquipoForm
    Inherits Form

    Private ReadOnly _equipoService As New EquipoService()
    Private ReadOnly _departamentoService As New DepartamentoService()
    Private _equipo As Equipo
    Private _isNew As Boolean

    Public Sub New(equipo As Equipo, isNew As Boolean)
        _equipo = If(equipo, New Equipo())
        _isNew = isNew
        InitializeComponent()
        LoadDepartamentos()
        If Not isNew Then
            LoadEquipoData()
        End If
    End Sub

    Private Sub InitializeComponent()
        Me.Text = If(_isNew, "Nuevo Equipo", "Editar Equipo")
        Me.Size = New Size(400, 500)
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

        ' Tipo
        Dim lblTipo As New Label() With {
            .Text = "Tipo:",
            .Location = New Point(20, 50),
            .AutoSize = True
        }
        Dim cmbTipo As New ComboBox() With {
            .Name = "cmbTipo",
            .Location = New Point(120, 47),
            .Width = 250,
            .DropDownStyle = ComboBoxStyle.DropDownList
        }
        cmbTipo.Items.AddRange({"PC", "Laptop", "Impresora", "Servidor", "Otro"})

        ' Serial
        Dim lblSerial As New Label() With {
            .Text = "Serial:",
            .Location = New Point(20, 80),
            .AutoSize = True
        }
        Dim txtSerial As New TextBox() With {
            .Name = "txtSerial",
            .Location = New Point(120, 77),
            .Width = 250
        }

        ' Departamento
        Dim lblDepartamento As New Label() With {
            .Text = "Departamento:",
            .Location = New Point(20, 110),
            .AutoSize = True
        }
        Dim cmbDepartamento As New ComboBox() With {
            .Name = "cmbDepartamento",
            .Location = New Point(120, 107),
            .Width = 250,
            .DropDownStyle = ComboBoxStyle.DropDownList
        }

        ' Fecha de Ingreso
        Dim lblFechaIngreso As New Label() With {
            .Text = "Fecha de Ingreso:",
            .Location = New Point(20, 140),
            .AutoSize = True
        }
        Dim dtpFechaIngreso As New DateTimePicker() With {
            .Name = "dtpFechaIngreso",
            .Location = New Point(120, 137),
            .Width = 250,
            .Format = DateTimePickerFormat.Short
        }

        ' Usuario a Cargo
        Dim lblUsuario As New Label() With {
            .Text = "Usuario a Cargo:",
            .Location = New Point(20, 170),
            .AutoSize = True
        }
        Dim txtUsuario As New TextBox() With {
            .Name = "txtUsuario",
            .Location = New Point(120, 167),
            .Width = 250
        }

        ' Activo
        Dim chkActivo As New CheckBox() With {
            .Name = "chkActivo",
            .Text = "Activo",
            .Location = New Point(120, 200),
            .AutoSize = True,
            .Checked = True
        }

        ' Botones
        Dim btnGuardar As New Button() With {
            .Text = "Guardar",
            .Location = New Point(120, 400),
            .Width = 100
        }
        AddHandler btnGuardar.Click, AddressOf GuardarEquipo

        Dim btnCancelar As New Button() With {
            .Text = "Cancelar",
            .Location = New Point(230, 400),
            .Width = 100
        }
        AddHandler btnCancelar.Click, AddressOf Cancelar

        ' Agregar controles al formulario
        Me.Controls.AddRange({
            lblNombre, txtNombre,
            lblTipo, cmbTipo,
            lblSerial, txtSerial,
            lblDepartamento, cmbDepartamento,
            lblFechaIngreso, dtpFechaIngreso,
            lblUsuario, txtUsuario,
            chkActivo,
            btnGuardar, btnCancelar
        })
    End Sub

    Private Sub LoadDepartamentos()
        Dim departamentos = _departamentoService.GetAllDepartamentos()
        Dim cmbDepartamento = DirectCast(Me.Controls.Find("cmbDepartamento", True)(0), ComboBox)
        cmbDepartamento.DisplayMember = "Nombre"
        cmbDepartamento.ValueMember = "DepartamentoId"
        cmbDepartamento.DataSource = departamentos
    End Sub

    Private Sub LoadEquipoData()
        ' Cargamos los datos del equipo seleccionado en los controles del formulario para su edición.
        Dim txtNombre = DirectCast(Me.Controls.Find("txtNombre", True)(0), TextBox)
        Dim cmbTipo = DirectCast(Me.Controls.Find("cmbTipo", True)(0), ComboBox)
        Dim txtSerial = DirectCast(Me.Controls.Find("txtSerial", True)(0), TextBox)
        Dim cmbDepartamento = DirectCast(Me.Controls.Find("cmbDepartamento", True)(0), ComboBox)
        Dim dtpFechaIngreso = DirectCast(Me.Controls.Find("dtpFechaIngreso", True)(0), DateTimePicker)
        Dim txtUsuario = DirectCast(Me.Controls.Find("txtUsuario", True)(0), TextBox)
        Dim chkActivo = DirectCast(Me.Controls.Find("chkActivo", True)(0), CheckBox)

        txtNombre.Text = _equipo.Nombre
        cmbTipo.SelectedItem = _equipo.Tipo
        txtSerial.Text = _equipo.Serial
        cmbDepartamento.SelectedValue = _equipo.DepartamentoId
        dtpFechaIngreso.Value = _equipo.FechaIngreso
        txtUsuario.Text = _equipo.UsuarioCargo
        chkActivo.Checked = _equipo.Activo
    End Sub

    Private Sub GuardarEquipo(sender As Object, e As EventArgs)
        Try
            ' Obtenemos los valores de los controles del formulario.
            Dim txtNombre = DirectCast(Me.Controls.Find("txtNombre", True)(0), TextBox)
            Dim cmbTipo = DirectCast(Me.Controls.Find("cmbTipo", True)(0), ComboBox)
            Dim txtSerial = DirectCast(Me.Controls.Find("txtSerial", True)(0), TextBox)
            Dim cmbDepartamento = DirectCast(Me.Controls.Find("cmbDepartamento", True)(0), ComboBox)
            Dim dtpFechaIngreso = DirectCast(Me.Controls.Find("dtpFechaIngreso", True)(0), DateTimePicker)
            Dim txtUsuario = DirectCast(Me.Controls.Find("txtUsuario", True)(0), TextBox)
            Dim chkActivo = DirectCast(Me.Controls.Find("chkActivo", True)(0), CheckBox)

            ' Asignamos los valores al objeto equipo.
            _equipo.Nombre = txtNombre.Text
            _equipo.Tipo = cmbTipo.SelectedItem.ToString()
            _equipo.Serial = txtSerial.Text
            _equipo.DepartamentoId = Convert.ToInt32(cmbDepartamento.SelectedValue)
            _equipo.FechaIngreso = dtpFechaIngreso.Value
            _equipo.UsuarioCargo = txtUsuario.Text
            _equipo.Activo = chkActivo.Checked

            ' Guardamos el equipo, ya sea nuevo o editado, aplicando las validaciones de negocio.
            If _isNew Then
                _equipoService.CreateEquipo(_equipo)
            Else
                _equipoService.UpdateEquipo(_equipo)
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