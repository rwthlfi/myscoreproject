using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

//this encryption is the one we use for encrypting a password.
public class EncryptionUtil
{
    //private static string KEY = "kPSwFT2IILOOzUOB6NpGgxwDyrBWjklO"; // pick some other 32 chars
    private static string KEY = "kjdfaslla3FOP89c42U834iuh40218f4"; // pick some other 32 chars
    private static byte[] KEY_BYTES = Encoding.UTF8.GetBytes(KEY);


    public static string Encrypt(string plainText)
    {
        // Check arguments.
        if (plainText == null || plainText.Length <= 0)
            throw new ArgumentNullException("plainText");

        byte[] encrypted;
        // Create an AesManaged object
        // with the specified key and IV.
        using (Rijndael algorithm = Rijndael.Create())
        {
            algorithm.Key = KEY_BYTES;

            // Create a decrytor to perform the stream transform.
            var encryptor = algorithm.CreateEncryptor(algorithm.Key, algorithm.IV);

            // Create the streams used for encryption.
            using (var msEncrypt = new MemoryStream())
            {
                using (var csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                {
                    using (var swEncrypt = new StreamWriter(csEncrypt))
                    {
                        // Write IV first
                        msEncrypt.Write(algorithm.IV, 0, algorithm.IV.Length);
                        //Write all data to the stream.
                        swEncrypt.Write(plainText);
                    }
                    encrypted = msEncrypt.ToArray();
                }
            }
        }

        // Return the encrypted bytes from the memory stream.
        return Convert.ToBase64String(encrypted);
    }

    public static string Decrypt(string cipherText)
    {
        // Check arguments.
        if (cipherText == null || cipherText.Length <= 0)
            throw new ArgumentNullException("cipherText");

        // Declare the string used to hold
        // the decrypted text.
        string plaintext = null;

        // Create an AesManaged object
        // with the specified key and IV.
        using (Rijndael algorithm = Rijndael.Create())
        {
            algorithm.Key = KEY_BYTES;

            // Get bytes from input string
            byte[] cipherBytes = Convert.FromBase64String(cipherText);

            // Create the streams used for decryption.
            using (MemoryStream msDecrypt = new MemoryStream(cipherBytes))
            {
                // Read IV first
                byte[] IV = new byte[16];
                msDecrypt.Read(IV, 0, IV.Length);

                // Assign IV to an algorithm
                algorithm.IV = IV;

                // Create a decrytor to perform the stream transform.
                var decryptor = algorithm.CreateDecryptor(algorithm.Key, algorithm.IV);

                using (var csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
                {
                    using (var srDecrypt = new StreamReader(csDecrypt))
                    {
                        // Read the decrypted bytes from the decrypting stream
                        // and place them in a string.
                        plaintext = srDecrypt.ReadToEnd();
                    }
                }
            }
        }
        return plaintext;
    }
}