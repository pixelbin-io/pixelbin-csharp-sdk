using Microsoft.Extensions.FileSystemGlobbing.Internal;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Pixelbin.Utils;
using System;
using System.Net;
using System.Net.Http.Headers;
using System.Text;
using System.Xml.Linq;

namespace PixelbinTest
{
    public static class test_utils
    {
        public static void GenerateMockResponses()
        {
            var response_data = JsonConvert.DeserializeObject<List<MockResponseData>>(File.ReadAllText("mock_responses.json"));


            foreach (MockResponseData mockResponse in response_data)
            {
                MOCK_RESPONSE[mockResponse.apiCall] = mockResponse;
            }
        }

        public static Dictionary<string, MockResponseData> MOCK_RESPONSE = new Dictionary<string, MockResponseData>();

        public static List<Dictionary<string, object>> URLS_TO_OBJ = new List<Dictionary<string, object>>() {
                new Dictionary<string, object>() {
                    { "url", "https://cdn.pixelbinx0.de/v2/white-pine-0c2307/t.flip(h:600,w:800)/W2.jpeg" },
                    {
                        "obj", new UrlObj(
                            version: "v2",
                            cloudName: "white-pine-0c2307",
                            pattern: "t.flip(h:600,w:800)",
                            filePath: "W2.jpeg",
                            options: new UrlObjOptions(),
                            zone: null,
                            baseUrl: "https://cdn.pixelbinx0.de",
                            transformations: new List<UrlTransformation>() {
                                new UrlTransformation(
                                    plugin: "t",
                                    name: "flip",
                                    values: new List<Dictionary<string, string>>() {
                                        new Dictionary<string, string>() { { "key", "h" }, { "value", "600" } },
                                        new Dictionary<string, string>() { { "key", "w" }, { "value", "800" } }
                                    }
                                )
                            }
                        )
                    }
                },
                new Dictionary<string, object>() {
                    { "url", "https://cdn.pixelbinx0.de/v2/white-pine-0c2307/z-slug/t.resize(h:600,w:800)/W2.jpeg" },
                    {
                        "obj", new UrlObj(
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
                        )
                    }
                },
                new Dictionary<string, object>() {
                    { "url", "https://cdn.pixelbinx0.de/v2/white-pine-0c2307/t.resize(h:600,w:800)~t.rotate(a:-249)/W2.jpeg" },
                    {
                        "obj", new UrlObj(
                            version: "v2",
                            cloudName: "white-pine-0c2307",
                            pattern: "t.resize(h:600,w:800)~t.rotate(a:-249)",
                            filePath: "W2.jpeg",
                            options: new UrlObjOptions(),
                            zone: null,
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
                        )
                    }
                },
                new Dictionary<string, object>() {
                    { "url", "https://cdn.pixelbinx0.de/v2/white-pine-0c2307/t.resize(h:600,w:800)~t.rotate(a:-249)~t.flip()~t.trim(t:217)/W2.jpeg" },
                    {
                        "obj", new UrlObj(
                            version: "v2",
                            cloudName: "white-pine-0c2307",
                            pattern: "t.resize(h:600,w:800)~t.rotate(a:-249)~t.flip()~t.trim(t:217)",
                            filePath: "W2.jpeg",
                            options: new UrlObjOptions(),
                            zone: null,
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
                        )
                    }
                },
                new Dictionary<string, object>() {
                    { "url", "https://cdn.pixelbinx0.de/v2/white-pine-0c2307/t.resize(h:200,w:100)~p:preset1(a:100,b:2.1,c:test)/W2.jpeg" },
                    {
                        "obj", new UrlObj(
                            version: "v2",
                            cloudName: "white-pine-0c2307",
                            pattern: "t.resize(h:200,w:100)~p:preset1(a:100,b:2.1,c:test)",
                            filePath: "W2.jpeg",
                            options: new UrlObjOptions(),
                            zone: null,
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
                        )
                    }
                },
                new Dictionary<string, object>() {
                    { "url", "https://cdn.pixelbinx0.de/v2/white-pine-0c2307/t.resize(h:200,w:100)~p:preset1/W2.jpeg" },
                    {
                        "obj", new UrlObj(
                            version: "v2",
                            cloudName: "white-pine-0c2307",
                            pattern: "t.resize(h:200,w:100)~p:preset1",
                            filePath: "W2.jpeg",
                            options: new UrlObjOptions(),
                            zone: null,
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
                        )
                    }
                },
                new Dictionary<string, object>() {
                    { "url", "https://cdn.pixelbinx0.de/v2/white-pine-0c2307/t.resize(h:200,w:100)~p:preset1()/W2.jpeg" },
                    {
                        "obj", new UrlObj(
                            version: "v2",
                            cloudName: "white-pine-0c2307",
                            pattern: "t.resize(h:200,w:100)~p:preset1()",
                            filePath: "W2.jpeg",
                            options: new UrlObjOptions(),
                            zone: null,
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
                        )
                    }
                },
                new Dictionary<string, object>() {
                    { "url", "https://cdn.pixelbinx0.de/v2/white-pine-0c2307/t.resize(h:200,w:100)~p:preset1(a:12/W2.jpeg" },
                    {
                        "obj", new UrlObj(
                            version: "v2",
                            cloudName: "white-pine-0c2307",
                            pattern: "t.resize(h:200,w:100)~p:preset1(a:12",
                            filePath: "W2.jpeg",
                            options: new UrlObjOptions(),
                            zone: null,
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
                        )
                    }
                },
                new Dictionary<string, object>() {
                    { "url", "https://cdn.pixelbinx0.de/v2/feel/erase.bg(shadow:true)~t.merge(m:underlay,i:eU44YkFJOHlVMmZrWVRDOUNTRm1D,b:screen,r:true)/MZZKB3e1hT48o0NYJ2Kxh.jpeg?dpr=2.0&f_auto=True" },
                    {
                        "obj", new UrlObj(
                            version: "v2",
                            baseUrl: "https://cdn.pixelbinx0.de",
                            filePath: "MZZKB3e1hT48o0NYJ2Kxh.jpeg",
                            pattern: "erase.bg(shadow:true)~t.merge(m:underlay,i:eU44YkFJOHlVMmZrWVRDOUNTRm1D,b:screen,r:true)",
                            cloudName: "feel",
                            options: new UrlObjOptions(
                                dpr: 2.0,
                                f_auto: true
                            ),
                            zone: null,
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
                        )
                    }
                }
            };

