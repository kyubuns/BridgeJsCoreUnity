#include <JavaScriptCore/JavaScriptCore.h>

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
}
