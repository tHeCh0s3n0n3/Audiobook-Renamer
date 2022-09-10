using CommunityToolkit.Mvvm.ComponentModel;

namespace Audiobook_Renamer.ViewModels;

public partial class InfoViewModel : ObservableObject
{
    [ObservableProperty]
    private string? filename;

    [ObservableProperty]
    private string? title;

    [ObservableProperty]
    private string? author;

    [ObservableProperty]
    private string? seriesName;

    [ObservableProperty]
    private string? bookNumber;

    [ObservableProperty]
    private string? jSON;
}
