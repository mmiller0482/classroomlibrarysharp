using System;
using System.IO;
using System.Text.Json;
using Avalonia;
using Avalonia.Styling;

namespace ClassroomLibrary.Services;

public enum AppTheme
{
    System,
    Light,
    Dark,
    CoolBlue
}

/// <summary>Custom variants inherit the matching Fluent base theme.</summary>
public static class AppThemeVariants
{
    public static ThemeVariant CoolBlue { get; } = new("CoolBlue", ThemeVariant.Light);
}

/// <summary>Applies the app-wide theme and remembers the user's selection.</summary>
public sealed class ThemeService
{
    private readonly string _settingsPath;

    public AppTheme CurrentTheme { get; private set; }

    public ThemeService()
    {
        var appData = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
        var directory = Path.Combine(appData, "ClassroomLibrary");
        Directory.CreateDirectory(directory);
        _settingsPath = Path.Combine(directory, "settings.json");

        CurrentTheme = LoadTheme();
        Apply(CurrentTheme, save: false);
    }

    public void SetTheme(AppTheme theme) => Apply(theme, save: true);

    private void Apply(AppTheme theme, bool save)
    {
        CurrentTheme = theme;

        if (Application.Current is { } application)
        {
            application.RequestedThemeVariant = theme switch
            {
                AppTheme.Light => ThemeVariant.Light,
                AppTheme.Dark => ThemeVariant.Dark,
                AppTheme.CoolBlue => AppThemeVariants.CoolBlue,
                _ => ThemeVariant.Default
            };
        }

        if (save)
        {
            SaveTheme();
        }
    }

    private AppTheme LoadTheme()
    {
        try
        {
            if (!File.Exists(_settingsPath)) return AppTheme.System;

            var settings = JsonSerializer.Deserialize<ThemeSettings>(File.ReadAllText(_settingsPath));
            return Enum.IsDefined(settings?.Theme ?? AppTheme.System)
                ? settings?.Theme ?? AppTheme.System
                : AppTheme.System;
        }
        catch
        {
            return AppTheme.System;
        }
    }

    private void SaveTheme()
    {
        try
        {
            var json = JsonSerializer.Serialize(
                new ThemeSettings { Theme = CurrentTheme },
                new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(_settingsPath, json);
        }
        catch
        {
            // A read-only settings location should not prevent changing the live theme.
        }
    }

    private sealed class ThemeSettings
    {
        public AppTheme Theme { get; set; }
    }
}
