using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;

namespace CefNet.Internal
{
	public partial class WebViewGlue
	{
		public void CreateOrDestroyAudioGlue()
		{
			if (AvoidGetAudioParameters()
				&& AvoidOnAudioStreamStarted()
				&& AvoidOnAudioStreamPacket()
				&& AvoidOnAudioStreamStopped()
				&& AvoidOnAudioStreamError())
			{
				this.AudioGlue = null;
			}
			else if (this.AudioGlue is null)
			{
				this.AudioGlue = new CefAudioHandlerGlue(this);
			}
		}

		[MethodImpl(MethodImplOptions.ForwardRef)]
		internal extern bool AvoidGetAudioParameters();

		/// <summary>
		/// Called on the UI thread to allow configuration of audio stream parameters.
		/// </summary>
		/// <param name="browser">The browser.</param>
		/// <param name="params">
		/// All members of <paramref name="params"/> can optionally be configured here, but
		/// they are also pre-filled with some sensible defaults.
		/// </param>
		/// <returns>Return true to proceed with audio stream capture, or false to cancel it. </returns>
		internal protected virtual bool GetAudioParameters(CefBrowser browser, ref CefAudioParameters @params)
		{
			return false;
		}

		[MethodImpl(MethodImplOptions.ForwardRef)]
		internal extern bool AvoidOnAudioStreamStarted();

		/// <summary>
		/// Called on a browser audio capture thread when the browser starts streaming
		/// audio.
		/// </summary>
		/// <param name="browser">The browser.</param>
		/// <param name="params">
		/// Contains the audio parameters like sample rate and channel layout.
		/// </param>
		/// <param name="channels">The number of channels.</param>
		/// <remarks>
		/// OnAudioStreamStopped will always be called after OnAudioStreamStarted;
		/// both functions may be called multiple times for the same browser.
		/// </remarks>
		internal protected virtual void OnAudioStreamStarted(CefBrowser browser, CefAudioParameters @params, int channels)
		{
			
		}

		[MethodImpl(MethodImplOptions.ForwardRef)]
		internal extern bool AvoidOnAudioStreamPacket();

		/// <summary>
		/// Called on the audio stream thread when a PCM packet is received for the
		/// stream.
		/// </summary>
		/// <param name="browser">The browser.</param>
		/// <param name="data">
		/// A pointer to an array representing the raw PCM data as a floating point type,
		/// i.e. 4-byte value(s).<para/>
		/// Based on <paramref name="frames"/> and the |channel_layout| value passed
		/// to OnAudioStreamStarted you can calculate the size of the <paramref name="data"/>
		/// array in bytes.
		/// </param>
		/// <param name="frames">The number of frames in the PCM packet.</param>
		/// <param name="pts">
		/// The presentation timestamp (in milliseconds since the Unix Epoch) and represents
		/// the time at which the decompressed packet should be presented to the user.
		/// </param>
		internal protected virtual void OnAudioStreamPacket(CefBrowser browser, IntPtr data, int frames, long pts)
		{
			
		}

		[MethodImpl(MethodImplOptions.ForwardRef)]
		internal extern bool AvoidOnAudioStreamStopped();

		/// <summary>
		/// Called on the UI thread when the stream has stopped. OnAudioSteamStopped
		/// will always be called after OnAudioStreamStarted; both functions may be
		/// called multiple times for the same stream.
		/// </summary>
		/// <param name="browser">The browser.</param>
		internal protected virtual void OnAudioStreamStopped(CefBrowser browser)
		{

		}

		[MethodImpl(MethodImplOptions.ForwardRef)]
		internal extern bool AvoidOnAudioStreamError();

		/// <summary>
		/// Called on the UI or audio stream thread when an error occurred. During the
		/// stream creation phase this callback will be called on the UI thread while
		/// in the capturing phase it will be called on the audio stream thread. The
		/// stream will be stopped immediately.
		/// </summary>
		/// <param name="browser">The browser.</param>
		/// <param name="message">The error message.</param>
		internal protected virtual void OnAudioStreamError(CefBrowser browser, string message)
		{

		}

	}
}
