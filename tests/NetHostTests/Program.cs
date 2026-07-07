using System.Runtime.InteropServices;
using Universe.NetHost;

if (NetHost.GetHostFxrPath() is string path)
{
    Console.WriteLine(path);

    var handle = NativeLibrary.Load(path);
    NativeLibrary.Free(handle);
}
else
{
    Console.WriteLine("Failed");
}
