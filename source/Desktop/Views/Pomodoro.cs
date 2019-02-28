/* Copyright 2015-2017 (C) Xeno Innovations, Inc.
 * Author: Damian Suess
 * Date: 2015-0106
 * Description:
 *  Cheap pomodoro timer
 *
 * To Do:
 *  [ ] Use a basic .config file - had this before but code was lost... damn you McAfee!
 *  [ ] Move the Application.Run to its own file
 *  [ ] Clean up the code.. other people have to look at this. This is ugly
 *
 * Change Log:
 *  2017-0401 + Added UI notifications [DJS]
 *  2016-0111 + Added sound and repeatable. Cleaned up code. [DJS]
 *  2015-0106 + Updated tray icon., old one was unreadable & ugly [DJS]
 */

using System;
using System.Windows.Forms;

namespace Xeno.Pomodoro
{
  public class PomodoroTimer : Form
  {
    //[STAThread]
    //public static void Main()
    //{
    //  Application.Run(new PomodoroTimer());
    //}

    private const int PomorodoMinutesToWait = 25;
    private const int ShortBreakMinutesToWait = 5;
    private const int LongBreakMinutesToWait = 15;

    private NotifyIcon _trayIcon;
    private ContextMenu _trayMenu;

    private TextIcon[] _numberIcons;

    private System.Timers.Timer _timer;
    private bool _running = false;
    private int _elapsedTime = 0;
    private int _minutesToWait;

    // Create a new app settings feature for this
    private bool _settingPlaySound = true;

    private bool _settingShowMsgBox = false;

    #region Initialization

    public PomodoroTimer()
    {
      //PlaySound();     // Uncomment to test sound
      //PlaySound(3);    // Uncomment to test sound

      InitializeMenu();
      ShowStoppedMenu();

      CreateNumberIcons();
      InitializeIcon();
    }

    private void CreateNumberIcons()
    {
      _numberIcons = new TextIcon[25];
      for (int i = 1; i <= 25; i++)
      {
        _numberIcons[i - 1] = new TextIcon(i.ToString());
      }
    }

    private void InitializeIcon()
    {
      _trayIcon = new NotifyIcon();
      _trayIcon.Text = "Pomodoro Timer";
      this.DefaultIcon();

      // Add menu to tray icon and show it.
      _trayIcon.ContextMenu = _trayMenu;
      _trayIcon.Visible = true;
    }

    private void InitializeMenu()
    {
      _trayMenu = new ContextMenu();
      _trayMenu.MenuItems.Add("Start Pomodoro", OnStartPomodoro);
      _trayMenu.MenuItems.Add("Start short break", OnStartShortBreak);
      _trayMenu.MenuItems.Add("Start long break", OnStartLongBreak);
      _trayMenu.MenuItems.Add("-");
      _trayMenu.MenuItems.Add("Pause", OnTogglePause);
      _trayMenu.MenuItems.Add("Stop", OnStop);
      _trayMenu.MenuItems.Add("-");
      _trayMenu.MenuItems.Add("About", OnAbout);
      _trayMenu.MenuItems.Add("Exit", OnExit);

      _trayMenu.MenuItems[0].DefaultItem = true;
    }

    private void DefaultIcon()
    {
      string iconChar = "Σ";
      //string iconChar = "🍅";         // Shows up as a square on Win10 due to a bug in .Net GDI
      //string iconChar = "\uDF45";     // '🍅' http://www.fileformat.info/info/unicode/char/1f345/index.htm

      TextIcon newIcon = new Pomodoro.TextIcon(iconChar);
      _trayIcon.Icon = newIcon.Get();
    }

    private void SetTextIcon(int number)
    {
      if (number - 1 < 0)
        number = 1;

      _trayIcon.Icon = _numberIcons[number - 1].Get();
    }

    #endregion Initialization

    #region Event Handlers

    protected override void OnLoad(EventArgs e)
    {
      Visible = false; // Hide form window.
      ShowInTaskbar = false; // Remove from taskbar.

      base.OnLoad(e);
    }

    protected override void Dispose(bool isDisposing)
    {
      if (isDisposing)
      {
        // Release the icon resource.
        _trayIcon.Dispose();
      }

      base.Dispose(isDisposing);
    }

    private void OnStartPomodoro(object sender, EventArgs e)
    {
      StartTimer(PomorodoMinutesToWait);
      NotifyStateChange(TimerState.Start);
      //NotifyStateChange(TimerState.Done);   // Here for debugging only
      ShowRunningMenu();
    }

    private void OnStartShortBreak(object sender, EventArgs e)
    {
      StartTimer(ShortBreakMinutesToWait);
      ShowRunningMenu();
    }

    private void OnStartLongBreak(object sender, EventArgs e)
    {
      StartTimer(LongBreakMinutesToWait);
      ShowRunningMenu();
    }

    private void OnTogglePause(object sender, EventArgs e)
    {
      if (ToggleTimerPaused())
        NotifyStateChange(TimerState.Pause);
      else
        NotifyStateChange(TimerState.Start);
    }

    private void OnStop(object sender, EventArgs e)
    {
      StopTimer();
      NotifyStateChange(TimerState.Stop);
      ShowStoppedMenu();
    }

