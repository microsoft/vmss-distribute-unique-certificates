using System.Security.Cryptography;

using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.Math;

namespace CertificateGeneration.Utilities
{
    /**
     * 
     * Utility methods copied from Org.BouncyCastle.Security.DotNetUtilities class that is not included in BouncyCastle.NetCore package
     * 
    */
    
    public class BouncyCastleUtilities
    {
        public static AsymmetricCipherKeyPair GetRsaKeyPair(
            RSA rsa)
        {
            return GetRsaKeyPair(rsa.ExportParameters(true));
        }

        public static AsymmetricCipherKeyPair GetRsaKeyPair(
            RSAParameters rp)
        {
            BigInteger modulus = new BigInteger(1, rp.Modulus);
            BigInteger pubExp = new BigInteger(1, rp.Exponent);

            RsaKeyParameters pubKey = new RsaKeyParameters(
                false,
                modulus,
                pubExp);

            RsaPrivateCrtKeyParameters privKey = new RsaPrivateCrtKeyParameters(
                modulus,
                pubExp,
                new BigInteger(1, rp.D),
                new BigInteger(1, rp.P),
                new BigInteger(1, rp.Q),
                new BigInteger(1, rp.DP),
                new BigInteger(1, rp.DQ),
                new BigInteger(1, rp.InverseQ));

            return new AsymmetricCipherKeyPair(pubKey, privKey);
        }

        public static RSA ToRSA(RsaKeyParameters rsaKey)
        {
            RSAParameters rp = ToRSAParameters(rsaKey);
            RSACryptoServiceProvider rsaCsp = new RSACryptoServiceProvider();
            rsaCsp.ImportParameters(rp);
            return rsaCsp;
        }

        public static RSAParameters ToRSAParameters(RsaKeyParameters rsaKey)
        {
            RSAParameters rp = new RSAParameters();
            rp.Modulus = rsaKey.Modulus.ToByteArrayUnsigned();
            if (rsaKey.IsPrivate)
                rp.D = rsaKey.Exponent.ToByteArrayUnsigned();
            else
                rp.Exponent = rsaKey.Exponent.ToByteArrayUnsigned();
            return rp;
        }

        public static RSA ToRSA(RsaPrivateCrtKeyParameters privKey)
        {
            RSAParameters rp = ToRSAParameters(privKey);
            var rsa = RSA.Create(rp);
            return rsa;
        }

        public static RSAParameters ToRSAParameters(RsaPrivateCrtKeyParameters privKey)
        {
            RSAParameters rp = new RSAParameters();
            rp.Modulus = privKey.Modulus.ToByteArrayUnsigned();
            rp.Exponent = privKey.PublicExponent.ToByteArrayUnsigned();
            rp.D = privKey.Exponent.ToByteArrayUnsigned();
            rp.P = privKey.P.ToByteArrayUnsigned();
            rp.Q = privKey.Q.ToByteArrayUnsigned();
            rp.DP = privKey.DP.ToByteArrayUnsigned();
            rp.DQ = privKey.DQ.ToByteArrayUnsigned();
            rp.InverseQ = privKey.QInv.ToByteArrayUnsigned();
            return rp;
        }
    }
}
