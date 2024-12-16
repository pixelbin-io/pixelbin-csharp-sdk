using System;
using System.IO;
using System.Net.Mime;
using System.Reflection;
using System.Security.Cryptography;
using System.Xml.Linq;
using Microsoft.Extensions.Configuration;
using Moq;
using Newtonsoft.Json;
using NUnit.Framework;
using Pixelbin.Common;
using Pixelbin.Common.Exceptions;
using Pixelbin.Platform;
using Pixelbin.Platform.Models;
using Pixelbin.Utils;
using static Pixelbin.Utils.Url;
using static PixelbinTest.test_utils;
using RichardSzalay.MockHttp;
using static Pixelbin.Platform.Enums;
using Pixelbin.Security;
using System.Collections.Specialized;
using System.Web;

namespace PixelbinTest
{
    public static class Helpers
    {
        public static Dictionary<string, string> Config = new Dictionary<string, string>() {
            {"domain", "https://api.pixelbin.io"},
            {"apiSecret", "token"},
            {"host", "api.pixelbin.io"}
        };

        public static Dictionary<string, object> MockedRequestsGet()
        {
            return new Dictionary<string, dynamic>(){
                {"status_code", 200},
                {"content", "{\"items\":[]}"}
            };
        }

        public static bool PublicInstancePropertiesEqual<T>(this T self, T to, params string[] ignore) where T : class
        {
            if (self != null && to != null)
            {
                var type = typeof(T);
                var ignoreList = new List<string>(ignore);
                var unequalProperties =
                    from prop in
                    type.GetProperties(BindingFlags.Public | BindingFlags.Instance)
                    where !ignoreList.Contains(prop.Name)
                    let selfValue = type.GetProperty(prop.Name).GetValue(self, null)
                    let toValue = type.GetProperty(prop.Name).GetValue(to, null)
                    where selfValue != toValue &&
                    (selfValue == null || !selfValue.Equals(toValue))
                    select selfValue;
                return !unequalProperties.Any();
            }
            return self == to;
        }
    }

    [TestFixture]
    public class TestPixelBin
    {
        private Dictionary<string, string> config = new Dictionary<string, string>();
        private PixelbinConfig pixelbinConfig;
        private PixelbinClient pixelbinClient;
        private MockHttpMessageHandler httpClientMock;
        private HttpResponseMessage mockResponse;

        private string folder_name;
        private string folder_path;
        private List<Dictionary<string, object>> urls_to_obj;
        private List<Dictionary<string, object>> obj_to_urls;

        private IConfigurationRoot _configuration;

        public MockHttpMessageHandler SetupMock(MockResponseData mockResponseData)
        {
            MockHttpMessageHandler mockHttp = new MockHttpMessageHandler();
            mockHttp.When(new HttpMethod(mockResponseData.response.method.ToUpper()), mockResponseData.response.url)
                    .Respond("application/json", mockResponseData.response.content);

            HttpHelper.client = new HttpClientWrapper(mockHttp.ToHttpClient());
            return mockHttp;
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

            CreateData();
        }

        private void CreateData()
        {
            this.folder_name = "testdir";
            this.folder_path = "/";
            this.urls_to_obj = URLS_TO_OBJ;
            this.obj_to_urls = OBJ_TO_URLS;
            GenerateMockResponses();
        }

        [Test]
        [Order(1)]
        [Category("Config & Client")]
        public void Test_PixelbinConfigAndClient()
        {
            Assert.That(this.pixelbinConfig.Domain, Is.EqualTo(this.config["domain"]));
            Assert.That(this.pixelbinConfig.ApiSecret, Is.EqualTo(this.config["apiSecret"]));

            Assert.That(this.pixelbinClient.config, Is.EqualTo(this.pixelbinConfig));
            Assert.That(this.pixelbinClient.assets, Is.InstanceOf<Assets>());
            Assert.That(this.pixelbinClient.organization, Is.InstanceOf<Organization>());
        }

