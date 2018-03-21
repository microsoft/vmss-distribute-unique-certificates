using System;
using System.Text;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;


using Org.BouncyCastle.Asn1;
using Org.BouncyCastle.Asn1.Pkcs;
using Org.BouncyCastle.Asn1.X509;
using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Crypto.Digests;
using Org.BouncyCastle.Crypto.Generators;
using Org.BouncyCastle.Crypto.Operators;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.Crypto.Prng;
using Org.BouncyCastle.Math;
using Org.BouncyCastle.OpenSsl;
using Org.BouncyCastle.Pkcs;
using Org.BouncyCastle.Security;
using Org.BouncyCastle.Utilities;
using Org.BouncyCastle.X509;

using CertificateGeneration.Utilities;

namespace CertificateGeneration.Wrappers
{
    public interface ICertificatesWrapper
    {
        X509Certificate2 GenerateCertificate(string subjectName, X509Certificate2 ca = null, int keyStrength = 2048);
        string ExportToPEM(X509Certificate2 cert);
        string ExportToPfx(X509Certificate2 cert);
    }

    public class CertificatesWrapper : ICertificatesWrapper
    {
        private static readonly IRandomGenerator sha256Generator = new DigestRandomGenerator(new Sha256Digest());

        public X509Certificate2 GenerateCertificate(string subjectName, X509Certificate2 ca = null, int keyStrength = 2048)
        {
            // Generating Random Number
            var random = new SecureRandom(sha256Generator);

            // The Certificate Generator
            var certificateGenerator = new X509V3CertificateGenerator();

            // Signature Algorithm
            const string signatureAlgorithm = "SHA256WithRSA";

            // Serial Number
            var serialNumber = BigIntegers.CreateRandomInRange(BigInteger.One, BigInteger.ValueOf(Int64.MaxValue), random);

            // Issuer and Subject Name
            var subjectDN = new X509Name(subjectName);
            var issuerDN = (ca != null) ? new X509Name(ca.SubjectName.Name) : subjectDN;

            // Valid For
            var notBefore = DateTime.UtcNow.Date;
            var notAfter = notBefore.AddDays(3);

            // Subject Public Key
            var keyGenerationParameters = new KeyGenerationParameters(random, keyStrength);
            var keyPairGenerator = new RsaKeyPairGenerator();
            keyPairGenerator.Init(keyGenerationParameters);
            AsymmetricCipherKeyPair subjectKeyPair = keyPairGenerator.GenerateKeyPair();

            // Generating the Certificate
            var issuerKeyPair = ca != null ? BouncyCastleUtilities.GetRsaKeyPair((RSA)ca.PrivateKey) : subjectKeyPair;
            ISignatureFactory signatureFactory = new Asn1SignatureFactory(signatureAlgorithm, issuerKeyPair.Private, random);

            certificateGenerator.SetSerialNumber(serialNumber);
            certificateGenerator.SetIssuerDN(issuerDN);
            certificateGenerator.SetSubjectDN(subjectDN);
            certificateGenerator.SetNotBefore(notBefore);
            certificateGenerator.SetNotAfter(notAfter);
            certificateGenerator.SetPublicKey(subjectKeyPair.Public);

            if (ca == null)
            {
                certificateGenerator.AddExtension(X509Extensions.BasicConstraints, true, new BasicConstraints(true));
                certificateGenerator.AddExtension(X509Extensions.KeyUsage, true, new KeyUsage(KeyUsage.KeyCertSign));
            }

            // selfsign certificate
            var certificate = certificateGenerator.Generate(signatureFactory);
            var x509 = new X509Certificate2(certificate.GetEncoded());

            PrivateKeyInfo info = PrivateKeyInfoFactory.CreatePrivateKeyInfo(subjectKeyPair.Private);
            var seq = (Asn1Sequence)info.ParsePrivateKey();

            if (seq.Count != 9)
                throw new PemException("malformed sequence in RSA private key");

            var rsa = RsaPrivateKeyStructure.GetInstance(seq);
            var keyparams = new RsaPrivateCrtKeyParameters(
                rsa.Modulus, // RsaPrivateCrtKeyParameters.modulus
                rsa.PublicExponent, // RsaPrivateCrtKeyParameters.publicExponent
                rsa.PrivateExponent, //RsaPrivateCrtKeyParameters.privateExponent                                          
                rsa.Prime1, // RsaPrivateCrtKeyParameters.p 
                rsa.Prime2, // RsaPrivateCrtKeyParameters.q
                rsa.Exponent1, // RsaPrivateCrtKeyParameters.dP
                rsa.Exponent2, // RsaPrivateCrtKeyParameters.qInv
                rsa.Coefficient // RsaPrivateCrtKeyParameters.dp
            );
            /* RsaPrivateKeyStructure parameter mapping description found here: https://github.com/neoeinstein/bouncycastle/blob/00ccedf8f4537a8621944f3072453e4603d09c4b/crypto/src/asn1/pkcs/RSAPrivateKeyStructure.cs#L101 */

            try
            {
                var privateKey = BouncyCastleUtilities.ToRSA(keyparams);
                x509 = x509.CopyWithPrivateKey(privateKey);
            }
            catch(Exception e) 
            {
                Console.WriteLine(e); 
            }

            return x509;
        }

        public string ExportToPEM(X509Certificate2 cert)
        {
            StringBuilder builder = new StringBuilder();

            builder.AppendLine("-----BEGIN CERTIFICATE-----");
            builder.AppendLine(Convert.ToBase64String(cert.Export(X509ContentType.Cert), Base64FormattingOptions.InsertLineBreaks));
            builder.AppendLine("-----END CERTIFICATE-----");

            return builder.ToString();
        }

        public string ExportToPfx(X509Certificate2 cert)
        {
            return Convert.ToBase64String(cert.Export(X509ContentType.Pfx));
        }
    }
}