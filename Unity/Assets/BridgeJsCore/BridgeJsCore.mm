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
    struct SerializedJsValue
    {
        char *Value;
        bool IsUndefined;
        bool IsNull;
        bool IsBoolean;
        bool IsNumber;
        bool IsString;
        bool IsObject;
        bool IsArray;
    };

    JSContext *_BridgeJsCore_New()
    {
        JSContext *context = [[JSContext alloc] init];
        return context;
    }

    void _BridgeJsCore_Dispose(JSContext *context)
    {
        [context release];
    }

    SerializedJsValue *_BridgeJsCore_EvaluateScript(JSContext *context, const char *text, char *&error)
    {
        __block NSString *exceptionString = @"";
        context.exceptionHandler = ^(JSContext *context, JSValue *exception) {
            exceptionString = [exception toString];
        };

        JSValue *original = [context evaluateScript: BridgeJsCore_CreateNSString(text)];
        error = BridgeJsCore_MakeStringCopy([exceptionString UTF8String]);
        
        SerializedJsValue *serialized = new SerializedJsValue();
        serialized->Value = BridgeJsCore_MakeStringCopy([original.toString UTF8String]);
        serialized->IsUndefined = original.isUndefined;
        serialized->IsNull = original.isNull;
        serialized->IsBoolean = original.isBoolean;
        serialized->IsNumber = original.isNumber;
        serialized->IsString = original.isString;
        serialized->IsObject = original.isObject;
        serialized->IsArray = original.isArray;
        return serialized;
    }

    void _BridgeJsCore_FreeJsValue(SerializedJsValue *serialized)
    {
        delete serialized;
    }
}