        [Test]
        [Order(2)]
        [Category("Config & Client")]
        public void Test_PixelbinConfigTokenCase1()
        {
            try
            {
                this.pixelbinConfig = new PixelbinConfig(new Dictionary<string, string>()
                {
                    { "domain", "https://api.pixelbin.io" }
                });

            }
            catch (Exception ex)
            {
                Assert.That(ex, Is.InstanceOf<PixelbinInvalidCredentialError>());
                Assert.That(ex.Message, Is.EqualTo("No API Secret Token Present"));
            }
        }

        [Test]
        [Order(3)]
        [Category("Config & Client")]
        public void Test_PixelbinConfigTokenCase2()
        {
            try
            {
                this.pixelbinConfig = new PixelbinConfig(new Dictionary<string, string>()
                {
                    { "domain", "https://api.pixelbin.io" },
                    { "apiSecret", "abc" }
                });

            }
            catch (Exception ex)
            {
                Assert.That(ex, Is.InstanceOf<PixelbinInvalidCredentialError>());
                Assert.That(ex.Message, Is.EqualTo("Invalid API Secret Token"));
            }
        }

        [Test]
        [Order(4)]
        [Category("API methods")]
        public void Test_CreateFolder()
        {
            var mockHttp = SetupMock(MOCK_RESPONSE["createFolder"]);

            var resp = this.pixelbinClient.assets.createFolder(this.folder_name, this.folder_path);

            mockHttp.VerifyNoOutstandingExpectation();
        }

        [Test]
        [Order(5)]
        [Category("API methods")]
        public void Test_FileUploadCase1()
        {
            var mockHttp = SetupMock(MOCK_RESPONSE["fileUpload1"]);

            FileStream file = new FileStream("./1.jpeg", FileMode.Open);
            var resp = this.pixelbinClient.assets.fileUpload(file);

            mockHttp.VerifyNoOutstandingExpectation();
        }

        [Test]
        [Order(6)]
        [Category("API methods")]
        public void Test_FileUploadCase2()
        {
            var mockHttp = SetupMock(MOCK_RESPONSE["fileUpload2"]);

            var tags = new List<string>() { "tag1", "tag2" };
            var resp = this.pixelbinClient.assets.fileUpload(
                file: new FileStream("./1.jpeg", FileMode.Open),
                path: this.folder_name,
                name: "1",
                access: Enums.AccessEnum.PUBLIC_READ,
                tags: tags,
                metadata: new Dictionary<string, object>(),
                overwrite: false,
                filenameOverride: true
            );

            Assert.That(resp.tags, Is.EqualTo(tags));
            mockHttp.VerifyNoOutstandingExpectation();
        }

        [Test]
        [Order(7)]
        [Category("API methods")]
        public void Test_GetFileById()
        {
            var mockHttp = SetupMock(MOCK_RESPONSE["getFileById"]);

            string _id = "9d331030-b695-475e-9d4a-a660696d5fa5";
            var resp = this.pixelbinClient.assets.getFileById(_id: _id);

            mockHttp.VerifyNoOutstandingExpectation();
        }

        [Test]
        [Order(8)]
        [Category("API methods")]
        public void Test_ListFilesCase1()
        {
            var mockHttp = SetupMock(MOCK_RESPONSE["listFiles1"]);

            var resp = this.pixelbinClient.assets.listFiles();

            mockHttp.VerifyNoOutstandingExpectation();
        }

        [Test]
        [Order(9)]
        [Category("API methods")]
        public void Test_ListFilesCase2()
        {
            var mockHttp = SetupMock(MOCK_RESPONSE["listFiles2"]);

            var resp = this.pixelbinClient.assets.listFiles(
                name: "1",
                path: this.folder_name,
                format: "jpeg",
                tags: new List<string>() { "tag4", "tag3" },
                onlyFiles: true,
                onlyFolders: false,
                pageNo: 1,
                pageSize: 10,
                sort: "name");

            mockHttp.VerifyNoOutstandingExpectation();
        }

