
public static class SQLChecker
{
    public static string isError(string _str)
    {
        if (_str.Contains("Warning:") && _str.Contains("Access denied"))
            return "Access Denied: possibly because of wrong pass";


        if (_str.Contains("Notice:") && _str.Contains("Undefined index"))
            return "Access Denied: possibly because of some variable is not defined. Please cross-Check it with the php script";


        else if (_str.Contains("InsertError"))
            return "Something is wrong during inserting. Possibly because of the weak internet conn";
        
        else if (_str.Contains("UpdateError"))
            return "Something is wrong during updating. Possibly because of the weak internet conn";

        else if (_str.Contains("RemoveError"))
            return "Something is wrong during removing. Possibly because of the weak internet conn";


        else if (_str == "")
        {
            return "no data found";
        }

        //No error at all, dont return anything...
        return null;
    }

    
}
