/* Copyright 2016-2017 (C) Xeno Innovations, Inc.
 * Author: Damian Suess
 * Description:
 *  Timer state enumeration
 * Change Log:
 *  
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
