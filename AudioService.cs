using System;
using System.IO;
using NAudio.Wave;
using System.Threading;

namespace CybersecurityAwarenessBot
{
    // Manages audio playback for the chatbot
    public class AudioService : IDisposable
    {
        private WaveOutEvent _waveOut;
        private AudioFileReader _audioFile;
        private bool _isDisposed;

        /// <summary>
        /// Plays a welcome greeting audio file when the chatbot starts.
        /// </summary>
        public void PlayWelcomeAudio()
        {
            try
            {
                // Locate the MP3 file in the base directory
                string filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "greetings_welcome_chatbot.mp3");

                if (File.Exists(filePath))
                {
                    StopPlayback(); // Stop any existing playback

                    _audioFile = new AudioFileReader(filePath);
                    _waveOut = new WaveOutEvent();
                    _waveOut.Init(_audioFile);
                    _waveOut.Play();

                    // Wait until playback finishes
                    while (_waveOut.PlaybackState == PlaybackState.Playing)
                    {
                        Thread.Sleep(100);
                    }
                }
                else
                {
                    Console.WriteLine("Audio file not found: greetings_welcome_chatbot.mp3");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Audio playback error: {ex.Message}");
                Dispose(); // Clean up resources if something goes wrong
            }
        }

        /// <summary>
        /// Plays a simple notification beep sound (cross-platform friendly).
        /// </summary>
        public void PlayNotificationSound()
        {
            try
            {
                Console.Beep(); // Works on most Windows terminals
            }
            catch
            {
                // Fail silently if beep is not supported
            }
        }

        /// <summary>
        /// Stops audio playback and disposes of the current audio file.
        /// </summary>
        public void StopPlayback()
        {
            _waveOut?.Stop();
            _audioFile?.Dispose();
            _audioFile = null;
        }

        /// <summary>
        /// Dispose method for cleaning up audio resources.
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Internal disposal logic
        /// </summary>
        protected virtual void Dispose(bool disposing)
        {
            if (!_isDisposed)
            {
                if (disposing)
                {
                    StopPlayback();
                    _waveOut?.Dispose();
                }
                _isDisposed = true;
            }
        }

        // Finalizer to ensure resources are cleaned up
        ~AudioService()
        {
            Dispose(false);
        }
    }
}
