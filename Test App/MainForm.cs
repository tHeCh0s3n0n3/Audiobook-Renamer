using MetadataHelper;

namespace TestApp;

public partial class MainForm : Form
{
    private Audiobook? _audiobook;

    public MainForm()
    {
        InitializeComponent();
    }

    private void BtnGetInfo_Click(object sender, EventArgs e)
    {
        using OpenFileDialog ofd = new()
        {
            Filter = "All Audio Files (*.mp3,*.m4a,*.m4b)|*.mp3;*.m4a;*.m4b|MP3 files (*.mp3)|*.mp3|M4A files (*.m4a)|*.m4a|M4B files (*.m4b)|*.m4b|All Files (*.*)|*.*"
            , Multiselect = false
            , Title = "Select Audio file"
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

        Audiobook? audiobook = Helper.ParseFile(ofd.FileName);
        if (audiobook is null)
        {
            _audiobook = null;
            return;
        }

        txtTitle.Text = audiobook.Title;
        txtAuthor.Text = audiobook.Author;
        txtSeriesName.Text = audiobook.Series;
        txtBookNumber.Text = audiobook.BookNumber;
        txtNumberOfChapters.Text = audiobook.Chapters?.Count.ToString() ?? "0";
        txtJSON.Text = audiobook.JSONRepresentation;

        btnShowChapters.Enabled = (audiobook.Chapters?.Count ?? 0) > 0;

        _audiobook = audiobook;
    }

    private void BtnShowChapters_Click(object sender, EventArgs e)
    {
        if (_audiobook?.Chapters is null)
        {
            return;
        }

        new FrmChapterListing(_audiobook.Chapters).ShowDialog(this);
    }
}
