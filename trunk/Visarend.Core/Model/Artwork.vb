Imports System.IO
Imports System.Net
Imports System.Xml.Serialization
Imports System.ComponentModel
Imports System.Drawing

Namespace Model
    Public Class Artwork

        Private _uri As String
        Public Property Uri() As String
            Get
                Return _uri
            End Get
            Set(ByVal value As String)
                _uri = value
            End Set
        End Property

        'Private _imageByteArray As Byte()
        'Public Property ImageByteArray() As Byte()
        '    Get
        '        Return _imageByteArray
        '    End Get
        '    Set(ByVal value As Byte())
        '        _imageByteArray = value
        '        Dim stream = New MemoryStream(value)
        '        _image = New Bitmap(stream)
        '        stream.Close()
        '    End Set
        'End Property

        Private _image As Image
        Public Property ImageByteArray() As Byte()
            Get
                If Image Is Nothing Then Return Nothing
                Dim converter As TypeConverter = TypeDescriptor.GetConverter(Image.GetType)
                Return DirectCast(converter.ConvertTo(Image, GetType(Byte())), Byte())
            End Get
            Set(ByVal value As Byte())
                If value Is Nothing Then
                    Image = Nothing
                Else
                    Image = New Bitmap(New MemoryStream(value))
                End If
            End Set
        End Property

        <XmlIgnore()> _
        Public Property Image() As Image
            Get
                If _image Is Nothing And Not String.IsNullOrEmpty(Uri) Then
                    _image = LoadImage(Uri)
                End If
                Return _image
            End Get
            Set(ByVal value As Image)
                _image = value
            End Set
        End Property

        Friend Sub LoadImage()
            _image = LoadImage(Uri)
        End Sub

        Private Function LoadImage(ByVal uri As String) As Image
            Dim stream As New MemoryStream
            Dim web As New WebClient

            stream = New MemoryStream(web.DownloadData(uri))
            Return New Bitmap(stream)
        End Function

    End Class
End Namespace