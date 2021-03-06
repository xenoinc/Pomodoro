# Desktop Pomodoro
Simple desktop Pomodoro timer that sits in your system tray's icons.

## Features
* 25 min pomodoro count down
* 15 min long break
* 5 min short break
* Pause/Stop/Start


## Pomodoro Technique
Pomodoro (/Po-mo`-do-ro/)simply means, tomato in Italian.

The **Pomodoro Technique** is a time management method developed by Francesco Cirillo in the late 1980s. The technique uses a timer to break down work into intervals, traditionally 20 minutes in length, separated by short breaks. These intervals are called **pomodoros**, the plural in English of the Italian word _pomodoro_, which means **tomato**. The method is based on the idea that frequent breaks can improve mental agility.

More information can be found at [PomodoroTechnique.com](http://pomodorotechnique.com/), or just read about it on Wikipedia.

## Six stages in the technique:

1. Decide on the task to be done
1. Set the pomodoro timer to n minutes (traditionally n = 25)
1. Work on the task until the timer rings. If a distraction pops into  your head, write it down, but immediately get back on task.
1. After the timer rings, put a checkmark on a piece of paper
1. If you have less than four checkmarks, take a short break (3–5 minutes), then go to step 1
1. Else (i.e. after four pomodoros) take a longer break (15–30 minutes), reset your check mark count to zero, then go to step 1


# Change Log
### 1.3.53
* Added single app-instance only
* Added base framework for settings engine
* Fixed sound chime on transition change
* Preparing to move to [ToolsHub](https://github.com/xenoinc/ToolsHub)

### 1.2.52
* Updated namespace and refactored code

### 1.1.51
* Updated Version system to use SemVer (Major.Minor.Patch). More info at, http://semver.org/.
* Fully integrated Squirrel updater.
* Improperly setting update folder (for initial testing).

### 1.1.17091.48
* Chime fixes
* Added some lost code from late January and February. Forgot to PUSH & drive became corrupted.
* Holy crap this thing is much more pretty now!
* Added GUI notifications to flash symbols on the screen during state changes.
    * Useful for when audio is unavailable. The old MsgBox was an annoying modal.
    * Notification window is topmost during flashes
    
### 1.1.17011.3
* Added chime sounds to end of 25min run.
    
### 1.0 - 2016
* Basic features. Needed a quick timer project pronto

### 0.x - 2016:
* Fixed TrayIcon to an easy to view color w/ backdrop
* Fixed initial application launch TrayIcon (unset previously)
