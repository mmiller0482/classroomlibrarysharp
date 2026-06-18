using System.Collections.Generic;
using System.Linq;
using ClassroomLibrary.Services;
using CommunityToolkit.Mvvm.ComponentModel;

namespace ClassroomLibrary.ViewModels;

public sealed record ThemeOption(AppTheme Theme, string Name, string Description);

public partial class SettingsViewModel : ObservableObject
{
    private readonly ThemeService _themeService;

    public IReadOnlyList<ThemeOption> ThemeOptions { get; } =
    [
        new(AppTheme.System, "Follow System", "Match your computer's light or dark appearance."),
        new(AppTheme.Light, "Classroom Light", "A bright, warm palette with a leafy green accent."),
        new(AppTheme.Dark, "Classroom Dark", "A low-glare palette with a softer green accent."),
        new(AppTheme.CoolBlue, "Cool Blue", "A crisp, airy palette with a calm blue accent.")
    ];

    [ObservableProperty]
    private ThemeOption _selectedTheme;

    public SettingsViewModel(ThemeService themeService)
    {
        _themeService = themeService;
        _selectedTheme = ThemeOptions.First(option => option.Theme == themeService.CurrentTheme);
    }

    partial void OnSelectedThemeChanged(ThemeOption value) =>
        _themeService.SetTheme(value.Theme);
}
