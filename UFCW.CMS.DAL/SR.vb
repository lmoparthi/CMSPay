'===============================================================================
' Microsoft patterns & practices Enterprise Library
' Logging and Instrumentation Application Block
'===============================================================================
' Copyright © 2004 Microsoft Corporation.  All rights reserved.
' THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
' OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
' LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
' FITNESS FOR A PARTICULAR PURPOSE.
'===============================================================================

Imports System.Globalization
Imports System.Resources

Namespace EnterpriseLibrariesLogging

    Friend Class SR

        Public Shared Property Culture() As CultureInfo

            Get
                Return Keys.Culture
            End Get

            Set(ByVal value As CultureInfo)
                Keys.Culture = Value
            End Set
        End Property

        Public Shared ReadOnly Property EventProcessedMessage(ByVal id As Integer) As String
            Get
                Return Keys.GetString(Keys.EventProcessedMessage, id)
            End Get
        End Property

        Public Shared ReadOnly Property DefaultCategoryMessage() As String
            Get
                Return Keys.GetString(Keys.DefaultCategoryMessage)
            End Get
        End Property

        Public Shared ReadOnly Property TracingCategoryMessage(ByVal filename As String) As String
            Get
                Return Keys.GetString(Keys.TracingCategoryMessage, filename)
            End Get
        End Property

        Public Shared ReadOnly Property InvalidDataMessage() As String
            Get
                Return Keys.GetString(Keys.InvalidDataMessage)
            End Get
        End Property

        Public Shared ReadOnly Property InvalidEventIDMessage() As String
            Get
                Return Keys.GetString(Keys.InvalidEventIDMessage)
            End Get
        End Property

        Public Shared ReadOnly Property InvalidPriorityMessage() As String
            Get
                Return Keys.GetString(Keys.InvalidPriorityMessage)
            End Get
        End Property

        Public Shared ReadOnly Property InvalidCategoryMessage() As String
            Get
                Return Keys.GetString(Keys.InvalidCategoryMessage)
            End Get
        End Property

        Public Shared ReadOnly Property EnterpriseLibrariesLoggingTitleMessage() As String
            Get
                Return Keys.GetString(Keys.EnterpriseLibrariesLoggingTitleMessage)
            End Get
        End Property

        Public Shared ReadOnly Property LogEventStartMessage(ByVal eventId As Integer, ByVal msg As String, ByVal cat As String, ByVal pri As Integer) As String
            Get
                Return Keys.GetString(Keys.LogEventStartMessage, eventId, msg, cat, pri)
            End Get
        End Property

        Public Shared ReadOnly Property TraceStartMessage() As String
            Get
                Return Keys.GetString(Keys.TraceStartMessage)
            End Get
        End Property

        Public Shared ReadOnly Property TraceDoneMessage(ByVal filename As String) As String
            Get
                Return Keys.GetString(Keys.TraceDoneMessage, filename)
            End Get
        End Property
        Public Shared ReadOnly Property ExtraInformationStartMessage() As String
            Get
                Return Keys.GetString(Keys.ExtraInformationStartMessage)
            End Get
        End Property

        Public Shared ReadOnly Property ExtraInformationEndMessage() As String
            Get
                Return Keys.GetString(Keys.ExtraInformationEndMessage)
            End Get
        End Property

        Public Shared ReadOnly Property SinkFailureMessage(ByVal exceptionString As String) As String
            Get
                Return Keys.GetString(Keys.SinkFailureMessage, exceptionString)
            End Get
        End Property

        Public Shared ReadOnly Property DebugSinkTestMessage() As String
            Get
                Return Keys.GetString(Keys.DebugSinkTestMessage)
            End Get
        End Property

        Public Shared ReadOnly Property DebugSinkMessage(ByVal id As Integer, ByVal msg As String) As String
            Get
                Return Keys.GetString(Keys.DebugSinkMessage, id, msg)
            End Get
        End Property

        Public Shared ReadOnly Property CustomizedSinkMessage() As String
            Get
                Return Keys.GetString(Keys.CustomizedSinkMessage)
            End Get
        End Property

        Friend Class Keys

            Private Shared resManager As ResourceManager = _
            New ResourceManager("EnterpriseLibrariesLogging.SR", GetType(SR).Module.Assembly)

            Private Shared cultureInformation As CultureInfo = Nothing

            Public Shared Property Culture() As CultureInfo
                Get
                    Return cultureInformation
                End Get
                Set(ByVal value As CultureInfo)
                    cultureInformation = Value
                End Set
            End Property

            Public Shared Function GetString(ByVal key As String) As String
                Return resManager.GetString(key, Culture)
            End Function

            Public Shared Function GetString(ByVal key As String, ByVal paramArray args As Object()) As String
                Dim msg As String = resManager.GetString(key, Culture)
                msg = String.Format(msg, args)
                Return msg
            End Function

            Public Const EventProcessedMessage As String = "EventProcessedMessage"
            Public Const DefaultCategoryMessage As String = "DefaultCategoryMessage"
            Public Const TracingCategoryMessage As String = "TracingCategoryMessage"
            Public Const CachingCategoryMessage As String = "CachingCategoryMessage"
            Public Const QueryCategoryMessage As String = "QueryCategoryMessage"
            Public Const ExceptionCategoryMessage As String = "ExceptionCategoryMessage"
            Public Const InvalidDataMessage As String = "InvalidDataMessage"
            Public Const InvalidEventIDMessage As String = "InvalidEventIDMessage"
            Public Const InvalidPriorityMessage As String = "InvalidPriorityMessage"
            Public Const InvalidCategoryMessage As String = "InvalidCategoryMessage"
            Public Const EnterpriseLibrariesLoggingTitleMessage As String = "EnterpriseLibrariesLoggingTitleMessage"
            Public Const LogEventStartMessage As String = "LogEventStartMessage"
            Public Const TraceStartMessage As String = "TraceStartMessage"
            Public Const TraceDoneMessage As String = "TraceDoneMessage"
            Public Const ExtraInformationStartMessage As String = "ExtraInformationStartMessage"
            Public Const ExtraInformationEndMessage As String = "ExtraInformationEndMessage"
            Public Const SinkFailureMessage As String = "SinkFailureMessage"
            Public Const DebugSinkTestMessage As String = "DebugSinkTestMessage"
            Public Const DebugSinkMessage As String = "DebugSinkMessage"
            Public Const CustomizedSinkMessage As String = "CustomizedSinkMessage"

        End Class

    End Class
End Namespace