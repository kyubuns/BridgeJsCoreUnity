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

struct SerializedJsValue
{
    char *Value;
    bool Bool;
    JSValue *Ptr;
    bool IsUndefined;
    bool IsNull;
    bool IsBoolean;
    bool IsNumber;
    bool IsString;
    bool IsObject;
    bool IsArray;
};

SerializedJsValue *BridgeJsCore_MakeSerializedJsValue(JSValue *original)
{
    SerializedJsValue *serialized = new SerializedJsValue();
    serialized->Value = BridgeJsCore_MakeStringCopy([original.toString UTF8String]);
    serialized->Bool = original.toBool;
    serialized->Ptr = original;
    serialized->IsUndefined = original.isUndefined;
    serialized->IsNull = original.isNull;
    serialized->IsBoolean = original.isBoolean;
    serialized->IsNumber = original.isNumber;
    serialized->IsString = original.isString;
    serialized->IsObject = original.isObject;
    serialized->IsArray = original.isArray;
    return serialized;
}

extern "C"
{
    SerializedJsValue *_BridgeJsCore_EvaluateScript(JSContext *context, const char *text, char *&error)
    {
        __block NSString *exceptionString = @"";
        context.exceptionHandler = ^(JSContext *context, JSValue *exception) {
            exceptionString = [exception toString];
        };

        JSValue *original = [context evaluateScript: BridgeJsCore_CreateNSString(text)];
        error = BridgeJsCore_MakeStringCopy([exceptionString UTF8String]);
        return BridgeJsCore_MakeSerializedJsValue(original);
    }

    void _BridgeJsCore_EvaluateScriptWithoutReturnValue(JSContext *context, const char *text, char *&error)
    {
        __block NSString *exceptionString = @"";
        context.exceptionHandler = ^(JSContext *context, JSValue *exception) {
            exceptionString = [exception toString];
        };

        [context evaluateScript: BridgeJsCore_CreateNSString(text)];
    }

    void _BridgeJsCore_EvaluateScriptWithoutReturnValueNoError(JSContext *context, const char *text)
    {
        [context evaluateScript: BridgeJsCore_CreateNSString(text)];
    }

    void _BridgeJsCore_FreeJsValue(SerializedJsValue *serialized)
    {
        delete serialized;
    }

    bool _BridgeJsCore_JsValue_HasProperty(JSValue *jsValue, const char *property)
    {
        return [jsValue hasProperty: BridgeJsCore_CreateNSString(property)];
    }

    SerializedJsValue *_BridgeJsCore_JsValue_AtIndex(JSValue *jsValue, int index)
    {
        JSValue *original = [jsValue valueAtIndex: index];
        return BridgeJsCore_MakeSerializedJsValue(original);
    }

    SerializedJsValue *_BridgeJsCore_JsValue_ForProperty(JSValue *jsValue, const char *property)
    {
        JSValue *original = [jsValue valueForProperty: BridgeJsCore_CreateNSString(property)];
        return BridgeJsCore_MakeSerializedJsValue(original);
    }

    JSContext *staticContext;

    SerializedJsValue *_BridgeJsCoreStatic_EvaluateScript(const char *text, char *&error)
    {
        if (staticContext == NULL) staticContext = [[JSContext alloc] init];
        return _BridgeJsCore_EvaluateScript(staticContext, text, error);
    }

    void _BridgeJsCoreStatic_EvaluateScriptWithoutReturnValue(const char *text, char *&error)
    {
        if (staticContext == NULL) staticContext = [[JSContext alloc] init];
        _BridgeJsCore_EvaluateScriptWithoutReturnValue(staticContext, text, error);
    }

    void _BridgeJsCoreStatic_EvaluateScriptWithoutReturnValueNoError(const char *text)
    {
        if (staticContext == NULL) staticContext = [[JSContext alloc] init];
        _BridgeJsCore_EvaluateScriptWithoutReturnValueNoError(staticContext, text);
    }
}
