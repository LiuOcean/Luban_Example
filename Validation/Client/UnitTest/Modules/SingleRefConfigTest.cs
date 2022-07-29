using Example;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTest;

[TestClass]
public class SingleRefConfigTest
{
    [TestMethod]
    public void ID_Exist()
    {
        var config = ConfigComponent.Instance.Get<SingleRefConfig>(1);

        Assert.IsNotNull(config);
    }
}