## About this release

A rebuild of the `26.6-beta` fixes against tuduce/JoinFS's new JFP2 network-protocol and
simulator-thread reorganization (merged upstream in `main` as #181). Seven fixes originally opened
against the pre-reorg `main` were re-verified and, where the underlying code had moved or changed
shape, re-implemented against the new architecture rather than just reapplied as-is.

## What's included

- **Callsign-from-flight-number suffix fix**: a flight number with a trailing letter suffix (e.g.
  `34U`) is now correctly combined with the ICAO airline into a full callsign (`EWG34U`) instead of
  being broadcast bare.
- **WebSocket/webhook feed improvements**: a new `trafficType` field distinguishes a real pilot
  from replayed/AI traffic; non-finite or out-of-range position data is now skipped and logged
  instead of crashing the feed; and aircraft that used to collapse onto the same identity (your own
  aircraft vs. a replayed one, or several aircraft from one peer) now each get a distinct, stable
  identity.
- **Global keyboard shortcuts for Record/Overdub/Stop/Replay**: four new hotkeys for VR users who
  can't easily reach the Recorder UI, off by default and reassignable via File → Shortcuts.
- **Duplicate ("ghost") aircraft on reconnect**: a network identity/position packet racing ahead of
  (or arriving just after) a peer's own join/leave handshake no longer creates a short-lived
  duplicate aircraft with an unresolvable identity.
- **Retry injections the simulator refused while loading**: traffic that failed to inject because
  the simulator was still on the main menu or loading a flight now retries automatically every 10
  seconds (up to 30 attempts) instead of being skipped forever until a manual `[Sim]` toggle.
- **Keep the pilot's callsign and aircraft type in sync with the simulator**: changing aircraft
  mid-session (a real swap, or a different registration/livery on the same airframe) now updates the
  broadcast callsign/type, so other clients' livery matching keys off the current value instead of a
  stale one.
- **Reconnect after losing every peer**: a client that loses every peer - including the hub, if
  joined directly - while still reporting "Connected" now reconnects automatically every 10 seconds,
  for both a direct join and an email/hub-account login. The Network button shows a third (Waiting)
  color while this is happening, instead of reading as a healthy Connected the whole time.

## Installation

Please follow the instructions for your simulator.

### MSFS2024 or MSFS2020

Please make sure that you have the .NET 8.0 runtime installed. You can download it from the
[.NET download page](https://dotnet.microsoft.com/en-us/download/dotnet/8.0).

Download the installer corresponding to your simulator version (`JoinFS-FS2024.msi` or
`JoinFS-FS2020.msi`). This installer upgrades an existing `26.6-beta` install automatically.

### FSX or P3D

Please make sure that you have the .NET 8.0 runtime installed. You can download it from the
[.NET download page](https://dotnet.microsoft.com/en-us/download/dotnet/8.0).

Download the installer corresponding to your simulator version (`JoinFS-FSX.msi` or
`JoinFS-P3D.msi`). Please make sure that you have the SimConnect SDK installed for your simulator
version.

### XPLANE

Please make sure that you have the .NET 8.0 runtime installed. You can download it from the
[.NET download page](https://dotnet.microsoft.com/en-us/download/dotnet/8.0).

Download the installer (`JoinFS-XPLANE.msi`). If you are installing JoinFS for the first time,
start JoinFS before starting XPLANE, then install the plugin into XPLANE using the
"Install XPLANE Plugin" button in the settings dialog.

### CONSOLE

The `CONSOLE` variant is compiled for `x64` architectures.

Please make sure that you have the .NET 8.0 runtime installed. You can download it from the
[.NET download page](https://dotnet.microsoft.com/en-us/download/dotnet/8.0).

Download the ZIP file (`JoinFS-CONSOLE.zip`) and extract it to a folder of your choice. Follow the
instructions in the `Old-Readme.txt` file.
