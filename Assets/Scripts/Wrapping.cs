using System;
using System.Runtime.InteropServices;
using System.Globalization;

namespace UnitouchEngine_Lite
{
    public class Wrapping
    {
        /// <summary>
        /// Available type of output whose haptic signals are sent to.
        /// </summary>
        public enum OutputDevice
        {
            eAudioDevice,               /**< Send computed signals to an audio stream */
            eSkinetic,                  /**< Send computed signals to Skinetic device */
            eHSDmkI,                    /**< Send computed signals to HSD mk.I device */
            eActronikaDeviceManager,    /**< Send computed signals to the Actronika Device Manager */
        }

        /// <summary>
        /// Error codes.
        /// </summary>
        public enum Error
        {
            eNoError = 0,                 /**< No error */
            eOther = -1,                  /**< Other */
            eInvalidParam = -2,           /**< Invalid parameter */
            eDeviceNotFound = -3,         /**< Device could not be found */
            eErrorPortAudio = -4,         /**< PortAudio raised an error */
            eErrorDeviceManager = -5,     /**< Device Manager not reachable*/
            eOutputNotSupported = -6,     /**< Output is not supported */
            eStillComputing = -7,         /**< Rendering thread is still running */
            eInvalidState = -8,           /**< Object state is invalid */
        };

        /// <summary>
        /// Structure to initialize the output.
        /// </summary>
        [Serializable, StructLayout(LayoutKind.Sequential, Pack = 8)]
        public struct OutputInitOptions
        {
            /* General settings */
            public Int32 n_stream_channel;

            /* Audio Stream settings */
            [MarshalAs(UnmanagedType.LPStr)] public string name_audio_API;
            public Double suggested_latency;
            public Int32 averaging_size_window;

            /* Skinetic settings */
            public Int32 buffer_offset;

            public void SetDefaultParameters()
            {
                this.n_stream_channel = -1;
                this.name_audio_API = "any_api";
                this.suggested_latency = 0;
                this.averaging_size_window = 1;
                this.buffer_offset = 14;
            }
        }

        private const string DLLNAME = "eng_lite";


        /// <summary>
        /// Internal use only.
        /// </summary>
        [global::System.Runtime.InteropServices.DllImport(DLLNAME, EntryPoint = "versionString_capi")]
        private static extern IntPtr versionString();

        /// <summary>
        /// Set a logger that the library will use.
        /// Two loggers are available, the stderr logger and the file logger.
        /// Passing true enable the file logger, otherwise, the stderr logger
        /// is selected by defaul.
        /// </summary>
        /// <param name="use_file_logger">boolean to select logger. </param>
        [global::System.Runtime.InteropServices.DllImport(DLLNAME, EntryPoint = "setLogger_capi")]
        public static extern void setLogger(bool use_file_logger);


        /// <summary>
        /// Initialize context. 
        /// Create internal structures according to the selected output device.
        /// </summary>
        /// <param name="selected_output">targeted output device.</param>
        /// <param name="n_virtual_actuators">number of virtual actuators to render.</param>
        /// <param name="sampling_rate">haptic rendering sample rate. </param>
        /// <param name="buffer_size">virtual actuator buffer size. </param>
        /// <returns>0 if the context has been initialized, an Error otherwise. </returns>
        [global::System.Runtime.InteropServices.DllImport(DLLNAME, EntryPoint = "initContext_capi")]
        public static extern int initContext(OutputDevice selected_output, UInt32 n_virtual_actuators, UInt32 sampling_rate, UInt32 buffer_size);


        /// <summary>
        /// Deinitialize context. 
        /// </summary>
        /// <returns> 0 if successful, an Error otherwise. </returns>
        [global::System.Runtime.InteropServices.DllImport(DLLNAME, EntryPoint = "deinitContext_capi")]
        public static extern int deinitContext();


        /// <summary>
        /// Check if the context is initialized. 
        /// </summary>
        /// <returns>true if context is initialized, false otherwise. </returns>
        [global::System.Runtime.InteropServices.DllImport(DLLNAME, EntryPoint = "isContextInit_capi")]
        public static extern bool isContextInit();


