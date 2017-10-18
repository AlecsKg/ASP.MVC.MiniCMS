Imports System.IO
Imports System.Runtime.InteropServices
Imports Microsoft.Win32.SafeHandles

Public Class GrafickeMetode
    Implements IDisposable

    Public function KreirajScreenShotProzora(Prozor As Window,optional putanja As String ="") As String
        Dim rez As String = nothing
        Dim sirina As Double = If( Prozor.ActualWidth <> Nothing andalso not Double.IsInfinity(Prozor.ActualWidth), Prozor.ActualWidth,1900)
        Dim visina As Double = If( Prozor.ActualHeight <> Nothing andalso not Double.IsInfinity(Prozor.ActualHeight), Prozor.ActualHeight,1080) 
        Dim bmp As New RenderTargetBitmap(sirina,visina, 96, 96, PixelFormats.Pbgra32)
        bmp.Render(Prozor)

        Dim png As New PngBitmapEncoder()
        png.Frames.Add(BitmapFrame.Create(bmp))
        If String.IsNullOrEmpty(putanja)           
                   putanja = MyDocumentsDirektorijum          
        End If
        putanja &="\ErrorScreenShots"
          If not Directory.Exists(putanja) Then
               Directory.CreateDirectory(putanja)
            End If
         Using km As New MetodeKonvertera
                km.PrivremenaKultura(kultura_pod )
                Dim dat As String = Now.tostring.Replace(" ","").Replace(".","_").Replace(":","_").Replace("/","_")
            If dat.Last = "_"
                dat.Remove(dat.Count-1)
            End If
            'ErrorScreenShots/
        putanja &= "\Error_" &  dat &".png"
                  km.PrivremenaKultura(kultura_americka  )
            End Using
          
        Dim stm As Stream = File.Create(putanja)
        Try
            png.Save(stm)
        Finally
            stm.Dispose()
        End Try
        Return putanja
    End function

    Public sub SpremiDGZaBojenje (optional dg As DataGrid = nothing)
        If dg Is Nothing
            dg =  mEjnVindou.dataGrid
        End If   
                  dg.RowStyle = PM.NadjiResurs("TestRed")
        
                     
    End sub
    







    Dim disposed As Boolean = False
    ' Instantiate a SafeHandle instance.
    Dim handle As SafeHandle = New SafeFileHandle(IntPtr.Zero, True)

    ' Public implementation of Dispose pattern callable by consumers.
    Public Sub Dispose() _
              Implements IDisposable.Dispose
        Dispose(True)
        GC.SuppressFinalize(Me)
    End Sub

    ' Protected implementation of Dispose pattern.
    Protected Overridable Sub Dispose(disposing As Boolean)
        If disposed Then Return

        If disposing Then
            handle.Dispose()
            ' Free any other managed objects here.
            '
        End If

        ' Free any unmanaged objects here.
        '
        disposed = True
    End Sub
End Class
