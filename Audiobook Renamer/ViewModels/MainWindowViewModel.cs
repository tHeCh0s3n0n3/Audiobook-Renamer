using CommunityToolkit.Mvvm.ComponentModel;

namespace Audiobook_Renamer.ViewModels;

public partial class MainWindowViewModel : ObservableObject
{

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(EnableSecondaryControls))]
    [NotifyPropertyChangedFor(nameof(IndicateWorking))]
    private bool enableMainControls = true;

    public bool EnableSecondaryControls
        => !EnableMainControls;

    public bool IndicateWorking
        => !EnableMainControls;
}
