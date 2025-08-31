using Microsoft.JSInterop;
using MudBlazor;

namespace SMS.UI.Shared.Services;

public class ThemeService
{
    private readonly IJSRuntime _jsRuntime;
    private bool _isDarkMode = false;

    public event Action? OnThemeChanged;

    public ThemeService(IJSRuntime jsRuntime)
    {
        _jsRuntime = jsRuntime;
    }

    public bool IsDarkMode => _isDarkMode;

    public async Task InitializeAsync()
    {
        try
        {
            var savedTheme = await _jsRuntime.InvokeAsync<string>("localStorage.getItem", "theme-preference");
            _isDarkMode = savedTheme == "dark";
        }
        catch
        {
            // If localStorage is not available, default to light mode
            _isDarkMode = false;
        }
    }

    public async Task ToggleThemeAsync()
    {
        _isDarkMode = !_isDarkMode;
        await SaveThemePreferenceAsync();
        OnThemeChanged?.Invoke();
    }

    public async Task SetThemeAsync(bool isDark)
    {
        if (_isDarkMode != isDark)
        {
            _isDarkMode = isDark;
            await SaveThemePreferenceAsync();
            OnThemeChanged?.Invoke();
        }
    }

    private async Task SaveThemePreferenceAsync()
    {
        try
        {
            await _jsRuntime.InvokeVoidAsync("localStorage.setItem", "theme-preference", _isDarkMode ? "dark" : "light");
        }
        catch
        {
            // Ignore if localStorage is not available
        }
    }

