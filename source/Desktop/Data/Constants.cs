/* Copyright Xeno Innovations, Inc. 2017
 * Author:  Damian Suess
 * Date:    2017-5-24
 * File:    Constants.cs
 * Description:
 *  
 * To Do:
 * Change Log:
 *  2017-524 * Initial creation
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pomodoro.Data
{
  public static class Constants
  {

#if DEBUG
    public const string UpdateURI = @"C:\work\lab\Pomodoro\bin";
#else
    public const string UpdateURI = @"http://releases.xenoinc.com/pomodoro";
#endif
  }
}
