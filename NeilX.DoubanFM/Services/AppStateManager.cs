using Microsoft.Practices.ServiceLocation;
using NeilSoft.UWP;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;

namespace NeilX.DoubanFM.Services
{
   public  class AppStateManager
    {
        public AppState ForegroundAppState { get; set; }

        public AppStateManager(Application app)
        {
            app.Resuming += App_Resuming;
            app.Suspending += App_Suspending;
        }



        private void App_Resuming(object sender, object e)
        {
            AppSettingsHelper.SaveSettingToLocalSettings(AppSettingsConstants.AppState, AppState.Active.ToString());
  // Verify the task is running
            //if (IsMyBackgroundTaskRunning)
            //{
            //    // If yes, it's safe to reconnect to media play handlers
            //   // AddMediaPlayerEventHandlers();

            //    // Send message to background task that app is resumed so it can start sending notifications again
            //    //MessageService.SendMessageToBackground(new AppResumedMessage());

            //    //UpdateTransportControls(CurrentPlayer.CurrentState);

            //    //var trackId = GetCurrentTrackIdAfterAppResume();
            //    //txtCurrentTrack.Text = trackId == null ? string.Empty : playlistView.GetSongById(trackId).Title;
            //    //txtCurrentState.Text = CurrentPlayer.CurrentState.ToString();
            //}
            //else
            //{
            //    //playButton.Content = ">";     // Change to play button
            //    //txtCurrentTrack.Text = string.Empty;
            //    //txtCurrentState.Text = "Background Task Not Running";
            //}
        }
        private void App_Suspending(object sender, Windows.ApplicationModel.SuspendingEventArgs e)
        {
            var deferral = e.SuspendingOperation.GetDeferral();
            if (PlayerSessionService.IsActived)
            {
                ServiceLocator.Current.GetInstance<PlayerSessionService>().Close();
            }
            AppSettingsHelper.SaveSettingToLocalSettings(AppSettingsConstants.AppState, AppState.Suspended.ToString());
            deferral.Complete();
        }
    }
}
