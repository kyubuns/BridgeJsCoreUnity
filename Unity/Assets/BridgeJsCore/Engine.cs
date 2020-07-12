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
        private static extern void _BridgeJsCore_FreeJsValue(IntPtr value);

        [StructLayout(LayoutKind.Sequential)]
        private class RawJsValue
        {
            public string Value;
            [MarshalAs(UnmanagedType.U1)] public bool IsUndefined;
            [MarshalAs(UnmanagedType.U1)] public bool IsNull;
            [MarshalAs(UnmanagedType.U1)] public bool IsBoolean;
            [MarshalAs(UnmanagedType.U1)] public bool IsNumber;
            [MarshalAs(UnmanagedType.U1)] public bool IsString;
            [MarshalAs(UnmanagedType.U1)] public bool IsObject;
            [MarshalAs(UnmanagedType.U1)] public bool IsArray;
        }

        private bool disposed;
        private readonly IntPtr context;

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

        public (JsValue, string Error) EvaluateScript(string script)
        {
            if (disposed) throw new InvalidOperationException("engine is disposed");
            var rawJsValuePtr = _BridgeJsCore_EvaluateScript(context, script, out var error);
            var rawJsValue = (RawJsValue) Marshal.PtrToStructure(rawJsValuePtr, typeof(RawJsValue));
            var jsValue = new JsValue(rawJsValue.Value, rawJsValue.IsUndefined, rawJsValue.IsNull, rawJsValue.IsBoolean, rawJsValue.IsNumber, rawJsValue.IsString, rawJsValue.IsObject, rawJsValue.IsArray);
            _BridgeJsCore_FreeJsValue(rawJsValuePtr);
            return (jsValue, error);
        }
    }
}
