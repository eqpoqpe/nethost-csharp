using System.Runtime.InteropServices;
using DotNext;
using Universe.NetHost.Errors;

namespace Universe.NetHost;

public readonly struct GetHostFxrParameters
{
    public string? AssemblyPath { get; init; }
    public string? DotNetRoot { get; init; }

    public GetHostFxrParameters() { }
}

public static partial class NetHost
{
    public static Result<string, NetHostError> GetHostFxrPath()
    {
        unsafe
        {
            var path = GetHostFxrPath(null, out int resultCode);

            return path is not null ? new(path) : new(MapErrorCode(resultCode));
        }
    }

    public static Result<string, NetHostError> GetHostFxrPath(in GetHostFxrParameters parameters)
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
                NetHostNative.get_hostfxr_parameters hostfxrParameters = new()
                {
                    size = (nuint)sizeof(NetHostNative.get_hostfxr_parameters),
                    assembly_path = (ushort*)assemblyPathPtr,
                    dotnet_root = (ushort*)dotnetRoot,
                };

                var path = GetHostFxrPath(null, out int resultCode);

                return path is not null ? new(path) : new(MapErrorCode(resultCode));
            }
        }
    }

    private static NetHostError MapErrorCode(int code)
    {
        return code switch
        {
            -2147450728 => NetHostError.HostApiBufferTooSmall,
            -2147450734 => NetHostError.LibHostInvalidArgs,
            -2147450748 => NetHostError.CoreHostLibMissingFailure,
            -2147450749 => NetHostError.CoreHostLibLoadFailure,
            -2147450751 => NetHostError.CoreHostResolveFailure,
            -2147450718 => NetHostError.HostApiUnsupportedVersion,
            _ => NetHostError.Unrecoverable,
        };
    }

    private static unsafe string? GetHostFxrPath(
        NetHostNative.get_hostfxr_parameters* parameters,
        out int resultCode
    )
    {
        const int initialSize = 512;

        char* buffer = stackalloc char[initialSize];
        nuint bufferSize = initialSize;

        resultCode = NetHostNative.GetHostFxrPath(buffer, &bufferSize, parameters);

        return resultCode != 0 ? null : new string(buffer, 0, (int)bufferSize - 1);
    }
}

static partial class NetHostNative
{
    private const string LibraryName = "nethost";

    [LibraryImport(LibraryName, EntryPoint = "get_hostfxr_path")]
    public static unsafe partial int GetHostFxrPath(
        char* buffer,
        nuint* bufferSize,
        get_hostfxr_parameters* parameters
    );

#pragma warning disable IDE1006 // Naming Styles
    public unsafe partial struct get_hostfxr_parameters
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
