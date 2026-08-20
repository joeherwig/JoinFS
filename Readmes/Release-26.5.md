## New Features

- **Auto-detect simulator folder on first install.** JoinFS now tries to resolve your simulator's aircraft/content folder on its own - MSFS's `UserCfg.opt` (FS2020/FS2024, both Steam and MS Store/Xbox), the FSX/Prepar3D registry keys, or X-Plane's `x-plane_install_*.txt` - instead of requiring a manual "Scan For Models" folder browse. For FSX/Prepar3D/X-Plane, the model scan now runs immediately once a folder is known (detected or picked); for FS2020/FS2024 the folder is pre-configured but the scan itself still runs at the next sim connect, since their model data isn't available until then.
- **Expanded first-run setup dialog.** The old "enter your nickname" prompt now also asks for your SimBrief username (fetching your flight plan immediately once saved) and, only if the simulator folder couldn't be auto-detected, a folder picker.
- **Double-click to join a hub.** In the Public Hubs window, double-clicking a hub now joins it and closes the window directly, instead of select → right-click → Join → close manually. A new hint label next to "Right-click for options" calls this out.
- **Model Matching Redesign: unified scoring-based matcher.** Replaces the old tiered ICAO/Category/Auto fall-through with a single weighted scorer (ICAO type, Doc8643 class code, wake turbulence category, engine count/type, registration, ICAO airline, typerole, livery, title-prefix). A candidate no longer needs an exact ICAO type match to win a substitution - sharing class code + WTC can now outscore a same-class-but-wrong-WTC candidate, fixing cases like a Beechcraft Baron remote preferring an installed Diamond DA62 over an installed Douglas DC-3.
- **Real aircraft.cfg/livery.cfg data for FS2024, now for the whole catalog, not just what you fly.** SimConnect's `LIVERY FOLDER` is read the moment a model is actually instantiated locally, parsing its real configuration file for `icao_type_designator`/`icao_WTC`/`icao_airline`/`atc_id` - the same reliability tier non-FS2024 builds already had from their upfront folder scan - ahead of the live category/engine-derived classification and title-guessing, both kept as fallbacks. Handles both same-package and cross-package `base_container` references. This same real-config-reading approach is now also applied while building the full installed-model catalog itself (SimConnect model enumeration only ever provided a title/livery string for that, with no file access at all) - see the Bug Fixes section for the misclassifications this fixes.
- **Hardened title-guessing.** The fallback guesser that infers an ICAO type from a model's title text now requires a longer, less ambiguous match, fixing false positives like a Cessna Caravan livery for an operator named "Baron Aviation" being mistagged as a Beechcraft Baron.
- **Non-flyable models filtered out.** Scenery props, wrecked vehicles, and static display liveries (e.g. tents, cranes, parking-lot cars bundled with some add-ons) are now excluded from the installed-model list entirely via an expanded ban list, so they can no longer appear in the Substitute dialog or become an accidental default fallback.
- **Fine-grained matching defaults.** The "Model Matching" defaults now go beyond one fallback per coarse typerole (Rotorcraft/Airliner/etc.) to also auto-configure defaults per class code + WTC combination, wherever more than one installed candidate exists to choose between.
- **Network protocol extended** (`Sim.VERSION` 21006→21007): peers now also broadcast their own resolved class code/WTC directly, so receiving clients use the sender's best-available classification instead of each independently re-deriving it from ICAO type - which previously failed whenever ICAO type was a bogus/non-standard string. Purely additive; older peers are unaffected.
- **Explain Match dialog implemented.** It shows per-attribute score contributions inline, an "other candidates considered" panel (what almost won and why), and a ban-list exclusion count - both in the dialog and the exported markdown report.
- **ICAO type designator is now validated, not trusted blindly.** A config-confirmed `icao_type_designator` is checked against the official ICAO Doc8643 reference list; if it's missing or isn't a real designator (a real, confirmed add-on authoring mistake: `icao_type_designator="500E"`, not a real code, with the actual designator sitting in the separate `icao_model` field instead), JoinFS now tries the `icao_model` field, then a title-text guess corroborated by a class code/WTC derived from that same file's engine/category fields, before finally falling back to the raw value. 
- **Localization.** All new UI text from the Model Matching redesign, plus existing untranslated text in the Explain Match/Model Matching dialogs and the SimBrief tooltips/status messages, is now localized into all 8 supported languages (German, Spanish, French, Italian, Korean, Dutch, Portuguese, Russian).
- **Elevated platform recognition: helicopters (and optionally other aircraft) no longer sink through helipads, ship decks, or rooftops for other pilots.** Remote aircraft position updates already carried an on-ground flag, but it was parsed and then discarded - JoinFS always corrected a nearby remote aircraft's altitude toward the receiver's own local terrain-mesh reading, which has no knowledge of scenery objects sitting above it. When a peer reports on-ground and the mismatch between what they measured and the receiver's local mesh is large enough to indicate a real platform (not just ordinary cross-client mesh noise), JoinFS now trusts the sender's reported altitude directly, and on MSFS/SimConnect builds also forwards on-ground to the sim's own AI-object placement so its structure-aware collision physics rests the object on the real geometry - the same mechanism your sim already uses correctly for your own aircraft. On by default for helicopters, with no configuration needed. Controlled entirely via command-line flags (no Settings-dialog entry, matching the existing WebSocket/webhook flags): `-elevatedplatformrecognition <true|false>` (master switch, default `true`), `-elevatedplatformhelicoptersonly <true|false>` (default `true`; set to `false` to also cover fixed-wing edge cases like carrier deck operations), and `-elevatedplatformthreshold <cm>` (default `50`). See the [wiki](https://github.com/tuduce/JoinFS/wiki/Elevated-Platform-Landing) for details and examples. Also added an `onGround` field to the WebSocket telemetry feed (`-websocket`) for external tools consuming live aircraft state. To check whether elevated platform recognition engaged for a particular aircraft, enable the **Network** category in JoinFS's Monitor window - it logs the callsign, computed mismatch, configured threshold, and whether platform-trust engaged whenever the decision changes.
- **SimBrief button restyle.** The SimBrief button on the main screen is now a text button colored the same green/red (Active/Inactive) as the Simulator/Network buttons on a successful/failed fetch, instead of an icon with a small badge overlay. It's sized and aligned to match the Join Global button, and is hidden entirely until a SimBrief username is configured - the Flight Plan button expands to fill the freed space in that case.
- **Callsign editing consolidated to one place.** The old "Edit Callsign" aircraft-list context menu and its persistent per-installed-model override (which also silently applied to any locally-simulated AI traffic sharing that model, not just your own aircraft) have been removed. The Flight Plan dialog's callsign field is now the single, session-scoped place to set a callsign.
- **Unhandled exceptions are now logged before the process exits.** JoinFS previously had no handler for unhandled exceptions on any thread (including background scans), so a crash would exit with no trace at all. This doesn't fix any specific crash, but turns future silent closes into diagnosable ones.

