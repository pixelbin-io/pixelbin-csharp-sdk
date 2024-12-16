# Pixelbin Backend SDK for C\#

Pixelbin Backend SDK for C\# helps you integrate the core Pixelbin features with your application.

## Getting Started

Getting started with Pixelbin Backend SDK for C\#

### Installation

```sh
dotnet add package pixelbin
```

---

### Usage

#### Quick Example

```csharp
using System;
using System.Collections.Generic;
using System.IO;

// import the Pixelbin Platform Namespace
using Pixelbin.Platform;

namespace ExampleNamespace
{
    class ExampleClass
    {
        async void Main(string[] args)
        {
            // Create a config with your API_TOKEN
            PixelbinConfig config = new PixelbinConfig(
                new Dictionary<string, string>() {
                    { "domain", "https://api.pixelbin.io" },
                    { "apiSecret", "API_TOKEN" }
                });

            // Create a pixelbin instance
            PixelbinClient pixelbin = new PixelbinClient(config);

            // List the assets stored on your organization's Pixelbin Storage
            try
            {
                var result = pixelbin.assets.listFiles();
                Console.WriteLine(result);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }
    }
}
```

## Uploader

### upload

Uploads a file to PixelBin with greater control over the upload process.

#### Arguments

| Argument            | Type                                                        | Required | Description                                                                                                                                                                                                                                           |
| ------------------- | ----------------------------------------------------------- | -------- | ----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------- |
| `file`              | `bytes` or `io.BufferedIOBase`                              | yes      | The file to be uploaded.                                                                                                                                                                                                                              |
| `name`              | `str`                                                       | no       | Name of the file.                                                                                                                                                                                                                                     |
| `path`              | `str`                                                       | no       | Path of the containing folder.                                                                                                                                                                                                                        |
| `format`            | `str`                                                       | no       | Format of the file.                                                                                                                                                                                                                                   |
| `access`            | [AccessEnum](./documentation/platform/ASSETS.md#accessenum) | no       | Access level of the asset, can be either `'public-read'` or `'private'`.                                                                                                                                                                              |
| `tags`              | `list[str]`                                                 | no       | Tags associated with the file.                                                                                                                                                                                                                        |
| `metadata`          | `dict`                                                      | no       | Metadata associated with the file.                                                                                                                                                                                                                    |
| `overwrite`         | `bool`                                                      | no       | Overwrite flag. If set to `True`, will overwrite any file that exists with the same path, name, and type. Defaults to `False`.                                                                                                                        |
| `filenameOverride`  | `bool`                                                      | no       | If set to `True`, will add unique characters to the name if an asset with the given name already exists. If the overwrite flag is set to `True`, preference will be given to the overwrite flag. If both are set to `False`, an error will be raised. |
| `expiry`            | `int`                                                       | no       | Expiry time in seconds for the underlying signed URL. Defaults to 3000 seconds.                                                                                                                                                                       |
| `uploadOptions`     | `dict`                                                      | no       | Additional options for fine-tuning the upload process. Default: `{ chunk_size: 10 * 1024 * 1024, max_retries: 2, concurrency: 3, exponential_factor: 2 }`.                                                                                            |
| `chunkSize`         | `int`                                                       | no       | Size of each chunk to upload. Default is 10 megabytes.                                                                                                                                                                                                |
| `maxRetries`        | `int`                                                       | no       | Maximum number of retries if an upload fails. Default is 2 retries.                                                                                                                                                                                   |
| `concurrency`       | `int`                                                       | no       | Number of concurrent chunk upload tasks. Default is 3 concurrent chunk uploads.                                                                                                                                                                       |
| `exponentialFactor` | `int`                                                       | no       | The exponential factor for retry delay. Default is 2.                                                                                                                                                                                                 |

#### Returns

`dict`: On success, returns a dictionary containing the details of the uploaded file.

| Property     | Description                                                  | Example                                                                                                                                                                                                                                                                                                                                                                                                                                    |
| ------------ | ------------------------------------------------------------ | ------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------ |
| `orgId`      | Organization ID                                              | `5320086`                                                                                                                                                                                                                                                                                                                                                                                                                                  |
| `type`       | Type of the uploaded entity                                  | `file`                                                                                                                                                                                                                                                                                                                                                                                                                                     |
| `name`       | Name of the file                                             | `testfile.jpeg`                                                                                                                                                                                                                                                                                                                                                                                                                            |
| `path`       | Path of the containing folder                                | `/path/to/image.jpeg`                                                                                                                                                                                                                                                                                                                                                                                                                      |
| `fileId`     | ID of the file                                               | `testfile.jpeg`                                                                                                                                                                                                                                                                                                                                                                                                                            |
| `access`     | Access level of the asset, can be `public-read` or `private` | `public-read`                                                                                                                                                                                                                                                                                                                                                                                                                              |
| `tags`       | Tags associated with the file                                | `["tag1", "tag2"]`                                                                                                                                                                                                                                                                                                                                                                                                                         |
| `metadata`   | Metadata associated with the file                            | `{"source":"", "publicUploadId":""}`                                                                                                                                                                                                                                                                                                                                                                                                       |
| `format`     | File format                                                  | `jpeg`                                                                                                                                                                                                                                                                                                                                                                                                                                     |
| `assetType`  | Type of asset                                                | `image`                                                                                                                                                                                                                                                                                                                                                                                                                                    |
| `size`       | File size (in bytes)                                         | `37394`                                                                                                                                                                                                                                                                                                                                                                                                                                    |
| `width`      | File width (in pixels)                                       | `720`                                                                                                                                                                                                                                                                                                                                                                                                                                      |
| `height`     | File height (in pixels)                                      | `450`                                                                                                                                                                                                                                                                                                                                                                                                                                      |
| `context`    | File metadata and additional context                         | `{"steps":[],"req":{"headers":{},"query":{}},"meta":{"format":"png","size":195337,"width":812,"height":500,"space":"srgb","channels":4,"depth":"uchar","density":144,"isProgressive":false,"resolutionUnit":"inch","hasProfile":true,"hasAlpha":true,"extension":"jpeg","contentType":"image/png","assetType":"image","isImageAsset":true,"isAudioAsset":false,"isVideoAsset":false,"isRawAsset":false,"isTransformationSupported":true}}` |
| `isOriginal` | Flag indicating if the file is original                      | `true`                                                                                                                                                                                                                                                                                                                                                                                                                                     |
| `_id`        | Record ID                                                    | `a0b0b19a-d526-4xc07-ae51-0xxxxxx`                                                                                                                                                                                                                                                                                                                                                                                                         |
| `url`        | URL of the uploaded file                                     | `https://cdn.pixelbin.io/v2/user-e26cf3/original/testfile.jpeg`                                                                                                                                                                                                                                                                                                                                                                            |

#### Example Usage

##### Uploading a Buffer

```csharp
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Pixelbin.Platform;
using static Pixelbin.Platform.Enums;

namespace PixelbinUploaderTest
{
    class Program
    {
        static async Task Main(string[] args)
        {
            // Create a config with your API_TOKEN
            var config = new PixelbinConfig(
                new Dictionary<string, string> {
                    { "domain", "https://api.pixelbin.io" },
                    { "apiSecret", "API_TOKEN" }
                };
            );
            var pixelbin = new PixelbinClient(config);
            var assets = new Assets(config);
            var uploader = new Uploader(assets);

            var filePath = Path.Combine(AppContext.BaseDirectory, "myimage.png");

            // Read the file into a buffer
            var buffer = await File.ReadAllBytesAsync(filePath);

            try
            {
                var resultBuffer = await uploader.Upload(
                    buffer,
                    "myimage",
                    "folder",
                    "png",
                    AccessEnum.PUBLIC_READ,
                    new List<string> { "test" },
                    new Dictionary<string, object>(),
                    false,
                    true,
                    1500,
                    new UploadOptions
                    {
                        ChunkSize = 5 * 1024 * 1024,   // 5MB
                        Concurrency = 2,               // 2 concurrent chunk uploads
                        MaxRetries = 1,                // 1 retry for errors that can be retried
                        ExponentialFactor = 2          // Exponential factor for retries
                    }
                );

                Console.WriteLine(JsonConvert.SerializeObject(resultBuffer));
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}
```

##### Uploading a Stream

```csharp
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Pixelbin.Platform;
using static Pixelbin.Platform.Enums;

namespace PixelbinUploaderTest
{
    class Program
    {
        static async Task Main(string[] args)
        {
            // Create a config with your API_TOKEN
            var config = new PixelbinConfig(
                new Dictionary<string, string> {
                    { "domain", "https://api.pixelbin.io" },
                    { "apiSecret", "API_TOKEN" }
                };
            );
            var pixelbin = new PixelbinClient(config);
            var assets = new Assets(config);
            var uploader = new Uploader(assets);

            var filePath = Path.Combine(AppContext.BaseDirectory, "myimage.png");

            // Open the file as a stream
            var stream = new MemoryStream(buffer);

            try
            {
                var resultStream = await uploader.Upload(
                    stream,
                    "myimage",
                    "folder",
                    "png",
                    AccessEnum.PUBLIC_READ,
                    new List<string> { "test" },
                    new Dictionary<string, object>(),
                    false,
                    true,
                    1500,
                    {
                        ChunkSize = 5 * 1024 * 1024,   // 5MB
                        Concurrency = 2,               // 2 concurrent chunk uploads
                        MaxRetries = 1,                // 1 retry for errors that can be retried
                        ExponentialFactor = 2          // Exponential factor for retries
                    }
                );

                Console.WriteLine("Upload using stream successful!");
                Console.WriteLine(JsonConvert.SerializeObject(resultStream));
            }
            catch (Exception ex)
            {
                Console.WriteLine("Upload using stream failed:");
                Console.WriteLine(ex.Message);
            }
        }
    }
}
```

## Security Utils

### For generating Signed URLs

Generate a signed PixelBin url

| Parameter                | Description                                          | Example                                                                                    |
| ------------------------ | ---------------------------------------------------- | ------------------------------------------------------------------------------------------ |
| `url` (string)           | A valid Pixelbin URL to be signed                    | `https://cdn.pixelbin.io/v2/dummy-cloudname/original/__playground/playground-default.jpeg` |
| `expirySeconds` (number) | Number of seconds the signed URL should be valid for | `20`                                                                                       |
| `accessKey` (string)     | Access key of the token used for signing             | `00000000-0000-0000-0000-000000000000`                                                     |
| `token` (string)         | Value of the token used for signing                  | `dummy-token`                                                                              |

Example:

```csharp
using Pixelbin.Security;

namespace ExampleNamespace
{
    class ExampleClass
    {
        void Main(string[] args)
        {
            string signedUrl = Security.SignURL(
                "https://cdn.pixelbin.io/v2/dummy-cloudname/original/__playground/playground-default.jpeg", // url
                20, // expirySeconds
                "0b55aaff-d7db-45f0-b556-9b45a6f2200e", // accessKey
                "dummy-token" // token
            );
            // signedUrl
            // https://cdn.pixelbin.io/v2/dummy-cloudname/original/__playground/playground-default.jpeg?pbs=8eb6a00af74e57967a42316e4de238aa88d92961649764fad1832c1bff101f25&pbe=1695635915&pbt=0b55aaff-d7db-45f0-b556-9b45a6f2200e
        }
    }
}
```

Usage with custom domain url

```csharp
using Pixelbin.Security;

namespace ExampleNamespace
{
    class ExampleClass
    {
        void Main(string[] args)
        {
            string signedUrl = Security.SignURL(
                "https://krit.imagebin.io/v2/original/__playground/playground-default.jpeg", // url
                30, // expirySeconds
                "0b55aaff-d7db-45f0-b556-9b45a6f2200e", // accessKey
                "dummy-token" // token
            );
            // signedUrl
            // https://krit.imagebin.io/v2/original/__playground/playground-default.jpeg?pbs=1aef31c1e0ecd8a875b1d3184f324327f4ab4bce419d81d1eb1a818ee5f2e3eb&pbe=1695705975&pbt=0b55aaff-d7db-45f0-b556-9b45a6f2200e
        }
    }
}
```

## URL Utils

Pixelbin provides url utilities to construct and deconstruct Pixelbin urls.

### url_to_obj

Deconstruct a pixelbin url

| parameter    | description          | example                                                                                               |
| ------------ | -------------------- | ----------------------------------------------------------------------------------------------------- |
| url (string) | A valid pixelbin url | `https://cdn.pixelbin.io/v2/your-cloud-name/z-slug/t.resize(h:100,w:200)~t.flip()/path/to/image.jpeg` |

**Returns**:

| property                                           | description                            | example                    |
| -------------------------------------------------- | -------------------------------------- | -------------------------- |
| cloudName (string)                                 | The cloudname extracted from the url   | `your-cloud-name`          |
| zone (string)                                      | 6 character zone slug                  | `z-slug`                   |
| version (string)                                   | cdn api version                        | `v2`                       |
| options (Dictionary<string, object>)               | optional query parameters              |                            |
| transformations (List<Dictionary<string, object>>) | Extracted transformations from the url |                            |
| filePath (string)                                  | Path to the file on Pixelbin storage   | `/path/to/image.jpeg`      |
| baseUrl (string)                                   | Base url                               | `https://cdn.pixelbin.io/` |

Example:

```csharp
using System;
using System.Collections.Generic;
using System.IO;
using Pixelbin.Utils;

namespace ExampleNamespace
{
    class ExampleClass
    {
        void Main(string[] args)
        {
            string pixelbinUrl = "https://cdn.pixelbin.io/v2/your-cloud-name/z-slug/t.resize(h:100,w:200)~t.flip()/path/to/image.jpeg?dpr=2.0&f_auto=True";

            UrlObj obj = Url.UrlToObj(pixelbinUrl);
            // returned obj schema when serialized to json
            // {
            //     "cloudName": "your-cloud-name",
            //     "zone": "z-slug",
            //     "version": "v2",
            //     "options": {
            //         "dpr": 2.0,
            //         "f_auto": True,
            //     },
            //     "transformations": [
            //         {
            //             "plugin": "t",
            //             "name": "resize",
            //             "values": [
            //                 {
            //                     "key": "h",
            //                     "value": "100"
            //                 },
            //                 {
            //                     "key": "w",
            //                     "value": "200"
            //                 }
            //             ]
            //         },
            //         {
            //             "plugin": "t",
            //             "name": "flip",
            //         }
            //     ],
            //     "filePath": "path/to/image.jpeg",
            //     "baseUrl": "https://cdn.pixelbin.io"
            // }
        }
    }
}
```

### obj_to_url

Converts the extracted url obj to a Pixelbin url.

| property                                           | description                            | example                    |
| -------------------------------------------------- | -------------------------------------- | -------------------------- |
| cloudName (string)                                 | The cloudname extracted from the url   | `your-cloud-name`          |
| zone (string)                                      | 6 character zone slug                  | `z-slug`                   |
| version (string)                                   | cdn api version                        | `v2`                       |
| options (Dictionary<string, object>)               | optional query parameters              |                            |
| transformations (List<Dictionary<string, object>>) | Extracted transformations from the url |                            |
| filePath (string)                                  | Path to the file on Pixelbin storage   | `/path/to/image.jpeg`      |
| baseUrl (string)                                   | Base url                               | `https://cdn.pixelbin.io/` |

```csharp
using System;
using System.Collections.Generic;
using System.IO;
using Pixelbin.Utils;

namespace ExampleNamespace
{
    class ExampleClass
    {
        void Main(string[] args)
        {
            UrlObj obj = new UrlObj (
                version: "v2",
                cloudName: "your-cloud-name",
                filePath: "path/to/image.jpeg",
                options: new UrlObjOptions() {
                    dpr: 2.0,
                    f_auto: true
                },
                zone: "z-slug",
                baseUrl: "https://cdn.pixelbin.io",
                transformations: new List<UrlTransformation>() {
                    new UrlTransformation(
                        plugin: "t",
                        name: "resize",
                        values: new List<Dictionary<string, string>>() {
                            new Dictionary<string, string>() { { "key", "h" }, { "value", "200" } },
                            new Dictionary<string, string>() { { "key", "w" }, { "value", "200" } }
                        }
                    ),
                    new UrlTransformation(
                        plugin: "t",
                        name: "flip"
                    )
                }
            );

            string url = obj_to_url(obj)
            // returned url string
            // https://cdn.pixelbin.io/v2/your-cloud-name/z-slug/t.resize(h:100,w:200)~t.flip()/path/to/image.jpeg?dpr=2.0&f_auto=True
        }
    }
}
```

## Documentation

-   [API docs](https://github.com/pixelbin-io/pixelbin-csharp-sdk/blob/main/documentation/platform/README.md)
