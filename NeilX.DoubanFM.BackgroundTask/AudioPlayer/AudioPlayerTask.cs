using System;
using System.Diagnostics;
using Windows.ApplicationModel.Background;
using Windows.Media.Playback;
using NeilX.DoubanFM.MusicPlayer.Messages;
using NeilX.DoubanFM.MusicPlayer.Server;
using Windows.Foundation.Collections;

namespace NeilX.DoubanFM.BackgroundTask
{
    public sealed class AudioPlayerTask : IBackgroundTask
    {
        private BackgroundTaskDeferral deferral;
        private MusicPlayerServer server;

        public void Run(IBackgroundTaskInstance taskInstance)
        {
            Debug.WriteLine("Background Audio Task " + taskInstance.Task.Name + " starting...");
            deferral = taskInstance.GetDeferral();
            // AttachMessageHandlers();
            server = new MusicPlayerServer();
            server.InitialServer();

            ValueSet valueSet = new ValueSet();
            valueSet["MessageId"] = MessageService.BackgroundMediaPlayerActivatedMessageKey;
            BackgroundMediaPlayer.SendMessageToForeground(valueSet);


            taskInstance.Task.Completed += TaskCompleted;
            taskInstance.Canceled += new BackgroundTaskCanceledEventHandler(OnCanceled);
        }

        #region TaskInstance Handler

        void TaskCompleted(BackgroundTaskRegistration sender, BackgroundTaskCompletedEventArgs args)
        {
            Debug.WriteLine("AudioPlayerTask " + sender.TaskId + " Completed...");
            deferral.Complete();
        }

        private void OnCanceled(IBackgroundTaskInstance sender, BackgroundTaskCancellationReason reason)
        {
            try
            {
                server?.Shutdown();
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.ToString());
            }
            deferral.Complete();
            Debug.WriteLine("AudioPlayerTask Cancel complete...");
        }

        #endregion     
    }
}
