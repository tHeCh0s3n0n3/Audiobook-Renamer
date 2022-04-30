using ID3Helper;

namespace Audiobook_Renamer;

public partial class MainForm : Form
{
    private readonly BindingSource _bindingSource = new();

    private List<Audiobook> _audiobooks = new();

    private string _currentDirectory = string.Empty;

    private static Exception? _exception;

    private static CancellationTokenSource? _cancellationTokenSource;

    private static int currentCount = 0;
    private static int totalCount = 0;

    public MainForm()
    {
        InitializeComponent();
        _bindingSource.DataSource = _audiobooks;
        dgvBooks.DataSource = _bindingSource;
        dgvBooks.Columns["BookNumber"].HeaderText = "Book #";
        dgvBooks.Columns["JSONRepresentation"].Visible = false;
        dgvBooks.Columns["SafeTitle"].Visible = false;
        dgvBooks.Columns["SafeAuthor"].Visible = false;
        dgvBooks.Columns["SafeSeries"].Visible = false;

        ssStatusBar.Padding = new Padding(ssStatusBar.Padding.Left
                                          , ssStatusBar.Padding.Top
                                          , ssStatusBar.Padding.Right / 4
                                          , ssStatusBar.Padding.Bottom);
        ssStatusBar.SizingGrip = false;
        ssStatusBar.GripStyle = ToolStripGripStyle.Hidden;
    }

    private void DgvBooks_SelectionChanged(object? sender, EventArgs e)
    {
        if (dgvBooks.SelectedRows.Count == 0)
        {
            return;
        }

        if (dgvBooks.SelectedRows[0]?.DataBoundItem is Audiobook audiobook)
        {
            txtFilename.Text = audiobook.Filename;
            txtTitle.Text = audiobook.Title;
            txtAuthor.Text = audiobook.Author;
            txtSeriesName.Text = audiobook.Series;
            txtBookNumber.Text = audiobook.BookNumber;
            txtJSON.Text = audiobook.JSONRepresentation;
        }
    }

    private void DgvBooks_DataBindingComplete(object? sender, DataGridViewBindingCompleteEventArgs e)
    {
        foreach (DataGridViewColumn col in dgvBooks.Columns)
        {
            col.SortMode = DataGridViewColumnSortMode.Programmatic;
        }
    }

    private void DgvBooks_ColumnHeaderMouseClick(object? sender, DataGridViewCellMouseEventArgs e)
    {
        DataGridViewColumn column = dgvBooks.Columns[e.ColumnIndex];
        if (column is null)
        {
            return;
        }

        // Cycle through directions (Asc -> Desc -> None)
        SortOrder newSortOrder = column.HeaderCell.SortGlyphDirection switch
        {
            SortOrder.None => SortOrder.Ascending,
            SortOrder.Ascending => SortOrder.Descending,
            SortOrder.Descending => SortOrder.None,
            _ => SortOrder.None
        };

        if (newSortOrder == SortOrder.None)
        {
            _bindingSource.DataSource = _audiobooks;
            dgvBooks.Columns[column.Name].HeaderCell.SortGlyphDirection = newSortOrder;
            return;
        }

        List<Audiobook> sortedList = column.Name switch
        {
            "Title" => _audiobooks.OrderBy(ab => ab.Title).ToList(),
            "Author" => _audiobooks.OrderBy(ab => ab.Author).ToList(),
            "Series" => _audiobooks.OrderBy(ab => ab.Series).ToList(),
            "BookNumber" => _audiobooks.OrderBy(ab => ab.BookNumber).ToList(),
            _ => _audiobooks
        };

        if (newSortOrder == SortOrder.Descending)
        {
            sortedList.Reverse();
        }        
        
        _bindingSource.DataSource = sortedList;
        dgvBooks.Columns[column.Name].HeaderCell.SortGlyphDirection = newSortOrder;
        return;
    }

    private void BtnGetInfo_Click(object sender, EventArgs e)
    {
        OpenFileDialog ofd = new()
        {
            Filter = "MP3 files (*.mp3)|*.mp3"
            , Multiselect = false
            , Title = "Select MP3 file"
        };

        if (ofd.ShowDialog() != DialogResult.OK)
        {
            return;
        }

        txtFilename.Text = ofd.FileName;
        txtTitle.Clear();
        txtAuthor.Clear();
        txtSeriesName.Clear();
        txtBookNumber.Clear();
        txtJSON.Clear();

        Audiobook? audiobook = ID3Helper.Helper.ParseID3Tags(ofd.FileName);
        if (audiobook is null)
        {
            return;
        }

        txtTitle.Text = audiobook.Title;
        txtAuthor.Text = audiobook.Author;
        txtSeriesName.Text = audiobook.Series;
        txtBookNumber.Text = audiobook.BookNumber;
        txtJSON.Text = audiobook.JSONRepresentation;
    }

