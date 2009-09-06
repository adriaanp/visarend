Imports Visarend.Core.Model

Namespace Source
    Public Class TheMovieDbSource
        Implements IMovieSource

        Private _apiKey As String = "b262dea20df6523321f155800e5e0d3d"

        Private Const SEARCH As String = "http://api.themoviedb.org/2.1/Movie.search/en/xml/"
        Private Const GETINFO As String = "http://api.themoviedb.org/2.1/Movie.getInfo/en/xml/"
        Private Const IMDBLOOKUP As String = "http://api.themoviedb.org/2.1/Movie.imdbLookup/en/xml/"

        Public Function GetMovie(ByVal name As String) As MovieInfo Implements IMovieSource.GetMovie

            ' find match with 1 score
            Dim movieId = SearchMovie(name)
            ' retrieve that movie
            If movieId = -1 Then
                Return Nothing
            End If

            Return GetMovie(movieId)

        End Function

        Private Function GetMethodUrl(ByVal method As String, ByVal parameter As String) As String
            Return String.Format("{0}{1}/{2}", method, _apiKey, parameter)
        End Function

        Private Function SearchMovie(ByVal movieName As String) As Integer
            Dim uri As String = GetMethodUrl(SEARCH, movieName)
            Dim results As XElement = XElement.Load(uri)

            Dim top = From movie In results.Descendants("movie") _
                      Where CInt(movie.<score>.Value) = 1 _
                      Select movie

            If top.Count > 0 Then
                Return CInt(top.First().<id>.Value)
            End If

            Return -1
        End Function

        Private Function GetMovie(ByVal movieId As Integer) As MovieInfo
            Dim uri = GetMethodUrl(GETINFO, movieId.ToString())
            Dim result As XElement = XElement.Load(uri)
            Dim movieElement As XElement = result.Descendants("movie").First()

            Dim movie As New MovieInfo() With {.Name = movieElement.<name>.Value, _
                                               .Popularity = CInt(movieElement.<popularity>.Value), _
                                               .Overview = movieElement.<overview>.Value, _
                                               .Rating = CDbl(movieElement.<rating>.Value), _
                                               .RunTime = CInt(movieElement.<runtime>.Value), _
                                               .HomePageUrl = movieElement.<homepage>.Value, _
                                               .TrailerUrl = movieElement.<trailer>.Value}

            Dim genres = From genre In movieElement.Descendants("category") _
                         Where genre.@type = "genre" _
                         Select genre.@name
            movie.Genres = genres.ToList()

            Dim studios = From studio In movieElement.Descendants("studio") _
                          Select studio.@name
            movie.Studios = studios.ToList()

            Dim countries = From country In movieElement.Descendants("country") _
                            Select country.@name
            movie.Countries = countries.ToList()

            Dim poster = From image In movieElement.Descendants("image") _
                         Where image.@type = "poster" And image.@size = "mid" _
                         Order By image.@id _
                         Select New Artwork() With {.Uri = image.@url}
            movie.Poster = poster.FirstOrDefault()

            Dim backdrop = From image In movieElement.Descendants("image") _
                         Where image.@type = "backdrop" And image.@size = "original" _
                         Order By image.@id _
                         Select New Artwork() With {.Uri = image.@url}
            movie.Backdrop = backdrop.FirstOrDefault()

            Dim actors = From actor In movieElement.Descendants("person") _
                         Where actor.@job = "Actor" _
                         Select New Actor() With {.Name = actor.@name, .Character = actor.@character}
            movie.Actors = actors.ToList()

            Return movie
        End Function
    End Class

End Namespace
