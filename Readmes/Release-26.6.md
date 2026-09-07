## ⚠️ Compatibility — read before upgrading

The P2P network protocol and the recording (`.jfs`) file format changed in the 26.5.1 / 26.6 line.

- Every peer in a session, **and the hub / console you connect through**, must run this version or newer. Mixing with 26.5.0 or older can silently corrupt other aircraft's positions instead of failing cleanly - update everything together.
- `.jfs` recordings saved by this version will **not** load in 26.5.0 or older. Older recordings still load here.
- Position records are now length-prefixed / self-describing, so future format additions load (unknown fields skipped) instead of throwing "Unable to read beyond the end of the stream".

## New Features

- **Flight Plan "Clear" now re-fetches your callsign and aircraft type from the sim** instead of leaving a stale manual/SimBrief value.
- **Callsign and type auto-refresh when you change aircraft mid-session**; SimBrief auto-import re-runs if enabled. Prevents JoinFS broadcasting a previous leg's callsign, which caused wrong-livery matches for everyone else. See the [Flight Plan and SimBrief](https://github.com/tuduce/JoinFS/wiki/Flight-Plan-and-SimBrief) wiki page.
- **New command-line tunables** (no Settings entry): `-groundaltitudedeltalimit <m>` (on-ground snap-back tolerance, default 1.5), `-injectionretryseconds <s>` (retry delay for injections the sim refused, default 10), `-tracediagnostics` (first-chance exception + ground-placement tracing, off by default).

## Bug Fixes

- **X-Plane: remote aircraft render again.** The shared position record was read with the X-Plane plugin link's protocol version, which crossed the version gate of a new MSFS-only ground field, so every packet failed with "Unable to read beyond the end of the stream". The X-Plane path is now pinned to the exact byte layout the native plugin speaks, and the position record was made length-prefixed so this can't recur.
- **Substitute aircraft are grounded using their own real `STATIC CG TO GROUND`, not the sender's**, so a substitute of any size sits correctly on the ground. Two on-ground regimes: ordinary ground hands the vertical axis to the sim's gear physics (JoinFS only commands horizontal position + heading); a genuine raised structure (helipad, deck, rig, rooftop) holds the sender's reported altitude and attitude. Retractable gear is forced down whenever the sender is on the ground. Applies to live and recorded/played-back injected aircraft.
- **FS2020/FS2024: traffic appears without toggling the sim connection.** Injections attempted while MSFS was still loading were marked permanently failed; they now retry on a backoff and re-arm on a fresh connection or SimStart.
- **Crashes to desktop now leave `crash-<port>.txt`** with a full stack trace. The work thread is guarded so a single error is logged and JoinFS keeps running; a storm escalates to a clean shutdown. Startup prompts once if a crash file from a previous run is waiting.
- **Fixed a SimConnect request-ID collision** that could apply one aircraft's ground-clearance reading to another; each polled object now has its own persistent request ID.
- **Fixed jitter right after a substitute spawns** - the sender's on-ground flag must now hold for 0.3 s before it is trusted.
- **Fixed persistent jitter on some substitutes** (several FSLTL models) that previously needed a 50 cm+ manual height tweak: the on-ground hard-reset tolerance is now 1.5 m instead of 20 cm, so it stops fighting the sim's gear physics.
- **Fixed `ATC FLIGHT NUMBER` callsign synthesis** misfiring on a numeric flight number with a trailing letter (e.g. `34U` → now `EWG34U` instead of bare `34U`).
- **Improved title-based model matching** - all words of 3+ characters are now treated as relevant for submatches.
- **Fixed the public hub list failing to populate** when `raw.githubusercontent.com` returned HTTP 404 - routed through the jsDelivr CDN with a fork fallback.

## Limitations

The `FSX` and `P3D` variants are built for the x86 (32bit) architecture. Since the Microsoft.ML package does not currently offer a x86 variant, the AI-enchanced model matching is not included for `FSX` or `P3D`.

## Known Issues

- A genuinely crooked platform that exists on neither your nor the sender's scenery cannot be reproduced. A very shallow platform may not be recognised as elevated and would settle slightly low; the manual height override in the Aircraft window remains available.
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
