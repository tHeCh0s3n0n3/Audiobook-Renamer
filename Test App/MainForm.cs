using ID3Helper;

namespace Test_App;

public partial class MainForm : Form
{
    public MainForm()
    {
        InitializeComponent();
    }

    private void BtnGetInfo_Click(object sender, EventArgs e)
    {
        using OpenFileDialog ofd = new()
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
}
