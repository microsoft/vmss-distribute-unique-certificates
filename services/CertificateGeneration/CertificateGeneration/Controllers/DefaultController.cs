using System;
using System.Collections.Generic;
using System.Web.Http;
using System.Threading.Tasks;
using System.Security.Cryptography.X509Certificates;

using CertificateGeneration.Wrappers;

namespace CertificateGeneration.Controllers
{
    public class CertificateProperties
    {
        public string SubjectName { get; set; }
        public string CertificateName { get; set; }
        public string SecretName { get; set; }
    }

    public class CertificatesRequest
    {
        public CertificateProperties[] CertificatesProperties { get; set; }
        public string IssuerBase64Pfx { get; set; }
        public string VaultBaseUrl { get; set; }
    }

    public class ResultJson
    {
        public string pfx { get; set; }
        public string result { get; set; }
    }


    public class DefaultController : ApiController
    {
        [HttpPost]
        public async Task<IHttpActionResult> GenerateCertificates([FromBody] CertificatesRequest request)
        {
            X509Certificate2 issuerX509 = request.IssuerBase64Pfx != "" ?
                                             new X509Certificate2(Convert.FromBase64String(request.IssuerBase64Pfx), "", X509KeyStorageFlags.Exportable) :
                                             null;
            KVWrapper wrapper = new KVWrapper();

            var tasks = new List<Task<ResultJson>>();
            foreach (var properties in request.CertificatesProperties)
            {
                tasks.Add(GenerateCertificate(wrapper, properties, request.VaultBaseUrl, issuerX509));
            }

            await Task.WhenAll(tasks);

            var result = new List<ResultJson>();
            foreach (var task in tasks)
            {
                result.Add(task.Result);
            }
            return Ok(result);
        }

        private async Task<ResultJson> GenerateCertificate(KVWrapper wrapper, CertificateProperties properties, string vaultBaseUrl, X509Certificate2 issuerX509)
        {
            var result = new ResultJson();
            try
            {
                var x = CertificatesWrapper.GenerateCertificate(properties.SubjectName, issuerX509);
                result.pfx = Convert.ToBase64String(x.Export(X509ContentType.Pfx));

                if (properties.CertificateName != "")
                {
                    await wrapper.UploadPfx(vaultBaseUrl, properties.CertificateName, result.pfx);
                }

                if (properties.SecretName != "")
                {
                    await wrapper.UploadPem(vaultBaseUrl, properties.SecretName, CertificatesWrapper.ExportToPEM(x));
                }
                result.result = "Success";
            }
            catch (Exception e)
            {
                result.result = e.Message;
            }
            return result;
        }
    }
}
