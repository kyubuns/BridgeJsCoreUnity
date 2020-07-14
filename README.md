# BridgeJsCoreUnity

The fast JavaScript executor for Unity iOS, macOS by using JavaScriptCore.

## Compare

| | BridgeJsCoreUnity | Native(C#) | [jint(js)](https://github.com/sebastienros/jint) | [xLua](https://github.com/Tencent/xLua) |
| --- | --- | --- | --- | --- |
| Factory * 1000Times | 0.40s | 0.00s | 0.09s | 1.00s |
| [Sum1-10000 * 10000Times](https://github.com/kyubuns/HaxeUnityBenchmark/blob/master/Haxe/src/bench/Main.hx#L10) | 1.56s | 0.00s | 36.50s | 2.30s |

The same code is generated in C#, JavaScript and Lua using Haxe.

- [HaxeUnityBenchmark](https://github.com/kyubuns/HaxeUnityBenchmark)
- on iPhone11Pro

## Instructions

- via Unity Package Manager
  - `https://github.com/kyubuns/BridgeJsCoreUnity.git?path=Unity/Assets/BridgeJsCore`
  - `git@github.com/kyubuns/BridgeJsCoreUnity.git?path=Unity/Assets/BridgeJsCore`

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

