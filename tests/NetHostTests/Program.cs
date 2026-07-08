using System.Runtime.InteropServices;
using Universe.NetHost;

var result = NetHost.GetHostFxrPath();

if (result is { IsSuccessful: true, Value: string path })
{
    Console.WriteLine(path);

    var handle = NativeLibrary.Load(path);
    NativeLibrary.Free(handle);
}
else
{
    Console.WriteLine($"Failed with code: {result.Error}");
}
