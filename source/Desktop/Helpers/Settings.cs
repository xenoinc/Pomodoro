/* Copyright Xeno Innovations, Inc. 2018
 * Author:  Damian Suess
 * Date:    2018-7-5
 * File:    Settings.cs
 * Description:
 *  Settings container
 *
 * Change Log:
 *  2018-0705 * Initial creation
 */

namespace Xeno.Pomodoro.Helpers
{
  public static class Settings
  {
    public static readonly bool AutoUpdateDefault = true;
    public static readonly bool DisplayOverlayDefault = true;
    public static readonly bool IsFirstRunDefault = true;
    public static readonly bool PlaySoundsDefault = true;
    public static readonly bool SendStatisticsDefault = true;

    public static readonly int TimerLongBreakDefault = 15;
    public static readonly int TimerPomodoroDefault = 25;
    public static readonly int TimerShortBreakDefault = 5;

    /// <summary>Automatically check for updates on startup</summary>
    public static bool AutoUpdates { get; set; }

    /// <summary>Displays overlay graphic on status change</summary>
    public static bool DisplayOverlay { get; set; }

    /// <summary>Is this the first run of the application</summary>
    public static bool IsFirstRun { get; set; }

    /// <summary>Play status change sounds</summary>
    public static bool PlaySounds { get; set; }

    /// <summary>Report usage statistics</summary>
    public static bool SendStatistics { get; set; }

    public static int TimerLongBreak { get; set; }

    public static int TimerPomodoro { get; set; }

    public static int TimerShortBreak { get; set; }
  }
}
