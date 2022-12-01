using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

public static class UniqueIDGenerator
{

    //this is method is using GUID (Globally unique identifier)
    //will generate something like
    //8f5a13c1-6f41-4c4d-b716-e034e729ff7f
    public static string GenerateID_Guid()
    {
        Guid guid = Guid.NewGuid();
        string str = guid.ToString();
        return str;
    }

    //this method is using string builder
    //its generate youtube's uniqueID like. (e.g: P0wgNn2TbRC)
    public static string GenerateID_StringBuilder()
    {
        StringBuilder builder = new StringBuilder();
        Enumerable
            .Range(65, 26)
            .Select(e => ((char)e).ToString())
            .Concat(Enumerable.Range(97, 26).Select(e => ((char)e).ToString()))
            .Concat(Enumerable.Range(0, 10).Select(e => e.ToString()))
            .OrderBy(e => Guid.NewGuid())
            .Take(7)
            .ToList().ForEach(e => builder.Append(e));

        string id = builder.ToString();
        return id;
    }

    //this method is to generate the MD5 UniqueID
    //will generate something like
    //c20ad4d76fe97759aa27a0c99bff6710
    public static string GenerateID_MD5(string _str1, string _str2)
    {
        string strMD5 = _str1 + _str2;

        StringBuilder hash = new StringBuilder();
        MD5CryptoServiceProvider md5provider = new MD5CryptoServiceProvider();
        byte[] bytes = md5provider.ComputeHash(new UTF8Encoding().GetBytes(strMD5));

        for (int i = 0; i < bytes.Length; i++)
        {
            hash.Append(bytes[i].ToString("x2"));
        }
        return hash.ToString();

    }

}