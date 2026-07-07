using System.Runtime.InteropServices;

namespace Universe.NetHost;

public readonly struct GetHostfxrParameters
{
    public string? AssemblyPath { get; init; }
    public string? DotNetRoot { get; init; }

    public GetHostfxrParameters() { }
}

public static partial class NetHost
{
    private const string LibraryName = "nethost";

    public static string? GetHostFxrPath()
    {
        unsafe
        {
            return Unsafe_GetHostFxrPath(null);
        }
    }

    public static string? GetHostFxrPath(in GetHostfxrParameters parameters)
    {
        unsafe
        {
            fixed (
                void* assemblyPathPtr =
                    &System.Runtime.InteropServices.Marshalling.Utf16StringMarshaller.GetPinnableReference(
                        parameters.AssemblyPath
                    )
            )
            fixed (
                void* dotnetRoot =
                    &System.Runtime.InteropServices.Marshalling.Utf16StringMarshaller.GetPinnableReference(
                        parameters.DotNetRoot
                    )
            )
            {
                get_hostfxr_parameters hostfxrParameters = new()
                {
                    size = (nuint)sizeof(get_hostfxr_parameters),
                    assembly_path = (ushort*)assemblyPathPtr,
                    dotnet_root = (ushort*)dotnetRoot,
                };

                return Unsafe_GetHostFxrPath(&hostfxrParameters);
            }
        }
    }

    private static unsafe string? Unsafe_GetHostFxrPath(get_hostfxr_parameters* parameters)
    {
        const int maxPath = 260;
        char* buffer = stackalloc char[maxPath];
        nuint bufferSize = maxPath;
        var rc = Native_GetHostFxrPath(buffer, &bufferSize, parameters);

        return rc != 0 ? null : new(buffer, 0, (int)(bufferSize - 1));
    }

    [LibraryImport(LibraryName, EntryPoint = "get_hostfxr_path")]
    private static unsafe partial int Native_GetHostFxrPath(
        char* buffer,
        nuint* bufferSize,
        get_hostfxr_parameters* parameters
    );

#pragma warning disable IDE1006 // Naming Styles
    private unsafe partial struct get_hostfxr_parameters
#pragma warning restore IDE1006 // Naming Styles
    {
#pragma warning disable IDE1006 // Naming Styles
        public nuint size;
#pragma warning restore IDE1006 // Naming Styles

#pragma warning disable IDE1006 // Naming Styles
        public ushort* assembly_path;
#pragma warning restore IDE1006 // Naming Styles

#pragma warning disable IDE1006 // Naming Styles
        public ushort* dotnet_root;
#pragma warning restore IDE1006 // Naming Styles
    }
}
