Imports System.IO
Imports System.Reflection
Imports System.Windows.Forms
Imports SharedInterfaces

Public Class PlugInController
    Private _PlugArray As SortedList = New SortedList
    Private _PluginFilter As String = "*.dll"

    Public Event AddingPlugIn(ByVal PlugIn As PlugInAttribute)
    Public Event PlugInLoading(ByVal PlugIn As PlugInAttribute, ByRef Cancel As Boolean)
    Public Event PlugInsClearing(ByRef Cancel As Boolean)

#Region "Constructors"
    Sub New()
        MyBase.New()
    End Sub

    Sub New(ByVal Filter As String)
        MyBase.New()
        _PluginFilter = Filter
    End Sub
#End Region

#Region "Properties"
    <System.ComponentModel.Description("Allows a Filter for the dll to shorten load time. Default = *.dll")>
    Public Property PluginFilter() As String
        Get
            Return _PluginFilter
        End Get
        Set(ByVal Value As String)
            _PluginFilter = Value
        End Set
    End Property
#End Region

#Region "Load Plugins"
    ' -----------------------------------------------------------------------------
    ' <summary>
    ' Loads PlugIns in the root directory with a mask of "*.dll" or user specified
    ' mask into an array.
    ' It launches an Event to signal to the calling module what it is adding and 
    ' gives the ability to cancel.
    ' </summary>
    ' <remarks>
    ' </remarks>
    ' <history>
    ' 	[Nick Snyder]	3/1/2006	Created
    ' </history>
    ' -----------------------------------------------------------------------------
    'Public Sub LoadPlugIns()
    '    Dim fInfo As FileInfo = New FileInfo(Application.ExecutablePath)
    '    Dim dInfo As DirectoryInfo = New DirectoryInfo(fInfo.DirectoryName)
    '    Dim asmFileArray() As FileInfo = dInfo.GetFiles(_PluginFilter)

    '    For Each fi As FileInfo In asmFileArray
    '        Dim asm As [Assembly] = [Assembly].LoadFrom(fi.FullName)
    '        Dim mi() As [Module] = asm.GetModules
    '        For Each m As [Module] In mi
    '            Dim typeArray() As Type = m.GetTypes
    '            For Each t As Type In typeArray
    '                Dim obj() As Object = t.GetCustomAttributes(GetType(PlugInAttribute), True)
    '                Dim Cancel As Boolean = False
    '                If obj.Length > 0 Then
    '                    Dim plugIn As PlugInAttribute = CType(obj(0), PlugInAttribute)
    '                    RaiseEvent PlugInLoading(plugIn, Cancel)
    '                    If Cancel = False Then
    '                        Dim objPInfo As PlugInInfo = New PlugInInfo(asm.GetName(), t.FullName)
    '                        Me._plugArray.Add(plugIn.MenuText, objPInfo)
    '                    End If
    '                End If
    '            Next
    '        Next
    '    Next
    'End Sub
    Public Sub LoadPlugIns()
        Dim fInfo As FileInfo
        Dim dInfo As DirectoryInfo
        Dim asmFileArray() As FileInfo
        Dim PlugCnt As Integer = 0
        Dim DR As DataRow
        Dim dt As New DataTable("Plugins")
        Dim dv As DataView
        Try

            fInfo = New FileInfo(Application.ExecutablePath)
            dInfo = New DirectoryInfo(fInfo.DirectoryName)
            asmFileArray = dInfo.GetFiles(_PluginFilter)

            dt.Columns.Add("Position", System.Type.GetType("System.Int32"))
            dt.Columns.Add("Key", System.Type.GetType("System.String"))
            dt.Columns.Add("Value", System.Type.GetType("System.Object"))
            dt.Columns.Add("PlugInAttribute", System.Type.GetType("System.Object"))

            For Each fi As FileInfo In asmFileArray
                Dim asm As [Assembly] = [Assembly].LoadFrom(fi.FullName)
                Dim mi() As [Module] = asm.GetModules

                For Each m As [Module] In mi
                    Dim typeArray() As Type = m.GetTypes

                    For Each t As Type In typeArray
                        Dim obj() As Object = t.GetCustomAttributes(GetType(PlugInAttribute), True)
                        Dim Cancel As Boolean = False

                        If obj.Length > 0 Then
                            Dim plugIn As PlugInAttribute = CType(obj(0), PlugInAttribute)
                            RaiseEvent PlugInLoading(plugIn, Cancel)

                            If Cancel = False Then
                                Dim objPInfo As PlugInInfo = New PlugInInfo(asm.GetName(), t.FullName)
                                DR = dt.NewRow

                                If plugIn.Position <> -1 Then
                                    DR("Position") = plugIn.Position
                                Else
                                    DR("Position") = PlugCnt
                                End If
                                DR("Key") = plugIn.MenuText
                                DR("Value") = objPInfo
                                DR("PlugInAttribute") = plugIn

                                dt.Rows.Add(DR)

                                'Me._plugArray.Add(plugIn.MenuText, objPInfo)
                                PlugCnt += 1
                            End If
                        End If
                    Next
                Next
            Next

            If dt.Rows.Count > 0 Then
                dv = New DataView(dt, "", "Position", DataViewRowState.CurrentRows)

                For cnt As Integer = 0 To dv.Count - 1
                    RaiseEvent AddingPlugIn(CType(dv(cnt)("PlugInAttribute"), PlugInAttribute))
                    Me._PlugArray.Add(dv(cnt)("Key"), dv(cnt)("Value"))
                Next
            End If

        Catch ex As Exception
            Throw
        End Try
    End Sub

    'Public Sub LoadPlugIns(ByRef mnu As MenuItem)
    '    Dim fInfo As FileInfo = New FileInfo(Application.ExecutablePath)
    '    Dim dInfo As DirectoryInfo = New DirectoryInfo(fInfo.DirectoryName)
    '    Dim asmFileArray() As FileInfo = dInfo.GetFiles("UFCW.*.dll")

    '    For Each fi As FileInfo In asmFileArray
    '        Dim asm As [Assembly] = [Assembly].LoadFrom(fi.FullName)
    '        Dim mi() As [Module] = asm.GetModules
    '        For Each m As [Module] In mi
    '            Dim typeArray() As Type = m.GetTypes
    '            For Each t As Type In typeArray
    '                Dim obj() As Object = t.GetCustomAttributes(GetType(PlugInAttribute), True)
    '                If obj.Length > 0 Then
    '                    Dim plugIn As PlugInAttribute = CType(obj(0), PlugInAttribute)
    '                    Dim newMenu As MenuItem = mnu.MenuItems.Add(plugIn.MenuText)
    '                    AddHandler newMenu.Click, AddressOf frmM.mnuPlugIn_Click
    '                    Dim objPInfo As PlugInInfo = New PlugInInfo(asm.GetName(), t.FullName)
    '                    Me._plugArray.Add(plugIn.MenuText, objPInfo)
    '                End If
    '            Next
    '        Next
    '    Next
    'End Sub

    'Public Sub LoadPlugIns(ByVal TBar As ToolBar)
    '    Dim fInfo As FileInfo = New FileInfo(Application.ExecutablePath)
    '    Dim dInfo As DirectoryInfo = New DirectoryInfo(fInfo.DirectoryName)
    '    Dim asmFileArray() As FileInfo = dInfo.GetFiles("UFCW.*.dll")

    '    For Each fi As FileInfo In asmFileArray
    '        Dim asm As [Assembly] = [Assembly].LoadFrom(fi.FullName)
    '        Dim mi() As [Module] = asm.GetModules
    '        For Each m As [Module] In mi
    '            Dim typeArray() As Type = m.GetTypes
    '            For Each t As Type In typeArray
    '                Dim obj() As Object = t.GetCustomAttributes(GetType(PlugInAttribute), True)
    '                If obj.Length > 0 Then
    '                    Dim plugIn As PlugInAttribute = CType(obj(0), PlugInAttribute)
    '                    If plugIn.Destination.ToLower = "main" Then
    '                        Dim TBButtonIndex As Integer = TBar.Buttons.Add(plugIn.MenuText)
    '                        TBar.Buttons(TBButtonIndex).Tag = plugIn.MenuText
    '                        TBar.Buttons(TBButtonIndex).ImageIndex = plugIn.ImageIndex
    '                        AddHandler TBar.ButtonClick, AddressOf frmM.btnPlugIn_Click
    '                        Dim objPInfo As PlugInInfo = New PlugInInfo(asm.GetName(), t.FullName)
    '                        Me._plugArray.Add(plugIn.MenuText, objPInfo)
    '                    End If
    '                End If
    '            Next
    '        Next
    '    Next
    'End Sub