        /// <summary>
        /// Connect to the output device. 
        /// Context must be initialized before this call.Parameters will be
        /// ignored if the context has been initialized to use the 
        /// Actronika Device Manager.
        /// </summary>
        /// <param name="name">name of the output device. </param>
        /// <param name="output_init_options">structure to define device options.</param>
        /// <returns>0 if the device is connected, an Error otherwise. </returns>
        [global::System.Runtime.InteropServices.DllImport(DLLNAME, EntryPoint = "connectDevice_capi")]
        public static extern int connectDevice([MarshalAs(UnmanagedType.LPStr)] string name, ref OutputInitOptions output_init_options);


        /// <summary>
        /// Disconnect the output device. 
        /// </summary>
        /// <returns>0 if successful, an Error otherwise. </returns>
        [global::System.Runtime.InteropServices.DllImport(DLLNAME, EntryPoint = "disconnectDevice_capi")]
        public static extern int disconnectDevice();


        /// <summary>
        /// Check if the output device is connected. 
        /// </summary>
        /// <returns>true if a device is connected, false otherwise. </returns>
        [global::System.Runtime.InteropServices.DllImport(DLLNAME, EntryPoint = "isDeviceConnected_capi")]
        public static extern bool isDeviceConnected();


        /// <summary>
        /// Enable rendering of the haptic feedback. 
        /// Context must be initialized calling this function.
        /// </summary>
        /// <returns>0 if successful, an Error otherwise.</returns>
        [global::System.Runtime.InteropServices.DllImport(DLLNAME, EntryPoint = "enableHapticRendering_capi")]
        public static extern int enableHapticRendering();


        /// <summary>
        /// Disable rendering of the haptic feedback. 
        /// The rendering must be running before this call.
        /// </summary>
        /// <returns>0 if successful, an Error otherwise. </returns>
        [global::System.Runtime.InteropServices.DllImport(DLLNAME, EntryPoint = "disableHapticRendering_capi")]
        public static extern int disableHapticRendering();


        /// <summary>
        /// Check if the haptic rendering is enabled. 
        /// </summary>
        /// <returns>true if enabled, false otherwise. </returns>
        [global::System.Runtime.InteropServices.DllImport(DLLNAME, EntryPoint = "isHapticRenderingEnabled_capi")]
        public static extern bool isHapticRenderingEnabled();


        /// <summary>
        /// Load a playback from the given audio file name and return the 
        /// playback ID in the context.
        /// The returned ID is constant, unique and is valid until the playback 
        /// is unloaded.If another playback is loaded using the same audio
        /// file, it will have another unique ID.
        /// </summary>
        /// <param name="name">name of the audio file to load.</param>
        /// <param name="ID">playback ID.</param>
        /// <returns>0 if playback has been created, an Error otherwise.</returns>
        [global::System.Runtime.InteropServices.DllImport(DLLNAME, EntryPoint = "loadPlayback_capi")]
        public static extern int loadPlayback([MarshalAs(UnmanagedType.LPStr)] string name, ref UInt32 ID);


        /// <summary>
        /// Remove a playback from the context.
        /// </summary>
        /// <param name="ID">playback ID.</param>
        /// <returns>0 if the playback has been removed, an Error otherwise.</returns>
        [global::System.Runtime.InteropServices.DllImport(DLLNAME, EntryPoint = "unloadPlayback_capi")]
        public static extern int unloadPlayback(UInt32 ID);


        /// <summary>
        /// Change playback's state to play.
        /// The context will then compute and render the playback.
        /// </summary>
        /// <param name="ID">playback ID.</param>
        /// <returns>0 if successful, an Error otherwise.</returns>
        [global::System.Runtime.InteropServices.DllImport(DLLNAME, EntryPoint = "startPlayback_capi")]
        public static extern int startPlayback(UInt32 ID);


        /// <summary>
        /// Change playback's state to stop.
        /// The context will then stop computing and rendering the playback.
        /// </summary>
        /// <param name="ID">playback ID.</param>
        /// <returns>0 if successful, an Error otherwise.</returns>
        [global::System.Runtime.InteropServices.DllImport(DLLNAME, EntryPoint = "stopPlayback_capi")]
        public static extern int stopPlayback(UInt32 ID);