        [Test]
        [Order(10)]
        [Category("API methods")]
        public void Test_UrlUpload()
        {
            var mockHttp = SetupMock(MOCK_RESPONSE["urlUpload"]);

            List<string> tags = new List<string>() { "cat", "animal" };
            var resp = this.pixelbinClient.assets.urlUpload(
                url: "https://www.fetchfind.com/blog/wp-content/uploads/2017/08/cat-2734999_1920-5-common-cat-sounds.jpg",
                path: this.folder_name,
                name: "2",
                access: Enums.AccessEnum.PUBLIC_READ,
                tags: tags,
                metadata: new Dictionary<string, dynamic>(),
                overwrite: false,
                filenameOverride: true);

            Assert.That(resp.tags, Is.EqualTo(tags));

            mockHttp.VerifyNoOutstandingExpectation();
        }

        [Test]
        [Order(11)]
        [Category("API methods")]
        public void Test_CreateSignedUrlCase1()
        {
            var mockHttp = SetupMock(MOCK_RESPONSE["createSignedURL1"]);

            var resp = this.pixelbinClient.assets.createSignedUrl();

            mockHttp.VerifyNoOutstandingExpectation();
        }

        [Test]
        [Order(12)]
        [Category("API methods")]
        public void Test_CreateSignedUrlCase2()
        {
            var mockHttp = SetupMock(MOCK_RESPONSE["createSignedURL2"]);

            var resp = this.pixelbinClient.assets.createSignedUrl(
                name: "1",
                path: this.folder_name,
                format: "jpeg",
                access: Enums.AccessEnum.PUBLIC_READ,
                tags: new List<string>() { "tag1", "tag2" },
                metadata: new Dictionary<string, dynamic>(),
                overwrite: false,
                filenameOverride: true);

            mockHttp.VerifyNoOutstandingExpectation();
        }

        [Test]
        [Order(13)]
        [Category("API methods")]
        public void Test_UpdateFileCase1()
        {
            var mockHttp = SetupMock(MOCK_RESPONSE["updateFile1"]);

            var resp = this.pixelbinClient.assets.updateFile(fileId: "1", name: "1_");

            mockHttp.VerifyNoOutstandingExpectation();
        }

        [Test]
        [Order(14)]
        [Category("API methods")]
        public void Test_UpdateFileCase2()
        {
            var mockHttp = SetupMock(MOCK_RESPONSE["updateFile2"]);

            List<string> tags = new List<string>() { "updated-tag1", "updated-tag2" };
            var resp = this.pixelbinClient.assets.updateFile(
                fileId: $"{this.folder_name}/1",
                name: $"{this.folder_name}_",
                path: this.folder_name,
                access: Enums.AccessEnum.PRIVATE,
                isActive: true,
                tags: tags,
                metadata: new Dictionary<string, object>() { { "key", "value" } });
            Assert.That(resp.tags, Is.EqualTo(tags));

            mockHttp.VerifyNoOutstandingExpectation();
        }

        [Test]
        [Order(15)]
        [Category("API methods")]
        public void Test_GetFileByFileId()
        {
            var mockHttp = SetupMock(MOCK_RESPONSE["getFileByFileId"]);

            var resp = this.pixelbinClient.assets.getFileByFileId(fileId: $"{this.folder_name}/2");

            mockHttp.VerifyNoOutstandingExpectation();
        }

        [Test]
        [Order(16)]
        [Category("API methods")]
        public void Test_DeleteFile()
        {
            var mockHttp = SetupMock(MOCK_RESPONSE["deleteFile"]);

            var resp = this.pixelbinClient.assets.deleteFile(fileId: "1_");

            mockHttp.VerifyNoOutstandingExpectation();
        }

        [Test]
        [Order(17)]
        [Category("API methods")]
        public void Test_DeleteFiles()
        {
            var mockHttp = SetupMock(MOCK_RESPONSE["deleteFiles"]);

            List<string> ids = new List<string>()
            {
                "9d331030-b695-475e-9d4a-a660696d5fa5",
                "aaf3f9c4-18bc-4aa5-8cac-2c45dd8df889"
            };
            var resp = this.pixelbinClient.assets.deleteFiles(ids: ids);

            mockHttp.VerifyNoOutstandingExpectation();
        }

        [Test]
        [Order(18)]
        [Category("API methods")]
        public void Test_UpdateFolder()
        {
            var mockHttp = SetupMock(MOCK_RESPONSE["updateFolder"]);

            var resp = this.pixelbinClient.assets.updateFolder(folderId: $"{this.folder_name}", isActive: true);

            mockHttp.VerifyNoOutstandingExpectation();
        }

