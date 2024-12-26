using NUnit.Framework;
using Pixelbin.Common.Exceptions;
using Pixelbin.Utils;
using static Pixelbin.Utils.Url;
using static PixelbinTest.test_utils;
using Pixelbin.Security;
using System.Collections.Specialized;
using System.Web;

namespace PixelbinTest
{
    [TestFixture]
    public class TestUrlUtils
    {
        [Test]
        [Category("URL-UrlObj convertion")]
        [TestCase(TestName = "get obj from url")]
        public void Test_1()
        {
            UrlObj obj = UrlToObj("https://cdn.pixelbin.io/v2/red-scene-95b6ea/t.resize()/__playground/playground-default.jpeg");
            UrlObj expectedObj = new UrlObj(
                    transformations: new List<UrlTransformation>() {
                        new(
                            plugin: "t",
                            name: "resize"
                        )
                    },
                    cloudName: "red-scene-95b6ea",
                    zone: "",
                    baseUrl: "https://cdn.pixelbin.io",
                    pattern: "t.resize()",
                    version: "v2",
                    filePath: "__playground/playground-default.jpeg",
                    worker: false,
                    workerPath: "",
                    options: new UrlObjOptions()
                );
            Assert.That(obj, Is.EqualTo(expectedObj));
        }

        [Test]
        [Category("URL-UrlObj convertion")]
        [TestCase(TestName = "get obj from url - 1")]
        public void Test_2()
        {
            UrlObj obj = UrlToObj("https://cdn.pixelbin.io/v2/dill-doe-36b4fc/t.rotate(a:102)/__playground/playground-default.jpeg");
            UrlObj expectedObj = new UrlObj(
                    transformations: new List<UrlTransformation>() {
                        new(
                            plugin: "t",
                            name: "rotate",
                            values: new List<Dictionary<string, string>>() {
                                new() { {"key", "a"}, {"value", "102"} }
                            }
                        )
                    },
                    cloudName: "dill-doe-36b4fc",
                    zone: "",
                    baseUrl: "https://cdn.pixelbin.io",
                    pattern: "t.rotate(a:102)",
                    version: "v2",
                    filePath: "__playground/playground-default.jpeg",
                    worker: false,
                    workerPath: "",
                    options: new UrlObjOptions()
                );
            Assert.That(obj, Is.EqualTo(expectedObj));
        }

        [Test]
        [Category("URL-UrlObj convertion")]
        [TestCase(TestName = "get obj from url no version")]
        public void Test_3()
        {
            UrlObj obj = UrlToObj("https://cdn.pixelbin.io/red-scene-95b6ea/t.resize()/__playground/playground-default.jpeg");
            UrlObj expectedObj = new UrlObj(
                    transformations: new List<UrlTransformation>() {
                        new(
                            plugin: "t",
                            name: "resize"
                        )
                    },
                    cloudName: "red-scene-95b6ea",
                    zone: "",
                    baseUrl: "https://cdn.pixelbin.io",
                    pattern: "t.resize()",
                    version: "v1",
                    filePath: "__playground/playground-default.jpeg",
                    worker: false,
                    workerPath: "",
                    options: new UrlObjOptions()
                );
            Assert.That(obj, Is.EqualTo(expectedObj));
        }

        [Test]
        [Category("URL-UrlObj convertion")]
        [TestCase(TestName = "get obj from url with zoneslug")]
        public void Test_4()
        {
            UrlObj obj = UrlToObj("https://cdn.pixelbin.io/red-scene-95b6ea/zonesl/t.resize()/__playground/playground-default.jpeg");
            UrlObj expectedObj = new UrlObj(
                    transformations: new List<UrlTransformation>() {
                        new(
                            plugin: "t",
                            name: "resize"
                        )
                    },
                    cloudName: "red-scene-95b6ea",
                    zone: "zonesl",
                    baseUrl: "https://cdn.pixelbin.io",
                    pattern: "t.resize()",
                    version: "v1",
                    filePath: "__playground/playground-default.jpeg",
                    worker: false,
                    workerPath: "",
                    options: new UrlObjOptions()
                );
            Assert.That(obj, Is.EqualTo(expectedObj));
        }

        [Test]
        [Category("URL-UrlObj convertion")]
        [TestCase(TestName = "get obj from url - error")]
        public void Test_5()
        {
            UrlObj obj = UrlToObj("https://cdn.pixelbin.io/v2/sparkling-moon-a7b75b/t.resize(w:200,h:200)/upload/p1/w/random.jpeg");
            UrlObj expectedObj = new UrlObj(
                    transformations: new List<UrlTransformation>() {
                        new(
                            plugin: "t",
                            name: "resize",
                            values: new List<Dictionary<string, string>>() {
                                new() { {"key", "w"}, {"value", "200"} },
                                new() { {"key", "h"}, {"value", "200"} }
                            }
                        )
                    },
                    cloudName: "sparkling-moon-a7b75b",
                    zone: "",
                    baseUrl: "https://cdn.pixelbin.io",
                    pattern: "t.resize(w:200,h:200)",
                    version: "v2",
                    filePath: "upload/p1/w/random.jpeg",
                    worker: false,
                    workerPath: "",
                    options: new UrlObjOptions()
                );
            Assert.That(obj, Is.EqualTo(expectedObj));
        }

        [Test]
        [Category("URL-UrlObj convertion")]
        [TestCase(TestName = "get obj from url - error 1")]
        public void Test_6()
        {
            var exception = Assert.Throws<PDKInvalidUrlError>(() => UrlToObj("https://cdn.pixelbin.io/v2"));
            Assert.That(exception.Message, Is.EqualTo("Invalid pixelbin url. Please make sure the url is correct."));
        }

        [Test]
        [Category("URL-UrlObj convertion")]
        [TestCase(TestName = "get obj from url - error 2")]
        public void Test_7()
        {
            UrlObj obj = UrlToObj("https://cdn.pixelbin.io/doc/original/searchlight/platform-panel/settings/policy/faq/add-faq-group.png");
            UrlObj expectedObj = new UrlObj(
                    transformations: new List<UrlTransformation>(),
                    cloudName: "doc",
                    zone: "",
                    baseUrl: "https://cdn.pixelbin.io",
                    pattern: "original",
                    version: "v1",
                    filePath: "searchlight/platform-panel/settings/policy/faq/add-faq-group.png",
                    worker: false,
                    workerPath: "",
                    options: new UrlObjOptions()
                );
            Assert.That(obj, Is.EqualTo(expectedObj));
        }

        [Test]
        [Category("URL-UrlObj convertion")]
        [TestCase(TestName = "get obj from url - error 3")]
        public void Test_8()
        {
            UrlObj obj = UrlToObj("https://cdn.pixelbin.io/v2/dac/ek69d0/original/__playground/playground-default.jpeg");
            UrlObj expectedObj = new UrlObj(
                    transformations: new List<UrlTransformation>(),
                    cloudName: "dac",
                    zone: "ek69d0",
                    baseUrl: "https://cdn.pixelbin.io",
                    pattern: "original",
                    version: "v2",
                    filePath: "__playground/playground-default.jpeg",
                    worker: false,
                    workerPath: "",
                    options: new UrlObjOptions()
                );
            Assert.That(obj, Is.EqualTo(expectedObj));
        }

        [Test]
        [Category("URL-UrlObj convertion")]
        [TestCase(TestName = "get obj from url - 2")]
        public void Test_9()
        {
            UrlObj obj = UrlToObj("https://cdn.pixelbin.io/v2/red-scene-95b6ea/t.resize(h:200,w:100)/__playground/playground-default.jpeg");
            UrlObj expectedObj = new UrlObj(
                    transformations: new List<UrlTransformation>() {
                        new(
                            plugin: "t",
                            name: "resize",
                            values: new List<Dictionary<string, string>>() {
                                new() { {"key", "h"}, {"value", "200"} },
                                new() { {"key", "w"}, {"value", "100"} }
                            }
                        )
                    },
                    cloudName: "red-scene-95b6ea",
                    zone: "",
                    baseUrl: "https://cdn.pixelbin.io",
                    pattern: "t.resize(h:200,w:100)",
                    version: "v2",
                    filePath: "__playground/playground-default.jpeg",
                    worker: false,
                    workerPath: "",
                    options: new UrlObjOptions()
                );
            Assert.That(obj, Is.EqualTo(expectedObj));
        }

        [Test]
        [Category("URL-UrlObj convertion")]
        [TestCase(TestName = "get obj from url - 3")]
        public void Test_10()
        {
            UrlObj obj = UrlToObj("https://cdn.pixelbin.io/v2/red-scene-95b6ea/t.resize(h:200,w:100,fill:999)~erase.bg()~t.extend()/__playground/playground-default.jpeg");
            UrlObj expectedObj = new UrlObj(
                    transformations: new List<UrlTransformation>() {
                        new(
                            plugin: "t",
                            name: "resize",
                            values: new List<Dictionary<string, string>>() {
                                new() { {"key", "h"}, {"value", "200"} },
                                new() { {"key", "w"}, {"value", "100"} },
                                new() { {"key", "fill"}, {"value", "999"} }
                            }
                        ),
                        new(
                            plugin: "erase",
                            name: "bg"
                        ),
                        new(
                            plugin: "t",
                            name: "extend"
                        )
                    },
                    cloudName: "red-scene-95b6ea",
                    zone: "",
                    baseUrl: "https://cdn.pixelbin.io",
                    pattern: "t.resize(h:200,w:100,fill:999)~erase.bg()~t.extend()",
                    version: "v2",
                    filePath: "__playground/playground-default.jpeg",
                    worker: false,
                    workerPath: "",
                    options: new UrlObjOptions()
                );
            Assert.That(obj, Is.EqualTo(expectedObj));
        }

        [Test]
        [Category("URL-UrlObj convertion")]
        [TestCase(TestName = "get obj from url with preset")]
        public void Test_11()
        {
            UrlObj obj = UrlToObj("https://cdn.pixelbin.io/v2/red-scene-95b6ea/z-slug/t.compress()~t.resize()~t.extend()~p.apply(n:presetNameXyx)/alien_fig_tree_planet_x_wallingford_seattle_washington_usa_517559.jpeg");
            UrlObj expectedObj = new UrlObj(
                    transformations: new List<UrlTransformation>() {
                        new(
                            plugin: "t",
                            name: "compress"
                        ),
                        new(
                            plugin: "t",
                            name: "resize"
                        ),
                        new(
                            plugin: "t",
                            name: "extend"
                        ),
                        new(
                            plugin: "p",
                            name: "apply",
                            values: new List<Dictionary<string, string>>() {
                                new() { {"key", "n"}, {"value", "presetNameXyx"} }
                            }
                        )
                    },
                    cloudName: "red-scene-95b6ea",
                    zone: "z-slug",
                    baseUrl: "https://cdn.pixelbin.io",
                    pattern: "t.compress()~t.resize()~t.extend()~p.apply(n:presetNameXyx)",
                    version: "v2",
                    filePath: "alien_fig_tree_planet_x_wallingford_seattle_washington_usa_517559.jpeg",
                    worker: false,
                    workerPath: "",
                    options: new UrlObjOptions()
                );
            Assert.That(obj, Is.EqualTo(expectedObj));
        }