    private void OnAbout(object sender, EventArgs e)
    {
      string desc = "Pomodoro Timer for Desktop" + System.Environment.NewLine +
                    "Version: " + System.Reflection.Assembly.GetEntryAssembly().GetName().Version + System.Environment.NewLine +
                    System.Environment.NewLine +
                    "Copyright 2017 Xeno Innovations" + System.Environment.NewLine +
                    "Open Source: https://github.com/xenoinc/Pomodoro" + System.Environment.NewLine;
      MessageBox.Show(desc, "About Pomodoro");
    }

    private void OnExit(object sender, EventArgs e)
    {
      if (_timer != null) _timer.Stop();
      Application.Exit();
    }

    #endregion Event Handlers

    #region State Change Notifications

    private void NotifyStateChange(TimerState state)
    {
      switch (state)
      {
        case TimerState.Start:
          // Single flash alpha-trans form on screen - (0%->50%) x1
          UiNotify.Flicker(1, TimerState.Start);
          PlaySound(1);
          break;

        case TimerState.Pause:
          // Fade in alpha-trans form as "pause" (2 rectangle forms)
          UiNotify.Flicker(1, TimerState.Pause);
          break;

        case TimerState.Stop:
          // Fade in alpha-trans form as square
          UiNotify.Flicker(1, TimerState.Stop);
          break;

        case TimerState.Done:
          // Double flash alpha-trans form on screen - (0%->50%) x3

          UiNotify.Flicker(3, TimerState.Done);
          PlaySound(3);

          if (_settingShowMsgBox)
            MessageBox.Show("Time's up!", "Pomodoro Timer", MessageBoxButtons.OK, MessageBoxIcon.Stop);

          break;

        default: break;
      }
    }

    #endregion State Change Notifications

    #region Timers

    private void StartTimer(int minutes)
    {
      _elapsedTime = 0;
      _timer = new System.Timers.Timer();
      _timer.Interval = 1000; // timer fires every 1s
      _timer.Elapsed += TimerFired;
      _timer.Start();
      _running = true;

      _minutesToWait = minutes;
      SetTextIcon(minutes);
    }

    private void StopTimer()
    {
      _timer.Stop();
      _running = false;
      DefaultIcon();
    }

    /// <summary>Pause or resume timer</summary>
    /// <returns>Returns true if paused</returns>
    private bool ToggleTimerPaused()
    {
      _running = !_running;

      if (!_running)
      {
        TextIcon newIcon = new Pomodoro.TextIcon("||");
        _trayIcon.Icon = newIcon.Get();
        _trayMenu.MenuItems[4].Text = "Resume";
        return true;
      }
      else
      {
        int minutesRemaining = _minutesToWait - (_elapsedTime / 60 / 1000);
        SetTextIcon(minutesRemaining);
        _trayMenu.MenuItems[4].Text = "Pause";
        return false;
      }
    }

    private void TimerFired(object sender, System.Timers.ElapsedEventArgs e)
    {
      if (_running)
      {
        // for simplicity assume that the exact amount of the timer's interval has passed
        // in practice, the actual elapsed time will probably be more than the interval
        _elapsedTime += (int)_timer.Interval;

        if (_elapsedTime > (_minutesToWait * 60 * 1000))
        {
          StopTimer();
          ShowStoppedMenu();

          NotifyStateChange(TimerState.Done);
          //PlaySound(3);
          //if (_settingShowMsgBox)
          //  MessageBox.Show("Time's up!", "Pomodoro Timer", MessageBoxButtons.OK, MessageBoxIcon.Stop);
        }
        else
        {
          int minutesRemaining = _minutesToWait - (_elapsedTime / 60 / 1000);
          SetTextIcon(minutesRemaining);
        }
      }
    }

    #endregion Timers

    #region Private Methods

    private void ShowStoppedMenu()
    {
      // enable 'Start' items; disable 'Pause' and 'Stop'
      _trayMenu.MenuItems[0].Enabled = true;
      _trayMenu.MenuItems[1].Enabled = true;
      _trayMenu.MenuItems[2].Enabled = true;

      _trayMenu.MenuItems[4].Enabled = false;
      _trayMenu.MenuItems[5].Enabled = false;
    }

    private void ShowRunningMenu()
    {
      // disable 'Start' items; enable 'Pause' and 'Stop'
      _trayMenu.MenuItems[0].Enabled = false;
      _trayMenu.MenuItems[1].Enabled = false;
      _trayMenu.MenuItems[2].Enabled = false;

      _trayMenu.MenuItems[4].Enabled = true;
      _trayMenu.MenuItems[5].Enabled = true;
    }

    private void PlaySound(uint repeat = 1)
    {
      if (!_settingPlaySound)
        return;

      try
      {
        for (uint times = 1; times <= repeat; ++times)
        {
          System.Reflection.Assembly a = System.Reflection.Assembly.GetExecutingAssembly();
          System.IO.Stream s = a.GetManifestResourceStream("Xeno.Pomodoro.chime.wav");

          // Place in a "using" and called via, PlaySync() so that we can call the sound multiple times
          using (System.Media.SoundPlayer player = new System.Media.SoundPlayer(s))
          {
            player.PlaySync();
          }

          //System.Media.SoundPlayer chime = new System.Media.SoundPlayer(@"chime.wav");    // Play physical file
          //WMPLib.WindowsMediaPlayer wplayer = new WMPLib.WindowsMediaPlayer();            // Play MP3
          //wplayer.URL = "chime.mp3";
          //wplayer.controls.play();
        }
      }
      catch { }
    }

    #endregion Private Methods
  }
}