        [Test]
        [Order(19)]
        [Category("API methods")]
        public void Test_GetFolderDetails()
        {
            var mockHttp = SetupMock(MOCK_RESPONSE["getFolderDetails"]);

            var resp = this.pixelbinClient.assets.getFolderDetails(
                path: "",
                name: this.folder_name);

            mockHttp.VerifyNoOutstandingExpectation();
        }

        [Test]
        [Order(20)]
        [Category("API methods")]
        public void Test_GetFolderAncestors()
        {
            var mockHttp = SetupMock(MOCK_RESPONSE["getFolderAncestors"]);

            var result = this.pixelbinClient.assets.getFolderAncestors(_id: "4a08fc27-e8ee-4e2d-9438-85c1ba07423e");

            mockHttp.VerifyNoOutstandingExpectation();
        }

        [Test]
        [Order(21)]
        [Category("API methods")]
        public void Test_DeleteFolder()
        {
            var mockHttp = SetupMock(MOCK_RESPONSE["deleteFolder"]);

            var deleteresp = this.pixelbinClient.assets.deleteFolder(_id: "b89b2fbb-c520-444a-98a4-f3c20276c0a3");

            mockHttp.VerifyNoOutstandingExpectation();
        }

        [Test]
        [Order(22)]
        [Category("API methods")]
        public void Test_GetModules()
        {
            var mockHttp = SetupMock(MOCK_RESPONSE["getModules"]);

            var resp = this.pixelbinClient.assets.getModules();

            mockHttp.VerifyNoOutstandingExpectation();
        }

        [Test]
        [Order(23)]
        [Category("API methods")]
        public void Test_GetModule()
        {
            var mockHttp = SetupMock(MOCK_RESPONSE["getModule"]);

            var resp = this.pixelbinClient.assets.getModule(identifier: "erase");

            mockHttp.VerifyNoOutstandingExpectation();
        }

        [Test]
        [Order(24)]
        [Category("API methods")]
        public void Test_AddCredentials()
        {
            var mockHttp = SetupMock(MOCK_RESPONSE["addCredentials"]);

            var resp = this.pixelbinClient.assets.addCredentials(
                credentials: new Dictionary<string, object>() { { "apiKey", MOCK_RESPONSE["updateCredentials"].apiKey } },
                pluginId: "erase");

            mockHttp.VerifyNoOutstandingExpectation();
        }

        [Test]
        [Order(25)]
        [Category("API methods")]
        public void Test_UpdateCredentials()
        {
            var mockHttp = SetupMock(MOCK_RESPONSE["updateCredentials"]);

            var resp = this.pixelbinClient.assets.updateCredentials(
                credentials: new Dictionary<string, object>() { { "apiKey", MOCK_RESPONSE["updateCredentials"].apiKey } },
                pluginId: "remove");

            mockHttp.VerifyNoOutstandingExpectation();
        }

        [Test]
        [Order(26)]
        [Category("API methods")]
        public void Test_DeleteCredentials()
        {
            var mockHttp = SetupMock(MOCK_RESPONSE["deleteCredentials"]);

            var resp = this.pixelbinClient.assets.deleteCredentials(pluginId: "remove");

            mockHttp.VerifyNoOutstandingExpectation();
        }

        [Test]
        [Order(27)]
        [Category("API methods")]
        public void Test_AddPreset()
        {
            var mockHttp = SetupMock(MOCK_RESPONSE["addPreset"]);

            var resp = this.pixelbinClient.assets.addPreset(
                presetName: "p1",
                transformation: "t.flip()~t.flop()",
                @params: new Dictionary<string, object>() {
                    {
                        "w", new Dictionary<string, object>() {
                            { "type", "integer" },
                            { "default", 200 }
                        }
                    },
                    {
                        "h", new Dictionary<string, object>()
                        {
                            { "type","integer" },
                            { "default",400 }
                        }
                    }
                });

            mockHttp.VerifyNoOutstandingExpectation();
        }

