namespace Universe.NetHost.Errors;

public enum NetHostError
{
    None,
    Unrecoverable,

    /// <summary>
    /// -2147450728
    /// </summary>
    HostApiBufferTooSmall,

    /// <summary>
    /// -2147450734
    /// </summary>
    LibHostInvalidArgs,

    /// <summary>
    /// -2147450748
    /// </summary>
    CoreHostLibMissingFailure,

    /// <summary>
    /// -2147450749
    /// </summary>
    CoreHostLibLoadFailure,

    /// <summary>
    /// -2147450751
    /// </summary>
    CoreHostResolveFailure,

    /// <summary>
    /// -2147450718
    /// </summary>
    HostApiUnsupportedVersion,
}
