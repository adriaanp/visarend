Imports Visarend.Core.Model
Imports System.IO
Imports Visarend.Core.Data

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

    Private _movieNameFinder As IMovieNameFinder = New DefaultMovieNameFinder()
    Private _persister As IMovieInfoPersister = New MovieInfoPersister()

    Public Function ScanDirectory(ByVal folder As String) As List(Of MovieFile)
        Dim movieFiles As New List(Of MovieFile)

        Dim files = Directory.GetFiles(folder, "*.avi")

        For Each file In files
            movieFiles.Add(GetMovieFile(file))
        Next

        Return movieFiles
    End Function

    Private Function GetMovieFile(ByVal filename As String) As MovieFile
        Dim moviefile As New MovieFile

        Dim movieName As String = _movieNameFinder.GetMovieNameFromFileName(filename)
        If String.IsNullOrEmpty(movieName) Then
            Return Nothing
        End If
        moviefile.FileNames = New String() {filename}
        moviefile.MovieFileName = filename

        moviefile.MovieInfo = GetMovieInfoFromFile(filename)

        Return moviefile
    End Function

    Private Function GetMovieInfoFromFile(ByVal filename As String) As MovieInfo
        Dim movieInfoFile = Path.Combine(Path.GetDirectoryName(filename), Path.GetFileNameWithoutExtension(filename) + ".xml")
        If Not File.Exists(movieInfoFile) Then
            Return Nothing
        End If
        Return _persister.Load(movieInfoFile)
    End Function

End Class

Public Class MovieFile

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
