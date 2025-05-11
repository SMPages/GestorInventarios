Imports System.Windows.Forms
Imports System.Data
Imports GestorInventarios.BLL
Imports System.Data.SqlClient
Imports GestorInventarios.DAL

Public Class MainForm
    Inherits Form

    Private ReadOnly _equipoService As New EquipoService()
    Private ReadOnly _departamentoService As New DepartamentoService()
    Private ReadOnly _exportService As New ExportService()
    Private _currentPage As Integer = 1
    Private _pageSize As Integer = 10

    ' Variables para paginación
    Private _totalPages As Integer = 1
    Private lblPage As Label
    Private btnPrevious As Button
    Private btnNext As Button
    Private _equiposFiltrados As List(Of GestorInventarios.Entities.Equipo)
    Private _equiposOriginales As List(Of GestorInventarios.Entities.Equipo)

    ' Controles globales para nueva interfaz
    Private tabControl As TabControl
    ' Equipos
    Private dgvEquipos As DataGridView
    Private cmbTipoEq As ComboBox
    Private txtUsuarioEq As TextBox
    Private cmbEstadoEq As ComboBox
    Private cmbDepartamentoEq As ComboBox
    Private btnFiltrarEq As Button
    Private btnNuevoEq As Button
    Private btnEditarEq As Button
    Private btnDesactivarEq As Button
    ' Departamentos
    Private dgvDepartamentos As DataGridView
    Private txtNombreDep As TextBox
    Private cmbActivoDep As ComboBox
    Private btnFiltrarDep As Button
    Private btnNuevoDep As Button
    Private btnEditarDep As Button
    Private btnEliminarDep As Button

    ' Controles para la nueva UI
    Private panelDashboard As Panel
    Private panelEquipos As Panel
    Private panelDepartamentos As Panel
    Private btnVerEquipos As Button
    Private btnVerDepartamentos As Button
    Private btnDashboard As Button

    Public Sub New()
        InitializeComponent()
        ' Instanciar los botones principales antes de usarlos
        btnDashboard = New Button() With {.Text = "Dashboard"}
        btnVerEquipos = New Button() With {.Text = "Equipos"}
        btnVerDepartamentos = New Button() With {.Text = "Departamentos"}

        ' Conectar los eventos de clic
        AddHandler btnDashboard.Click, Sub(sender, e) ShowDashboard()
        AddHandler btnVerEquipos.Click, Sub(sender, e) ShowEquipos()
        AddHandler btnVerDepartamentos.Click, Sub(sender, e) ShowDepartamentos()

        ' Cambiar fondo principal a blanco
        Me.BackColor = Color.White
        ' Agregar panel superior verde
        Dim panelSuperior As New Panel() With {
            .BackColor = ColorTranslator.FromHtml("#009640"),
            .Dock = DockStyle.Top,
            .Height = 60
        }
        Me.Controls.Add(panelSuperior)
        ' Agregar logo de Asocebú (debe estar en los recursos del proyecto como LogoAsocebu)
        Dim logo As New PictureBox() With {
            .Image = My.Resources.LogoAsocebu.ToBitmap(),
            .SizeMode = PictureBoxSizeMode.Zoom,
            .Location = New Point(10, 5),
            .Size = New Size(120, 50)
        }
        panelSuperior.Controls.Add(logo)
        ' Ajustar posición y estilo de los botones
        btnDashboard.BackColor = ColorTranslator.FromHtml("#009640")
        btnDashboard.ForeColor = Color.White
        btnDashboard.FlatStyle = FlatStyle.Flat
        btnDashboard.FlatAppearance.BorderSize = 0
        btnDashboard.Location = New Point(150, 10)
        btnDashboard.Height = 40
        btnDashboard.Width = 120
        btnDashboard.Font = New Font("Segoe UI", 12, FontStyle.Bold)
        panelSuperior.Controls.Add(btnDashboard)

        btnVerEquipos.BackColor = ColorTranslator.FromHtml("#009640")
        btnVerEquipos.ForeColor = Color.White
        btnVerEquipos.FlatStyle = FlatStyle.Flat
        btnVerEquipos.FlatAppearance.BorderSize = 0
        btnVerEquipos.Location = New Point(280, 10)
        btnVerEquipos.Height = 40
        btnVerEquipos.Width = 120
        btnVerEquipos.Font = New Font("Segoe UI", 12, FontStyle.Bold)
        panelSuperior.Controls.Add(btnVerEquipos)

        btnVerDepartamentos.BackColor = ColorTranslator.FromHtml("#009640")
        btnVerDepartamentos.ForeColor = Color.White
        btnVerDepartamentos.FlatStyle = FlatStyle.Flat
        btnVerDepartamentos.FlatAppearance.BorderSize = 0
        btnVerDepartamentos.Location = New Point(410, 10)
        btnVerDepartamentos.Height = 40
        btnVerDepartamentos.Width = 160
        btnVerDepartamentos.Font = New Font("Segoe UI", 12, FontStyle.Bold)
        panelSuperior.Controls.Add(btnVerDepartamentos)

        ShowDashboard()
    End Sub

    Private Sub InitializeComponent()
        Me.Text = "Gestor de Inventario de Equipos Informáticos"
        Me.Size = New Size(1024, 768)
        Me.StartPosition = FormStartPosition.CenterScreen
        ' Solo inicializar el formulario, sin menús ni filtros generales
    End Sub

    Private Sub LoadData(Optional pagina As Integer = 1)
        Dim equipos = _equipoService.GetAllEquipos()
        _equiposFiltrados = equipos
        MostrarPagina(pagina)
    End Sub

    Private Sub MostrarPagina(pagina As Integer)
        Dim dgv = Me.Controls.OfType(Of DataGridView)().FirstOrDefault()
        If dgv Is Nothing Then Return
        Dim pageSize = _pageSize
        Dim total = _equiposFiltrados.Count
        _totalPages = Math.Ceiling(total / pageSize)
        If pagina < 1 Then pagina = 1
        If pagina > _totalPages Then pagina = _totalPages
        _currentPage = pagina
        Dim equiposPagina = _equiposFiltrados.Skip((pagina - 1) * pageSize).Take(pageSize).ToList()
        dgv.DataSource = Nothing
        dgv.DataSource = equiposPagina
        lblPage.Text = $"Página {_currentPage} de {_totalPages}"
        btnPrevious.Enabled = (_currentPage > 1)
        btnNext.Enabled = (_currentPage < _totalPages)
    End Sub

    Private Sub PaginaAnterior(sender As Object, e As EventArgs)
        If _currentPage > 1 Then MostrarPagina(_currentPage - 1)
    End Sub
    Private Sub PaginaSiguiente(sender As Object, e As EventArgs)
        If _currentPage < _totalPages Then MostrarPagina(_currentPage + 1)
    End Sub

    Private Sub NewEquipo(sender As Object, e As EventArgs)
        Dim form As New EquipoForm(Nothing, True)
        If form.ShowDialog() = DialogResult.OK Then
            LoadEquipos() ' Refrescar la tabla automáticamente
        End If
    End Sub

    Private Sub EditEquipo(sender As Object, e As EventArgs)
        Dim dgv = Me.Controls.OfType(Of DataGridView)().FirstOrDefault()
        If dgv Is Nothing OrElse dgv.CurrentRow Is Nothing Then
            MessageBox.Show("Seleccione un equipo para editar.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Information)
            Return
        End If
        Dim equipo = CType(dgv.CurrentRow.DataBoundItem, GestorInventarios.Entities.Equipo)
        Dim form As New EquipoForm(equipo, False)
        If form.ShowDialog() = DialogResult.OK Then
            LoadData(_currentPage)
        End If
    End Sub

    Private Sub DeleteEquipo(sender As Object, e As EventArgs)
        Dim dgv = Me.Controls.OfType(Of DataGridView)().FirstOrDefault()
        If dgv Is Nothing OrElse dgv.CurrentRow Is Nothing Then
            MessageBox.Show("Seleccione un equipo para desactivar.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Information)
            Return
        End If
        Dim equipo = CType(dgv.CurrentRow.DataBoundItem, GestorInventarios.Entities.Equipo)
        If MessageBox.Show($"¿Está seguro de desactivar el equipo '{equipo.Nombre}'?", "Confirmar", MessageBoxButtons.YesNo, MessageBoxIcon.Question) = DialogResult.Yes Then
            _equipoService.UpdateEquipo(New GestorInventarios.Entities.Equipo With {.EquipoId = equipo.EquipoId, .Nombre = equipo.Nombre, .Tipo = equipo.Tipo, .Serial = equipo.Serial, .DepartamentoId = equipo.DepartamentoId, .FechaIngreso = equipo.FechaIngreso, .UsuarioCargo = equipo.UsuarioCargo, .Activo = False, .FechaCreacion = equipo.FechaCreacion})
            LoadData(_currentPage)
        End If
    End Sub

    Private Sub NewDepartamento(sender As Object, e As EventArgs)
        Dim form As New DepartamentoForm(Nothing, True)
        If form.ShowDialog() = DialogResult.OK Then
            LoadData(_currentPage)
        End If
    End Sub

    Private Sub EditDepartamento(sender As Object, e As EventArgs)
        Dim departamentos = _departamentoService.GetAllDepartamentos()
        Dim depForm As New SelectDepartamentoForm(departamentos)
        If depForm.ShowDialog() = DialogResult.OK Then
            Dim dep = depForm.DepartamentoSeleccionado
            Dim form As New DepartamentoForm(dep, False)
            If form.ShowDialog() = DialogResult.OK Then
                LoadData(_currentPage)
            End If
        End If
    End Sub

    Private Sub DeleteDepartamento(sender As Object, e As EventArgs)
        Dim departamentos = _departamentoService.GetAllDepartamentos()
        Dim depForm As New SelectDepartamentoForm(departamentos)
        If depForm.ShowDialog() = DialogResult.OK Then
            Dim dep = depForm.DepartamentoSeleccionado
            If MessageBox.Show($"¿Está seguro de eliminar el departamento '{dep.Nombre}'?", "Confirmar", MessageBoxButtons.YesNo, MessageBoxIcon.Question) = DialogResult.Yes Then
                Try
                    _departamentoService.DeleteDepartamento(dep.DepartamentoId)
                    LoadData(_currentPage)
                Catch ex As Exception
                    MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
                End Try
            End If
        End If
    End Sub

    Private Sub ExportarExcel(sender As Object, e As EventArgs)
        Try
            Using saveDialog As New SaveFileDialog()
                saveDialog.Filter = "Archivos Excel (*.xlsx)|*.xlsx"
                saveDialog.Title = "Exportar a Excel"
                saveDialog.DefaultExt = "xlsx"

                If saveDialog.ShowDialog() = DialogResult.OK Then
                    Dim data As DataTable = GetDataForExport()
                    _exportService.ExportToExcel(data, saveDialog.FileName)
                    MessageBox.Show("Archivo exportado correctamente.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information)
                End If
            End Using
        Catch ex As Exception
            MessageBox.Show("Error al exportar: " & ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub ExportarPDF(sender As Object, e As EventArgs)
        Try
            Using saveDialog As New SaveFileDialog()
                saveDialog.Filter = "Archivos PDF (*.pdf)|*.pdf"
                saveDialog.Title = "Exportar a PDF"
                saveDialog.DefaultExt = "pdf"

                If saveDialog.ShowDialog() = DialogResult.OK Then
                    Dim data As DataTable = GetDataForExport()
                    _exportService.ExportToPDF(data, saveDialog.FileName, "Reporte de Inventario")
                    MessageBox.Show("Archivo exportado correctamente.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information)
                End If
            End Using
        Catch ex As Exception
            MessageBox.Show("Error al exportar: " & ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Function GetDataForExport() As DataTable
        Dim data As New DataTable()

        ' Agregar columnas
        data.Columns.Add("ID", GetType(Integer))
        data.Columns.Add("Nombre", GetType(String))
        data.Columns.Add("Departamento", GetType(String))
        data.Columns.Add("Estado", GetType(String))

        ' Obtener datos de los servicios
        Dim equipos = _equipoService.GetAllEquipos()
        Dim departamentos = _departamentoService.GetAllDepartamentos().ToDictionary(Function(d) d.DepartamentoId)

        ' Llenar datos
        For Each equipo In equipos
            Dim departamento = If(departamentos.ContainsKey(equipo.DepartamentoId),
                                departamentos(equipo.DepartamentoId).Nombre,
                                "Sin departamento")

            data.Rows.Add(
                equipo.EquipoId,
                equipo.Nombre,
                departamento,
                If(equipo.Activo, "Activo", "Inactivo")
            )
        Next

        Return data
    End Function

    Private Sub ShowResumenDepartamentos(sender As Object, e As EventArgs)
        Dim resumen = _departamentoService.GetResumenEquiposPorDepartamento()
        Dim mensaje As String = String.Join(Environment.NewLine, resumen.Select(Function(r) $"{r.Key}: {r.Value} equipos"))
        MessageBox.Show(mensaje, "Resumen por Departamento", MessageBoxButtons.OK, MessageBoxIcon.Information)
    End Sub

    Private Sub ExitApplication(sender As Object, e As EventArgs)
        Application.Exit()
    End Sub

    Private Sub ProbarConexionBaseDatos()
        Dim connectionString As String = DatabaseConfig.GetConnectionString()
        Using connection As New SqlConnection(connectionString)
            Try
                connection.Open()
                MessageBox.Show("¡Conexión exitosa a la base de datos!", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information)
            Catch ex As Exception
                MessageBox.Show("Error al conectar a la base de datos: " & ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            End Try
        End Using
    End Sub

    ' Ajustar encabezados y solo permitir edición en campos específicos
    Private Sub Dgv_DataBindingComplete(sender As Object, e As DataGridViewBindingCompleteEventArgs)
        Dim dgv = CType(sender, DataGridView)
        If dgv.Columns.Contains("EquipoId") Then dgv.Columns("EquipoId").HeaderText = "ID" : dgv.Columns("EquipoId").ReadOnly = True
        If dgv.Columns.Contains("Nombre") Then dgv.Columns("Nombre").HeaderText = "Nombre"
        If dgv.Columns.Contains("Tipo") Then dgv.Columns("Tipo").HeaderText = "Tipo"
        If dgv.Columns.Contains("Serial") Then dgv.Columns("Serial").HeaderText = "Serial" : dgv.Columns("Serial").ReadOnly = True
        If dgv.Columns.Contains("DepartamentoId") Then dgv.Columns("DepartamentoId").HeaderText = "Departamento" : dgv.Columns("DepartamentoId").ReadOnly = True
        If dgv.Columns.Contains("FechaIngreso") Then dgv.Columns("FechaIngreso").HeaderText = "Fecha Ingreso" : dgv.Columns("FechaIngreso").ReadOnly = True
        If dgv.Columns.Contains("UsuarioCargo") Then dgv.Columns("UsuarioCargo").HeaderText = "Usuario a Cargo"
        If dgv.Columns.Contains("Activo") Then dgv.Columns("Activo").HeaderText = "Activo"
        If dgv.Columns.Contains("FechaCreacion") Then dgv.Columns("FechaCreacion").HeaderText = "Fecha Creación" : dgv.Columns("FechaCreacion").ReadOnly = True
    End Sub

    ' Guardar cambios al editar directamente en la grilla
    Private Sub Dgv_CellEndEdit(sender As Object, e As DataGridViewCellEventArgs)
        Dim dgv = CType(sender, DataGridView)
        If e.RowIndex < 0 Then Return
        Dim equipo = CType(dgv.Rows(e.RowIndex).DataBoundItem, GestorInventarios.Entities.Equipo)
        Try
            _equipoService.UpdateEquipo(equipo)
        Catch ex As Exception
            MessageBox.Show("Error al guardar el cambio: " & ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            LoadData(_currentPage)
        End Try
    End Sub

    Private Sub InitMainMenu()
        ' Botones principales
        btnDashboard = New Button() With {.Text = "Dashboard", .Location = New Point(20, 20), .Width = 120, .Height = 40, .Font = New Font("Segoe UI", 12, FontStyle.Bold)}
        btnVerEquipos = New Button() With {.Text = "Equipos", .Location = New Point(160, 20), .Width = 120, .Height = 40, .Font = New Font("Segoe UI", 12, FontStyle.Bold)}
        btnVerDepartamentos = New Button() With {.Text = "Departamentos", .Location = New Point(300, 20), .Width = 160, .Height = 40, .Font = New Font("Segoe UI", 12, FontStyle.Bold)}
        AddHandler btnDashboard.Click, Sub(sender, e) ShowDashboard()
        AddHandler btnVerEquipos.Click, Sub(sender, e) ShowEquipos()
        AddHandler btnVerDepartamentos.Click, Sub(sender, e) ShowDepartamentos()
        Me.Controls.Add(btnDashboard)
        Me.Controls.Add(btnVerEquipos)
        Me.Controls.Add(btnVerDepartamentos)
    End Sub

    Private Sub ShowDashboard()
        HideAllPanels()
        ' Eliminar panelDashboard anterior si existe
        If panelDashboard IsNot Nothing Then
            Me.Controls.Remove(panelDashboard)
            panelDashboard.Dispose()
        End If
        panelDashboard = New Panel() With {.Dock = DockStyle.Fill, .BackColor = Color.White, .Location = New Point(0, 70)}

        ' Paneles tipo tarjeta para cada métrica
        Dim totalEquipos = _equipoService.GetAllEquipos().Count
        Dim totalDepartamentos = _departamentoService.GetAllDepartamentos().Count
        Dim activos = _equipoService.GetAllEquipos().Where(Function(e) e.Activo).Count
        Dim inactivos = totalEquipos - activos

        Dim cardWidth As Integer = 220
        Dim cardHeight As Integer = 90
        Dim espacio As Integer = 30
        Dim topCards As Integer = 80
        Dim leftStart As Integer = 40

        ' Card: Total de Equipos
        Dim cardEquipos As New Panel() With {
            .BackColor = ColorTranslator.FromHtml("#009640"),
            .Size = New Size(cardWidth, cardHeight),
            .Location = New Point(leftStart, topCards),
            .BorderStyle = BorderStyle.None
        }
        Dim lblEquiposTitulo As New Label() With {
            .Text = "Total de Equipos",
            .ForeColor = Color.White,
            .Font = New Font("Segoe UI", 12, FontStyle.Bold),
            .Location = New Point(20, 10),
            .AutoSize = True
        }
        Dim lblEquiposValor As New Label() With {
            .Text = totalEquipos.ToString(),
            .ForeColor = Color.White,
            .Font = New Font("Segoe UI", 28, FontStyle.Bold),
            .Location = New Point(20, 35),
            .AutoSize = True
        }
        cardEquipos.Controls.Add(lblEquiposTitulo)
        cardEquipos.Controls.Add(lblEquiposValor)
        panelDashboard.Controls.Add(cardEquipos)

        ' Card: Total de Departamentos
        Dim cardDepartamentos As New Panel() With {
            .BackColor = ColorTranslator.FromHtml("#007d3c"),
            .Size = New Size(cardWidth, cardHeight),
            .Location = New Point(leftStart + cardWidth + espacio, topCards),
            .BorderStyle = BorderStyle.None
        }
        Dim lblDepartamentosTitulo As New Label() With {
            .Text = "Total de Departamentos",
            .ForeColor = Color.White,
            .Font = New Font("Segoe UI", 12, FontStyle.Bold),
            .Location = New Point(20, 10),
            .AutoSize = True
        }
        Dim lblDepartamentosValor As New Label() With {
            .Text = totalDepartamentos.ToString(),
            .ForeColor = Color.White,
            .Font = New Font("Segoe UI", 28, FontStyle.Bold),
            .Location = New Point(20, 35),
            .AutoSize = True
        }
        cardDepartamentos.Controls.Add(lblDepartamentosTitulo)
        cardDepartamentos.Controls.Add(lblDepartamentosValor)
        panelDashboard.Controls.Add(cardDepartamentos)

        ' Card: Equipos Activos
        Dim cardActivos As New Panel() With {
            .BackColor = ColorTranslator.FromHtml("#FFD700"),
            .Size = New Size(cardWidth, cardHeight),
            .Location = New Point(leftStart + 2 * (cardWidth + espacio), topCards),
            .BorderStyle = BorderStyle.None
        }
        Dim lblActivosTitulo As New Label() With {
            .Text = "Equipos Activos",
            .ForeColor = Color.FromArgb(0, 51, 102),
            .Font = New Font("Segoe UI", 12, FontStyle.Bold),
            .Location = New Point(20, 10),
            .AutoSize = True
        }
        Dim lblActivosValor As New Label() With {
            .Text = activos.ToString(),
            .ForeColor = Color.FromArgb(0, 51, 102),
            .Font = New Font("Segoe UI", 28, FontStyle.Bold),
            .Location = New Point(20, 35),
            .AutoSize = True
        }
        cardActivos.Controls.Add(lblActivosTitulo)
        cardActivos.Controls.Add(lblActivosValor)
        panelDashboard.Controls.Add(cardActivos)

        ' Card: Equipos Inactivos
        Dim cardInactivos As New Panel() With {
            .BackColor = ColorTranslator.FromHtml("#e0e0e0"),
            .Size = New Size(cardWidth, cardHeight),
            .Location = New Point(leftStart + 3 * (cardWidth + espacio), topCards),
            .BorderStyle = BorderStyle.None
        }
        Dim lblInactivosTitulo As New Label() With {
            .Text = "Equipos Inactivos",
            .ForeColor = Color.FromArgb(80, 80, 80),
            .Font = New Font("Segoe UI", 12, FontStyle.Bold),
            .Location = New Point(20, 10),
            .AutoSize = True
        }
        Dim lblInactivosValor As New Label() With {
            .Text = inactivos.ToString(),
            .ForeColor = Color.FromArgb(80, 80, 80),
            .Font = New Font("Segoe UI", 28, FontStyle.Bold),
            .Location = New Point(20, 35),
            .AutoSize = True
        }
        cardInactivos.Controls.Add(lblInactivosTitulo)
        cardInactivos.Controls.Add(lblInactivosValor)
        panelDashboard.Controls.Add(cardInactivos)

        ' Título principal
        Dim lblTitulo As New Label() With {
            .Text = "Dashboard de Inventario",
            .Font = New Font("Segoe UI", 20, FontStyle.Bold),
            .Location = New Point(leftStart, 30),
            .AutoSize = True
        }
        panelDashboard.Controls.Add(lblTitulo)

        panelDashboard.Visible = True
        Me.Controls.Add(panelDashboard)
        panelDashboard.BringToFront()
    End Sub

    Private Sub ShowEquipos()
        HideAllPanels()
        If panelEquipos Is Nothing Then
            panelEquipos = CreateEquiposPanel()
        End If
        panelEquipos.Location = New Point(0, 70)
        panelEquipos.Size = New Size(Me.ClientSize.Width, Me.ClientSize.Height - 70)
        panelEquipos.Anchor = AnchorStyles.Top Or AnchorStyles.Bottom Or AnchorStyles.Left Or AnchorStyles.Right
        panelEquipos.Visible = True
        Me.Controls.Add(panelEquipos)
        panelEquipos.BringToFront()
        btnDashboard.BringToFront()
        btnVerEquipos.BringToFront()
        btnVerDepartamentos.BringToFront()
        LoadEquipos()
    End Sub

    Private Sub ShowDepartamentos()
        HideAllPanels()
        If panelDepartamentos Is Nothing Then
            ' Crear panel de departamentos directamente aquí
            panelDepartamentos = New Panel() With {.Dock = DockStyle.Fill}
            ' Filtros
            Dim filterPanel As New Panel() With {.Dock = DockStyle.Top, .Height = 70}
            Dim lblNombreDep As New Label() With {.Text = "Nombre:", .Location = New Point(10, 5), .AutoSize = True}
            txtNombreDep = New TextBox() With {.Location = New Point(10, 25), .Width = 150}
            Dim lblActivoDep As New Label() With {.Text = "Activo:", .Location = New Point(170, 5), .AutoSize = True}
            cmbActivoDep = New ComboBox() With {.Location = New Point(170, 25), .Width = 100, .DropDownStyle = ComboBoxStyle.DropDownList}
            btnFiltrarDep = New Button() With {.Text = "Filtrar", .Location = New Point(280, 25), .Width = 70}
            AddHandler btnFiltrarDep.Click, AddressOf FiltrarDepartamentos
            filterPanel.Controls.AddRange({lblNombreDep, txtNombreDep, lblActivoDep, cmbActivoDep, btnFiltrarDep})
            ' Botones CRUD y exportar
            Dim crudPanel As New Panel() With {.Dock = DockStyle.Top, .Height = 45}
            btnNuevoDep = New Button() With {.Text = "Nuevo", .Location = New Point(10, 10), .Width = 70}
            Dim btnExportarExcelDep As New Button() With {.Text = "Excel", .Location = New Point(90, 10), .Width = 70}
            Dim btnExportarPDFDep As New Button() With {.Text = "PDF", .Location = New Point(170, 10), .Width = 70}
            AddHandler btnNuevoDep.Click, AddressOf NewDepartamento
            AddHandler btnExportarExcelDep.Click, AddressOf ExportarExcelDepartamentosFiltrados
            AddHandler btnExportarPDFDep.Click, AddressOf ExportarPDFDepartamentosFiltrados
            crudPanel.Controls.AddRange({btnNuevoDep, btnExportarExcelDep, btnExportarPDFDep})
            ' DataGridView Departamentos
            dgvDepartamentos = New DataGridView() With {
                .Dock = DockStyle.Fill,
                .ReadOnly = True,
                .SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                .MultiSelect = False,
                .AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                .RowHeadersVisible = False,
                .AllowUserToAddRows = False
            }
            AddHandler dgvDepartamentos.CellContentClick, AddressOf DgvDepartamentos_CellContentClick
            ' Agregar controles en el orden correcto
            panelDepartamentos.Controls.Add(dgvDepartamentos)
            panelDepartamentos.Controls.Add(crudPanel)
            panelDepartamentos.Controls.Add(filterPanel)
        End If
        panelDepartamentos.Location = New Point(0, 70)
        panelDepartamentos.Size = New Size(Me.ClientSize.Width, Me.ClientSize.Height - 70)
        panelDepartamentos.Anchor = AnchorStyles.Top Or AnchorStyles.Bottom Or AnchorStyles.Left Or AnchorStyles.Right
        panelDepartamentos.Visible = True
        Me.Controls.Add(panelDepartamentos)
        panelDepartamentos.BringToFront()
        btnDashboard.BringToFront()
        btnVerEquipos.BringToFront()
        btnVerDepartamentos.BringToFront()
        LoadDepartamentos()
    End Sub

    Private Sub HideAllPanels()
        If panelDashboard IsNot Nothing Then panelDashboard.Visible = False
        If panelEquipos IsNot Nothing Then panelEquipos.Visible = False
        If panelDepartamentos IsNot Nothing Then panelDepartamentos.Visible = False
    End Sub

    ' PANEL EQUIPOS
    Private Function CreateEquiposPanel() As Panel
        Dim panel As New Panel() With {.Dock = DockStyle.Fill}
        ' Filtros
        Dim filterPanel As New Panel() With {.Dock = DockStyle.Top, .Height = 70}
        ' Labels arriba de los filtros
        Dim lblTipo As New Label() With {.Text = "Tipo:", .Location = New Point(10, 5), .AutoSize = True}
        cmbTipoEq = New ComboBox() With {.Location = New Point(10, 25), .Width = 100, .DropDownStyle = ComboBoxStyle.DropDownList}
        Dim lblDepartamento As New Label() With {.Text = "Departamento:", .Location = New Point(120, 5), .AutoSize = True}
        cmbDepartamentoEq = New ComboBox() With {.Location = New Point(120, 25), .Width = 150, .DropDownStyle = ComboBoxStyle.DropDownList}
        Dim lblUsuario As New Label() With {.Text = "Usuario:", .Location = New Point(280, 5), .AutoSize = True}
        txtUsuarioEq = New TextBox() With {.Location = New Point(280, 25), .Width = 120}
        Dim lblEstado As New Label() With {.Text = "Estado:", .Location = New Point(410, 5), .AutoSize = True}
        cmbEstadoEq = New ComboBox() With {.Location = New Point(410, 25), .Width = 100, .DropDownStyle = ComboBoxStyle.DropDownList}
        btnFiltrarEq = New Button() With {.Text = "Filtrar", .Location = New Point(520, 25), .Width = 70}
        AddHandler btnFiltrarEq.Click, AddressOf FiltrarEquipos
        filterPanel.Controls.AddRange({lblTipo, cmbTipoEq, lblDepartamento, cmbDepartamentoEq, lblUsuario, txtUsuarioEq, lblEstado, cmbEstadoEq, btnFiltrarEq})

        ' Botón Nuevo y exportar
        Dim crudPanel As New Panel() With {.Dock = DockStyle.Top, .Height = 45}
        btnNuevoEq = New Button() With {.Text = "Nuevo", .Location = New Point(10, 10), .Width = 70}
        Dim btnExportarExcel As New Button() With {.Text = "Excel", .Location = New Point(90, 10), .Width = 70}
        Dim btnExportarPDF As New Button() With {.Text = "PDF", .Location = New Point(170, 10), .Width = 70}
        AddHandler btnNuevoEq.Click, AddressOf NewEquipo
        AddHandler btnExportarExcel.Click, AddressOf ExportarExcelEquiposFiltrados
        AddHandler btnExportarPDF.Click, AddressOf ExportarPDFEquiposFiltrados
        crudPanel.Controls.AddRange({btnNuevoEq, btnExportarExcel, btnExportarPDF})

        ' DataGridView debajo de los botones
        dgvEquipos = New DataGridView() With {
            .Dock = DockStyle.Fill,
            .ReadOnly = True,
            .SelectionMode = DataGridViewSelectionMode.FullRowSelect,
            .MultiSelect = False,
            .AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
            .RowHeadersVisible = False,
            .AllowUserToAddRows = False
        }
        AddHandler dgvEquipos.CellContentClick, AddressOf DgvEquipos_CellContentClick
        AddHandler dgvEquipos.CellValueChanged, AddressOf DgvEquipos_CellValueChanged
        AddHandler dgvEquipos.CurrentCellDirtyStateChanged, AddressOf DgvEquipos_CurrentCellDirtyStateChanged
        AddHandler dgvEquipos.DataBindingComplete, AddressOf Dgv_DataBindingComplete

        ' Agregar controles en el orden correcto
        panel.Controls.Add(dgvEquipos)      ' Primero el DataGridView (Fill)
        panel.Controls.Add(crudPanel)       ' Luego el panel de botones (Top)
        panel.Controls.Add(filterPanel)     ' Luego el panel de filtros (Top)
        Return panel
    End Function

    ' Exportar equipos filtrados a Excel
    Private Sub ExportarExcelEquiposFiltrados(sender As Object, e As EventArgs)
        Try
            Using saveDialog As New SaveFileDialog()
                saveDialog.Filter = "Archivos Excel (*.xlsx)|*.xlsx"
                saveDialog.Title = "Exportar a Excel"
                saveDialog.DefaultExt = "xlsx"
                If saveDialog.ShowDialog() = DialogResult.OK Then
                    Dim data As DataTable = GetDataTableFromDGV(dgvEquipos)
                    _exportService.ExportToExcel(data, saveDialog.FileName)
                    MessageBox.Show("Archivo exportado correctamente.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information)
                End If
            End Using
        Catch ex As Exception
            MessageBox.Show("Error al exportar: " & ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    ' Exportar equipos filtrados a PDF
    Private Sub ExportarPDFEquiposFiltrados(sender As Object, e As EventArgs)
        Try
            Using saveDialog As New SaveFileDialog()
                saveDialog.Filter = "Archivos PDF (*.pdf)|*.pdf"
                saveDialog.Title = "Exportar a PDF"
                saveDialog.DefaultExt = "pdf"
                If saveDialog.ShowDialog() = DialogResult.OK Then
                    Dim data As DataTable = GetDataTableFromDGV(dgvEquipos)
                    _exportService.ExportToPDF(data, saveDialog.FileName, "Reporte de Inventario")
                    MessageBox.Show("Archivo exportado correctamente.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information)
                End If
            End Using
        Catch ex As Exception
            MessageBox.Show("Error al exportar: " & ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    ' Obtener DataTable desde el DataGridView mostrado
    Private Function GetDataTableFromDGV(dgv As DataGridView) As DataTable
        Dim dt As New DataTable()
        ' Agregar columnas visibles
        For Each col As DataGridViewColumn In dgv.Columns
            If col.Visible AndAlso col.Name <> "Editar" Then
                dt.Columns.Add(col.HeaderText, GetType(String))
            End If
        Next
        ' Agregar filas
        For Each row As DataGridViewRow In dgv.Rows
            If Not row.IsNewRow Then
                Dim values As New List(Of String)
                For Each col As DataGridViewColumn In dgv.Columns
                    If col.Visible AndAlso col.Name <> "Editar" Then
                        Dim value = row.Cells(col.Index).FormattedValue
                        values.Add(If(value IsNot Nothing, value.ToString(), ""))
                    End If
                Next
                dt.Rows.Add(values.ToArray())
            End If
        Next
        Return dt
    End Function

    ' CARGA Y FILTROS EQUIPOS
    Private Sub LoadEquipos()
        ' Traer todos los equipos (activos e inactivos)
        Dim equipos = _equipoService.GetAllEquipos(True) ' True = traer todos
        _equiposOriginales = equipos
        SetEquiposDataSource(equipos)
        LlenarCombosFiltro(equipos)
    End Sub

    Private Sub LlenarCombosFiltro(equipos As List(Of GestorInventarios.Entities.Equipo))
        cmbTipoEq.Items.Clear()
        cmbTipoEq.Items.Add("")
        For Each tipo In equipos.Select(Function(e) e.Tipo).Distinct().OrderBy(Function(t) t)
            cmbTipoEq.Items.Add(tipo)
        Next
        cmbTipoEq.SelectedIndex = 0
        cmbDepartamentoEq.Items.Clear()
        cmbDepartamentoEq.Items.Add("")
        Dim departamentos = _departamentoService.GetAllDepartamentos()
        For Each dep In departamentos
            cmbDepartamentoEq.Items.Add(New With {.Id = dep.DepartamentoId, .Nombre = dep.Nombre})
        Next
        cmbDepartamentoEq.DisplayMember = "Nombre"
        cmbDepartamentoEq.ValueMember = "Id"
        cmbDepartamentoEq.SelectedIndex = 0
        cmbEstadoEq.Items.Clear()
        cmbEstadoEq.Items.Add("")
        cmbEstadoEq.Items.Add("Activo")
        cmbEstadoEq.Items.Add("Inactivo")
        cmbEstadoEq.SelectedIndex = 0
    End Sub

    Private Sub SetEquiposDataSource(equipos As List(Of GestorInventarios.Entities.Equipo))
        dgvEquipos.DataSource = Nothing
        dgvEquipos.Columns.Clear()
        dgvEquipos.DataSource = equipos
        ' Eliminar columna duplicada "Departamento" si existe
        Dim countDep = dgvEquipos.Columns.Cast(Of DataGridViewColumn)().Count(Function(c) c.HeaderText = "Departamento")
        If countDep > 1 Then
            ' Elimina la segunda columna con HeaderText "Departamento"
            Dim found = False
            For i = 0 To dgvEquipos.Columns.Count - 1
                If dgvEquipos.Columns(i).HeaderText = "Departamento" Then
                    If found = False Then
                        found = True ' Deja la primera
                    Else
                        dgvEquipos.Columns.RemoveAt(i)
                        Exit For
                    End If
                End If
            Next
        End If
        ' Asegurar que la columna Activo sea tipo CheckBox y editable
        If dgvEquipos.Columns.Contains("Activo") Then
            dgvEquipos.Columns("Activo").ReadOnly = False
            If Not TypeOf dgvEquipos.Columns("Activo") Is DataGridViewCheckBoxColumn Then
                Dim colIndex = dgvEquipos.Columns("Activo").Index
                dgvEquipos.Columns.Remove("Activo")
                Dim chkCol As New DataGridViewCheckBoxColumn()
                chkCol.Name = "Activo"
                chkCol.HeaderText = "Activo"
                chkCol.DataPropertyName = "Activo"
                chkCol.ReadOnly = False
                dgvEquipos.Columns.Insert(colIndex, chkCol)
            End If
        End If
        ' El resto de columnas solo lectura
        For Each col As DataGridViewColumn In dgvEquipos.Columns
            If col.Name <> "Activo" AndAlso col.Name <> "Editar" Then
                col.ReadOnly = True
            End If
        Next
        ' Quitar columna Editar si existe
        If dgvEquipos.Columns.Contains("Editar") Then
            dgvEquipos.Columns.Remove("Editar")
        End If
        ' Agregar columna de botón Editar en la penúltima posición
        Dim editarCol As New DataGridViewButtonColumn()
        editarCol.Name = "Editar"
        editarCol.HeaderText = "Editar"
        editarCol.Text = "Editar"
        editarCol.UseColumnTextForButtonValue = True
        editarCol.Width = 70
        Dim penultima = dgvEquipos.Columns.Count - 1
        If penultima < 0 Then penultima = 0
        dgvEquipos.Columns.Insert(penultima, editarCol)
        ' Agregar el evento CellFormatting para mostrar el nombre del departamento
        RemoveHandler dgvEquipos.CellFormatting, AddressOf DgvEquipos_CellFormatting
        AddHandler dgvEquipos.CellFormatting, AddressOf DgvEquipos_CellFormatting
    End Sub

    ' Filtrar equipos según los filtros seleccionados
    Private Sub FiltrarEquipos(sender As Object, e As EventArgs)
        Dim equiposFiltrados = _equiposOriginales
        ' Filtro por tipo
        If cmbTipoEq.SelectedIndex > 0 Then
            Dim tipoSeleccionado = cmbTipoEq.SelectedItem.ToString()
            equiposFiltrados = equiposFiltrados.Where(Function(eq) eq.Tipo = tipoSeleccionado).ToList()
        End If
        ' Filtro por departamento
        If cmbDepartamentoEq.SelectedIndex > 0 Then
            Dim depSeleccionado = cmbDepartamentoEq.SelectedItem
            Dim depId As Integer = depSeleccionado.Id
            equiposFiltrados = equiposFiltrados.Where(Function(eq) eq.DepartamentoId = depId).ToList()
        End If
        ' Filtro por usuario
        If Not String.IsNullOrWhiteSpace(txtUsuarioEq.Text) Then
            equiposFiltrados = equiposFiltrados.Where(Function(eq) eq.UsuarioCargo IsNot Nothing AndAlso eq.UsuarioCargo.ToLower().Contains(txtUsuarioEq.Text.ToLower())).ToList()
        End If
        ' Filtro por estado
        If cmbEstadoEq.SelectedIndex > 0 Then
            Dim activo = cmbEstadoEq.SelectedItem.ToString() = "Activo"
            equiposFiltrados = equiposFiltrados.Where(Function(eq) eq.Activo = activo).ToList()
        End If
        SetEquiposDataSource(equiposFiltrados)
        If equiposFiltrados.Count = 0 Then
            MessageBox.Show("No hay coincidencias con los datos de búsqueda.", "Sin resultados", MessageBoxButtons.OK, MessageBoxIcon.Information)
        End If
    End Sub

    ' Evento para mostrar el nombre del departamento en la columna DepartamentoId
    Private Sub DgvEquipos_CellFormatting(sender As Object, e As DataGridViewCellFormattingEventArgs)
        If dgvEquipos.Columns(e.ColumnIndex).Name = "DepartamentoId" AndAlso e.Value IsNot Nothing Then
            Dim depId As Integer = CInt(e.Value)
            Dim departamentos = _departamentoService.GetAllDepartamentos()
            Dim dep = departamentos.FirstOrDefault(Function(d) d.DepartamentoId = depId)
            If dep IsNot Nothing Then
                e.Value = dep.Nombre
            End If
        End If
    End Sub

    ' Evento para guardar el cambio de estado Activo con confirmación
    Private Sub DgvEquipos_CellValueChanged(sender As Object, e As DataGridViewCellEventArgs)
        If e.RowIndex >= 0 AndAlso dgvEquipos.Columns(e.ColumnIndex).Name = "Activo" Then
            Dim row = dgvEquipos.Rows(e.RowIndex)
            Dim nombre = row.Cells("Nombre").Value.ToString()
            Dim nuevoEstado = CBool(row.Cells("Activo").Value)
            Dim mensaje = If(nuevoEstado, $"¿Está seguro que desea ACTIVAR el equipo '{nombre}'?", $"¿Está seguro que desea DESACTIVAR el equipo '{nombre}'?")
            If MessageBox.Show(mensaje, "Confirmar", MessageBoxButtons.YesNo, MessageBoxIcon.Question) = DialogResult.Yes Then
                ' Buscar el equipo original por ID en la lista real
                Dim id = CInt(row.Cells("EquipoId").Value)
                Dim equipo = _equiposOriginales.FirstOrDefault(Function(eq) eq.EquipoId = id)
                If equipo IsNot Nothing Then
                    equipo.Activo = nuevoEstado
                    Try
                        _equipoService.UpdateEquipo(equipo)
                    Catch ex As Exception
                        MessageBox.Show("Error al actualizar el estado: " & ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
                        LoadEquipos()
                    End Try
                End If
            Else
                ' Revertir el cambio si el usuario cancela
                RemoveHandler dgvEquipos.CellValueChanged, AddressOf DgvEquipos_CellValueChanged
                row.Cells("Activo").Value = Not nuevoEstado
                AddHandler dgvEquipos.CellValueChanged, AddressOf DgvEquipos_CellValueChanged
            End If
        End If
    End Sub

    ' Para que el cambio de check se dispare inmediatamente
    Private Sub DgvEquipos_CurrentCellDirtyStateChanged(sender As Object, e As EventArgs)
        If dgvEquipos.IsCurrentCellDirty Then
            dgvEquipos.CommitEdit(DataGridViewDataErrorContexts.Commit)
        End If
    End Sub

    ' CARGA Y FILTROS DEPARTAMENTOS
    Private Sub LoadDepartamentos()
        ' Carga todos los departamentos (activos e inactivos) y los muestra en la grilla, junto con el resumen de equipos.
        Dim departamentos = _departamentoService.GetAllDepartamentos(True)
        Dim resumen = _departamentoService.GetResumenEquiposPorDepartamento()
        Dim departamentosConEquipos = departamentos.Select(Function(dep) New With {
            .DepartamentoId = dep.DepartamentoId,
            .Nombre = dep.Nombre,
            .Descripcion = dep.Descripcion,
            .FechaCreacion = dep.FechaCreacion,
            .EquiposAsignados = If(resumen.ContainsKey(dep.Nombre), resumen(dep.Nombre), 0),
            .Activo = dep.Activo
        }).ToList()
        dgvDepartamentos.DataSource = Nothing
        dgvDepartamentos.Columns.Clear()
        dgvDepartamentos.DataSource = departamentosConEquipos
        AgregarColumnasDepartamentosPersonalizadas()
        OrdenarColumnasDepartamentos()
        cmbActivoDep.Items.Clear()
        cmbActivoDep.Items.Add("")
        cmbActivoDep.Items.Add("Activo")
        cmbActivoDep.Items.Add("Inactivo")
        cmbActivoDep.SelectedIndex = 0
    End Sub

    Private Sub FiltrarDepartamentos(sender As Object, e As EventArgs)
        ' Aplica los filtros seleccionados por el usuario sobre la lista de departamentos.
        Dim departamentos = _departamentoService.GetAllDepartamentos(True)
        If Not String.IsNullOrWhiteSpace(txtNombreDep.Text) Then
            departamentos = departamentos.Where(Function(dep) dep.Nombre.ToLower().Contains(txtNombreDep.Text.ToLower())).ToList()
        End If
        If cmbActivoDep.SelectedIndex > 0 Then
            Dim activo = cmbActivoDep.SelectedItem.ToString() = "Activo"
            departamentos = departamentos.Where(Function(dep) dep.Activo = activo).ToList()
        End If
        Dim resumen = _departamentoService.GetResumenEquiposPorDepartamento()
        Dim departamentosConEquipos = departamentos.Select(Function(dep) New With {
            .DepartamentoId = dep.DepartamentoId,
            .Nombre = dep.Nombre,
            .Descripcion = dep.Descripcion,
            .FechaCreacion = dep.FechaCreacion,
            .EquiposAsignados = If(resumen.ContainsKey(dep.Nombre), resumen(dep.Nombre), 0),
            .Activo = dep.Activo
        }).ToList()
        dgvDepartamentos.DataSource = Nothing
        dgvDepartamentos.Columns.Clear()
        dgvDepartamentos.DataSource = departamentosConEquipos
        AgregarColumnasDepartamentosPersonalizadas()
        OrdenarColumnasDepartamentos()
        If departamentosConEquipos.Count = 0 Then
            MessageBox.Show("No hay coincidencias con los datos de búsqueda.", "Sin resultados", MessageBoxButtons.OK, MessageBoxIcon.Information)
        End If
    End Sub

    ' Agrega columnas de botones y checkbox solo si no existen
    Private Sub AgregarColumnasDepartamentosPersonalizadas()
        ' Quitar columna Editar si existe
        If dgvDepartamentos.Columns.Contains("Editar") Then dgvDepartamentos.Columns.Remove("Editar")
        ' Quitar columna Activo si existe
        If dgvDepartamentos.Columns.Contains("Activo") Then dgvDepartamentos.Columns.Remove("Activo")
        ' Agregar columna de botón Editar
        Dim editarCol As New DataGridViewButtonColumn()
        editarCol.Name = "Editar"
        editarCol.HeaderText = "Editar"
        editarCol.Text = "Editar"
        editarCol.UseColumnTextForButtonValue = True
        editarCol.Width = 70
        dgvDepartamentos.Columns.Add(editarCol)
        ' Agregar columna Activo tipo CheckBox
        Dim chkCol As New DataGridViewCheckBoxColumn()
        chkCol.Name = "Activo"
        chkCol.HeaderText = "Activo"
        chkCol.DataPropertyName = "Activo"
        chkCol.ReadOnly = False
        dgvDepartamentos.Columns.Add(chkCol)
    End Sub

    ' Ordena las columnas en el orden solicitado
    Private Sub OrdenarColumnasDepartamentos()
        If dgvDepartamentos.Columns.Contains("DepartamentoId") Then dgvDepartamentos.Columns("DepartamentoId").DisplayIndex = 0
        If dgvDepartamentos.Columns.Contains("Nombre") Then dgvDepartamentos.Columns("Nombre").DisplayIndex = 1
        If dgvDepartamentos.Columns.Contains("Descripcion") Then dgvDepartamentos.Columns("Descripcion").DisplayIndex = 2
        If dgvDepartamentos.Columns.Contains("FechaCreacion") Then dgvDepartamentos.Columns("FechaCreacion").DisplayIndex = 3
        If dgvDepartamentos.Columns.Contains("EquiposAsignados") Then dgvDepartamentos.Columns("EquiposAsignados").DisplayIndex = 4
        If dgvDepartamentos.Columns.Contains("Editar") Then dgvDepartamentos.Columns("Editar").DisplayIndex = 5
        If dgvDepartamentos.Columns.Contains("Activo") Then dgvDepartamentos.Columns("Activo").DisplayIndex = 6
    End Sub

    ' Evento para los botones Editar y Desactivar en la tabla de departamentos
    Private Sub DgvDepartamentos_CellContentClick(sender As Object, e As DataGridViewCellEventArgs)
        ' Maneja el clic en el botón Editar de la grilla de departamentos.
        If e.RowIndex >= 0 Then
            Dim colName = dgvDepartamentos.Columns(e.ColumnIndex).Name
            Dim depId = CInt(dgvDepartamentos.Rows(e.RowIndex).Cells("DepartamentoId").Value)
            Dim dep = _departamentoService.GetDepartamentoById(depId)
            If colName = "Editar" AndAlso dep IsNot Nothing Then
                Dim form As New DepartamentoForm(dep, False)
                If form.ShowDialog() = DialogResult.OK Then
                    LoadDepartamentos()
                End If
            End If
        End If
    End Sub

    ' Evento para el botón Editar en la tabla de equipos
    Private Sub DgvEquipos_CellContentClick(sender As Object, e As DataGridViewCellEventArgs)
        If e.RowIndex >= 0 AndAlso dgvEquipos.Columns(e.ColumnIndex).Name = "Editar" Then
            Dim equipo = CType(dgvEquipos.Rows(e.RowIndex).DataBoundItem, GestorInventarios.Entities.Equipo)
            Dim form As New EquipoForm(equipo, False)
            If form.ShowDialog() = DialogResult.OK Then
                LoadEquipos()
            End If
        End If
    End Sub

    ' Exportar departamentos filtrados a Excel
    Private Sub ExportarExcelDepartamentosFiltrados(sender As Object, e As EventArgs)
        Try
            Using saveDialog As New SaveFileDialog()
                saveDialog.Filter = "Archivos Excel (*.xlsx)|*.xlsx"
                saveDialog.Title = "Exportar a Excel"
                saveDialog.DefaultExt = "xlsx"
                If saveDialog.ShowDialog() = DialogResult.OK Then
                    Dim data As DataTable = GetDataTableFromDGV(dgvDepartamentos)
                    _exportService.ExportToExcel(data, saveDialog.FileName)
                    MessageBox.Show("Archivo exportado correctamente.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information)
                End If
            End Using
        Catch ex As Exception
            MessageBox.Show("Error al exportar: " & ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    ' Exportar departamentos filtrados a PDF
    Private Sub ExportarPDFDepartamentosFiltrados(sender As Object, e As EventArgs)
        Try
            Using saveDialog As New SaveFileDialog()
                saveDialog.Filter = "Archivos PDF (*.pdf)|*.pdf"
                saveDialog.Title = "Exportar a PDF"
                saveDialog.DefaultExt = "pdf"
                If saveDialog.ShowDialog() = DialogResult.OK Then
                    Dim data As DataTable = GetDataTableFromDGV(dgvDepartamentos)
                    _exportService.ExportToPDF(data, saveDialog.FileName, "Reporte de Departamentos")
                    MessageBox.Show("Archivo exportado correctamente.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information)
                End If
            End Using
        Catch ex As Exception
            MessageBox.Show("Error al exportar: " & ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub
End Class