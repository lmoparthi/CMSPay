Imports System.Configuration
Imports System.Data.Common
Imports Microsoft.Practices.EnterpriseLibrary.Data
Imports Microsoft.Practices.EnterpriseLibrary.Common.Configuration
Imports Microsoft.Practices.EnterpriseLibrary.Data.Configuration
Imports System.Collections.Generic

Friend Class WCFWrapperDAL

    Private Shared ReadOnly _GetRoleSettingsSyncLock As New Object
    Private Shared _RoleSettingsKP As New Dictionary(Of String, String)
    Private Shared _DefaultSQLInstance As String = Nothing
    Private Shared _DefaultDB2Instance As String = Nothing
    Private Shared _EnvironmentOverride As String = Nothing

    Public Shared Property EnvironmentOverride As String
        Get
            Return _EnvironmentOverride
        End Get
        Set
            Debug.Print($"WCFWrapperDAL.Set.EnvironmentOverride")
            _EnvironmentOverride = Value
        End Set
    End Property

    Public Shared ReadOnly Property DefaultDatabase() As String
        'Returns dataConfiguration defaultDatabase setting modified to include Override when appropriate
        Get
            Dim Settings As DatabaseSettings

            Try

                Using ConfigSource As IConfigurationSource = New SystemConfigurationSource()

                    Settings = DatabaseSettings.GetDatabaseSettings(ConfigSource)

                    Return Settings.DefaultDatabase.Replace(" P ", $" {If(_EnvironmentOverride?.Trim.Length > 0, _EnvironmentOverride, "P")} ").Replace(" SQL ", If(_EnvironmentOverride?.Trim.Length > 0, " SQ1 ", " SQL "))

                End Using

            Catch ex As Exception
                Throw
            End Try

        End Get

    End Property
    Public Shared Function RetrieveRoleSettings(ByVal settingName As String) As Object

        SyncLock _GetRoleSettingsSyncLock
            Try

                If _RoleSettingsKP Is Nothing OrElse Not _RoleSettingsKP.ContainsKey(settingName) Then

                    Dim SETTING_VALUE As String = LoadRoleSettings(settingName).ToString

                    If SETTING_VALUE Is Nothing Then Return Nothing

                    _RoleSettingsKP.Add(settingName, SETTING_VALUE)

                End If

                Return _RoleSettingsKP(settingName)

            Catch ex As Exception
                Throw
            End Try
        End SyncLock

    End Function

    Public Shared Function LoadRoleSettings(ByVal settingName As String) As Object

        Dim DS As DataSet
        Dim DB As Database
        Dim DBCommandWrapper As DbCommand
        Dim SQLCall As String = "FDBMD.RETRIEVE_ROLE_SETTINGS_BY_NAME"

        Try
            DB = CreateDatabase()

            DBCommandWrapper = DB.GetStoredProcCommand(SQLCall)
            DB.AddInParameter(DBCommandWrapper, "@SETTING_NAME", DbType.String, settingName)

            DS = DB.ExecuteDataSet(DBCommandWrapper)

            If DS.Tables.Count > 0 AndAlso DS.Tables(0).Rows.Count > 0 Then
                Return DS.Tables(0).Rows(0)("SETTING_VALUE")
            End If

            Return Nothing

        Catch ex As Exception
            Throw

        End Try
    End Function

    Public Shared Function CreateDatabase() As Database

        If String.IsNullOrWhiteSpace(_DefaultDB2Instance) Then
            Return CreateDatabase(Nothing)
        Else
            Return CreateDatabase(_DefaultDB2Instance)
        End If

    End Function

    Public Shared Function CreateDatabase(ByVal dbConnection As String) As Database

        Dim ReplaceUserID As Boolean = True
        Dim DBConnectionA As String()
        Dim ConnectionStringName As String
        Dim CSS As ConnectionStringSettings
        Dim ConnString As String

        Dim ConnectionStringParamsA() As String
        Dim UserName As String = Nothing
        Dim Password As String = Nothing

        Dim DBProviderFactory As DbProviderFactory
        Dim Database As Database

        Try

            ConnectionStringName = DefaultDatabase

            If String.IsNullOrWhiteSpace(dbConnection) Then
                dbConnection = ConnectionStringName.Trim
            End If

            DBConnectionA = dbConnection.Split(New Char() {CChar(";")}, StringSplitOptions.RemoveEmptyEntries)

            'Override only works to switch production to QA.
            dbConnection = dbConnection.Replace(" P ", $" {If(_EnvironmentOverride?.Trim.Length > 0, _EnvironmentOverride, "P")} ").Replace(" SQL ", If(_EnvironmentOverride?.Trim.Length > 0, " SQ1 ", " SQL "))

            Debug.Print($"CMSDALCommon.CreateDatabase({dbConnection})")

            CSS = ConfigurationManager.ConnectionStrings(dbConnection)

            If CSS Is Nothing Then
                CSS = ConfigurationManager.ConnectionStrings($"{dbConnection} Database Instance")

                If CSS Is Nothing Then
                    Throw New ApplicationException($"No connection string found for {dbConnection} in Config file")
                Else
                    dbConnection = $"{dbConnection} Database Instance"
                End If
            End If

            ConnString = CSS.ConnectionString.ToString.Replace("DB2PL", $"DB2{If(_EnvironmentOverride?.Trim.Length > 0, _EnvironmentOverride, "P")}L").Replace("SQL", $"SQ{If(_EnvironmentOverride?.Trim.Length > 0, "1", "L")}")

            If DBConnectionA.Length > 1 Then ConnString = ConnString.Replace("$mdb$", DBConnectionA(1)).Replace("$MDB$", DBConnectionA(1))

            If Not CSS.ConnectionString.ToLower.Contains("kerberos") AndAlso (dbConnection.ToLower.Contains("ddtek") OrElse dbConnection.ToLower.Contains("accdb") OrElse dbConnection.ToLower.Contains("mdb") OrElse dbConnection.ToLower.Contains("oledb")) Then 'DB2
                ConnectionStringParamsA = ConnString.Split(New Char() {CChar(";")}, StringSplitOptions.RemoveEmptyEntries)

                For x As Integer = 0 To ConnectionStringParamsA.Length - 1

                    If ConnectionStringParamsA(x).Contains("user id=") Then
                        UserName = ConnectionStringParamsA(x)
                    End If
                    If ConnectionStringParamsA(x).ToLower.Contains("password=") OrElse ConnectionStringParamsA(x).Contains("Database Password=") Then
                        Password = ConnectionStringParamsA(x)
                    End If

                    If Not String.IsNullOrWhiteSpace(UserName) AndAlso Not String.IsNullOrWhiteSpace(Password) Then Exit For

                Next

                If ConfigurationManager.AppSettings("ReplaceUserID") IsNot Nothing Then
                    ReplaceUserID = CBool(ConfigurationManager.AppSettings("ReplaceUserID"))
                End If

                If ReplaceUserID Then
                    If UserName.Trim.Length < 1 Then
                        ConnString &= ";user id=" & UFCWGeneral.ComputerName
                    Else
                        ConnString = ConnString.Replace(UserName, "user id=" & UFCWGeneral.ComputerName)
                    End If
                End If

                If Password.Trim.Length < 1 AndAlso Not dbConnection.ToUpper.Contains("ACCDB") AndAlso Not dbConnection.ToUpper.Contains("MDB") AndAlso Not dbConnection.ToUpper.Contains("OLEDB") Then
                    ConnString &= ";password=" & UFCWCryptor.Password("DB2")
                Else
                    Select Case True
                        Case ((dbConnection.ToUpper.Contains("MDB") OrElse dbConnection.ToUpper.Contains("ACCDB")) AndAlso Password.ToUpper.Contains("MDB")) OrElse (dbConnection.ToUpper.Contains("OLEDB") AndAlso Password.ToUpper.Contains("OLEDB"))
                            ConnString = ConnString.Replace(Password, "Jet OLEDB:Database Password=" & UFCWCryptor.Password("MDB"))
                        Case dbConnection.ToUpper.Contains("ACCDB"), dbConnection.ToUpper.Contains("MDB"), dbConnection.ToUpper.Contains("OLEDB")
                            'do nothing
                        Case Else
                            ConnString = ConnString.Replace(Password, "password=" & UFCWCryptor.Password("DB2"))
                    End Select
                End If

            End If

            DBProviderFactory = DbProviderFactories.GetFactory(CSS.ProviderName)
            Database = New GenericDatabase(ConnString, DBProviderFactory)

            Return Database

        Catch ex As Exception
            Throw
        Finally

        End Try

    End Function

End Class
