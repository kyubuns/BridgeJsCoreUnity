using System;
using System.Runtime.InteropServices;

namespace BridgeJsCore
{
    public interface IJsValue
    {
    }

    public class JsUndefined : IJsValue
    {
        public override string ToString() => "undefined";
    }

    public class JsNull : IJsValue
    {
        public override string ToString() => "null";
    }

    public class JsBoolean : IJsValue
    {
        public JsBoolean(bool value)
        {
            Value = value;
        }

        public bool Value { get; }
        public override string ToString() => Value.ToString();
    }

    public class JsNumber : IJsValue
    {
        public JsNumber(string rawValue)
        {
            this.rawValue = rawValue;
        }

        private string rawValue;
        public Int32 ToInt32() => Int32.Parse(rawValue);
        public UInt32 ToUInt32() => UInt32.Parse(rawValue);
        public double ToDouble() => double.Parse(rawValue);
        public float ToFloat() => (float) ToDouble();
        public override string ToString() => rawValue;
    }

    public class JsString : IJsValue
    {
        public JsString(string value)
        {
            Value = value;
        }

        public string Value { get; }
        public override string ToString() => Value;
    }

    public class JsObject : IJsValue
    {
        [DllImport("__Internal")]
        [return: MarshalAs(UnmanagedType.U1)]
        private static extern bool _BridgeJsCore_JsValue_HasProperty(IntPtr value, string property);

        [DllImport("__Internal")]
        private static extern IntPtr _BridgeJsCore_JsValue_ForProperty(IntPtr value, string property);

        [DllImport("__Internal")]
        private static extern IntPtr _BridgeJsCore_JsValue_AtIndex(IntPtr value, int index);

        public JsObject(IntPtr ptr, string rawValue)
        {
            this.ptr = ptr;
            this.rawValue = rawValue;
        }

        private readonly IntPtr ptr;
        private readonly string rawValue;
        public override string ToString() => rawValue;

        public bool HasProperty(string property) => _BridgeJsCore_JsValue_HasProperty(ptr, property);

        public IJsValue ForProperty(string property)
        {
            var rawJsValuePtr = _BridgeJsCore_JsValue_ForProperty(ptr, property);
            var jsValue = Engine.ToJsValue(rawJsValuePtr);
            return jsValue;
        }

        public IJsValue AtIndex(int index)
        {
            var rawJsValuePtr = _BridgeJsCore_JsValue_AtIndex(ptr, index);
            var jsValue = Engine.ToJsValue(rawJsValuePtr);
            return jsValue;
        }
    }

    public class JsArray : JsObject
    {
        public JsArray(IntPtr ptr, string rawValue) : base(ptr, rawValue)
        {
        }

        public int Length => ((JsNumber) ForProperty("length")).ToInt32();
    }

    public class JsUnknown : IJsValue
    {
        public JsUnknown(string value)
        {
            Value = value;
        }

        public string Value { get; }
        public override string ToString() => Value;
    }
}