        /// <summary>
        /// Change all playbacks' states to stop.
        /// The context will then stop computing and rendering 
        /// any playback and only zeros will be output.
        /// </summary>
        [global::System.Runtime.InteropServices.DllImport(DLLNAME, EntryPoint = "stopAllPlayback_capi")]
        public static extern void stopAllPlayback();


        /// <summary>
        /// Get the number of output channels of the playback.
        /// It is taken from the audio file loaded within the playback.
        /// Each channel can then be bound to a set of actuators.
        /// </summary>
        /// <param name="ID">playback ID.</param>
        /// <returns>the number of the playback's channel, an Error otherwise.</returns>
        [global::System.Runtime.InteropServices.DllImport(DLLNAME, EntryPoint = "getNbChannels_capi")]
        public static extern int getNbChannels(UInt32 ID);


        /// <summary>
        /// Route the given playback's output channels to the device output's 
        /// channels.Each bit of the provided bitfield represents one channel 
        /// of the output, i.e, an actuator on the device.The playback's output 
        /// associated with such binding will be connected in a binary way. 
        /// Either fully connected or fully disconnected. 
        /// The low-weight bit corresponds to the channel 0. 
        /// Setting a bit that doesn't correspond to an actual channel of the 
        /// output device will have no effect. 
        /// </summary>
        /// <param name="ID">playback ID.</param>
        /// <param name="index">index playback's output channel. </param>
        /// <param name="bitfield">mapping between playback output 
        ///         channels and output device's channels. </param>
        /// <returns>0 if successful, an Error otherwise.</returns>
        [global::System.Runtime.InteropServices.DllImport(DLLNAME, EntryPoint = "bindPlayback_capi")]
        public static extern int bindPlayback(UInt32 ID, UInt32 index, UInt32 bitfield);


        /// <summary>
        /// Get the current volume of the playback between 0 and 1. 
        /// </summary>
        /// <param name="ID">playback ID.</param>
        /// <param name="volume">the volume of the playback. </param>
        /// <returns>0 if the volume parameter has been set, an Error otherwise.</returns>
        [global::System.Runtime.InteropServices.DllImport(DLLNAME, EntryPoint = "getPlaybackVolume_capi")]
        public static extern int getPlaybackVolume(UInt32 ID, ref float volume);


        /// <summary>
        /// Set the current volume of the playback.
        /// </summary>
        /// <param name="ID">playback ID.</param>
        /// <param name="volume">the volume to set.</param>
        /// <returns>0 if successful, an Error otherwise.</returns>
        [global::System.Runtime.InteropServices.DllImport(DLLNAME, EntryPoint = "setPlaybackVolume_capi")]
        public static extern int setPlaybackVolume(UInt32 ID, float volume);


        /// <summary>
        /// Get the current volume of the actuator between 0 and 1.
        /// </summary>
        /// <param name="index">index of the targeted actuator.</param>
        /// <param name="volume">volume of the actuator.</param>
        /// <returns>0 if the volume parameter has been set, an Error otherwise.</returns>
        [global::System.Runtime.InteropServices.DllImport(DLLNAME, EntryPoint = "getActuatorVolume_capi")]
        public static extern int getActuatorVolume(UInt32 index, ref float volume);


        /// <summary>
        /// Set the volume of the actuator.
        /// The volume shall be between 0 and 1 and is applied
        /// after all computation of the haptic signal.
        /// </summary>
        /// <param name="index">index of the targeted actuator.</param>
        /// <param name="volume">the volume to set.</param>
        /// <returns>0 if the volume has been set, an error otherwise.</returns>
        [global::System.Runtime.InteropServices.DllImport(DLLNAME, EntryPoint = "setActuatorVolume_capi")]
        public static extern int setActuatorVolume(UInt32 index, float volume);


        /// <summary>
        /// Return the average computation time of the playbacks for 
        /// a single chunk of data of size "buffer_size" passed
        /// to the initPlayer() function.
        /// No compatibility with ASIO.
        ///
        /// The average is computed on "OutputInitOptions.averaging_size_window" 
        /// chunks.
        /// </summary>
        /// <returns>average computation time, in second.</returns>
        [global::System.Runtime.InteropServices.DllImport(DLLNAME, EntryPoint = "getAverageComputeTime_capi")]
        public static extern double getAverageComputeTime();