    public MudTheme GetCurrentTheme()
    {
        return new MudTheme()
        {
            PaletteLight = new PaletteLight()
            {
                Primary = "#1976d2",
                Secondary = "#dc004e",
                Success = "#4caf50",
                Warning = "#ff9800",
                Error = "#f44336",
                Info = "#2196f3",
                AppbarBackground = "#1976d2",
                Background = "#f5f5f5",
                Surface = "#ffffff",
                DrawerBackground = "#ffffff",
                DrawerText = "rgba(0,0,0, 0.87)",
                DrawerIcon = "rgba(0,0,0, 0.54)",
                TextPrimary = "rgba(0,0,0, 0.87)",
                TextSecondary = "rgba(0,0,0, 0.60)",
                TextDisabled = "rgba(0,0,0, 0.38)",
                ActionDefault = "rgba(0,0,0, 0.54)",
                ActionDisabled = "rgba(0,0,0, 0.26)",
                ActionDisabledBackground = "rgba(0,0,0, 0.12)",
                Divider = "rgba(0,0,0, 0.12)",
                DividerLight = "rgba(0,0,0, 0.06)",
                TableLines = "rgba(224, 224, 224, 1)",
                LinesDefault = "rgba(0,0,0, 0.12)",
                LinesInputs = "rgba(0,0,0, 0.42)",
                GrayDefault = "rgba(0,0,0, 0.26)",
                GrayLight = "rgba(0,0,0, 0.12)",
                GrayLighter = "rgba(0,0,0, 0.06)",
                GrayDark = "rgba(0,0,0, 0.38)",
                GrayDarker = "rgba(0,0,0, 0.60)",
                OverlayDark = "rgba(33,33,33,0.46)",
                OverlayLight = "rgba(255,255,255,0.46)"
            },
            PaletteDark = new PaletteDark()
            {
                Primary = "#776be7",
                Secondary = "#ff4081",
                Success = "#00e676",
                Warning = "#ff9800",
                Error = "#f44336",
                Info = "#00bcd4",
                AppbarBackground = "#27272f",
                Background = "#32333d",
                Surface = "#383838",
                DrawerBackground = "#27272f",
                DrawerText = "rgba(255,255,255, 0.70)",
                DrawerIcon = "rgba(255,255,255, 0.50)",
                TextPrimary = "rgba(255,255,255, 0.70)",
                TextSecondary = "rgba(255,255,255, 0.50)",
                TextDisabled = "rgba(255,255,255, 0.30)",
                ActionDefault = "rgba(255,255,255, 0.80)",
                ActionDisabled = "rgba(255,255,255, 0.26)",
                ActionDisabledBackground = "rgba(255,255,255, 0.12)",
                Divider = "rgba(255,255,255, 0.12)",
                DividerLight = "rgba(255,255,255, 0.06)",
                TableLines = "rgba(81, 81, 81, 1)",
                LinesDefault = "rgba(255,255,255, 0.12)",
                LinesInputs = "rgba(255,255,255, 0.30)",
                GrayDefault = "rgba(255,255,255, 0.26)",
                GrayLight = "rgba(255,255,255, 0.12)",
                GrayLighter = "rgba(255,255,255, 0.06)",
                GrayDark = "rgba(255,255,255, 0.38)",
                GrayDarker = "rgba(255,255,255, 0.60)",
                OverlayDark = "rgba(33,33,33,0.46)",
                OverlayLight = "rgba(33,33,33,0.46)"
            },
            Typography = new Typography()
            {
                Default = new Default()
                {
                    FontFamily = new[] { "Inter", "Roboto", "Arial", "sans-serif" }
                }
            },
            LayoutProperties = new LayoutProperties()
            {
                DrawerWidthLeft = "280px",
                AppbarHeight = "64px"
            },
            Shadows = new Shadow()
            {
                Elevation = new string[]
                {
                    "none",
                    "0px 2px 1px -1px rgba(0,0,0,0.2),0px 1px 1px 0px rgba(0,0,0,0.14),0px 1px 3px 0px rgba(0,0,0,0.12)",
                    "0px 3px 1px -2px rgba(0,0,0,0.2),0px 2px 2px 0px rgba(0,0,0,0.14),0px 1px 5px 0px rgba(0,0,0,0.12)",
                    "0px 3px 3px -2px rgba(0,0,0,0.2),0px 3px 4px 0px rgba(0,0,0,0.14),0px 1px 8px 0px rgba(0,0,0,0.12)",
                    "0px 2px 4px -1px rgba(0,0,0,0.2),0px 4px 5px 0px rgba(0,0,0,0.14),0px 1px 10px 0px rgba(0,0,0,0.12)",
                    "0px 3px 5px -1px rgba(0,0,0,0.2),0px 5px 8px 0px rgba(0,0,0,0.14),0px 1px 14px 0px rgba(0,0,0,0.12)",
                    "0px 3px 5px -1px rgba(0,0,0,0.2),0px 6px 10px 0px rgba(0,0,0,0.14),0px 1px 18px 0px rgba(0,0,0,0.12)",
                    "0px 4px 5px -2px rgba(0,0,0,0.2),0px 7px 10px 1px rgba(0,0,0,0.14),0px 2px 16px 1px rgba(0,0,0,0.12)",
                    "0px 5px 5px -3px rgba(0,0,0,0.2),0px 8px 10px 1px rgba(0,0,0,0.14),0px 3px 14px 2px rgba(0,0,0,0.12)",
                    "0px 5px 6px -3px rgba(0,0,0,0.2),0px 9px 12px 1px rgba(0,0,0,0.14),0px 3px 16px 2px rgba(0,0,0,0.12)",
                    "0px 6px 6px -3px rgba(0,0,0,0.2),0px 10px 14px 1px rgba(0,0,0,0.14),0px 4px 18px 3px rgba(0,0,0,0.12)",
                    "0px 6px 7px -4px rgba(0,0,0,0.2),0px 11px 15px 1px rgba(0,0,0,0.14),0px 4px 20px 3px rgba(0,0,0,0.12)",
                    "0px 7px 8px -4px rgba(0,0,0,0.2),0px 12px 17px 2px rgba(0,0,0,0.14),0px 5px 22px 4px rgba(0,0,0,0.12)",
                    "0px 7px 8px -4px rgba(0,0,0,0.2),0px 13px 19px 2px rgba(0,0,0,0.14),0px 5px 24px 4px rgba(0,0,0,0.12)",
                    "0px 7px 9px -4px rgba(0,0,0,0.2),0px 14px 21px 2px rgba(0,0,0,0.14),0px 5px 26px 4px rgba(0,0,0,0.12)",
                    "0px 8px 9px -5px rgba(0,0,0,0.2),0px 15px 22px 2px rgba(0,0,0,0.14),0px 6px 28px 5px rgba(0,0,0,0.12)",
                    "0px 8px 10px -5px rgba(0,0,0,0.2),0px 16px 24px 2px rgba(0,0,0,0.14),0px 6px 30px 5px rgba(0,0,0,0.12)",
                    "0px 8px 11px -5px rgba(0,0,0,0.2),0px 17px 26px 2px rgba(0,0,0,0.14),0px 6px 32px 5px rgba(0,0,0,0.12)",
                    "0px 9px 11px -5px rgba(0,0,0,0.2),0px 18px 28px 2px rgba(0,0,0,0.14),0px 7px 34px 6px rgba(0,0,0,0.12)",
                    "0px 9px 12px -6px rgba(0,0,0,0.2),0px 19px 29px 2px rgba(0,0,0,0.14),0px 7px 36px 6px rgba(0,0,0,0.12)",
                    "0px 10px 13px -6px rgba(0,0,0,0.2),0px 20px 31px 3px rgba(0,0,0,0.14),0px 8px 38px 7px rgba(0,0,0,0.12)",
                    "0px 10px 13px -6px rgba(0,0,0,0.2),0px 21px 33px 3px rgba(0,0,0,0.14),0px 8px 40px 7px rgba(0,0,0,0.12)",
                    "0px 10px 14px -6px rgba(0,0,0,0.2),0px 22px 35px 3px rgba(0,0,0,0.14),0px 8px 42px 7px rgba(0,0,0,0.12)",
                    "0px 11px 14px -7px rgba(0,0,0,0.2),0px 23px 36px 3px rgba(0,0,0,0.14),0px 9px 44px 8px rgba(0,0,0,0.12)",
                    "0px 11px 15px -7px rgba(0,0,0,0.2),0px 24px 38px 3px rgba(0,0,0,0.14),0px 9px 46px 8px rgba(0,0,0,0.12)"
                }
            }
        };
    }
}
