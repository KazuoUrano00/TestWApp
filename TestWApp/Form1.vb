Imports System.Data.Common
Imports System.Data.OleDb
Imports System.Data.SqlClient

Public Class Form1

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        Dim prov As DbProviderFactory = Nothing
        Dim con As DbConnection = Nothing
        Dim cmd As DbCommand = Nothing

        'False:Oracle接続、True:SQLServer接続
        Dim isDbSQLServer As Boolean = True
        Dim strProvider As String
        Dim strConnection As String

        'DbProviderFactoryオブジェクトの生成
        If isDbSQLServer Then
            'SQLServer接続の場合
            strProvider = "System.Data.SqlClient"
            strConnection = String.Format("Server=DESKTOP-HMSGQGP\SQLEXPRESS;Database=Test;Integrated Security=True;",
            "192.168.1.1",
            "sqlDB")

        Else
            'Oracle接続の場合
            strProvider = "Oracle.DataAccess.Client"
            strConnection = String.Format("Data Source={0};User Id={1};Password={2};",
            "oraDB",
            "userid",
            "userpw")
        End If

        prov = DbProviderFactories.GetFactory(strProvider)

        'DbConnectionオブジェクトの生成、及び、オープン
        con = prov.CreateConnection()
        con.ConnectionString = strConnection
        con.Open()
        '---------

        'DBCommandオブジェクトの生成、及び、コネクションの設定
        cmd = prov.CreateCommand()
        cmd.Connection = con

        Dim strSql As New System.Text.StringBuilder()
        Dim param As DbParameter = Nothing
        Dim adapter As DbDataAdapter = Nothing
        Dim ds As New DataTable()

        'SQL 文の生成
        strSql.AppendLine("SELECT * ")
        strSql.AppendLine("FROM Students")

        '実行する SQL 文を設定
        cmd.CommandText = strSql.ToString

        'パラメーターの初期化
        cmd.Parameters.Clear()

        '変換パラメーターの設定
        'param = prov.CreateParameter()
        'param.DbType = DbType.Int32
        'param.ParameterName = "PERSON_NUM"
        'param.Value = 5
        'cmd.Parameters.Add(param)
        '
        'param = prov.CreateParameter()
        'param.DbType = DbType.String
        'param.ParameterName = "DEPT_ID_STR"
        'param.Value = "A100-01"
        'cmd.Parameters.Add(param)

        'アダプターの生成、及び、SQL文の発行
        adapter = prov.CreateDataAdapter()
        adapter.SelectCommand = cmd
        adapter.Fill(ds)

        MsgBox(ds.Rows.Count)

        DataGridView1.Columns.Add("StudentID", "StudentID")
        DataGridView1.Columns.Add("FirstName", "FirstName")
        DataGridView1.Columns.Add("LastName", "LastName")
        DataGridView1.Columns.Add("Birthday", "Birthday")
        DataGridView1.Columns.Add("Gender", "Gender")

        For Each row As DataRow In ds.Rows
            DataGridView1.Rows.Add(row(0), row(1), row(2), row(3), row(4))
        Next

        ' DB接続を閉じる
        If Not con Is Nothing Then
            con.Close()
        End If

    End Sub

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        Dim strProvider As String
        Dim strConnection As String
        Dim prov As DbProviderFactory = Nothing
        Dim con As DbConnection = Nothing
        Dim cmd As SqlCommand = Nothing
        Dim strSql As New System.Text.StringBuilder()
        Dim adapter2 As DbDataAdapter = Nothing
        Dim ds2 As New DataTable()

        'SQLServer接続の場合
        strProvider = "System.Data.SqlClient"
        strConnection = String.Format("Server=DESKTOP-HMSGQGP\SQLEXPRESS;Database=Test;Integrated Security=True;",
            "192.168.1.1",
            "sqlDB")

        prov = DbProviderFactories.GetFactory(strProvider)

        'DbConnectionオブジェクトの生成、及び、オープン
        con = prov.CreateConnection()
        con.ConnectionString = strConnection
        con.Open()

        'SQL 文の生成
        strSql.AppendLine("SELECT * ")
        strSql.AppendLine("FROM Students")

        'DBCommandオブジェクトの生成、及び、コネクションの設定
        cmd = prov.CreateCommand()
        cmd.Connection = con

        '実行する SQL 文を設定
        cmd.CommandText = strSql.ToString

        'パラメーターの初期化
        cmd.Parameters.Clear()

        'アダプターの生成、及び、SQL文の発行
        adapter2 = prov.CreateDataAdapter()
        adapter2.SelectCommand = cmd
        adapter2.Fill(ds2)

        ''実行する SQL 文を設定
        'cmd.CommandText = strSql.ToString

        ' SQL文の設定
        cmd.CommandText = "INSERT INTO Students (StudentID,FirstName,LastName,Birthday,Gender) VALUES(@temp0,@temp1,@temp2,@temp3,@temp4)"

        ' パラメータの設定
        cmd.Parameters.Add(New SqlParameter("@temp0", SqlDbType.Int)).Value = ds2.Rows.Count + 1
        cmd.Parameters.Add(New SqlParameter("@temp1", SqlDbType.NChar)).Value = TextBox1.Text
        cmd.Parameters.Add(New SqlParameter("@temp2", SqlDbType.NChar)).Value = TextBox2.Text
        cmd.Parameters.Add(New SqlParameter("@temp3", SqlDbType.Date)).Value = TextBox3.Text
        cmd.Parameters.Add(New SqlParameter("@temp4", SqlDbType.NChar)).Value = TextBox4.Text

        cmd.ExecuteReader()

        MsgBox("追加成功")

        ' DB接続を閉じる
        If Not con Is Nothing Then
            con.Close()
        End If
    End Sub

    Private Sub Button3_Click(sender As Object, e As EventArgs) Handles Button3.Click
        Dim strProvider As String
        Dim strConnection As String
        Dim prov As DbProviderFactory = Nothing
        Dim con As DbConnection = Nothing
        Dim cmd As SqlCommand = Nothing
        Dim count As Integer

        'SQLServer接続の場合
        strProvider = "System.Data.SqlClient"
        strConnection = String.Format("Server=DESKTOP-HMSGQGP\SQLEXPRESS;Database=Test;Integrated Security=True;",
            "192.168.1.1",
            "sqlDB")

        prov = DbProviderFactories.GetFactory(strProvider)

        'DbConnectionオブジェクトの生成、及び、オープン
        con = prov.CreateConnection()
        con.ConnectionString = strConnection
        con.Open()

        ' コネクションの設定
        cmd = prov.CreateCommand()
        cmd.Connection = con

        ' SQL文の設定
        cmd.CommandText = "UPDATE Students "
        cmd.CommandText &= "SET FirstName =@temp1, "
        cmd.CommandText &= "LastName =@temp2, "
        cmd.CommandText &= "Birthday =@temp3, "
        cmd.CommandText &= "Gender =@temp4 "
        cmd.CommandText &= "WHERE StudentID =@temp0"

        ' パラメータの設定
        cmd.Parameters.Add(New SqlParameter("@temp0", SqlDbType.Int)).Value = TextBox5.Text
        cmd.Parameters.Add(New SqlParameter("@temp1", SqlDbType.NChar)).Value = TextBox1.Text
        cmd.Parameters.Add(New SqlParameter("@temp2", SqlDbType.NChar)).Value = TextBox2.Text
        cmd.Parameters.Add(New SqlParameter("@temp3", SqlDbType.Date)).Value = TextBox3.Text
        cmd.Parameters.Add(New SqlParameter("@temp4", SqlDbType.NChar)).Value = TextBox4.Text

        count = cmd.ExecuteNonQuery()

        If count > 0 Then
            ' 更新成功
            MsgBox("更新成功")
        Else
            ' 更新するレコードがなかった時の処理
            MsgBox("更新なし")
        End If

        ' DB接続を閉じる
        If Not con Is Nothing Then
            con.Close()
        End If

    End Sub

    Private Sub Button4_Click(sender As Object, e As EventArgs) Handles Button4.Click
        Dim strProvider As String
        Dim strConnection As String
        Dim prov As DbProviderFactory = Nothing
        Dim con As DbConnection = Nothing
        Dim cmd As SqlCommand = Nothing
        Dim count As Integer

        'SQLServer接続の場合
        strProvider = "System.Data.SqlClient"
        strConnection = String.Format("Server=DESKTOP-HMSGQGP\SQLEXPRESS;Database=Test;Integrated Security=True;",
            "192.168.1.1",
            "sqlDB")

        prov = DbProviderFactories.GetFactory(strProvider)

        'DbConnectionオブジェクトの生成、及び、オープン
        con = prov.CreateConnection()
        con.ConnectionString = strConnection
        con.Open()

        ' コネクションの設定
        cmd = prov.CreateCommand()
        cmd.Connection = con

        ' SQL文の設定
        cmd.CommandText = "DELETE FROM Students "
        cmd.CommandText &= "WHERE StudentID=@temp0 "

        ' SQL文パラメータの設定
        cmd.Parameters.Add(New SqlParameter("@temp0", SqlDbType.Int)).Value = TextBox5.Text

        count = cmd.ExecuteNonQuery()
        If count > 0 Then
            ' レコードの削除が成功した時の処理
            MsgBox("削除成功")
        Else
            ' 削除するレコードがなかった時の処理
            MsgBox("削除なし")
        End If
    End Sub

End Class
