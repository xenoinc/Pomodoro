/* Copyright Xeno Innovations, Inc. 2018
 * Author:  Damian Suess
 * Date:    2018-7-5
 * File:    CrossSettings.cs
 * Description:
 *
 * Change Log:
 *  2018-75 * Initial creation
 */

using System;

namespace Xeno.Pomodoro.Data.Settings
{
  public static class CrossSettings
  {
    private static Lazy<ISettings> _implementation = new Lazy<ISettings>(() => CreateSettings(), System.Threading.LazyThreadSafetyMode.PublicationOnly);

    public static ISettings Current
    {
      get
      {
        ISettings ret = _implementation.Value;
        if (ret == null)
          throw new NotImplementedException();

        return ret;
      }
    }

    private static ISettings CreateSettings()
    {
      //return null;
      return new SettingsImplementation();
    }
  }
}
