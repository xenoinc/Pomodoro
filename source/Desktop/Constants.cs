/* Copyright Xeno Innovations, Inc. 2017
 * Author:  Damian Suess
 * Date:    2017-10-25
 * File:    Constants.cs
 * Description:
 *  
 * To Do:
 * Change Log:
 *  2017-1025 * Initial creation
 */

namespace Pomodoro
{
  public static class Constants
  {
#if DEBUG
    public const string UpdatePath = @"C:\temp\Pomodoro\";
#else
    //public const string UpdatePath = "https://software.xenoinc.com/pomodoro/releases";
    public const string UpdatePath = @"C:\temp\Pomodoro\";
#endif
  }
}
