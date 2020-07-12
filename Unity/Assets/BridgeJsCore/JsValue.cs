using System;

namespace BridgeJsCore
{
    // https://developer.apple.com/documentation/javascriptcore/jsvalue
    public class JsValue
    {
        public string Value { get; }
        public bool IsUndefined { get; }
        public bool IsNull { get; }
        public bool IsBoolean { get; }
        public bool IsNumber { get; }
        public bool IsString { get; }
        public bool IsObject { get; }
        public bool IsArray { get; }

        public JsValue(string value, bool isUndefined, bool isNull, bool isBoolean, bool isNumber, bool isString, bool isObject, bool isArray)
        {
            Value = value;
            IsUndefined = isUndefined;
            IsNull = isNull;
            IsBoolean = isBoolean;
            IsNumber = isNumber;
            IsString = isString;
            IsObject = isObject;
            IsArray = isArray;
        }

        public bool ToBool()
        {
            if (!IsBoolean) throw new InvalidCastException();
            return string.Equals(Value, "true", StringComparison.Ordinal);
        }

        public double ToDouble()
        {
            if (!IsNumber) throw new InvalidCastException();
            return double.Parse(Value);
        }

        public float ToFloat()
        {
            if (!IsNumber) throw new InvalidCastException();
            return float.Parse(Value);
        }

        public Int32 ToInt32()
        {
            if (!IsNumber) throw new InvalidCastException();
            return Int32.Parse(Value);
        }

        public UInt32 ToUInt32()
        {
            if (!IsNumber) throw new InvalidCastException();
            return UInt32.Parse(Value);
        }

        public override string ToString()
        {
            return Value;
        }
    }
}
