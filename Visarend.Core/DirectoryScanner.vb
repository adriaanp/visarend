Imports Visarend.Core.Model
Imports System.IO
Imports Visarend.Core.Data
Imports Visarend.Core.Source

''' <summary>
''' Scans directory for all movies... and maybe bring back directory and file
''' </summary>
''' <remarks>
''' 1. Need to figure out the movie title
''' 2. Need to do the logic for files and directories
''' 3. Should bring back already saved info about movies
''' 4. 
''' </remarks>
Public Class DirectoryScanner

    Private _persister As IMovieInfoPersister = New MovieInfoPersister()
    'Private _movieSource As IMovieSource = New TheMovieDbSource()

    Public Function ScanDirectory(ByVal folder As String) As List(Of MovieFile)
        Dim movieFiles As New List(Of MovieFile)

        Dim files = Directory.GetFiles(folder, "*.avi")

        For Each file In files
            movieFiles.Add(GetMovieFile(file))
        Next

        Return movieFiles
    End Function

    Private Function GetMovieFile(ByVal filename As String) As MovieFile
        Dim moviefile As New MovieFile(filename)

        If String.IsNullOrEmpty(moviefile.MovieNameFromFile) Then
            Return Nothing
        End If

        moviefile.FileNames = New String() {filename}

        moviefile.MovieInfo = GetMovieInfoFromFile(filename)
        'If moviefile.MovieInfo Is Nothing Then 'try loading it
        '    'todo: need to load this on seperate thread, or we need to handle it outside this class???
        '    moviefile.MovieInfo = GetMovieInfoFromSource(moviefile.MovieNameFromFile)
        '    If moviefile.MovieInfo IsNot Nothing Then
        '        SaveMovieInfo(moviefile.MovieInfo, filename)
        '    End If
        'End If

        Return moviefile
    End Function

    Private Function GetMovieInfoXmlFileName(ByVal filename As String) As String
        Dim movieInfoFile = Path.Combine(Path.GetDirectoryName(filename), Path.GetFileNameWithoutExtension(filename) + ".xml")
        Return movieInfoFile
    End Function

    Private Function GetMovieInfoFromFile(ByVal filename As String) As MovieInfo
        Dim movieInfoFile As String = GetMovieInfoXmlFileName(filename)
        If Not File.Exists(movieInfoFile) Then
            Return Nothing
        End If
        Return _persister.Load(movieInfoFile)
    End Function

    'Private Function GetMovieInfoFromSource(ByVal movieName As String) As MovieInfo
    '    Dim info = _movieSource.GetMovie(movieName)
    '    Return info
    'End Function

    'Private Sub SaveMovieInfo(ByVal movieInfo As MovieInfo, ByVal filename As String)
    '    Dim xmlFilename = GetMovieInfoXmlFileName(filename)
    '    _persister.Save(movieInfo, xmlFilename)
    'End Sub

End Class

Public Class MovieFile

    Public Sub New(ByVal filename As String)
        _movieFileName = filename
    End Sub

    Private _fileNames As String()
    Public Property FileNames() As String()
        Get
            Return _fileNames
        End Get
        Set(ByVal value As String())
            _fileNames = value
        End Set
    End Property

    Private _movieFileName As String
    Public Property MovieFileName() As String
        Get
            Return _movieFileName
        End Get
        Set(ByVal value As String)
            _movieFileName = value
        End Set
    End Property

    Private _movieNameFinder As IMovieNameFinder = New DefaultMovieNameFinder()

    Public ReadOnly Property MovieNameFromFile() As String
        Get
            Return _movieNameFinder.GetMovieNameFromFileName(MovieFileName)
        End Get
    End Property


    Private _movieInfo As MovieInfo
    Public Property MovieInfo() As MovieInfo
        Get
            Return _movieInfo
        End Get
        Set(ByVal value As MovieInfo)
            _movieInfo = value
        End Set
    End Property

End Class
