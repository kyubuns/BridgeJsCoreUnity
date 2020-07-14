using System;
using System.Runtime.InteropServices;

namespace BridgeJsCore
{
    public static class FastEngine
    {
        [DllImport("__Internal")]
        private static extern void _BridgeJsCoreStatic_Release();

        [DllImport("__Internal")]
        private static extern IntPtr _BridgeJsCoreStatic_EvaluateScript(string script, out string error);

        [DllImport("__Internal")]
        private static extern string _BridgeJsCoreStatic_EvaluateScriptReturnString(string script, out string error);

        [DllImport("__Internal")]
        private static extern void _BridgeJsCoreStatic_EvaluateScriptWithoutReturnValue(string script, out string error);

        [DllImport("__Internal")]
        private static extern void _BridgeJsCoreStatic_EvaluateScriptWithoutReturnValueNoError(string script);

        public static void Release()
        {
            _BridgeJsCoreStatic_Release();
        }

        public static (IJsValue Result, string Error) EvaluateScript(string script)
        {
            var rawJsValuePtr = _BridgeJsCoreStatic_EvaluateScript(script, out var error);
            var jsValue = Engine.ToJsValue(rawJsValuePtr);
            return (jsValue, error);
        }

        public static (string Result, string Error) EvaluateScriptReturnString(string script)
        {
            var result = _BridgeJsCoreStatic_EvaluateScriptReturnString(script, out var error);
            return (result, error);
        }

        public static string EvaluateScriptWithoutReturnValue(string script)
        {
            _BridgeJsCoreStatic_EvaluateScriptWithoutReturnValue(script, out var error);
            return error;
        }

        public static void EvaluateScriptWithoutReturnValueNoError(string script)
        {
            _BridgeJsCoreStatic_EvaluateScriptWithoutReturnValueNoError(script);
        }
    }
}
