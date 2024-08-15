Imports System.Net.Http
Imports System.IO

Public Class DetailForm
    Private ReadOnly _characterId As Integer

    Public Sub New(characterId As Integer)
        InitializeComponent()
        _characterId = characterId
    End Sub

    Private Async Sub DetailForm_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Dim apiService = New ApiService()
        Dim character = Await apiService.GetCharacterByIdAsync(_characterId)

        If Not String.IsNullOrEmpty(character.Image) Then
            Try
                Using client As New HttpClient()
                    Dim imageBytes As Byte() = Await client.GetByteArrayAsync(character.Image)
                    Using ms As New MemoryStream(imageBytes)
                        PictureBox1.Image = Image.FromStream(ms)
                    End Using
                End Using
            Catch ex As Exception
                MessageBox.Show("Failed to load image: " & ex.Message)
            End Try
        End If

        CharacterLabel.Text = character.Name
        ListBox1.Items.Clear()

        If character.Episode IsNot Nothing Then
            For Each episode As String In character.Episode
                ListBox1.Items.Add(episode)
            Next
        End If
    End Sub
End Class
