using UnityEditor;

public class MyScoreTestServerBuild_Windows
{
    public static string ScenesRoot = "Assets/Scenes/";
    public static string ServerRoot = "Servers/TestServers/";
    private static readonly BuildTarget buildTarget = BuildTarget.StandaloneWindows64;
    private static readonly BuildOptions buildOptions = BuildOptions.EnableHeadlessMode;

    //public static readonly BuildOptions buildOptions = BuildOptions.None;
    
    public static string PrevPath = null;
    public static string buildPath = "Builds/Tests_7000/";

    [MenuItem("Tools/Build Test MyScoreServer Windows/Build All", false, 0)]
    public static void BuildAllMenu()
    {
        string path = GetPath();
        if (!string.IsNullOrEmpty(path))
        {
            BuildMasterServer(path);
            BuildRemoteSpawner(path);
            BuildGameServer(path);
            //BuildPlayerClient(path);
        }
    }

    [MenuItem("Tools/Build Test MyScoreServer Windows/MasterServer", false, 100)]
    public static void BuildMasterServerMenu()
    {
        string path = GetPath();
        if (!string.IsNullOrEmpty(path))
        {
            BuildMasterServer(path);
        }
    }

    [MenuItem("Tools/Build Test MyScoreServer Windows/RemoteSpawner", false, 101)]
    public static void BuildRemoteSpawnerMenu()
    {
        string path = GetPath();
        if (!string.IsNullOrEmpty(path))
        {
            BuildRemoteSpawner(path);
        }
    }

    [MenuItem("Tools/Build Test MyScoreServer Windows/GameServer", false, 102)]
    public static void BuildGameServerMenu()
    {
        string path = GetPath();
        if (!string.IsNullOrEmpty(path))
        {
            BuildGameServer(path);
        }
    }

    [MenuItem("Tools/Build Test MyScoreServer Windows/PlayerClient", false, 103)]
    public static void BuildPlayerClientMenu()
    {
        string path = GetPath();
        if (!string.IsNullOrEmpty(path))
        {
            BuildPlayerClient(path);
        }
    }

    public static void BuildMasterServer(string path)
    {
        string[] scenes = new[] { ScenesRoot + ServerRoot + "MasterServer.unity" };
        /*
        PlayerSettings.productName = "MasterServer";
        BuildPipeline.BuildPlayer(scenes, path + "/MasterServer.exe", GetBuildTarget(), buildOptions);
        */

        BuildPlayerOptions buildPlayerOptions = new BuildPlayerOptions();
        buildPlayerOptions.scenes = scenes;
        buildPlayerOptions.locationPathName = buildPath + "MasterServer.exe";
        buildPlayerOptions.target = buildTarget;
        buildPlayerOptions.options = buildOptions;
        BuildPipeline.BuildPlayer(buildPlayerOptions);

    }

    public static void BuildRemoteSpawner(string path)
    {
        string[] gameServerScenes = new[] { ScenesRoot + ServerRoot + "RemoteSpawner.unity" };

        /*
        PlayerSettings.productName = "RemoteSpawner";
        BuildPipeline.BuildPlayer(gameServerScenes, path + "/RemoteSpawner.exe", GetBuildTarget(), buildOptions);
        */

        BuildPlayerOptions buildPlayerOptions = new BuildPlayerOptions();
        buildPlayerOptions.scenes = gameServerScenes;
        buildPlayerOptions.locationPathName = buildPath + "RemoteSpawner.exe";
        buildPlayerOptions.target = buildTarget;
        buildPlayerOptions.options = buildOptions;
        BuildPipeline.BuildPlayer(buildPlayerOptions);


    }

    public static void BuildGameServer(string path)
    {
        string[] gameServerScenes = new[]
        {
            ScenesRoot + ServerRoot + "GameServer.unity",
            //Scene used for MasterServer Demo
            
            
            ScenesRoot+"Katschhof.unity",
            ScenesRoot+"RolePlayHall.unity",
            ScenesRoot+"Rektoratsraum.unity",
            ScenesRoot+"MatrixScene.unity",
            ScenesRoot+"Conference360Scene.unity",
            ScenesRoot+"WZLHall.unity",
            ScenesRoot+"SozialWerk_Restaurant.unity",
            ScenesRoot+"RaymondsLab.unity",
            ScenesRoot+"CARESScene.unity",
            ScenesRoot+"RiverBasinLandscape.unity",
            ScenesRoot+"RWTHAachen.unity",
            ScenesRoot+"PresentationHall.unity",
            ScenesRoot+"WaterEnergyFood_WaterCascade.unity",
            ScenesRoot+"WaterEnergyFood_WaterDesalination.unity",
            ScenesRoot+"MobileFloodUnit.unity",
            ScenesRoot+"GruppenArbeit.unity"
        };

        /*
        PlayerSettings.productName = "GameServer";
        BuildPipeline.BuildPlayer(gameServerScenes, path + "/GameServer.exe", GetBuildTarget(), buildOptions);
        */

        //PlayerSettings.productName = "GameServer";
        BuildPlayerOptions buildPlayerOptions = new BuildPlayerOptions();
        buildPlayerOptions.scenes = gameServerScenes;
        buildPlayerOptions.locationPathName = buildPath + "GameServer.exe";
        buildPlayerOptions.target = buildTarget;
        buildPlayerOptions.options = buildOptions;
        BuildPipeline.BuildPlayer(buildPlayerOptions);
    }

    public static void BuildPlayerClient(string path)
    {
        string[] scenes = new[]
        {
            ScenesRoot + "WelcomeScene.unity",
            //Scene used for MasterServer Demo
            
            
            ScenesRoot+"Katschhof.unity",
            ScenesRoot+"RolePlayHall.unity",
            ScenesRoot+"Rektoratsraum.unity",
            ScenesRoot+"MatrixScene.unity",
            ScenesRoot+"Conference360Scene.unity",
            ScenesRoot+"WZLHall.unity",
            ScenesRoot+"SozialWerk_Restaurant.unity",
            ScenesRoot+"RaymondsLab.unity",
            ScenesRoot+"CARESScene.unity",
            ScenesRoot+"RiverBasinLandscape.unity",
            ScenesRoot+"RWTHAachen.unity",
            ScenesRoot+"PresentationHall.unity",
            ScenesRoot+"WaterEnergyFood_WaterCascade.unity",
            ScenesRoot+"WaterEnergyFood_WaterDesalination.unity",
            ScenesRoot+"MobileFloodUnit.unity",
            ScenesRoot+"GruppenArbeit.unity"
        };

        /*
        PlayerSettings.productName = "PlayerClient";
        BuildPipeline.BuildPlayer(scenes, path + "/PlayerClient.exe", GetBuildTarget(), buildOptions);
        */

        BuildPlayerOptions buildPlayerOptions = new BuildPlayerOptions();
        buildPlayerOptions.scenes = scenes;
        buildPlayerOptions.locationPathName = buildPath + "PlayerClient.exe";
        buildPlayerOptions.target = buildTarget;
        buildPlayerOptions.options = BuildOptions.None;
        BuildPipeline.BuildPlayer(buildPlayerOptions);
    }

    #region Helpers
    public static string GetPath()
    {
        string prevPath = EditorPrefs.GetString("msf.buildPath", "");
        string path = EditorUtility.SaveFolderPanel("Choose Location for binaries", prevPath, "");

        if (!string.IsNullOrEmpty(path))
        {
            EditorPrefs.SetString("msf.buildPath", path);
        }
        return path;
    }

    public static BuildTarget GetBuildTarget()
    {
        return EditorUserBuildSettings.activeBuildTarget;
    }
    #endregion
}