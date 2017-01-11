/* Copyright 2016 (C) Xeno Innovations, Inc.
 * Author: Damian Suess
 * Description:
 * 
 * Change Log:
 *  2015-0106 + [DJS] Updated tray icon, old one was unreadable & ugly
 */
 
using System;
using System.Drawing;

namespace Pomodoro
{
  public class TextIcon
  {
    private Brush brush1 = new SolidBrush(Color.Black);
    private Brush brush2 = new SolidBrush(Color.Red);
    private Bitmap bitmap = new Bitmap(16, 16);
    private Graphics graphics;
    private Font font = new Font(FontFamily.GenericSansSerif, 8);
    private Icon icon;

    public TextIcon(string text)
    {
      graphics = Graphics.FromImage(bitmap);
      graphics.DrawString(text, font, brush1, 1, 1);
      graphics.DrawString(text, font, brush2, 0, 0);
      IntPtr hIcon = bitmap.GetHicon();
      icon = Icon.FromHandle(hIcon);
    }

    public Icon Get()
    {
      return icon;
    }

    ~TextIcon()
    {
      icon = null;
      font = null;
      graphics = null;
      bitmap = null;
      brush1 = null;
      brush2 = null;
    }

  }
}
