''' -----------------------------------------------------------------------------
''' Project	 : UFCW.CMS.Binder
''' Class	 : CMS.Binder.MedicalBinderItem
'''
''' -----------------------------------------------------------------------------
''' <summary>
''' Represents a Medical Binder Item
''' </summary>
''' <remarks>
''' </remarks>
''' <history>
''' 	[paulw]	4/4/2006	Created
''' </history>
''' -----------------------------------------------------------------------------

<Serializable()> _
Public Class MedicalBinderItem
    Inherits BinderItem

    'Private Shared _TraceSwitch As New BooleanSwitch("TraceCloning", "Trace Switch in App.Config")
    ' -----------------------------------------------------------------------------
    ' <summary>
    ' This is done so that only a class within this dll can
    '  instaniate a new binder item.  This functionality
    '  mimics datatable.NewDataRow
    ' </summary>
    ' <remarks>
    ' </remarks>
    ' <history>
    ' 	[paulw]	9/20/2006	Created
    ' </history>
    ' -----------------------------------------------------------------------------
    Friend Sub New()

    End Sub

End Class