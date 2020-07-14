# BridgeJsCoreUnity

Run JavaScript on Unity iOS, macOS

## Instructions

- via Unity Package Manager
  - `https://github.com/kyubuns/BridgeJsCoreUnity.git?path=Unity/Assets/BridgeJsCore` or `git@github.com/kyubuns/BridgeJsCoreUnity.git?path=Unity/Assets/BridgeJsCore`

## Getting started

```csharp
var engine = new Engine();
engine.EvaluateScript(@"
function fib(num) {
  if (num <= 2) return 1;
  return fib(num - 1) + fib(num - 2);
}");

var (result, error) = engine.EvaluateScript(@"fib(10);");
Debug.Log(result); // -> 55
```

Supported primitive types, object and array.

```csharp
var (result, error) = engine.EvaluateScript(@"
var obj = new Object();
obj.var1 = ""abc"";
obj.var2 = 123;
obj
");

var jsObject = (JsObject) result;
var var1 = jsObject.ForProperty("var1");
Debug.Log(var1); // -> abc
```

see [Test Code](https://github.com/kyubuns/BridgeJsCoreUnity/blob/master/Unity/Assets/Tests/BridgeJsCoreTest.cs)

## Requirements

- Requires Unity2019.4 or later

## License

MIT License (see [LICENSE](LICENSE))