        public static List<Dictionary<string, object>> OBJ_TO_URLS = new List<Dictionary<string, object>>() {
                new Dictionary<string, object>() {
                    {"url", "https://cdn.pixelbinx0.de/v2/white-pine-0c2307/t.resize(h:600,w:800)/W2.jpeg"},
                    {"obj", new UrlObj(
                        version: "v2",
                        cloudName: "white-pine-0c2307",
                        pattern: "t.resize(h:600,w:800)",
                        filePath: "W2.jpeg",
                        options: new UrlObjOptions(),
                        zone: null,
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
                    )}
                },
                new Dictionary<string, object>() {
                    {"url", "https://cdn.pixelbinx0.de/v2/white-pine-0c2307/z-slug/t.resize(h:600,w:800)/W2.jpeg"},
                    {"obj", new UrlObj(
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
                    )}
                },
                new Dictionary<string, object>() {
                    {"url", "https://cdn.pixelbinx0.de/v2/white-pine-0c2307/t.resize(h:600,w:800)~t.rotate(a:-249)/W2.jpeg"},
                    {"obj", new UrlObj(
                        version: "v2",
                        cloudName: "white-pine-0c2307",
                        pattern: "t.resize(h:600,w:800)~t.rotate(a:-249)",
                        filePath: "W2.jpeg",
                        options: new UrlObjOptions(),
                        zone: null,
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
                    )}
                },
                new Dictionary<string, object>() {
                    {"url", "https://cdn.pixelbinx0.de/v2/white-pine-0c2307/t.resize(h:600,w:800)~t.rotate(a:-249)~t.flip()~t.trim(t:217)/W2.jpeg"},
                    {"obj", new UrlObj(
                        version: "v2",
                        cloudName: "white-pine-0c2307",
                        pattern: "t.resize(h:600,w:800)~t.rotate(a:-249)~t.flip()~t.trim(t:217)",
                        filePath: "W2.jpeg",
                        options: new UrlObjOptions(),
                        zone: null,
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
                    )}
                },
                new Dictionary<string, object>() {
                    {"url", "https://cdn.pixelbinx0.de/v2/white-pine-0c2307/t.resize(h:200,w:100)~p:preset1(a:100,b:2.1,c:test)/W2.jpeg"},
                    {"obj", new UrlObj(
                        version: "v2",
                        cloudName: "white-pine-0c2307",
                        pattern: "t.resize(h:200,w:100)~p:preset1(a:100,b:2.1,c:test)",
                        filePath: "W2.jpeg",
                        zone: null,
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
                    )}
                },
                new Dictionary<string, object>() {
                    {"url", "https://cdn.pixelbinx0.de/v2/white-pine-0c2307/t.resize(h:200,w:100)~p:preset1/W2.jpeg"},
                    {"obj", new UrlObj(
                        version: "v2",
                        cloudName: "white-pine-0c2307",
                        pattern: "t.resize(h:200,w:100)~p:preset1",
                        filePath: "W2.jpeg",
                        zone: null,
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
                    )}
                },
                new Dictionary<string, object>() {
                    {"url", "https://cdn.pixelbinx0.de/v2/white-pine-0c2307/t.resize(h:200,w:100)~p:preset1(a:12)/W2.jpeg"},
                    {"obj", new UrlObj(
                        version: "v2",
                        cloudName: "white-pine-0c2307",
                        pattern: "t.resize(h:200,w:100)~p:preset1(a:12",
                        filePath: "W2.jpeg",
                        zone: null,
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
                    )}
                },
                new Dictionary<string, object>() {
                    {"url", "https://cdn.pixelbinx0.de/v2/white-pine-0c2307/t.resize(h:200,w:100)~p:preset1/W2.jpeg"},
                    {"obj", new UrlObj(
                        version: "v2",
                        cloudName: "white-pine-0c2307",
                        pattern: "t.resize(h:200,w:100)~p:preset1(a:12",
                        filePath: "W2.jpeg",
                        zone: null,
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
                    )},
                },
                new Dictionary<string, object>() {
                    {"url", "https://cdn.pixelbinx0.de/v2/white-pine-0c2307/t.resize(h:,w:100)~p:preset1/W2.jpeg"},
                    {"obj", new UrlObj(
                        version: "v2",
                        cloudName: "white-pine-0c2307",
                        pattern: "t.resize(h:,w:100)~p:preset1(a:12",
                        filePath: "W2.jpeg",
                        zone: null,
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
                    )},
                    {"error", "value not specified for 'h' in 'resize'"}
                },
                new Dictionary<string, object>() {
                    {"url", "https://cdn.pixelbinx0.de/v2/white-pine-0c2307/t.resize(h:200,w:100)~p:preset1/W2.jpeg"},
                    {"obj", new UrlObj(
                        version: "v2",
                        cloudName: "white-pine-0c2307",
                        pattern: "t.resize(h:200,w:100)~p:preset1(a:12",
                        filePath: "W2.jpeg",
                        zone: null,
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
                    )},
                    {"error", "key not specified in 'resize'"}
                },
                new Dictionary<string, object>() {
                    {"url", "https://cdn.pixelbinx0.de/v2/white-pine-0c2307/t.resize(h:200,w:100)~p:preset1/W2.jpeg"},
                    {"obj", new UrlObj(
                        version: "v2",
                        cloudName: "white-pine-0c2307",
                        pattern: "t.resize(h:200,w:100)~p:preset1(a:12",
                        filePath: "W2.jpeg",
                        zone: null,
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
                    )},
                    {"error", "key not specified in 'resize'"}
                },
                new Dictionary<string, object>() {
                    {"url", "https://cdn.pixelbinx0.de/v2/feel/erase.bg(shadow:true)~t.merge(m:underlay,i:eU44YkFJOHlVMmZrWVRDOUNTRm1D,b:screen,r:true)/MZZKB3e1hT48o0NYJ2Kxh.jpeg?dpr=2.0&f_auto=True"},
                    {"obj", new UrlObj(
                        version: "v2",
                        baseUrl: "https://cdn.pixelbinx0.de",
                        filePath: "MZZKB3e1hT48o0NYJ2Kxh.jpeg",
                        pattern: "erase.bg(shadow:true)~t.merge(m:underlay,i:eU44YkFJOHlVMmZrWVRDOUNTRm1D,b:screen,r:true)",
                        cloudName: "feel",
                        options: new UrlObjOptions(
                            dpr: 2.0,
                            f_auto: true
                        ),
                        zone: null,
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
                    )}
                }
            };
	}

    [JsonObject(ItemNullValueHandling = NullValueHandling.Ignore)]
    public class MockResponseData
    {
        public string? apiKey { get; set; }
        public string apiCall { get; set; }
        public MockResponse response { get; set; }
    }

    public class MockResponse
    {
        public string url { get; set; }
        public string method { get; set; }
        public Dictionary<string, object> @params { get; set; }
        public Dictionary<string, object> data { get; set; }
        public int status_code { get; set; }
        public string content { get; set; }
        public Dictionary<string, string> headers { get; set; }
        public Dictionary<string, object> cookies { get; set; }
        public string error_message { get; set; }
    }

    public static class DictionaryExcetions
    {
        public static T Get<T>(this Dictionary<string, object> instance, string name)
        {
            return (T)instance[name];
        }

    }
}
