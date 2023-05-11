##### [Back to Pixelbin API docs](./README.md)

## Assets Methods

Asset Uploader Service

-   [fileUpload](#fileupload)
-   [urlUpload](#urlupload)
-   [createSignedUrl](#createsignedurl)
-   [listFiles](#listfiles)
-   [listFilesPaginator](#listfilespaginator)
-   [getFileById](#getfilebyid)
-   [getFileByFileId](#getfilebyfileid)
-   [updateFile](#updatefile)
-   [deleteFile](#deletefile)
-   [deleteFiles](#deletefiles)
-   [createFolder](#createfolder)
-   [getFolderDetails](#getfolderdetails)
-   [updateFolder](#updatefolder)
-   [deleteFolder](#deletefolder)
-   [getFolderAncestors](#getfolderancestors)
-   [addCredentials](#addcredentials)
-   [updateCredentials](#updatecredentials)
-   [deleteCredentials](#deletecredentials)
-   [addPreset](#addpreset)
-   [getPresets](#getpresets)
-   [updatePreset](#updatepreset)
-   [deletePreset](#deletepreset)
-   [getPreset](#getpreset)
-   [getDefaultAssetForPlayground](#getdefaultassetforplayground)
-   [getModules](#getmodules)
-   [getModule](#getmodule)

## Methods with example and description

### fileUpload

**Summary**: Upload File

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
            PixelbinConfig config = new PixelbinConfig(
                new Dictionary<string, string>() {
                    { "domain", "https://api.pixelbin.io" },
                    { "apiSecret", "API_SECRECT_TOKEN" }
                });

            PixelbinClient pixelbin = new PixelbinClient(config);

            // Sync method call
            try
            {
                var result = pixelbin.assets.fileUpload(
                    file: new FileStream("your-file-path", FileMode.Open),
                    path: path/to/containing/folder,
                    name: filename,
                    access: public-read,
                    tags: new List<string>() { "tag1", "tag2" },
                    metadata: new Dictionary<string, object> {  },
                    overwrite: False,
                    filenameOverride: True
                );
                // use result
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }

            // Async method call
            try
            {
                var result = await pixelbin.assets.fileUploadAsync(
                    file: new FileStream("your-file-path", FileMode.Open),
                    path: path/to/containing/folder,
                    name: filename,
                    access: public-read,
                    tags: new List<string>() { "tag1", "tag2" },
                    metadata: new Dictionary<string, object> {  },
                    overwrite: False,
                    filenameOverride: True
                );
                // use result
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }
    }
}
```

| Argument         | Type                       | Required | Description                                                                                                                                                                                                                      |
| ---------------- | -------------------------- | -------- | -------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------- |
| file             | FileStream                 | yes      | Asset file                                                                                                                                                                                                                       |
| path             | string                     | no       | Path where you want to store the asset. Path of containing folder                                                                                                                                                                |
| name             | string                     | no       | Name of the asset, if not provided name of the file will be used. Note - The provided name will be slugified to make it URL safe                                                                                                 |
| access           | AccessEnum                 | no       | Access level of asset, can be either `public-read` or `private`                                                                                                                                                                  |
| tags             | List<string>               | no       | Asset tags                                                                                                                                                                                                                       |
| metadata         | Dictionary<string, object> | no       | Asset related metadata                                                                                                                                                                                                           |
| overwrite        | bool                       | no       | Overwrite flag. If set to `true` will overwrite any file that exists with same path, name and type. Defaults to `false`.                                                                                                         |
| filenameOverride | bool                       | no       | If set to `true` will add unique characters to name if asset with given name already exists. If overwrite flag is set to `true`, preference will be given to overwrite flag. If both are set to `false` an error will be raised. |

Upload File to Pixelbin

_Returned Response:_

[UploadResponse](#uploadresponse)

Success

<details>
<summary><i>&nbsp; Example:</i></summary>

```json
{
    "_id": "dummy-uuid",
    "name": "asset",
    "path": "dir",
    "fileId": "dir/asset",
    "format": "jpeg",
    "size": 1000,
    "access": "private",
    "isActive": true,
    "tags": ["tag1", "tag2"],
    "metadata": {
        "key": "value"
    },
    "url": "https://domain.com/filename.jpeg"
}
```

</details>

### urlUpload

**Summary**: Upload Asset with url

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
            PixelbinConfig config = new PixelbinConfig(
                new Dictionary<string, string>() {
                    { "domain", "https://api.pixelbin.io" },
                    { "apiSecret", "API_SECRECT_TOKEN" }
                });

            PixelbinClient pixelbin = new PixelbinClient(config);

            // Sync method call
            try
            {
                var result = pixelbin.assets.urlUpload(
                    url: www.dummy.com/image.png,
                    path: path/to/containing/folder,
                    name: filename,
                    access: public-read,
                    tags: new List<string>() { "tag1", "tag2" },
                    metadata: new Dictionary<string, object> {  },
                    overwrite: False,
                    filenameOverride: True
                );
                // use result
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }

            // Async method call
            try
            {
                var result = await pixelbin.assets.urlUploadAsync(
                    url: www.dummy.com/image.png,
                    path: path/to/containing/folder,
                    name: filename,
                    access: public-read,
                    tags: new List<string>() { "tag1", "tag2" },
                    metadata: new Dictionary<string, object> {  },
                    overwrite: False,
                    filenameOverride: True
                );
                // use result
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }
    }
}
```

| Argument         | Type                       | Required | Description                                                                                                                                                                                                                      |
| ---------------- | -------------------------- | -------- | -------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------- |
| url              | string                     | yes      | Asset URL                                                                                                                                                                                                                        |
| path             | string                     | no       | Path where you want to store the asset. Path of containing folder.                                                                                                                                                               |
| name             | string                     | no       | Name of the asset, if not provided name of the file will be used. Note - The provided name will be slugified to make it URL safe                                                                                                 |
| access           | AccessEnum                 | no       | Access level of asset, can be either `public-read` or `private`                                                                                                                                                                  |
| tags             | List<string>               | no       | Asset tags                                                                                                                                                                                                                       |
| metadata         | Dictionary<string, object> | no       | Asset related metadata                                                                                                                                                                                                           |
| overwrite        | bool                       | no       | Overwrite flag. If set to `true` will overwrite any file that exists with same path, name and type. Defaults to `false`.                                                                                                         |
| filenameOverride | bool                       | no       | If set to `true` will add unique characters to name if asset with given name already exists. If overwrite flag is set to `true`, preference will be given to overwrite flag. If both are set to `false` an error will be raised. |

Upload Asset with url

_Returned Response:_

[UploadResponse](#uploadresponse)

Success

<details>
<summary><i>&nbsp; Example:</i></summary>

```json
{
    "_id": "dummy-uuid",
    "name": "asset",
    "path": "dir",
    "fileId": "dir/asset",
    "format": "jpeg",
    "size": 1000,
    "access": "private",
    "isActive": true,
    "tags": ["tag1", "tag2"],
    "metadata": {
        "key": "value"
    },
    "url": "https://domain.com/filename.jpeg"
}
```

</details>

### createSignedUrl

**Summary**: S3 Signed URL upload

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
            PixelbinConfig config = new PixelbinConfig(
                new Dictionary<string, string>() {
                    { "domain", "https://api.pixelbin.io" },
                    { "apiSecret", "API_SECRECT_TOKEN" }
                });

            PixelbinClient pixelbin = new PixelbinClient(config);

            // Sync method call
            try
            {
                var result = pixelbin.assets.createSignedUrl(
                    name: filename,
                    path: path/to/containing/folder,
                    format: jpeg,
                    access: public-read,
                    tags: new List<string>() { "tag1", "tag2" },
                    metadata: new Dictionary<string, object> {  },
                    overwrite: False,
                    filenameOverride: True
                );
                // use result
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }

            // Async method call
            try
            {
                var result = await pixelbin.assets.createSignedUrlAsync(
                    name: filename,
                    path: path/to/containing/folder,
                    format: jpeg,
                    access: public-read,
                    tags: new List<string>() { "tag1", "tag2" },
                    metadata: new Dictionary<string, object> {  },
                    overwrite: False,
                    filenameOverride: True
                );
                // use result
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }
    }
}
```

| Argument         | Type                       | Required | Description                                                                                                                                                                                                                      |
| ---------------- | -------------------------- | -------- | -------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------- |
| name             | string                     | no       | name of the file                                                                                                                                                                                                                 |
| path             | string                     | no       | Path of containing folder.                                                                                                                                                                                                       |
| format           | string                     | no       | Format of the file                                                                                                                                                                                                               |
| access           | AccessEnum                 | no       | Access level of asset, can be either `public-read` or `private`                                                                                                                                                                  |
| tags             | List<string>               | no       | Tags associated with the file.                                                                                                                                                                                                   |
| metadata         | Dictionary<string, object> | no       | Metadata associated with the file.                                                                                                                                                                                               |
| overwrite        | bool                       | no       | Overwrite flag. If set to `true` will overwrite any file that exists with same path, name and type. Defaults to `false`.                                                                                                         |
| filenameOverride | bool                       | no       | If set to `true` will add unique characters to name if asset with given name already exists. If overwrite flag is set to `true`, preference will be given to overwrite flag. If both are set to `false` an error will be raised. |

For the given asset details, a S3 signed URL will be generated,
which can be then used to upload your asset.

_Returned Response:_

[SignedUploadResponse](#signeduploadresponse)

Success

<details>
<summary><i>&nbsp; Example:</i></summary>

```json
{
    "s3PresignedUrl": {
        "url": "https://domain.com/xyz",
        "fields": {
            "field1": "value",
            "field2": "value"
        }
    }
}
```

</details>

### listFiles

**Summary**: List and search files and folders.

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
            PixelbinConfig config = new PixelbinConfig(
                new Dictionary<string, string>() {
                    { "domain", "https://api.pixelbin.io" },
                    { "apiSecret", "API_SECRECT_TOKEN" }
                });

            PixelbinClient pixelbin = new PixelbinClient(config);

            // Sync method call
            try
            {
                var result = pixelbin.assets.listFiles(
                    name: "cat",
                    path: "cat-photos",
                    format: "jpeg",
                    tags: new List<string>() {"cats","animals"},
                    onlyFiles: "false",
                    onlyFolders: "false",
                    pageNo: 1,
                    pageSize: 10,
                    sort: "name"
                );
                // use result
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }

            // Async method call
            try
            {
                var result = await pixelbin.assets.listFilesAsync(
                    name: "cat",
                    path: "cat-photos",
                    format: "jpeg",
                    tags: new List<string>() {"cats","animals"},
                    onlyFiles: "false",
                    onlyFolders: "false",
                    pageNo: 1,
                    pageSize: 10,
                    sort: "name"
                );
                // use result
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }
    }
}
```

| Argument    | Type         | Required | Description                                                                  |
| ----------- | ------------ | -------- | ---------------------------------------------------------------------------- |
| name        | string       | no       | Find items with matching name                                                |
| path        | string       | no       | Find items with matching path                                                |
| format      | string       | no       | Find items with matching format                                              |
| tags        | List<string> | no       | Find items containing these tags                                             |
| onlyFiles   | bool         | no       | If true will fetch only files                                                |
| onlyFolders | bool         | no       | If true will fetch only folders                                              |
| pageNo      | int          | no       | Page No.                                                                     |
| pageSize    | int          | no       | Page Size                                                                    |
| sort        | string       | no       | Key to sort results by. A "-" suffix will sort results in descending orders. |

List all files and folders in root folder. Search for files if name is provided. If path is provided, search in the specified path.

_Returned Response:_

[ListFilesResponse](#listfilesresponse)

Success

<details>
<summary><i>&nbsp; Example:</i></summary>

```json
{
    "items": [
        {
            "_id": "dummy-uuid",
            "name": "dir",
            "type": "folder"
        },
        {
            "_id": "dummy-uuid",
            "name": "asset2",
            "type": "file",
            "path": "dir",
            "fileId": "dir/asset2",
            "format": "jpeg",
            "size": 1000,
            "access": "private"
        },
        {
            "_id": "dummy-uuid",
            "name": "asset1",
            "type": "file",
            "path": "dir",
            "fileId": "dir/asset1",
            "format": "jpeg",
            "size": 1000,
            "access": "private"
        }
    ],
    "page": {
        "type": "number",
        "size": 4,
        "current": 1,
        "hasNext": false
    }
}
```

</details>

### getFileById

**Summary**: Get file details with \_id

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
            PixelbinConfig config = new PixelbinConfig(
                new Dictionary<string, string>() {
                    { "domain", "https://api.pixelbin.io" },
                    { "apiSecret", "API_SECRECT_TOKEN" }
                });

            PixelbinClient pixelbin = new PixelbinClient(config);

            // Sync method call
            try
            {
                var result = pixelbin.assets.getFileById(
                    _id: "c9138153-94ea-4dbe-bea9-65d43dba85ae"
                );
                // use result
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }

            // Async method call
            try
            {
                var result = await pixelbin.assets.getFileByIdAsync(
                    _id: "c9138153-94ea-4dbe-bea9-65d43dba85ae"
                );
                // use result
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }
    }
}
```

| Argument | Type   | Required | Description  |
| -------- | ------ | -------- | ------------ |
| \_id     | string | yes      | \_id of File |

_Returned Response:_

[FilesResponse](#filesresponse)

Success

<details>
<summary><i>&nbsp; Example:</i></summary>

```json
{
    "_id": "dummy-uuid",
    "name": "asset",
    "path": "dir",
    "fileId": "dir/asset",
    "format": "jpeg",
    "size": 1000,
    "access": "private",
    "isActive": true,
    "tags": ["tag1", "tag2"],
    "metadata": {
        "key": "value"
    },
    "url": "https://domain.com/filename.jpeg"
}
```

</details>

### getFileByFileId

**Summary**: Get file details with fileId

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
            PixelbinConfig config = new PixelbinConfig(
                new Dictionary<string, string>() {
                    { "domain", "https://api.pixelbin.io" },
                    { "apiSecret", "API_SECRECT_TOKEN" }
                });

            PixelbinClient pixelbin = new PixelbinClient(config);

            // Sync method call
            try
            {
                var result = pixelbin.assets.getFileByFileId(
                    fileId: "path/to/file/name"
                );
                // use result
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }

            // Async method call
            try
            {
                var result = await pixelbin.assets.getFileByFileIdAsync(
                    fileId: "path/to/file/name"
                );
                // use result
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }
    }
}
```

| Argument | Type   | Required | Description                              |
| -------- | ------ | -------- | ---------------------------------------- |
| fileId   | string | yes      | Combination of `path` and `name` of file |

_Returned Response:_

[FilesResponse](#filesresponse)

Success

<details>
<summary><i>&nbsp; Example:</i></summary>

```json
{
    "_id": "dummy-uuid",
    "name": "asset",
    "path": "dir",
    "fileId": "dir/asset",
    "format": "jpeg",
    "size": 1000,
    "access": "private",
    "isActive": true,
    "tags": ["tag1", "tag2"],
    "metadata": {
        "key": "value"
    },
    "url": "https://domain.com/filename.jpeg"
}
```

</details>

### updateFile

**Summary**: Update file details

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
            PixelbinConfig config = new PixelbinConfig(
                new Dictionary<string, string>() {
                    { "domain", "https://api.pixelbin.io" },
                    { "apiSecret", "API_SECRECT_TOKEN" }
                });

            PixelbinClient pixelbin = new PixelbinClient(config);

            // Sync method call
            try
            {
                var result = pixelbin.assets.updateFile(
                    fileId: "path/to/file/name",
                    name: asset,
                    path: dir,
                    access: private,
                    isActive: False,
                    tags: new List<string>() { "tag1", "tag2" },
                    metadata: new Dictionary<string, object> { { "key", "value" } }
                );
                // use result
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }

            // Async method call
            try
            {
                var result = await pixelbin.assets.updateFileAsync(
                    fileId: "path/to/file/name",
                    name: asset,
                    path: dir,
                    access: private,
                    isActive: False,
                    tags: new List<string>() { "tag1", "tag2" },
                    metadata: new Dictionary<string, object> { { "key", "value" } }
                );
                // use result
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }
    }
}
```

| Argument | Type                       | Required | Description                                                     |
| -------- | -------------------------- | -------- | --------------------------------------------------------------- |
| fileId   | string                     | yes      | Combination of `path` and `name`                                |
| name     | string                     | no       | Name of the file                                                |
| path     | string                     | no       | path of containing folder.                                      |
| access   | string                     | no       | Access level of asset, can be either `public-read` or `private` |
| isActive | bool                       | no       | Whether the file is active                                      |
| tags     | List<string>               | no       | Tags associated with the file                                   |
| metadata | Dictionary<string, object> | no       | Metadata associated with the file                               |

_Returned Response:_

[FilesResponse](#filesresponse)

Success

<details>
<summary><i>&nbsp; Example:</i></summary>

```json
{
    "_id": "dummy-uuid",
    "name": "asset",
    "path": "dir",
    "fileId": "dir/asset",
    "format": "jpeg",
    "size": 1000,
    "access": "private",
    "isActive": true,
    "tags": ["tag1", "tag2"],
    "metadata": {
        "key": "value"
    },
    "url": "https://domain.com/filename.jpeg"
}
```

</details>

### deleteFile

**Summary**: Delete file

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
            PixelbinConfig config = new PixelbinConfig(
                new Dictionary<string, string>() {
                    { "domain", "https://api.pixelbin.io" },
                    { "apiSecret", "API_SECRECT_TOKEN" }
                });

            PixelbinClient pixelbin = new PixelbinClient(config);

            // Sync method call
            try
            {
                var result = pixelbin.assets.deleteFile(
                    fileId: "path/to/file/name"
                );
                // use result
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }

            // Async method call
            try
            {
                var result = await pixelbin.assets.deleteFileAsync(
                    fileId: "path/to/file/name"
                );
                // use result
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }
    }
}
```

| Argument | Type   | Required | Description                      |
| -------- | ------ | -------- | -------------------------------- |
| fileId   | string | yes      | Combination of `path` and `name` |

_Returned Response:_

[FilesResponse](#filesresponse)

Success

<details>
<summary><i>&nbsp; Example:</i></summary>

```json
{
    "_id": "dummy-uuid",
    "name": "asset",
    "path": "dir",
    "fileId": "dir/asset",
    "format": "jpeg",
    "size": 1000,
    "access": "private",
    "isActive": true,
    "tags": ["tag1", "tag2"],
    "metadata": {
        "key": "value"
    },
    "url": "https://domain.com/filename.jpeg"
}
```

</details>

### deleteFiles

**Summary**: Delete multiple files

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
            PixelbinConfig config = new PixelbinConfig(
                new Dictionary<string, string>() {
                    { "domain", "https://api.pixelbin.io" },
                    { "apiSecret", "API_SECRECT_TOKEN" }
                });

            PixelbinClient pixelbin = new PixelbinClient(config);

            // Sync method call
            try
            {
                var result = pixelbin.assets.deleteFiles(
                    ids: new List<string>() { "_id_1", "_id_2", "_id_3" }
                );
                // use result
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }

            // Async method call
            try
            {
                var result = await pixelbin.assets.deleteFilesAsync(
                    ids: new List<string>() { "_id_1", "_id_2", "_id_3" }
                );
                // use result
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }
    }
}
```

| Argument | Type         | Required | Description                   |
| -------- | ------------ | -------- | ----------------------------- |
| ids      | List<string> | yes      | Array of file \_ids to delete |

_Returned Response:_

[List<FilesResponse>](#list<filesresponse>)

Success

<details>
<summary><i>&nbsp; Example:</i></summary>

```json
[
    {
        "_id": "dummy-uuid",
        "name": "asset",
        "path": "dir",
        "fileId": "dir/asset",
        "format": "jpeg",
        "size": 1000,
        "access": "private",
        "isActive": true,
        "tags": ["tag1", "tag2"],
        "metadata": {
            "key": "value"
        },
        "url": "https://domain.com/filename.jpeg"
    }
]
```

</details>

### createFolder

**Summary**: Create folder

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
            PixelbinConfig config = new PixelbinConfig(
                new Dictionary<string, string>() {
                    { "domain", "https://api.pixelbin.io" },
                    { "apiSecret", "API_SECRECT_TOKEN" }
                });

            PixelbinClient pixelbin = new PixelbinClient(config);

            // Sync method call
            try
            {
                var result = pixelbin.assets.createFolder(
                    name: subDir,
                    path: dir
                );
                // use result
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }

            // Async method call
            try
            {
                var result = await pixelbin.assets.createFolderAsync(
                    name: subDir,
                    path: dir
                );
                // use result
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }
    }
}
```

| Argument | Type   | Required | Description                |
| -------- | ------ | -------- | -------------------------- |
| name     | string | yes      | Name of the folder         |
| path     | string | no       | path of containing folder. |

Create a new folder at the specified path. Also creates the ancestors if they do not exist.

_Returned Response:_

[FoldersResponse](#foldersresponse)

Success - List of all created folders

<details>
<summary><i>&nbsp; Example:</i></summary>

```json
{
    "_id": "dummy-uuid",
    "name": "subDir",
    "path": "dir",
    "isActive": true
}
```

</details>

### getFolderDetails

**Summary**: Get folder details

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
            PixelbinConfig config = new PixelbinConfig(
                new Dictionary<string, string>() {
                    { "domain", "https://api.pixelbin.io" },
                    { "apiSecret", "API_SECRECT_TOKEN" }
                });

            PixelbinClient pixelbin = new PixelbinClient(config);

            // Sync method call
            try
            {
                var result = pixelbin.assets.getFolderDetails(
                    path: "dir1/dir2",
                    name: "dir"
                );
                // use result
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }

            // Async method call
            try
            {
                var result = await pixelbin.assets.getFolderDetailsAsync(
                    path: "dir1/dir2",
                    name: "dir"
                );
                // use result
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }
    }
}
```

| Argument | Type   | Required | Description |
| -------- | ------ | -------- | ----------- |
| path     | string | no       | Folder path |
| name     | string | no       | Folder name |

Get folder details

_Returned Response:_

[ExploreItem](#exploreitem)

Success

<details>
<summary><i>&nbsp; Example:</i></summary>

```json
[
    {
        "_id": "dummy-uuid",
        "createdAt": "2022-10-05T10:43:04.117Z",
        "updatedAt": "2022-10-05T10:43:04.117Z",
        "name": "asset2",
        "type": "file",
        "path": "dir",
        "fileId": "dir/asset2",
        "format": "jpeg",
        "size": 1000,
        "access": "private",
        "metadata": {},
        "height": 100,
        "width": 100
    }
]
```

</details>

### updateFolder

**Summary**: Update folder details

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
            PixelbinConfig config = new PixelbinConfig(
                new Dictionary<string, string>() {
                    { "domain", "https://api.pixelbin.io" },
                    { "apiSecret", "API_SECRECT_TOKEN" }
                });

            PixelbinClient pixelbin = new PixelbinClient(config);

            // Sync method call
            try
            {
                var result = pixelbin.assets.updateFolder(
                    folderId: "path/to/folder/name",
                    isActive: False
                );
                // use result
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }

            // Async method call
            try
            {
                var result = await pixelbin.assets.updateFolderAsync(
                    folderId: "path/to/folder/name",
                    isActive: False
                );
                // use result
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }
    }
}
```

| Argument | Type   | Required | Description                      |
| -------- | ------ | -------- | -------------------------------- |
| folderId | string | yes      | combination of `path` and `name` |
| isActive | bool   | no       | whether the folder is active     |

Update folder details. Eg: Soft delete it
by making `isActive` as `false`.
We currently do not support updating folder name or path.

_Returned Response:_

[FoldersResponse](#foldersresponse)

Success

<details>
<summary><i>&nbsp; Example:</i></summary>

```json
{
    "_id": "dummy-uuid",
    "name": "subDir",
    "path": "dir",
    "isActive": true
}
```

</details>

### deleteFolder

**Summary**: Delete folder

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
            PixelbinConfig config = new PixelbinConfig(
                new Dictionary<string, string>() {
                    { "domain", "https://api.pixelbin.io" },
                    { "apiSecret", "API_SECRECT_TOKEN" }
                });

            PixelbinClient pixelbin = new PixelbinClient(config);

            // Sync method call
            try
            {
                var result = pixelbin.assets.deleteFolder(
                    _id: "c9138153-94ea-4dbe-bea9-65d43dba85ae"
                );
                // use result
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }

            // Async method call
            try
            {
                var result = await pixelbin.assets.deleteFolderAsync(
                    _id: "c9138153-94ea-4dbe-bea9-65d43dba85ae"
                );
                // use result
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }
    }
}
```

| Argument | Type   | Required | Description                  |
| -------- | ------ | -------- | ---------------------------- |
| \_id     | string | yes      | \_id of folder to be deleted |

Delete folder and all its children permanently.

_Returned Response:_

[FoldersResponse](#foldersresponse)

Success

<details>
<summary><i>&nbsp; Example:</i></summary>

```json
{
    "_id": "dummy-uuid",
    "name": "subDir",
    "path": "dir",
    "isActive": true
}
```

</details>

### getFolderAncestors

**Summary**: Get all ancestors of a folder

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
            PixelbinConfig config = new PixelbinConfig(
                new Dictionary<string, string>() {
                    { "domain", "https://api.pixelbin.io" },
                    { "apiSecret", "API_SECRECT_TOKEN" }
                });

            PixelbinClient pixelbin = new PixelbinClient(config);

            // Sync method call
            try
            {
                var result = pixelbin.assets.getFolderAncestors(
                    _id: "c9138153-94ea-4dbe-bea9-65d43dba85ae"
                );
                // use result
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }

            // Async method call
            try
            {
                var result = await pixelbin.assets.getFolderAncestorsAsync(
                    _id: "c9138153-94ea-4dbe-bea9-65d43dba85ae"
                );
                // use result
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }
    }
}
```

| Argument | Type   | Required | Description        |
| -------- | ------ | -------- | ------------------ |
| \_id     | string | yes      | \_id of the folder |

Get all ancestors of a folder, using the folder ID.

_Returned Response:_

[GetAncestorsResponse](#getancestorsresponse)

Success

<details>
<summary><i>&nbsp; Example:</i></summary>

```json
{
    "folder": {
        "_id": "dummy-uuid",
        "name": "subDir",
        "path": "dir1/dir2",
        "isActive": true
    },
    "ancestors": [
        {
            "_id": "dummy-uuid-2",
            "name": "dir1",
            "path": "",
            "isActive": true
        },
        {
            "_id": "dummy-uuid-2",
            "name": "dir2",
            "path": "dir1",
            "isActive": true
        }
    ]
}
```

</details>

### addCredentials

**Summary**: Add credentials for a transformation module.

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
            PixelbinConfig config = new PixelbinConfig(
                new Dictionary<string, string>() {
                    { "domain", "https://api.pixelbin.io" },
                    { "apiSecret", "API_SECRECT_TOKEN" }
                });

            PixelbinClient pixelbin = new PixelbinClient(config);

            // Sync method call
            try
            {
                var result = pixelbin.assets.addCredentials(
                    credentials: new Dictionary<string, object> { { "region", "ap-south-1" }, { "accessKeyId", "123456789ABC" }, { "secretAccessKey", "DUMMY1234567890" } },
                    pluginId: awsRek
                );
                // use result
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }

            // Async method call
            try
            {
                var result = await pixelbin.assets.addCredentialsAsync(
                    credentials: new Dictionary<string, object> { { "region", "ap-south-1" }, { "accessKeyId", "123456789ABC" }, { "secretAccessKey", "DUMMY1234567890" } },
                    pluginId: awsRek
                );
                // use result
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }
    }
}
```

| Argument    | Type                       | Required | Description                                                 |
| ----------- | -------------------------- | -------- | ----------------------------------------------------------- |
| credentials | Dictionary<string, object> | yes      | Credentials of the plugin                                   |
| pluginId    | string                     | yes      | Unique identifier for the plugin this credential belongs to |

Add a transformation modules's credentials for an organization.

_Returned Response:_

[AddCredentialsResponse](#addcredentialsresponse)

Success

<details>
<summary><i>&nbsp; Example:</i></summary>

```json
{
    "_id": "123ee789-7ae8-4336-b9bd-e4f33c049002",
    "createdAt": "2022-10-04T09:52:09.545Z",
    "updatedAt": "2022-10-04T09:52:09.545Z",
    "orgId": 23,
    "pluginId": "awsRek"
}
```

</details>

### updateCredentials

**Summary**: Update credentials of a transformation module.

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
            PixelbinConfig config = new PixelbinConfig(
                new Dictionary<string, string>() {
                    { "domain", "https://api.pixelbin.io" },
                    { "apiSecret", "API_SECRECT_TOKEN" }
                });

            PixelbinClient pixelbin = new PixelbinClient(config);

            // Sync method call
            try
            {
                var result = pixelbin.assets.updateCredentials(
                    pluginId: "awsRek",
                    credentials: new Dictionary<string, object> { { "region", "ap-south-1" }, { "accessKeyId", "123456789ABC" }, { "secretAccessKey", "DUMMY1234567890" } }
                );
                // use result
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }

            // Async method call
            try
            {
                var result = await pixelbin.assets.updateCredentialsAsync(
                    pluginId: "awsRek",
                    credentials: new Dictionary<string, object> { { "region", "ap-south-1" }, { "accessKeyId", "123456789ABC" }, { "secretAccessKey", "DUMMY1234567890" } }
                );
                // use result
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }
    }
}
```

| Argument    | Type                       | Required | Description                                          |
| ----------- | -------------------------- | -------- | ---------------------------------------------------- |
| pluginId    | string                     | yes      | ID of the plugin whose credentials are being updated |
| credentials | Dictionary<string, object> | yes      | Credentials of the plugin                            |

Update credentials of a transformation module, for an organization.

_Returned Response:_

[AddCredentialsResponse](#addcredentialsresponse)

Success

<details>
<summary><i>&nbsp; Example:</i></summary>

```json
{
    "_id": "123ee789-7ae8-4336-b9bd-e4f33c049002",
    "createdAt": "2022-10-04T09:52:09.545Z",
    "updatedAt": "2022-10-04T09:52:09.545Z",
    "orgId": 23,
    "pluginId": "awsRek"
}
```

</details>

### deleteCredentials

**Summary**: Delete credentials of a transformation module.

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
            PixelbinConfig config = new PixelbinConfig(
                new Dictionary<string, string>() {
                    { "domain", "https://api.pixelbin.io" },
                    { "apiSecret", "API_SECRECT_TOKEN" }
                });

            PixelbinClient pixelbin = new PixelbinClient(config);

            // Sync method call
            try
            {
                var result = pixelbin.assets.deleteCredentials(
                    pluginId: "awsRek"
                );
                // use result
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }

            // Async method call
            try
            {
                var result = await pixelbin.assets.deleteCredentialsAsync(
                    pluginId: "awsRek"
                );
                // use result
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }
    }
}
```

| Argument | Type   | Required | Description                                          |
| -------- | ------ | -------- | ---------------------------------------------------- |
| pluginId | string | yes      | ID of the plugin whose credentials are being deleted |

Delete credentials of a transformation module, for an organization.

_Returned Response:_

[AddCredentialsResponse](#addcredentialsresponse)

Success

<details>
<summary><i>&nbsp; Example:</i></summary>

```json
{
    "_id": "123ee789-7ae8-4336-b9bd-e4f33c049002",
    "createdAt": "2022-10-04T09:52:09.545Z",
    "updatedAt": "2022-10-04T09:52:09.545Z",
    "orgId": 23,
    "pluginId": "awsRek"
}
```

</details>

### addPreset

**Summary**: Add a preset.

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
            PixelbinConfig config = new PixelbinConfig(
                new Dictionary<string, string>() {
                    { "domain", "https://api.pixelbin.io" },
                    { "apiSecret", "API_SECRECT_TOKEN" }
                });

            PixelbinClient pixelbin = new PixelbinClient(config);

            // Sync method call
            try
            {
                var result = pixelbin.assets.addPreset(
                    presetName: p1,
                    transformation: t.flip()~t.flop(),
                    params: new Dictionary<string, object> { { "w", new Dictionary<string, object> { { "type", "integer" }, { "default", 200 } } }, { "h", new Dictionary<string, object> { { "type", "integer" }, { "default", 400 } } } }
                );
                // use result
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }

            // Async method call
            try
            {
                var result = await pixelbin.assets.addPresetAsync(
                    presetName: p1,
                    transformation: t.flip()~t.flop(),
                    params: new Dictionary<string, object> { { "w", new Dictionary<string, object> { { "type", "integer" }, { "default", 200 } } }, { "h", new Dictionary<string, object> { { "type", "integer" }, { "default", 400 } } } }
                );
                // use result
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }
    }
}
```

| Argument       | Type                       | Required | Description                                    |
| -------------- | -------------------------- | -------- | ---------------------------------------------- |
| presetName     | string                     | yes      | Name of the preset                             |
| transformation | string                     | yes      | A chain of transformations, separated by `~`   |
| params         | Dictionary<string, object> | no       | Parameters object for transformation variables |

Add a preset for an organization.

_Returned Response:_

[AddPresetResponse](#addpresetresponse)

Success

<details>
<summary><i>&nbsp; Example:</i></summary>

```json
{
    "presetName": "p1",
    "transformation": "t.flip()~t.flop()",
    "params": {
        "w": {
            "type": "integer",
            "default": 200
        },
        "h": {
            "type": "integer",
            "default": 400
        }
    },
    "archived": false
}
```

</details>

### getPresets

**Summary**: Get all presets.

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
            PixelbinConfig config = new PixelbinConfig(
                new Dictionary<string, string>() {
                    { "domain", "https://api.pixelbin.io" },
                    { "apiSecret", "API_SECRECT_TOKEN" }
                });

            PixelbinClient pixelbin = new PixelbinClient(config);

            // Sync method call
            try
            {
                var result = pixelbin.assets.getPresets();
                // use result
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }

            // Async method call
            try
            {
                var result = await pixelbin.assets.getPresetsAsync();
                // use result
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }
    }
}
```

Get all presets of an organization.

_Returned Response:_

[AddPresetResponse](#addpresetresponse)

Success

<details>
<summary><i>&nbsp; Example:</i></summary>

```json
{
    "items": [
        {
            "presetName": "p1",
            "transformation": "t.flip()~t.flop()",
            "params": {
                "w": {
                    "type": "integer",
                    "default": 200
                },
                "h": {
                    "type": "integer",
                    "default": 400
                }
            },
            "archived": true
        }
    ],
    "page": {
        "type": "number",
        "size": 1,
        "current": 1,
        "hasNext": false
    }
}
```

</details>

### updatePreset

**Summary**: Update a preset.

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
            PixelbinConfig config = new PixelbinConfig(
                new Dictionary<string, string>() {
                    { "domain", "https://api.pixelbin.io" },
                    { "apiSecret", "API_SECRECT_TOKEN" }
                });

            PixelbinClient pixelbin = new PixelbinClient(config);

            // Sync method call
            try
            {
                var result = pixelbin.assets.updatePreset(
                    presetName: "p1",
                    archived: True
                );
                // use result
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }

            // Async method call
            try
            {
                var result = await pixelbin.assets.updatePresetAsync(
                    presetName: "p1",
                    archived: True
                );
                // use result
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }
    }
}
```

| Argument   | Type   | Required | Description                               |
| ---------- | ------ | -------- | ----------------------------------------- |
| presetName | string | yes      | Name of the preset to be updated          |
| archived   | bool   | yes      | Indicates if the preset has been archived |

Update a preset of an organization.

_Returned Response:_

[AddPresetResponse](#addpresetresponse)

Success

<details>
<summary><i>&nbsp; Example:</i></summary>

```json
{
    "presetName": "p1",
    "transformation": "t.flip()~t.flop()",
    "params": {
        "w": {
            "type": "integer",
            "default": 200
        },
        "h": {
            "type": "integer",
            "default": 400
        }
    },
    "archived": true
}
```

</details>

### deletePreset

**Summary**: Delete a preset.

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
            PixelbinConfig config = new PixelbinConfig(
                new Dictionary<string, string>() {
                    { "domain", "https://api.pixelbin.io" },
                    { "apiSecret", "API_SECRECT_TOKEN" }
                });

            PixelbinClient pixelbin = new PixelbinClient(config);

            // Sync method call
            try
            {
                var result = pixelbin.assets.deletePreset(
                    presetName: "p1"
                );
                // use result
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }

            // Async method call
            try
            {
                var result = await pixelbin.assets.deletePresetAsync(
                    presetName: "p1"
                );
                // use result
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }
    }
}
```

| Argument   | Type   | Required | Description                      |
| ---------- | ------ | -------- | -------------------------------- |
| presetName | string | yes      | Name of the preset to be deleted |

Delete a preset of an organization.

_Returned Response:_

[AddPresetResponse](#addpresetresponse)

Success

<details>
<summary><i>&nbsp; Example:</i></summary>

```json
{
    "presetName": "p1",
    "transformation": "t.flip()~t.flop()",
    "params": {
        "w": {
            "type": "integer",
            "default": 200
        },
        "h": {
            "type": "integer",
            "default": 400
        }
    },
    "archived": true
}
```

</details>

### getPreset

**Summary**: Get a preset.

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
            PixelbinConfig config = new PixelbinConfig(
                new Dictionary<string, string>() {
                    { "domain", "https://api.pixelbin.io" },
                    { "apiSecret", "API_SECRECT_TOKEN" }
                });

            PixelbinClient pixelbin = new PixelbinClient(config);

            // Sync method call
            try
            {
                var result = pixelbin.assets.getPreset(
                    presetName: "p1"
                );
                // use result
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }

            // Async method call
            try
            {
                var result = await pixelbin.assets.getPresetAsync(
                    presetName: "p1"
                );
                // use result
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }
    }
}
```

| Argument   | Type   | Required | Description                      |
| ---------- | ------ | -------- | -------------------------------- |
| presetName | string | yes      | Name of the preset to be fetched |

Get a preset of an organization.

_Returned Response:_

[AddPresetResponse](#addpresetresponse)

Success

<details>
<summary><i>&nbsp; Example:</i></summary>

```json
{
    "presetName": "p1",
    "transformation": "t.flip()~t.flop()",
    "params": {
        "w": {
            "type": "integer",
            "default": 200
        },
        "h": {
            "type": "integer",
            "default": 400
        }
    },
    "archived": true
}
```

</details>

### getDefaultAssetForPlayground

**Summary**: Get default asset for playground

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
            PixelbinConfig config = new PixelbinConfig(
                new Dictionary<string, string>() {
                    { "domain", "https://api.pixelbin.io" },
                    { "apiSecret", "API_SECRECT_TOKEN" }
                });

            PixelbinClient pixelbin = new PixelbinClient(config);

            // Sync method call
            try
            {
                var result = pixelbin.assets.getDefaultAssetForPlayground();
                // use result
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }

            // Async method call
            try
            {
                var result = await pixelbin.assets.getDefaultAssetForPlaygroundAsync();
                // use result
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }
    }
}
```

Get default asset for playground

_Returned Response:_

[UploadResponse](#uploadresponse)

Success

<details>
<summary><i>&nbsp; Example:</i></summary>

```json
{
    "_id": "dummy-uuid",
    "name": "asset",
    "path": "dir",
    "fileId": "dir/asset",
    "format": "jpeg",
    "size": 1000,
    "access": "private",
    "isActive": true,
    "tags": ["tag1", "tag2"],
    "metadata": {
        "key": "value"
    },
    "url": "https://domain.com/filename.jpeg"
}
```

</details>

### getModules

**Summary**: Get all transformation modules

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
            PixelbinConfig config = new PixelbinConfig(
                new Dictionary<string, string>() {
                    { "domain", "https://api.pixelbin.io" },
                    { "apiSecret", "API_SECRECT_TOKEN" }
                });

            PixelbinClient pixelbin = new PixelbinClient(config);

            // Sync method call
            try
            {
                var result = pixelbin.assets.getModules();
                // use result
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }

            // Async method call
            try
            {
                var result = await pixelbin.assets.getModulesAsync();
                // use result
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }
    }
}
```

Get all transformation modules.

_Returned Response:_

[TransformationModulesResponse](#transformationmodulesresponse)

Success

<details>
<summary><i>&nbsp; Example:</i></summary>

```json
{
    "delimiters": {
        "operationSeparator": "~",
        "parameterSeparator": ":"
    },
    "plugins": {
        "erase": {
            "identifier": "erase",
            "name": "EraseBG",
            "description": "EraseBG Background Removal Module",
            "credentials": {
                "required": false
            },
            "operations": [
                {
                    "params": {
                        "name": "Industry Type",
                        "type": "enum",
                        "enum": ["general", "ecommerce"],
                        "default": "general",
                        "identifier": "i",
                        "title": "Industry type"
                    },
                    "displayName": "Remove background of an image",
                    "method": "bg",
                    "description": "Remove the background of any image"
                }
            ],
            "enabled": true
        }
    },
    "presets": [
        {
            "_id": "dummy-id",
            "createdAt": "2022-02-14T10:06:17.803Z",
            "updatedAt": "2022-02-14T10:06:17.803Z",
            "isActive": true,
            "orgId": "265",
            "presetName": "compressor",
            "transformation": "t.compress(q:95)",
            "archived": false
        }
    ]
}
```

</details>

### getModule

**Summary**: Get Transformation Module by module identifier

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
            PixelbinConfig config = new PixelbinConfig(
                new Dictionary<string, string>() {
                    { "domain", "https://api.pixelbin.io" },
                    { "apiSecret", "API_SECRECT_TOKEN" }
                });

            PixelbinClient pixelbin = new PixelbinClient(config);

            // Sync method call
            try
            {
                var result = pixelbin.assets.getModule(
                    identifier: "t"
                );
                // use result
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }

            // Async method call
            try
            {
                var result = await pixelbin.assets.getModuleAsync(
                    identifier: "t"
                );
                // use result
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }
    }
}
```

| Argument   | Type   | Required | Description                         |
| ---------- | ------ | -------- | ----------------------------------- |
| identifier | string | yes      | identifier of Transformation Module |

Get Transformation Module by module identifier

_Returned Response:_

[TransformationModuleResponse](#transformationmoduleresponse)

Success

<details>
<summary><i>&nbsp; Example:</i></summary>

```json
{
    "identifier": "erase",
    "name": "EraseBG",
    "description": "EraseBG Background Removal Module",
    "credentials": {
        "required": false
    },
    "operations": [
        {
            "params": {
                "name": "Industry Type",
                "type": "enum",
                "enum": ["general", "ecommerce"],
                "default": "general",
                "identifier": "i",
                "title": "Industry type"
            },
            "displayName": "Remove background of an image",
            "method": "bg",
            "description": "Remove the background of any image"
        }
    ],
    "enabled": true
}
```

</details>

### Schemas

#### folderItem

| Properties | Type   | Nullable | Description                          |
| ---------- | ------ | -------- | ------------------------------------ |
| \_id       | string | yes      | Id of the folder item                |
| name       | string | yes      | Name of the folder item              |
| path       | string | yes      | Path of containing folder            |
| type       | string | yes      | Type of the item. `file` or `folder` |

#### exploreItem

| Properties | Type   | Nullable | Description                                                     |
| ---------- | ------ | -------- | --------------------------------------------------------------- |
| \_id       | string | yes      | id of the exploreItem                                           |
| name       | string | yes      | name of the item                                                |
| type       | string | yes      | Type of item whether `file` or `folder`                         |
| path       | string | yes      | Path of containing folder                                       |
| fileId     | string | no       | Combination of `path` and `name` of file                        |
| format     | string | no       | Format of the file                                              |
| size       | int    | no       | Size of the file in bytes                                       |
| access     | string | no       | Access level of asset, can be either `public-read` or `private` |

#### page

| Properties | Type   | Nullable | Description                   |
| ---------- | ------ | -------- | ----------------------------- |
| type       | string | yes      | Type of page                  |
| size       | int    | yes      | Number of items on the page   |
| current    | int    | yes      | Current page number.          |
| hasNext    | bool   | yes      | Whether the next page exists. |
| itemTotal  | int    | yes      | Total number of items.        |

#### exploreResponse

| Properties | Type              | Nullable | Description                  |
| ---------- | ----------------- | -------- | ---------------------------- |
| items      | List<ExploreItem> | yes      | exploreItems in current page |
| page       | Page              | yes      | page details                 |

#### ListFilesResponse

| Properties | Type              | Nullable | Description                  |
| ---------- | ----------------- | -------- | ---------------------------- |
| items      | List<ExploreItem> | yes      | exploreItems in current page |
| page       | Page              | yes      | page details                 |

#### exploreFolderResponse

| Properties | Type              | Nullable | Description                  |
| ---------- | ----------------- | -------- | ---------------------------- |
| folder     | FolderItem        | yes      | requested folder item        |
| items      | List<ExploreItem> | yes      | exploreItems in current page |
| page       | Page              | yes      | page details                 |

#### FileUploadRequest

| Properties       | Type                       | Nullable | Description                                                                                                                                                                                                                      |
| ---------------- | -------------------------- | -------- | -------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------- |
| file             | FileStream                 | yes      | Asset file                                                                                                                                                                                                                       |
| path             | string                     | no       | Path where you want to store the asset. Path of containing folder                                                                                                                                                                |
| name             | string                     | no       | Name of the asset, if not provided name of the file will be used. Note - The provided name will be slugified to make it URL safe                                                                                                 |
| access           | AccessEnum                 | no       | Access level of asset, can be either `public-read` or `private`                                                                                                                                                                  |
| tags             | List<string>               | no       | Asset tags                                                                                                                                                                                                                       |
| metadata         | Dictionary<string, object> | no       | Asset related metadata                                                                                                                                                                                                           |
| overwrite        | bool                       | no       | Overwrite flag. If set to `true` will overwrite any file that exists with same path, name and type. Defaults to `false`.                                                                                                         |
| filenameOverride | bool                       | no       | If set to `true` will add unique characters to name if asset with given name already exists. If overwrite flag is set to `true`, preference will be given to overwrite flag. If both are set to `false` an error will be raised. |

#### UrlUploadRequest

| Properties       | Type                       | Nullable | Description                                                                                                                                                                                                                      |
| ---------------- | -------------------------- | -------- | -------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------- |
| url              | string                     | yes      | Asset URL                                                                                                                                                                                                                        |
| path             | string                     | no       | Path where you want to store the asset. Path of containing folder.                                                                                                                                                               |
| name             | string                     | no       | Name of the asset, if not provided name of the file will be used. Note - The provided name will be slugified to make it URL safe                                                                                                 |
| access           | AccessEnum                 | no       | Access level of asset, can be either `public-read` or `private`                                                                                                                                                                  |
| tags             | List<string>               | no       | Asset tags                                                                                                                                                                                                                       |
| metadata         | Dictionary<string, object> | no       | Asset related metadata                                                                                                                                                                                                           |
| overwrite        | bool                       | no       | Overwrite flag. If set to `true` will overwrite any file that exists with same path, name and type. Defaults to `false`.                                                                                                         |
| filenameOverride | bool                       | no       | If set to `true` will add unique characters to name if asset with given name already exists. If overwrite flag is set to `true`, preference will be given to overwrite flag. If both are set to `false` an error will be raised. |

#### UploadResponse

| Properties | Type                       | Nullable | Description                                                 |
| ---------- | -------------------------- | -------- | ----------------------------------------------------------- |
| \_id       | string                     | yes      | \_id of the item                                            |
| fileId     | string                     | yes      | Combination of `path` and `name` of file                    |
| name       | string                     | yes      | name of the item                                            |
| path       | string                     | yes      | path to the parent folder                                   |
| format     | string                     | yes      | format of the file                                          |
| size       | int                        | yes      | size of file in bytes                                       |
| access     | string                     | yes      | Access level of asset, can be either public-read or private |
| tags       | List<string>               | no       | tags associated with the item                               |
| metadata   | Dictionary<string, object> | no       | metadata associated with the item                           |
| url        | string                     | no       | url of the item                                             |
| thumbnail  | string                     | no       | url of item thumbnail                                       |

#### SignedUploadRequest

| Properties       | Type                       | Nullable | Description                                                                                                                                                                                                                      |
| ---------------- | -------------------------- | -------- | -------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------- |
| name             | string                     | no       | name of the file                                                                                                                                                                                                                 |
| path             | string                     | no       | Path of containing folder.                                                                                                                                                                                                       |
| format           | string                     | no       | Format of the file                                                                                                                                                                                                               |
| access           | AccessEnum                 | no       | Access level of asset, can be either `public-read` or `private`                                                                                                                                                                  |
| tags             | List<string>               | no       | Tags associated with the file.                                                                                                                                                                                                   |
| metadata         | Dictionary<string, object> | no       | Metadata associated with the file.                                                                                                                                                                                               |
| overwrite        | bool                       | no       | Overwrite flag. If set to `true` will overwrite any file that exists with same path, name and type. Defaults to `false`.                                                                                                         |
| filenameOverride | bool                       | no       | If set to `true` will add unique characters to name if asset with given name already exists. If overwrite flag is set to `true`, preference will be given to overwrite flag. If both are set to `false` an error will be raised. |

#### SignedUploadResponse

| Properties     | Type         | Nullable | Description                                  |
| -------------- | ------------ | -------- | -------------------------------------------- |
| s3PresignedUrl | PresignedUrl | yes      | `signedDetails` for upload with frontend sdk |
|  |

#### PresignedUrl

| Properties | Type                       | Nullable | Description                                 |
| ---------- | -------------------------- | -------- | ------------------------------------------- |
| url        | string                     | no       | `presigned url` for upload                  |
|  |
| fields     | Dictionary<string, object> | no       | signed fields to be sent along with request |

#### FilesResponse

| Properties | Type                       | Nullable | Description                                                    |
| ---------- | -------------------------- | -------- | -------------------------------------------------------------- |
| \_id       | string                     | yes      | \_id of the file                                               |
| name       | string                     | yes      | name of the file                                               |
| path       | string                     | yes      | path of containing folder.                                     |
| fileId     | string                     | yes      | Combination of `path` and `name` of file                       |
| format     | string                     | yes      | format of the file                                             |
| size       | int                        | yes      | size of the file in bytes                                      |
| access     | string                     | yes      | Access level of file, can be either `public-read` or `private` |
| isActive   | bool                       | yes      | Whether the file is active                                     |
| tags       | List<string>               | no       | Tags associated with the file                                  |
| metadata   | Dictionary<string, object> | no       | Metadata associated with the file                              |
| url        | string                     | no       | url of the file                                                |
| thumbnail  | string                     | no       | url of the thumbnail of the file                               |

#### UpdateFileRequest

| Properties | Type                       | Nullable | Description                                                     |
| ---------- | -------------------------- | -------- | --------------------------------------------------------------- |
| name       | string                     | no       | Name of the file                                                |
| path       | string                     | no       | path of containing folder.                                      |
| access     | string                     | no       | Access level of asset, can be either `public-read` or `private` |
| isActive   | bool                       | no       | Whether the file is active                                      |
| tags       | List<string>               | no       | Tags associated with the file                                   |
| metadata   | Dictionary<string, object> | no       | Metadata associated with the file                               |

#### FoldersResponse

| Properties | Type   | Nullable | Description                  |
| ---------- | ------ | -------- | ---------------------------- |
| \_id       | string | yes      | \_id of the folder           |
| name       | string | yes      | name of the folder           |
| path       | string | yes      | path of containing folder.   |
| isActive   | bool   | yes      | whether the folder is active |

#### CreateFolderRequest

| Properties | Type   | Nullable | Description                |
| ---------- | ------ | -------- | -------------------------- |
| name       | string | yes      | Name of the folder         |
| path       | string | no       | path of containing folder. |

#### UpdateFolderRequest

| Properties | Type | Nullable | Description                  |
| ---------- | ---- | -------- | ---------------------------- |
| isActive   | bool | no       | whether the folder is active |

#### TransformationModulesResponse

| Properties | Type                             | Nullable | Description                                         |
| ---------- | -------------------------------- | -------- | --------------------------------------------------- |
| delimiters | Delimiter                        | no       | Delimiter for parsing plugin schema                 |
| plugins    | Dictionary<string, object>       | no       | Transformations currently supported by the pixelbin |
| presets    | List<Dictionary<string, object>> | no       | List of saved presets                               |

#### DeleteMultipleFilesRequest

| Properties | Type         | Nullable | Description                   |
| ---------- | ------------ | -------- | ----------------------------- |
| ids        | List<string> | yes      | Array of file \_ids to delete |

#### Delimiter

| Properties         | Type   | Nullable | Description                                                              |
| ------------------ | ------ | -------- | ------------------------------------------------------------------------ |
| operationSeparator | string | no       | separator to separate operations in the url pattern                      |
| parameterSeparator | string | no       | separator to separate parameters used with operations in the url pattern |

#### TransformationModuleResponse

| Properties  | Type                             | Nullable | Description                                     |
| ----------- | -------------------------------- | -------- | ----------------------------------------------- |
| identifier  | string                           | no       | identifier for the plugin type                  |
| name        | string                           | no       | name of the plugin                              |
| description | string                           | no       | description of the plugin                       |
| credentials | Dictionary<string, object>       | no       | credentials, if any, associated with the plugin |
| operations  | List<Dictionary<string, object>> | no       | supported operations in the plugin              |
| enabled     | bool                             | no       | whether the plugin is enabled                   |

#### Credentials

| Properties  | Type                       | Nullable | Description                                                 |
| ----------- | -------------------------- | -------- | ----------------------------------------------------------- |
| \_id        | string                     | no       | Unique ID for credential                                    |
| createdAt   | string                     | no       | Credential creation ISO timestamp                           |
| updatedAt   | string                     | no       | Credential update ISO timestamp                             |
| isActive    | bool                       | no       | Tells if credential is active or not                        |
| orgId       | string                     | no       | ID of the organization this credential belongs to           |
| pluginId    | string                     | no       | Unique identifier for the plugin this credential belongs to |
| credentials | Dictionary<string, object> | no       | Credentials object. It is different for each plugin         |
| description | Dictionary<string, object> | no       |                                                             |

#### CredentialsItem

| Properties | Type                       | Nullable | Description |
| ---------- | -------------------------- | -------- | ----------- |
| pluginId   | Dictionary<string, object> | no       |             |

#### AddCredentialsRequest

| Properties  | Type                       | Nullable | Description                                                 |
| ----------- | -------------------------- | -------- | ----------------------------------------------------------- |
| credentials | Dictionary<string, object> | yes      | Credentials of the plugin                                   |
| pluginId    | string                     | yes      | Unique identifier for the plugin this credential belongs to |

#### UpdateCredentialsRequest

| Properties  | Type                       | Nullable | Description               |
| ----------- | -------------------------- | -------- | ------------------------- |
| credentials | Dictionary<string, object> | yes      | Credentials of the plugin |

#### AddCredentialsResponse

| Properties  | Type                       | Nullable | Description |
| ----------- | -------------------------- | -------- | ----------- |
| credentials | Dictionary<string, object> | no       |             |

#### DeleteCredentialsResponse

| Properties  | Type                       | Nullable | Description                                                 |
| ----------- | -------------------------- | -------- | ----------------------------------------------------------- |
| \_id        | string                     | no       | Unique Credential ID                                        |
| createdAt   | string                     | no       | Credential creation ISO timestamp                           |
| updatedAt   | string                     | no       | Credential update ISO timestamp                             |
| isActive    | bool                       | no       | Tells if credential is active or not                        |
| orgId       | string                     | no       | ID of the organization this credential belongs to           |
| pluginId    | string                     | no       | Unique identifier for the plugin this credential belongs to |
| credentials | Dictionary<string, object> | no       | Credentials object. It is different for each plugin         |

#### GetAncestorsResponse

| Properties | Type                  | Nullable | Description |
| ---------- | --------------------- | -------- | ----------- |
| folder     | FolderItem            | no       |             |
| ancestors  | List<FoldersResponse> | no       |             |

#### GetFilesWithConstraintsItem

| Properties | Type   | Nullable | Description |
| ---------- | ------ | -------- | ----------- |
| path       | string | no       |             |
| name       | string | no       |             |
| type       | string | no       |             |

#### GetFilesWithConstraintsRequest

| Properties | Type                              | Nullable | Description |
| ---------- | --------------------------------- | -------- | ----------- |
| items      | List<GetFilesWithConstraintsItem> | no       |             |
| maxCount   | float                             | no       |             |
| maxSize    | float                             | no       |             |

#### AddPresetRequest

| Properties     | Type                       | Nullable | Description                                    |
| -------------- | -------------------------- | -------- | ---------------------------------------------- |
| presetName     | string                     | yes      | Name of the preset                             |
| transformation | string                     | yes      | A chain of transformations, separated by `~`   |
| params         | Dictionary<string, object> | no       | Parameters object for transformation variables |

#### AddPresetResponse

| Properties     | Type                       | Nullable | Description                                    |
| -------------- | -------------------------- | -------- | ---------------------------------------------- |
| presetName     | string                     | yes      | Name of the preset                             |
| transformation | string                     | yes      | A chain of transformations, separated by `~`   |
| params         | Dictionary<string, object> | no       | Parameters object for transformation variables |
| archived       | bool                       | no       | Indicates if the preset has been archived      |

#### UpdatePresetRequest

| Properties | Type | Nullable | Description                               |
| ---------- | ---- | -------- | ----------------------------------------- |
| archived   | bool | yes      | Indicates if the preset has been archived |

#### GetPresetsResponse

| Properties | Type                    | Nullable | Description             |
| ---------- | ----------------------- | -------- | ----------------------- |
| items      | List<AddPresetResponse> | yes      | Presets in current page |
| page       | Page                    | yes      | page details            |

### Enums

#### [AccessEnum](#AccessEnum)

Type : string

| Name        | Value       | Description |
| ----------- | ----------- | ----------- |
| PUBLIC_READ | public-read | public-read |
| PRIVATE     | private     | private     |

---