        /// <summary>
        /// Return the average time the audio stream spent idle waiting for 
        /// the computation to complete.This value may differ from the
        /// "AverageComputeTime" depending on how the platform is handling
        /// thread scheduling.
        /// No compatibility with ASIO.
        ///
        /// The average is computed on "OutputInitOptions.averaging_size_window" 
        /// chunks.
        /// </summary>
        /// <returns>average time of the computation request, in second.</returns>
        [global::System.Runtime.InteropServices.DllImport(DLLNAME, EntryPoint = "getAverageRequestComputeTime_capi")]
        public static extern double getAverageRequestComputeTime();


        /// <summary>
        /// Return the average time the callback of the audio stream uses 
        /// to prepare the next chunk of data to stream.This time includes
        /// the computation time and the handling of internal buffers.
        ///
        /// The callback may timeout if it exceeds the device's latency.
        /// No compatibility with ASIO.
        /// 
        /// The average is computed on "OutputInitOptions.averaging_size_window" 
        /// chunks.
        /// </summary>
        /// <returns>average time for callback to complete, in second.</returns>
        [global::System.Runtime.InteropServices.DllImport(DLLNAME, EntryPoint = "getAverageCallbackTime_capi")]
        public static extern double getAverageCallbackTime();


        /// <summary>
        /// Return the average rate the computation of the effects takes for 
        /// the whole callback to complete.Along the "CPULoad", this value
        /// helps to monitor the cost of the haptic effects computation.
        ///
        /// No compatibility with ASIO.
        /// 
        /// The average is computed on "OutputInitOptions.averaging_size_window" 
        /// chunks.
        /// </summary>
        /// <returns>average computation rate on callback completion.</returns>
        [global::System.Runtime.InteropServices.DllImport(DLLNAME, EntryPoint = "getAverageComputeRate_capi")]
        public static extern double getAverageComputeRate();


        /// <summary>
        /// Return the average CPULoad of the callback. A high CPULoad indicates 
        /// that the audio stream is close to saturation and might cause 
        /// underflows.In that case, it might be needed to change the player's 
        /// settings or reduce the amount of effects to be rendered 
        /// simultaneously, in order to meet the platform computation capability.
        /// 
        /// The average is independent of the
        /// "OutputInitOptions.averaging_size_window" setting, as to make it
        /// independent of the streaming rates.
        /// </summary>
        /// <returns>average CPU load of the audio stream callback.</returns>
        [global::System.Runtime.InteropServices.DllImport(DLLNAME, EntryPoint = "getAverageCPULoad_capi")]
        public static extern double getAverageCPULoad();


        /// <summary>
        /// Internal use only.
        /// </summary>
        [global::System.Runtime.InteropServices.DllImport(DLLNAME, EntryPoint = "getOutputDevicesNames_capi")]
        private static extern IntPtr getOutputDevicesNames();


        /// <summary>
        /// Internal use only.
        /// </summary>
        [global::System.Runtime.InteropServices.DllImport(DLLNAME, EntryPoint = "getOutputDeviceAPIs_capi")]
        private static extern IntPtr getOutputDeviceAPIs([MarshalAs(UnmanagedType.LPStr)] string outputName);


        /// <summary>
        /// Get settings extremum values of the output device identified by 
        /// name and API.
        /// </summary>
        /// <param name="outputName">name of the output. </param>
        /// <param name="apiName">name of the API. </param>
        /// <param name="max_channels">max number of channel. </param>
        /// <param name="default_low_latency">minimum latency of the output device. </param>
        /// <param name="default_high_latency">maximum latency of the output device. </param>
        /// <returns>0 if the values have been set successfully, an Error otherwise. </returns>
        [global::System.Runtime.InteropServices.DllImport(DLLNAME, EntryPoint = "getOutputDeviceInfo_capi")]
        public static extern int getOutputDeviceInfo([MarshalAs(UnmanagedType.LPStr)] string outputName, [MarshalAs(UnmanagedType.LPStr)] string apiName, ref int max_channels, ref double default_low_latency, ref double default_high_latency);