        [Test]
        [Order(28)]
        [Category("API methods")]
        public void Test_GetPresets()
        {
            var mockHttp = SetupMock(MOCK_RESPONSE["getPresets"]);

            var resp = this.pixelbinClient.assets.getPresets();

            mockHttp.VerifyNoOutstandingExpectation();
        }

        [Test]
        [Order(29)]
        [Category("API methods")]
        public void Test_UpdatePresets()
        {
            var mockHttp = SetupMock(MOCK_RESPONSE["updatePresets"]);

            var resp = this.pixelbinClient.assets.updatePreset(
                presetName: "p1",
                archived: true
            );

            mockHttp.VerifyNoOutstandingExpectation();
        }

        [Test]
        [Order(30)]
        [Category("API methods")]
        public void Test_GetPreset()
        {
            var mockHttp = SetupMock(MOCK_RESPONSE["getPreset"]);

            var resp = this.pixelbinClient.assets.getPreset(
                presetName: "p1"
            );

            mockHttp.VerifyNoOutstandingExpectation();
        }

        [Test]
        [Order(31)]
        [Category("API methods")]
        public void Test_DeletePreset()
        {
            var mockHttp = SetupMock(MOCK_RESPONSE["deletePreset"]);

            var resp = this.pixelbinClient.assets.deletePreset(
                presetName: "p1"
            );

            mockHttp.VerifyNoOutstandingExpectation();
        }

        [Test]
        [Order(32)]
        [Category("API methods")]
        public void Test_GetDefaultAssetForPlayground()
        {
            var mockHttp = SetupMock(MOCK_RESPONSE["getDefaultAssetForPlayground"]);

            var resp = this.pixelbinClient.assets.getDefaultAssetForPlayground();

            mockHttp.VerifyNoOutstandingExpectation();
        }

        [Test]
        [Order(33)]
        [Category("API methods")]
        public void Test_GetAppOrgDetails()
        {
            var mockHttp = SetupMock(MOCK_RESPONSE["getAppOrgDetails"]);

            var resp = this.pixelbinClient.organization.getAppOrgDetails();

            mockHttp.VerifyNoOutstandingExpectation();
        }

        [Test]
        [Order(34)]
        [Category("URL-UrlObj convertion")]
        public void Test_UrlToObj()
        {
            foreach (var url_case in this.urls_to_obj)
            {
                var input_url = (string)url_case["url"];
                UrlObj expected_obj = (UrlObj)url_case["obj"];
                var generated_obj = UrlToObj(input_url);

                Assert.That(generated_obj.ToString(), Is.EqualTo(expected_obj.ToString()));
            }
        }

        [Test]
        [Order(35)]
        [Category("URL-UrlObj convertion")]
        public void Test_ObjToUrl()
        {
            foreach (var obj_case in this.obj_to_urls)
            {
                var input_obj = (UrlObj)obj_case["obj"];
                var expected_url = obj_case["url"].ToString();
                try
                {
                    var generated_url = ObjToUrl(input_obj);
                    Assert.That(generated_url, Is.EqualTo(expected_url));
                }
                catch (Exception err)
                {
                    Console.WriteLine(err.Message);
                    Assert.That(err.Message, Is.EqualTo((string)obj_case["error"]));
                }
            }
        }

        [Test]
        [Order(36)]
        [Category("URL-UrlObj convertion")]
        public void Test_FailureForOptionDprQueryParams()
        {
            UrlObj obj = new UrlObj(
                baseUrl: "https://cdn.pixelbin.io",
                filePath: "__playground/playground-default.jpeg",
                version: "v2",
                zone: "z-slug",
                cloudName: "red-scene-95b6ea",
                options: new UrlObjOptions(
                    dpr: 5.5,
                    f_auto: true
                    ),
                transformations: new List<UrlTransformation>()
                );
            Assert.Throws<PixelbinIllegalQueryParameterError>(() => ObjToUrl(obj));
        }

