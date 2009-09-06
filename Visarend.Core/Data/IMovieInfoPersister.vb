Imports System.Xml.Serialization
Imports System.IO
Imports Visarend.Core.Model

Namespace Data
    Public Interface IMovieInfoPersister
        Sub Save(ByVal movie As MovieInfo, ByVal filename As String)
        Function Load(ByVal filename As String) As MovieInfo
    End Interface
End Namespace
