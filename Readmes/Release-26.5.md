## New Features

- **SimBrief flight-plan import.** Save your SimBrief username once (Flight plan → SimBrief username → Import from SimBrief) and JoinFS automatically fetches your latest OFP on every startup from then on - no need to open any dialog. A new main-screen button shows the active flight plan as `DEP ➜ DEST` (or "Flight plan" when none is set), and a SimBrief icon button shows a green ✓/red ✕ badge for the last fetch and lets you re-fetch on demand.
- **Real callsigns instead of tail numbers.** JoinFS now reads `ATC FLIGHT NUMBER` and synthesizes a proper callsign from ICAO airline + flight number (falling back to the live in-sim Call Sign, then the tail number), and separates `registration` from `callsign` as distinct fields end-to-end - network protocol, WebSocket feed, and the EuroScope bridge all now show the real callsign instead of the aircraft's tail number.
- **Registration-aware model matching.** Substitution now also scores candidate liveries by registration - an exact match against each livery's scanned `aircraft.cfg atc_id`, or a weaker text match against title/variation - alongside the existing ICAO-airline and livery-name-overlap signals, improving matches for one-off/special liveries (e.g. a specific tail's retro livery).
- **New fields on the wire:** `registration`, `icaoAirline`, `flightNumber`, and `livery` are now part of the network protocol (gated behind a `dataVersion` bump, so older peers are unaffected) and the WebSocket feed. [joinfs-euroscope-bridge](https://github.com/joeherwig/joinfs-euroscope-bridge) forwards the real callsign and full flight plan to EuroScope, and [joinfs-map-websocket-webcomponent](https://github.com/joeherwig/joinfs-map-websocket-webcomponent)'s aircraft popup now shows Airline/Flight No./Registration/Livery rows and a hover tooltip.
- Substitution model loads now run on a background thread instead of blocking the sim/network loop and UI refresh timers, so large model-scan folders no longer cause a stall.

## Bug Fixes

- Fixed COM1/COM2 changes not being broadcast (and not triggering the COM webhook) in some cases.
- Fixed EuroScope flight-plan lookups (`$CQ:FP`) silently failing on callsign case/whitespace mismatches; a genuine miss is now logged instead of dropped silently.
- Departure/destination ICAO codes are now normalized to uppercase in the flight-plan dialog.

## Limitations

Peers running a version older than 26.5 won't see the new `registration`/`icaoAirline`/`flightNumber` fields from a 26.5 peer (and vice versa) - the network protocol negotiates this automatically via `dataVersion`, so mixed-version sessions remain compatible, just without the new fields between the older peer and everyone else.

The COM1/2 channel with 8.33kHz separation is displayed correctly only if the other pilots use joinfs version 26.4 or later. The previous versions of joinfs do not support 8.33kHz separation and will broadcast the COM frequencies in the format `XXX.XX` (e.g., `123.50`).

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
