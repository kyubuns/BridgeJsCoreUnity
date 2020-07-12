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
    JSContext *_BridgeJsCore_New()
    {
        JSContext *context = [[JSContext alloc] init];
        return context;
    }

    void _BridgeJsCore_Dispose(JSContext *context)
    {
        [context release];
    }

    char *_BridgeJsCore_EvaluateScript(JSContext *context, const char* text)
    {
        JSValue *result = [context evaluateScript: BridgeJsCore_CreateNSString(text)];
        NSString *resultText = [result toString];
        return BridgeJsCore_MakeStringCopy([resultText UTF8String]);
    }
}
