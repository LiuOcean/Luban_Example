using System.IO;
using Example;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTest;


public class AssetLoader : IAssetLoader
{
    public string Load(int hash_code, string name)
    {
        string file = Path.Combine("../../../../../../Json/Client", $"{name}.json");

        file = Path.GetFullPath(file);

        return File.ReadAllText(file);
    }

    public void Release(int hash_code) { }
}

[TestClass]
public class ConfigSetUp
{
    [AssemblyInitialize]
    public static void Initialize(TestContext test_context)
    {
        ConfigComponent.Instance.Awake(new ConfigComponentConfig(typeof(AssemblyMarker).Assembly, new AssetLoader()));
        ConfigComponent.Instance.Load();
    }

    [AssemblyCleanup]
    public static void Close() { }
}