Imports Visarend.Core.Model
Imports System.IO


Public Class DefaultMovieNameFinder
    Implements IMovieNameFinder

    Public Function GetMovieNameFromFileName(ByVal fileName As String) As String Implements IMovieNameFinder.GetMovieNameFromFileName
        Return Path.GetFileNameWithoutExtension(fileName)
    End Function
End Class
