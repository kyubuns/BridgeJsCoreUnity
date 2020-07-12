#include <JavaScriptCore/JavaScriptCore.h>

NSString* BridgeJsCore_CreateNSString(const char* string)
{
    return [NSString stringWithUTF8String: string ? string : ""];
}

char* BridgeJsCore_MakeStringCopy(const char* string)
{
    if (string == NULL) return NULL;
    char* res = (char*) malloc(strlen(string) + 1);
    strcpy(res, string);
    return res;
}

extern "C"
{
    JSValue *_BridgeJsCore_EvaluateScript(JSContext *context, const char *text, char *&error)
    {
        __block NSString *exceptionString = @"";
        context.exceptionHandler = ^(JSContext *context, JSValue *exception) {
            exceptionString = [exception toString];
        };

        JSValue *original = [context evaluateScript: BridgeJsCore_CreateNSString(text)];
        error = BridgeJsCore_MakeStringCopy([exceptionString UTF8String]);
        
        return original;
    }

    void _BridgeJsCore_JsValue_Dispose(JSValue *jsValue)
    {
    }

    bool _BridgeJsCore_JsValue_IsUndefined(JSValue *jsValue)
    {
        return jsValue.isUndefined;
    }

    bool _BridgeJsCore_JsValue_IsNull(JSValue *jsValue)
    {
        return jsValue.isNull;
    }

    bool _BridgeJsCore_JsValue_IsBoolean(JSValue *jsValue)
    {
        return jsValue.isBoolean;
    }

    bool _BridgeJsCore_JsValue_IsNumber(JSValue *jsValue)
    {
        return jsValue.isNumber;
    }

    bool _BridgeJsCore_JsValue_IsString(JSValue *jsValue)
    {
        return jsValue.isString;
    }

    bool _BridgeJsCore_JsValue_IsObject(JSValue *jsValue)
    {
        return jsValue.isObject;
    }

    bool _BridgeJsCore_JsValue_IsArray(JSValue *jsValue)
    {
        return jsValue.isArray;
    }

    bool _BridgeJsCore_JsValue_ToBool(JSValue *jsValue)
    {
        return jsValue.toBool;
    }

    double _BridgeJsCore_JsValue_ToDouble(JSValue *jsValue)
    {
        return jsValue.toDouble;
    }

    int32_t _BridgeJsCore_JsValue_ToInt32(JSValue *jsValue)
    {
        return jsValue.toInt32;
    }

    uint32_t _BridgeJsCore_JsValue_ToUInt32(JSValue *jsValue)
    {
        return jsValue.toUInt32;
    }

    char *_BridgeJsCore_JsValue_ToString(JSValue *jsValue)
    {
        return BridgeJsCore_MakeStringCopy([jsValue.toString UTF8String]);
    }

    bool _BridgeJsCore_JsValue_HasProperty(JSValue *jsValue, const char *property)
    {
        return [jsValue hasProperty: BridgeJsCore_CreateNSString(property)];
    }

    JSValue *_BridgeJsCore_JsValue_AtIndex(JSValue *jsValue, int index)
    {
        return [jsValue valueAtIndex: index];
    }

    JSValue *_BridgeJsCore_JsValue_ForProperty(JSValue *jsValue, const char *property)
    {
        return [jsValue valueForProperty: BridgeJsCore_CreateNSString(property)];
    }
}
