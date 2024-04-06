Imports System.Runtime.InteropServices

Public Structure MemberInfo

    Public MemFamilyID As Integer
    Public MemName As String
    Public MemAddress As String
    Public MedEligible As String
    Public DentalEligible As String
    Public MedPlan As String
    Public MedPlanDesc As String
    Public DentalPlan As String
    Public DentalPlanDesc As String
    Public MemType As String
    Public Status As String
    Public MemLocal As String
    Public MemEntryDate As String
    Public MemRetirementDate As String
    Public MemRetireePlan As String
    Public MemLastEmployer As String
    Public LastVacationUpdate As String
    Public VacationDate As String
    Public VacationMonths As String

    Public Sub New(Optional memberFamilyID As Integer = -1, Optional memberName As String = "", Optional memberAddress As String = "", Optional memberMedEligible As String = "", Optional memberDentalEligible As String = "", Optional memberMedPlan As String = "", Optional memberMedPlanDesc As String = "", Optional memberDentalPlan As String = "", Optional memberDentalPlanDesc As String = "", Optional membermemType As String = "", Optional memberstatus As String = "", Optional memberLocal As String = "", Optional memberEntryDate As String = "", Optional memberRetirementDate As String = "", Optional memberRetireePlan As String = "", Optional memberLastEmployer As String = "", Optional memberLastVacationUpdate As String = "", Optional memberVacationDate As String = "", Optional memberVacationMonths As String = "")
        Me.New()

        MemName = memberName
        MemAddress = memberAddress
        MedEligible = memberMedEligible
        DentalEligible = memberDentalEligible
        MedPlan = memberMedPlan
        MedPlanDesc = memberMedPlanDesc
        DentalPlan = memberDentalPlan
        DentalPlanDesc = memberDentalPlanDesc
        MemType = membermemType
        Status = memberstatus
        MemLocal = memberLocal
        MemEntryDate = memberEntryDate
        MemRetirementDate = memberRetirementDate
        MemRetireePlan = memberRetireePlan
        MemLastEmployer = memberLastEmployer
        LastVacationUpdate = memberLastVacationUpdate
        VacationDate = memberVacationDate
        VacationMonths = memberVacationMonths

    End Sub

    Public Sub Clear()
        MemFamilyID = -1
        MemName = ""
        MemAddress = ""
        MedEligible = ""
        DentalEligible = ""
        MedPlan = ""
        MedPlanDesc = ""
        DentalPlan = ""
        DentalPlanDesc = ""
        MemType = ""
        Status = ""
        MemLocal = ""
        MemEntryDate = ""
        MemRetirementDate = ""
        MemRetireePlan = ""
        MemLastEmployer = ""
        LastVacationUpdate = ""
        VacationDate = ""
        VacationMonths = ""
    End Sub
End Structure

<StructLayout(LayoutKind.Sequential, CharSet:=CharSet.Auto)>
Public Structure RECT
    Public left As Integer
    Public top As Integer
    Public right As Integer
    Public bottom As Integer
End Structure

<StructLayout(LayoutKind.Sequential, CharSet:=CharSet.Auto)>
Public Structure SPParameter
    Sub New(theParameterName As String, theParameterType As DbType, theParameterContent As String)
        ParameterName = theParameterName
        ParameterType = theParameterType
        ParameterContent = theParameterContent
    End Sub
    Sub New(theParameter As String)

        Dim SplitBy() As String = theParameter.Split(New Char() {"|"c}, 3)

        ParameterName = SplitBy(0)

        Select Case SplitBy(1).ToUpper
            Case "DATE"
                ParameterType = DbType.Date
            Case "STRING"
                ParameterType = DbType.String
            Case "INTEGER", "INT"
                ParameterType = DbType.Int32
            Case "DECIMAL", "DEC"
                ParameterType = DbType.Decimal
            Case "SHORT"
                ParameterType = DbType.Int16
            Case "TIMESTAMP"
                ParameterType = DbType.DateTime
            Case "ARRAY"
                ParameterType = DbType.Object
            Case Else
                Throw New ArgumentException("Unrecognized Stored Procedure Type Parameter (Name/Type(DATE,STRING,INTEGER,DECIMAL,SHORT,TIMESTAMP)/Value)", SplitBy(1).ToUpper)
        End Select

        ParameterContent = SplitBy(2)

    End Sub
    Public ParameterName As String
    Public ParameterType As DbType
    Public ParameterContent As String
End Structure

<StructLayout(LayoutKind.Sequential)>
Public Structure NMHDR
    Public hwndFrom As IntPtr
    Public idFrom As IntPtr
    'This is declared as UINT_PTR in winuser.h
    Public code As Integer
End Structure

' Declare the INPUT struct
<StructLayout(LayoutKind.Sequential)>
Public Structure INPUT
    Public type As UInteger
    Public U As InputUnion
    Public Shared ReadOnly Property Size() As Integer
        Get
            Return Marshal.SizeOf(GetType(INPUT))
        End Get
    End Property
End Structure

<StructLayout(LayoutKind.Sequential)>
Public Structure MOUSEINPUT
    Friend dx As Integer
    Friend dy As Integer
    Friend mouseData As MouseEventDataXButtons
    Friend dwFlags As MOUSEEVENTF
    Friend time As UInteger
    Friend dwExtraInfo As UIntPtr
End Structure

''' <summary>
''' Define HARDWAREINPUT struct
''' </summary>
<StructLayout(LayoutKind.Sequential)>
Public Structure HARDWAREINPUT
    Public uMsg As Integer
    Public wParamL As Short
    Public wParamH As Short
End Structure

<StructLayout(LayoutKind.Sequential)>
Public Structure KEYBDINPUT
    Public wVk As VirtualKeyShort
    Public wScan As ScanCodeShort
    Public dwFlags As KEYEVENTF
    Public time As Integer
    Public dwExtraInfo As UIntPtr
End Structure

<StructLayout(LayoutKind.Explicit)>
Public Structure InputUnion
    <FieldOffset(0)>
    Public mi As MOUSEINPUT
    <FieldOffset(0)>
    Public ki As KEYBDINPUT
    <FieldOffset(0)>
    Public hi As HARDWAREINPUT
End Structure

Public Structure KBDLLHOOKSTRUCT
    Public vkCode As Integer
    Public scancode As Integer
    Public flags As Integer
    Public time As Integer
    Public dwExtraInfo As Integer
End Structure
