namespace VisioForge.DirectShowAPI
{
    using System.Runtime.InteropServices;

    /// <summary>
    /// LAME Encoder interface.
    /// </summary>
    /// <remarks>
    /// Configuring MPEG audio encoder parameters with unspecified
    /// input stream type may lead to misbehavior and confusing
    /// results. In most cases the specified parameters will be
    /// overridden by defaults for the input media type.
    /// To archive proper results use this interface on the
    /// audio encoder filter with input pin connected to the valid
    /// source. 
    /// </remarks>
    [ComImport]
    [System.Security.SuppressUnmanagedCodeSecurity]
    [Guid("595EB9D1-F454-41AD-A1FA-EC232AD9DA52")]
    [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    public interface IAudioEncoderProperties
    {
        // Is PES output enabled? Return TRUE or FALSE
        [PreserveSig]
        int get_PESOutputEnabled(out int dwEnabled);

        // Enable/disable PES output
        [PreserveSig]
        int set_PESOutputEnabled([In] int dwEnabled);

        // Get target compression bitrate in Kbits/s
        [PreserveSig]
        int get_Bitrate(out int dwBitrate);

        // Set target compression bitrate in Kbits/s
        // Not all numbers available! See spec for details!
        [PreserveSig]
        int set_Bitrate([In] int dwBitrate);

        // Get variable bitrate flag
        [PreserveSig]
        int get_Variable(out int dwVariable);

        // Set variable bitrate flag
        [PreserveSig]
        int set_Variable([In] int dwVariable);

        // Get variable bitrate in Kbits/s
        [PreserveSig]
        int get_VariableMin(out int dwmin);

        // Set variable bitrate in Kbits/s
        // Not all numbers available! See spec for details!
        [PreserveSig]
        int set_VariableMin([In] int dwmin);

        // Get variable bitrate in Kbits/s
        [PreserveSig]
        int get_VariableMax(out int dwmax);

        // Set variable bitrate in Kbits/s
        // Not all numbers available! See spec for details!
        [PreserveSig]
        int set_VariableMax([In] int dwmax);

        // Get compression quality
        [PreserveSig]
        int get_Quality(out int dwQuality);

        // Set compression quality
        // Not all numbers available! See spec for details!
        [PreserveSig]
        int set_Quality([In] int dwQuality);

        // Get VBR quality
        [PreserveSig]
        int get_VariableQ(out int dwVBRq);

        // Set VBR quality
        // Not all numbers available! See spec for details!
        [PreserveSig]
        int set_VariableQ([In] int dwVBRq);

        // Get source sample rate. Return E_FAIL if input pin
        // in not connected.
        [PreserveSig]
        int get_SourceSampleRate(out int dwSampleRate);

        // Get source number of channels. Return E_FAIL if
        // input pin is not connected.
        [PreserveSig]
        int get_SourceChannels(out int dwChannels);

        // Get sample rate for compressed audio bitstream
        [PreserveSig]
        int get_SampleRate(out int dwSampleRate);

        // Set sample rate. See genaudio spec for details
        [PreserveSig]
        int set_SampleRate([In] int dwSampleRate);

        // Get channel mode. See genaudio.h for details
        [PreserveSig]
        int get_ChannelMode(out int dwChannelMode);

        // Set channel mode
        [PreserveSig]
        int set_ChannelMode([In] int dwChannelMode);

        // Is CRC enabled?
        [PreserveSig]
        int get_CRCFlag(out int dwFlag);

        // Enable/disable CRC
        [PreserveSig]
        int set_CRCFlag([In] int dwFlag);

        // Control 'original' flag
        [PreserveSig]
        int get_OriginalFlag(out int dwFlag);

        [PreserveSig]
        int set_OriginalFlag([In] int dwFlag);

        // Control 'copyright' flag
        [PreserveSig]
        int get_CopyrightFlag(out int dwFlag);

        [PreserveSig]
        int set_CopyrightFlag([In] int dwFlag);

        // Control 'Enforce VBR Minimum bitrate' flag
        [PreserveSig]
        int get_EnforceVBRmin(out int dwFlag);

        [PreserveSig]
        int set_EnforceVBRmin([In] int dwFlag);

        // Control 'Voice' flag
        [PreserveSig]
        int get_VoiceMode(out int dwFlag);

        [PreserveSig]
        int set_VoiceMode([In] int dwFlag);

        // Control 'Keep All Frequencies' flag
        [PreserveSig]
        int get_KeepAllFreq(out int dwFlag);

        [PreserveSig]
        int set_KeepAllFreq([In] int dwFlag);

        // Control 'Strict ISO compliance' flag
        [PreserveSig]
        int get_StrictISO(out int dwFlag);

        [PreserveSig]
        int set_StrictISO([In] int dwFlag);

        // Control 'Disable short block' flag
        [PreserveSig]
        int get_NoShortBlock(out int dwDisable);

        [PreserveSig]
        int set_NoShortBlock([In] int dwDisable);

        // Control 'Xing VBR Tag' flag
        [PreserveSig]
        int get_XingTag(out int dwXingTag);

        [PreserveSig]
        int set_XingTag([In] int dwXingTag);

        // Control 'Forced mid/ side stereo' flag
        [PreserveSig]
        int get_ForceMS(out int dwFlag);

        [PreserveSig]
        int set_ForceMS([In] int dwFlag);

        // Control 'ModeFixed' flag
        [PreserveSig]
        int get_ModeFixed(out int dwFlag);

        [PreserveSig]
        int set_ModeFixed([In] int dwFlag);

        // Receive the block of encoder
        // configuration parameters
        [PreserveSig]
        int get_ParameterBlockSize(out byte pcBlock, out int pdwSize);

        // Set encoder configuration parameters
        [PreserveSig]
        int set_ParameterBlockSize([In] byte pcBlock, [In] int dwSize);

        // Set default audio encoder parameters depending
        // on current input stream type
        [PreserveSig]
        int DefaultAudioEncoderProperties();

        // By default the modified properties are not saved to
        // registry immediately, so the filter needs to be
        // forced to do  Omitting this steps may lead to
        // misbehavior and confusing results.
        [PreserveSig]
        int LoadAudioEncoderPropertiesFromRegistry();

        [PreserveSig]
        int SaveAudioEncoderPropertiesToRegistry();

        // Determine, whether the filter can be configured. If this
        // functions returns E_FAIL, input format hasn't been
        // specified and filter behavior unpredicated. If S_OK,
        // the filter could be configured with correct values.
        [PreserveSig]
        int InputTypeDefined();
    }
}
