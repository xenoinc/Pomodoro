/* Copyright Xeno Innovations, Inc. 2017
 * Author:  Damian Suess
 * Date:    2017-4-1
 * File:    TimerState.cs
 * Description:
 *
 * Change Log:
 *  2017-0401 * Initial creation
 */

namespace Pomodoro
{
  public enum TimerState
  {
    /// <summary>Timer started</summary>
    Start,

    /// <summary>Timer paused</summary>
    Pause,

    /// <summary>Timer stopped</summary>
    Stop,

    /// <summary>Timer finished</summary>
    Done
  };
}
