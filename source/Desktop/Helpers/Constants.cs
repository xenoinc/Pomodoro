/* Copyright Xeno Innovations, Inc. 2017-2018
 * Author:  Damian Suess
 * Date:    2017-5-24
 * File:    Constants.cs
 * Description:
 *  System constants
 *
 * Change Log:
 *  2017-0524 * Initial creation
 *  2018-0705 + Merged in alt/local squirrel updater (currently in test)
 */

namespace Xeno.Pomodoro.Helpers
{
  public static class Constants
  {
#if DEBUG

    public const string UpdateURI = @"C:\work\lab\Pomodoro\bin";
    public const string UpdatePath = @"C:\temp\Pomodoro\";

#else

    // Local Squirrel Update Method (2017-1025)
    public const string UpdatePath = @"C:\temp\Pomodoro\";

    public const string UpdateURI = @"http://releases.xenoinc.com/pomodoro";
    // Alt-url: string UpdatePath = @"https://software.xenoinc.com/pomodoro/releases";

#endif
  }
}