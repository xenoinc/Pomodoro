/* Copyright 2016 (C) Xeno Innovations, Inc.
 * Author: Damian Suess
 * Description:
 * 
 * Change Log:
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

    private System.Timers.Timer timer;
    private bool running = false;
    private int elapsedTime = 0;

    private const int pomorodoMinutesToWait = 25;
    private const int shortBreakMinutesToWait = 5;
    private const int longBreakMinutesToWait = 15;

    private int minutesToWait;

    public PomodoroTimer()
    {
      InitializeMenu();
      ShowStoppedMenu();

      CreateNumberIcons();
      InitializeIcon();
    }

    private void PomodoroTimer_Load(object sender, EventArgs e)
    {
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
      //_trayIcon.Icon = new Icon(SystemIcons.Application, 40, 40);
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

    protected override void OnLoad(EventArgs e)
    {
      Visible = false; // Hide form window.
      ShowInTaskbar = false; // Remove from taskbar.

      base.OnLoad(e);
    }

    private void OnStartPomodoro(object sender, EventArgs e)
    {
      StartTimer(pomorodoMinutesToWait);
      ShowRunningMenu();
    }

    private void OnStartShortBreak(object sender, EventArgs e)
    {
      StartTimer(shortBreakMinutesToWait);
      ShowRunningMenu();
    }

    private void OnStartLongBreak(object sender, EventArgs e)
    {
      StartTimer(longBreakMinutesToWait);
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

    private void StartTimer(int minutes)
    {
      elapsedTime = 0;
      timer = new System.Timers.Timer();
      timer.Interval = 1000; // timer fires every 1s
      timer.Elapsed += TimerFired;
      timer.Start();
      running = true;

      minutesToWait = minutes;
      TextIcon(minutes);
    }

    private void ToggleTimerPaused()
    {
      running = !running;
    }

    private void TimerFired(object sender, System.Timers.ElapsedEventArgs e)
    {
      if (running)
      {
        // for simplicity assume that the exact amount of the timer's interval has passed
        // in practice, the actual elapsed time will probably be more than the interval
        elapsedTime += (int)timer.Interval;

        if (elapsedTime > (minutesToWait * 60 * 1000))
        {
          StopTimer();
          ShowStoppedMenu();
          
          System.Media.SoundPlayer chime = new System.Media.SoundPlayer(@"chime.wav");
          chime.Play();
          
          MessageBox.Show("Time's up!", "Pomodoro Timer", MessageBoxButtons.OK, MessageBoxIcon.Stop);
        }
        else
        {
          int minutesRemaining = minutesToWait - (elapsedTime / 60 / 1000);
          TextIcon(minutesRemaining);
        }
      }
    }

    private void StopTimer()
    {
      timer.Stop();
      running = false;
      DefaultIcon();
    }

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

    private void OnExit(object sender, EventArgs e)
    {
      if (timer != null) timer.Stop();
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

    private void DefaultIcon()
    {

      TextIcon newIcon = new Pomodoro.TextIcon("Σ");
      
      _trayIcon.Icon = newIcon.Get();
    }

    private void TextIcon(int number)
    {
      _trayIcon.Icon = _numberIcons[number - 1].Get();
    }

    private void InitializeComponent()
    {
      this.SuspendLayout();
      //
      // PomodoroTimer
      //
      this.ClientSize = new System.Drawing.Size(284, 261);
      this.Name = "PomodoroTimer";
      this.Load += new System.EventHandler(this.PomodoroTimer_Load);
      this.ResumeLayout(false);
    }
  }
}