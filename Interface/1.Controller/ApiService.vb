Imports System.Net.Http
Imports System.Net.Http.Headers
Imports Newtonsoft.Json
Imports System.Timers

Public Class ApiService
    Private ReadOnly _httpClient As HttpClient
    Private _jwtToken As String
    Private _tokenTimer As Timer

    Public Sub New()
        _httpClient = New HttpClient()
        _httpClient.BaseAddress = New Uri("https://localhost:7024/")
        _tokenTimer = New Timer(30 * 60 * 1000) ' 30 minutes
        AddHandler _tokenTimer.Elapsed, AddressOf RefreshToken
    End Sub

    Public Async Function GetCharactersAsync(gender As String, status As String) As Task(Of List(Of Character))
        If String.IsNullOrEmpty(_jwtToken) Then
            Await GetTokenAsync()
        End If

        Dim query = New List(Of String)
        If Not String.IsNullOrEmpty(gender) AndAlso gender <> "All" Then query.Add($"gender={gender}")
        If Not String.IsNullOrEmpty(status) AndAlso status <> "All" Then query.Add($"status={status}")

        Dim queryString = String.Join("&", query)
        Dim response = Await _httpClient.GetAsync($"api/Characters?{queryString}")

        response.EnsureSuccessStatusCode()
        Dim content = Await response.Content.ReadAsStringAsync()
        Return JsonConvert.DeserializeObject(Of List(Of Character))(content)
    End Function

    Public Async Function GetCharacterByIdAsync(characterId As Integer) As Task(Of Character)
        If String.IsNullOrEmpty(_jwtToken) Then
            Await GetTokenAsync()
        End If

        Dim response = Await _httpClient.GetAsync($"api/Characters/{characterId}")
        response.EnsureSuccessStatusCode()

        Dim content = Await response.Content.ReadAsStringAsync()
        Return JsonConvert.DeserializeObject(Of Character)(content)
    End Function

    Private Async Function GetTokenAsync() As Task
        Dim response = Await _httpClient.GetAsync("api/Auth/token")
        response.EnsureSuccessStatusCode()

        Dim content = Await response.Content.ReadAsStringAsync()
        Dim tokenResponse = JsonConvert.DeserializeObject(Of Dictionary(Of String, String))(content)
        _jwtToken = tokenResponse("token")

        _httpClient.DefaultRequestHeaders.Authorization = New AuthenticationHeaderValue("Bearer", _jwtToken)
        _tokenTimer.Start()
    End Function

    Private Async Sub RefreshToken(sender As Object, e As ElapsedEventArgs)
        Await GetTokenAsync()
    End Sub
End Class
