using System;
using System.Runtime.InteropServices;

namespace BridgeJsCore
{
    public static class FastEngine
    {
        [DllImport("__Internal")]
        private static extern IntPtr _BridgeJsCoreStatic_EvaluateScript(string script, out string error);

        [DllImport("__Internal")]
        private static extern void _BridgeJsCoreStatic_EvaluateScriptWithoutReturnValue(string script, out string error);

        [DllImport("__Internal")]
        private static extern void _BridgeJsCoreStatic_EvaluateScriptWithoutReturnValueNoError(string script);

        public static (IJsValue, string Error) EvaluateScript(string script)
        {
            var rawJsValuePtr = _BridgeJsCoreStatic_EvaluateScript(script, out var error);
            var jsValue = Engine.ToJsValue(rawJsValuePtr);
            return (jsValue, error);
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