        [Test]
        [Category("URL-UrlObj convertion")]
        [TestCase(TestName = "get obj from url with preset - error")]
        public void Test_12()
        {
            string presetUrl = "https://cdn.pixelbin.io/v3/red-scene-95b6ea/t.compress()~t.resize()~t.extend()~p.apply(n:presetNameXyx)/alien_fig_tree_planet_x_wallingford_seattle_washington_usa_517559.jpeg";
            var exception = Assert.Throws<PDKInvalidUrlError>(() => UrlToObj(presetUrl));
            Assert.That(exception.Message, Is.EqualTo("Invalid pixelbin url. Please make sure the url is correct."));
        }

        [Test]
        [Category("URL-UrlObj convertion")]
        [TestCase(TestName = "handle incorrect urls")]
        public void Test_13()
        {
            var exception = Assert.Throws<PDKInvalidUrlError>(() => UrlToObj("https://cdn.pixelbin.io//v2/dill-doe-36b4fc/original~original/__playground/playground-default.jpeg"));
            Assert.That(exception.Message, Is.EqualTo("Invalid pixelbin url. Please make sure the url is correct."));
        }

        [Test]
        [Category("URL-UrlObj convertion")]
        [TestCase(TestName = "handle incorrect urls - incorrect zone")]
        public void Test_14()
        {
            string presetUrl = "https://cdn.pixelbin.io/v2/red-scene-95b6ea/test/t.compress()~t.resize()~t.extend()~p.apply(n:presetNameXyx)/alien_fig_tree_planet_x_wallingford_seattle_washington_usa_517559.jpeg";
            var exception = Assert.Throws<PDKInvalidUrlError>(() => UrlToObj(presetUrl));
            Assert.That(exception.Message, Is.EqualTo("Error Processing url. Please check the url is correct"));
        }

        [Test]
        [Category("URL-UrlObj convertion")]
        [TestCase(TestName = "handle incorrect urls - incorrect pattern")]
        public void Test_15()
        {
            string presetUrl = "https://cdn.pixelbin.io/v2/red-scene-95b6ea/t.compress~t.resize()~t.extend()~p.apply(n:presetNameXyx)/alien_fig_tree_planet_x_wallingford_seattle_washington_usa_517559.jpeg";
            var exception = Assert.Throws<PDKInvalidUrlError>(() => UrlToObj(presetUrl));
            Assert.That(exception.Message, Is.EqualTo("Error Processing url. Please check the url is correct"));
        }

        [Test]
        [Category("URL-UrlObj convertion")]
        [TestCase(TestName = "handle incorrect urls - incorrect pattern - 2")]
        public void Test_16()
        {
            string presetUrl = "https://cdn.pixelbin.io/v2/red-scene-95b6ea/zonesls/t.resize()/__playground/playground-default.jpeg";
            var exception = Assert.Throws<PDKInvalidUrlError>(() => UrlToObj(presetUrl));
            Assert.That(exception.Message, Is.EqualTo("Error Processing url. Please check the url is correct"));
        }

        [Test]
        [Category("URL-UrlObj convertion")]
        [TestCase(TestName = "get obj from url - worker path - fullpath with depth 1")]
        public void Test_17()
        {
            UrlObj obj = UrlToObj("https://cdn.pixelbin.io/v2/red-scene-95b6ea/wrkr/image.jpeg");
            UrlObj expectedObj = new UrlObj(
                    transformations: new List<UrlTransformation>(),
                    cloudName: "red-scene-95b6ea",
                    zone: "",
                    baseUrl: "https://cdn.pixelbin.io",
                    pattern: "",
                    version: "v2",
                    filePath: "",
                    worker: true,
                    workerPath: "image.jpeg",
                    options: new UrlObjOptions()
                );
            Assert.That(obj, Is.EqualTo(expectedObj));
        }

        [Test]
        [Category("URL-UrlObj convertion")]
        [TestCase(TestName = "get obj from url - worker path - fullpath with depth > 1")]
        public void Test_18()
        {
            UrlObj obj = UrlToObj("https://cdn.pixelbin.io/v2/falling-surf-7c8bb8/wrkr/misc/general/free/original/images/favicon.ico");
            UrlObj expectedObj = new UrlObj(
                    transformations: new List<UrlTransformation>(),
                    cloudName: "falling-surf-7c8bb8",
                    zone: "",
                    baseUrl: "https://cdn.pixelbin.io",
                    pattern: "",
                    version: "v2",
                    filePath: "",
                    worker: true,
                    workerPath: "misc/general/free/original/images/favicon.ico",
                    options: new UrlObjOptions()
                );
            Assert.That(obj, Is.EqualTo(expectedObj));
        }

        [Test]
        [Category("URL-UrlObj convertion")]
        [TestCase(TestName = "get obj from url - worker path with zone - fullpath with depth 1")]
        public void Test_19()
        {
            UrlObj obj = UrlToObj("https://cdn.pixelbin.io/v2/falling-surf-7c8bb8/fyndnp/wrkr/robots.txt");
            UrlObj expectedObj = new UrlObj(
                    transformations: new List<UrlTransformation>(),
                    cloudName: "falling-surf-7c8bb8",
                    zone: "fyndnp",
                    baseUrl: "https://cdn.pixelbin.io",
                    pattern: "",
                    version: "v2",
                    filePath: "",
                    worker: true,
                    workerPath: "robots.txt",
                    options: new UrlObjOptions()
                );
            Assert.That(obj, Is.EqualTo(expectedObj));
        }

        [Test]
        [Category("URL-UrlObj convertion")]
        [TestCase(TestName = "get obj from url - worker path with zone - fullpath with depth > 1")]
        public void Test_20()
        {
            UrlObj obj = UrlToObj("https://cdn.pixelbin.io/v2/falling-surf-7c8bb8/fyprod/wrkr/misc/general/free/original/images/favicon.ico");
            UrlObj expectedObj = new UrlObj(
                    transformations: new List<UrlTransformation>(),
                    cloudName: "falling-surf-7c8bb8",
                    zone: "fyprod",
                    baseUrl: "https://cdn.pixelbin.io",
                    pattern: "",
                    version: "v2",
                    filePath: "",
                    worker: true,
                    workerPath: "misc/general/free/original/images/favicon.ico",
                    options: new UrlObjOptions()
                );
            Assert.That(obj, Is.EqualTo(expectedObj));
        }

        [Test]
        [Category("URL-UrlObj convertion")]
        [TestCase(TestName = "generate url from obj")]
        public void Test_21()
        {
            List<UrlTransformation> transformations = new List<UrlTransformation>() {
                new(
                    plugin: "t",
                    name: "resize",
                    values: new List<Dictionary<string, string>>() {
                        new() {{"key", "h"}, {"value", "200"}},
                        new() {{"key", "w"}, {"value", "100"}},
                        new() {{"key", "fill"}, {"value", "999"}},
                    }
                ),
                new(
                    plugin: "erase",
                    name: "bg"
                ),
                new(
                    plugin: "t",
                    name: "extend"
                ),
                new(
                    plugin: "p",
                    name: "preset1"
                )
            };
            UrlObj obj = new UrlObj(
                    transformations: transformations,
                    cloudName: "red-scene-95b6ea",
                    zone: "z-slug",
                    baseUrl: "https://cdn.pixelbin.io",
                    version: "v2",
                    filePath: "__playground/playground-default.jpeg"
                );
            string generatedUrl = ObjToUrl(obj);
            Assert.That(generatedUrl, Is.EqualTo("https://cdn.pixelbin.io/v2/red-scene-95b6ea/z-slug/t.resize(h:200,w:100,fill:999)~erase.bg()~t.extend()~p:preset1/__playground/playground-default.jpeg"));
            obj.version = "v1";
            generatedUrl = ObjToUrl(obj);
            Assert.That(generatedUrl, Is.EqualTo("https://cdn.pixelbin.io/v1/red-scene-95b6ea/z-slug/t.resize(h:200,w:100,fill:999)~erase.bg()~t.extend()~p:preset1/__playground/playground-default.jpeg"));
        }

        [Test]
        [Category("URL-UrlObj convertion")]
        [TestCase(TestName = "generate url from obj - 1")]
        public void Test_22()
        {
            List<UrlTransformation> transformations = new List<UrlTransformation>() {
                new(
                    plugin: "t",
                    name: "resize",
                    values: new List<Dictionary<string, string>>() {
                        new() {{"key", "h"}, {"value", "200"}},
                        new() {{"key", "w"}, {"value", "100"}},
                        new() {{"key", "fill"}, {"value", "999"}},
                    }
                ),
                new(
                    plugin: "erase",
                    name: "bg",
                    values: new List<Dictionary<string, string>>() {
                        new() {{"key", "i"}, {"value", "general"}},
                    }
                ),
                new(
                    plugin: "t",
                    name: "extend"
                ),
                new(
                    plugin: "p",
                    name: "preset1"
                )
            };
            UrlObj obj = new UrlObj(
                isCustomDomain: false,
                transformations: transformations,
                cloudName: "red-scene-95b6ea",
                zone: "z-slug",
                baseUrl: "https://cdn.pixelbin.io",
                version: "v2",
                filePath: "__playground/playground-default.jpeg"
            );
            string generatedUrl = ObjToUrl(obj);
            Assert.That(generatedUrl, Is.EqualTo("https://cdn.pixelbin.io/v2/red-scene-95b6ea/z-slug/t.resize(h:200,w:100,fill:999)~erase.bg(i:general)~t.extend()~p:preset1/__playground/playground-default.jpeg"));
            obj.version = "v1";
            generatedUrl = ObjToUrl(obj);
            Assert.That(generatedUrl, Is.EqualTo("https://cdn.pixelbin.io/v1/red-scene-95b6ea/z-slug/t.resize(h:200,w:100,fill:999)~erase.bg(i:general)~t.extend()~p:preset1/__playground/playground-default.jpeg"));
        }

