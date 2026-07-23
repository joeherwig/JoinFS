namespace JoinFS.Tests;

/// <summary>
/// Exercises SimPathDetector's UserCfg.opt parsing and folder-validation helpers
/// directly (they're internal, exposed via InternalsVisibleTo) using temp files/folders,
/// so tests never touch a real simulator install or %APPDATA%.
/// </summary>
public class SimPathDetectorTests : IDisposable
{
    readonly string tempDir;

    public SimPathDetectorTests()
    {
        tempDir = Path.Combine(Path.GetTempPath(), "JoinFSTests_" + Guid.NewGuid());
        Directory.CreateDirectory(tempDir);
    }

    public void Dispose()
    {
        if (Directory.Exists(tempDir))
        {
            Directory.Delete(tempDir, true);
        }
    }

    [Fact]
    public void ReadInstalledPackagesPath_MissingFile_ReturnsNull()
    {
        string missingFile = Path.Combine(tempDir, "does-not-exist", "UserCfg.opt");

        string? result = SimPathDetector.ReadInstalledPackagesPath(missingFile);

        Assert.Null(result);
    }

    [Fact]
    public void ReadInstalledPackagesPath_ParsesQuotedValue()
    {
        string userCfg = Path.Combine(tempDir, "UserCfg.opt");
        File.WriteAllLines(userCfg,
        [
            "// comment line",
            "MainMenuMusic 0",
            "InstalledPackagesPath \"D:\\MSFS2024\\Packages\"",
            "OtherSetting 1",
        ]);

        string? result = SimPathDetector.ReadInstalledPackagesPath(userCfg);

        Assert.Equal(@"D:\MSFS2024\Packages", result);
    }

    [Fact]
    public void ReadInstalledPackagesPath_NoMatchingLine_ReturnsNull()
    {
        string userCfg = Path.Combine(tempDir, "UserCfg.opt");
        File.WriteAllLines(userCfg, ["MainMenuMusic 0", "OtherSetting 1"]);

        string? result = SimPathDetector.ReadInstalledPackagesPath(userCfg);

        Assert.Null(result);
    }

    [Fact]
    public void IsValidMsfsPackagesFolder_WithOfficialSubfolder_ReturnsTrue()
    {
        Directory.CreateDirectory(Path.Combine(tempDir, "Official"));

        Assert.True(SimPathDetector.IsValidMsfsPackagesFolder(tempDir));
    }

    [Fact]
    public void IsValidMsfsPackagesFolder_WithCommunitySubfolder_ReturnsTrue()
    {
        Directory.CreateDirectory(Path.Combine(tempDir, "Community"));

        Assert.True(SimPathDetector.IsValidMsfsPackagesFolder(tempDir));
    }

    [Fact]
    public void IsValidMsfsPackagesFolder_WithNeitherSubfolder_ReturnsFalse()
    {
        Assert.False(SimPathDetector.IsValidMsfsPackagesFolder(tempDir));
    }

    [Fact]
    public void IsValidMsfsPackagesFolder_WithYearSuffixedSubfolders_ReturnsTrue()
    {
        // real-world case: a Packages folder shared between MSFS2020 and MSFS2024
        // (common with the MS Store/Xbox versions) uses "Official2024"/"Community2024"
        // and "Official2020"/"Community" instead of the plain exact names
        Directory.CreateDirectory(Path.Combine(tempDir, "Official2020"));
        Directory.CreateDirectory(Path.Combine(tempDir, "Official2024"));
        Directory.CreateDirectory(Path.Combine(tempDir, "Community"));
        Directory.CreateDirectory(Path.Combine(tempDir, "Community2024"));

        Assert.True(SimPathDetector.IsValidMsfsPackagesFolder(tempDir));
    }

    [Fact]
    public void IsValidMsfsPackagesFolder_PathDoesNotExist_ReturnsFalse()
    {
        string missingPath = Path.Combine(tempDir, "does-not-exist");

        Assert.False(SimPathDetector.IsValidMsfsPackagesFolder(missingPath));
    }

    [Fact]
    public void TryDetectMsfsPackagesPath_PrefersFirstValidCandidate()
    {
        string steamCfg = Path.Combine(tempDir, "steam", "UserCfg.opt");
        Directory.CreateDirectory(Path.GetDirectoryName(steamCfg)!);
        string steamPackages = Path.Combine(tempDir, "steam-packages");
        Directory.CreateDirectory(Path.Combine(steamPackages, "Official"));
        File.WriteAllLines(steamCfg, [$"InstalledPackagesPath \"{steamPackages}\""]);

        string storeCfg = Path.Combine(tempDir, "store", "UserCfg.opt");
        Directory.CreateDirectory(Path.GetDirectoryName(storeCfg)!);
        string storePackages = Path.Combine(tempDir, "store-packages");
        Directory.CreateDirectory(Path.Combine(storePackages, "Official"));
        File.WriteAllLines(storeCfg, [$"InstalledPackagesPath \"{storePackages}\""]);

        string? result = SimPathDetector.TryDetectMsfsPackagesPath(steamCfg, storeCfg);

        Assert.Equal(steamPackages, result);
    }

    [Fact]
    public void TryDetectMsfsPackagesPath_FallsBackToSecondCandidate()
    {
        string steamCfg = Path.Combine(tempDir, "steam", "UserCfg.opt"); // never created
        string storeCfg = Path.Combine(tempDir, "store", "UserCfg.opt");
        Directory.CreateDirectory(Path.GetDirectoryName(storeCfg)!);
        string storePackages = Path.Combine(tempDir, "store-packages");
        Directory.CreateDirectory(Path.Combine(storePackages, "Community"));
        File.WriteAllLines(storeCfg, [$"InstalledPackagesPath \"{storePackages}\""]);

        string? result = SimPathDetector.TryDetectMsfsPackagesPath(steamCfg, storeCfg);

        Assert.Equal(storePackages, result);
    }

    [Fact]
    public void TryDetectMsfsPackagesPath_NeitherCandidateValid_ReturnsNull()
    {
        string steamCfg = Path.Combine(tempDir, "steam", "UserCfg.opt");
        string storeCfg = Path.Combine(tempDir, "store", "UserCfg.opt");

        string? result = SimPathDetector.TryDetectMsfsPackagesPath(steamCfg, storeCfg);

        Assert.Null(result);
    }
}
