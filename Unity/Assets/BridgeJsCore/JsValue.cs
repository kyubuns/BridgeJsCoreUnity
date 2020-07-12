using System;
using System.Runtime.InteropServices;

namespace BridgeJsCore
{
    // https://developer.apple.com/documentation/javascriptcore/jsvalue
    public class JsValue : IDisposable
    {
        [DllImport("__Internal")]
        private static extern void _BridgeJsCore_JsValue_Dispose(IntPtr value);

        [DllImport("__Internal")]
        [return: MarshalAs(UnmanagedType.U1)]
        private static extern bool _BridgeJsCore_JsValue_IsUndefined(IntPtr value);

        [DllImport("__Internal")]
        [return: MarshalAs(UnmanagedType.U1)]
        private static extern bool _BridgeJsCore_JsValue_IsNull(IntPtr value);

        [DllImport("__Internal")]
        [return: MarshalAs(UnmanagedType.U1)]
        private static extern bool _BridgeJsCore_JsValue_IsBoolean(IntPtr value);

        [DllImport("__Internal")]
        [return: MarshalAs(UnmanagedType.U1)]
        private static extern bool _BridgeJsCore_JsValue_IsNumber(IntPtr value);

        [DllImport("__Internal")]
        [return: MarshalAs(UnmanagedType.U1)]
        private static extern bool _BridgeJsCore_JsValue_IsString(IntPtr value);

        [DllImport("__Internal")]
        [return: MarshalAs(UnmanagedType.U1)]
        private static extern bool _BridgeJsCore_JsValue_IsObject(IntPtr value);

        [DllImport("__Internal")]
        [return: MarshalAs(UnmanagedType.U1)]
        private static extern bool _BridgeJsCore_JsValue_IsArray(IntPtr value);

        [DllImport("__Internal")]
        [return: MarshalAs(UnmanagedType.U1)]
        private static extern bool _BridgeJsCore_JsValue_ToBool(IntPtr value);

        [DllImport("__Internal")]
        private static extern double _BridgeJsCore_JsValue_ToDouble(IntPtr value);

        [DllImport("__Internal")]
        private static extern Int32 _BridgeJsCore_JsValue_ToInt32(IntPtr value);

        [DllImport("__Internal")]
        private static extern UInt32 _BridgeJsCore_JsValue_ToUInt32(IntPtr value);

        [DllImport("__Internal")]
        private static extern string _BridgeJsCore_JsValue_ToString(IntPtr value);

        [DllImport("__Internal")]
        [return: MarshalAs(UnmanagedType.U1)]
        private static extern bool _BridgeJsCore_JsValue_HasProperty(IntPtr value, string property);

        [DllImport("__Internal")]
        private static extern IntPtr _BridgeJsCore_JsValue_AtIndex(IntPtr value, int index);

        [DllImport("__Internal")]
        private static extern IntPtr _BridgeJsCore_JsValue_ForProperty(IntPtr value, string property);

        private readonly IntPtr value;
        private bool disposed;

        public JsValue(IntPtr value)
        {
            this.value = value;
        }

        public void Dispose()
        {
            if (disposed) return;
            disposed = true;
            _BridgeJsCore_JsValue_Dispose(value);
        }

        public bool IsUndefined() => _BridgeJsCore_JsValue_IsUndefined(value);
        public bool IsNull() => _BridgeJsCore_JsValue_IsNull(value);
        public bool IsBoolean() => _BridgeJsCore_JsValue_IsBoolean(value);
        public bool IsNumber() => _BridgeJsCore_JsValue_IsNumber(value);
        public bool IsString() => _BridgeJsCore_JsValue_IsString(value);
        public bool IsObject() => _BridgeJsCore_JsValue_IsObject(value);
        public bool IsArray() => _BridgeJsCore_JsValue_IsArray(value);

        public bool ToBool() => _BridgeJsCore_JsValue_ToBool(value);
        public double ToDouble() => _BridgeJsCore_JsValue_ToDouble(value);
        public float ToFloat() => (float) ToDouble();
        public Int32 ToInt32() => _BridgeJsCore_JsValue_ToInt32(value);
        public UInt32 ToUInt32() => _BridgeJsCore_JsValue_ToUInt32(value);
        public override string ToString() => _BridgeJsCore_JsValue_ToString(value);

        public bool HasProperty(string property) => _BridgeJsCore_JsValue_HasProperty(value, property);

        public JsValue AtIndex(int index)
        {
            var rawJsValuePtr = _BridgeJsCore_JsValue_AtIndex(value, index);
            return new JsValue(rawJsValuePtr);
        }

        public JsValue ForProperty(string property)
        {
            var rawJsValuePtr = _BridgeJsCore_JsValue_ForProperty(value, property);
            return new JsValue(rawJsValuePtr);
        }
    }
}
