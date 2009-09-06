Imports System.Xml.Serialization
Imports System.IO
Imports Visarend.Core.Model

Namespace Data
    Public Class MovieInfoPersister
        Implements IMovieInfoPersister

        Public Sub Save(ByVal movie As MovieInfo, ByVal filename As String) Implements IMovieInfoPersister.Save
            Dim xmlSerial As New XmlSerializer(GetType(MovieInfo))
            Dim stream = New FileStream(filename, FileMode.Create)
            xmlSerial.Serialize(stream, movie)
            stream.Close()
        End Sub

        Public Function Load(ByVal filename As String) As MovieInfo Implements IMovieInfoPersister.Load
            Dim xmlSerial As New XmlSerializer(GetType(MovieInfo))
            Dim stream = New FileStream(filename, FileMode.Open)
            Dim movie As MovieInfo = DirectCast(xmlSerial.Deserialize(stream), MovieInfo)
            stream.Close()
            Return movie
        End Function
    End Class
End Namespace