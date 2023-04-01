using MetadataHelper;

namespace TestApp;

public partial class FrmChapterListing : Form
{
    private readonly ICollection<AudiobookChapter> audiobookChapters;

    public FrmChapterListing(ICollection<AudiobookChapter> chapters)
    {
        InitializeComponent();

        audiobookChapters = chapters;

        dgvList.DataSource = audiobookChapters;
        dgvList.Columns["Start"].DefaultCellStyle.Format = @"hh\:mm\:ss\.fff";
        dgvList.Columns["End"].DefaultCellStyle.Format = @"hh\:mm\:ss\.fff";
        dgvList.Columns["Duration"].DefaultCellStyle.Format = @"hh\:mm\:ss\.fff";

        dgvList.Columns["Title"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
    }

    private void BtnClose_Click(object sender, EventArgs e)
    {
        DialogResult = DialogResult.OK;
        Close();
    }
}
