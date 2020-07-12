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
        private static extern string _BridgeJsCore_EvaluateScript(IntPtr context, string script);

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

        public string EvaluateScript(string script)
        {
            return _BridgeJsCore_EvaluateScript(context, script);
        }
    }
}
