# Pixelbin Backend SDK for C#

Pixelbin Backend SDK for C# helps you integrate the core Pixelbin features with your application.

## Getting Started

Getting started with Pixelbin Backend SDK for C#

### Installation

```
dotnet add package pixelbin
```

---

### Usage

#### Quick Example

```csharp
using System;
using System.Collections.Generic;
using System.IO;
using Pixelbin.Platform;

namespace ExampleNamespace
{
    class ExampleClass
    {
        async void Main(string[] args)
        {
            // create client with your API_TOKEN
            PixelbinConfig config = new PixelbinConfig(
                new Dictionary<string, string>() {
                    { "domain", "https://api.pixelbin.io" },
                    { "apiSecret", "API_TOKEN" }
                });

            // Create a pixelbin instance
            PixelbinClient pixelbin = new PixelbinClient(config);

            // Sync method call
            try
            {
                var result = pixelbin.assets.listFiles();
                Console.WriteLine(result);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }

            // Async method call
            try
            {
                var result = await pixelbin.assets.listFilesAsync();
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

## Utilities

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