        [Test]
        [Category("URL-UrlObj convertion")]
        [TestCase(TestName = "throw error if transformation object is incorrect")]
        public void Test_23()
        {
            List<UrlTransformation> transformations = new List<UrlTransformation>() {
                new(
                    plugin: "t",
                    name: "resize",
                    values: new List<Dictionary<string, string>>() {
                        new() {{"key", ""}},
                        new() {{"key", "w"}, {"value", "100"}},
                        new() {{"key", "fill"}, {"value", "999"}},
                    }
                ),
                new(
                    plugin: "erase",
                    name: "bg",
                    values: new List<Dictionary<string, string>>() {
                        new() {{"key", "i"}, {"value", "general"}},
                    }
                ),
                new(
                    plugin: "t",
                    name: "extend"
                ),
                new(
                    plugin: "p",
                    name: "preset1"
                )
            };
            UrlObj obj = new UrlObj(
                isCustomDomain: false,
                transformations: transformations,
                cloudName: "red-scene-95b6ea",
                zone: "z-slug",
                baseUrl: "https://cdn.pixelbin.io",
                version: "v2",
                filePath: "__playground/playground-default.jpeg"
            );
            var exception = Assert.Throws<PDKIllegalArgumentError>(() => ObjToUrl(obj));
            Assert.That(exception.Message, Is.EqualTo("key not specified in 'resize'"));
            obj.version = "v1";
            exception = Assert.Throws<PDKIllegalArgumentError>(() => ObjToUrl(obj));
            Assert.That(exception.Message, Is.EqualTo("key not specified in 'resize'"));
        }

        [Test]
        [Category("URL-UrlObj convertion")]
        [TestCase(TestName = "throw error if transformation object is incorrect - 1")]
        public void Test_24()
        {
            List<UrlTransformation> transformations = new List<UrlTransformation>() {
                new(
                    plugin: "t",
                    name: "resize",
                    values: new List<Dictionary<string, string>>() {
                        new() {{"key", "h"}},
                        new() {{"key", "w"}, {"value", "100"}},
                        new() {{"key", "fill"}, {"value", "999"}},
                    }
                ),
                new(
                    plugin: "erase",
                    name: "bg",
                    values: new List<Dictionary<string, string>>() {
                        new() {{"key", "i"}, {"value", "general"}},
                    }
                ),
                new(
                    plugin: "t",
                    name: "extend"
                ),
                new(
                    plugin: "p",
                    name: "preset1"
                )
            };
            UrlObj obj = new UrlObj(
                isCustomDomain: false,
                transformations: transformations,
                cloudName: "red-scene-95b6ea",
                zone: "z-slug",
                baseUrl: "https://cdn.pixelbin.io",
                version: "v2",
                filePath: "__playground/playground-default.jpeg"
            );
            var exception = Assert.Throws<PDKIllegalArgumentError>(() => ObjToUrl(obj));
            Assert.That(exception.Message, Is.EqualTo("value not specified for key 'h' in 'resize'"));
            obj.version = "v1";
            exception = Assert.Throws<PDKIllegalArgumentError>(() => ObjToUrl(obj));
            Assert.That(exception.Message, Is.EqualTo("value not specified for key 'h' in 'resize'"));
        }

        [Test]
        [Category("URL-UrlObj convertion")]
        [TestCase(TestName = "throw error if worker is true but workerPath is not defined")]
        public void Test_25()
        {
            List<UrlTransformation> transformations = new List<UrlTransformation>();
            UrlObj obj = new UrlObj(
                isCustomDomain: false,
                transformations: transformations,
                cloudName: "red-scene-95b6ea",
                zone: "z-slug",
                baseUrl: "https://cdn.pixelbin.io",
                version: "v2",
                filePath: "__playground/playground-default.jpeg",
                worker: true
            );
            var exception = Assert.Throws<PDKIllegalArgumentError>(() => ObjToUrl(obj));
            Assert.That(exception.Message, Is.EqualTo("key workerPath should be defined"));
        }

        [Test]
        [Category("URL-UrlObj convertion")]
        [TestCase(TestName = "generate url from obj when empty")]
        public void Test_26()
        {
            List<UrlTransformation> transformations = new List<UrlTransformation>();
            UrlObj obj = new UrlObj(
                isCustomDomain: false,
                transformations: transformations,
                cloudName: "red-scene-95b6ea",
                zone: "z-slu1",
                baseUrl: "https://cdn.pixelbin.io",
                version: "v2",
                filePath: "__playground/playground-default.jpeg"
            );

            string generatedUrl = ObjToUrl(obj);
            Assert.That(generatedUrl, Is.EqualTo("https://cdn.pixelbin.io/v2/red-scene-95b6ea/z-slu1/original/__playground/playground-default.jpeg"));
            obj.version = "v1";
            generatedUrl = ObjToUrl(obj);
            Assert.That(generatedUrl, Is.EqualTo("https://cdn.pixelbin.io/v1/red-scene-95b6ea/z-slu1/original/__playground/playground-default.jpeg"));
        }

        [Test]
        [Category("URL-UrlObj convertion")]
        [TestCase(TestName = "generate url from obj undefined")]
        public void Test_27()
        {
            UrlObj obj = new UrlObj(
                isCustomDomain: false,
                cloudName: "red-scene-95b6ea",
                zone: "z-slug",
                baseUrl: "https://cdn.pixelbin.io",
                version: "v2",
                filePath: "__playground/playground-default.jpeg"
            );

            string generatedUrl = ObjToUrl(obj);
            Assert.That(generatedUrl, Is.EqualTo("https://cdn.pixelbin.io/v2/red-scene-95b6ea/z-slug/original/__playground/playground-default.jpeg"));
            obj.version = "v1";
            generatedUrl = ObjToUrl(obj);
            Assert.That(generatedUrl, Is.EqualTo("https://cdn.pixelbin.io/v1/red-scene-95b6ea/z-slug/original/__playground/playground-default.jpeg"));
        }

        [Test]
        [Category("URL-UrlObj convertion")]
        [TestCase(TestName = "generate url from obj - empty object")]
        public void Test_28()
        {
            UrlObj obj = new UrlObj(
                transformations: new List<UrlTransformation>() {
                    new()
                },
                isCustomDomain: false,
                cloudName: "red-scene-95b6ea",
                zone: "z-slug",
                baseUrl: "https://cdn.pixelbin.io",
                version: "v2",
                filePath: "__playground/playground-default.jpeg"
            );

            string generatedUrl = ObjToUrl(obj);
            Assert.That(generatedUrl, Is.EqualTo("https://cdn.pixelbin.io/v2/red-scene-95b6ea/z-slug/original/__playground/playground-default.jpeg"));
            obj.version = "v1";
            generatedUrl = ObjToUrl(obj);
            Assert.That(generatedUrl, Is.EqualTo("https://cdn.pixelbin.io/v1/red-scene-95b6ea/z-slug/original/__playground/playground-default.jpeg"));
        }

        [Test]
        [Category("URL-UrlObj convertion")]
        [TestCase(TestName = "throw error to generate url from obj if filePath not defined")]
        public void Test_29()
        {
            UrlObj obj = new UrlObj(
                transformations: new List<UrlTransformation>() {
                    new()
                },
                isCustomDomain: false,
                cloudName: "red-scene-95b6ea",
                zone: "z-slug",
                baseUrl: "https://cdn.pixelbin.io",
                version: "v2",
                filePath: ""
            );
            Assert.Throws<PDKIllegalArgumentError>(() => ObjToUrl(obj));
        }

        [Test]
        [Category("URL-UrlObj convertion")]
        [TestCase(TestName = "get obj from url with dpr = auto")]
        public void Test_30()
        {
            UrlObj obj = UrlToObj("https://cdn.pixelbin.io/v2/feel/erase.bg(shadow:true)~t.merge(m:underlay,i:eU44YkFJOHlVMmZrWVRDOUNTRm1D,b:screen,r:true)/MZZKB3e1hT48o0NYJ2Kxh.jpeg?dpr=auto&f_auto=true");
            UrlObj expectedObj = new UrlObj(
                baseUrl: "https://cdn.pixelbin.io",
                filePath: "MZZKB3e1hT48o0NYJ2Kxh.jpeg",
                pattern: "erase.bg(shadow:true)~t.merge(m:underlay,i:eU44YkFJOHlVMmZrWVRDOUNTRm1D,b:screen,r:true)",
                version: "v2",
                zone: "",
                cloudName: "feel",
                options: new UrlObjOptions(
                    dpr: "auto",
                    f_auto: true
                ),
                worker: false,
                workerPath: "",
                transformations: new List<UrlTransformation>() {
                    new(
                        plugin: "erase",
                        name: "bg",
                        values: new List<Dictionary<string, string>>() {
                            new() {{"key", "shadow"}, {"value", "true"}}
                        }
                    ),
                    new(
                        plugin: "t",
                        name: "merge",
                        values: new List<Dictionary<string, string>>() {
                            new() {{"key", "m"}, {"value", "underlay"}},
                            new() {{"key", "i"}, {"value", "eU44YkFJOHlVMmZrWVRDOUNTRm1D"}},
                            new() {{"key", "b"}, {"value", "screen"}},
                            new() {{"key", "r"}, {"value", "true"}}
                        }
                    )
                }
            );
            Assert.That(obj, Is.EqualTo(expectedObj));
        }

        [Test]
        [Category("URL-UrlObj convertion")]
        [TestCase(TestName = "get obj from url with options if available")]
        public void Test_31()
        {
            UrlObj obj = UrlToObj("https://cdn.pixelbin.io/v2/feel/erase.bg(shadow:true)~t.merge(m:underlay,i:eU44YkFJOHlVMmZrWVRDOUNTRm1D,b:screen,r:true)/MZZKB3e1hT48o0NYJ2Kxh.jpeg?dpr=2.5&f_auto=true");
            UrlObj expectedObj = new UrlObj(
                baseUrl: "https://cdn.pixelbin.io",
                filePath: "MZZKB3e1hT48o0NYJ2Kxh.jpeg",
                pattern: "erase.bg(shadow:true)~t.merge(m:underlay,i:eU44YkFJOHlVMmZrWVRDOUNTRm1D,b:screen,r:true)",
                version: "v2",
                zone: "",
                cloudName: "feel",
                options: new UrlObjOptions(
                    dpr: 2.5,
                    f_auto: true
                ),
                worker: false,
                workerPath: "",
                transformations: new List<UrlTransformation>() {
                    new(
                        plugin: "erase",
                        name: "bg",
                        values: new List<Dictionary<string, string>>() {
                            new() {{"key", "shadow"}, {"value", "true"}}
                        }
                    ),
                    new(
                        plugin: "t",
                        name: "merge",
                        values: new List<Dictionary<string, string>>() {
                            new() {{"key", "m"}, {"value", "underlay"}},
                            new() {{"key", "i"}, {"value", "eU44YkFJOHlVMmZrWVRDOUNTRm1D"}},
                            new() {{"key", "b"}, {"value", "screen"}},
                            new() {{"key", "r"}, {"value", "true"}}
                        }
                    )
                }
            );
            Assert.That(obj, Is.EqualTo(expectedObj));
        }

