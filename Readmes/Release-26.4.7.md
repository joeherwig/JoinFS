## New Features

- **Auto-detect simulator folder on first install.** JoinFS now tries to resolve your simulator's aircraft/content folder on its own - MSFS's `UserCfg.opt` (FS2020/FS2024, both Steam and MS Store/Xbox), the FSX/Prepar3D registry keys, or X-Plane's `x-plane_install_*.txt` - instead of requiring a manual "Scan For Models" folder browse. For FSX/Prepar3D/X-Plane, the model scan now runs immediately once a folder is known (detected or picked); for FS2020/FS2024 the folder is pre-configured but the scan itself still runs at the next sim connect, since their model data isn't available until then.
- **Expanded first-run setup dialog.** The old "enter your nickname" prompt now also asks for your SimBrief username (fetching your flight plan immediately once saved) and, only if the simulator folder couldn't be auto-detected, a folder picker.
- **Double-click to join a hub.** In the Public Hubs window, double-clicking a hub now joins it and closes the window directly, instead of select → right-click → Join → close manually. A new hint label next to "Right-click for options" calls this out.

## Bug Fixes

- Fixed FS2024/FS2020 folder auto-detection rejecting valid MSFS Packages folders shared between both MSFS versions (e.g. `Official2024`/`Community2024` instead of the exact `Official`/`Community` names).
- Fixed a stale-but-existing folders file (blank simulator folder, left over from an earlier version or an abandoned manual scan) permanently blocking auto-detection from ever running.
- FS2024's SimConnect-based community model fetch (the "My MSFS 2024" add-on, now labeled "FS2024 models via SimConnect" in Scan For Models) is enabled by default, instead of requiring a manual visit to Scan For Models to tick it.
- Fixed the first-run setup dialog's folder-picker section never appearing, due to reading a flag that had already been reset by the caller.
- Fixed the dialog's tab order jumping to the OK button before the SimBrief username field, and the nickname field not being pre-filled (which could blank an already-valid nickname if the dialog was shown for another reason).
- Hid the non-functional "Subfolder" grid in Scan For Models on FS2020/FS2024 builds, where a modern MSFS Packages layout can never populate it (that layout only applies to FSX/Prepar3D installs).

## Documentation

- Added a "Getting Started" section to the [wiki's Home page](https://github.com/tuduce/JoinFS/wiki) covering the first-run setup flow, including the auto-detected simulator folder and the hub-list double-click shortcut.

## Installation

Please follow the instructions for your simulator.

### MSFS2024 or MSFS2020

Please make sure that you have the .NET 8.0 runtime installed. You can download it from the [.NET download page](https://dotnet.microsoft.com/en-us/download/dotnet/8.0).

Download the installer corresponding to your simulator version (`JoinFS-FS2024.msi` or `JoinFS-FS2020.msi`). If upgrading from a `3.2.x` version, please uninstall the previous version before installing the new one.

### FSX or P3D

Please make sure that you have the .NET 8.0 runtime installed. You can download it from the [.NET download page](https://dotnet.microsoft.com/en-us/download/dotnet/8.0).

Download the installer corresponding to your simulator version (`JoinFS-FSX.msi` or `JoinFS-P3D.msi`). If upgrading from a `3.2.x` version, please uninstall the previous version before installing the new one.

Please make sure that you have the SimConnect SDK installed for your simulator version.

### XPLANE

Please make sure that you have the .NET 8.0 runtime installed. You can download it from the [.NET download page](https://dotnet.microsoft.com/en-us/download/dotnet/8.0).

Download the installer corresponding to your simulator version (`JoinFS-XPLANE.msi`). If upgrading from a `3.2.x` version, please uninstall the previous version before installing the new one.

If you are installing JoinFS for the first time, start JoinFS before starting XPLANE. From JoinFS install the plugin into XPLANE using the "Install XPLANE Plugin" button in the settings dialog.

### CONSOLE

The `CONSOLE` variant is compiled for `x64` architectures.

Please make sure that you have the .NET 8.0 runtime installed. You can download it from the [.NET download page](https://dotnet.microsoft.com/en-us/download/dotnet/8.0).

Download the ZIP file (`JoinFS-CONSOLE.zip`) and extract it to a folder of your choice. Follow the instructions in the `Old-Readme.txt` file.
