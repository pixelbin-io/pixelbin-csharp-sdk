using System.Reflection;
using Microsoft.Extensions.Configuration;
using NUnit.Framework;
using Pixelbin.Common;
using Pixelbin.Common.Exceptions;
using Pixelbin.Platform;
using Pixelbin.Utils;
using static Pixelbin.Utils.Url;
using static PixelbinTest.test_utils;
using RichardSzalay.MockHttp;
using static Pixelbin.Platform.Enums;

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
                    let selfValue = type.GetProperty(prop.Name)?.GetValue(self, null)
                    let toValue = type.GetProperty(prop.Name)?.GetValue(to, null)
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

        private string folder_name;
        private string folder_path;
        private List<UrlObjTestData> urls_to_obj;
        private List<UrlObjTestData> obj_to_urls;

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
            config["domain"] = _configuration["domain"] ?? Helpers.Config["domain"];
            config["apiSecret"] = _configuration["apiSecret"] ?? Helpers.Config["apiSecret"];
            pixelbinConfig = new PixelbinConfig(config);
            pixelbinClient = new PixelbinClient(pixelbinConfig);

            CreateData();
        }

        private void CreateData()
        {
            folder_name = "testdir";
            folder_path = "/";
            urls_to_obj = URLS_TO_OBJ;
            obj_to_urls = OBJ_TO_URLS;
            GenerateMockResponses();
        }

        [Test]
        [Order(1)]
        [Category("Config & Client")]
        public void Test_PixelbinConfigAndClient()
        {
            Assert.That(pixelbinConfig.Domain, Is.EqualTo(config["domain"]));
            Assert.That(pixelbinConfig.ApiSecret, Is.EqualTo(config["apiSecret"]));

            Assert.That(pixelbinClient.config, Is.EqualTo(pixelbinConfig));
            Assert.That(pixelbinClient.assets, Is.InstanceOf<Assets>());
            Assert.That(pixelbinClient.organization, Is.InstanceOf<Organization>());
        }

        [Test]
        [Order(2)]
        [Category("Config & Client")]
        public void Test_PixelbinConfigTokenCase1()
        {
            try
            {
                pixelbinConfig = new PixelbinConfig(new Dictionary<string, string>()
                {
                    { "domain", "https://api.pixelbin.io" }
                });

            }
            catch (Exception ex)
            {
                Assert.That(ex, Is.InstanceOf<PDKInvalidCredentialError>());
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
                pixelbinConfig = new PixelbinConfig(new Dictionary<string, string>()
                {
                    { "domain", "https://api.pixelbin.io" },
                    { "apiSecret", "abc" }
                });

            }
            catch (Exception ex)
            {
                Assert.That(ex, Is.InstanceOf<PDKInvalidCredentialError>());
                Assert.That(ex.Message, Is.EqualTo("Invalid API Secret Token"));
            }
        }

        [Test]
        [Order(4)]
        [Category("API methods")]
        public void Test_CreateFolder()
        {
            var mockHttp = SetupMock(MOCK_RESPONSE["createFolder"]);

            var resp = pixelbinClient.assets.createFolder(folder_name, folder_path);

            mockHttp.VerifyNoOutstandingExpectation();
        }

        [Test]
        [Order(5)]
        [Category("API methods")]
        public void Test_FileUploadCase1()
        {
            var mockHttp = SetupMock(MOCK_RESPONSE["fileUpload1"]);

            FileStream file = new FileStream("./1.jpeg", FileMode.Open);
            var resp = pixelbinClient.assets.fileUpload(file);

            mockHttp.VerifyNoOutstandingExpectation();
        }

        [Test]
        [Order(6)]
        [Category("API methods")]
        public void Test_FileUploadCase2()
        {
            var mockHttp = SetupMock(MOCK_RESPONSE["fileUpload2"]);

            var tags = new List<string>() { "tag1", "tag2" };
            var resp = pixelbinClient.assets.fileUpload(
                file: new FileStream("./1.jpeg", FileMode.Open),
                path: folder_name,
                name: "1",
                access: Enums.AccessEnum.PUBLIC_READ,
                tags: tags,
                metadata: new Dictionary<string, object>(),
                overwrite: false,
                filenameOverride: true
            );

            Assert.That(resp?.tags, Is.EqualTo(tags));
            mockHttp.VerifyNoOutstandingExpectation();
        }

        [Test]
        [Order(7)]
        [Category("API methods")]
        public void Test_GetFileById()
        {
            var mockHttp = SetupMock(MOCK_RESPONSE["getFileById"]);

            string _id = "9d331030-b695-475e-9d4a-a660696d5fa5";
            var resp = pixelbinClient.assets.getFileById(_id: _id);

            mockHttp.VerifyNoOutstandingExpectation();
        }

        [Test]
        [Order(8)]
        [Category("API methods")]
        public void Test_ListFilesCase1()
        {
            var mockHttp = SetupMock(MOCK_RESPONSE["listFiles1"]);

            var resp = pixelbinClient.assets.listFiles();

            mockHttp.VerifyNoOutstandingExpectation();
        }

        [Test]
        [Order(9)]
        [Category("API methods")]
        public void Test_ListFilesCase2()
        {
            var mockHttp = SetupMock(MOCK_RESPONSE["listFiles2"]);

            var resp = pixelbinClient.assets.listFiles(
                name: "1",
                path: folder_name,
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
            var resp = pixelbinClient.assets.urlUpload(
                url: "https://www.fetchfind.com/blog/wp-content/uploads/2017/08/cat-2734999_1920-5-common-cat-sounds.jpg",
                path: folder_name,
                name: "2",
                access: Enums.AccessEnum.PUBLIC_READ,
                tags: tags,
                metadata: new Dictionary<string, dynamic>(),
                overwrite: false,
                filenameOverride: true);

            Assert.That(resp?.tags, Is.EqualTo(tags));

            mockHttp.VerifyNoOutstandingExpectation();
        }

        [Test]
        [Order(11)]
        [Category("API methods")]
        public void Test_CreateSignedUrlCase1()
        {
            var mockHttp = SetupMock(MOCK_RESPONSE["createSignedURL1"]);

            var resp = pixelbinClient.assets.createSignedUrl();

            mockHttp.VerifyNoOutstandingExpectation();
        }

        [Test]
        [Order(12)]
        [Category("API methods")]
        public void Test_CreateSignedUrlCase2()
        {
            var mockHttp = SetupMock(MOCK_RESPONSE["createSignedURL2"]);

            var resp = pixelbinClient.assets.createSignedUrl(
                name: "1",
                path: folder_name,
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

            var resp = pixelbinClient.assets.updateFile(fileId: "1", name: "1_");

            mockHttp.VerifyNoOutstandingExpectation();
        }

        [Test]
        [Order(14)]
        [Category("API methods")]
        public void Test_UpdateFileCase2()
        {
            var mockHttp = SetupMock(MOCK_RESPONSE["updateFile2"]);

            List<string> tags = new List<string>() { "updated-tag1", "updated-tag2" };
            var resp = pixelbinClient.assets.updateFile(
                fileId: $"{folder_name}/1",
                name: $"{folder_name}_",
                path: folder_name,
                access: Enums.AccessEnum.PRIVATE,
                isActive: true,
                tags: tags,
                metadata: new Dictionary<string, object>() { { "key", "value" } });
            Assert.That(resp?.tags, Is.EqualTo(tags));

            mockHttp.VerifyNoOutstandingExpectation();
        }

        [Test]
        [Order(15)]
        [Category("API methods")]
        public void Test_GetFileByFileId()
        {
            var mockHttp = SetupMock(MOCK_RESPONSE["getFileByFileId"]);

            var resp = pixelbinClient.assets.getFileByFileId(fileId: $"{folder_name}/2");

            mockHttp.VerifyNoOutstandingExpectation();
        }

        [Test]
        [Order(16)]
        [Category("API methods")]
        public void Test_DeleteFile()
        {
            var mockHttp = SetupMock(MOCK_RESPONSE["deleteFile"]);

            var resp = pixelbinClient.assets.deleteFile(fileId: "1_");

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
            var resp = pixelbinClient.assets.deleteFiles(ids: ids);

            mockHttp.VerifyNoOutstandingExpectation();
        }

        [Test]
        [Order(18)]
        [Category("API methods")]
        public void Test_UpdateFolder()
        {
            var mockHttp = SetupMock(MOCK_RESPONSE["updateFolder"]);

            var resp = pixelbinClient.assets.updateFolder(folderId: $"{folder_name}", isActive: true);

            mockHttp.VerifyNoOutstandingExpectation();
        }

        [Test]
        [Order(19)]
        [Category("API methods")]
        public void Test_GetFolderDetails()
        {
            var mockHttp = SetupMock(MOCK_RESPONSE["getFolderDetails"]);

            var resp = pixelbinClient.assets.getFolderDetails(
                path: "",
                name: folder_name);

            mockHttp.VerifyNoOutstandingExpectation();
        }

        [Test]
        [Order(20)]
        [Category("API methods")]
        public void Test_GetFolderAncestors()
        {
            var mockHttp = SetupMock(MOCK_RESPONSE["getFolderAncestors"]);

            var result = pixelbinClient.assets.getFolderAncestors(_id: "4a08fc27-e8ee-4e2d-9438-85c1ba07423e");

            mockHttp.VerifyNoOutstandingExpectation();
        }

        [Test]
        [Order(21)]
        [Category("API methods")]
        public void Test_DeleteFolder()
        {
            var mockHttp = SetupMock(MOCK_RESPONSE["deleteFolder"]);

            var deleteresp = pixelbinClient.assets.deleteFolder(_id: "b89b2fbb-c520-444a-98a4-f3c20276c0a3");

            mockHttp.VerifyNoOutstandingExpectation();
        }

        [Test]
        [Order(22)]
        [Category("API methods")]
        public void Test_GetModules()
        {
            var mockHttp = SetupMock(MOCK_RESPONSE["getModules"]);

            var resp = pixelbinClient.assets.getModules();

            mockHttp.VerifyNoOutstandingExpectation();
        }

        [Test]
        [Order(23)]
        [Category("API methods")]
        public void Test_GetModule()
        {
            var mockHttp = SetupMock(MOCK_RESPONSE["getModule"]);

            var resp = pixelbinClient.assets.getModule(identifier: "erase");

            mockHttp.VerifyNoOutstandingExpectation();
        }

        [Test]
        [Order(24)]
        [Category("API methods")]
        public void Test_AddCredentials()
        {
            var mockHttp = SetupMock(MOCK_RESPONSE["addCredentials"]);

            var resp = pixelbinClient.assets.addCredentials(
                credentials: new Dictionary<string, object>() { { "apiKey", MOCK_RESPONSE["updateCredentials"].apiKey ?? "123" } },
                pluginId: "erase");

            mockHttp.VerifyNoOutstandingExpectation();
        }

        [Test]
        [Order(25)]
        [Category("API methods")]
        public void Test_UpdateCredentials()
        {
            var mockHttp = SetupMock(MOCK_RESPONSE["updateCredentials"]);

            var resp = pixelbinClient.assets.updateCredentials(
                credentials: new Dictionary<string, object>() { { "apiKey", MOCK_RESPONSE["updateCredentials"].apiKey ?? "123" } },
                pluginId: "remove");

            mockHttp.VerifyNoOutstandingExpectation();
        }

        [Test]
        [Order(26)]
        [Category("API methods")]
        public void Test_DeleteCredentials()
        {
            var mockHttp = SetupMock(MOCK_RESPONSE["deleteCredentials"]);

            var resp = pixelbinClient.assets.deleteCredentials(pluginId: "remove");

            mockHttp.VerifyNoOutstandingExpectation();
        }

        [Test]
        [Order(27)]
        [Category("API methods")]
        public void Test_AddPreset()
        {
            var mockHttp = SetupMock(MOCK_RESPONSE["addPreset"]);

            var resp = pixelbinClient.assets.addPreset(
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

            var resp = pixelbinClient.assets.getPresets();

            mockHttp.VerifyNoOutstandingExpectation();
        }

        [Test]
        [Order(29)]
        [Category("API methods")]
        public void Test_UpdatePresets()
        {
            var mockHttp = SetupMock(MOCK_RESPONSE["updatePresets"]);

            var resp = pixelbinClient.assets.updatePreset(
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

            var resp = pixelbinClient.assets.getPreset(
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

            var resp = pixelbinClient.assets.deletePreset(
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

            var resp = pixelbinClient.assets.getDefaultAssetForPlayground();

            mockHttp.VerifyNoOutstandingExpectation();
        }

        [Test]
        [Order(33)]
        [Category("API methods")]
        public void Test_GetAppOrgDetails()
        {
            var mockHttp = SetupMock(MOCK_RESPONSE["getAppOrgDetails"]);

            var resp = pixelbinClient.organization.getAppOrgDetails();

            mockHttp.VerifyNoOutstandingExpectation();
        }
    }
}