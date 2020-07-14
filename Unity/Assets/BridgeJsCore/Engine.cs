using System;
using System.Runtime.InteropServices;

namespace BridgeJsCore
{
    public class Engine : IDisposable
    {
        [DllImport("__Internal")]
        private static extern IntPtr _BridgeJsCore_New();

        [DllImport("__Internal")]
        private static extern void _BridgeJsCore_Dispose(IntPtr context);

        [DllImport("__Internal")]
        private static extern IntPtr _BridgeJsCore_EvaluateScript(IntPtr context, string script, out string error);

        [DllImport("__Internal")]
        private static extern string _BridgeJsCore_EvaluateScriptReturnString(IntPtr context, string script, out string error);

        [DllImport("__Internal")]
        private static extern void _BridgeJsCore_EvaluateScriptWithoutReturnValue(IntPtr context, string script, out string error);

        [DllImport("__Internal")]
        private static extern void _BridgeJsCore_EvaluateScriptWithoutReturnValueNoError(IntPtr context, string script);

        [DllImport("__Internal")]
        private static extern void _BridgeJsCore_FreeJsValue(IntPtr value);

        [StructLayout(LayoutKind.Sequential)]
        private class RawJsValue
        {
            public string Value;
            [MarshalAs(UnmanagedType.U1)] public bool Bool;
            public IntPtr Ptr;
            [MarshalAs(UnmanagedType.U1)] public bool IsUndefined;
            [MarshalAs(UnmanagedType.U1)] public bool IsNull;
            [MarshalAs(UnmanagedType.U1)] public bool IsBoolean;
            [MarshalAs(UnmanagedType.U1)] public bool IsNumber;
            [MarshalAs(UnmanagedType.U1)] public bool IsString;
            [MarshalAs(UnmanagedType.U1)] public bool IsObject;
            [MarshalAs(UnmanagedType.U1)] public bool IsArray;
        }

        private readonly IntPtr context;
        private bool disposed;

        public Engine()
        {
            context = _BridgeJsCore_New();
        }

        public void Dispose()
        {
            if (disposed) return;
            disposed = true;
            _BridgeJsCore_Dispose(context);
        }

        public (IJsValue Result, string Error) EvaluateScript(string script)
        {
            if (disposed) throw new InvalidOperationException("engine is disposed");
            var rawJsValuePtr = _BridgeJsCore_EvaluateScript(context, script, out var error);
            var jsValue = ToJsValue(rawJsValuePtr);
            return (jsValue, error);
        }

        public (string Result, string Error) EvaluateScriptReturnString(string script)
        {
            if (disposed) throw new InvalidOperationException("engine is disposed");
            var result = _BridgeJsCore_EvaluateScriptReturnString(context, script, out var error);
            return (result, error);
        }

        public string EvaluateScriptWithoutReturnValue(string script)
        {
            if (disposed) throw new InvalidOperationException("engine is disposed");
            _BridgeJsCore_EvaluateScriptWithoutReturnValue(context, script, out var error);
            return error;
        }

        public void EvaluateScriptWithoutReturnValueNoError(string script)
        {
            if (disposed) throw new InvalidOperationException("engine is disposed");
            _BridgeJsCore_EvaluateScriptWithoutReturnValueNoError(context, script);
        }

        public static IJsValue ToJsValue(IntPtr rawJsValuePtr)
        {
            var rawJsValue = (RawJsValue) Marshal.PtrToStructure(rawJsValuePtr, typeof(RawJsValue));

            IJsValue jsValue;
            if (rawJsValue.IsUndefined) jsValue = new JsUndefined();
            else if (rawJsValue.IsNull) jsValue = new JsNull();
            else if (rawJsValue.IsBoolean) jsValue = new JsBoolean(rawJsValue.Bool);
            else if (rawJsValue.IsNumber) jsValue = new JsNumber(rawJsValue.Value);
            else if (rawJsValue.IsString) jsValue = new JsString(rawJsValue.Value);
            else if (rawJsValue.IsArray) jsValue = new JsArray(rawJsValue.Ptr, rawJsValue.Value);
            else if (rawJsValue.IsObject) jsValue = new JsObject(rawJsValue.Ptr, rawJsValue.Value);
            else jsValue = new JsUnknown(rawJsValue.Value);

            _BridgeJsCore_FreeJsValue(rawJsValuePtr);

            return jsValue;
        }
    }
}
