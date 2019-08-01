'
' VB.NET
' https://archsmarter.com/portfolio/renumber-sheets/
' Created by SharpDevelop.
' User: ArchSmarter.com
' Date: 10/2/2015
' Time: 10:35 AM
' 
' ArchSmarter Revit Macro Tempate - Revit 2016 - Version 1.0
'
Imports System
Imports Autodesk.Revit.UI
Imports Autodesk.Revit.DB
Imports Autodesk.Revit.UI.Selection
Imports System.Collections.Generic
Imports System.Linq
Imports System.Diagnostics
imports System.Globalization

<Autodesk.Revit.Attributes.Transaction(Autodesk.Revit.Attributes.TransactionMode.Manual)> _
<Autodesk.Revit.DB.Macros.AddInId("22B7418D-5AFC-4651-993A-67A14F2B9DE3")> _
Partial Public Class ThisDocument
	
	Sub RenumberSheets()
		'define current app and document
		Dim curUIApp As UIApplication = Me.Application
		Dim curDoc As Document = curUIApp.ActiveUIDocument.Document
		
		'set counter
		Dim counter As Integer = 0

		'open form
		Using curForm As New frmForm(curDoc)
			'show form
			curForm.ShowDialog
			
			If curForm.DialogResult = System.Windows.Forms.DialogResult.Cancel Then
				'Cancel pressed - do something
				Exit Sub
			Else
				'OK pressed - renumber sheets
				Dim sheetList As List(Of ViewSheet) = mCollectorsAll.getAllSheetsByNumber(curDoc)
				
				'get sheet range
				Dim startRange As Integer 
				Dim endRange As Integer
						
				'get start number
				Dim startNum As Integer = curForm.getStartNum
				
				If startNum = 0 Then
					Exit Sub
				End If
				
				'get sheet prefix
				Dim sheetPrefix As String = curForm.getPrefix
				
				'get sheet step
				Dim sheetStep As Integer = curForm.getStep
				
				'loop through sheet list and find start and end sheets
				For Each curSheet As ViewSheet In sheetList
					If curSheet.SheetNumber = curForm.getStartSheet Then
						startRange = counter
					ElseIf curSheet.SheetNumber = curForm.getEndSheet Then
						endRange = counter
					End If
					
					'increment counter
					counter = counter + 1
				Next
				
				'NEED TO UPDATE THE NUMBERS TWICE TO AVOID DUPLICATE SHEET NUMBER ISSUE
				'start transaction
				Using curTrans As New Transaction(curDoc, "Renumber Sheets")
					If curTrans.Start = TransactionStatus.Started Then	

						'update sheet numbers - first time
						Dim curNum As Integer = startNum
						For i = startRange To endRange
							'update sheet number
							Dim newSheetNum As String = sheetPrefix & CStr(curNum) & "Z"
							mParameters.setParameterValueString(sheetList(i), "Sheet Number", newSheetNum)
							
							'increment stuff
							curNum = curNum + sheetStep
						Next i
						
						'update sheet numbers - second time
						Dim curNum2 As Integer = startNum
						For j = startRange To endRange
							'update sheet number
							Dim newSheetNum2 As String = sheetPrefix & CStr(curNum2)
							mParameters.setParameterValueString(sheetList(j), "Sheet Number", newSheetNum2)
							
							'increment stuff
							curNum2 = curNum2 + sheetStep
						Next j
					End If
							
					'commit changes
					curTrans.Commit
					curTrans.Dispose
				End Using
			End If 
		End Using

	End Sub

End Class
