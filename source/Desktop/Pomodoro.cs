/* Copyright 2016-2017 (C) Xeno Innovations, Inc.
 * Author: Damian Suess
 * Description:
 *  Cheap pomodoro timer
 * 
 * To Do:
 *  [ ] Move the Application.Run to its own file
 *  [ ] Clean up the code.. other people have to look at this. This is ugly
 * 
 * Change Log:
 *  2016-0111 + [DJS] Added sound and repeatable. Cleaned up code.
 *  2015-0106 + [DJS] Updated tray icon., old one was unreadable & ugly
 */

using System;
using System.Drawing;
using System.Windows.Forms;

namespace Pomodoro
{
  public class PomodoroTimer : Form
  {
    [STAThread]
    public static void Main()
    {
      Application.Run(new PomodoroTimer());
    }

    private NotifyIcon _trayIcon;
    private ContextMenu _trayMenu;

    private TextIcon[] _numberIcons;

    private System.Timers.Timer _timer;
    private bool _running = false;
    private int _elapsedTime = 0;
    private int _minutesToWait;

    private const int PomorodoMinutesToWait = 25;
    private const int ShortBreakMinutesToWait = 5;
    private const int LongBreakMinutesToWait = 15;
    

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
      _trayMenu.MenuItems.Add("Exit", OnExit);
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

    private void OnStartPomodoro(object sender, EventArgs e)
    {
      StartTimer(PomorodoMinutesToWait);
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
      ToggleTimerPaused();
    }

    private void OnStop(object sender, EventArgs e)
    {
      StopTimer();
      ShowStoppedMenu();
    }

    private void OnExit(object sender, EventArgs e)
    {
      if (_timer != null) _timer.Stop();
      Application.Exit();
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

    #endregion Actions

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

    private void ToggleTimerPaused()
    {
      _running = !_running;

      if (!_running)
      {
        TextIcon newIcon = new Pomodoro.TextIcon("||");
        _trayIcon.Icon = newIcon.Get();

        _trayMenu.MenuItems[4].Text = "Resume";
      }
      else
      {
        int minutesRemaining = _minutesToWait - (_elapsedTime / 60 / 1000);
        SetTextIcon(minutesRemaining);
        _trayMenu.MenuItems[4].Text = "Pause";
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
          
          PlaySound(3);
          
          if (_settingShowMsgBox)
            MessageBox.Show("Time's up!", "Pomodoro Timer", MessageBoxButtons.OK, MessageBoxIcon.Stop);
        }
        else
        {
          int minutesRemaining = _minutesToWait - (_elapsedTime / 60 / 1000);
          SetTextIcon(minutesRemaining);
        }
      }
    }
    
    #endregion

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

    private void PlaySound(uint repeat=1)
    {
      if (_settingPlaySound)
        return;

      try
      {
        for (uint times = 1; times <= repeat; ++times)
        {
          System.Reflection.Assembly a = System.Reflection.Assembly.GetExecutingAssembly();
          System.IO.Stream s = a.GetManifestResourceStream("Pomodoro.chime.wav");
          
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