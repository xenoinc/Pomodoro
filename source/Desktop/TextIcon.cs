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
    private Brush _brushColor = new SolidBrush(Color.Red);
    private Brush _brushAccent = new SolidBrush(Color.Black);
    private Bitmap _bitmap = new Bitmap(16, 16);
    private Graphics _graphics;
    private Font _font = new Font(FontFamily.GenericSansSerif, 8);
    private Icon _icon;

    public TextIcon(string text)
    {
      _graphics = Graphics.FromImage(_bitmap);
      _graphics.DrawString(text, _font, _brushAccent, 1, 1);
      _graphics.DrawString(text, _font, _brushColor, 0, 0);
      IntPtr hIcon = _bitmap.GetHicon();
      _icon = Icon.FromHandle(hIcon);
    }

    public Icon Get()
    {
      return _icon;
    }

    ~TextIcon()
    {
      _icon = null;
      _font = null;
      _graphics = null;
      _bitmap = null;
      _brushAccent = null;
      _brushColor = null;
    }

  }
}
