# Dapplo.CaliburnMicro
This will help you to get started with bootstrapping Caliburn.Micro (and eventually MahApps or NotifyIcon WPF)

- Documentation can be found [here](http://www.dapplo.net/blocks/Dapplo.CaliburnMicro) (soon)
- Current build status: [![Build status](https://ci.appveyor.com/api/projects/status/fuaq8ppel23aqqva?svg=true)](https://ci.appveyor.com/project/dapplo/dapplo-caliburnmicro)
- Coverage Status: [![Coverage Status](https://coveralls.io/repos/github/dapplo/Dapplo.CaliburnMicro/badge.svg?branch=master)](https://coveralls.io/github/dapplo/Dapplo.CaliburnMicro?branch=master)
- NuGet package Dapplo.CaliburnMicro: [![NuGet package](https://badge.fury.io/nu/Dapplo.CaliburnMicro.svg)](https://badge.fury.io/nu/Dapplo.CaliburnMicro)
- NuGet package Dapplo.CaliburnMicro.Metro: [![NuGet package](https://badge.fury.io/nu/Dapplo.CaliburnMicro.Metro.svg)](https://badge.fury.io/nu/Dapplo.CaliburnMicro.Metro)
- NuGet package Dapplo.CaliburnMicro.NotifyIconWpf: [![NuGet package](https://badge.fury.io/nu/Dapplo.CaliburnMicro.NotifyIconWpf.svg)](https://badge.fury.io/nu/Dapplo.CaliburnMicro.NotifyIconWpf)


This project is a combination of multiple Dapplo libraries, and brings roughly:
- A bootstrapper for Caliburn.Micro with MEF (Managed Extension Framework)
- addons to your application, via Dapplo.Addons. Use MVVM via composition (e.g. you can make a context menu and have the items provided by addons)
- configuration & translations based on interfaces via Dapplo.Config
- Some UI components like System-tray icon, menus, wizard are provided with a MVVM first approach.
- Automatically style your Views via MahApps (if there is a reference to the Dapplo.CaliburnMicro.Metro.dll this will be used automatically)

THIS IS WORK IN PROGRESS


Available Packages:
- Dapplo.CaliburnMicro, Caliburn.Micro Bootstrapper for Dapplo.Addons which takes care of initializing MEF and o.a. your IShell ViewModel
- Dapplo.CaliburnMicro.Dapp, adds a replacement for Application which helps to bootstrap
- Dapplo.CaliburnMicro.Configuration, adds a MVVM configuration component and automatic Dapplo.Ini importing
- Dapplo.CaliburnMicro.Translation, adds automatic Dapplo.Language importing
- Dapplo.CaliburnMicro.Security, adds security to your UI
- Dapplo.CaliburnMicro.Security.ActiveDirectory, adds AD-Role based permissions to your UI
- Dapplo.CaliburnMicro.Wizard, adds a MVVM wizard component
- Dapplo.CaliburnMicro.Menu, adds a MVVM menu component
- Dapplo.CaliburnMicro.Toasts, adds a MVVM toasts
- Dapplo.CaliburnMicro.Cards, adds a MVVM adaptive card implementation
- Dapplo.CaliburnMicro.Metro, contains a MetroWindowManager to make it work with MahApps
- Dapplo.CaliburnMicro.NotifyIconWpf, adds functionality to display a system tray icon ViewModel first, via Hardcodet.NotifyIcon.Wpf

# Quick-start documentation

A demo-project is supplied, see: Application.Demo

## Dapplo.CaliburnMicro

A Caliburn.Micro support project, with Dapplo.CaliburnMicro.Dapp taking care of bootstrapping Caliburn.Micro with Dapplo.Addons, which takes care of initializing MEF and o.a. your IShell ViewModel

There is functionality available to support your with building:
- Simplified binding of properties
- Some build in behaviors, like Visibility, IsEnabled and authentication behaviors which hide/disable elements depending on a value.
- Dapplo.CaliburnMicro.Configuration a structured configuration, with a tree where you can hang your "config screens"
- Dapplo.CaliburnMicro.Menu Context menus (with tree support)
- Dapplo.CaliburnMicro.Translations to support translations
- Dapplo.CaliburnMicro.Wizard a simple wizard component
- Dapplo.CaliburnMicro.Security add security behaviour (enables / visible) depending on rights
 
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
  // Make sure Dapplo and all modules are loaded
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
- Make your ViewModel extend the TrayIconViewModel, this forces you to implement the IViewAware from Caliburn.Micro but this can be done by extending e.g. Screen or ViewAware.
- Create a View with a TrayIcon, you don't NEED code behind, e.g. <ni:TrayIcon xmlns:ni="clr-namespace:Dapplo.CaliburnMicro.NotifyIconWpf;assembly=Dapplo.CaliburnMicro.NotifyIconWpf" />
- TrayIcon extends TaskbarIcon (from Hardcodet.Wpf.TaskbarNotification) and only adds an interface and a minimal implementation. This allows you to use it exactly like the documentation of Hardcodet.Wpf.TaskbarNotification describes. Also due to the way things are wired, all Caliburn.Micro logic works as designed (even cal:Message.Attach="[Event TrayLeftMouseDown] = [Action xxxx]")
- ToolTipText -> The IHasDisplayName interface forces the DisplayName property, this is automatically bound to the ToolTipText

## Dapplo.CaliburnMicro.Metro

This is based on a MahApps.Metro dependency, and supplies a IWindowManager implementation which makes things look like "metro" apps.

Usage:
- Make Dapplo.Addons.Bootstrapper scan the dll, by e.g. adding it like this: _bootstrapper.Add(@".", "Dapplo.CaliburnMicro.Metro.dll");

Note: Dialog boxes are *not yet* tested or supported... I might need to have a look at this: https://dragablz.net/2015/05/29/using-mahapps-dialog-boxes-in-a-mvvm-setup/


TODO: The popup is shown briefly, so briefly it's impossible to use actions.


General TODO:
Add better error support