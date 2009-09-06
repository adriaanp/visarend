Imports Visarend.Core
Imports System.IO

Class Window1

    Private Sub Button_Click(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs)
        Dim scanner As New DirectoryScanner()

        Dim list = scanner.ScanDirectory(folderTextBox.Text)

        ListBox.ItemsSource = list

    End Sub

    Private Sub ListBox_SelectionChanged(ByVal sender As System.Object, ByVal e As System.Windows.Controls.SelectionChangedEventArgs)
        Dim movie As MovieFile = DirectCast(ListBox.SelectedItem, MovieFile)
        If movie Is Nothing Then Return
        Me.DataContext = movie.MovieInfo
        Dim bitmap As BitmapImage = New BitmapImage()
        bitmap.BeginInit()
        bitmap.StreamSource = New MemoryStream(movie.MovieInfo.Backdrop.ImageByteArray)
        bitmap.EndInit()
        backDropImage.Source = bitmap 'movie.MovieInfo.Backdrop.Image

        bitmap = New BitmapImage()
        bitmap.BeginInit()
        bitmap.StreamSource = New MemoryStream(movie.MovieInfo.Poster.ImageByteArray)
        bitmap.EndInit()
        posterImage.Source = bitmap
    End Sub
End Class
