using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Security.Cryptography.X509Certificates;
using System.Diagnostics;
using System.Security.Cryptography;
using System.Text;
using Org.BouncyCastle.Security;
using Org.BouncyCastle.Cms;
using System.Collections;
using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Crypto.Engines;
using Org.BouncyCastle.Crypto.Parameters;

/// <summary>
/// Utility class for signing and verifying Point E-Commerce signatures.
/// </summary>
public class PointSignatureUtil
{

    /// <summary>
    /// Formats payment parameters single content string for signature generation.
    /// </summary>
    /// <param name="parameters">the payment parameters</param>
    /// <returns>the content string</returns>
    public static String FormatParameters(IDictionary<String, String> parameters)
    {
        String[] keys = new String[parameters.Count];
        int index = 0;
        foreach (KeyValuePair<string, string> kvp in parameters)
        {
            keys[index] = kvp.Key;
            index++;
        }

        Array.Sort(keys, new PointStringComparer());

        StringBuilder builder = new StringBuilder();
        for (int i = 0; i < keys.Length; i++)
        {
            builder.Append(keys[i] + "=" + parameters[keys[i]].Replace(";",";;") + ";");
        }
        return builder.ToString();
    }

    /// <summary>
    /// Creates payment token.
    /// </summary>
    /// <param name="parameters">the payment parameters</param>
    /// <returns>the content string</returns>
    public static String CreatePaymentToken(IDictionary<String, String> parameters)
    {
        String tokenContent =
                parameters["s-f-1-36_merchant-agreement-code"] + ";"
                + parameters["s-f-1-36_order-number"] + ";"
                + parameters["t-f-14-19_payment-timestamp"];
        byte[] tokenBytes = Encoding.UTF8.GetBytes(tokenContent);
        using (SHA256 sha256Hash = SHA256.Create())
        {
            byte[] hash = sha256Hash.ComputeHash(tokenBytes);
            return ByteArrayToHexString(hash).Substring(0, 32);
        }
    }

    /// <summary>
    /// Signs content.
    /// </summary>
    /// <param name="certificate"></param>
    /// <param name="content"></param>
    /// <param name="hashAlgorithm"></param>
    /// <returns>the content signature</returns>
    public static String CreateSignature(X509Certificate2 certificate, String content, HashAlgorithm hashAlgorithm)
    {
        RSACryptoServiceProvider RSA = (RSACryptoServiceProvider)certificate.PrivateKey;
        AsymmetricCipherKeyPair keyPair = DotNetUtilities.GetRsaKeyPair(RSA);
        byte[] contentBytes = Encoding.UTF8.GetBytes(content);

        String algorithm = null;
        if (hashAlgorithm == HashAlgorithm.SHA1)
        {
            algorithm = "SHA1withRSA";
        }
        else
        {
            algorithm = "SHA512withRSA";
        }
        ISigner signer = SignerUtilities.GetSigner(algorithm);

        signer.Init(true, keyPair.Private);

        signer.BlockUpdate(contentBytes, 0, contentBytes.Length);
        byte[] signatureBytes = signer.GenerateSignature();

        Debug.WriteLine("DATA:" + ByteArrayToHexString(contentBytes));
        Debug.WriteLine("SIGN:" + ByteArrayToHexString(signatureBytes));

        return ByteArrayToHexString(signatureBytes);
        /*
        RSACryptoServiceProvider RSA = (RSACryptoServiceProvider) certificate.PrivateKey;
        AsymmetricCipherKeyPair keyPair = DotNetUtilities.GetRsaKeyPair(RSA);

        IBufferedCipher cipher = CipherUtilities.GetCipher("RSA/ECB/PKCS1Padding");
        cipher.Init(true, keyPair.Private);

        byte[] contentBytes = Encoding.UTF8.GetBytes(content);
        byte[] hashedBytes = hashAlgorithm == HashAlgorithm.SHA1 ? Sha1(contentBytes) : Sha512(contentBytes);
        byte[] cipheredBytes = cipher.DoFinal(hashedBytes, 0, hashedBytes.Length);

        Debug.WriteLine("DATA:" + ByteArrayToHexString(contentBytes));
        Debug.WriteLine("HASH:" + ByteArrayToHexString(hashedBytes));
        Debug.WriteLine("SIGN:" + ByteArrayToHexString(cipheredBytes));

        return ByteArrayToHexString(cipheredBytes);
        */
    }

