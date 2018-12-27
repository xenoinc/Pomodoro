/* Copyright Xeno Innovations, Inc. 2017
 * Author:  Damian Suess
 * Date:    2017-5-23
 * File:    Program.cs
 * Description:
 *  Entry point
 *
 * Change Log:
 *  2017-0523 * Initial creation
 */

using System;
using System.Threading.Tasks;
using System.Windows.Forms;
using Squirrel;

namespace Xeno.Pomodoro
{
  internal static class Program
  {
    [STAThread]
    private static void Main()
    {
      // TestSettings();

      // Method 1
      Task.Run(async () =>
      {
        //using (var mgr = new UpdateManager(UpdatePath, "Pomodoro"))
        using (var mgr = new UpdateManager(Helpers.Constants.UpdatePath))
        {
          await mgr.UpdateApp();
        }
      });

      // Method 2.A
      SquirrelAwareApp.HandleEvents(
        onAppUpdate: UpdaterForm.OnAppUpdate,
        onAppUninstall: UpdaterForm.OnAppUninstall,
        onInitialInstall: UpdaterForm.OnInitialInstall);

      // Method 2.B
      SquirrelAwareApp.HandleEvents(ver =>
      {
        var mggr = default(UpdateManager);
        mggr.CreateShortcutsForExecutable("exeName", ShortcutLocation.Desktop, true);
      });

      Application.Run(new PomodoroTimer());
    }

    private static void TestSettings()
    {
      System.IO.IsolatedStorage.IsolatedStorageFile store = System.IO.IsolatedStorage.IsolatedStorageFile.GetUserStoreForDomain();

      long size = store.UsedSize;


      foreach (var dirName in store.GetDirectoryNames())
      {
        MessageBox.Show("Dir: " + dirName);
      }

      foreach (var file in store.GetFileNames())
      {
        MessageBox.Show("File: " + file);
      }

      // Deletes entire store - Use for Unisntaller
      //  https://docs.microsoft.com/en-us/dotnet/standard/io/how-to-delete-stores-in-isolated-storage
      //store.Remove();


      //// Delete
      //Helpers.Settings.AQuickTest = null;
      //
      ////Create tests
      //string test = Helpers.Settings.AQuickTest;
      //if (test == string.Empty)
      //{
      //  MessageBox.Show("Empty");
      //  Helpers.Settings.AQuickTest = "1";
      //}
      //else
      //{
      //  int num;
      //  int.TryParse(test, out num);
      //  MessageBox.Show(num.ToString());
      //  num++;
      //  Helpers.Settings.AQuickTest = num.ToString();
      //}
    }
  }
}
