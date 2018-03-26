using System;
using System.Text;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;

namespace CertificateGeneration.Wrappers
{
    public interface ICertificatesWrapper
    {
        X509Certificate2 GenerateCertificate(string subjectName, int validDays, X509Certificate2 ca = null, int keyStrength = 2048);
        string ExportToPEM(X509Certificate2 cert);
        string ExportToPfx(X509Certificate2 cert);
    }

    public class CertificatesWrapper : ICertificatesWrapper
    {
        public X509Certificate2 GenerateCertificate(string subjectName, int validDays, X509Certificate2 ca = null, int keyStrength = 2048)
        {
            var random = new Random(DateTime.Now.Millisecond);
            RSA key = RSA.Create(keyStrength);
            CertificateRequest req = new CertificateRequest(
                subjectName,
                key,
                HashAlgorithmName.SHA256,
                RSASignaturePadding.Pkcs1);
            if (ca == null)
            {
                req.CertificateExtensions.Add(new X509BasicConstraintsExtension(true, false, 0, true));
                req.CertificateExtensions.Add(new X509SubjectKeyIdentifierExtension(req.PublicKey, false));
                return req.CreateSelfSigned(DateTimeOffset.UtcNow, DateTime.UtcNow.Date.AddDays(validDays));
            }
            else
            {
                var notBefore = DateTime.UtcNow;
                var notAfter = notBefore.AddDays(validDays);
                var serialNumber = new byte[4];
                random.NextBytes(serialNumber);

                var cert = req.Create(ca, notBefore, notAfter, serialNumber);
                return cert.CopyWithPrivateKey(key);
            }
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