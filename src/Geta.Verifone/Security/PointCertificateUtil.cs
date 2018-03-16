using System;
using System.Configuration;
using System.IO;
using System.Security;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Web;
using System.Web.Hosting;

namespace Geta.Verifone.Security
{
    /// <summary>
    /// Summary description for PointCertificateUtil
    /// </summary>
    public class PointCertificateUtil
    {
        public static X509Certificate2 GetMerchantCertificate()
        {
            return GetCertificate("Verifone:MerchantCertificatePath", "Verifone:MerchantCertificatePassword");
        }

        public static X509Certificate2 GetPointCertificate()
        {
            return GetCertificate("Verifone:PointCertificatePath", "Verifone:PointCertificatePassword");
        }

        private static X509Certificate2 GetCertificate(string certName, string passwordSettingKey)
        {
            string certFilePath = ConfigurationManager.AppSettings[certName];

            if (string.IsNullOrWhiteSpace(certFilePath))
            {
                throw new ConfigurationErrorsException(string.Format("{0} is missing.", certName));
            }

            string password = ConfigurationManager.AppSettings[passwordSettingKey];

            if (string.IsNullOrWhiteSpace(password))
            {
                throw new ConfigurationErrorsException(string.Format("{0} is missing.", passwordSettingKey));
            }

            return new X509Certificate2(certFilePath, password, X509KeyStorageFlags.Exportable);
        }

        public static RSA GetPointPublicKey()
        {
            string publicKeyFilePath = ConfigurationManager.AppSettings["Verifone:PublicKeyPath"];

            return DecodeX509PublicKey(GetPublicKeyFromFile(publicKeyFilePath));
        }

        private static byte[] GetPublicKeyFromFile(string filePath)
        {
            string key = File.ReadAllText(filePath)
                    .Replace("-----BEGIN PUBLIC KEY-----", "")
                    .Replace("-----END PUBLIC KEY-----", "");

            return Convert.FromBase64String(key);
        }

        private static RSACryptoServiceProvider DecodeX509PublicKey(byte[] x509Key)
        {
            // encoded OID sequence for  PKCS #1 rsaEncryption szOID_RSA_RSA = "1.2.840.113549.1.1.1"
            byte[] seqOid = { 0x30, 0x0D, 0x06, 0x09, 0x2A, 0x86, 0x48, 0x86, 0xF7, 0x0D, 0x01, 0x01, 0x01, 0x05, 0x00 };
            byte[] seq = new byte[15];
            // ---------  Set up stream to read the asn.1 encoded SubjectPublicKeyInfo blob  ------
            MemoryStream mem = new MemoryStream(x509Key);
            BinaryReader binr = new BinaryReader(mem);    //wrap Memory Stream with BinaryReader for easy reading
            byte bt = 0;
            ushort twobytes = 0;

            try
            {

                twobytes = binr.ReadUInt16();
                if (twobytes == 0x8130) //data read as little endian order (actual data order for Sequence is 30 81)
                    binr.ReadByte();    //advance 1 byte
                else if (twobytes == 0x8230)
                    binr.ReadInt16();   //advance 2 bytes
                else
                    return null;

                seq = binr.ReadBytes(15);       //read the Sequence OID
                if (!CompareBytearrays(seq, seqOid))    //make sure Sequence for OID is correct
                    return null;

                twobytes = binr.ReadUInt16();
                if (twobytes == 0x8103) //data read as little endian order (actual data order for Bit String is 03 81)
                    binr.ReadByte();    //advance 1 byte
                else if (twobytes == 0x8203)
                    binr.ReadInt16();   //advance 2 bytes
                else
                    return null;

                bt = binr.ReadByte();
                if (bt != 0x00)     //expect null byte next
                    return null;

                twobytes = binr.ReadUInt16();
                if (twobytes == 0x8130) //data read as little endian order (actual data order for Sequence is 30 81)
                    binr.ReadByte();    //advance 1 byte
                else if (twobytes == 0x8230)
                    binr.ReadInt16();   //advance 2 bytes
                else
                    return null;

                twobytes = binr.ReadUInt16();
                byte lowbyte = 0x00;
                byte highbyte = 0x00;

                if (twobytes == 0x8102) //data read as little endian order (actual data order for Integer is 02 81)
                    lowbyte = binr.ReadByte();  // read next bytes which is bytes in modulus
                else if (twobytes == 0x8202)
                {
                    highbyte = binr.ReadByte(); //advance 2 bytes
                    lowbyte = binr.ReadByte();
                }
                else
                    return null;
                byte[] modint = { lowbyte, highbyte, 0x00, 0x00 };   //reverse byte order since asn.1 key uses big endian order
                int modsize = BitConverter.ToInt32(modint, 0);

                byte firstbyte = binr.ReadByte();
                binr.BaseStream.Seek(-1, SeekOrigin.Current);

                if (firstbyte == 0x00)
                {   //if first byte (highest order) of modulus is zero, don't include it
                    binr.ReadByte();    //skip this null byte
                    modsize -= 1;   //reduce modulus buffer size by 1
                }

                byte[] modulus = binr.ReadBytes(modsize);   //read the modulus bytes

                if (binr.ReadByte() != 0x02)            //expect an Integer for the exponent data
                    return null;
                int expbytes = (int)binr.ReadByte();        // should only need one byte for actual exponent data (for all useful values)
                byte[] exponent = binr.ReadBytes(expbytes);

                // ------- create RSACryptoServiceProvider instance and initialize with public key -----
                RSACryptoServiceProvider rsa = new RSACryptoServiceProvider();
                RSAParameters rsaKeyInfo = new RSAParameters
                {
                    Modulus = modulus,
                    Exponent = exponent
                };
                rsa.ImportParameters(rsaKeyInfo);
                return rsa;
            }
            catch (Exception)
            {
                return null;
            }

            finally { binr.Close(); }
        }

        private static bool CompareBytearrays(byte[] a, byte[] b)
        {
            if (a.Length != b.Length)
                return false;
            int i = 0;
            foreach (byte c in a)
            {
                if (c != b[i])
                    return false;
                i++;
            }
            return true;
        }
    }
}