using Moq;
using NUnit.Framework;
using Pixelbin.Platform;
using static Pixelbin.Platform.Enums;
using Pixelbin.Platform.Models;
using Pixelbin.Common;
using RichardSzalay.MockHttp;
using static PixelbinTest.test_utils;
using Microsoft.Extensions.Configuration;
using System.Net;

namespace PixelbinTest
{
    [TestFixture]
    public class UploaderTests
    {
        private Mock<Assets> mockAssets;
        private Uploader uploader;
        private Dictionary<string, string> config = new Dictionary<string, string>();
        private PixelbinConfig pixelbinConfig;
        private PixelbinClient pixelbinClient;

        private IConfigurationRoot _configuration;

        public MockHttpMessageHandler SetupMock(MockResponseData[] mockResponses)
        {
            MockHttpMessageHandler mockHttp = new MockHttpMessageHandler();
            foreach (var mockResponseData in mockResponses)
            {
                mockHttp.When(new HttpMethod(mockResponseData.response.method.ToUpper()), mockResponseData.response.url)
                        .Respond((HttpStatusCode)mockResponseData.response.status_code, "application/json", mockResponseData.response.content);
            }

            HttpHelper.client = new HttpClientWrapper(mockHttp.ToHttpClient());
            return mockHttp;
        }

        private void CreateData()
        {
            GenerateMockResponses();
        }

        [SetUp]
        public void SetUp()
        {
            _configuration = new ConfigurationBuilder()
                .AddUserSecrets<TestPixelBin>()
                .Build();
            this.config["domain"] = _configuration["domain"] ?? Helpers.Config["domain"];
            this.config["apiSecret"] = _configuration["apiSecret"] ?? Helpers.Config["apiSecret"];
            pixelbinConfig = new PixelbinConfig(this.config);
            pixelbinClient = new PixelbinClient(this.pixelbinConfig);
            mockAssets = new Mock<Assets>(pixelbinConfig);
            uploader = new Uploader(mockAssets.Object);
            CreateData();
        }

        [Test]
        public async Task TestUploadWithBuffer()
        {
            var mockHttp = SetupMock(new MockResponseData[]
            {
                MOCK_RESPONSE["createSignedUrlV2Case1"],
                MOCK_RESPONSE["signedUrlChunkUpload"],
                MOCK_RESPONSE["signedUrlUploadComplete"]
            });

            SignedUploadV2Response presignedUrlResponse = new SignedUploadV2Response();
            presignedUrlResponse.presignedUrl = new PresignedUrlV2();
            presignedUrlResponse.presignedUrl.url = "https://mockurl.com/upload";
            presignedUrlResponse.presignedUrl.fields = new Dictionary<string, object>();

            var buffer = System.Text.Encoding.UTF8.GetBytes("test file content");
            await uploader.Upload(
                buffer,
                "test.txt",
                "/uploads",
                "txt",
                AccessEnum.PUBLIC_READ,
                new List<string>(),
                new Dictionary<string, object>(),
                true,
                true,
                1500,
                new UploadOptions
                {
                    ChunkSize = 5 * 1024 * 1024,
                    Concurrency = 1
                }
            );

            mockHttp.VerifyNoOutstandingExpectation();
        }

        [Test]
        public async Task TestUploadWithStream()
        {
            var mockHttp = SetupMock(new MockResponseData[]
            {
                MOCK_RESPONSE["createSignedUrlV2Case1"],
                MOCK_RESPONSE["signedUrlChunkUpload"],
                MOCK_RESPONSE["signedUrlUploadComplete"]
            });

            SignedUploadV2Response presignedUrlResponse = new SignedUploadV2Response();
            presignedUrlResponse.presignedUrl = new PresignedUrlV2();
            presignedUrlResponse.presignedUrl.url = "https://mockurl.com/upload";
            presignedUrlResponse.presignedUrl.fields = new Dictionary<string, object>();

            var stream = new MemoryStream(System.Text.Encoding.UTF8.GetBytes("test file content"));
            await uploader.Upload(
                stream,
                "test.txt",
                "/uploads",
                "txt",
                AccessEnum.PUBLIC_READ,
                new List<string>(),
                new Dictionary<string, object>(),
                true,
                true,
                1500,
                new UploadOptions
                {
                    ChunkSize = 5 * 1024 * 1024,
                    Concurrency = 1
                }
            );

            mockHttp.VerifyNoOutstandingExpectation();
        }
    }
}