        /// <summary>
        /// Internal use only.
        /// </summary>
        [global::System.Runtime.InteropServices.DllImport(DLLNAME, EntryPoint = "getSupportedStandardFrameRates_capi")]
        private static extern IntPtr getSupportedStandardFrameRates([MarshalAs(UnmanagedType.LPStr)] string outputName, [MarshalAs(UnmanagedType.LPStr)] string apiName);


        /// <summary>
        /// Initialize context with default parameters. 
        /// Create internal structures according to the selected output device.
        /// </summary>
        /// <param name="selected_output">targeted output device.</param>
        /// <returns>0 if the context has been initialized, an Error otherwise. </returns>
        public static int initDefaultContext(OutputDevice selected_output)
        {
            return initContext(selected_output, 1, 8000, 512);
        }

        /// <summary>
        /// Get the version as a string. The format of the string is: 
        /// <pre>major.minor.revision</pre>
        /// </summary>
        /// <returns>the version string</returns>
        public static string GetEngineVersion()
        {
            IntPtr ptr = versionString();
            // assume returned string is utf-8 encoded
            return Marshal.PtrToStringAnsi(ptr);
        }


        /// <summary>
        /// Get names of available audio output devices.
        /// This allows to select which device to use if initializing the 
        /// context with eAudioDevice.
        /// </summary>
        /// <returns>array of device names.</returns>
        public static string[] GetListAudioOutputs()
        {
            string[] tempList = Marshal.PtrToStringAnsi(getOutputDevicesNames()).Split('/');
            string[] availableAudioOutputs = new string[tempList.Length + 1];
            availableAudioOutputs[0] = "default_output";
            tempList.CopyTo(availableAudioOutputs, 1);
            return availableAudioOutputs;
        }


        /// <summary>
        /// Get available APIs for a given output device identified by name. 
        /// </summary>
        /// <param name="outputName">name of the output.</param>
        /// <returns>array of api names.</returns>
        public static string[] GetListAPIs(string outputName)
        {
            string[] availableAPIs;
            if (string.Equals(outputName, "default_output"))
            {
                availableAPIs = new string[] { "any_api" };
            }
            else
            {
                string concatList = Marshal.PtrToStringAnsi(getOutputDeviceAPIs(outputName));
                if (concatList.Length > 0)
                {
                    string[] tempList = concatList.Split('/');
                    availableAPIs = new string[tempList.Length + 1];
                    availableAPIs[0] = "any_api";
                    tempList.CopyTo(availableAPIs, 1);
                }
                else
                {
                    availableAPIs = new string[] { "nothing was returned" };
                }
            }
            return availableAPIs;
        }


        /// <summary>
        /// Get all supported standard sample rates of the output device 
        /// identified by name and API.
        /// </summary>
        /// <param name="outputName">name of the output. </param>
        /// <param name="apiName">name of the API. </param>
        /// <returns>array of available framerates.</returns>
        public static double[] GetListAvailableStandardFrameRates(string outputName, string apiName)
        {
            double[] framerates;
            string concatList = Marshal.PtrToStringAnsi(getSupportedStandardFrameRates(outputName, apiName));
            if (concatList.Length > 0)
            {
                string[] tempList = concatList.Split('/');
                framerates = new double[tempList.Length];
                for (int i = 0; i < tempList.Length; i++)
                {
                    framerates[i] = GetDouble(tempList[i], -1);
                }
            }
            else
            {
                framerates = new double[1];
                framerates[0] = -1;
            }
            return framerates;
        }


        /// <summary>
        /// Internal use only.
        /// </summary>
        private static double GetDouble(string value, double defaultValue)
        {
            double result;
            // Try parsing in the current culture
            if (!double.TryParse(value, System.Globalization.NumberStyles.Any, CultureInfo.CurrentCulture, out result) &&
                // Then try in US english
                !double.TryParse(value, System.Globalization.NumberStyles.Any, CultureInfo.GetCultureInfo("en-US"), out result) &&
                // Then in neutral language
                !double.TryParse(value, System.Globalization.NumberStyles.Any, CultureInfo.InvariantCulture, out result))
            {
                result = defaultValue;
            }
            return result;
        }
    }
}