## Bug Fixes

- Fixed the first-run setup dialog's tab order jumping to the OK button before the SimBrief username field, and the nickname field not being pre-filled (which could blank an already-valid nickname if the dialog was shown for another reason).
- Fixed `LIVERY FOLDER`-based config reading silently finding nothing for the many FS2024 add-ons that ship a single `aircraft.cfg` with no separate `livery.cfg` - these now resolve via a title-based lookup against your installed packages instead.
- Fixed a `base_container` resolution bug where a variation package's real config data could be discarded if that package had no `livery.cfg` of its own.
- Fixed the installed-package index used by the two fixes above missing every package when your configured simulator folder is the sim's base install directory rather than a `Community`-style folder directly - it now looks one level deeper to find the actual package folders.
- **Fixed real, reproducible misclassifications of installed FS2024 community liveries.** With no real config data available while building the installed-model catalog, JoinFS previously fell back to guessing the ICAO type purely from the title text - which could pick the wrong aircraft entirely: e.g. an Airbus A320 livery titled `..._Smart_Lynx` was tagged as a Rotorcraft, because "Lynx" (from the airline name "Smart Lynx") is also a real Doc8643 helicopter designator and won a tie-break against the correct "A320". JoinFS now tries reading the real `aircraft.cfg` from disk (following `base_container` references where needed) while cataloging, before ever falling back to guessing. Also fixed the underlying folder discovery to correctly walk a modern Community/Official package-based install instead of assuming a flat layout that doesn't exist there.
- Fixed a data error in the bundled model-to-typerole reference list: a real airship (Skyship 600) was tagged as a Rotorcraft.
- Manual "Scan For Models" no longer freezes the app while scanning - it now runs in the background like the automatic on-connect scan already did.
- **Fixed callsign duplication after upgrading from an older version.** The real-callsign synthesis (ICAO airline + flight number) concatenated the two unconditionally. `ATC FLIGHT NUMBER` was write-only/unread by JoinFS before this line of releases, so many add-ons/pilots had stored an entire pre-existing callsign there instead of a bare numeric flight number - once read, the airline code got glued onto it again (e.g. `DLH1234` becoming `DLHDLH1234`). Now trusted as-is whenever it already carries the airline prefix or isn't purely numeric.
- Fixed the Flight Plan dialog's callsign field being read-only - it's the one place to set/override a callsign now that the old context-menu override has been removed.
- Pressing Enter in the SimBrief username field now saves the username and runs the import before closing the dialog, instead of closing immediately without saving anything.
- Fixed the Flight Plan dialog's tab order (SimBrief username → Import → Clear → OK → Cancel), which had collided with the OK/Cancel buttons' tab indices.
- **Fixed a SimBrief-imported callsign being overwritten by a sim/livery-derived guess (e.g. `DLH1234` reverting to `Lufthansa 320`).** Whenever the user's aircraft object was (re-)listed by the sim, its callsign was unconditionally re-derived from the aircraft's `ATC AIRLINE`/`ATC FLIGHT NUMBER` (aircraft.cfg/livery.cfg or MSFS2024 aircraft customization data), clobbering any callsign already fetched from SimBrief - regardless of fetch order. The sim-derived callsign is now only used as a fallback when no callsign is already set, matching how ICAO type/airline are already handled in the same code path.

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
