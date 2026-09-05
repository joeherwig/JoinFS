## Bug Fixes

- **Remote aircraft now sit on the ground the same way locally-flown ones do, whatever model your client substitutes.** When a sender flew a small aircraft (e.g. a C172) and your client substituted a much larger model, the substitute could be shown nose-gear-up, with one helicopter skid in the air, or jittering several metres in altitude - the per-model ground-clearance correction was being driven against the simulator's own gear/ground-contact physics every frame. On-ground handling is now split into two cases. On ordinary ground JoinFS seeds the substitute at *its own* real clearance above the local terrain and then hands the vertical axis and pitch/bank entirely to the simulator's own contact-point physics, commanding only horizontal position and heading - so the substitute settles on its gear exactly like a local aircraft, regardless of how differently sized it is from the reported aircraft. On a genuine raised structure (helipad, ship deck, oil rig, rooftop) that your scenery doesn't have, JoinFS still holds the aircraft at the sender's reported altitude and attitude so it doesn't drop to the terrain below. The landing gear of a retractable substitute is now also forced down whenever the sender is on the ground, so it can't animate mid-travel while the simulator's AI gear logic and JoinFS disagree. (Thanks @joeherwig for the contribution)
- **FS2020/FS2024: other aircraft now appear without having to toggle the simulator connection.** If JoinFS tried to inject traffic while MSFS was still on the main menu or loading a flight, the injection was marked failed permanently and nothing retried it until a `[Sim]` reconnect flushed and re-listed every object. Failed injections now retry automatically on a short backoff (default 10s, `-injectionretryseconds`), and are re-armed outright when the simulator sends a fresh connection or SimStart event. (Thanks @joeherwig for the contribution)
- **Crashes to desktop now leave a diagnostic file.** JoinFS's work thread had no exception guard, so any error raised in it killed the process silently on .NET 8 with nothing written anywhere. That loop is now guarded - a single error is logged with a full stack trace to `crash-<port>.txt` (context, version, simulator, recent log lines) and JoinFS keeps running; a storm of repeated errors escalates to a clean shutdown with a dialog instead of a zombie process. All three unhandled-exception handlers now write the same crash file first (independently of the normal log, so a fault while the work thread holds its lock still produces a file), the previous-log file is kept for several generations so a crash log survives your next few launches, and JoinFS prompts you once on startup if a crash file from a previous run is waiting to be sent in. (Thanks @joeherwig for the contribution)

## Command-line options

Three test-phase tunables were added (no Settings-dialog entry, matching the existing elevated-platform flags):

- `-groundaltitudedeltalimit <m>` - vertical divergence tolerance before an on-ground substitute on ordinary ground is snapped back to its target (default `1.5`).
- `-injectionretryseconds <s>` - delay before retrying an injection the simulator refused while still loading (default `10`).
- `-tracediagnostics` - enables first-chance exception logging to `firstchance-<port>.txt` and the per-tick ground-placement trace in the Monitor window's Network category. Off by default.

## Limitations

The `FSX` and `P3D` variants are built for the x86 (32bit) architecture. Since the Microsoft.ML package does not currently offer a x86 variant, the AI-enchanced model matching is not included for `FSX` or `P3D`.

## Known Issues

- A genuinely crooked platform that exists on neither your nor the sender's scenery cannot be reproduced. A very shallow platform (a low deck the sender's own terrain probe still "sees over") may not be recognised as elevated and would then settle slightly low. The manual height override in the Aircraft window remains available for both.
- Some XPLANE models appear incomplete (when the model has a space in the filenames of the model data).
- When moving the timeline of a recording in XPLANE, the recorded aircraft disappears.
- When in XPLANE an aircraft model is substituted, the new model is displayed in the center of gravity of the original model. If the replacement model is smaller than the original model, it may appear to be floating in the air. If the replacement model is larger than the original model, it may appear to be embedded in the ground.

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
