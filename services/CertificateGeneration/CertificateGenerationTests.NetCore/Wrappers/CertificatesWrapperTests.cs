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

            // act
            var cert = certificatesWrapper.GenerateCertificate("CN=microsoft");

            // assert
            Assert.IsNotNull(cert);
        }

        [TestMethod()]
        public void GenerateCertificateTest_ValidKeyStrengthGeneratesCert()
        {
            // arrange
            var certificatesWrapper = new CertificatesWrapper();

            // act
            var cert = certificatesWrapper.GenerateCertificate("CN=microsoft", null, 1024);

            // assert
            Assert.IsNotNull(cert);
        }

        [TestMethod()]
        public void GenerateCertificateTest_InvalidSubjectNameThrowsException()
        {
            // arrange
            var certificatesWrapper = new CertificatesWrapper();

            // act and assert
            Assert.ThrowsException<ArgumentException>(() => certificatesWrapper.GenerateCertificate("www.microsoft.com"));
        }

        [TestMethod()]
        public void GenerateCertificateTest_InValidKeyStrengthThrowsException()
        {
            // arrange
            var certificatesWrapper = new CertificatesWrapper();

            // act and assert
            Assert.ThrowsException<ArithmeticException>(() => certificatesWrapper.GenerateCertificate("CN=microsoft", null, 2));
        }
    }
}