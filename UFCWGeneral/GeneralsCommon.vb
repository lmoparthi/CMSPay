
Option Infer On
Option Strict On

Imports System.Security.Principal
Imports System.Configuration
Imports System.Drawing
Imports System.Drawing.Imaging
Imports System.Globalization
Imports System.IO
Imports System.Text
Imports System.Text.RegularExpressions
Imports System.Threading
Imports System.Windows.Forms
Imports Microsoft.Win32
Imports System.DirectoryServices.AccountManagement

Partial Class UFCWGeneral

    Private Shared ReadOnly _GetUniqueKey As New Object

    Private Shared ReadOnly _WindowsUserID As WindowsIdentity = WindowsIdentity.GetCurrent()
    Private Shared ReadOnly _WindowsPrincipalForID As New WindowsPrincipal(_WindowsUserID)
    Private Shared ReadOnly _ComputerName As String = SystemInformation.ComputerName
    Private Shared ReadOnly _StartupPath As String = Application.StartupPath

    Private Sub New()

    End Sub

#Region "Shared Properties"
    Public Shared Property StackLevels As Integer = CInt(If(System.Configuration.ConfigurationManager.AppSettings("StackLevels") Is Nothing, 0, CInt(System.Configuration.ConfigurationManager.AppSettings("StackLevels"))))
    Public Shared ReadOnly Mode As String = If(ConfigurationManager.AppSettings("MODE")?.ToString, "")


    Public Shared ReadOnly Property MainAPPKEY As String
        Get
            Return CStr(If(ConfigurationManager.AppSettings("APPKEY") Is Nothing, $"UFCW\CMS{Mode}\", ConfigurationManager.AppSettings("APPKEY").ToString.Replace("#Mode#", Mode)))
        End Get
    End Property
    Public Shared ReadOnly Property CustomerServiceAPPKEY As String
        Get
            Return CStr(If(ConfigurationManager.AppSettings("APPKEY") Is Nothing, $"UFCW\CS{Mode}\", ConfigurationManager.AppSettings("APPKEY").ToString.Replace("#Mode#", Mode)))
        End Get
    End Property
    Public Shared ReadOnly Property WorkflowAPPKEY As String
        Get
            Return CStr(If(ConfigurationManager.AppSettings("APPKEY") Is Nothing, $"UFCW\WorkFlow{Mode}\", ConfigurationManager.AppSettings("APPKEY").ToString.Replace("#Mode#", Mode)))
        End Get
    End Property

    Public Shared ReadOnly Property NowDate As Date
        Get
            Return Date.Now.AddDays(CInt(IIf(System.Configuration.ConfigurationManager.AppSettings("AddDaysToCurrentDate") Is Nothing, 0, CInt(System.Configuration.ConfigurationManager.AppSettings("AddDaysToCurrentDate")))))
        End Get
    End Property

    Public Shared ReadOnly Property StartupPath As String
        Get
            Return _StartupPath
        End Get
    End Property

    Public Shared ReadOnly Property WindowsPrincipalForID As WindowsPrincipal
        Get
            Return _WindowsPrincipalForID
        End Get
    End Property

    Public Shared ReadOnly Property WindowsUserID As WindowsIdentity
        Get
            Return _WindowsUserID
        End Get
    End Property

    Public Shared ReadOnly Property UserName As String
        Get
            Using ctx As New PrincipalContext(ContextType.Domain)

                ' Find the currently logged on user (or replace Environment.UserName with a specific username)
                Dim user As UserPrincipal = UserPrincipal.FindByIdentity(ctx, Environment.UserName)

                If user IsNot Nothing Then
                    Return user.DisplayName
                Else
                    Return Nothing
                End If

            End Using

        End Get
    End Property

    Public Shared ReadOnly Property ComputerName As String
        Get
            Return _ComputerName
        End Get
    End Property

#End Region

#Region "Shared Methods"

    Public Shared Function ClipBoardSSNCleaner() As Boolean

        'If (e.KeyCode = Keys.V AndAlso e.Modifiers = Keys.Control) OrElse (e.KeyCode = Keys.Insert AndAlso e.Modifiers = Keys.Shift) Then
        'End If

        Try
            'removes non numeric, with the exception of dashes(-) if length is 11 characters
            Return PatternMatcher.ClipBoardCleaner(New String() {If(Clipboard.GetText.Trim.Length = 11, "[^0-9]|\-", "[^0-9]")})

        Catch ex As Exception
            Throw
        End Try

    End Function
    Public Shared Function ClipboardNumberTrimmer() As Boolean

        Try
            'removes non numeric, then leading zeroes
            Return PatternMatcher.ClipBoardCleaner(New String() {"[^0-9]", "^0*"})

        Catch ex As Exception
            Throw
        End Try
    End Function

    Public Shared Function ClipBoardFIDCleaner() As Boolean
        Try
            'removes non numeric, then leading zeroes
            Return PatternMatcher.ClipBoardCleaner(New String() {"[^0-9]", "^0*"})

        Catch ex As Exception
            Throw
        End Try

    End Function

    'Public Shared Sub SSNFormattedClipBoard()

    '    Dim ClipText As String

    '    Try


    '        ClipText = Clipboard.GetText

    '        If ClipText.Length < 20 Then
    '            For X As Integer = 1 To ClipText.Length
    '                ClipText = Regex.Replace(ClipText, "[^0-9|\-]", String.Empty, RegexOptions.IgnoreCase)
    '            Next

    '            If Clipboard.GetText <> ClipText Then
    '                If ClipText.Length > 0 Then
    '                    Clipboard.SetText(ClipText)
    '                Else
    '                    Clipboard.Clear()
    '                End If
    '            End If
    '        End If

    '    Catch ex As Exception
    '        Throw
    '    End Try

    'End Sub
    'Public Shared Sub NumberTrimmedClipboard()

    '    Dim ReturnText As String
    '    Dim ClipText As String
    '    Dim Reg As New Regex("[^0-9]") 'restrict to numbers only

    '    Try

    '        ClipText = Clipboard.GetText

    '        If ClipText.Length < 20 Then
    '            ReturnText = Reg.Replace(ClipText, String.Empty)
    '            Reg = New Regex("^0*") 'remove leading zeroes
    '            ReturnText = Reg.Replace(ReturnText, String.Empty)

    '            If ClipText <> ReturnText Then
    '                If ReturnText.Length > 0 Then
    '                    Clipboard.SetText(ReturnText)
    '                Else
    '                    Clipboard.Clear()
    '                End If
    '            End If
    '        End If

    '    Catch ex As Exception
    '        Throw
    '    End Try
    'End Sub

    Public Shared Function GetUniqueKey() As String

        SyncLock (_GetUniqueKey)

            Dim MaxSize As Integer = 8
            Dim MinSize As Integer = 5
            Dim Chars(62) As Char
            Dim Size As Integer = MaxSize
            Dim DataArray(1) As Byte
            Dim Crypto As System.Security.Cryptography.RNGCryptoServiceProvider
            Dim A As String = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890"

            Try

                Chars = A.ToCharArray()
                Crypto = New System.Security.Cryptography.RNGCryptoServiceProvider()

                Crypto.GetNonZeroBytes(DataArray)
                Size = MaxSize

                DataArray = New Byte(Size - 1) {}

                Crypto.GetNonZeroBytes(DataArray)

                Dim SBResult As New StringBuilder(Size)
                For Each B As Byte In DataArray
                    SBResult.Append(B)
                Next

                Return SBResult.ToString

            Catch ex As Exception
                Throw
            Finally
                If Crypto IsNot Nothing Then Crypto.Dispose()
                Crypto = Nothing
            End Try

        End SyncLock

    End Function

    Public Shared Function MonthEndDate(ByVal originalDate As Date) As Date

        If originalDate.Year = 9999 Then Return CDate("999-12-31")

        Return originalDate.Date.AddDays(-(originalDate.Day - 1)).AddMonths(1).AddDays(-1)

    End Function

    Public Shared Function ValidateSSN(ssn As String) As Boolean

        Dim EntryOkRegex As New Regex("(?!(\d){3}(-| |)\1{2}\2\1{4})(?!666|000|9\d{2})(\b\d{3}(-| |)(?!00)\d{2}\4(?!0{4})\d{4}\b)")

        If ssn.Trim.Length > 0 AndAlso EntryOkRegex.IsMatch(ssn) Then Return True

        Return False

    End Function

    Public Shared Function ValidateDate(ByVal value As Object, Optional ByVal fieldname As String = Nothing) As Date?


        Dim DisplayDate As Date? 'only nullable date can use method HasValue
        Dim TestValue As Object
        Dim DTFI As DateTimeFormatInfo = CultureInfo.CreateSpecificCulture("en-US").DateTimeFormat

        Dim DateTimeFormats() As String = {"M/d/yyyy h:m:ss tt", "M/d/yyyy h:m:ss.ffffff", "M/d/yyyy HH:m:ss tt", "M/d/yyyy HH:m:ss.ffffff", "yyyy-MM-dd HH:mm:ss.ffffff"}
        Dim DateFormats() As String = {"yyyyMMdd", "MMdd", "MMddyy", "MMddyyyy", "M/d/yyyy", "M/d/yy", "yyyy-M-d", "M-d-yyyy", "M-d-yy", "M/d/yyyy h:m:ss tt", "M/d/yyyy h:m:ss.ffffff", "M/d/yyyy HH:m:ss tt", "M/d/yyyy HH:m:ss.ffffff", "yyyy-MM-dd HH:mm:ss.ffffff"}

        Try

            DTFI.DateSeparator = "-"
            DTFI.ShortDatePattern = "MM-dd-yyyy"

            'TestValue = CType(New Date, Object)
            'TestValue = CType(New Date?, Object)
            'TestValue = CType(CType(Nothing, Date), Object)
            'TestValue = CType(CType(Nothing, Date?), Object)

            TestValue = value

            If TypeOf (TestValue) Is Date Then 'only an actual date field or nullable date field with a value pass this test

                If TestValue Is DBNull.Value Then Return CType(Nothing, Date?)
                If CDate(TestValue) = Date.MinValue Then Return CType(Nothing, Date?)

                Try
                    DisplayDate = Date.ParseExact(TestValue.ToString.Trim, DateFormats, New System.Globalization.CultureInfo("en-US"), System.Globalization.DateTimeStyles.None)
                    DisplayDate = DirectCast(TestValue, Date?) 'Parse loses timestamp but we now that the original value will cast correctly

                Catch ex As FormatException '
                    DisplayDate = DateTime.ParseExact(TestValue.ToString.Trim, DateTimeFormats, New System.Globalization.CultureInfo("en-US"), System.Globalization.DateTimeStyles.None)
                End Try

                Return DisplayDate 'return original value that was confirmed to be a valid date

            ElseIf TypeOf (TestValue) Is String Then

                If TestValue Is DBNull.Value Then Return CType(Nothing, Date?)
                If TestValue Is Nothing OrElse TestValue.ToString.Trim.Length = 0 Then Return CType(Nothing, Date?)

                Try
                    DisplayDate = Date.ParseExact(TestValue.ToString.Trim, DateFormats, New System.Globalization.CultureInfo("en-US"), System.Globalization.DateTimeStyles.None)
                Catch ex As FormatException '
                    DisplayDate = DateTime.ParseExact(TestValue.ToString.Trim, DateTimeFormats, New System.Globalization.CultureInfo("en-US"), System.Globalization.DateTimeStyles.None)
                End Try

                Return DisplayDate 'return original value that was confirmed to be a valid date

            Else 'must be uninitialized nullable date or nullable date set to nothing

                If value IsNot Nothing AndAlso value.ToString.Length > 0 Then Throw New ArgumentException("Date test failed.", If(fieldname IsNot Nothing AndAlso fieldname.Length > 0, fieldname & " -> ", "") & value.ToString)

                Return CType(Nothing, Date?)

            End If

        Catch ex As FormatException

            Try 'test if date routine can convert submitted 
                DisplayDate = CDate(value)

                Return DisplayDate

            Catch exInner As Exception

                If fieldname IsNot Nothing AndAlso fieldname.Length > 0 Then
                    Throw New ArgumentException("Date test failed.", If(fieldname IsNot Nothing AndAlso fieldname.Length > 0, fieldname & " -> ", "") & value.ToString)
                Else
                    Return CType(Nothing, Date?)
                End If
            End Try

        Catch ex As Exception
            Throw
        End Try

    End Function

    Public Shared Function ValidateDateOnly(value As Object, Optional fieldname As String = Nothing) As Date?
        'rejects strings that includes time
        Dim Formats() As String = {"yyyyMMdd", "MMdd", "MMddyy", "MMddyyyy", "M/d/yyyy", "M/d/yy", "MM/dd/yy", "M/dd/yy", "yyyy-M-d", "M-d-yyyy", "M-d-yy"}

        Try

            If value Is DBNull.Value Then Return Nothing
            If value Is Nothing OrElse value.ToString.Trim.Length < 1 Then Throw New FormatException 'The = Nothing is not the correct use of Nothing but if a date is set nothing and looks like #12:00:00 AM# it compares as equal due to it being equal to date.min

            Return Date.ParseExact(value.ToString.Trim, Formats, New System.Globalization.CultureInfo("en-US"), System.Globalization.DateTimeStyles.None)

        Catch ex As FormatException

            Return Nothing

        Catch ex As Exception
            Throw
        End Try

    End Function

    Public Shared Function ValidateDateTime(value As Object, Optional fieldname As String = Nothing) As Date?
        'Allows Time to be included
        Dim DisplayDate As Date?
        Dim Formats() As String = {"M-d-yyyy h:m:ss tt", "M-d-yyyy h:m:ss.ffffff", "M-d-yy h:m:ss tt", "M-d-yy h:m:ss.ffffff", "M/d/yy h:m:ss tt", "M/d/yy h:m:ss.ffffff", "M/d/yyyy h:m:ss tt", "M/d/yyyy h:m:ss.ffffff", "yyyy-MM-dd HH:mm:ss.ffffff"}

        Try

            If value Is DBNull.Value Then Return Nothing
            If value Is Nothing OrElse value.ToString.Trim.Length < 1 Then Throw New FormatException 'The = Nothing is not the correct use of Nothing but if a date is set nothing and looks like #12:00:00 AM# it compares as equal due to it being equal to date.min

            'check if provided value conforms to valid format and assigned as default format
            DisplayDate = DateTime.ParseExact(value.ToString.Trim, Formats, New System.Globalization.CultureInfo("en-US"), System.Globalization.DateTimeStyles.None)

            Try
                DisplayDate = CDate(value) 'try and cast entered format as date first, else return parseexact format
            Catch ex As Exception
            End Try

            Return DisplayDate 'return original value that was confirmed to be a valid date

        Catch ex As FormatException

            Return Nothing

        Catch ex As Exception
            Throw
        End Try

    End Function

    Public Shared Function ApplyDateFormatting(toBeFormatted As String, Optional addBraces As Boolean = False) As String

        Try
            If addBraces Then
                toBeFormatted = "[" & toBeFormatted & "]"
            End If

            Dim LastFoundBegin As Integer
            Dim LastFoundEnd As Integer = -1

            Do While True
                Dim ModifiedDate As Date = Now

                If toBeFormatted IsNot Nothing AndAlso toBeFormatted.IndexOf("[") > LastFoundEnd Then

                    LastFoundBegin = toBeFormatted.IndexOf("[")
                    LastFoundEnd = toBeFormatted.IndexOf("]")

                    Dim FormatMaskAndModifier As String = toBeFormatted.Substring(LastFoundBegin, LastFoundEnd - LastFoundBegin + 1)
                    Dim ModifierRegex As New Regex("[-+]\d") 'Look for modifiers to todays date e.g -1 +1 etc

                    Dim DateModifier As Match = ModifierRegex.Match(FormatMaskAndModifier.ToUpper)

                    Dim ActualFormatMask As String

                    If DateModifier.Length > 0 Then

                        Dim PreModifierLast As String = FormatMaskAndModifier.Substring(DateModifier.Index - 1, 1)

                        Select Case PreModifierLast.ToUpper.ToString
                            Case "M"
                                ModifiedDate = ModifiedDate.AddMonths(CInt(DateModifier.ToString))
                            Case "Y"
                                ModifiedDate = ModifiedDate.AddYears(CInt(DateModifier.ToString))
                            Case "D"
                                ModifiedDate = ModifiedDate.AddDays(CInt(DateModifier.ToString))
                        End Select

                        ActualFormatMask = FormatMaskAndModifier.Replace(DateModifier.ToString, "")
                    Else
                        ActualFormatMask = FormatMaskAndModifier
                    End If

                    Dim FinalFormatted As String = Format(ModifiedDate, ActualFormatMask).Replace("[", "").Replace("]", "")

                    Dim FormatRegex As New Regex(FormatMaskAndModifier.Replace("+", "\+").Replace("-", "\-").Replace("[", "\[").Replace("]", "\]"))

                    toBeFormatted = FormatRegex.Replace(toBeFormatted, FinalFormatted, 1)

                Else
                    Exit Do
                End If

            Loop

            Return toBeFormatted

        Catch ex As Exception
            Throw
        End Try

    End Function

    Public Shared Function MeasureWidthInChars(ByVal controlWidth As Integer, ByVal fontName As String, ByVal fontSize As Single) As Integer

        Try

            ' Compute the visible characters for a control in the given font 
            Using B As New Bitmap(1, 1, PixelFormat.Format32bppArgb)
                Using G As Graphics = Graphics.FromImage(B)
                    Using F As New Font(fontName, fontSize)
                        Dim CharacterSize As SizeF = G.MeasureString(CChar("A"), F)
                        Return CInt(controlWidth \ CInt(CharacterSize.Width))
                    End Using
                End Using
            End Using

        Catch ex As Exception
            Throw
        Finally
        End Try

    End Function

    Public Shared Function MeasureWidthinPixels(ByVal textWidth As Integer, ByVal fontName As String, ByVal fontSize As Single) As Integer

        Try

            Dim BannerText As String = New String(CChar("A"), textWidth)

            ' Compute the string dimensions in the given font 
            Using B As Bitmap = New Bitmap(1, 1, PixelFormat.Format32bppArgb)
                Using G As Graphics = Graphics.FromImage(B)
                    Using F As Font = New Font(fontName, fontSize)
                        Dim stringSize As SizeF = G.MeasureString(BannerText, F)
                        Return CInt(stringSize.Width)
                    End Using
                End Using
            End Using

        Catch ex As Exception
            Throw
        Finally
        End Try

    End Function

    Public Shared Function GetJavaInstallationPath() As String

        Dim environmentPath As String = Environment.GetEnvironmentVariable("JAVA_HOME")
        If Not String.IsNullOrEmpty(environmentPath) Then
            Return environmentPath
        End If

        Dim javaKey As String = "SOFTWARE\JavaSoft\Java Runtime Environment\"
        If Not Environment.Is64BitOperatingSystem Then
            Using rk As Microsoft.Win32.RegistryKey = Microsoft.Win32.Registry.LocalMachine.OpenSubKey(javaKey)
                Dim currentVersion As String = rk.GetValue("CurrentVersion").ToString()
                Using key As Microsoft.Win32.RegistryKey = rk.OpenSubKey(currentVersion)
                    Return key.GetValue("JavaHome").ToString()
                End Using
            End Using
        Else
            Using view64 = RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, RegistryView.Registry64)
                Using clsid64 = view64.OpenSubKey(javaKey)
                    Dim currentVersion As String = clsid64.GetValue("CurrentVersion").ToString()
                    Using key As RegistryKey = clsid64.OpenSubKey(currentVersion)
                        Return key.GetValue("JavaHome").ToString() & If(key.GetValue("JavaHome").ToString.ToUpper.EndsWith("BIN"), "", "\bin")
                    End Using
                End Using
            End Using
        End If

    End Function

    Public Shared Function CheckInstalled(ByVal findByName As String) As String
        Dim displayName As String
        Dim InstallPath As String
        Dim registryKey As String = "SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall"
        Dim key64 As Microsoft.Win32.RegistryKey
        Dim key As Microsoft.Win32.RegistryKey

        Try

            key64 = Microsoft.Win32.RegistryKey.OpenBaseKey(Microsoft.Win32.RegistryHive.LocalMachine, Microsoft.Win32.RegistryView.Registry64)
            key = key64.OpenSubKey(registryKey)

            If key IsNot Nothing Then
                For Each subkey As Microsoft.Win32.RegistryKey In key.GetSubKeyNames().Select(Function(keyName) key.OpenSubKey(keyName))
                    displayName = TryCast(subkey.GetValue("DisplayName"), String)
                    If displayName IsNot Nothing AndAlso displayName.Contains(findByName) Then

                        InstallPath = subkey.GetValue("InstallLocation").ToString()

                        Return InstallPath 'or displayName

                    End If
                Next subkey
                key.Close()
            End If

        Catch ex As Exception
            Stop
        End Try

        Return findByName

    End Function

    Private Shared Function CreateDataSetFromXML(ByVal xmlFile As String) As DataSet

        'load xml into a dataset to use here
        Dim DS As DataSet
        Dim FileStream As FileStream


        'open the xml file so we can use it to fill the dataset
        Try
            DS = New DataSet

            FileStream = New FileStream(System.Windows.Forms.Application.StartupPath & "\" & xmlFile, FileMode.Open, FileAccess.Read)

            DS.ReadXml(FileStream)

            'add required columns if they were missing from xml
            If Not DS.Tables("Column").Columns.Contains("Visible") Then DS.Tables("Column").Columns.Add("Visible")
            If Not DS.Tables("Column").Columns.Contains("FormatIsRegEx") Then DS.Tables("Column").Columns.Add("FormatIsRegEx")
            If Not DS.Tables("Column").Columns.Contains("WordWrap") Then DS.Tables("Column").Columns.Add("WordWrap")
            If Not DS.Tables("Column").Columns.Contains("ReadOnly") Then DS.Tables("Column").Columns.Add("ReadOnly")
            If Not DS.Tables("Column").Columns.Contains("MinimumCharWidth") Then DS.Tables("Column").Columns.Add("MinimumCharWidth")
            If Not DS.Tables("Column").Columns.Contains("MaximumCharWidth") Then DS.Tables("Column").Columns.Add("MaximumCharWidth")

            Return DS

        Catch ex As Exception
            Throw
        Finally

            If FileStream IsNot Nothing Then FileStream.Close()
            If DS IsNot Nothing Then DS.Dispose()

        End Try

    End Function

    <System.ComponentModel.Description("Creates dataset with 2 tables representing Grid defaults and the associated style.")>
    Public Shared Function GetTableStyle(ByVal xmlName As String) As DataSet

        Dim XMLStyleName As String

        Try

            If ConfigurationManager.GetSection(xmlName & If(xmlName.Contains(".xml"), "", ".xml")) IsNot Nothing Then
                XMLStyleName = CStr(CType(ConfigurationManager.GetSection(xmlName & If(xmlName.Contains(".xml"), "", ".xml")), IDictionary)("StyleLocation"))
            Else
                XMLStyleName = xmlName & If(xmlName.Contains(".xml"), "", ".xml")
            End If

            Return CreateDataSetFromXML(XMLStyleName)

        Catch ex As Exception When TypeOf (ex) Is NullReferenceException OrElse TypeOf (ex) Is FileNotFoundException
            MessageBox.Show("Please check < " & xmlName & " > entry in Config files, and check for file in execution folder. ", "Missing Entry", MessageBoxButtons.OK, MessageBoxIcon.Information)

            Return Nothing

        Catch ex As Exception

            Throw
        End Try

    End Function

    Public Shared Function IdentifyChanges(ByVal dr As DataRow, ByVal dgts As DataGridTableStyle) As String

        Dim ColumnNum As Integer = 0
        Dim DispName As String = ""
        Dim HistRow As String = ""
        Dim ColName As String = ""
        Dim NewVal As String
        Dim OrigVal As String

        Try
            If dr IsNot Nothing AndAlso dgts IsNot Nothing Then

                For Each DGCS As DataGridColumnStyle In dgts.GridColumnStyles

                    DispName = DGCS.HeaderText
                    ColName = DGCS.MappingName

                    If dr.Table.Columns.Contains(ColName) Then
                        If dr.RowState = DataRowState.Added OrElse IsDBNull(dr(ColName, DataRowVersion.Original)) Then
                            OrigVal = "NULL"
                        Else
                            OrigVal = IsNullStringHandler(dr(ColName, DataRowVersion.Original), "").ToUpper.Trim
                        End If

                        If dr.RowState <> DataRowState.Added AndAlso (dr.RowState = DataRowState.Deleted OrElse IsDBNull(dr(ColName, DataRowVersion.Current))) Then
                            NewVal = "NULL"
                        Else
                            NewVal = IsNullStringHandler(dr(ColName, DataRowVersion.Current), "").ToUpper.Trim
                        End If

                        If DispName <> "" AndAlso (dr.RowState = DataRowState.Added OrElse dr.RowState = DataRowState.Deleted OrElse Not dr(ColName, DataRowVersion.Current).Equals(dr(ColName, DataRowVersion.Original))) Then
                            HistRow &= DispName & " = " & NewVal & " (was '" & OrigVal & "') " & Microsoft.VisualBasic.vbCrLf
                        End If

                    End If

                Next

                If String.Compare(HistRow, "Type =  (was '') " & vbCrLf & "") = 0 Then
                    HistRow = ""
                End If

                Return HistRow
            End If

        Catch ex As Exception

            Throw

        End Try

    End Function


    Public Shared Function WaitForFile(ByVal fullPath As String, ByVal mode As FileMode, ByVal access As FileAccess, ByVal share As FileShare) As FileStream
        For numTries As Integer = 0 To 9

            Dim FS As FileStream
            Try
                FS = New FileStream(fullPath, mode, access, share)
                Return FS
            Catch e1 As IOException
                If FS IsNot Nothing Then
                    FS.Close()
                    FS.Dispose()
                End If
                Thread.Sleep(50)
            End Try
        Next numTries

        Return Nothing
    End Function

    Public Shared Function TextRowCount(dataSetName As String, Optional excludeHeader As Boolean = True) As Integer?

        Dim Rows As Integer

        Try

            Rows = File.ReadAllLines(dataSetName).Length + CInt(excludeHeader)

            Return Rows

        Catch ex As Exception

            Return Nothing

        Finally
        End Try

    End Function

    Public Shared Sub InvokeControlAction(Of t As Control)(control As t, action As Action(Of t))

        If Not control.IsDisposed AndAlso Not control.Disposing Then
            If control.InvokeRequired Then
                control.Invoke(New Action(Of t, Action(Of t))(AddressOf InvokeControlAction), New Object() {control, action})
            Else
                action(control)
            End If
        End If

    End Sub

    Public Shared Sub WaitCursor()
        WaitCursor(True)
    End Sub

    Public Shared Sub WaitCursor(value As Boolean)

        Application.UseWaitCursor = value
        System.Windows.Forms.Cursor.Position = System.Windows.Forms.Cursor.Position

    End Sub

    Public Shared Sub DateOnlyBinding_Parse(ByVal sender As Object, ByVal e As System.Windows.Forms.ConvertEventArgs)

        Try

            If IsDBNull(e.Value) = False AndAlso Not IsDate(e.Value) Then

                Dim HoldDate As Date? = UFCWGeneral.ValidateDate(e.Value)
                If HoldDate Is Nothing Then
                    e.Value = System.DBNull.Value
                Else
                    e.Value = CDate(HoldDate).ToShortDateString
                End If

            End If

        Catch ex As Exception
            Throw
        Finally

        End Try
    End Sub

    Public Shared Sub DateOnlyBinding_Format(ByVal sender As Object, ByVal e As System.Windows.Forms.ConvertEventArgs)
        ' -----------------------------------------------------------------------------
        ' <summary>
        ' formats date values entered for a databinding
        ' </summary>
        ' <param name="sender"></param>
        ' <param name="e"></param>
        ' <remarks>
        ' </remarks>
        ' <history>
        ' 	[nick snyder]	8/16/2006	Created
        ' </history>
        ' -----------------------------------------------------------------------------
        Dim OriginalVal As String = e.Value.ToString

        Try
            If Not IsDBNull(e.Value) AndAlso e.Value IsNot Nothing AndAlso e.Value.ToString.Trim.Length > 0 AndAlso IsDate(e.Value) Then
                e.Value = Format(e.Value, "MM-dd-yyyy")
            End If

        Catch ex As Exception
            Throw
        Finally
        End Try
    End Sub

    Public Shared Sub IntegerBinding_Parse(ByVal sender As Object, ByVal e As System.Windows.Forms.ConvertEventArgs)
        ' -----------------------------------------------------------------------------
        ' <summary>
        ' adjusts money values entered for a databinding
        ' </summary>
        ' <param name="sender"></param>
        ' <param name="e"></param>
        ' <remarks>
        ' </remarks>
        ' <history>
        ' 	[nick snyder]	8/16/2006	Created
        ' </history>
        ' -----------------------------------------------------------------------------
        Dim OriginalVal As String = e.Value.ToString

        Try
            If Not IsDBNull(e.Value) AndAlso Not e.Value.ToString.IsInteger() Then
                e.Value = DBNull.Value
            End If

        Catch ex As Exception
            Throw
        Finally
        End Try
    End Sub

    Public Shared Sub MoneyBinding_Parse(ByVal sender As Object, ByVal e As System.Windows.Forms.ConvertEventArgs)
        ' -----------------------------------------------------------------------------
        ' <summary>
        ' adjusts money values entered for a databinding
        ' </summary>
        ' <param name="sender"></param>
        ' <param name="e"></param>
        ' <remarks>
        ' </remarks>
        ' <history>
        ' 	[nick snyder]	8/16/2006	Created
        ' </history>
        ' -----------------------------------------------------------------------------
        Dim OriginalVal As String = e.Value.ToString

        Try
            If Not IsDBNull(e.Value) AndAlso Not e.Value.ToString.IsDecimal() Then
                e.Value = DBNull.Value
            End If

        Catch ex As Exception
            Throw
        Finally
        End Try
    End Sub

    Public Shared Sub MoneyBinding_Format(ByVal sender As Object, ByVal e As System.Windows.Forms.ConvertEventArgs)
        ' -----------------------------------------------------------------------------
        ' <summary>
        ' formats money values entered for a databinding
        ' </summary>
        ' <param name="sender"></param>
        ' <param name="e"></param>
        ' <remarks>
        ' </remarks>
        ' <history>
        ' 	[nick snyder]	8/16/2006	Created
        ' </history>
        ' -----------------------------------------------------------------------------
        Dim OriginalVal As String = e.Value.ToString

        Try
            If Not IsDBNull(e.Value) AndAlso e.Value IsNot Nothing AndAlso e.Value.ToString.Trim.Length > 0 Then
                e.Value = Format(e.Value, "0.00;-0.00")
            End If

        Catch ex As Exception
            Throw
        Finally
        End Try
    End Sub

    Public Shared Function IsNullDateHandler(value As Object, Optional ByVal fieldName As String = "") As Date?
        'Do not use this for timestamps as it does not accomodate subseconds (ffffff) correctly and may truncate the value
        Dim NullDate As Date? = CType(Nothing, Date?)
        Dim ValidateDateResponse As Object

        Try

            ValidateDateResponse = ValidateDate(value)

            'only an actual date field or nullable date field with a value pass this test
            If TypeOf (ValidateDateResponse) Is Date Then
                Return CDate(ValidateDateResponse)
            Else
                Return NullDate
            End If

        Catch ex As ArgumentException
            Throw
        Catch ex As Exception
            Throw
        End Try

    End Function

    Public Shared Function IsNullDateTimeHandler(value As Object, Optional ByVal fieldName As String = "") As DateTime?

        Dim NullDate As Date? = CType(Nothing, Date?)

        Dim ValidateDateResponse As Object

        Try

            ValidateDateResponse = ValidateDateTime(value)

            'only an actual date field or nullable date field with a value pass this test
            If TypeOf (ValidateDateResponse) Is Date Then
                Return CDate(ValidateDateResponse)
            Else
                Return NullDate
            End If

        Catch ex As ArgumentException
            Throw
        Catch ex As Exception
            Throw
        End Try

    End Function

    Public Shared Function ToNullDateHandler(value As Object) As Object

        Try

            If ValidateDate(value) Is Nothing Then
                Return System.DBNull.Value
            Else
                Return CType(value, Date)
            End If

        Catch ex As Exception
            Throw
        End Try

    End Function

    Public Shared Function ToNullDateTimeHandler(value As Object) As Object

        Try

            If ValidateDateTime(value) Is Nothing Then
                Return System.DBNull.Value
            Else
                Return CType(value, DateTime)
            End If

        Catch ex As Exception
            Throw
        End Try

    End Function

    Public Shared Function IsNullDecimalHandler(value As Object, Optional ByVal fieldName As String = "", Optional ByVal removeBadData As Boolean = False) As Decimal?

        Try

            If value Is Nothing OrElse IsDBNull(value) Then
                Return Nothing
            ElseIf Not value.ToString.IsDecimal() Then
                If removeBadData Then
                    Return Nothing
                Else
                    Throw New ArgumentException("Decimal test failed.", If(fieldName IsNot Nothing AndAlso fieldName.Length > 0, fieldName & " -> ", "") & value.ToString)
                End If
            Else
                Return CType(value, Decimal)
            End If

        Catch ex As Exception
            Throw
        End Try

    End Function

    Public Shared Function ToNullDecimalHandler(value As Object) As Object

        If value Is Nothing OrElse value.ToString.Trim.Length = 0 Then
            Return System.DBNull.Value
        Else
            Return CType(value, Decimal)
        End If

    End Function

    Public Shared Function IsNullShortHandler(value As Object, Optional ByVal fieldName As String = "", Optional ByVal removeBadData As Boolean = False) As Short?

        If value Is Nothing OrElse IsDBNull(value) Then
            Return Nothing
        ElseIf Not value.ToString.IsShort() Then
            If removeBadData Then
                Return Nothing
            Else
                Throw New ArgumentException("Short test failed.", If(fieldName IsNot Nothing AndAlso fieldName.Length > 0, fieldName & " -> ", "") & value.ToString)
            End If
        Else
            Return CType(value, Short)
        End If

    End Function

    Public Shared Function ToNullShortHandler(value As Object) As Object

        If value Is Nothing OrElse value.ToString.Trim.Length = 0 Then
            Return System.DBNull.Value
        Else
            Return CType(value, Short)
        End If

    End Function

    Public Shared Function IsNullIntegerHandler(value As Object, Optional ByVal fieldName As String = "", Optional ByVal removeBadData As Boolean = False) As Integer?

        Try

            If value Is Nothing OrElse IsDBNull(value) OrElse value.ToString.Trim.Length = 0 Then
                Return Nothing
            ElseIf Not value.ToString.IsInteger() Then
                If removeBadData Then
                    Return Nothing
                Else
                    Throw New ArgumentException("Integer test failed.", If(fieldName IsNot Nothing AndAlso fieldName.Length > 0, fieldName & " -> ", "") & value.ToString)
                End If
            Else
                Return CType(value, Integer)
            End If

        Catch ex As Exception
            Throw
        End Try

    End Function

    Public Shared Function ToNullIntegerHandler(value As Object) As Object

        If value Is Nothing OrElse value.ToString.Trim.Length = 0 Then
            Return System.DBNull.Value
        Else
            Return CType(value, Integer)
        End If

    End Function

    Public Shared Function IsNullLongHandler(value As Object, Optional ByVal fieldName As String = "", Optional ByVal removeBadData As Boolean = False) As Long?

        Try

            If value Is Nothing OrElse IsDBNull(value) Then
                Return Nothing
            ElseIf Not IsLong(value.ToString) Then
                If removeBadData Then
                    Return Nothing
                Else
                    Throw New ArgumentException("Long test failed.", If(fieldName IsNot Nothing AndAlso fieldName.Length > 0, fieldName & " -> ", "") & value.ToString)
                End If
            Else
                Return CType(value, Long)
            End If

        Catch ex As Exception
            Throw
        End Try

    End Function

    Public Shared Function ToNullLongHandler(value As Object) As Object

        If value Is Nothing OrElse value.ToString.Trim.Length = 0 Then
            Return System.DBNull.Value
        Else
            Return CType(value, Long)
        End If

    End Function

    Public Shared Function ToNullStringHandler(value As Object, Optional convertEmpty2Null As Boolean = False) As Object

        If value Is Nothing Then 'allow a blank string to be processed
            Return System.DBNull.Value
        ElseIf convertEmpty2Null AndAlso value.ToString = "" Then 'allow a blank string to be processed
            Return System.DBNull.Value
        Else
            Return value
        End If

    End Function

    Public Shared Function IsNullStringHandler(value As Object, Optional emptyValue As String = Nothing) As String

        If value Is Nothing OrElse IsDBNull(value) OrElse value.ToString.Length = 0 Then
            Return emptyValue
        Else
            If value.GetType Is GetType(Date) Then
                If CType(value, Date).TimeOfDay.Ticks = 0 Then
                    Return CType(value, Date).Date.ToShortDateString
                Else
                    Return CType(value, Date).ToString
                End If
            Else
                Return value.ToString
            End If

        End If

    End Function

    Public Shared Function FixFileName(value As Object) As String
        Dim FileName As String

        FileName = value.ToString.Replace("\\", "\")
        FileName = If(FileName.StartsWith("\"), "\" & FileName, FileName)

        Return FileName

    End Function

    Public Shared Function ReturnDuration(ByVal dteStartTime As Date) As String
        Dim dteEndTime As Date = NowDate
        Dim strDays As String
        Dim strHours As String
        Dim strMinutes As String
        Dim strSeconds As String
        Dim strMiliSeconds As String
        Dim Tmp As String
        Dim RunLength As System.TimeSpan = dteEndTime.Subtract(dteStartTime)

        '' Format the duration
        Select Case RunLength.Days
            Case Is < 1
                strDays = ""
            Case Is = 1
                strDays = RunLength.Days & " Day "
            Case Is > 1
                strDays = RunLength.Days & " Days "
        End Select

        Select Case RunLength.Hours
            Case Is < 1
                strHours = ""
            Case Is = 1
                strHours = RunLength.Hours & " Hr "
            Case Is > 1
                strHours = RunLength.Hours & " Hrs "
        End Select

        Select Case RunLength.Minutes
            Case Is < 1
                strMinutes = ""
            Case Is = 1
                strMinutes = RunLength.Minutes & " Min "
            Case Is > 1
                strMinutes = RunLength.Minutes & " Mins "
        End Select

        Select Case RunLength.Seconds
            Case Is < 1
                strSeconds = ""
            Case 1
                strSeconds = RunLength.Seconds & " Sec "
            Case Is > 1
                strSeconds = RunLength.Seconds & " Secs "
        End Select

        Select Case RunLength.Milliseconds
            Case Is < 1
                strMiliSeconds = ""
            Case 1
                strMiliSeconds = RunLength.Milliseconds & " MilliSec "
            Case Is > 1
                strMiliSeconds = RunLength.Milliseconds & " MilliSecs "
        End Select

        '' Display the duration to the user
        Tmp = strDays & strHours & strMinutes & strSeconds & strMiliSeconds
        If Tmp = "" Then Tmp = "0 Secs"

        Return Tmp
    End Function

    Public Shared Function ColumnEqual(ByVal a As Object, ByVal b As Object) As Boolean
        ' -----------------------------------------------------------------------------
        ' <summary>
        '  Compares two values to determine if they are equal. Also compares DBNULL.Value.
        ' </summary>
        ' <param name="A"></param>
        ' <param name="B"></param>
        ' <returns></returns>
        ' <remarks>
        ' </remarks>
        ' <history>
        ' 	[paulw]	10/18/2006	Created
        ' </history>
        ' -----------------------------------------------------------------------------
        Try

            If (a Is Nothing AndAlso b Is Nothing) Then
                Return True
            ElseIf (a Is Nothing OrElse b Is Nothing) Then
                Return False
            Else
                Return a.Equals(b)
            End If

        Catch ex As Exception
            Throw
        End Try

    End Function

    Public Shared Function Min(ByVal a As Object, ByVal b As Object) As Object
        '
        ' Returns MIN of two values. DBNull is less than all others.
        Try

            If a Is DBNull.Value OrElse b Is DBNull.Value Then Return DBNull.Value

            If CDbl(a) < CDbl(b) Then Return a Else Return b

        Catch ex As Exception
            Throw
        End Try

    End Function

    Public Shared Function Max(ByVal a As Object, ByVal b As Object) As Object
        '
        ' Returns Max of two values. DBNull is less than all others.
        '
        Try

            If a Is DBNull.Value Then Return b
            If b Is DBNull.Value Then Return a

            If CDbl(a) > CDbl(b) Then Return a Else Return b

        Catch ex As Exception
            Throw
        End Try

    End Function

    Public Shared Function Add(ByVal a As Object, ByVal b As Object) As Object
        '
        ' Adds two values. If one is DBNull, returns the other.
        '
        Try

            If a Is DBNull.Value Then Return b
            If b Is DBNull.Value Then Return a

            Return CDbl(a) + CDbl(b)

        Catch ex As Exception
            Throw
        End Try

    End Function

    Public Shared Function MyTime() As String
        MyTime = Format(NowDate, "dd-MMM-yyyy HH:nn:ss.fff")
    End Function

    Public Shared Function GetSimilarity(string1 As String, string2 As String) As Single
        Dim dis As Single = ComputeDistance(string1, string2)
        Dim maxLen As Single = string1.Length
        If maxLen < string2.Length Then
            maxLen = string2.Length
        End If
        If maxLen = 0.0F Then
            Return 1.0F
        Else
            Return 1.0F - dis / maxLen
        End If
    End Function

    Private Shared Function ComputeDistance(s As String, t As String) As Integer
        Dim n As Integer = s.Length
        Dim m As Integer = t.Length
        Dim distance As Integer(,) = New Integer(n, m) {}
        ' matrix
        Dim Cost As Integer
        If n = 0 Then
            Return m
        End If
        If m = 0 Then
            Return n
        End If
        'init1

        Dim i As Integer = 0
        While i <= n
            distance(i, 0) = System.Math.Min(System.Threading.Interlocked.Increment(i), i - 1)
        End While
        Dim j As Integer = 0
        While j <= m
            distance(0, j) = System.Math.Min(System.Threading.Interlocked.Increment(j), j - 1)
        End While
        'find min distance

        For i = 1 To n
            For j = 1 To m
                Cost = (If(t.Substring(j - 1, 1) = s.Substring(i - 1, 1), 0, 1))
                distance(i, j) = Math.Min(distance(i - 1, j) + 1, Math.Min(distance(i, j - 1) + 1, distance(i - 1, j - 1) + Cost))
            Next
        Next
        Return distance(n, m)

    End Function

    Public Shared Function CompareFolders(ByVal distributionFolder As String, ByVal localFolder As String) As String
        Return CompareFolders(distributionFolder, localFolder, "")
    End Function

    Public Shared Function CompareFolders(ByVal distributionFolder As String, ByVal localFolder As String, ByVal excludeXMLFiles As String) As String
        Dim DirInfo As DirectoryInfo
        Dim Remotefile As IO.FileInfo
        Dim Localfile As IO.FileInfo
        Dim Local As String = ""
        Dim Details As String = ""
        Dim RemoteVersion As String
        Dim LocalVersion As String
        Dim LogFileName As String = localFolder & "\" & "FileVersions.Log"
        Dim StreamWriter As StreamWriter

        Try
            DirInfo = New DirectoryInfo(distributionFolder)

            If My.Computer.Keyboard.ShiftKeyDown Then 'bypass validation if shift key is being held down.
                Details = ""
            Else

                ''Checking if DistributionFolder is valid
                If Not DirInfo.Exists Then
                    Details = "Distribution Folder is Invalid"
                    Exit Try
                End If
                If My.Computer.FileSystem.GetFiles(distributionFolder).Count > 0 Then
                    For Each file As String In My.Computer.FileSystem.GetFiles(distributionFolder, FileIO.SearchOption.SearchTopLevelOnly, "*.*")
                        Remotefile = My.Computer.FileSystem.GetFileInfo(file)
                        If Not excludeXMLFiles.Contains(Remotefile.Name.Trim()) Then
                            Local = localFolder & "\" & Remotefile.Name
                            Localfile = New IO.FileInfo(Local)

                            RemoteVersion = FileVersionInfo.GetVersionInfo(Remotefile.FullName).FileVersion

                            If Localfile.Exists = False Then
                                Details += "MISSING     :                                                                                         " & Localfile.Name & Environment.NewLine
                            ElseIf Localfile.Exists = True Then
                                LocalVersion = FileVersionInfo.GetVersionInfo(Localfile.FullName).FileVersion
                                If RemoteVersion IsNot Nothing AndAlso LocalVersion IsNot Nothing Then
                                    If LocalVersion.CompareTo(RemoteVersion) > 0 Then
                                        Details += "WARNING  :        Local File is Newer/Greater than Distribution            " & Localfile.Name & "                LOCAL : V " & LocalVersion & "     DISTRIBUTION : V " & RemoteVersion & Environment.NewLine
                                    ElseIf LocalVersion.CompareTo(RemoteVersion) < 0 Then
                                        Details += "DIFFERENT:                                                                                         " & Localfile.Name & "                LOCAL : V " & LocalVersion & "     DISTRIBUTION : V " & RemoteVersion & Environment.NewLine
                                    End If
                                    ''If RemoteVersion <> LocalVersion Then strdetails += "DIFFERENT:          " & Localfile.Name & "          LOCAL : V " & LocalVersion & "          DISTRIBUTION : V " & RemoteVersion & Environment.NewLine
                                Else
                                    If Remotefile.LastWriteTime > Localfile.LastWriteTime Then
                                        Details += "DIFFERENT:                                                                                         " & Remotefile.Name.Trim() & "                LOCAL File Date: " & Localfile.LastWriteTime & "     DISTRIBUTION File Date: " & Remotefile.LastWriteTime & Environment.NewLine
                                    ElseIf Remotefile.LastWriteTime < Localfile.LastWriteTime Then
                                        Details += "WARNING  :        Local File is Newer/Greater than Distribution            " & Remotefile.Name.Trim() & "                LOCAL File Date: " & Localfile.LastWriteTime & "     DISTRIBUTION File Date: " & Remotefile.LastWriteTime & Environment.NewLine
                                    End If
                                End If
                            End If
                        End If
                        file = "" : Local = ""
                    Next file
                Else
                    Details = ""
                End If
            End If

            StreamWriter = File.CreateText(LogFileName)
            StreamWriter.Write(Details)

            Return Details

        Catch ex As Exception
            Throw
        Finally

            If StreamWriter IsNot Nothing Then
                StreamWriter.Close()
            End If
            StreamWriter = Nothing

        End Try

    End Function

    Public Shared Function FileInUse(ByVal sFile As String) As Boolean
        Dim FStream As FileStream

        Try

            If System.IO.File.Exists(sFile) Then

                Try
                    FStream = New FileStream(sFile, FileMode.Open, FileAccess.Read, FileShare.None)
                Catch
                    Return True
                Finally
                    If FStream IsNot Nothing Then
                        FStream.Close()
                    End If

                    FStream = Nothing
                End Try
            End If

        Catch ex As Exception
            Throw
        End Try
    End Function

    Public Shared Function GetSuffix(fileName As String) As String

        Dim SuffixBeginsAt As Integer = fileName.LastIndexOf(".")

        Try

            If (fileName.Length - 5 <> SuffixBeginsAt AndAlso fileName.Length - 4 <> SuffixBeginsAt) OrElse SuffixBeginsAt = 0 Then Return Nothing 'if file is not a standard name.xxx format then assume 

            If SuffixBeginsAt > 0 Then
                Return fileName.Substring(SuffixBeginsAt + 1).Trim
            End If

        Catch ex As Exception
            Throw
        End Try

    End Function

    Public Shared Function FlattenStack(st As StackTrace) As String

        Dim SB As New StringBuilder

        For X As Integer = 0 To CInt(StackLevels)
            If X > st.FrameCount - 1 Then Exit For
            SB.Append(If(SB.Length > 0, " <- ", "") & st.GetFrame(X).GetMethod.ToString)
        Next

        Return SB.ToString

    End Function

    Public Shared Function FindCustomerService() As FileInfo

        Dim CustomerServiceFI As FileInfo
        Dim CustomerServiceHoldFI As FileInfo

        'find newest available version of Customer Service

        'start with current folder (useful for debugging)
        CustomerServiceFI = New FileInfo(Environment.CurrentDirectory & "\" & "UFCW.CMS.CustomerService.exe")

        If CustomerServiceFI.Exists Then
            CustomerServiceHoldFI = New FileInfo(CustomerServiceFI.FullName)
        End If

        'use location identified in config, or ignore error if config is not populated
        Try

            Dim CustomerServiceExe As String = TryCast(TryCast(ConfigurationManager.GetSection("CustomerService"), IDictionary)?("EXEName"), String)

            If Not String.IsNullOrEmpty(CustomerServiceExe) Then
                CustomerServiceFI = New FileInfo(CustomerServiceExe)

                If CustomerServiceFI.Exists Then
                    Dim FileLastWriteTime As DateTime = CustomerServiceFI.LastWriteTime
                    Dim FileVersionInfo As FileVersionInfo = FileVersionInfo.GetVersionInfo(CustomerServiceFI.FullName)

                    If CustomerServiceHoldFI Is Nothing OrElse CustomerServiceFI.LastWriteTime > CustomerServiceHoldFI.LastWriteTime Then
                        CustomerServiceHoldFI = New FileInfo(CustomerServiceFI.FullName)
                    End If

                End If
            End If

        Catch ex As Exception

        End Try

        'code registry clickonce lookup.
        Dim ClickOnceFileName As String = GetSetting("UFCW\\UFCW.CMS.CustomerService\", "ClickOnce", "FileInfo")
        CustomerServiceFI = New FileInfo(ClickOnceFileName & "\" & "UFCW.CMS.CustomerService.exe")

        If CustomerServiceFI.Exists Then
            Dim FileLastWriteTime As DateTime = CustomerServiceFI.LastWriteTime
            Dim FileVersionInfo As FileVersionInfo = FileVersionInfo.GetVersionInfo(CustomerServiceFI.FullName)

            If CustomerServiceHoldFI Is Nothing OrElse CustomerServiceFI.LastWriteTime > CustomerServiceHoldFI.LastWriteTime Then
                CustomerServiceHoldFI = New FileInfo(CustomerServiceFI.FullName)
            End If

        End If

        Dim ProdFileName As String = GetSetting("UFCW\\UFCW.CMS.CustomerService\", "Prod", "FileInfo")
        CustomerServiceFI = New FileInfo(ProdFileName & "\" & "UFCW.CMS.CustomerService.exe")

        If CustomerServiceFI.Exists Then
            Dim FileLastWriteTime As DateTime = CustomerServiceFI.LastWriteTime
            Dim FileVersionInfo As FileVersionInfo = FileVersionInfo.GetVersionInfo(CustomerServiceFI.FullName)

            If CustomerServiceHoldFI Is Nothing OrElse CustomerServiceFI.LastWriteTime > CustomerServiceHoldFI.LastWriteTime Then
                CustomerServiceHoldFI = New FileInfo(CustomerServiceFI.FullName)
            End If

        End If

        Return CustomerServiceHoldFI

    End Function
#End Region

End Class



'Public Class KeyboardHook

'    Private Const HC_ACTION As Integer = 0
'    Private Const WM_KEYDOWN As Integer = &H100
'    Private Const WM_KEYUP As Integer = &H101
'    Private Const WM_SYSKEYDOWN As Integer = &H104
'    Private Const WM_SYSKEYUP As Integer = &H105

'    <DllImport("user32.dll", SetLastError:=True)>
'    Private Shared Function SetWindowsHookEx(ByVal hookType As HookType, ByVal lpfn As KeyboardProcDelegate, ByVal hMod As IntPtr, ByVal dwThreadId As UInteger) As IntPtr
'    End Function

'    <DllImport("user32.dll", SetLastError:=True)>
'    Private Shared Function CallNextHookEx(ByVal hhk As IntPtr, ByVal nCode As Integer, ByVal wParam As UInteger, <[In]()> ByRef lParam As KBDLLHOOKSTRUCT) As IntPtr
'    End Function

'    <DllImport("user32.dll", SetLastError:=True)>
'    Private Shared Function UnhookWindowsHookEx(ByVal hhk As IntPtr) As Boolean
'    End Function

'    Private Delegate Function KeyboardProcDelegate(ByVal nCode As Integer, ByVal wParam As UInteger, ByRef lParam As KBDLLHOOKSTRUCT) As IntPtr

'    Private Shared KeyHook As IntPtr
'    Private Shared KeyHookDelegate As KeyboardProcDelegate

'    Public Shared Event KeyDown(ByVal Key As Keys)
'    Public Shared Event KeyUp(ByVal Key As Keys)

'    Shared Sub New() ' Installs The Hook

'        KeyHookDelegate = New KeyboardProcDelegate(AddressOf KeyboardProc)
'        KeyHook = SetWindowsHookEx(HookType.WH_KEYBOARD_LL, KeyHookDelegate, IntPtr.Zero, 0)

'    End Sub

'    Private Shared Function KeyboardProc(ByVal nCode As Integer, ByVal wParam As UInteger, ByRef lParam As KBDLLHOOKSTRUCT) As IntPtr

'        If (nCode = HC_ACTION) Then
'            Select Case wParam
'                Case WM_KEYDOWN, WM_SYSKEYDOWN
'                    RaiseEvent KeyDown(CType(lParam.vkCode, Keys))
'                Case WM_KEYUP, WM_SYSKEYUP
'                    RaiseEvent KeyUp(CType(lParam.vkCode, Keys))
'            End Select
'        End If

'        Return CallNextHookEx(KeyHook, nCode, wParam, lParam)
'    End Function
'    Protected Overrides Sub Finalize()

'        UnhookWindowsHookEx(KeyHook)   'Un-Hooks When Program Closes
'        MyBase.Finalize()
'    End Sub
'End Class