    private void BtnParseDirectory_Click(object sender, EventArgs e)
    {
        string? selectedDirectory = SelectFolder("Select MP3 directory");
        if (selectedDirectory is null)
        {
            return;
        }

        _currentDirectory = selectedDirectory;
        RefreshAudiobookList();
    }

    private void RefreshAudiobookList()
    {
        _audiobooks = ID3Helper.Helper.ParseDirectory(_currentDirectory);
        _bindingSource.DataSource = _audiobooks;
    }

    private void BtnRenameMissingBookNumbers_Click(object sender, EventArgs e)
    {
        Helper.RenameMissingSeriesNumber(_audiobooks);
        RefreshAudiobookList();
    }

    private async void BtnMassRename_Click(object sender, EventArgs e)
    {
        string? basePath = SelectFolder("Select destination");
        currentCount = 0;
        totalCount = _audiobooks.Count;
        tspbProgress.Step = 1;
        tspbProgress.Value = 0;
        tspbProgress.Maximum = totalCount;
        tspbProgress.Visible = true;

        if (basePath is null)
        {
            return;
        }

        _cancellationTokenSource = new();
        try
        {
            IndicateWorking();
            CancellationToken cancellationToken = _cancellationTokenSource.Token;
            await Task.Run(() =>
                Parallel.ForEach(_audiobooks
                                 , new ParallelOptions()
                                 {
                                     MaxDegreeOfParallelism = 5
                                 }
                                 , item => DoWork(basePath, item, cancellationToken))
                , cancellationToken);

            if (cancellationToken.IsCancellationRequested
                && _exception is not null)
            {
                throw _exception;
            }
        }
        catch (IOException ioEx)
        {
            TaskDialog.ShowDialog(new TaskDialogPage()
                                  {
                                      Text = ioEx.Message
                                      , Caption = "Exception encountered"
                                      , Heading = "Exception!"
                                      , Buttons = new TaskDialogButtonCollection() { TaskDialogButton.OK }
                                      , AllowCancel = false
                                      , AllowMinimize = true
                                      , Icon = TaskDialogIcon.Error
                                      , SizeToContent = true
                                  }
                                  , TaskDialogStartupLocation.CenterOwner);
        }
        if (_cancellationTokenSource.IsCancellationRequested)
        {
            tsslStatus.Text = "Canceled";
        }
        tsslStatus.Text = "Ready!";
        tspbProgress.Visible = false;
        btnCancel.Visible = false;
        IndicateWorking(false);
    }

    public void IndicateWorking(bool disableControls = true)
    {
        btnParseDirectory.Enabled = !disableControls;
        btnCancel.Enabled = disableControls;
        btnCancel.Visible = disableControls;
        btnMassRename.Enabled = !disableControls;
        btnRenameMissingBookNumbers.Enabled = !disableControls;
        btnGetInfo.Enabled = !disableControls;
        UseWaitCursor = disableControls;
        btnCancel.UseWaitCursor = false;
    }

    private void DoWork(string basePath, Audiobook item, CancellationToken cancellationToken)
    {
        if (_cancellationTokenSource is null) return;

        Interlocked.Increment(ref currentCount);
        btnMassRename.Invoke(() => tspbProgress.PerformStep());
        btnMassRename.Invoke(() => tsslStatus.Text = $"{currentCount:#,##0} / {totalCount:#,##0}");
        Application.DoEvents();
        try
        {
            if (!cancellationToken.IsCancellationRequested)
            {
                item.CreateDirectoryStructureAndCopyBook(basePath);
            }
        }
        catch (Exception ioEx)
        {
            _exception = ioEx;
            _cancellationTokenSource.Cancel();
        }
    }

    private static string? SelectFolder(string title)
    {
        FolderPicker fp = new()
        {
            Title = title
            , OkButtonLabel = "Select Folder"
        };

        bool? success = fp.ShowDialog();
        if (success is null || success == false)
        {
            return null;
        }

        return fp.ResultPath;
    }

    private void BtnCancel_Click(object sender, EventArgs e)
    {
        if (_cancellationTokenSource is not null)
        {
            _cancellationTokenSource.Cancel();
            btnCancel.Enabled = false;
        }
    }
}
