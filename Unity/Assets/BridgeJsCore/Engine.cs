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

        public (JsValue, string Error) EvaluateScript(string script)
        {
            if (disposed) throw new InvalidOperationException("engine is disposed");
            var rawJsValuePtr = _BridgeJsCore_EvaluateScript(context, script, out var error);
            return (new JsValue(rawJsValuePtr), error);
        }
    }
}
