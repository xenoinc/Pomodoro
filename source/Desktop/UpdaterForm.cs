/* Copyright Xeno Innovations, Inc. 2017
 * Author:  Damian Suess
 * Date:    2017-5-23
 * File:    UpdaterForm.cs
 * Description:
 *  Updater form
 * To Do:
 *  [ ] Finish me
 * Change Log:
 *  2017-523 * Initial creation
 */

using System;
using System.Data;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows.Forms;
using Squirrel;

namespace Pomodoro
{
  public partial class UpdaterForm : Form
  {

    private string _updatePath = @"C:\work\lab\Pomodoro\bin";
    private string _packageId = @"Pomodoro";
    public UpdaterForm()
    {
      InitializeComponent();

      _updatePath = Data.Constants.UpdateURI;

      // Pull from config file?
      //_updatePath = ConfigurationManager.AppSettings["UpdatePathFolder"];
      //_packageId = ConfigurationManager.AppSettings["PackageID"];
    }

    private void UpdaterForm_Load(object sender, EventArgs e)
    {
    }

    private const ShortcutLocation DefaultLocations = ShortcutLocation.StartMenu | ShortcutLocation.Desktop;

    private void checkForUpdatesToolStripMenuItem_Click(object sender, EventArgs e)
    {
      // Check for Squirrel Updates
      //var t = UpdateApp();
    }

    public async Task UpdateApp()
    {
      Console.WriteLine("UpdateApp()");
      

      using (var mgr = new UpdateManager(_updatePath, _packageId))
      {
        var updates = await mgr.CheckForUpdate();
        if (updates.ReleasesToApply.Any())
        {
          var lastVersion = updates.ReleasesToApply.OrderBy(x => x.Version).Last();

          // User already knows, no need to prompt if they wish to continue

          await mgr.DownloadReleases(new[] { lastVersion });
          await mgr.ApplyReleases(updates);
          await mgr.UpdateApp();

          MessageBox.Show("The application has been updated - please close and restart.");
        }
        else
        {
          MessageBox.Show("You are running the most recent version.");
        }
      }
    }

    public static void OnAppUpdate(Version version)
    {
      // Could use this to do stuff here too.
      Console.WriteLine("OnAppUpdate() - ver: " + version.ToString());
      
    }

    /// <summary>First time running. Show the settings dialog</summary>
    /// <param name="version"></param>
    public static void OnInitialInstall(Version version)
    {
      Console.WriteLine("OnInitialInstall() - ver: " + version.ToString());

      //var exePath = Assembly.GetEntryAssembly().Location;
      //string appName = Path.GetFileName(exePath);
      //
      //var updatePath = ConfigurationManager.AppSettings["UpdatePathFolder"];
      //var packageId = ConfigurationManager.AppSettings["PackageID"];
      //
      //using (var mgr = new UpdateManager(updatePath, packageId, FrameworkVersion.Net45))
      //{
      //  // Create Desktop and Start Menu shortcuts
      //  mgr.CreateShortcutsForExecutable(appName, DefaultLocations, false);
      //}
    }

    public static void OnAppUninstall(Version version)
    {
      Console.WriteLine("OnAppUninstall() - ver: " + version.ToString());
      //var exePath = Assembly.GetEntryAssembly().Location;
      //string appName = Path.GetFileName(exePath);
      //
      //var updatePath = ConfigurationManager.AppSettings["UpdatePathFolder"];
      //var packageId = ConfigurationManager.AppSettings["PackageID"];
      //
      //using (var mgr = new UpdateManager(updatePath, packageId, FrameworkVersion.Net45))
      //{
      //  // Remove Desktop and Start Menu shortcuts
      //  mgr.RemoveShortcutsForExecutable(appName, DefaultLocations);
      //}
    }
  }
}
