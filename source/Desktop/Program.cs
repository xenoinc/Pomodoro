/* Copyright Xeno Innovations, Inc. 2017
 * Author:  Damian Suess
 * Date:    2017-5-23
 * File:    Program.cs
 * Description:
 *  
 * To Do:
 * Change Log:
 *  2017-523 * Initial creation
 */

using System;
using System.Windows.Forms;
using Squirrel;

namespace Pomodoro
{
  
  static class Program
  {
    [STAThread]
    static void Main()
    {
      SquirrelAwareApp.HandleEvents(
        onAppUpdate: UpdaterForm.OnAppUpdate,
        onAppUninstall: UpdaterForm.OnAppUninstall,
        onInitialInstall: UpdaterForm.OnInitialInstall);

      Application.Run(new PomodoroTimer());
    }
  }
}