        [Test]
        [Category("URL-UrlObj convertion")]
        [TestCase(TestName = "generate url from obj with options if available")]
        public void Test_32()
        {
            UrlObj obj = new UrlObj(
                baseUrl: "https://cdn.pixelbin.io",
                filePath: "__playground/playground-default.jpeg",
                pattern: "erase.bg(shadow:true)~t.merge(m:underlay,i:eU44YkFJOHlVMmZrWVRDOUNTRm1D,b:screen,r:true)",
                version: "v2",
                zone: "z-slug",
                cloudName: "red-scene-95b6ea",
                options: new UrlObjOptions(
                    dpr: 2.5,
                    f_auto: true
                ),
                worker: false,
                workerPath: "",
                transformations: new List<UrlTransformation>() {
                    new()
                }
            );
            string generatedUrl = ObjToUrl(obj);
            Assert.That(generatedUrl, Is.EqualTo("https://cdn.pixelbin.io/v2/red-scene-95b6ea/z-slug/original/__playground/playground-default.jpeg?dpr=2.5&f_auto=true"));
        }

        [Test]
        [Category("URL-UrlObj convertion")]
        [TestCase(TestName = "get failure while retrieving obj from url with invalid options")]
        public void Test_33()
        {
            string optionsUrl = "https://cdn.pixelbin.io/v2/feel/erase.bg(shadow:true)~t.merge(m:underlay,i:eU44YkFJOHlVMmZrWVRDOUNTRm1D,b:screen,r:true)/MZZKB3e1hT48o0NYJ2Kxh.jpeg?dpr=5.5&f_auto=true";
            Assert.Throws<PDKIllegalQueryParameterError>(() => UrlToObj(optionsUrl));
        }

