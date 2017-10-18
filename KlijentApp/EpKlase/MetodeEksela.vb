Imports ClosedXML
Imports ClosedXML.Excel
Imports System.Data
Imports System.Data.OleDb
Imports System.Runtime.InteropServices
Imports Microsoft.Win32.SafeHandles
Imports Microsoft.WindowsAPICodePack
Imports Microsoft.WindowsAPICodePack.Dialogs
Imports System.IO
Imports EnterprisePlanner.KlasePrikaza
Imports FastMember
Imports EnterprisePlanner.Entiteti
Imports System.Text
Imports System.Net.Mail

Public Class MetodeEksela
    Implements IDisposable
    Public Function EkselUTabelu(filename As String) As DataTable
        Dim dt As New DataTable()
        dt = CitajEkselinjo(filename)
        Return dt
    End Function
    Public Function KreirajTabeluIzListe(Lista As Object, ListaKolona As List(Of PrikazKoloneDG)) As DataTable
        Dim dt As New DataTable
        For Each kol In ListaKolona
            If kol.NeVidljiva = False Then
                If trenutnaklasa.GetType Is GetType(Dokument) Then
                    If Not (kol.Original = "PreparationStatus" Or kol.Original = "SendReceiveStatus" Or kol.Original = "StockStatus" Or kol.Original = "UpdatedOn") Then
                        dt.Columns.Add(kol.Opis)
                    End If
                Else
                    dt.Columns.Add(kol.Opis)
                End If
            End If
        Next
        Dim sors = Lista
        For Each obj In sors
            '        Dim ob = ObjectAccessor.Create(obj)
            Dim redic As DataRow = dt.NewRow
            For Each k In ListaKolona
                If k.NeVidljiva = False Then
                    If trenutnaklasa.GetType Is GetType(Dokument) Then
                        If Not (k.Original = "PreparationStatus" Or k.Original = "SendReceiveStatus" Or k.Original = "StockStatus" Or k.Original = "UpdatedOn") Then
                            Dim original As String = k.Original
                            Using pm As New MetodePretrazivanja
                                Dim vred = pm.PronadjiVrednost(obj, k).Vrednost
                                '  MessageBox.Show(vred)
                                redic(k.Opis) = vred
                            End Using
                        End If
                    Else
                        Dim original As String = k.Original
                        Using pm As New MetodePretrazivanja
                            Dim vred = pm.PronadjiVrednost(obj, k).Vrednost
                            '  MessageBox.Show(vred)
                            redic(k.Opis) = vred
                        End Using
                    End If
                End If
            Next
            dt.Rows.Add(redic)
        Next
        Return dt
    End Function

    Public Function KreirajTabelu(dg As DataGrid, Optional listakolona As List(Of PrikazKoloneDG) = Nothing) As DataTable
        Dim dt As New DataTable

        Try


            If listakolona Is Nothing
                For Each kol In dg.Columns
                    dt.Columns.Add(kol.Header)
                Next
                Dim BrojKolona As Integer = dg.Columns.Count - 1
                For Each row As DataRowView In dg.Items
                    Dim red As DataRow = dt.NewRow
                    For i As Integer = 0 To BrojKolona
                        red(i) = row.Row.ItemArray(i).ToString()
                    Next
                    dt.Rows.Add(red)
                Next
            Else
                Dim sors = dg.ItemsSource
                dt = KreirajTabeluIzListe(sors, listakolona)
            End If

        Catch ex As Exception
            Greskonja(ex)
        End Try
        Return dt
    End Function
    Public Sub KreirajExcel(dt As DataTable, Putanja As String, Optional ImeSheeta As String = "1")
        Try

            Dim wb As New XLWorkbook()
            wb.Worksheets.Add(dt, ImeSheeta)
            wb.SaveAs(Putanja)


        Catch ex As Exception
            Greskonja(ex)
        End Try


    End Sub
    Public Sub KreirajExcel2(dts As DataSet, dt As DataTable, dt2 As DataTable, Putanja As String, Optional ImeSheeta As String = "1")
        Try
            dts.Tables.Add(dt)
            dts.Tables.Add(dt2)
            Dim wb As New XLWorkbook()
            wb.Worksheets.Add(dts)
            wb.SaveAs(Putanja)


        Catch ex As Exception
            Greskonja(ex)
        End Try


    End Sub


    Public Function CitajEkselinjo(tb As String) As DataTable
        Dim extabvb As New DataTable
        extabvb.Clear()
        extabvb.Columns.Clear()
        extabvb.Rows.Clear()
        Dim Excel03ConString As String = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source={0};Extended Properties='Excel 8.0;HDR={1}'"
        Dim Excel07ConString As String = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source={0};Extended Properties='Excel 8.0;HDR={1}'"
        Dim extension As String = System.IO.Path.GetExtension(tb)
        Dim conStr As String, sheetName As String
        ' Dim win As New Window5
        '   stekizbordokumenata.Visibility = Visibility.Hidden

        Try
            conStr = String.Empty

            Select Case extension

                Case ".xls"
                    'Excel 97-03
                    conStr = String.Format(Excel03ConString, tb, "YES")
                    Exit Select

                Case ".xlsx"
                    'Excel 07
                    conStr = String.Format(Excel07ConString, tb, "YES")
                    Exit Select
            End Select
            ' messagebox.show(conStr)
            'Get the name of the First Sheet.

            'Await Task.Run(Sub()
            Using con As New OleDbConnection(conStr)
                Using cmd As New OleDbCommand()
                    cmd.Connection = con
                    con.Open()
                    Dim dtExcelSchema As DataTable = con.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, Nothing)
                    sheetName = dtExcelSchema.Rows(0)("TABLE_NAME").ToString()
                    con.Close()
                    releaseObject(dtExcelSchema)
                End Using
            End Using
            'End Sub)



            Dim fName As String = IO.Path.GetFileNameWithoutExtension(tb)
            Dim lokejsn As String
            lokejsn = tb
            Using con As New OleDbConnection(conStr)
                Using cmd As New OleDbCommand()
                    Using oda As New OleDbDataAdapter()
                        cmd.CommandText = (Convert.ToString("SELECT * From [") & sheetName) + "]"
                        cmd.Connection = con
                        con.Open()
                        oda.SelectCommand = cmd
                        oda.Fill(extabvb)
                        con.Close()



                    End Using
                End Using
            End Using


            '   Exit Sub

        Catch ex As Exception
            Greskonja(ex, "Greška u čitanju dokumenta, proverite description dokumenta, ne sme imati razmake kao i još -, za razmak koristite _.")

        End Try
        '      stekizbordokumenata.Visibility = Visibility.Hidden
        Return extabvb
    End Function

    Public Sub CitajEksel(Putanja As String, dg As DataGrid)
        Try
            Dim dt = CitajEkselinjo(Putanja)
            dg.ItemsSource = Nothing
            dg.ItemsSource = dt.AsDataView
        Catch ex As Exception

        End Try
    End Sub

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

    Public Function OdaberiFolder() As String
        Dim folder As String = ""
        Try
            Dim dlg = New CommonOpenFileDialog()
            dlg.Title = PM.NadjiResurs("Odaberite putanju")
            dlg.IsFolderPicker = True

            dlg.InitialDirectory = SadasnjiFolder
            dlg.AddToMostRecentlyUsedList = False
            dlg.AllowNonFileSystemItems = False
            dlg.DefaultDirectory = SadasnjiFolder
            dlg.EnsureFileExists = True
            dlg.EnsurePathExists = True
            dlg.EnsureReadOnly = False
            dlg.EnsureValidNames = True
            dlg.Multiselect = False
            dlg.ShowPlacesList = True
            If dlg.ShowDialog() = CommonFileDialogResult.Ok Then
                folder = dlg.FileName
            End If

        Catch ex As Exception

        End Try
        Return folder
    End Function

    Public Function NadjiFajl() As String
        ' Konfiguracija dijaloga za otvaranje
        Dim dlg As New Microsoft.Win32.OpenFileDialog()
        dlg.FileName = "Document" ' Difoltno ime 
        dlg.DefaultExt = ".xlsx" ' Difoltna ekstenzija
        'dlg.Filter = " (.xlsx)|*.xlsx" ' Filtriranje po ekstenziji
        Dim Putanja As String = ""
        ' Prikazi dijalog
        Dim result As Boolean = dlg.ShowDialog()

        ' Obradi dijalog
        If result = True Then
            ' Otvori dokument
            Putanja = dlg.FileName
        Else
            Putanja = ""
        End If
        Return Putanja
    End Function

    Public Sub OtvoriOutLookIUbaciUMejl(Kome As String, Naslov As String, Telo As String, optional Atačment As String = nothing)
       
        Dim mapi As New MAPI()
        If Atačment IsNot Nothing
mapi.AddAttachment(Atačment)
        End If
mapi.AddRecipientTo(Kome)
mapi.SendMailPopup(Naslov, Telo)

    End Sub
End Class
