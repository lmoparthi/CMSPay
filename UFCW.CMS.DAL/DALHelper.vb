Imports System.Configuration

Public Class DALSchemaHelper
    Private Const ALT_Q_POSTFIX_FOR_CONFIG As String = "_ALT_Q_Schema"
    Private Const ALT_Q_SPROC_POSTFIX_FOR_CONFIG As String = "_ALT_Q_SPROC_Schema"
    Public Shared Function GetSchemaFromConfig(Schema As String) As String

        Dim useAltQScehma As Boolean = False
        If ConfigurationManager.AppSettings("UseAltQSchema") IsNot Nothing Then
            useAltQScehma = CBool(ConfigurationManager.AppSettings("UseAltQSchema"))
        End If

        If Not useAltQScehma Then Return Schema

        Schema = String.Format("{0}{1}", Schema, ALT_Q_POSTFIX_FOR_CONFIG)
        Return ConfigurationManager.AppSettings(Schema).ToString
    End Function

    Public Shared ReadOnly Property UseALTQSchemaForSPROCs As Boolean
        Get
            Return ConfigurationManager.AppSettings("UseAltQSchema") IsNot Nothing AndAlso
                CBool(ConfigurationManager.AppSettings("UseAltQSchema"))

        End Get
    End Property

    Public Shared Function GetSPROCSchemaFromConfig(Schema As String) As String

        If Not UseALTQSchemaForSPROCs Then Return Schema

        Schema = String.Format("{0}{1}", Schema, ALT_Q_SPROC_POSTFIX_FOR_CONFIG)
        Return ConfigurationManager.AppSettings(Schema).ToString

    End Function

End Class
