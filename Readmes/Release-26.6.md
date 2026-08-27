## New Features

- **The Flight Plan dialog's Clear button now re-fetches your callsign and aircraft type from the sim**, instead of leaving whatever was last shown (which could be a stale manually-typed or SimBrief-imported value with no relation to what's actually loaded).
- **Callsign/type now auto-refresh when you change aircraft mid-session, and SimBrief auto-import re-runs if autoimport is enabled** Previously, swapping to a different aircraft after landing a leg left JoinFS still broadcasting the previous aircraft's callsign and type indefinitely - since other pilots' clients key livery matching off your callsign, a stale one caused wrong-livery matches for everyone else on the network, not just a local display issue. JoinFS now detects a real change (a different aircraft type, or a different registration/callsign - even on the same airframe) and automatically re-derives your callsign/type from the sim, resetting to auto-tracking even if you'd manually edited it for the previous leg. If SimBrief auto-import is enabled, a fresh SimBrief fetch is triggered too, the same way it already happens once at JoinFS startup. See the [Flight Plan and SimBrief](https://github.com/tuduce/JoinFS/wiki/Flight-Plan-and-SimBrief) wiki page for a recommended workflow when flying consecutive legs with SimBrief.

## Bug Fixes

- **Substitute aircraft are now grounded using their own real clearance, not the sender's.** JoinFS now reads each aircraft's own live `STATIC CG TO GROUND` and, whenever the sender reports on-ground, corrects the local substitute's altitude by the difference between its own clearance and the sender's - so it sits correctly on the ground no matter how differently sized the substitute is from the original aircraft. Strictly gated on the sender's own reported on-ground state, so a flying aircraft is never pulled toward a ground-relative correction, and smoothed rather than snapped so a flickering on-ground flag doesn't itself introduce a visible pop. applied that correction to recorded/played-back injected aircraft as well.
- **Fixed a SimConnect request-ID collision that could momentarily apply one aircraft's ground-clearance reading to a different aircraft.** Every locally-polled object shared a single request ID for its periodic position/geometry poll, which SimConnect could occasionally cross-match between concurrent requests. Each polled object now gets its own persistent request ID. 
- **Fixed jitter right after a substitute is spawned.** The sender's raw on-ground flag can flicker for a moment while a newly-injected object's physics is still settling onto the ground; each flicker re-targeted the ground-clearance correction above and produced a short but visible jitter until the flag settled. The flag now has to hold its value for 0.3s before it's trusted.
- **Fixed persistent jitter on some substitutes (several FSLTL models, among others) that previously needed a large manual height adjustment (50cm+) to work around.** Injected aircraft are normally moved by nudging their velocity, letting the sim's own gear/suspension physics carry them smoothly - but once an object was on the ground, any altitude gap over just 20cm between JoinFS's computed placement and where the object's own physics had already settled it forced a hard position reset, which the sim's gear physics then fought every time, producing jitter. That tolerance is now 1.5m, comfortably absorbing realistic per-model ground-clearance imprecision instead of fighting it.
- **Fixed `ATC FLIGHT NUMBER`-based callsign synthesis misfiring on an ordinary flight number with a trailing letter suffix** (e.g. a real Eurowings flight reporting `34U`). The "is this field already a complete pre-existing callsign" check treated any non-purely-numeric flight number as already complete and used it bare, instead of combining it with the ICAO airline into a real callsign (`EWG34U`). Now uses the same callsign-shape check already used elsewhere in JoinFS to tell an already-complete callsign apart from a normal flight number.
- **Fixed aircraft title submatch not taken into account (all words with minimum 3 chars are treated as relevant now)

## Limitations

The `FSX` and `P3D` variants are built for the x86 (32bit) architecture. Since the Microsoft.ML package does not currently offer a x86 variant, the AI-enchanced model matching is not included for `FSX` or `P3D`.

## Known Issues

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
