Imports System.Windows.Forms
Imports System.Text
Imports System.Reflection
Imports System.Linq
Imports System.ComponentModel
Imports System.Collections.Generic
Imports System

' <copyright file="Extensions.cs" company="Brown University">
' Copyright (c) 2009 by John Mertus
' </copyright>
' <author>John Mertus</author>
' <date>10/31/2009 9:30:22 AM</date>
' <summary></summary>


''' <summary>
''' This is a set of extensions for accessing the Event Handlers as well as cloning menu items
''' </summary>
Public Module Extensions
		'////////////////////////////////////////////////
		' Private static fields
		'////////////////////////////////////////////////
		#Region "Public static methods"

		''' <summary>
		''' This contains a counter to help make names unique
		''' </summary>
		Private menuNameCounter As Integer = 0

		#End Region

		'////////////////////////////////////////////////
		' Public static methods
		'////////////////////////////////////////////////
		#Region "Public static methods"

		''' <summary>
		''' Clones the specified source tool strip menu item. 
		''' </summary>
		''' <param name="sourceToolStripMenuItem">The source tool strip menu item.</param>
		''' <returns>A cloned version of the toolstrip menu item</returns>
		<System.Runtime.CompilerServices.Extension> _
		Public Function Clone(ByVal sourceToolStripMenuItem As ToolStripMenuItem) As ToolStripMenuItem
			Dim menuItem As New ToolStripMenuItem()



			Dim propInfoList = From p In GetType(ToolStripMenuItem).GetProperties()
							   Let attributes = p.GetCustomAttributes(True)
							   Let notBrowsable = (From a In attributes
												   Where a.GetType() = GetType(BrowsableAttribute)
												   Select Not CType(a, BrowsableAttribute).Browsable).FirstOrDefault()
							   Where Not notBrowsable AndAlso p.CanRead AndAlso p.CanWrite AndAlso p.Name <> "DropDown"
							   Order By p.Name
							   Select p

			'Dim propInfoList = From p In GetType(ToolStripMenuItem).GetProperties()
			'	Let attributes = p.GetCustomAttributes(True)
			'	Let notBrowseable = (
			'		From a In attributes
			'		Where a.GetType() Is GetType(BrowsableAttribute) [select] (Not (TryCast(a, BrowsableAttribute)).Browsable)).FirstOrDefault()
			'	Where Not notBrowseable AndAlso p.CanRead AndAlso p.CanWrite AndAlso p.Name <> "DropDown"
			'	Order By p.Name
			'	Select p

			' Copy over using reflections
			For Each propertyInfo In propInfoList
				Dim propertyInfoValue As Object = propertyInfo.GetValue(sourceToolStripMenuItem, Nothing)
				propertyInfo.SetValue(menuItem, propertyInfoValue, Nothing)
			Next propertyInfo

			' Create a new menu name
			menuItem.Name = sourceToolStripMenuItem.Name '+ "-" + menuNameCounter++;

			' Process any other properties
			If sourceToolStripMenuItem.ImageIndex <> -1 Then
				menuItem.ImageIndex = sourceToolStripMenuItem.ImageIndex
			End If

			If Not String.IsNullOrEmpty(sourceToolStripMenuItem.ImageKey) Then
				menuItem.ImageKey = sourceToolStripMenuItem.ImageKey
			End If

			' We need to make this visible 
			menuItem.Visible = True

			' Recursively clone the drop down list
			For Each item In sourceToolStripMenuItem.DropDownItems
				Dim newItem As ToolStripItem
				If TypeOf item Is ToolStripMenuItem Then
					newItem = CType(item, ToolStripMenuItem).Clone()
				ElseIf TypeOf item Is ToolStripSeparator Then
					newItem = New ToolStripSeparator()
				Else
					Throw New NotImplementedException("Menu item is not a ToolStripMenuItem or a ToolStripSeparatorr")
				End If

				menuItem.DropDownItems.Add(newItem)
			Next item

			' The handler list starts empty because we created its parent via a new
			' So this is equivalen to a copy.
			menuItem.AddHandlers(sourceToolStripMenuItem)

			Return menuItem
		End Function

		''' <summary>
		''' Adds the handlers from the source component to the destination component
		''' </summary>
		''' <typeparam name="T">An IComponent type</typeparam>
		''' <param name="destinationComponent">The destination component.</param>
		''' <param name="sourceComponent">The source component.</param>
		<System.Runtime.CompilerServices.Extension> _
		Public Sub AddHandlers(Of T As IComponent)(ByVal destinationComponent As T, ByVal sourceComponent As T)
			' If there are other handlers, they will not be erased
			Dim destEventHandlerList = destinationComponent.GetEventHandlerList()
			Dim sourceEventHandlerList = sourceComponent.GetEventHandlerList()

			destEventHandlerList.AddHandlers(sourceEventHandlerList)
		End Sub

		''' <summary>
		''' Gets the event handler list from a component
		''' </summary>
		''' <param name="component">The source component.</param>
		''' <returns>The EventHanderList or null if none</returns>
		<System.Runtime.CompilerServices.Extension> _
		Public Function GetEventHandlerList(ByVal component As IComponent) As EventHandlerList
			Dim eventsInfo = DirectCast(component, Object).GetType().GetProperty("Events", BindingFlags.Instance Or BindingFlags.NonPublic)
			Return DirectCast(eventsInfo.GetValue(component, Nothing), EventHandlerList)
		End Function

		#End Region

		'////////////////////////////////////////////////
		' Private static methods
		'////////////////////////////////////////////////
	End Module

