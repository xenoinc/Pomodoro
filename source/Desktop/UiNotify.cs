/* Copyright Xeno Innovations, Inc. 2017
 * Author:  Damian Suess
 * Date:    2017-4-1
 * File:    UiNotify.cs
 * Description:
 *  Animate screen notifications and shapes
 * TO DO:
 *  [ ] Operate on Background worker thread
 *  [ ] Pause: Make sizes dynamic (from frm.Size) if we don't like 300x300
 * Change Log:
 *  2017-0401 * Initial creation
 */

using System;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Pomodoro
{
  public static class UiNotify
  {
    private static Size _defaultFormSize = new System.Drawing.Size(300, 300);

    /// <summary>Animante screen notifications</summary>
    /// <param name="blinkTimes">Number of times to flash</param>
    /// <param name="state">Used to display as certain shapes. For now full screen</param>
    public static void Flicker(int blinkTimes, TimerState state = TimerState.Done)
    {
      using (
        var form = new System.Windows.Forms.Form()
        {
          AllowTransparency = true,
          Opacity = 0,
          FormBorderStyle = System.Windows.Forms.FormBorderStyle.None,
          Visible = false,
          ShowInTaskbar = false,
          Size = _defaultFormSize,
          StartPosition = FormStartPosition.CenterScreen,
          WindowState = System.Windows.Forms.FormWindowState.Maximized,
          TopMost = true
        })
      {
        CutFormRegion(form, state);

        FormCenterToScreen(form);

        form.Visible = true;
        form.Show();

        for (int ndx = 1; ndx <= blinkTimes; ndx++)
        {
          FadeIn(form);
          FadeOut(form);
        }
      }
    }

    #region Shapes

    private static void CutFormRegion(Form form, TimerState state)
    {
      form.BackColor = Color.LightBlue;

      switch (state)
      {
        case TimerState.Start:

          // Triangle
          form.Size = _defaultFormSize;
          form.WindowState = System.Windows.Forms.FormWindowState.Normal;
          form.Region = new System.Drawing.Region(MakeTriangle(form.Size));

          break;

        case TimerState.Pause:

          // 2 rectangles
          form.Size = _defaultFormSize;
          form.WindowState = System.Windows.Forms.FormWindowState.Normal;
          form.Region = new System.Drawing.Region(MakePause(form.Size));

          break;

        case TimerState.Stop:

          // Fade in alpha-trans form as square
          form.Size = _defaultFormSize;
          form.WindowState = System.Windows.Forms.FormWindowState.Normal;
          form.Region = new System.Drawing.Region(MakeStop(form.Size));

          break;

        case TimerState.Done:

          // Double flash alpha-trans form on screen - (0%->50%) x3
          form.WindowState = System.Windows.Forms.FormWindowState.Maximized;

          break;

        default: break;
      }
    }

    /// <summary>Make play button</summary>
    /// <param name="size"></param>
    /// <returns></returns>
    private static GraphicsPath MakeTriangle(Size size)
    {
      Point[] triangle =
      {
        new Point(0, 0),
        new Point(Convert.ToInt32(size.Width / 2), Convert.ToInt32(size.Height / 2)),
        new Point(0, size.Height),
        new Point(0, 0)
      };
      var myPath = new GraphicsPath();
      myPath.AddLines(triangle);
      return myPath;
    }

    /// <summary>Make pause buttons</summary>
    /// <param name="size"></param>
    /// <returns></returns>
    private static GraphicsPath MakePause(Size size)
    {
      var pause = new System.Drawing.Drawing2D.GraphicsPath();

      int firstThird = Convert.ToInt32(size.Width / 3);
      int secondThird = Convert.ToInt32(firstThird * 2);

      pause.AddRectangle(new RectangleF(0, 0, firstThird, size.Height));
      pause.AddRectangle(new RectangleF(secondThird, 0, firstThird, size.Height));

      return pause;
    }

    /// <summary>Make stop sign</summary>
    /// <param name="size"></param>
    /// <returns></returns>
    private static GraphicsPath MakeStop(Size size)
    {
      var stop = new System.Drawing.Drawing2D.GraphicsPath();
      stop.AddEllipse(0, 0, size.Width, size.Height);
      return stop;
    }

    #endregion Shapes

    #region Delay Helpers

    /// <summary></summary>
    /// <param name="form"></param>
    /// <param name="interval"></param>
    /// <param name="max">Make 1.0 for 100%</param>
    private static void FadeIn(Form form, uint interval = 20, double max = 0.5)
    {
      //Object is not fully invisible. Fade it in
      while (form.Opacity < max)
      {
        form.Opacity += 0.05;
        Timeout(interval);
      }
      // o.Opacity = 1; //make fully visible
    }

    private static void FadeOut(Form o, uint interval = 20, double min = 0.0)
    {
      //if (o.Opacity < min)
      //  return;

      //Object is fully visible. Fade it out
      while (o.Opacity > min)
      {
        o.Opacity -= 0.05;
        Timeout(interval);
      }
      o.Opacity = 0; //make fully invisible
    }

    private static async void FadeInA(Form o, int interval = 20)
    {
      //Object is not fully invisible. Fade it in
      while (o.Opacity < 1.0)
      {
        await Task.Delay(interval);
        o.Opacity += 0.05;
      }
      o.Opacity = 1; //make fully visible
    }

    private static async void FadeOutA(Form o, int interval = 20)
    {
      //Object is fully visible. Fade it out
      while (o.Opacity > 0.0)
      {
        await Task.Delay(interval);
        o.Opacity -= 0.05;
      }
      o.Opacity = 0; //make fully invisible
    }

    private static void FormCenterToScreen(Form frm)
    {
      Screen screen = Screen.FromControl(frm);

      Rectangle workingArea = screen.WorkingArea;
      frm.Location = new Point()
      {
        X = Math.Max(workingArea.X, workingArea.X + (workingArea.Width - frm.Width) / 2),
        Y = Math.Max(workingArea.Y, workingArea.Y + (workingArea.Height - frm.Height) / 2)
      };
    }

    /// <summary>Timeout the specified duration in milliseconds.</summary>
    /// <param name="msDuration">Ms duration.</param>
    private static void Timeout(uint msDuration)
    {
      Stopwatch sw = new Stopwatch();
      sw.Start();
      while (true)
      {
        if (sw.ElapsedMilliseconds > msDuration)
          break;
      }
    }

    #endregion Delay Helpers
  }
}
