using System.Text;

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
            Filter = "MP3 files (*.mp3)|*.mp3|All Files (*.*)|*.*"
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

        //Audiobook? audiobook = ID3Helper.Helper.ParseID3Tags(ofd.FileName);
        //if (audiobook is null)
        //{
        //    return;
        //}

        //txtTitle.Text = audiobook.Title;
        //txtAuthor.Text = audiobook.Author;
        //txtSeriesName.Text = audiobook.Series;
        //txtBookNumber.Text = audiobook.BookNumber;
        //txtJSON.Text = audiobook.JSONRepresentation;

        TagLib.File tagFile = TagLib.File.Create(ofd.FileName);

        txtTitle.Text = tagFile.Tag.Title;
        foreach(TagLib.Tag tagContainer in (tagFile.Tag as TagLib.NonContainer.Tag)?.Tags ?? Array.Empty<TagLib.Tag>())
        {
            if (tagContainer.TagTypes == TagLib.TagTypes.Id3v2)
            {
                if (tagContainer is not TagLib.Id3v2.Tag id3Tags)
                {
                    continue;
                }
                foreach (var id3Tag in id3Tags)
                {
                    var tif = id3Tag as TagLib.Id3v2.UserTextInformationFrame;
                    if (tif is not null)
                    {
                        string? desc = tif.Description;
                        string[] tift = tif.Text;
                        string? tiftStr = string.Join(", ", tift);
                        if (desc is not null
                            && tiftStr is not null)
                        {
                            if (desc.ToUpper().Equals("AUTHOR"))
                            {
                                txtAuthor.Text = tiftStr;
                            }
                            if (desc.ToUpper().Equals("SERIES"))
                            {
                                txtSeriesName.Text = tiftStr;
                            }
                            if (desc.ToUpper().Equals("SERIES-PART"))
                            {
                                txtBookNumber.Text = tiftStr;
                            }
                            if (desc.ToUpper().Equals("JSON64"))
                            {
                                int encoding = tif.TextEncoding switch
                                {
                                    TagLib.StringType.Latin1 => 28591,
                                    TagLib.StringType.UTF16 => 1200,
                                    TagLib.StringType.UTF16LE => 1200,
                                    TagLib.StringType.UTF16BE => 1201,
                                    TagLib.StringType.UTF8 => 65001,
                                    _ => 65001
                                };
                                txtJSON.Text = Encoding.GetEncoding(encoding)
                                                       .GetString(Convert.FromBase64String(tiftStr));
                            }
                        }

#pragma warning disable IDE0059 // Unnecessary assignment of a value
                        string result = $"[{desc}] {tiftStr}";
#pragma warning restore IDE0059 // Unnecessary assignment of a value
                    }
                }
            }
        }
    }
}
