using Microsoft.VisualStudio.TestTools.UnitTesting;
using CertificateGeneration.Wrappers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CertificateGenerationTests.Wrappers
{
    [TestClass()]
    public class CertificatesWrapperTests
    {
        [TestMethod()]
        public void GenerateCertificateTest_ValidInputGeneratesCert()
        {
            // arrange
            var certificatesWrapper = new CertificatesWrapper();
            CertificateProperties properties = new CertificateProperties
            {
                SubjectName = "CN=microsoft",
                ValidDays = 1
            };

            // act
            var cert = certificatesWrapper.GenerateCertificate(properties);

            // assert
            Assert.IsNotNull(cert);
        }

        [TestMethod()]
        public void GenerateCertificateTest_ValidKeyStrengthGeneratesCert()
        {
            // arrange
            var certificatesWrapper = new CertificatesWrapper();
            CertificateProperties properties = new CertificateProperties
            {
                SubjectName = "CN=microsoft",
                ValidDays = 1,
                KeyStrength = 1024
            };

            // act
            var cert = certificatesWrapper.GenerateCertificate(properties, null);

            // assert
            Assert.IsNotNull(cert);
        }

        [TestMethod()]
        public void GenerateCertificateTest_InvalidSubjectNameThrowsException()
        {
            // arrange
            var certificatesWrapper = new CertificatesWrapper();
            CertificateProperties properties = new CertificateProperties
            {
                SubjectName = "www.microsoft.com",
                ValidDays = 1
            };

            // act and assert
            Assert.ThrowsException<ArgumentException>(() => certificatesWrapper.GenerateCertificate(properties));
        }

        [TestMethod()]
        public void GenerateCertificateTest_InValidKeyStrengthThrowsException()
        {
            // arrange
            var certificatesWrapper = new CertificatesWrapper();
            CertificateProperties properties = new CertificateProperties
            {
                SubjectName = "CN=microsoft",
                ValidDays = 1,
                KeyStrength = 2
            };

            // act and assert
            Assert.ThrowsException<ArithmeticException>(() => certificatesWrapper.GenerateCertificate(properties, null));
        }
    }
}