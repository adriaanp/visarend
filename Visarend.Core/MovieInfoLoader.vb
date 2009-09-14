Imports Visarend.Core.Model
Imports System.IO
Imports Visarend.Core.Data
Imports Visarend.Core.Source
Imports System.ComponentModel
Imports System.Collections.Specialized
Imports System.Threading

''' <summary>
''' Movie info loader facade to UI. Supports async loading of info.
''' </summary>
''' <remarks></remarks>
Public Class MovieInfoLoader
    Private _movieSource As IMovieSource = New TheMovieDbSource()
    Private _userStateToLifetime As HybridDictionary = New HybridDictionary()

    Public Function GetMovieInfo(ByVal movieName As String) As MovieInfo
        Return _movieSource.GetMovie(movieName)
    End Function

    'Public Sub GetMovieAsync(ByVal movieName As String)

    'End Sub

    Public Sub GetMovieAsync(ByVal movieName As String, ByVal userState As Object)

        Dim asyncOp As AsyncOperation = AsyncOperationManager.CreateOperation(userState)
        _userStateToLifetime.Add(userState, asyncOp)

        Dim worker As New MovieLoaderWorkerHandler(AddressOf MovieLoaderWorker)
        worker.BeginInvoke(movieName, asyncOp, Nothing, Nothing)

    End Sub

    Public Sub GetMovieAsyncCancel(ByVal userState As Object)
        Dim obj As Object = _userStateToLifetime(userState)
        If obj IsNot Nothing Then
            SyncLock _userStateToLifetime.SyncRoot
                _userStateToLifetime.Remove(userState)
            End SyncLock
        End If
    End Sub

    Private Function TaskCancelled(ByVal userState As Object) As Boolean
        Return _userStateToLifetime(userState) Is Nothing
    End Function

    Private Delegate Sub MovieLoaderWorkerHandler(ByVal movieName As String, ByVal asyncOp As AsyncOperation)

    Private Sub MovieLoaderWorker(ByVal movieName As String, ByVal asyncOp As AsyncOperation)
        Dim [error] As Exception = Nothing
        Dim movieInfo As MovieInfo = Nothing

        If Not TaskCancelled(asyncOp.UserSuppliedState) Then
            Try
                movieInfo = _movieSource.GetMovie(movieName)
            Catch ex As Exception
                [error] = ex
            End Try
        End If

        CompleteGetMovie([error], TaskCancelled(asyncOp.UserSuppliedState), asyncOp.UserSuppliedState, movieInfo, asyncOp)
    End Sub

    Private Sub CompleteGetMovie(ByVal [error] As Exception, ByVal cancelled As Boolean, ByVal userState As Object, ByVal movieInfo As MovieInfo, ByVal asyncOp As AsyncOperation)
        Dim e As New GetMovieCompletedEventArgs([error], cancelled, userState, movieInfo)

        If Not cancelled Then
            SyncLock _userStateToLifetime.SyncRoot
                _userStateToLifetime.Remove(userState)
            End SyncLock
        End If

        asyncOp.PostOperationCompleted(New SendOrPostCallback(AddressOf MovieLoaderWorkerCompleted), e)
    End Sub

    Public Event GetMovieCompleted As EventHandler(Of GetMovieCompletedEventArgs)
    Private Sub OnGetMovieCompleted(ByVal e As GetMovieCompletedEventArgs)
        RaiseEvent GetMovieCompleted(Me, e)
    End Sub

    Private Sub MovieLoaderWorkerCompleted(ByVal state As Object)
        Dim e As GetMovieCompletedEventArgs = DirectCast(state, GetMovieCompletedEventArgs)
        OnGetMovieCompleted(e)
    End Sub
End Class

Public Class GetMovieCompletedEventArgs
    Inherits AsyncCompletedEventArgs

    Private _movieInfo As MovieInfo = Nothing
    Public Sub New(ByVal [error] As Exception, ByVal cancelled As Boolean, ByVal userState As Object, ByVal movieInfo As MovieInfo)
        MyBase.New([error], cancelled, userState)
        _movieInfo = movieInfo
    End Sub

    Public ReadOnly Property Result() As MovieInfo
        Get
            RaiseExceptionIfNecessary()
            Return _movieInfo
        End Get
    End Property
End Class