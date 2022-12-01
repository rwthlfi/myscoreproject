using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class SQLurlConfig 
{
    //public const string header = "http://localhost";
    //public const string header = "http://134.130.88.14";
    public const string header = "http://myscore-webapp.lfi.rwth-aachen.de";
    public const string database = "/myscore/";

    public const string password = "l2AV43gop5JwG9tAAsdl";

    //Table list
    public const string tableAvatar = "avatartable";
    public const string tableLogo = "logotable";
    public const string tablePic360 = "pic360table";
    public const string tablePlayerProfile = "playerProfile";
    public const string tableVideo360 = "video360Table";

    //Posting list
    public const string dbPass_Post = "dbPass_Post";
    public const string mediaTable_post = "mediaTable_Post";
    public const string uniqueID_Post = "uniqueID_Post";


    //the Player Manager - PHP
    public const string playerLoadAllPHP = header + database + "playerLoadAll.php";
    public const string playerLoadSpecificPHP = header + database + "playerLoadSpecific.php";
    public const string playerInserterPHP = header + database + "playerInserter.php";
    public const string playerUpdaterPHP = header + database + "playerUpdater.php";
    public const string playerRemoverPHP = header + database + "playerRemover.php";

    //the media (logo, pic360, video360) - PHP
    public const string mediaLoadAllPHP = header + database + "mediaLoadAll.php";
    public const string mediaLoadAllLinksPHP = header + database + "mediaLoadAllLinks.php";
    public const string mediaLoadThumbnailSpecificPHP = header + database + "mediaLoadThumbnailSpecific.php";
    public const string mediaLoadSpecificPHP = header + database + "mediaLoadSpecific.php";
    //The table name for media
    

    // add the new php below here.

}