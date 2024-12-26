using Newtonsoft.Json;
using Pixelbin.Utils;

namespace PixelbinTest
{
    public class UrlObjTestData
    {
        public readonly string url;
        public readonly UrlObj obj;
        public readonly string error;

        public UrlObjTestData(string url, UrlObj obj, string error)
        {
            this.url = url;
            this.obj = obj;
            this.error = error;
        }
    }

    public static class test_utils
    {
        public static void GenerateMockResponses()
        {
            List<MockResponseData> response_data = JsonConvert.DeserializeObject<List<MockResponseData>>(File.ReadAllText("mock_responses.json")) ?? new List<MockResponseData>();

            if (response_data.Count == 0)
                throw new JsonException("There was an error deserializing the mock responses");

            foreach (MockResponseData mockResponse in response_data)
            {
                MOCK_RESPONSE[mockResponse.apiCall] = mockResponse;
            }
        }

        public static Dictionary<string, MockResponseData> MOCK_RESPONSE = new Dictionary<string, MockResponseData>();

        public static List<UrlObjTestData> URLS_TO_OBJ = new List<UrlObjTestData>() {
            new UrlObjTestData(
                "https://cdn.pixelbin.io/v2/red-scene-95b6ea/t.resize()/__playground/playground-default.jpeg",
                new UrlObj(
                    version: "v2",
                    cloudName: "red-scene-95b6ea",
                    pattern: "t.resize()",
                    filePath: "__playground/playground-default.jpeg",
                    options: new UrlObjOptions(),
                    zone: "",
                    baseUrl: "https://cdn.pixelbin.io",
                    worker: false,
                    workerPath: "",
                    transformations: new List<UrlTransformation>() {
                        new UrlTransformation(
                            plugin: "t",
                            name: "resize"
                        )
                    }
                ),
                ""
            ),
            new UrlObjTestData (
                "https://cdn.pixelbin.io/v2/dill-doe-36b4fc/t.rotate(a:102)/__playground/playground-default.jpeg" ,
                new UrlObj(
                    version: "v2",
                    cloudName: "dill-doe-36b4fc",
                    pattern: "t.rotate(a:102)",
                    filePath: "__playground/playground-default.jpeg",
                    options: new UrlObjOptions(),
                    zone: "",
                    baseUrl: "https://cdn.pixelbin.io",
                    worker: false,
                    workerPath: "",
                    transformations: new List<UrlTransformation>() {
                        new UrlTransformation(
                            plugin: "t",
                            name: "rotate",
                            values: new List<Dictionary<string, string>>() {
                                new Dictionary<string, string>() { { "key", "a" }, { "value", "102" } }
                            }
                        )
                    }
                ),
                ""
            ),
            new UrlObjTestData (
                "https://cdn.pixelbin.io/red-scene-95b6ea/t.resize()/__playground/playground-default.jpeg" ,
                new UrlObj(
                    version: "v1",
                    cloudName: "red-scene-95b6ea",
                    pattern: "t.resize()",
                    filePath: "__playground/playground-default.jpeg",
                    options: new UrlObjOptions(),
                    zone: "",
                    baseUrl: "https://cdn.pixelbin.io",
                    worker: false,
                    workerPath: "",
                    transformations: new List<UrlTransformation>() {
                        new UrlTransformation(
                            plugin: "t",
                            name: "resize"
                        )
                    }
                ),
                ""
            ),
            new UrlObjTestData (
                "https://cdn.pixelbin.io/red-scene-95b6ea/zonesl/t.resize()/__playground/playground-default.jpeg" ,
                new UrlObj(
                    version: "v1",
                    cloudName: "red-scene-95b6ea",
                    pattern: "t.resize()",
                    filePath: "__playground/playground-default.jpeg",
                    options: new UrlObjOptions(),
                    zone: "zonesl",
                    baseUrl: "https://cdn.pixelbin.io",
                    worker: false,
                    workerPath: "",
                    transformations: new List<UrlTransformation>() {
                        new UrlTransformation(
                            plugin: "t",
                            name: "resize"
                        )
                    }
                ),
                ""
            ),
            new UrlObjTestData (
                "https://cdn.pixelbin.io/v2/sparkling-moon-a7b75b/t.resize(w:200,h:200)/upload/p1/w/random.jpeg" ,
                new UrlObj(
                    version: "v2",
                    cloudName: "sparkling-moon-a7b75b",
                    pattern: "t.resize(w:200,h:200)",
                    filePath: "upload/p1/w/random.jpeg",
                    options: new UrlObjOptions(),
                    zone: "",
                    baseUrl: "https://cdn.pixelbin.io",
                    worker: false,
                    workerPath: "",
                    transformations: new List<UrlTransformation>() {
                        new UrlTransformation(
                            plugin: "t",
                            name: "resize",
                            values: new List<Dictionary<string, string>>() {
                                new Dictionary<string, string>() { { "key", "w" }, { "value", "200" } },
                                new Dictionary<string, string>() { { "key", "h" }, { "value", "200" } }
                            }
                        )
                    }
                ),
                ""
            ),
            new UrlObjTestData (
                "https://cdn.pixelbin.io/doc/original/searchlight/platform-panel/settings/policy/faq/add-faq-group.png" ,
                new UrlObj(
                    version: "v1",
                    cloudName: "doc",
                    pattern: "original",
                    filePath: "searchlight/platform-panel/settings/policy/faq/add-faq-group.png",
                    options: new UrlObjOptions(),
                    zone: "",
                    baseUrl: "https://cdn.pixelbin.io",
                    worker: false,
                    workerPath: "",
                    transformations: new List<UrlTransformation>()
                ),
                ""
            ),
            new UrlObjTestData (
                "https://cdn.pixelbin.io/v2/dac/ek69d0/original/__playground/playground-default.jpeg" ,
                new UrlObj(
                    version: "v2",
                    cloudName: "dac",
                    pattern: "original",
                    filePath: "__playground/playground-default.jpeg",
                    options: new UrlObjOptions(),
                    zone: "ek69d0",
                    baseUrl: "https://cdn.pixelbin.io",
                    worker: false,
                    workerPath: "",
                    transformations: new List<UrlTransformation>()
                ),
                ""
            ),
            new UrlObjTestData (
                "https://cdn.pixelbin.io/v2/red-scene-95b6ea/t.resize(h:200,w:100)/__playground/playground-default.jpeg" ,
                new UrlObj(
                    version: "v2",
                    cloudName: "red-scene-95b6ea",
                    pattern: "t.resize(h:200,w:100)",
                    filePath: "__playground/playground-default.jpeg",
                    options: new UrlObjOptions(),
                    zone: "",
                    baseUrl: "https://cdn.pixelbin.io",
                    worker: false,
                    workerPath: "",
                    transformations: new List<UrlTransformation>() {
                        new UrlTransformation(
                            plugin: "t",
                            name: "resize",
                            values: new List<Dictionary<string, string>>() {
                                new Dictionary<string, string>() { { "key", "h" }, { "value", "200" } },
                                new Dictionary<string, string>() { { "key", "w" }, { "value", "100" } }
                            }
                        )
                    }
                ),
                ""
            ),
            new UrlObjTestData (
                "https://cdn.pixelbin.io/v2/red-scene-95b6ea/t.resize(h:200,w:100,fill:999)~erase.bg()~t.extend()/__playground/playground-default.jpeg" ,
                new UrlObj(
                    version: "v2",
                    baseUrl: "https://cdn.pixelbin.io",
                    filePath: "__playground/playground-default.jpeg",
                    pattern: "t.resize(h:200,w:100,fill:999)~erase.bg()~t.extend()",
                    cloudName: "red-scene-95b6ea",
                    options: new UrlObjOptions(),
                    zone: "",
                    worker: false,
                    workerPath: "",
                    transformations: new List<UrlTransformation>() {
                        new UrlTransformation(
                            plugin: "t",
                            name: "resize",
                            values: new List<Dictionary<string, string>>() {
                                new Dictionary<string, string>() { { "key", "h" }, { "value", "200" } },
                                new Dictionary<string, string>() { { "key", "w" }, { "value", "100" } },
                                new Dictionary<string, string>() { { "key", "fill" }, { "value", "999" } },
                            }
                        ),
                        new UrlTransformation(
                            plugin: "erase",
                            name: "bg"
                        ),
                        new UrlTransformation(
                            plugin: "t",
                            name: "extend"
                        )
                    }
                ),
                ""
            ),
            new UrlObjTestData (
                "https://cdn.pixelbin.io/v2/red-scene-95b6ea/z-slug/t.compress()~t.resize()~t.extend()~p.apply(n:presetNameXyx)/alien_fig_tree_planet_x_wallingford_seattle_washington_usa_517559.jpeg",
                new UrlObj(
                    version: "v2",
                    baseUrl: "https://cdn.pixelbin.io",
                    filePath: "alien_fig_tree_planet_x_wallingford_seattle_washington_usa_517559.jpeg",
                    pattern: "t.compress()~t.resize()~t.extend()~p.apply(n:presetNameXyx)",
                    cloudName: "red-scene-95b6ea",
                    options: new UrlObjOptions(),
                    zone: "z-slug",
                    worker: false,
                    workerPath: "",
                    transformations: new List<UrlTransformation>() {
                        new UrlTransformation(
                            plugin: "t",
                            name: "compress"
                        ),
                        new UrlTransformation(
                            plugin: "t",
                            name: "resize"
                        ),
                        new UrlTransformation(
                            plugin: "t",
                            name: "extend"
                        ),
                        new UrlTransformation(
                            plugin: "p",
                            name: "apply",
                            values: new List<Dictionary<string, string>>() {
                                new Dictionary<string, string>() { { "key", "n" }, { "value", "presetNameXyx" } }
                            }
                        )
                    }
                ),
                ""
            ),
            new UrlObjTestData (
                "https://cdn.pixelbin.io/v2/red-scene-95b6ea/wrkr/image.jpeg",
                new UrlObj(
                    version: "v2",
                    baseUrl: "https://cdn.pixelbin.io",
                    filePath: "",
                    pattern: "",
                    cloudName: "red-scene-95b6ea",
                    options: new UrlObjOptions(),
                    zone: "",
                    worker: true,
                    workerPath: "image.jpeg",
                    transformations: new List<UrlTransformation>()
                ),
                ""
            ),
            new UrlObjTestData (
                "https://cdn.pixelbin.io/v2/falling-surf-7c8bb8/wrkr/misc/general/free/original/images/favicon.ico",
                new UrlObj(
                    version: "v2",
                    baseUrl: "https://cdn.pixelbin.io",
                    filePath: "",
                    pattern: "",
                    cloudName: "falling-surf-7c8bb8",
                    options: new UrlObjOptions(),
                    zone: "",
                    worker: true,
                    workerPath: "misc/general/free/original/images/favicon.ico",
                    transformations: new List<UrlTransformation>()
                ),
                ""
            ),
            new UrlObjTestData (
                "https://cdn.pixelbin.io/v2/falling-surf-7c8bb8/fyndnp/wrkr/robots.txt",
                new UrlObj(
                    version: "v2",
                    baseUrl: "https://cdn.pixelbin.io",
                    filePath: "",
                    pattern: "",
                    cloudName: "falling-surf-7c8bb8",
                    options: new UrlObjOptions(),
                    zone: "fyndnp",
                    worker: true,
                    workerPath: "robots.txt",
                    transformations: new List<UrlTransformation>()
                ),
                ""
            ),
            new UrlObjTestData (
                "https://cdn.pixelbin.io/v2/falling-surf-7c8bb8/fyprod/wrkr/misc/general/free/original/images/favicon.ico",
                new UrlObj(
                    version: "v2",
                    baseUrl: "https://cdn.pixelbin.io",
                    filePath: "",
                    pattern: "",
                    cloudName: "falling-surf-7c8bb8",
                    options: new UrlObjOptions(),
                    zone: "fyprod",
                    worker: true,
                    workerPath: "misc/general/free/original/images/favicon.ico",
                    transformations: new List<UrlTransformation>()
                ),
                ""
            ),
            new UrlObjTestData (
                "https://cdn.pixelbin.io/v2",
                new UrlObj(),
                "Invalid pixelbin url. Please make sure the url is correct."
            ),
            new UrlObjTestData(
                "https://cdn.pixelbin.io/v3/red-scene-95b6ea/t.compress()~t.resize()~t.extend()~p.apply(n:presetNameXyx)/alien_fig_tree_planet_x_wallingford_seattle_washington_usa_517559.jpeg",
                new UrlObj(),
                "Invalid pixelbin url. Please make sure the url is correct."
            ),
            new UrlObjTestData(
                "https://cdn.pixelbin.io//v2/dill-doe-36b4fc/original~original/__playground/playground-default.jpeg",
                new UrlObj(),
                "Invalid pixelbin url. Please make sure the url is correct."
            ),
            new UrlObjTestData(
                "https://cdn.pixelbin.io/v2/red-scene-95b6ea/test/t.compress()~t.resize()~t.extend()~p.apply(n:presetNameXyx)/alien_fig_tree_planet_x_wallingford_seattle_washington_usa_517559.jpeg",
                new UrlObj(),
                "Error Processing url. Please check the url is correct"
            ),
            new UrlObjTestData(
                "https://cdn.pixelbin.io/v2/red-scene-95b6ea/t.compress~t.resize()~t.extend()~p.apply(n:presetNameXyx)/alien_fig_tree_planet_x_wallingford_seattle_washington_usa_517559.jpeg",
                new UrlObj(),
                "Error Processing url. Please check the url is correct"
            ),
            new UrlObjTestData(
                "https://cdn.pixelbin.io/v2/red-scene-95b6ea/zonesls/t.resize()/__playground/playground-default.jpeg",
                new UrlObj(),
                "Error Processing url. Please check the url is correct"
            )
        };