#End Region

#Region "Launch Plugins"
    ' -----------------------------------------------------------------------------
    ' <summary>
    ' Launches a PlugIn and allows for any constructor as long as the plugin
    ' constructor is suppilied.
    ' </summary>
    ' <param name="pluginName"></param>
    ' <param name="objIMsg"></param>
    ' <param name="ConstructorOptions"></param>
    ' <returns></returns>
    ' <remarks>
    ' </remarks>
    ' <history>
    ' 	[Nick Snyder]	3/1/2006	Created
    ' </history>
    ' -----------------------------------------------------------------------------
    Public Function LaunchPlugIn(ByVal pluginName As String, ByVal objIMsg As IMessage, ByVal ParamArray ConstructorOptions() As Object) As Form
        Dim pi As PlugInInfo = CType(Me._PlugArray(pluginName), PlugInInfo)
        Dim asm As [Assembly] = [Assembly].Load(pi.AssemblyName)
        Dim frm As Form
        Dim t As Type = asm.GetType(pi.TypeName)

        Try
            If ConstructorOptions.Length = 0 Then
                frm = CType(t.InvokeMember("New", BindingFlags.Public _
                                Or BindingFlags.Instance Or BindingFlags.CreateInstance,
                                Nothing, t, New Object() {objIMsg}), Form)
            Else
                Dim obj() As Object
                Dim Cnt As Integer = 0
                ReDim obj(ConstructorOptions.Length)
                obj(0) = objIMsg
                For Cnt = 0 To ConstructorOptions.Length - 1
                    obj(Cnt + 1) = ConstructorOptions(Cnt)
                Next
                frm = CType(t.InvokeMember("New", BindingFlags.Public _
                                Or BindingFlags.Instance Or BindingFlags.CreateInstance, _
                                Nothing, t, obj), Form)
            End If

            Return frm

        Catch ex As Exception

            Throw

        End Try
        'Catch ex As Exception
        '    MessageBox.Show(ex.Message & ex.StackTrace, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        'End Try

    End Function
#End Region

    Public Function Contains(ByVal pluginName As String) As Boolean
        Return Me._PlugArray.Contains(pluginName)
    End Function

    Public Sub ReLoadPlugIns()
        Me._PlugArray.Clear()
        LoadPlugIns()
    End Sub
End Class