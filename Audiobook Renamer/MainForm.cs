using MetadataHelper;
using Audiobook_Renamer.ViewModels;
using OpenFolder;

namespace Audiobook_Renamer;

public partial class MainForm : Form
{
    private readonly BindingSource _bindingSource = new();

    private List<Audiobook> _audiobooks = new();

    private string _currentDirectory = string.Empty;

    private static Exception? _exception;

    private static readonly CancellationTokenSource _cancellationTokenSource = new();
    private readonly ParallelOptions _parallelOptions = new();

    private static int currentCount = 0;
    private static int totalCount = 0;

    private readonly MainWindowViewModel _vm = new();
    private readonly InfoViewModel _infoVM = new();


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
        dgvBooks.AllowUserToResizeRows = false;

        ssStatusBar.Padding = new Padding(ssStatusBar.Padding.Left
                                          , ssStatusBar.Padding.Top
                                          , ssStatusBar.Padding.Right / 4
                                          , ssStatusBar.Padding.Bottom);
        ssStatusBar.SizingGrip = false;
        ssStatusBar.GripStyle = ToolStripGripStyle.Hidden;

        // Bind controls to the view model
        btnParseDirectory.DataBindings.Add("Enabled", _vm, nameof(_vm.EnableMainControls));
        btnCancel.DataBindings.Add("Enabled", _vm, nameof(_vm.EnableSecondaryControls));
        btnCancel.DataBindings.Add("Visible", _vm, nameof(_vm.EnableSecondaryControls));
        btnCancel.DataBindings.Add("UseWaitCursor", _vm, nameof(_vm.EnableMainControls));
        btnMassRename.DataBindings.Add("Enabled", _vm, nameof(_vm.EnableMainControls));
        btnRenameMissingBookNumbers.DataBindings.Add("Enabled", _vm, nameof(_vm.EnableMainControls));
        btnGetInfo.DataBindings.Add("Enabled", _vm, nameof(_vm.EnableMainControls));
        DataBindings.Add("UseWaitCursor", _vm, nameof(_vm.IndicateWorking));

        // Bind info controls to the view model
        txtFilename.DataBindings.Add("Text", _infoVM, nameof(_infoVM.Filename));
        txtTitle.DataBindings.Add("Text", _infoVM, nameof(_infoVM.Title));
        txtAuthor.DataBindings.Add("Text", _infoVM, nameof(_infoVM.Author));
        txtSeriesName.DataBindings.Add("Text", _infoVM, nameof(_infoVM.SeriesName));
        txtBookNumber.DataBindings.Add("Text", _infoVM, nameof(_infoVM.BookNumber));
        txtJSON.DataBindings.Add("Text", _infoVM, nameof(_infoVM.JSON));

        _parallelOptions.MaxDegreeOfParallelism = 5;
    }

    private void DgvBooks_SelectionChanged(object? sender, EventArgs e)
    {
        if (dgvBooks.SelectedRows.Count == 0)
        {
            return;
        }

        if (dgvBooks.SelectedRows[0]?.DataBoundItem is Audiobook audiobook)
        {
            _infoVM.Filename = audiobook.Filename;
            _infoVM.Title = audiobook.Title;
            _infoVM.Author = audiobook.Author;
            _infoVM.SeriesName = audiobook.Series;
            _infoVM.BookNumber = audiobook.BookNumber;
            _infoVM.JSON = audiobook.JSONRepresentation;
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
        using OpenFileDialog ofd = new()
        {
            Filter = "All Audio Files (*.mp3,*.m4a,*.m4b)|*.mp3,*.m4a,*.m4b|MP3 files (*.mp3)|*.mp3|M4A files (*.m4a)|*.m4a|M4B files (*.m4b)|*.m4b"
            , Multiselect = false
            , Title = "Select audio file"
        };

        if (ofd.ShowDialog() != DialogResult.OK)
        {
            return;
        }

        _infoVM.Filename = ofd.FileName;
        _infoVM.Title = string.Empty;
        _infoVM.Author = string.Empty;
        _infoVM.SeriesName = string.Empty;
        _infoVM.BookNumber = string.Empty;
        _infoVM.JSON = string.Empty;

        Audiobook? audiobook = Helper.ParseFile(ofd.FileName);

        if (audiobook is null)
        {
            return;
        }

        _infoVM.Title = audiobook.Title;
        _infoVM.Author = audiobook.Author;
        _infoVM.SeriesName = audiobook.Series;
        _infoVM.BookNumber = audiobook.BookNumber;
        _infoVM.JSON = audiobook.JSONRepresentation;
    }

    private void BtnParseDirectory_Click(object sender, EventArgs e)
    {
        string? selectedDirectory = SelectFolder("Select audio directory");
        if (selectedDirectory is null)
        {
            return;
        }

        _currentDirectory = selectedDirectory;
        RefreshAudiobookList();
    }

    private void RefreshAudiobookList()
    {
        _audiobooks = Helper.ParseDirectory(_currentDirectory, Helper.FileTypes.MP3 | Helper.FileTypes.M4A | Helper.FileTypes.M4B);
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
        _ = Interlocked.Exchange(ref currentCount, 0);
        _ = Interlocked.Exchange(ref totalCount, _audiobooks.Count);
        tspbProgress.Step = 1;
        tspbProgress.Value = 0;
        tspbProgress.Maximum = totalCount;
        tspbProgress.Visible = true;

        if (basePath is null)
        {
            return;
        }
        try
        {

            if (!_cancellationTokenSource.TryReset())
            {
                throw new InvalidOperationException("Cannot reset cancellation token source");
            }

            _vm.EnableMainControls = false;
            CancellationToken cancellationToken = _cancellationTokenSource.Token;
            await Task.Run(() =>
                Parallel.ForEach(_audiobooks
                                 , _parallelOptions
                                 , item => DoWork(basePath, item, cancellationToken))
                , cancellationToken);

            if (cancellationToken.IsCancellationRequested
                && _exception is not null)
            {
                throw _exception;
            }
        }
        catch (OperationCanceledException)
        {
            tsslStatus.Text = "Canceled";
        }
        catch (Exception ex)
        {
            TaskDialog.ShowDialog(new TaskDialogPage()
                                  {
                                      Text = ex.Message
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

        tsslStatus.Text = _cancellationTokenSource.IsCancellationRequested
                          ? "Canceled"
                          : "Ready!";

        tspbProgress.Visible = false;
        _vm.EnableMainControls = true;
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
        catch (Exception ex) when(
            ex is ArgumentException
            || ex is ArgumentNullException
            || ex is System.Text.RegularExpressions.RegexMatchTimeoutException
            || ex is IOException
            || ex is UnauthorizedAccessException
            || ex is PathTooLongException
            || ex is DirectoryNotFoundException
            || ex is FileNotFoundException
            || ex is NotSupportedException)
        {
            SetException(ex);
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

    public static void SetException(Exception ex)
    {
        _exception = ex;
    }
}
