using BridgeJsCore;
using NUnit.Framework;

namespace BridgeJsCoreTests
{
    public class SimpleMethodTest
    {
        private Engine engine;

        [SetUp]
        public void Setup()
        {
            engine = new Engine();
            var (_, error) = engine.EvaluateScript(@"
function fib(num) {
  if (num <= 2) return 1;
  return fib(num - 1) + fib(num - 2);
}");
            Assert.IsEmpty(error);
        }

        [TearDown]
        public void TearDown()
        {
            engine.Dispose();
        }

        [Test]
        public void Calc10()
        {
            var (result, error) = engine.EvaluateScript(@"fib(10);");
            Assert.IsEmpty(error);
            Assert.AreEqual(typeof(JsNumber), result.GetType());
            Assert.AreEqual(55, ((JsNumber) result).ToInt32());
        }
    }

    public class StringTest
    {
        [Test]
        public void Japanese()
        {
            using (var engine = new Engine())
            {
                var (result, error) = engine.EvaluateScript(@"
function test() {
  return ""あいう"" + ""えお"";
}

test();
");
                Assert.IsEmpty(error);
                Assert.AreEqual(typeof(JsString), result.GetType());
                Assert.AreEqual("あいうえお", ((JsString) result).ToString());
            }
        }
    }

    public class ObjectTest
    {
        [Test]
        public void GetProperties()
        {
            using (var engine = new Engine())
            {
                var (result, error) = engine.EvaluateScript(@"
var obj = new Object();
obj.var1 = ""abc"";
obj.var2 = 123;
obj
");
                Assert.IsEmpty(error);
                Assert.AreEqual(typeof(JsObject), result.GetType());

                var jsObject = (JsObject) result;

                Assert.IsTrue(jsObject.HasProperty("var1"));
                var var1 = jsObject.ForProperty("var1");
                Assert.AreEqual(typeof(JsString), var1.GetType());
                Assert.AreEqual("abc", ((JsString) var1).ToString());

                Assert.IsTrue(jsObject.HasProperty("var2"));
                var var2 = jsObject.ForProperty("var2");
                Assert.AreEqual(typeof(JsNumber), var2.GetType());
                Assert.AreEqual(123, ((JsNumber) var2).ToInt32());
            }
        }
    }
}
