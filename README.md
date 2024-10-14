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
            // Create a config with you API_TOKEN
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

-   [API docs](https://github.com/pixelbin-dev/pixelbin-csharp-sdk)
