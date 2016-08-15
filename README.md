# Dapplo.CaliburnMicro
This will help you to get started with bootstrapping Caliburn.Micro (and eventually MahApps or NotifyIcon WPF)

WORK IN PROGRESS

- Documentation can be found [here](http://www.dapplo.net/blocks/Dapplo.CaliburnMicro) (soon)
- Current build status: [![Build status](https://ci.appveyor.com/api/projects/status/fuaq8ppel23aqqva?svg=true)](https://ci.appveyor.com/project/dapplo/dapplo-caliburnmicro)
- Coverage Status: [![Coverage Status](https://coveralls.io/repos/github/dapplo/Dapplo.CaliburnMicro/badge.svg?branch=master)](https://coveralls.io/github/dapplo/Dapplo.CaliburnMicro?branch=master)
- NuGet package Dapplo.CaliburnMicro: [![NuGet package](https://badge.fury.io/nu/Dapplo.CaliburnMicro.svg)](https://badge.fury.io/nu/Dapplo.CaliburnMicro)
- NuGet package Dapplo.CaliburnMicro.Metro: [![NuGet package](https://badge.fury.io/nu/Dapplo.CaliburnMicro.Metro.svg)](https://badge.fury.io/nu/Dapplo.CaliburnMicro.Metro)
- NuGet package Dapplo.CaliburnMicro.NotifyIconWpf: [![NuGet package](https://badge.fury.io/nu/Dapplo.CaliburnMicro.NotifyIconWpf.svg)](https://badge.fury.io/nu/Dapplo.CaliburnMicro.NotifyIconWpf)

Available Packages:
- Dapplo.CaliburnMicro, Caliburn.Micro Bootstrapper for Dapplo.Addons which takes care of initializing MEF and o.a. your IShell ViewModel
- Dapplo.CaliburnMicro.Metro, contains a MetroWindowManager to make it work with MahApps
- Dapplo.CaliburnMicro.NotifyIconWpf, adds functionality to display a system tray icon ViewModel first, via Hardcodet.NotifyIcon.Wpf

# Quick-start documentation

A demo-project is supplied, see: Dapplo.CaliburnMicro.Demo

## Dapplo.CaliburnMicro

Caliburn.Micro Bootstrapper with Dapplo.Addons which takes care of initializing MEF and o.a. your IShell ViewModel
This bases on Dapplo.Addons.Bootstrapper, to use it you will need to instanciate the bootstrapper.

There is functionality available to support your with building:
- Context menus (with tree support)
- Wizard
- a structured configuration, with a tree where you can hang your "config screens"

Usage:

1. Create ViewModels & Views for Caliburn.Micro which have MEF [Export] attributes, and use [Import] on the Properties you want
2. Implement & export an IShell if you want to have a default "Window" to be opened

3. Create a Dapplication

```
			var application = new Dapplication("Your applicatio name", "optional unique Guid to prevent multiple instances")
			{
				ShutdownMode = ShutdownMode.OnExplicitShutdown
			};
			
```

4. Scan/load assemblies

```
  // Add optional directory to scan
  application.Bootstrapper.AddScanDirectory(@"MyComponents");
  // Make sure Dapplo is loaded
  application.Bootstrapper.FindAndLoadAssemblies("Dapplo.CaliburnMicro*");
```

5. Run

```
			application.Run();
```


## Dapplo.CaliburnMicro.NotifyIconWpf

This is based on a Hardcodet.Wpf.TaskbarNotification dependency, and supplies code to a have ViewModel first approach for a System-Tray icon

Usage:
- Annotate your ViewModel class with the ExportAttribute, give it the typeof(ITrayIconViewModel) parameter. This make sure it's found and instanciated, you can set the initial visibility (as expected) in the view.
- Make your ViewModel extend the ITrayIconViewModel, this forces you to implement the IViewAware from Caliburn.Micro but this can be done by extending e.g. Screen or ViewAware.
- Create a View with a TrayIcon, you don't NEED code behind, e.g. <ni:TrayIcon xmlns:ni="clr-namespace:Dapplo.CaliburnMicro.NotifyIconWpf;assembly=Dapplo.CaliburnMicro.NotifyIconWpf" />
- TrayIcon extends TaskbarIcon (from Hardcodet.Wpf.TaskbarNotification) and only adds an interface and a minimal implementation. This allows you to use it exactly like the documentation of Hardcodet.Wpf.TaskbarNotification describes. Also due to the way things are wired, all Caliburn.Micro logic works as designed (even cal:Message.Attach="[Event TrayLeftMouseDown] = [Action xxxx]")
- ToolTipText -> The IHasDisplayName interface forces the DisplayName property, this is automatically bound to the ToolTipText

## Dapplo.CaliburnMicro.Metro

This is based on a MahApps.Metro dependency, and supplies a IWindowManager implementation which makes things look like "metro" apps.

Usage:
- Make Dapplo.Addons.Bootstrapper scan the dll, by e.g. adding it like this: _bootstrapper.Add(@".", "Dapplo.CaliburnMicro.Metro.dll");

Note: Dialog boxes are *not yet* tested or supported... I might need to have a look at this: https://dragablz.net/2015/05/29/using-mahapps-dialog-boxes-in-a-mvvm-setup/