        [Test]
        [Category("URL-UrlObj convertion")]
        [TestCase(TestName = "get failure while retrieving url from obj with invalid options")]
        public void Test_34()
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
                transformations: new List<UrlTransformation>() {
                    new()
                }
            );
            Assert.Throws<PDKIllegalQueryParameterError>(() => ObjToUrl(obj));
        }

        [Test]
        [Category("URL-UrlObj convertion")]
        [TestCase(TestName = "get obj from url - 4")]
        public void Test_35()
        {
            UrlObj obj = UrlToObj("https://cdn.pixelbin.io/v2/dill-doe-36b4fc/t.rotate(a:102)~p:preset1(a:100,b:2.1,c:test)/__playground/playground-default.jpeg");
            UrlObj expectedObj = new UrlObj(
                transformations: new List<UrlTransformation>() {
                    new(
                        plugin: "t",
                        name: "rotate",
                        values: new List<Dictionary<string, string>>() {
                            new() {{"key", "a"}, {"value", "102"}}
                        }
                    ),
                    new(
                        plugin: "p",
                        name: "preset1",
                        values: new List<Dictionary<string, string>>() {
                            new() {{"key", "a"}, {"value", "100"}},
                            new() {{"key", "b"}, {"value", "2.1"}},
                            new() {{"key", "c"}, {"value", "test"}}
                        }
                    )
                },
                cloudName: "dill-doe-36b4fc",
                zone: "",
                baseUrl: "https://cdn.pixelbin.io",
                pattern: "t.rotate(a:102)~p:preset1(a:100,b:2.1,c:test)",
                version: "v2",
                filePath: "__playground/playground-default.jpeg"
            );
            if (obj.transformations != null && expectedObj.transformations != null)
                Assert.That(obj.transformations.GetString(), Is.EqualTo(expectedObj.transformations.GetString()));
            Assert.That(obj.cloudName, Is.EqualTo(expectedObj.cloudName));
            Assert.That(obj.zone, Is.EqualTo(expectedObj.zone));
            Assert.That(obj.baseUrl, Is.EqualTo(expectedObj.baseUrl));
            Assert.That(obj.pattern, Is.EqualTo(expectedObj.pattern));
            Assert.That(obj.version, Is.EqualTo(expectedObj.version));
            Assert.That(obj.filePath, Is.EqualTo(expectedObj.filePath));
        }

        [Test]
        [Category("URL-UrlObj convertion")]
        [TestCase(TestName = "get obj from url - 5")]
        public void Test_36()
        {
            UrlObj obj = UrlToObj("https://cdn.pixelbin.io/v2/dill-doe-36b4fc/t.rotate(a:102)~p:preset1/__playground/playground-default.jpeg");
            UrlObj expectedObj = new UrlObj(
                transformations: new List<UrlTransformation>() {
                    new(
                        plugin: "t",
                        name: "rotate",
                        values: new List<Dictionary<string, string>>() {
                            new() {{"key", "a"}, {"value", "102"}}
                        }
                    ),
                    new(
                        plugin: "p",
                        name: "preset1"
                    )
                },
                cloudName: "dill-doe-36b4fc",
                zone: "",
                baseUrl: "https://cdn.pixelbin.io",
                pattern: "t.rotate(a:102)~p:preset1",
                version: "v2",
                filePath: "__playground/playground-default.jpeg"
            );
            if (obj.transformations != null && expectedObj.transformations != null)
                Assert.That(string.Concat(obj.transformations.Select(o => o.ToString())), Is.EqualTo(string.Concat(expectedObj.transformations.Select(o => o.ToString()))));
            Assert.That(obj.cloudName, Is.EqualTo(expectedObj.cloudName));
            Assert.That(obj.zone, Is.EqualTo(expectedObj.zone));
            Assert.That(obj.baseUrl, Is.EqualTo(expectedObj.baseUrl));
            Assert.That(obj.pattern, Is.EqualTo(expectedObj.pattern));
            Assert.That(obj.version, Is.EqualTo(expectedObj.version));
            Assert.That(obj.filePath, Is.EqualTo(expectedObj.filePath));
        }

        [Test]
        [Category("URL-UrlObj convertion")]
        [TestCase(TestName = "get obj from url - 6")]
        public void Test_37()
        {
            UrlObj obj = UrlToObj("https://cdn.pixelbin.io/v2/dill-doe-36b4fc/t.rotate(a:102)~p:preset1()/__playground/playground-default.jpeg");
            UrlObj expectedObj = new UrlObj(
                transformations: new List<UrlTransformation>() {
                    new(
                        plugin: "t",
                        name: "rotate",
                        values: new List<Dictionary<string, string>>() {
                            new() {{"key", "a"}, {"value", "102"}}
                        }
                    ),
                    new(
                        plugin: "p",
                        name: "preset1"
                    )
                },
                cloudName: "dill-doe-36b4fc",
                zone: "",
                baseUrl: "https://cdn.pixelbin.io",
                pattern: "t.rotate(a:102)~p:preset1()",
                version: "v2",
                filePath: "__playground/playground-default.jpeg"
            );
            if (obj.transformations != null && expectedObj.transformations != null)
                Assert.That(string.Concat(obj.transformations.Select(o => o.ToString())), Is.EqualTo(string.Concat(expectedObj.transformations.Select(o => o.ToString()))));
            Assert.That(obj.cloudName, Is.EqualTo(expectedObj.cloudName));
            Assert.That(obj.zone, Is.EqualTo(expectedObj.zone));
            Assert.That(obj.baseUrl, Is.EqualTo(expectedObj.baseUrl));
            Assert.That(obj.pattern, Is.EqualTo(expectedObj.pattern));
            Assert.That(obj.version, Is.EqualTo(expectedObj.version));
            Assert.That(obj.filePath, Is.EqualTo(expectedObj.filePath));
        }

        [Test]
        [Category("URL-UrlObj convertion")]
        [TestCase(TestName = "get obj from url - 7")]
        public void Test_38()
        {
            UrlObj obj = UrlToObj("https://cdn.pixelbin.io/v2/dill-doe-36b4fc/t.rotate(a:102)~p:preset1(a:12/__playground/playground-default.jpeg");
            UrlObj expectedObj = new UrlObj(
                transformations: new List<UrlTransformation>() {
                    new(
                        plugin: "t",
                        name: "rotate",
                        values: new List<Dictionary<string, string>>() {
                            new() {{"key", "a"}, {"value", "102"}}
                        }
                    ),
                    new(
                        plugin: "p",
                        name: "preset1",
                        values: new List<Dictionary<string, string>>() {
                            new() {{"key", "a"}, {"value", "12"}}
                        }
                    )
                },
                cloudName: "dill-doe-36b4fc",
                zone: "",
                baseUrl: "https://cdn.pixelbin.io",
                pattern: "t.rotate(a:102)~p:preset1(a:12",
                version: "v2",
                filePath: "__playground/playground-default.jpeg"
            );
            if (obj.transformations != null && expectedObj.transformations != null)
                Assert.That(string.Concat(obj.transformations.Select(o => o.ToString())), Is.EqualTo(string.Concat(expectedObj.transformations.Select(o => o.ToString()))));
            Assert.That(obj.cloudName, Is.EqualTo(expectedObj.cloudName));
            Assert.That(obj.zone, Is.EqualTo(expectedObj.zone));
            Assert.That(obj.baseUrl, Is.EqualTo(expectedObj.baseUrl));
            Assert.That(obj.pattern, Is.EqualTo(expectedObj.pattern));
            Assert.That(obj.version, Is.EqualTo(expectedObj.version));
            Assert.That(obj.filePath, Is.EqualTo(expectedObj.filePath));
        }

        [Test]
        [Category("URL-UrlObj convertion")]
        [TestCase(TestName = "generate url from obj - 2")]
        public void Test_39()
        {
            List<UrlTransformation> transformations = new List<UrlTransformation>() {
                    new(
                        plugin: "t",
                        name: "resize",
                        values: new List<Dictionary<string, string>>() {
                            new() {{"key", "h"}, {"value", "200"}},
                            new() {{"key", "w"}, {"value", "100"}},
                            new() {{"key", "fill"}, {"value", "999"}}
                        }
                    ),
                    new(
                        plugin: "erase",
                        name: "bg"
                    ),
                    new(
                        plugin: "t",
                        name: "extend"
                    ),
                    new(
                        plugin: "p",
                        name: "preset1",
                        values: new List<Dictionary<string, string>>() {
                            new() {{"key", "a"}, {"value", "200"}},
                            new() {{"key", "b"}, {"value", "1.2"}},
                            new() {{"key", "c"}, {"value", "test"}}
                        }
                    )
                };
            UrlObj obj = new UrlObj(
                cloudName: "red-scene-95b6ea",
                zone: "z-slug",
                version: "v2",
                transformations: transformations,
                baseUrl: "https://cdn.pixelbin.io",
                filePath: "__playground/playground-default.jpeg"
            );

            string generatedUrl = ObjToUrl(obj);
            Assert.That(generatedUrl, Is.EqualTo("https://cdn.pixelbin.io/v2/red-scene-95b6ea/z-slug/t.resize(h:200,w:100,fill:999)~erase.bg()~t.extend()~p:preset1(a:200,b:1.2,c:test)/__playground/playground-default.jpeg"));
            obj.version = "v1";
            generatedUrl = ObjToUrl(obj);
            Assert.That(generatedUrl, Is.EqualTo("https://cdn.pixelbin.io/v1/red-scene-95b6ea/z-slug/t.resize(h:200,w:100,fill:999)~erase.bg()~t.extend()~p:preset1(a:200,b:1.2,c:test)/__playground/playground-default.jpeg"));
        }

        [Test]
        [Category("URL-UrlObj convertion")]
        [TestCase(TestName = "generate url from obj - 3")]
        public void Test_40()
        {
            List<UrlTransformation> transformations = new List<UrlTransformation>() {
                    new(
                        plugin: "t",
                        name: "resize",
                        values: new List<Dictionary<string, string>>() {
                            new() {{"key", "h"}, {"value", "200"}},
                            new() {{"key", "w"}, {"value", "100"}},
                            new() {{"key", "fill"}, {"value", "999"}}
                        }
                    ),
                    new(
                        plugin: "erase",
                        name: "bg"
                    ),
                    new(
                        plugin: "t",
                        name: "extend"
                    ),
                    new(
                        plugin: "p",
                        name: "preset1",
                        values: new List<Dictionary<string, string>>()
                    )
                };
            UrlObj obj = new UrlObj(
                cloudName: "red-scene-95b6ea",
                zone: "z-slug",
                version: "v2",
                transformations: transformations,
                baseUrl: "https://cdn.pixelbin.io",
                filePath: "__playground/playground-default.jpeg"
            );

            string generatedUrl = ObjToUrl(obj);
            Assert.That(generatedUrl, Is.EqualTo("https://cdn.pixelbin.io/v2/red-scene-95b6ea/z-slug/t.resize(h:200,w:100,fill:999)~erase.bg()~t.extend()~p:preset1/__playground/playground-default.jpeg"));
            obj.version = "v1";
            generatedUrl = ObjToUrl(obj);
            Assert.That(generatedUrl, Is.EqualTo("https://cdn.pixelbin.io/v1/red-scene-95b6ea/z-slug/t.resize(h:200,w:100,fill:999)~erase.bg()~t.extend()~p:preset1/__playground/playground-default.jpeg"));
        }

        [Test]
        [Category("URL-UrlObj convertion")]
        [TestCase(TestName = "obj to url then same url to obj - wrkr - 1")]
        public void Test_41()
        {
            string url = "https://cdn.pixelbin.io/v2/red-scene-95b6ea/wrkr/image.jpeg";
            UrlObj obj = UrlToObj(url);
            UrlObj expectedObj = new UrlObj(
                transformations: new List<UrlTransformation>(),
                cloudName: "red-scene-95b6ea",
                zone: "",
                baseUrl: "https://cdn.pixelbin.io",
                pattern: "",
                filePath: "",
                version: "v2",
                worker: true,
                workerPath: "image.jpeg",
                options: new UrlObjOptions()
            );

            Assert.That(obj, Is.EqualTo(expectedObj));
            string generatedUrl = ObjToUrl(obj);
            Assert.That(generatedUrl, Is.EqualTo(url));
        }

        [Test]
        [Category("URL-UrlObj convertion")]
        [TestCase(TestName = "obj to url then same url to obj - wrkr - 2")]
        public void Test_42()
        {
            string url = "https://cdn.pixelbin.io/v2/falling-surf-7c8bb8/abcdef/wrkr/misc/general/free/original/images/favicon.ico";
            UrlObj obj = UrlToObj(url);
            UrlObj expectedObj = new UrlObj(
                transformations: new List<UrlTransformation>(),
                cloudName: "falling-surf-7c8bb8",
                zone: "abcdef",
                baseUrl: "https://cdn.pixelbin.io",
                pattern: "",
                filePath: "",
                version: "v2",
                worker: true,
                workerPath: "misc/general/free/original/images/favicon.ico",
                options: new UrlObjOptions()
            );

            Assert.That(obj, Is.EqualTo(expectedObj));
            string generatedUrl = ObjToUrl(obj);
            Assert.That(generatedUrl, Is.EqualTo(url));
        }

        [Test]
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
            Assert.Throws<PDKIllegalQueryParameterError>(() => ObjToUrl(obj));
        }

        [Test]
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
            Assert.Throws<PDKIllegalQueryParameterError>(() => ObjToUrl(obj));
        }

    }

    [TestFixture]
    public class CustomDomain
    {
        [Test]
        [Category("URL-UrlObj convertion")]
        [TestCase(TestName = "get obj from url - default zone with transformations - filedepth 1")]
        public void Test_1()
        {
            string url = "https://cdn.twist.vision/v2/t.resize()/playground-default.jpeg";
            UrlObj obj = UrlToObj(url, new UrlConfig(isCustomDomain: true));
            UrlObj expectedObj = new UrlObj(
                transformations: new List<UrlTransformation>() {
                    new(
                        plugin: "t",
                        name: "resize"
                    )
                },
                cloudName: "",
                zone: "",
                baseUrl: "https://cdn.twist.vision",
                pattern: "t.resize()",
                version: "v2",
                filePath: "playground-default.jpeg",
                worker: false,
                workerPath: "",
                options: new UrlObjOptions()
            );

            Assert.That(obj, Is.EqualTo(expectedObj));
        }

        [Test]
        [Category("URL-UrlObj convertion")]
        [TestCase(TestName = "get obj from url - default zone with transformations - filedepth > 1")]
        public void Test_2()
        {
            string url = "https://cdn.twist.vision/v2/t.resize()/test/__playground/playground-default.jpeg";
            UrlObj obj = UrlToObj(url, new UrlConfig(isCustomDomain: true));
            UrlObj expectedObj = new UrlObj(
                transformations: new List<UrlTransformation>() {
                    new(
                        plugin: "t",
                        name: "resize"
                    )
                },
                cloudName: "",
                zone: "",
                baseUrl: "https://cdn.twist.vision",
                pattern: "t.resize()",
                version: "v2",
                filePath: "test/__playground/playground-default.jpeg",
                worker: false,
                workerPath: "",
                options: new UrlObjOptions()
            );

            Assert.That(obj, Is.EqualTo(expectedObj));
        }

        [Test]
        [Category("URL-UrlObj convertion")]
        [TestCase(TestName = "get obj from url - default zone with transformations with param")]
        public void Test_3()
        {
            string url = "https://cdn.twist.vision/v2/t.rotate(a:102)/__playground/playground-default.jpeg";
            UrlObj obj = UrlToObj(url, new UrlConfig(isCustomDomain: true));
            UrlObj expectedObj = new UrlObj(
                transformations: new List<UrlTransformation>() {
                    new(
                        plugin: "t",
                        name: "rotate",
                        values: new List<Dictionary<string, string>>() {
                            new() {{"key", "a"}, {"value", "102"}}
                        }
                    )
                },
                cloudName: "",
                zone: "",
                baseUrl: "https://cdn.twist.vision",
                pattern: "t.rotate(a:102)",
                version: "v2",
                filePath: "__playground/playground-default.jpeg",
                worker: false,
                workerPath: "",
                options: new UrlObjOptions()
            );

            Assert.That(obj, Is.EqualTo(expectedObj));
        }

        [Test]
        [Category("URL-UrlObj convertion")]
        [TestCase(TestName = "get obj from url - default zone with transformations with params")]
        public void Test_4()
        {
            string url = "https://cdn.twist.vision/v2/t.rotate(a:102,b:200)/__playground/playground-default.jpeg";
            UrlObj obj = UrlToObj(url, new UrlConfig(isCustomDomain: true));
            UrlObj expectedObj = new UrlObj(
                transformations: new List<UrlTransformation>() {
                    new(
                        plugin: "t",
                        name: "rotate",
                        values: new List<Dictionary<string, string>>() {
                            new() {{"key", "a"}, {"value", "102"}},
                            new() {{"key", "b"}, {"value", "200"}}
                        }
                    )
                },
                cloudName: "",
                zone: "",
                baseUrl: "https://cdn.twist.vision",
                pattern: "t.rotate(a:102,b:200)",
                version: "v2",
                filePath: "__playground/playground-default.jpeg",
                worker: false,
                workerPath: "",
                options: new UrlObjOptions()
            );

            Assert.That(obj, Is.EqualTo(expectedObj));
        }

        [Test]
        [Category("URL-UrlObj convertion")]
        [TestCase(TestName = "get obj from url with zoneslug - filedepth 1")]
        public void Test_5()
        {
            string url = "https://cdn.twist.vision/v2/zonesl/t.resize()/playground-default.jpeg";
            UrlObj obj = UrlToObj(url, new UrlConfig(isCustomDomain: true));
            UrlObj expectedObj = new UrlObj(
                transformations: new List<UrlTransformation>() {
                    new(
                        plugin: "t",
                        name: "resize"
                    )
                },
                cloudName: "",
                zone: "zonesl",
                baseUrl: "https://cdn.twist.vision",
                pattern: "t.resize()",
                version: "v2",
                filePath: "playground-default.jpeg",
                worker: false,
                workerPath: "",
                options: new UrlObjOptions()
            );

            Assert.That(obj, Is.EqualTo(expectedObj));
        }

        [Test]
        [Category("URL-UrlObj convertion")]
        [TestCase(TestName = "get obj from url with zoneslug - filedepth > 1")]
        public void Test_6()
        {
            string url = "https://cdn.twist.vision/v2/zonesl/t.resize()/test/__playground/playground-default.jpeg";
            UrlObj obj = UrlToObj(url, new UrlConfig(isCustomDomain: true));
            UrlObj expectedObj = new UrlObj(
                transformations: new List<UrlTransformation>() {
                    new(
                        plugin: "t",
                        name: "resize"
                    )
                },
                cloudName: "",
                zone: "zonesl",
                baseUrl: "https://cdn.twist.vision",
                pattern: "t.resize()",
                version: "v2",
                filePath: "test/__playground/playground-default.jpeg",
                worker: false,
                workerPath: "",
                options: new UrlObjOptions()
            );

            Assert.That(obj, Is.EqualTo(expectedObj));
        }

        [Test]
        [Category("URL-UrlObj convertion")]
        [TestCase(TestName = "get obj from url - error")]
        public void Test_7()
        {
            string url = "https://cdn.twist.vision/v2";
            var exception = Assert.Throws<PDKInvalidUrlError>(() => UrlToObj(url, new UrlConfig(isCustomDomain: true)));
            Assert.That(exception.Message, Is.EqualTo("Invalid pixelbin url. Please make sure the url is correct."));
        }

        [Test]
        [Category("URL-UrlObj convertion")]
        [TestCase(TestName = "get obj from url - multiple transformations")]
        public void Test_8()
        {
            string url = "https://cdn.twist.vision/v2/t.resize(h:200,w:100,fill:999)~erase.bg()~t.extend()/__playground/playground-default.jpeg";
            UrlObj obj = UrlToObj(url, new UrlConfig(isCustomDomain: true));
            UrlObj expectedObj = new UrlObj(
                transformations: new List<UrlTransformation>() {
                    new(
                        plugin: "t",
                        name: "resize",
                        values: new List<Dictionary<string, string>>() {
                            new() {{"key", "h"}, {"value", "200"}},
                            new() {{"key", "w"}, {"value", "100"}},
                            new() {{"key", "fill"}, {"value", "999"}}
                        }
                    ),
                    new(
                        plugin: "erase",
                        name: "bg"
                    ),
                    new(
                        plugin: "t",
                        name: "extend"
                    )
                },
                cloudName: "",
                zone: "",
                baseUrl: "https://cdn.twist.vision",
                pattern: "t.resize(h:200,w:100,fill:999)~erase.bg()~t.extend()",
                version: "v2",
                filePath: "__playground/playground-default.jpeg",
                worker: false,
                workerPath: "",
                options: new UrlObjOptions()
            );
            Assert.That(obj, Is.EqualTo(expectedObj));
        }

        [Test]
        [Category("URL-UrlObj convertion")]
        [TestCase(TestName = "get obj from url with preset")]
        public void Test_9()
        {
            string url = "https://cdn.twist.vision/v2/z-slug/t.compress()~t.resize()~t.extend()~p.apply(n:presetNameXyx)/alien_fig_tree_planet_x_wallingford_seattle_washington_usa_517559.jpeg";
            UrlObj obj = UrlToObj(url, new UrlConfig(isCustomDomain: true));
            UrlObj expectedObj = new UrlObj(
                transformations: new List<UrlTransformation>() {
                    new(
                        plugin: "t",
                        name: "compress"
                    ),
                    new(
                        plugin: "t",
                        name: "resize"
                    ),
                    new(
                        plugin: "t",
                        name: "extend"
                    ),
                    new(
                        plugin: "p",
                        name: "apply",
                        values: new List<Dictionary<string, string>>() {
                            new() { { "key", "n" }, {"value", "presetNameXyx"}}
                        }
                    )
                },
                cloudName: "",
                zone: "z-slug",
                baseUrl: "https://cdn.twist.vision",
                pattern: "t.compress()~t.resize()~t.extend()~p.apply(n:presetNameXyx)",
                filePath: "alien_fig_tree_planet_x_wallingford_seattle_washington_usa_517559.jpeg",
                version: "v2",
                worker: false,
                workerPath: "",
                options: new UrlObjOptions()
            );
            Assert.That(obj, Is.EqualTo(expectedObj));
        }

        [Test]
        [Category("URL-UrlObj convertion")]
        [TestCase(TestName = "get obj from url with preset - 1")]
        public void Test_10()
        {
            string url = "https://cdn.twist.vision/v3/t.compress()~t.resize()~t.extend()~p.apply(n:presetNameXyx)/alien_fig_tree_planet_x_wallingford_seattle_washington_usa_517559.jpeg";
            var exception = Assert.Throws<PDKInvalidUrlError>(() => UrlToObj(url));
            Assert.That(exception.Message, Is.EqualTo("Invalid pixelbin url. Please make sure the url is correct."));
        }

        [Test]
        [Category("URL-UrlObj convertion")]
        [TestCase(TestName = "handle incorrect urls")]
        public void Test_11()
        {
            string url = "https://cdn.twist.vision//v2/original~original/__playground/playground-default.jpeg";
            var exception = Assert.Throws<PDKInvalidUrlError>(() => UrlToObj(url));
            Assert.That(exception.Message, Is.EqualTo("Invalid pixelbin url. Please make sure the url is correct."));
        }

        [Test]
        [Category("URL-UrlObj convertion")]
        [TestCase(TestName = "handle incorrect urls - incorrect zone")]
        public void Test_12()
        {
            string url = "https://cdn.twist.vision/v2/test/t.compress()~t.resize()~t.extend()~p.apply(n:presetNameXyx)/alien_fig_tree_planet_x_wallingford_seattle_washington_usa_517559.jpeg";
            var exception = Assert.Throws<PDKInvalidUrlError>(() => UrlToObj(url, new UrlConfig(isCustomDomain: true)));
            Assert.That(exception.Message, Is.EqualTo("Error Processing url. Please check the url is correct"));
        }

        [Test]
        [Category("URL-UrlObj convertion")]
        [TestCase(TestName = "handle incorrect urls - incorrect pattern")]
        public void Test_13()
        {
            string url = "https://cdn.pixelbin.io/v2/red-scene-95b6ea/t.compress~t.resize()~t.extend()~p.apply(n:presetNameXyx)/alien_fig_tree_planet_x_wallingford_seattle_washington_usa_517559.jpeg";
            var exception = Assert.Throws<PDKInvalidUrlError>(() => UrlToObj(url, new UrlConfig(isCustomDomain: true)));
            Assert.That(exception.Message, Is.EqualTo("Error Processing url. Please check the url is correct"));
        }

        [Test]
        [Category("URL-UrlObj convertion")]
        [TestCase(TestName = "handle incorrect urls - incorrect pattern - 2")]
        public void Test_14()
        {
            string url = "https://cdn.twist.vision/v2/zonesls/t.resize()/__playground/playground-default.jpeg";
            var exception = Assert.Throws<PDKInvalidUrlError>(() => UrlToObj(url, new UrlConfig(isCustomDomain: true)));
            Assert.That(exception.Message, Is.EqualTo("Error Processing url. Please check the url is correct"));
        }

        [Test]
        [Category("URL-UrlObj convertion")]
        [TestCase(TestName = "get obj from url - default zone url - 6 character length path segment")]
        public void Test_15()
        {
            string url = "https://cdn.twist.vision/v2/original/z0/orgs/33/skills/icons/Fynd_Platform_Commerce_Extension.png";
            UrlObj obj = UrlToObj(url, new UrlConfig(isCustomDomain: true));
            UrlObj expectedObj = new UrlObj(
                transformations: new List<UrlTransformation>(),
                cloudName: "",
                zone: "",
                baseUrl: "https://cdn.twist.vision",
                pattern: "original",
                filePath: "z0/orgs/33/skills/icons/Fynd_Platform_Commerce_Extension.png",
                version: "v2",
                worker: false,
                workerPath: "",
                options: new UrlObjOptions()
            );
            Assert.That(obj, Is.EqualTo(expectedObj));
        }

        [Test]
        [Category("URL-UrlObj convertion")]
        [TestCase(TestName = "get obj from url - default zone url - path segment having 'wrkr'")]
        public void Test_16()
        {
            string url = "https://cdn.twist.vision/v2/original/z0/orgs/33/wrkr/icons/Fynd_Platform_Commerce_Extension.png";
            UrlObj obj = UrlToObj(url, new UrlConfig(isCustomDomain: true));
            UrlObj expectedObj = new UrlObj(
                transformations: new List<UrlTransformation>(),
                cloudName: "",
                zone: "",
                baseUrl: "https://cdn.twist.vision",
                pattern: "original",
                filePath: "z0/orgs/33/wrkr/icons/Fynd_Platform_Commerce_Extension.png",
                version: "v2",
                worker: false,
                workerPath: "",
                options: new UrlObjOptions()
            );
            Assert.That(obj, Is.EqualTo(expectedObj));
        }

        [Test]
        [Category("URL-UrlObj convertion")]
        [TestCase(TestName = "get obj from url - worker path - fullpath with depth 1")]
        public void Test_17()
        {
            string url = "https://cdn.twist.vision/v2/wrkr/image.jpeg";
            UrlObj obj = UrlToObj(url, new UrlConfig(isCustomDomain: true));
            UrlObj expectedObj = new UrlObj(
                transformations: new List<UrlTransformation>(),
                cloudName: "",
                zone: "",
                baseUrl: "https://cdn.twist.vision",
                pattern: "",
                filePath: "",
                version: "v2",
                worker: true,
                workerPath: "image.jpeg",
                options: new UrlObjOptions()
            );
            Assert.That(obj, Is.EqualTo(expectedObj));
        }

        [Test]
        [Category("URL-UrlObj convertion")]
        [TestCase(TestName = "get obj from url - worker path - fullpath with depth > 1")]
        public void Test_18()
        {
            string url = "https://cdn.twist.vision/v2/wrkr/misc/general/free/original/images/favicon.ico";
            UrlObj obj = UrlToObj(url, new UrlConfig(isCustomDomain: true));
            UrlObj expectedObj = new UrlObj(
                transformations: new List<UrlTransformation>(),
                cloudName: "",
                zone: "",
                baseUrl: "https://cdn.twist.vision",
                pattern: "",
                filePath: "",
                version: "v2",
                worker: true,
                workerPath: "misc/general/free/original/images/favicon.ico",
                options: new UrlObjOptions()
            );
            Assert.That(obj, Is.EqualTo(expectedObj));
        }

        [Test]
        [Category("URL-UrlObj convertion")]
        [TestCase(TestName = "get obj from url - worker path with zone - fullpath with depth 1")]
        public void Test_19()
        {
            string url = "https://cdn.twist.vision/v2/fyndnp/wrkr/robots.txt";
            UrlObj obj = UrlToObj(url, new UrlConfig(isCustomDomain: true));
            UrlObj expectedObj = new UrlObj(
                    transformations: new List<UrlTransformation>(),
                    cloudName: "",
                    zone: "fyndnp",
                    baseUrl: "https://cdn.twist.vision",
                    pattern: "",
                    filePath: "",
                    version: "v2",
                    worker: true,
                    workerPath: "robots.txt",
                    options: new UrlObjOptions()
                );
            Assert.That(obj, Is.EqualTo(expectedObj));
        }

        // ref -> https://fynd-f7.sentry.io/issues/3515703764/?project=6193211&referrer=slack
        [Test]
        [Category("URL-UrlObj convertion")]
        [TestCase(TestName = "get obj from url - worker path with zone - fullpath with depth > 1")]
        public void Test_20()
        {
            string url = "https://cdn.twist.vision/v2/fyprod/wrkr/misc/general/free/original/images/favicon.ico";
            UrlObj obj = UrlToObj(url, new UrlConfig(isCustomDomain: true));
            UrlObj expectedObj = new UrlObj(
                transformations: new List<UrlTransformation>(),
                cloudName: "",
                zone: "fyprod",
                baseUrl: "https://cdn.twist.vision",
                pattern: "",
                filePath: "",
                version: "v2",
                worker: true,
                workerPath: "misc/general/free/original/images/favicon.ico",
                options: new UrlObjOptions()
            );
            Assert.That(obj, Is.EqualTo(expectedObj));
        }

        [Test]
        [Category("URL-UrlObj convertion")]
        [TestCase(TestName = "get obj from url with options if available")]
        public void Test_21()
        {
            string url = "https://cdn.twist.vision/v2/feelzz/erase.bg(shadow:true)~t.merge(m:underlay,i:eU44YkFJOHlVMmZrWVRDOUNTRm1D,b:screen,r:true)/MZZKB3e1hT48o0NYJ2Kxh.jpeg?dpr=2.5&f_auto=true";
            UrlObj obj = UrlToObj(url, new UrlConfig(isCustomDomain: true));
            UrlObj expectedObj = new UrlObj(
                baseUrl: "https://cdn.twist.vision",
                filePath: "MZZKB3e1hT48o0NYJ2Kxh.jpeg",
                pattern: "erase.bg(shadow:true)~t.merge(m:underlay,i:eU44YkFJOHlVMmZrWVRDOUNTRm1D,b:screen,r:true)",
                version: "v2",
                cloudName: "",
                zone: "feelzz",
                options: new UrlObjOptions(
                    dpr: 2.5,
                    f_auto: true
                ),
                transformations: new List<UrlTransformation>() {
                        new(
                            plugin: "erase",
                            name: "bg",
                            values: new List<Dictionary<string, string>>() {
                                new() { {"key", "shadow"}, {"value", "true"} },
                            }
                        ),
                        new(
                            plugin: "t",
                            name: "merge",
                            values: new List<Dictionary<string, string>>() {
                                new() { {"key", "m"}, {"value", "underlay"} },
                                new () { {"key", "i"}, {"value", "eU44YkFJOHlVMmZrWVRDOUNTRm1D"} },
                                new () { {"key", "b"}, {"value", "screen"} },
                                new() { {"key", "r"}, {"value", "true"} },
                            }
                        )
                    },
                    worker: false,
                    workerPath: ""
                );
            Assert.That(obj, Is.EqualTo(expectedObj));
        }

        [Test]
        [Category("URL-UrlObj convertion")]
        [TestCase(TestName = "get failure while retrieving obj from url with invalid options")]
        public void Test_22()
        {
            string url = "https://cdn.twist.vision/v2/erase.bg(shadow:true)~t.merge(m:underlay,i:eU44YkFJOHlVMmZrWVRDOUNTRm1D,b:screen,r:true)/MZZKB3e1hT48o0NYJ2Kxh.jpeg?dpr=5.5&f_auto=true";
            Assert.Throws<PDKIllegalQueryParameterError>(() => UrlToObj(url, new UrlConfig(isCustomDomain: true)));
        }

        [Test]
        [Category("URL-UrlObj convertion")]
        [TestCase(TestName = "generate url from obj with options if available")]
        public void Test_23()
        {
            UrlObj obj = new UrlObj(
                    baseUrl: "https://cdn.twist.vision",
                    isCustomDomain: true,
                    filePath: "__playground/playground-default.jpeg",
                    version: "v2",
                    zone: "z-slug",
                    options: new UrlObjOptions(dpr: 2.5, f_auto: true),
                    transformations: new List<UrlTransformation>() { new() }
                );
            string generatedUrl = ObjToUrl(obj);
            Assert.That(generatedUrl, Is.EqualTo("https://cdn.twist.vision/v2/z-slug/original/__playground/playground-default.jpeg?dpr=2.5&f_auto=true"));
        }

        [Test]
        [Category("URL-UrlObj convertion")]
        [TestCase(TestName = "get failure while retrieving url from obj with cloudname")]
        public void Test_24()
        {
            UrlObj obj = new UrlObj(
                    baseUrl: "https://cdn.twist.vision",
                    isCustomDomain: true,
                    filePath: "__playground/playground-default.jpeg",
                    version: "v2",
                    zone: "z-slug",
                    cloudName: "red-scene-95b6ea",
                    options: new UrlObjOptions(dpr: 2.5, f_auto: true),
                    transformations: new List<UrlTransformation>() { new() }
                );
            var exception = Assert.Throws<PDKIllegalArgumentError>(() => ObjToUrl(obj));
        }

        [Test]
        [Category("URL-UrlObj convertion")]
        [TestCase(TestName = "get failure while retrieving url from obj with invalid options")]
        public void Test_25()
        {
            UrlObj obj = new UrlObj(
                baseUrl: "https://cdn.twist.vision",
                isCustomDomain: true,
                filePath: "__playground/playground-default.jpeg",
                version: "v2",
                zone: "z-slug",
                options: new UrlObjOptions(dpr: 2.5, f_auto: "abc"),
                transformations: new List<UrlTransformation>() { new() }
            );
            var exception = Assert.Throws<PDKIllegalQueryParameterError>(() => ObjToUrl(obj));
        }

        // presets variable support
        [Test]
        [Category("URL-UrlObj convertion")]
        [TestCase(TestName = "get obj from url - 1")]
        public void Test_26()
        {
            string url = "https://cdn.twist.vision/v2/t.rotate(a:102)~p:preset1(a:100,b:2.1,c:test)/__playground/playground-default.jpeg";
            UrlObj obj = UrlToObj(url, new UrlConfig(isCustomDomain: true));
            UrlObj expectedObj = new UrlObj(
                transformations: new List<UrlTransformation>() {
                    new (
                        plugin: "t",
                        name: "rotate",
                        values: new List<Dictionary<string, string>>() {
                            new() { {"key", "a"}, {"value", "102"} }
                        }
                    ),
                    new (
                        plugin: "p",
                        name: "preset1",
                        values: new List<Dictionary<string, string>>() {
                            new () { {"key", "a"}, {"value", "100"} },
                            new () { {"key", "b"}, {"value", "2.1"} },
                            new () { {"key", "c"}, {"value", "test"} }
                        }
                    ),
                },
                zone: "",
                baseUrl: "https://cdn.twist.vision",
                pattern: "t.rotate(a:102)~p:preset1(a:100,b:2.1,c:test)",
                version: "v2",
                filePath: "__playground/playground-default.jpeg",
                options: new UrlObjOptions()
            );
            Assert.That(obj, Is.EqualTo(expectedObj));
        }

        [Test]
        [Category("URL-UrlObj convertion")]
        [TestCase(TestName = "get obj from url - 2")]
        public void Test_27()
        {
            string url = "https://cdn.twist.vision/v2/t.rotate(a:102)~p:preset1/__playground/playground-default.jpeg";
            UrlObj obj = UrlToObj(url, new UrlConfig(isCustomDomain: true));
            UrlObj expectedObj = new UrlObj(
                transformations: new List<UrlTransformation>() {
                    new(
                        plugin: "t",
                        name: "rotate",
                        values: new List<Dictionary<string, string>>() {
                            new() {
                                {"key", "a"},
                                {"value", "102"}
                            },
                        }
                    ),
                    new(
                        plugin: "p",
                        name: "preset1"
                    ),
                },
                zone: "",
                baseUrl: "https://cdn.twist.vision",
                pattern: "t.rotate(a:102)~p:preset1",
                version: "v2",
                filePath: "__playground/playground-default.jpeg",
                options: new UrlObjOptions()
            );
            Assert.That(obj, Is.EqualTo(expectedObj));
        }

        [Test]
        [Category("URL-UrlObj convertion")]
        [TestCase(TestName = "get obj from url - 3")]
        public void Test_28()
        {
            string url = "https://cdn.twist.vision/v2/t.rotate(a:102)~p:preset1()/__playground/playground-default.jpeg";
            UrlObj obj = UrlToObj(url, new UrlConfig(isCustomDomain: true));
            UrlObj expectedObj = new UrlObj(
                transformations: new List<UrlTransformation>() {
                    new(
                        plugin: "t",
                        name: "rotate",
                        values: new List<Dictionary<string, string>>() {
                            new() {
                                {"key", "a"},
                                {"value", "102"},
                            }
                        }
                    ),
                    new(
                        plugin: "p",
                        name: "preset1"
                    ),
                },
                zone: "",
                baseUrl: "https://cdn.twist.vision",
                pattern: "t.rotate(a:102)~p:preset1()",
                version: "v2",
                filePath: "__playground/playground-default.jpeg",
                options: new UrlObjOptions()
                );
            Assert.That(obj, Is.EqualTo(expectedObj));
        }

        [Test]
        [Category("URL-UrlObj convertion")]
        [TestCase(TestName = "get obj from url - 4")]
        public void Test_29()
        {
            string url = "https://cdn.twist.vision/v2/t.rotate(a:102)~p:preset1(a:12/__playground/playground-default.jpeg";
            UrlObj obj = UrlToObj(url, new UrlConfig(isCustomDomain: true));
            UrlObj expectedObj = new UrlObj(
                transformations: new List<UrlTransformation>() {
                    new(
                        plugin: "t",
                        name: "rotate",
                        values: new List<Dictionary<string, string>>() {
                            new() {
                                {"key", "a"},
                                {"value", "102"},
                            },
                        }
                    ),
                    new(
                        plugin: "p",
                        name: "preset1",
                        values: new List<Dictionary<string, string>>() {
                            new() {
                                {"key", "a"},
                                {"value", "12"},
                            },
                        }
                    ),
                },
                zone: "",
                baseUrl: "https://cdn.twist.vision",
                pattern: "t.rotate(a:102)~p:preset1(a:12",
                version: "v2",
                filePath: "__playground/playground-default.jpeg",
                options: new UrlObjOptions()
            );
            Assert.That(obj, Is.EqualTo(expectedObj));
        }

        [Test]
        [Category("URL-UrlObj convertion")]
        [TestCase(TestName = "generate url from obj")]
        public void Test_30()
        {
            List<UrlTransformation> transformations = new List<UrlTransformation>() {
                new(
                    plugin: "t",
                    name: "resize",
                    values: new List<Dictionary<string, string>>() {
                        new() {
                            {"key", "h"},
                            {"value", "200"}
                        },
                        new() {
                            {"key", "w"},
                            {"value", "100"}
                        },
                        new() {
                            {"key", "fill"},
                            {"value", "999"}
                        },
                    }
                ),
                new(
                    plugin: "erase",
                    name: "bg"
                ),
                new(
                    plugin: "t",
                    name: "extend"
                ),
                new(
                    plugin: "p",
                    name: "preset1"
                ),
            };
            UrlObj obj = new UrlObj(
                isCustomDomain: true,
                zone: "z-slug",
                version: "v2",
                transformations: transformations,
                baseUrl: "https://cdn.twist.vision",
                filePath: "__playground/playground-default.jpeg",
                options: new UrlObjOptions()
            );
            string generatedUrl = ObjToUrl(obj);
            Assert.That(generatedUrl, Is.EqualTo("https://cdn.twist.vision/v2/z-slug/t.resize(h:200,w:100,fill:999)~erase.bg()~t.extend()~p:preset1/__playground/playground-default.jpeg"));
        }

        [Test]
        [Category("URL-UrlObj convertion")]
        [TestCase(TestName = "generate url from obj - 1")]
        public void Test_31()
        {
            List<UrlTransformation> transformations = new List<UrlTransformation>() {
                    new(
                        plugin: "t",
                        name: "resize",
                        values: new List<Dictionary<string, string>>() {
                            new() {
                                {"key", "h"},
                                {"value", "200"}
                            },
                            new() {
                                {"key", "w"},
                                {"value", "100"}
                            },
                            new() {
                                {"key", "fill"},
                                {"value", "999"}
                            },
                        }
                    ),
                    new(
                        plugin: "erase",
                        name: "bg",
                        values: new List<Dictionary<string, string>>(){
                            new() {
                                {"key", "i"},
                                {"value", "general"}
                            },
                        }
                    ),
                    new(
                        plugin: "t",
                        name: "extend"
                    ),
                    new(
                        plugin: "p",
                        name: "preset1"
                    ),
                };
            UrlObj obj = new UrlObj(
                isCustomDomain: true,
                zone: "z-slug",
                version: "v2",
                transformations: transformations,
                baseUrl: "https://cdn.twist.vision",
                filePath: "__playground/playground-default.jpeg"
                );
            string generatedUrl = ObjToUrl(obj);
            Assert.That(generatedUrl, Is.EqualTo("https://cdn.twist.vision/v2/z-slug/t.resize(h:200,w:100,fill:999)~erase.bg(i:general)~t.extend()~p:preset1/__playground/playground-default.jpeg"));
        }

        [Test]
        [Category("URL-UrlObj convertion")]
        [TestCase(TestName = "throw error if worker is true but workerPath is not defined")]
        public void Test_32()
        {
            List<UrlTransformation> transformations = new();
            UrlObj obj = new UrlObj(
                isCustomDomain: true,
                zone: "z-slug",
                version: "v2",
                transformations: transformations,
                baseUrl: "https://cdn.twist.vision",
                filePath: "__playground/playground-default.jpeg",
                worker: true
            );
            var exception = Assert.Throws<PDKIllegalArgumentError>(() => ObjToUrl(obj));
            Assert.That(exception.Message, Is.EqualTo("key workerPath should be defined"));
        }

        [Test]
        [Category("URL-UrlObj convertion")]
        [TestCase(TestName = "generate url from obj when empty")]
        public void Test_33()
        {
            List<UrlTransformation> transformations = new();
            UrlObj obj = new UrlObj(
                isCustomDomain: true,
                zone: "z-slu1",
                version: "v2",
                transformations: transformations,
                baseUrl: "https://cdn.twist.vision",
                filePath: "__playground/playground-default.jpeg"
            );
            string generatedUrl = ObjToUrl(obj);
            Assert.That(generatedUrl, Is.EqualTo("https://cdn.twist.vision/v2/z-slu1/original/__playground/playground-default.jpeg"));
        }

        [Test]
        [Category("URL-UrlObj convertion")]
        [TestCase(TestName = "generate url from obj undefined")]
        public void Test_34()
        {
            UrlObj obj = new UrlObj(
                isCustomDomain: true,
                zone: "z-slug",
                version: "v2",
                baseUrl: "https://cdn.twist.vision",
                filePath: "__playground/playground-default.jpeg"
            );
            string generatedUrl = ObjToUrl(obj);
            Assert.That(generatedUrl, Is.EqualTo("https://cdn.twist.vision/v2/z-slug/original/__playground/playground-default.jpeg"));
        }

        [Test]
        [Category("URL-UrlObj convertion")]
        [TestCase(TestName = "generate url from obj  empty object")]
        public void Test_35()
        {
            UrlObj obj = new UrlObj(
                isCustomDomain: true,
                zone: "z-slug",
                version: "v2",
                transformations: new List<UrlTransformation>() { new() },
                baseUrl: "https://cdn.twist.vision",
                filePath: "__playground/playground-default.jpeg"
            );
            string generatedUrl = ObjToUrl(obj);
            Assert.That(generatedUrl, Is.EqualTo("https://cdn.twist.vision/v2/z-slug/original/__playground/playground-default.jpeg"));
        }

        [Test]
        [Category("URL-UrlObj convertion")]
        [TestCase(TestName = "generate url from obj  empty object - 1")]
        public void Test_36()
        {
            UrlObj obj = new UrlObj(
                isCustomDomain: true,
                zone: "z-slug",
                version: "v2",
                transformations: new List<UrlTransformation>() { new() },
                baseUrl: "https://cdn.twist.vision",
                filePath: "__playground/playground-default.jpeg"
            );
            string generatedUrl = ObjToUrl(obj);
            Assert.That(generatedUrl, Is.EqualTo("https://cdn.twist.vision/v2/z-slug/original/__playground/playground-default.jpeg"));
        }

        [Test]
        [Category("URL-UrlObj convertion")]
        [TestCase(TestName = "throw error to generate url from obj if filePath not defined")]
        public void Test_37()
        {
            UrlObj obj = new UrlObj(
                isCustomDomain: true,
                zone: "z-slug",
                version: "v2",
                transformations: new List<UrlTransformation>() { new() },
                baseUrl: "https://cdn.twist.vision",
                filePath: ""
            );
            Assert.Throws<PDKIllegalArgumentError>(() => ObjToUrl(obj));
        }

        [Test]
        [Category("URL-UrlObj convertion")]
        [TestCase(TestName = "generate url from obj - 2")]
        public void Test_38()
        {
            List<UrlTransformation> transformations = new() {
                new(
                    plugin: "t",
                    name: "resize",
                    values: new List<Dictionary<string, string>>() {
                        new() {
                            {"key", "h"},
                            {"value", "200"}
                        },
                        new() {
                            {"key", "w"},
                            {"value", "100"}
                        },
                        new() {
                            {"key", "fill"},
                            {"value", "999"}
                        }
                    }
                ),
                new(
                    plugin: "erase",
                    name: "bg"
                ),
                new(
                    plugin: "t",
                    name: "extend"
                ),
                new(
                    plugin: "p",
                    name: "preset1",
                    values: new List<Dictionary<string, string>>() {
                        new() {
                            {"key", "a"},
                            {"value", "200"}
                        },
                        new() {
                            {"key", "b"},
                            {"value", "1.2"}
                        },
                        new() {
                            {"key", "c"},
                            {"value", "test"}
                        },
                    }
                )
            };
            UrlObj obj = new UrlObj(
                isCustomDomain: true,
                zone: "z-slug",
                version: "v2",
                transformations: transformations,
                baseUrl: "https://cdn.twist.vision",
                filePath: "__playground/playground-default.jpeg"
            );
            string generatedUrl = ObjToUrl(obj);
            Assert.That(generatedUrl, Is.EqualTo("https://cdn.twist.vision/v2/z-slug/t.resize(h:200,w:100,fill:999)~erase.bg()~t.extend()~p:preset1(a:200,b:1.2,c:test)/__playground/playground-default.jpeg"));
        }

        [Test]
        [Category("URL-UrlObj convertion")]
        [TestCase(TestName = "generate url from obj - 3")]
        public void Test_39()
        {
            List<UrlTransformation> transformations = new() {
                new(
                    plugin: "t",
                    name: "resize",
                    values: new List<Dictionary<string, string>>() {
                        new() {
                            {"key", "h"},
                            {"value", "200"}
                        },
                        new() {
                            {"key", "w"},
                            {"value", "100"}
                        },
                        new() {
                            {"key", "fill"},
                            {"value", "999"}
                        }
                    }
                ),
                new(
                    plugin: "erase",
                    name: "bg"
                ),
                new(
                    plugin: "t",
                    name: "extend"
                ),
                new(
                    plugin: "p",
                    name: "preset1",
                    values: new List<Dictionary<string, string>>()
                ),
            };
            UrlObj obj = new UrlObj(
                isCustomDomain: true,
                zone: "z-slug",
                version: "v2",
                transformations: transformations,
                baseUrl: "https://cdn.twist.vision",
                filePath: "__playground/playground-default.jpeg"
            );
            string generatedUrl = ObjToUrl(obj);
            Assert.That(generatedUrl, Is.EqualTo("https://cdn.twist.vision/v2/z-slug/t.resize(h:200,w:100,fill:999)~erase.bg()~t.extend()~p:preset1/__playground/playground-default.jpeg"));
        }

        [Test]
        [Category("URL-UrlObj convertion")]
        [TestCase(TestName = "obj to url then same url to obj - wrkr - 1")]
        public void Test_40()
        {
            string url = "https://cdn.twist.vision/v2/wrkr/image.jpeg";
            UrlObj obj = UrlToObj(url, new UrlConfig(isCustomDomain: true));
            UrlObj expectedObj = new UrlObj(
                transformations: new List<UrlTransformation>(),
                cloudName: "",
                zone: "",
                baseUrl: "https://cdn.twist.vision",
                pattern: "",
                filePath: "",
                version: "v2",
                worker: true,
                workerPath: "image.jpeg",
                options: new UrlObjOptions()
            );
            Assert.That(obj, Is.EqualTo(expectedObj));
            obj.isCustomDomain = true;
            string generatedUrl = ObjToUrl(obj);
            Assert.That(generatedUrl, Is.EqualTo(url));
        }

        [Test]
        [Category("URL-UrlObj convertion")]
        [TestCase(TestName = "obj to url then same url to obj - wrkr - 2")]
        public void Test_41()
        {
            string url = "https://cdn.twist.vision/v2/abcdef/wrkr/misc/general/free/original/images/favicon.ico";
            UrlObj obj = UrlToObj(url, new UrlConfig(isCustomDomain: true));
            UrlObj expectedObj = new UrlObj(
                transformations: new List<UrlTransformation>(),
                cloudName: "",
                zone: "abcdef",
                baseUrl: "https://cdn.twist.vision",
                pattern: "",
                filePath: "",
                version: "v2",
                worker: true,
                workerPath: "misc/general/free/original/images/favicon.ico",
                options: new UrlObjOptions()
            );
            Assert.That(obj, Is.EqualTo(expectedObj));
            obj.isCustomDomain = true;
            string generatedUrl = ObjToUrl(obj);
            Assert.That(generatedUrl, Is.EqualTo(url));
        }


        [Test]
        [Category("URL Signing")]
        [TestCase(TestName = "sign url")]
        public void Test_42()
        {
            string signedURL = Security.SignURL(
                "https://cdn.pixelbin.io/v2/dummy-cloudname/original/__playground/playground-default.jpeg",
                20,
                "459337ed-f378-4ddf-bad7-d7a4555c4572",
                "dummy-token"
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
        [Category("URL Signing")]
        [TestCase(TestName = "sign url with query")]
        public void Test_43()
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
        [Category("URL Signing")]
        [TestCase(TestName = "sign url custom domain")]
        public void Test_44()
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
        [Category("URL Signing")]
        [TestCase(TestName = "sign url failure when empty url provided")]
        public void Test_45()
        {
            Assert.Throws<PixelbinIllegalArgumentError>(() => Security.SignURL("", 20, "1", "dummy-token"));
        }

        [Test]
        [Category("URL Signing")]
        [TestCase(TestName = "sign url failure when empty access key provided")]
        public void Test_46()
        {
            Assert.Throws<PixelbinIllegalArgumentError>(() => Security.SignURL("https://cdn.pixelbin.io/v2/dummy-cloudname/original/__playground/playground-default.jpeg", 20, "", "dummy-token"));
        }

        [Test]
        [Category("URL Signing")]
        [TestCase(TestName = "sign url failure when empty token provided")]
        public void Test_47()
        {
            Assert.Throws<PixelbinIllegalArgumentError>(() => Security.SignURL("https://cdn.pixelbin.io/v2/dummy-cloudname/original/__playground/playground-default.jpeg", 20, "1", ""));
        }
    };
}