        public static List<UrlObjTestData> OBJ_TO_URLS = new List<UrlObjTestData>() {
                new UrlObjTestData(
                    "https://cdn.pixelbinx0.de/v2/white-pine-0c2307/t.resize(h:600,w:800)/W2.jpeg",
                    new UrlObj(
                        version: "v2",
                        cloudName: "white-pine-0c2307",
                        pattern: "t.resize(h:600,w:800)",
                        filePath: "W2.jpeg",
                        options: new UrlObjOptions(),
                        zone: "",
                        baseUrl: "https://cdn.pixelbinx0.de",
                        transformations: new List<UrlTransformation>() {
                            new UrlTransformation(
                                plugin: "t",
                                name: "resize",
                                values: new List<Dictionary<string, string>>() {
                                    new Dictionary<string, string>() { { "key", "h" }, { "value", "600" } },
                                    new Dictionary<string, string>() { { "key", "w" }, { "value", "800" } }
                                }
                            )
                        }
                    ),
                    ""
                ),
                new UrlObjTestData(
                    "https://cdn.pixelbinx0.de/v2/white-pine-0c2307/z-slug/t.resize(h:600,w:800)/W2.jpeg",
                    new UrlObj(
                        version: "v2",
                        cloudName: "white-pine-0c2307",
                        pattern: "t.resize(h:600,w:800)",
                        filePath: "W2.jpeg",
                        options: new UrlObjOptions(),
                        zone: "z-slug",
                        baseUrl: "https://cdn.pixelbinx0.de",
                        transformations: new List<UrlTransformation>() {
                            new UrlTransformation(
                                plugin: "t",
                                name: "resize",
                                values: new List<Dictionary<string, string>>() {
                                    new Dictionary<string, string>() { { "key", "h" }, { "value", "600" } },
                                    new Dictionary<string, string>() { { "key", "w" }, { "value", "800" } }
                                }
                            )
                        }
                    ),
                    ""
                ),
                new UrlObjTestData(
                    "https://cdn.pixelbinx0.de/v2/white-pine-0c2307/t.resize(h:600,w:800)~t.rotate(a:-249)/W2.jpeg",
                    new UrlObj(
                        version: "v2",
                        cloudName: "white-pine-0c2307",
                        pattern: "t.resize(h:600,w:800)~t.rotate(a:-249)",
                        filePath: "W2.jpeg",
                        options: new UrlObjOptions(),
                        zone: "",
                        baseUrl: "https://cdn.pixelbinx0.de",
                        transformations: new List<UrlTransformation>() {
                            new UrlTransformation(
                                plugin: "t",
                                name: "resize",
                                values: new List<Dictionary<string, string>>() {
                                    new Dictionary<string, string>() { { "key", "h" }, { "value", "600" } },
                                    new Dictionary<string, string>() { { "key", "w" }, { "value", "800" } }
                                }
                            ),
                            new UrlTransformation(
                                plugin: "t",
                                name: "rotate",
                                values: new List<Dictionary<string, string>>() {
                                    new Dictionary<string, string>() { { "key", "a" }, { "value", "-249" } }
                                }
                            )
                        }
                    ),
                    ""
                ),
                new UrlObjTestData(
                    "https://cdn.pixelbinx0.de/v2/white-pine-0c2307/t.resize(h:600,w:800)~t.rotate(a:-249)~t.flip()~t.trim(t:217)/W2.jpeg",
                    new UrlObj(
                        version: "v2",
                        cloudName: "white-pine-0c2307",
                        pattern: "t.resize(h:600,w:800)~t.rotate(a:-249)~t.flip()~t.trim(t:217)",
                        filePath: "W2.jpeg",
                        options: new UrlObjOptions(),
                        zone: "",
                        baseUrl: "https://cdn.pixelbinx0.de",
                        transformations: new List<UrlTransformation>() {
                            new UrlTransformation(
                                plugin: "t",
                                name: "resize",
                                values: new List<Dictionary<string, string>>() {
                                    new Dictionary<string, string>() { { "key", "h" }, { "value", "600" } },
                                    new Dictionary<string, string>() { { "key", "w" }, { "value", "800" } }
                                }
                            ),
                            new UrlTransformation(
                                plugin: "t",
                                name: "rotate",
                                values: new List<Dictionary<string, string>>() {
                                    new Dictionary<string, string>() { { "key", "a" }, { "value", "-249" } }
                                }
                            ),
                            new UrlTransformation(
                                plugin: "t",
                                name: "flip"
                            ),
                            new UrlTransformation(
                                plugin: "t",
                                name: "trim",
                                values: new List<Dictionary<string, string>>() {
                                    new Dictionary<string, string>() { { "key", "t" }, { "value", "217" } }
                                }
                            )
                        }
                    ),
                    ""
                ),
                new UrlObjTestData(
                    "https://cdn.pixelbinx0.de/v2/white-pine-0c2307/t.resize(h:200,w:100)~p:preset1(a:100,b:2.1,c:test)/W2.jpeg",
                    new UrlObj(
                        version: "v2",
                        cloudName: "white-pine-0c2307",
                        pattern: "t.resize(h:200,w:100)~p:preset1(a:100,b:2.1,c:test)",
                        filePath: "W2.jpeg",
                        zone: "",
                        baseUrl: "https://cdn.pixelbinx0.de",
                        transformations: new List<UrlTransformation>() {
                            new UrlTransformation(
                                plugin: "t",
                                name: "resize",
                                values: new List<Dictionary<string, string>>() {
                                    new Dictionary<string, string>() { { "key", "h" }, { "value", "200" } },
                                    new Dictionary<string, string>() { { "key", "w" }, { "value", "100" } }
                                }
                            ),
                            new UrlTransformation(
                                plugin: "p",
                                name: "preset1",
                                values: new List<Dictionary<string, string>>() {
                                    new Dictionary<string, string>() { { "key", "a" }, { "value", "100" } },
                                    new Dictionary<string, string>() { { "key", "b" }, { "value", "2.1" } },
                                    new Dictionary<string, string>() { { "key", "c" }, { "value", "test" } }
                                }
                            )
                        }
                    ),
                    ""
                ),
                new UrlObjTestData(
                    "https://cdn.pixelbinx0.de/v2/white-pine-0c2307/t.resize(h:200,w:100)~p:preset1/W2.jpeg",
                    new UrlObj(
                        version: "v2",
                        cloudName: "white-pine-0c2307",
                        pattern: "t.resize(h:200,w:100)~p:preset1",
                        filePath: "W2.jpeg",
                        zone: "",
                        baseUrl: "https://cdn.pixelbinx0.de",
                        transformations: new List<UrlTransformation>() {
                            new UrlTransformation(
                                plugin: "t",
                                name: "resize",
                                values: new List<Dictionary<string, string>>() {
                                    new Dictionary<string, string>() { { "key", "h" }, { "value", "200" } },
                                    new Dictionary<string, string>() { { "key", "w" }, { "value", "100" } }
                                }
                            ),
                            new UrlTransformation(
                                plugin: "p",
                                name: "preset1"
                            )
                        }
                    ),
                    ""
                ),
                new UrlObjTestData(
                    "https://cdn.pixelbinx0.de/v2/white-pine-0c2307/t.resize(h:200,w:100)~p:preset1(a:12)/W2.jpeg",
                    new UrlObj(
                        version: "v2",
                        cloudName: "white-pine-0c2307",
                        pattern: "t.resize(h:200,w:100)~p:preset1(a:12",
                        filePath: "W2.jpeg",
                        zone: "",
                        baseUrl: "https://cdn.pixelbinx0.de",
                        transformations: new List<UrlTransformation>() {
                            new UrlTransformation(
                                plugin: "t",
                                name: "resize",
                                values: new List<Dictionary<string, string>>() {
                                    new Dictionary<string, string>() { { "key", "h" }, { "value", "200" } },
                                    new Dictionary<string, string>() { { "key", "w" }, { "value", "100" } }
                                }
                            ),
                            new UrlTransformation(
                                plugin: "p",
                                name: "preset1",
                                values: new List<Dictionary<string, string>>() {
                                    new Dictionary<string, string>() { { "key", "a" }, { "value", "12" } }
                                }
                            )
                        }
                    ),
                    ""
                ),
                new UrlObjTestData(
                    "https://cdn.pixelbinx0.de/v2/white-pine-0c2307/t.resize(h:200,w:100)~p:preset1/W2.jpeg",
                    new UrlObj(
                        version: "v2",
                        cloudName: "white-pine-0c2307",
                        pattern: "t.resize(h:200,w:100)~p:preset1(a:12",
                        filePath: "W2.jpeg",
                        zone: "",
                        baseUrl: "https://cdn.pixelbinx0.de",
                        transformations: new List<UrlTransformation>() {
                            new UrlTransformation(
                                plugin: "t",
                                name: "resize",
                                values: new List<Dictionary<string, string>>() {
                                    new Dictionary<string, string>() { { "key", "h" }, { "value", "200" } },
                                    new Dictionary<string, string>() { { "key", "w" }, { "value", "100" } }
                                }
                            ),
                            new UrlTransformation(
                                plugin: "p",
                                name: "preset1",
                                values: new List<Dictionary<string, string>>()
                            )
                        }
                    ),
                    ""
                ),
                new UrlObjTestData(
                    "https://cdn.pixelbinx0.de/v2/white-pine-0c2307/t.resize(h:,w:100)~p:preset1/W2.jpeg",
                    new UrlObj(
                        version: "v2",
                        cloudName: "white-pine-0c2307",
                        pattern: "t.resize(h:,w:100)~p:preset1(a:12",
                        filePath: "W2.jpeg",
                        zone: "",
                        baseUrl: "https://cdn.pixelbinx0.de",
                        transformations: new List<UrlTransformation>() {
                            new UrlTransformation(
                                plugin: "t",
                                name: "resize",
                                values: new List<Dictionary<string, string>>() {
                                    new Dictionary<string, string>() { { "key", "h" }, { "value", "" } },
                                    new Dictionary<string, string>() { { "key", "w" }, { "value", "100" } }
                                }
                            ),
                            new UrlTransformation(
                                plugin: "p",
                                name: "preset1",
                                values: new List<Dictionary<string, string>>()
                            )
                        }
                    ),
                    "value not specified for 'h' in 'resize'"
                ),
                new UrlObjTestData(
                    "https://cdn.pixelbinx0.de/v2/white-pine-0c2307/t.resize(h:200,w:100)~p:preset1/W2.jpeg",
                    new UrlObj(
                        version: "v2",
                        cloudName: "white-pine-0c2307",
                        pattern: "t.resize(h:200,w:100)~p:preset1(a:12",
                        filePath: "W2.jpeg",
                        zone: "",
                        baseUrl: "https://cdn.pixelbinx0.de",
                        transformations: new List<UrlTransformation>() {
                            new UrlTransformation(
                                plugin: "t",
                                name: "resize",
                                values: new List<Dictionary<string, string>>() {
                                    new Dictionary<string, string>() { { "value", "" } },
                                    new Dictionary<string, string>() { { "key", "w" }, { "value", "100" } }
                                }
                            ),
                            new UrlTransformation(
                                plugin: "p",
                                name: "preset1",
                                values: new List<Dictionary<string, string>>()
                            )
                        }
                    ),
                    "key not specified in 'resize'"
                ),
                new UrlObjTestData(
                    "https://cdn.pixelbinx0.de/v2/white-pine-0c2307/t.resize(h:200,w:100)~p:preset1/W2.jpeg",
                    new UrlObj(
                        version: "v2",
                        cloudName: "white-pine-0c2307",
                        pattern: "t.resize(h:200,w:100)~p:preset1(a:12",
                        filePath: "W2.jpeg",
                        zone: "",
                        baseUrl: "https://cdn.pixelbinx0.de",
                        transformations: new List<UrlTransformation>() {
                            new UrlTransformation(
                                plugin: "t",
                                name: "resize",
                                values: new List<Dictionary<string, string>>() {
                                    new Dictionary<string, string>(),
                                    new Dictionary<string, string>() { { "key", "w" }, { "value", "100" } }
                                }
                            ),
                            new UrlTransformation(
                                plugin: "p",
                                name: "preset1",
                                values: new List<Dictionary<string, string>>()
                            )
                        }
                    ),
                    "key not specified in 'resize'"
                ),
                new UrlObjTestData(
                    "https://cdn.pixelbinx0.de/v2/feel/erase.bg(shadow:true)~t.merge(m:underlay,i:eU44YkFJOHlVMmZrWVRDOUNTRm1D,b:screen,r:true)/MZZKB3e1hT48o0NYJ2Kxh.jpeg?dpr=2.0&f_auto=True",
                    new UrlObj(
                        version: "v2",
                        baseUrl: "https://cdn.pixelbinx0.de",
                        filePath: "MZZKB3e1hT48o0NYJ2Kxh.jpeg",
                        pattern: "erase.bg(shadow:true)~t.merge(m:underlay,i:eU44YkFJOHlVMmZrWVRDOUNTRm1D,b:screen,r:true)",
                        cloudName: "feel",
                        options: new UrlObjOptions(
                            dpr: 2.0,
                            f_auto: true
                        ),
                        zone: "",
                        transformations: new List<UrlTransformation>() {
                            new UrlTransformation(
                                plugin: "erase",
                                name: "bg",
                                values: new List<Dictionary<string, string>>() {
                                    new Dictionary<string, string>() { { "key", "shadow" }, { "value", "true" } }
                                }
                            ),
                            new UrlTransformation(
                                plugin: "t",
                                name: "merge",
                                values: new List<Dictionary<string, string>>() {
                                    new Dictionary<string, string>() { { "key", "m" }, { "value", "underlay" } },
                                    new Dictionary<string, string>() { { "key", "i" }, { "value", "eU44YkFJOHlVMmZrWVRDOUNTRm1D" } },
                                    new Dictionary<string, string>() { { "key", "b" }, { "value", "screen" } },
                                    new Dictionary<string, string>() { { "key", "r" }, { "value", "true" } }
                                }
                            )
                        }
                    ),
                    ""
                )
            };
    }

    [JsonObject(ItemNullValueHandling = NullValueHandling.Ignore)]
    public class MockResponseData
    {
        public string? apiKey { get; set; }
        public string apiCall { get; set; } = "";
        public MockResponse response { get; set; } = new MockResponse();
    }

    public class MockResponse
    {
        public string url { get; set; } = "";
        public string method { get; set; } = "";
        public Dictionary<string, object> @params { get; set; } = new Dictionary<string, object>();
        public Dictionary<string, object> data { get; set; } = new Dictionary<string, object>();
        public int status_code { get; set; } = 0;
        public string content { get; set; } = "";
        public Dictionary<string, string> headers { get; set; } = new Dictionary<string, string>();
        public Dictionary<string, object> cookies { get; set; } = new Dictionary<string, object>();
        public string error_message { get; set; } = "";
    }

    public static class DictionaryExcetions
    {
        public static T Get<T>(this Dictionary<string, object> instance, string name)
        {
            return (T)instance[name];
        }

    }
}
