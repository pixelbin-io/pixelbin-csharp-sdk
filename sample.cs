using System;
using System.Collections.Generic;
using System.IO;
using Pixelbin.Platform;

namespace Sample
{
    class SampleClass
    {
        async void Main(string[] args)
        {
            PixelbinConfig config = new PixelbinConfig(new Dictionary<string, string>(){
                "domain" = "https://api.pixelbin.io",
                "apiSecret" = "API_TOKEN"
            });

            PixelbinClient pixelbin = new PixelbinClient(config);

            // # Sync method call
            try
            {
                var result = pixelbin.assets.fileUpload(file: new FileStream("mydata", FileMode.Open));
                // Use result
                Console.WriteLine(result);
            }
            catch (System.Exception e)
            {
                Console.WriteLine(e);
            }

            // # Async method call
            try
            {
                var result = await pixelbin.assets.fileUploadAsync(file: new FileStream("mydata", FileMode.Open));
                // Use result
                Console.WriteLine(result);
            }
            catch (System.Exception e)
            {
                Console.WriteLine(e);
            }
        }
    }
}