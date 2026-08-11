## Elevated Platform Recognition

- **Helicopters (and optionally other aircraft) no longer sink through helipads, ship decks, or rooftops for other pilots.** Remote aircraft position updates already carried an on-ground flag, but it was parsed and then discarded - JoinFS always corrected a nearby remote aircraft's altitude toward the receiver's own local terrain-mesh reading, which has no knowledge of scenery objects sitting above it. When a peer reports on-ground and the mismatch between what they measured and the receiver's local mesh is large enough to indicate a real platform (not just ordinary cross-client mesh noise), JoinFS now trusts the sender's reported altitude directly, and on MSFS/SimConnect builds also forwards on-ground to the sim's own AI-object placement so its structure-aware collision physics rests the object on the real geometry - the same mechanism your sim already uses correctly for your own aircraft.
- On by default for helicopters, with no configuration needed. Controlled entirely via command-line flags (no Settings-dialog entry, matching the existing WebSocket/webhook flags): `-elevatedplatformrecognition <true|false>` (master switch, default `true`), `-elevatedplatformhelicoptersonly <true|false>` (default `true`; set to `false` to also cover fixed-wing edge cases like carrier deck operations), and `-elevatedplatformthreshold <cm>` (default `50`). See the [wiki](https://github.com/joeherwig/JoinFS/wiki/Elevated-Platform-Landing) for details and examples.
- Added an `onGround` field to the WebSocket telemetry feed (`-websocket`) for external tools consuming live aircraft state.
- If you want to check whether elevated platform recognition engaged for a particular aircraft, enable the **Network** category in JoinFS's Monitor window - it logs the callsign, computed mismatch, configured threshold, and whether platform-trust engaged whenever the decision changes.

## SimBrief / Flight Plan

- The SimBrief button on the main screen is now a text button colored the same green/red (Active/Inactive) as the Simulator/Network buttons on a successful/failed fetch, instead of an icon with a small badge overlay. It's sized and aligned to match the Join Global button, and is hidden entirely until a SimBrief username is configured - the Flight Plan button expands to fill the freed space in that case.
- Fixed the Flight Plan dialog's callsign field being read-only - it's now the one place to set/override a callsign (see below).
- Pressing Enter in the SimBrief username field now saves the username and runs the import before closing the dialog, instead of closing immediately without saving anything.
- Fixed the Flight Plan dialog's tab order (SimBrief username -> Import -> Clear -> OK -> Cancel), which had collided with the OK/Cancel buttons' tab indices.

## Callsign Handling

- **Fixed callsign duplication after upgrading from an older version.** The new real-callsign synthesis (ICAO airline + flight number) concatenated the two unconditionally. `ATC FLIGHT NUMBER` was write-only/unread by JoinFS before this line of releases, so many add-ons/pilots had stored an entire pre-existing callsign there instead of a bare numeric flight number - once read, the airline code got glued onto it again (e.g. `DLH1234` becoming `DLHDLH1234`). Now trusted as-is whenever it already carries the airline prefix or isn't purely numeric.
- Removed the old "Edit Callsign" aircraft-list context menu and its persistent per-installed-model override (which also silently applied to any locally-simulated AI traffic sharing that model, not just your own aircraft). The Flight Plan dialog's callsign field is now the single, session-scoped place to set a callsign.

## Model Matching (FS2024)

- **Fixed real, reproducible misclassifications of installed community liveries.** FS2024 catalogs community aircraft via SimConnect model enumeration, which provides only a title/livery string - no file path, no config content - so JoinFS previously fell back to guessing the ICAO type purely from the title text whenever real data wasn't available. This could pick the wrong aircraft entirely: e.g. an Airbus A320 livery titled `..._Smart_Lynx` was tagged as a Rotorcraft, because "Lynx" (from the airline name "Smart Lynx") is also a real Doc8643 helicopter designator and won a tie-break against the correct "A320".
- JoinFS now tries reading the real `aircraft.cfg` - following `base_container` references to the correct file when the installed variant is a bare livery overlay - directly from disk before falling back to guessing, the same logic already used for aircraft you've actually spawned/flown, now also applied to the full installed-model catalog. Also fixed the classic-scan folder discovery to correctly walk a modern Community/Official package-based install instead of assuming a flat layout that doesn't exist there.
- Fixed a data error in the bundled model-to-typerole reference list: a real airship (Skyship 600) was tagged as a Rotorcraft.
- Manual "Scan For Models" no longer freezes the app while scanning - it now runs in the background like the automatic on-connect scan already did.
- Added logging for any unhandled exception (on any thread, including background scans) before the process would otherwise exit with no trace at all - not a fix for any specific crash, but turns future silent closes into diagnosable ones.

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
