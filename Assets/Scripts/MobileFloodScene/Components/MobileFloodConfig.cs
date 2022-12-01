using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class MobileFloodConfig
{
    //glass cube things
    public static float glassCubeOpenY_pos = -1.55f;
    public static float glassCubeCloseY_pos = 1.55f;
    public static float glassCubeTolerance = 0.05f;


    //screwing component
    public static float screwingSpeed = 500f;
    public static float maxScrewTolerance = 0.01f;
    public static float maxScrewDepth = 0.025f; // the max. depth of the screw that can be screwed in
    
    public static float maxPlugOut = 0.025f; // the max. depth of the screw that can be screwed out

    public static float oiledDuration = 5f; 

    //this is the maximum falling speed before reseting the position
    public static float maxFallingSpeed = 1f;

    //the height on which the y position will be reseted
    public static float resetYpos = 0.5f;

    //oiled color
    public static Color colorOiled = Color.black;
    public static Color colorNotOiled = Color.white;
    public static float colorTolerance = 0.07f;

    //the min. staying duration of the triggered object until its being acknowledge
    public static float minStayDuration = 3.5f;

    //locking device things
    public static float lockMainScrewDepth = 0.12f;   // Y pos ori: 0,169
    public static float lockScrewDepth = -0.0115f; // X pos ori:-0,0187


    //Components name
    public static string SupportColumn = "SupportColumn";
    public static string TopScrew = "TopScrew";
    public static string Plug = "Plug";
    public static string screw = "Screw";
    public static string drillScrewPoint = "DrillScrewPoint";
}
