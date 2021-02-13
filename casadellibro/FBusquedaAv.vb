Imports System.Data.OleDb
Public Class FBusquedaAv
    Public conexion As New OleDbConnection("Provider = Microsoft.ACE.OLEDB.12.0; Data Source = Agenda.accdb")
    Public adaptador As New OleDbDataAdapter("Select * from Tabla", conexion)
    Public midataset As New DataSet
    Private Sub FBusquedaAv_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        midataset.Clear()
        'Cargar la memoria cache con datos
        adaptador.Fill(midataset, "Tabla")
        'cargar en el datagridview
        DataGridView1.DataSource = midataset 'la memoria cache que se quiere
        DataGridView1.DataMember = "Tabla" 'indicar cual se quiere

        'Ver el total de los elementos del dataGridView
        Label3.Text = DataGridView1.RowCount - 1
        'cargamos en el combobox los vaslores del campo unico
        ComboBox1.DataSource = midataset.Tables("Tabla")
        ComboBox1.DisplayMember = "apellidos"
    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        Me.Close()
        FMenu.Show()

    End Sub

    Private Sub ComboBox1_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ComboBox1.SelectedIndexChanged
        'Buscar a través del combobox1
        'se crea un nuevo DataSet (imprescindible)
        Dim ds As New DataSet
        'Buscar por apellidos que es el campo clave
        Dim cb As New OleDbDataAdapter
        Dim comando As New OleDbCommand("Select * from Tabla where apellidos =?", conexion)

        cb.SelectCommand = comando
        'Para realizar la busqueda avanzada por un campo clave unico
        comando.Parameters.Add("@apellidos", OleDbType.VarChar, 15).Value = ComboBox1.Text
        'se limpia el dataset y se crea uno nuevo
        ds.Clear()
        cb.Fill(ds, "Tabla")

        'se carga el datagridView
        DataGridView1.DataSource = ds
        'ver total de elementos del datagridView--- coincidencias
        Label3.Text = DataGridView1.RowCount - 1

    End Sub

    Private Sub TextBox1_TextChanged(sender As Object, e As EventArgs) Handles TextBox1.TextChanged
        'se busca solo con pulsar un caracter, los apellidos 
        Dim comando As New OleDbCommand(("Select * from Tabla where apellidos LIKE '%" & TextBox1.Text & "%'"), conexion)

        adaptador.SelectCommand = comando

        midataset.Clear()
        adaptador.Fill(midataset, "Tabla")

        DataGridView1.DataSource = midataset

        'ver total de elementos del datagridView
        Label3.Text = DataGridView1.RowCount - 1
    End Sub

    Private Sub TextBox2_TextChanged(sender As Object, e As EventArgs) Handles TextBox2.TextChanged
        'Se busca ´solo con pulsar una cadena completa, los apellidos
        Dim comando As New OleDbCommand(("Select * from Tabla where (apellidos LIKE @var)"), conexion)

        adaptador.SelectCommand = comando

        Dim ds As New DataSet

        comando.Parameters.Add("@var", OleDbType.VarChar, 15).Value = TextBox2.Text
        'limpiamos y rellenamos el dataset
        ds.Clear()
        adaptador.Fill(ds, "Tabla")

        DataGridView1.DataSource = ds
        'ver total de elementos del datagridView
        Label3.Text = DataGridView1.RowCount - 1

    End Sub
End Class