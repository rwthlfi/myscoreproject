using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Management;

public static class GlobalSettings 
{
    public static string appVersion = "0.8";

    public static int fontHeader = 34; // not yet defined
    public static int fontDefault1 = 28;
    public static int fontDefault2 = 18;
    public static int fontCaption = 16; // not yet defined

    //Button
    //Width: 200
    //height: 50
    //text: fontDefault2

    //UDP Variable, due to fast sending.
    public static float waitSetup = 5f;

    //TagList
    public static string TagSwimable = "SwimableArea";

    //LayerList
    public static int layerDefault = 0;
    public static int layerTransparentFX = 1;
    public static int layerIgnoreRaycast = 2;
    public static int layerWater = 4;
    public static int layerUI = 5;

    public static int layerPlayer = 6;
    public static int layerGrabbable = 8;
    public static int layerDrawing_3D = 12;
    public static int layerFadeObject = 13;
    public static int layerFadeCamera = 14;
    public static int layerRealTime = 15;
    
    //IP address
    public static string serverIP = "134.130.88.14";

    //player Network name
    public static string playerPrefabName = "XR Rig Network - AvatarCreation(Clone)";
    public static string playerPrefabOfflineName = "XR Rig Offline - AvatarCreation";
    public static string playerHip = "HipSensor"; // needed for sitting


    //the scenarios name
    public const string MatrixScene = "MatrixScene";
    public const string AllScene = "AllScene";
    public const string WelcomeScene = "WelcomeScene";
    public const string KatschofScene = "Katschhof";
    public const string RolePlayScene = "RolePlayHall";
    public const string RektoratsScene = "Rektoratsraum";
    //public const string MobileFloodScene = "MobileFloodScene_URP";
    public const string Conference360Scene = "Conference360Scene";
    //public const string HochschuleMainzScene = "Hochschule_Mainz";
    public const string WZL_HallSceme = "WZLHall";
    public const string SocialWerk_Scene = "SozialWerk_Restaurant";
    public const string RaymondsLab_Scene = "RaymondsLab";
    public const string CARESScene = "CARESScene";
    public const string RiverBasinLandscapeScene = "RiverBasinLandscape";
    public const string RWTHAachenScene = "RWTHAachen";
    public const string PresentationHallScene = "PresentationHall";
    public const string WaterEnergyFood_WaterCascadeScene = "WaterEnergyFood_WaterCascade";
    public const string WaterEnergyFood_WaterDesalinationScene = "WaterEnergyFood_WaterDesalination";
    public const string MobileFloodUnitScene = "MobileFloodUnit";
    public const string GroupworkHallScene = "GruppenArbeit";

    // misc
    public static bool haptics = true;
    public static bool uIAudio = true;

    /// <summary>
    /// request the scene name based on the scene id.
    /// </summary>
    /// <param name="_sceneID"></param>
    /// <returns></returns>
    public static string requestedSceneName(int _sceneID)
    {
        switch (_sceneID)
        {
            case -2: return MatrixScene;
            case -1: return AllScene;
            case 0: return KatschofScene;
            case 1: return RolePlayScene;
            case 2: return RektoratsScene;
            case 3: return Conference360Scene;
            case 4: return WZL_HallSceme;
            case 5: return SocialWerk_Scene;
            case 6: return RaymondsLab_Scene;
            case 7: return CARESScene;
            case 8: return RiverBasinLandscapeScene;
            case 9: return RWTHAachenScene;
            case 10: return PresentationHallScene;
            case 11: return WaterEnergyFood_WaterCascadeScene;
            case 12: return WaterEnergyFood_WaterDesalinationScene;
            case 13: return MobileFloodUnitScene;
            case 14: return GroupworkHallScene;

            default: return MatrixScene;
        }
    }

    public enum Device
    {
        WindowsVR,
        WindowsNonVR,
        Android,
        Unknown
    }

    public static Device DeviceType()
    {
        if (Application.platform == RuntimePlatform.WindowsPlayer
            || Application.platform == RuntimePlatform.WindowsEditor)
        {
            // REturn that the current app is not using the any VR
            if (XRGeneralSettings.Instance.Manager.activeLoader != null)
                return Device.WindowsVR;
            else
                return Device.WindowsNonVR;
        }

        //Should be oculus
        else if (Application.platform == RuntimePlatform.Android)
        {
            return Device.Android;
        }

        else
        {
            return Device.Unknown;
        }
            
    }

    public enum PlayerRole
    {
        normal,
        admin,
        lecturer //-> do we need this ?
    }

    //Extra attribute
    public const string att_Incognito = "Incognito";
    public const string att_Mute = "Mute";
    public const string att_Streamer = "Streamer";



    public enum StreamStatus
    {
        notStreaming,
        isStreaming
    }

    public const float speakRadiusDefault = 10f;
    public const float speakRadiusEveryone = 20f;


    //might not be needed anymore.
    public enum SpeakingStatus
    {
        speaking,
        mute
    }


    //Net object attribute
    public const string netObj_att_useGravity = "UseGravity;";
    public const string netObj_att_isKinematic = "IsKinematic;";
    public const string netObj_att_isWandering = "IsWandering;";


    //to check if the application is using the handtracking in the oculus
    public static bool handTrackingActive()
    {
        if (Application.platform == RuntimePlatform.Android
            || Application.platform == RuntimePlatform.WindowsEditor)
        {
            if (OVRPlugin.GetHandTrackingEnabled())
                return true;
            else
                return false;
        }

        else
            return false;
    }

    public const string UnityEngineMeshString = " (UnityEngine.Mesh)";
    public const string string_custome_TShirt = "TShirt";


    /// <summary>
    /// this is to convert the given scale to the right height
    /// </summary>
    /// <returns></returns>
    public static float heightScaleConverter(float _scale)
    {
        //note: taken in the real life measurement
        /*
         scale 0.9 -> height: 150-155
         scale 1 -> height: 170-175
         scale 1.15 -> height: 195-200
        the minus in 170-175 is approximation -> currently i take the middle value which is 172
        */
        //with linear interpolation we get this formula


        //return (167f * _scale) + 5f;
        return (170f * _scale) + 5f;


    }


    public static string formatNone = "None";
    public static string formatVideo = ".mp4";
    public static string formatImage = ".jpg";
}
