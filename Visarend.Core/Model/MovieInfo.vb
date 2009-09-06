Namespace Model
    Public Class MovieInfo
        Private _popularity As Integer
        Public Property Popularity() As Integer
            Get
                Return _popularity
            End Get
            Set(ByVal value As Integer)
                _popularity = value
            End Set
        End Property

        Private _name As String
        Public Property Name() As String
            Get
                Return _name
            End Get
            Set(ByVal value As String)
                _name = value
            End Set
        End Property

        Private _overview As String
        Public Property Overview() As String
            Get
                Return _overview
            End Get
            Set(ByVal value As String)
                _overview = value
            End Set
        End Property

        Private _rating As Double
        Public Property Rating() As Double
            Get
                Return _rating
            End Get
            Set(ByVal value As Double)
                _rating = value
            End Set
        End Property

        Private _runTime As Integer
        Public Property RunTime() As Integer
            Get
                Return _runTime
            End Get
            Set(ByVal value As Integer)
                _runTime = value
            End Set
        End Property

        Private _homePageUrl As String
        Public Property HomePageUrl() As String
            Get
                Return _homePageUrl
            End Get
            Set(ByVal value As String)
                _homePageUrl = value
            End Set
        End Property

        Private _trailerUrl As String
        Public Property TrailerUrl() As String
            Get
                Return _trailerUrl
            End Get
            Set(ByVal value As String)
                _trailerUrl = value
            End Set
        End Property

        Private _genres As List(Of String)
        Public Property Genres() As List(Of String)
            Get
                Return _genres
            End Get
            Set(ByVal value As List(Of String))
                _genres = value
            End Set
        End Property

        Private _studios As List(Of String)
        Public Property Studios() As List(Of String)
            Get
                Return _studios
            End Get
            Set(ByVal value As List(Of String))
                _studios = value
            End Set
        End Property

        Private _countries As List(Of String)
        Public Property Countries() As List(Of String)
            Get
                Return _countries
            End Get
            Set(ByVal value As List(Of String))
                _countries = value
            End Set
        End Property


        Private _poster As Artwork
        Public Property Poster() As Artwork
            Get
                Return _poster
            End Get
            Set(ByVal value As Artwork)
                _poster = value
            End Set
        End Property

        Private _backdrop As Artwork
        Public Property Backdrop() As Artwork
            Get
                Return _backdrop
            End Get
            Set(ByVal value As Artwork)
                _backdrop = value
            End Set
        End Property

        Private _actors As List(Of Actor)
        Public Property Actors() As List(Of Actor)
            Get
                Return _actors
            End Get
            Set(ByVal value As List(Of Actor))
                _actors = value
            End Set
        End Property

    End Class
End Namespace
