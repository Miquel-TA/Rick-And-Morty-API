﻿Public Class MainForm
    Private Async Sub MainForm_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        ComboBox1.SelectedItem = "All"
        ComboBox2.SelectedItem = "All"
        Await LoadCharactersAsync()
        AddHandler ComboBox1.SelectedIndexChanged, AddressOf ComboBox_SelectedIndexChanged
        AddHandler ComboBox2.SelectedIndexChanged, AddressOf ComboBox_SelectedIndexChanged
    End Sub

    Private Async Sub ComboBox_SelectedIndexChanged(sender As Object, e As EventArgs)
        Await LoadCharactersAsync()
    End Sub

    Private Async Function LoadCharactersAsync() As Task
        Dim apiService = New ApiService()
        Dim selectedGender = If(ComboBox1.SelectedItem?.ToString() = "All", "", ComboBox1.SelectedItem?.ToString())
        Dim selectedStatus = If(ComboBox2.SelectedItem?.ToString() = "All", "", ComboBox2.SelectedItem?.ToString())

        Dim characters = Await apiService.GetCharactersAsync(selectedGender, selectedStatus)
        DataGridView1.DataSource = characters.Select(Function(c) New With {
            .Id = c.Id,
            .Name = c.Name,
            .Gender = c.Gender,
            .Status = c.Status
        }).ToList()

        If DataGridView1.Columns("DetailsButton") Is Nothing Then
            Dim buttonColumn As New DataGridViewButtonColumn()
            buttonColumn.Name = "DetailsButton"
            buttonColumn.HeaderText = "Details"
            buttonColumn.Text = "View Details"
            buttonColumn.UseColumnTextForButtonValue = True
            DataGridView1.Columns.Add(buttonColumn)
        End If
    End Function

    Private Sub DataGridView1_CellContentClick(sender As Object, e As DataGridViewCellEventArgs) Handles DataGridView1.CellContentClick
        If e.ColumnIndex = DataGridView1.Columns("DetailsButton").Index AndAlso e.RowIndex >= 0 Then
            Dim characterId = Convert.ToInt32(DataGridView1.Rows(e.RowIndex).Cells("Id").Value)
            Dim detailForm = New DetailForm(characterId)
            detailForm.ShowDialog()
        End If
    End Sub
End Class
