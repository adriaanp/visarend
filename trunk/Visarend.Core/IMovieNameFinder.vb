Imports Visarend.Core.Model
Imports System.IO

Public Interface IMovieNameFinder
    Function GetMovieNameFromFileName(ByVal fileName As String) As String
End Interface
