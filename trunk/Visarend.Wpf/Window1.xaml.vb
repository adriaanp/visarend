Imports Visarend.Core
Imports System.IO
Imports Visarend.Core.Data

Class Window1

    Private WithEvents _movieLoader As MovieInfoLoader = New MovieInfoLoader()

    Private Sub Button_Click(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs)
        Dim scanner As New DirectoryScanner()

        Dim list = scanner.ScanDirectory(folderTextBox.Text)

        ListBox.ItemsSource = list

        For Each file In list.Where(Function(m) m.MovieInfo Is Nothing)
            Debug.WriteLine("load info...")
            _movieLoader.GetMovieAsync(file.MovieNameFromFile, file)
        Next
    End Sub

    Public Sub _movieLoader_GetMovieCompleted(ByVal sender As Object, ByVal e As GetMovieCompletedEventArgs) Handles _movieLoader.GetMovieCompleted
        If Not e.Cancelled Or e.Error IsNot Nothing Then
            Dim movie = DirectCast(e.UserState, MovieFile)
            movie.MovieInfo = e.Result
            Debug.WriteLine("movie info loaded")
        End If
    End Sub

    Private Sub ListBox_SelectionChanged(ByVal sender As System.Object, ByVal e As System.Windows.Controls.SelectionChangedEventArgs)
        Dim movie As MovieFile = DirectCast(ListBox.SelectedItem, MovieFile)
        If movie Is Nothing Then Return
        If movie.MovieInfo Is Nothing Then Return

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

    Private Sub btnGetInfo_Click(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs)
        Dim movie As MovieFile = DirectCast(ListBox.SelectedItem, MovieFile)
        If movie Is Nothing Then Return
        If movie.MovieInfo Is Nothing Then Return

        Dim persister As New MovieInfoPersister
        persister.Save(movie.MovieInfo, Path.Combine(Path.GetDirectoryName(movie.MovieFileName), Path.GetFileNameWithoutExtension(movie.MovieFileName) + ".xml"))
    End Sub
End Class
