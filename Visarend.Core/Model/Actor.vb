Public Class Actor

    Private _name As String
    Public Property Name() As String
        Get
            Return _name
        End Get
        Set(ByVal value As String)
            _name = value
        End Set
    End Property

    Private _character As String
    Public Property Character() As String
        Get
            Return _character
        End Get
        Set(ByVal value As String)
            _character = value
        End Set
    End Property

End Class