        [Test]
        [Order(37)]
        [Category("URL-UrlObj convertion")]
        public void Test_FailureForOptionFAutoQueryParam()
        {
            UrlObj obj = new UrlObj(
                baseUrl: "https://cdn.pixelbin.io",
                filePath: "__playground/playground-default.jpeg",
                version: "v2",
                zone: "z-slug",
                cloudName: "red-scene-95b6ea",
                options: new UrlObjOptions(
                    dpr: 2.5,
                    f_auto: "abc"
                    ),
                transformations: new List<UrlTransformation>()
                );
            Assert.Throws<PixelbinIllegalQueryParameterError>(() => ObjToUrl(obj));
        }

        [Test]
        [Order(38)]
        [Category("URL Signing")]
        public void Test_SignUrl()
        {
            string signedURL = Security.SignURL(
                "https://cdn.pixelbin.io/v2/dummy-cloudname/original/__playground/playground-default.jpeg",
                20,
                "459337ed-f378-4ddf-bad7-d7a4555c4572",
                "dummy-token"
            );

            Console.WriteLine(signedURL);

            Uri signedUrlObj = new Uri(signedURL);
            NameValueCollection searchParams = HttpUtility.ParseQueryString(signedUrlObj.Query);

            string[] keys = new string[] { "pbs", "pbe", "pbt" };

            foreach (string key in keys)
            {
                Assert.That(searchParams.AllKeys.Contains(key), Is.True);
                Assert.That(searchParams.Get(key), Is.Not.Null);
            }
        }

        [Test]
        [Order(39)]
        [Category("URL Signing")]
        public void Test_SignUrlWithQuery()
        {
            string signedURL = Security.SignURL(
                "https://cdn.pixelbin.io/v2/dummy-cloudname/original/__playground/playground-default.jpeg?testquery1=testval&testquery2=testval",
                20,
                "459337ed-f378-4ddf-bad7-d7a4555c4572",
                "dummy-token"
            );

            Uri signedUrlObj = new Uri(signedURL);
            NameValueCollection searchParams = HttpUtility.ParseQueryString(signedUrlObj.Query);

            string[] keys = new string[] { "pbs", "pbe", "pbt", "testquery1", "testquery2" };

            foreach (string key in keys)
            {
                Assert.That(searchParams.AllKeys.Contains(key), Is.True);
                Assert.That(searchParams.Get(key), Is.Not.Null);
                if (key.Contains("testquery"))
                    Assert.That(searchParams.Get(key), Is.EqualTo("testval"));
            }
        }

        [Test]
        [Order(40)]
        [Category("URL Signing")]
        public void Test_SignUrlCustomDomain()
        {
            string signedURL = Security.SignURL(
                "https://krit.imagebin.io/v2/original/__playground/playground-default.jpeg",
                20,
                "08040485-dc83-450b-9e1f-f1040044ae3f",
                "dummy-token-2"
            );

            Uri signedUrlObj = new Uri(signedURL);
            NameValueCollection searchParams = HttpUtility.ParseQueryString(signedUrlObj.Query);

            string[] keys = new string[] { "pbs", "pbe", "pbt" };

            foreach (string key in keys)
            {
                Assert.That(searchParams.AllKeys.Contains(key), Is.True);
                Assert.That(searchParams.Get(key), Is.Not.Null);
            }
        }

        [Test]
        [Order(41)]
        [Category("URL Signing")]
        public void Test_FailureWhenEmptyUrlProvided()
        {
            Assert.Throws<PixelbinIllegalArgumentError>(() => Security.SignURL("", 20, "1", "dummy-token"));
        }

        [Test]
        [Order(42)]
        [Category("URL Signing")]
        public void Test_FailureWhenEmptyAccessKeyProvided()
        {
            Assert.Throws<PixelbinIllegalArgumentError>(() => Security.SignURL("https://cdn.pixelbin.io/v2/dummy-cloudname/original/__playground/playground-default.jpeg", 20, "", "dummy-token"));
        }

        [Test]
        [Order(43)]
        [Category("URL Signing")]
        public void Test_FailureWhenEmptyTokenProvided()
        {
            Assert.Throws<PixelbinIllegalArgumentError>(() => Security.SignURL("https://cdn.pixelbin.io/v2/dummy-cloudname/original/__playground/playground-default.jpeg", 20, "1", ""));
        }
    }
}