Imports Microsoft.VisualBasic
Imports System.Windows.Controls
Imports System.IO
Imports System.Xml.Serialization
Imports DevExpress.Xpf.PivotGrid

Namespace DXPivotGrid_GroupFilter
	Partial Public Class MainPage
		Inherits UserControl
        Private dataFileName As String = "nwind.xml"
		Public Sub New()
			InitializeComponent()

			' Parses an XML file and creates a collection of data items.
            Dim assembly As System.Reflection.Assembly = _
                System.Reflection.Assembly.GetExecutingAssembly()
            Dim stream As Stream = assembly.GetManifestResourceStream(dataFileName)
			Dim s As New XmlSerializer(GetType(OrderData))
			Dim dataSource As Object = s.Deserialize(stream)

			' Binds a pivot grid to this collection.
			pivotGridControl1.DataSource = dataSource
		End Sub
        Private Sub LayoutRoot_Loaded(ByVal sender As Object, _
                                      ByVal e As System.Windows.RoutedEventArgs)
            fieldMonth.CollapseAll()
        End Sub
        Private Sub listBox1_SelectedIndexChanged(ByVal sender As Object, _
                                                  ByVal e As System.Windows.RoutedEventArgs)
            If pivotGridControl1 Is Nothing Then
                Return
            End If
            Me.pivotGridControl1.BeginUpdate()
            Dim group As PivotGridGroup = Me.pivotGridControl1.Groups(0)
            group.FilterValues.Reset()
            group.FilterValues.BeginUpdate()
            Select Case listBox1.SelectedIndex
                Case 0
                    group.FilterValues.FilterType = FieldFilterType.Excluded
                Case 1
                    group.FilterValues.FilterType = FieldFilterType.Included
                    group.FilterValues.Values.Add(1994).ChildValues.Add(12)
                    group.FilterValues.Values.Add(1995).ChildValues.Add(1)
                Case 2
                    group.FilterValues.FilterType = FieldFilterType.Excluded
                    group.FilterValues.Values.Add(1994)
                Case 3
                    group.FilterValues.FilterType = FieldFilterType.Included
                    SelectFirstDays(group)
            End Select
            group.FilterValues.EndUpdate()
            Me.pivotGridControl1.EndUpdate()
            If listBox1.SelectedIndex = 3 Then
                fieldMonth.ExpandAll()
            Else
                fieldMonth.CollapseAll()
            End If
        End Sub
		Private Sub SelectFirstDays(ByVal group As PivotGridGroup)
			For Each year As Object In group.GetUniqueValues(Nothing)
                Dim value As DevExpress.XtraPivotGrid.PivotGroupFilterValue = _
                    group.FilterValues.Values.Add(year)
				For Each month As Object In group.GetUniqueValues(New Object() { year })
					value.ChildValues.Add(month).ChildValues.Add(1)
				Next month
			Next year
		End Sub

	End Class
End Namespace