using Microsoft.VisualStudio.TestTools.UnitTesting;
using CertificateGeneration.Controllers;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using System.Web.Http.Results;
using CertificateGeneration.Wrappers;
using Moq;

namespace CertificateGenerationTests.Controllers
{
    [TestClass()]
    public class DefaultControllerTests
    {
        private static IKVWrapper KvWrapper { get; set; }
        private static ICertificatesWrapper CertificatesWrapper { get; set; }

        [ClassInitialize()]
        public static void ClassInit(TestContext context)
        {
            // mock dependencies
            var kvWrapperMock = new Mock<IKVWrapper>();

            kvWrapperMock
                .Setup(kvWrapper => kvWrapper.UploadPem(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()))
                .Returns(Task.FromResult(default(object)));

            kvWrapperMock
                .Setup(kvWrapper => kvWrapper.UploadPfx(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()))
                .Returns(Task.FromResult(default(object)));

            KvWrapper = kvWrapperMock.Object;

            var certificatesWrapperMock = new Mock<ICertificatesWrapper>();

            certificatesWrapperMock
                .Setup(certificatesWrapper => certificatesWrapper.GenerateCertificate(It.IsAny<string>(), It.IsAny<X509Certificate2>(), It.IsAny<int>()))
                .Returns(new X509Certificate2());

            certificatesWrapperMock
                .Setup(certificatesWrapper => certificatesWrapper.ExportToPEM(It.IsAny<X509Certificate2>()))
                .Returns("");

            certificatesWrapperMock
                .Setup(certificatesWrapper => certificatesWrapper.ExportToPfx(It.IsAny<X509Certificate2>()))
                .Returns("");

            CertificatesWrapper = certificatesWrapperMock.Object;
        }

        [TestMethod()]
        public async Task GenerateCertificatesAsync_ValidRequestEmptyIssuerReturnsOk()
        {
            // arrange
            var defaultController = new DefaultController(KvWrapper, CertificatesWrapper);
            var request = new CertificatesRequest()
            {
                IssuerBase64Pfx = "",
                VaultBaseUrl = "http://microsoft.com",
                CertificatesProperties = new CertificateProperties[] {new CertificateProperties()
                {
                    CertificateName = "name",
                    SecretName = "name",
                    SubjectName = "name"
                }}
            };

            // act
            var result = await defaultController.GenerateCertificatesAsync(request);

            // assert
            Assert.IsInstanceOfType(result, typeof(OkNegotiatedContentResult<List<ResultJson>>));
        }

        [TestMethod()]
        public async Task GenerateCertificatesAsync_ValidRequestNullIssuerReturnsOk()
        {
            // arrange
            var defaultController = new DefaultController(KvWrapper, CertificatesWrapper);
            var request = new CertificatesRequest()
            {
                IssuerBase64Pfx = null,
                VaultBaseUrl = "http://microsoft.com",
                CertificatesProperties = new CertificateProperties[] {new CertificateProperties()
                {
                    CertificateName = "name",
                    SecretName = "name",
                    SubjectName = "name"
                }}
            };

            // act
            var result = await defaultController.GenerateCertificatesAsync(request);

            // assert
            Assert.IsInstanceOfType(result, typeof(OkNegotiatedContentResult<List<ResultJson>>));
        }

        [TestMethod()]
        public async Task GenerateCertificatesAsync_NullRequestBodyReturnsBadRequest()
        {
            // arrange
            var defaultController = new DefaultController(KvWrapper, CertificatesWrapper);

            // act
            var result = await defaultController.GenerateCertificatesAsync(null);

            // assert
            Assert.IsInstanceOfType(result, typeof(BadRequestResult));
        }

        [TestMethod()]
        public async Task GenerateCertificatesAsync_EmptyRequestBodyReturnsBadRequest()
        {
            // arrange
            var defaultController = new DefaultController(KvWrapper, CertificatesWrapper);

            // act
            var result = await defaultController.GenerateCertificatesAsync(new CertificatesRequest());

            // assert
            Assert.IsInstanceOfType(result, typeof(BadRequestResult));
        }

        [TestMethod()]
        public async Task GenerateCertificatesAsync_BadIssuerReturnsInternalServerError()
        {
            // arrange
            var defaultController = new DefaultController(KvWrapper, CertificatesWrapper);
            var request = new CertificatesRequest()
            {
                IssuerBase64Pfx = "hello bob",
                VaultBaseUrl = "http://microsoft.com",
                CertificatesProperties = new CertificateProperties[] {new CertificateProperties()
                {
                    CertificateName = "",
                    SecretName = "",
                    SubjectName = ""
                }}
            };

            // act
            var result = await defaultController.GenerateCertificatesAsync(request);

            // assert
            Assert.IsInstanceOfType(result, typeof(InternalServerErrorResult));
        }
    }
}