Imports Visarend.Core.Model

Namespace Source
    Public Interface IMovieSource
        Function GetMovie(ByVal name As String) As MovieInfo
    End Interface
End Namespace