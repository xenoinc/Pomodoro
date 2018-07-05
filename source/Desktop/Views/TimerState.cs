/* Copyright Xeno Innovations, Inc. 2017
 * Author:  Damian Suess
 * Date:    2017-4-1
 * File:    TimerState.cs
 * Description:
 *  Timer states
 *
 * Change Log:
 *  2017-0401 * Initial creation
 */

namespace Xeno.Pomodoro
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
