Pomodoro uses Squirrel for distribution and updating

See details on [Squirrel Installer](https://github.com/Squirrel/Squirrel.Windows/blob/master/docs/getting-started/0-overview.md
)

## Integrating Squirrel
Reference: [Integrating Squirrel](https://github.com/Squirrel/Squirrel.Windows/blob/master/docs/getting-started/1-integrating.md)

### Install Squirrel

```powershell
PM> Install-Package Squirrel.Windows
```

### Implementing

```cs
using Squirrel;
...
using (var mgr = new UpdateManager("C:\\Projects\\MyApp\\Releases"))
{
    await mgr.UpdateApp();
}
```


## Creating new package

### Building
1. VS: Change project to Release and build and record Version number

### Packaging

1. Open .\Pomodoro.nuspec
2. Make sure version info matches our output (_1.1.49_)
    * Must use _MAJOR.MINOR.PATCH_ format
3. Include all files from **.\bin**
4. Save package (_Pomodoro.1.1.49.nupkg_) to root

### Releasifying
1. VS: Open **Package Manager Console**
2. Make sure we're in the root folder
```
PM> pwd
PM> cd ..
```
3. **Type:** ```Squirrel --releasify Pomodoro.1.1.49.nupkg```

## Distributing

### Where to Copy
Copy contents of _Releases_ folder to

* **Local Debug:** ```C:\temp\pomodoro\```
* **Production:** ```https://software.xenoinc.com/pomodoro/releases```

### Give to Public
Give the public the Setup.EXE or MSI file. (usually .exe)

## Installer
The ```Setup.exe``` application does the following (see [Install Process](https://github.com/Squirrel/Squirrel.Windows/blob/master/docs/using/install-process.md) for details):

* Creates a ```%LocalAppData%\Pomodoro``` directory for the Pomodoro to be installed.
* Extracts and prepares the Pomodoro files under an ```app-1.0.0``` directory.
* Launches ```app-1.0.0\Pomodoro.exe``` at the end of the setup process.

## Updating
See also [Squirrel's Updating](https://github.com/Squirrel/Squirrel.Windows/blob/master/docs/using/install-process.md) page for more info.

1. Update the version numbers in **AssemblyInfo.cs** (_1.1.51_)
2. Update the .nuspec version number to match
3. Execute Releasify from VS (_or PowerShell_)
```
PM> pwd
PM> cd ..
PM> Squirrel --releasify Pomodoro.1.1.51.nupkg
```

## Loading GIF
Squirrel installers don't have any UI - the goal of a Squirrel installer is to install so blindingly fast that double-clicking on Setup.exe *feels* like double-clicking on an app shortcut. Make your installer **fast**.

However, for large applications, this isn't possible. For these apps, Squirrel will optionally display a graphic as a "splash screen" while installation is processing, but only if installation takes more than a pre-set amount of time. This will be centered, backed by a transparent window, and can optionally be an animated GIF. Specify this via the `-g` parameter.

```powershell
PM> Squirrel --releasify MyApp.1.0.0.nupkg -g .\loading.gif
```

## See Also
* [Squirrel Command Line](squirrel-command-line.md) - command line options for `Squirrel --releasify`


---
| Return: [Table of Contents](../readme.md) |
|----|
