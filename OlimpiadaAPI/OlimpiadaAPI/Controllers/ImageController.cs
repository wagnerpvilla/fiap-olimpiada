using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Amazon;
using Amazon.Runtime.CredentialManagement;
using Amazon.S3;
using Amazon.S3.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace OlimpiadaAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ImageController : ControllerBase
    {
        private readonly IConfiguration configuration;

        public ImageController(IConfiguration configuration)
        {
            this.configuration = configuration;
        }

        [HttpGet]
        public IEnumerable<SignedUrlModel> GetUrl([FromQuery(Name = "fileName")] IEnumerable<string> fileName)
        {
            var bucket = "fiap-url-test";
            var accessKey = configuration.GetValue<string>("aws_access_key_id");
            var secretKey = configuration.GetValue<string>("aws_secret_access_key");
            var s3Client = new AmazonS3Client(accessKey, secretKey, RegionEndpoint.USEast2);

            return fileName.Select(file =>
            {
                var req = new GetPreSignedUrlRequest()
                {
                    BucketName = bucket,
                    Key = $"artigos/{file}",
                    Expires = DateTime.Now.AddSeconds(120),
                    ContentType = "image/png",
                    Verb = HttpVerb.PUT,
                };
                req.Parameters.Add("ACL", "public-read");
                req.Headers["x-amz-acl"] = "public-read";

                var url = s3Client.GetPreSignedURL(req);
                return new SignedUrlModel()
                {
                    SignedUrl = url,
                    ImageUrl = $"https://{bucket}.s3.amazonaws.com/artigos/{file}",
                    FileName = file
                };
            });
        }
    }
}