    /// <summary>
    /// Verifies content signature.
    /// </summary>
    /// <param name="certificate"></param>
    /// <param name="signature"></param>
    /// <param name="content"></param>
    /// <param name="hashAlgorithm"></param>
    /// <returns>true if signature was succesfully verified</returns>
    public static bool VerifySignature(X509Certificate2 certificate, String signature, String content, HashAlgorithm hashAlgorithm)
    {
        byte[] contentBytes = Encoding.UTF8.GetBytes(content);
        byte[] signatureBytes = HexStringToByteArray(signature);

        RSACryptoServiceProvider RSA = (RSACryptoServiceProvider)certificate.PublicKey.Key;
        RsaKeyParameters publicKey = DotNetUtilities.GetRsaPublicKey(RSA);

        String algorithm = null;
        if (hashAlgorithm == HashAlgorithm.SHA1)
        {
            algorithm = "SHA1withRSA";
        }
        else
        {
            algorithm = "SHA512withRSA";
        }
        ISigner signer = SignerUtilities.GetSigner(algorithm);

        signer.Init(false, publicKey);

        signer.BlockUpdate(contentBytes, 0, contentBytes.Length);

        Debug.WriteLine("DATA:" + ByteArrayToHexString(contentBytes));
        Debug.WriteLine("SIGN:" + ByteArrayToHexString(signatureBytes));

        return signer.VerifySignature(signatureBytes);

        /*RSACryptoServiceProvider RSA = (RSACryptoServiceProvider)certificate.PublicKey.Key;
        RsaKeyParameters publicKey = DotNetUtilities.GetRsaPublicKey(RSA);

        IBufferedCipher cipher = CipherUtilities.GetCipher("RSA/ECB/PKCS1Padding");
        cipher.Init(false, publicKey);

        byte[] contentBytes = Encoding.UTF8.GetBytes(content);
        byte[] signatureBytes = HexStringToByteArray(signature);
        byte[] candidateHashBytes = hashAlgorithm == HashAlgorithm.SHA1 ? Sha1(contentBytes) : Sha512(contentBytes);
        byte[] hashBytes = cipher.DoFinal(signatureBytes, 0, signatureBytes.Length);

        Debug.WriteLine("DATA:" + ByteArrayToHexString(contentBytes));
        Debug.WriteLine("SIGN:" + ByteArrayToHexString(signatureBytes));
        Debug.WriteLine("HASH1:" + ByteArrayToHexString(hashBytes));
        Debug.WriteLine("HASH2:" + ByteArrayToHexString(candidateHashBytes));

        if (candidateHashBytes.Length != hashBytes.Length)
        {
            return false;
        }
        for (int i = 0; i < candidateHashBytes.Length; i++)
        {
            if (candidateHashBytes[i] != hashBytes[i])
            {
                return false;
            }
        }

        return true;*/
    }

    /// <summary>
    /// Converts byte array to hexadecimal string.
    /// </summary>
    /// <param name="ba"></param>
    /// <returns></returns>
    public static string ByteArrayToHexString(byte[] ba)
    {
        StringBuilder hex = new StringBuilder(ba.Length * 2);
        foreach (byte b in ba)
            hex.AppendFormat("{0:X2}", b);
        return hex.ToString();
    }

    /// <summary>
    /// Converts hexadecimal string to byte array.
    /// </summary>
    /// <param name="hex"></param>
    /// <returns></returns>
    public static byte[] HexStringToByteArray(String hex)
    {
        int NumberChars = hex.Length;
        byte[] bytes = new byte[NumberChars / 2];
        for (int i = 0; i < NumberChars; i += 2)
            bytes[i / 2] = Convert.ToByte(hex.Substring(i, 2), 16);
        return bytes;
    }

    /// <summary>
    /// Calculates SHA1 hash.
    /// </summary>
    /// <param name="data"></param>
    /// <returns></returns>
    private static byte[] Sha1(byte[] data)
    {
        SHA1 sha = new SHA1CryptoServiceProvider();
        return sha.ComputeHash(data);
    }

    /// <summary>
    /// Calculates SHA512 hash.
    /// </summary>
    /// <param name="data"></param>
    /// <returns></returns>
    private static byte[] Sha512(byte[] data)
    {
        SHA512 sha = new SHA512CryptoServiceProvider();
        return sha.ComputeHash(data);
    }

}