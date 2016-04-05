using NeilSoft.UWP;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Threading;
using Windows.ApplicationModel.Background;
using Windows.Foundation;
using Windows.Media;
using Windows.Media.Core;
using Windows.Media.Playback;
using Windows.Storage.Streams;
using NeilX.DoubanFM.MusicPlayer;
using NeilX.DoubanFM.MusicPlayer.Messages;
using NeilX.DoubanFM.Core;
using NeilX.DoubanFM.BackgroundTask.Helper;
using NeilX.DoubanFM.MusicPlayer.Server;
using Windows.Foundation.Collections;

namespace NeilX.DoubanFM.BackgroundTask
{
    public sealed class AudioPlayerTask : IBackgroundTask
    {
        #region Private fields, properties
        private SystemMediaTransportControls smtc;
        private MediaPlaybackList playbackList = new MediaPlaybackList();
        private BackgroundTaskDeferral deferral; // Used to keep task alive
        private AppState foregroundAppState = AppState.Unknown;
        #endregion


        #region IBackgroundTask and IBackgroundTaskInstance Interface Members and handlers

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


        #endregion



        private void CheckOutAppState()
        {
            var value = ApplicationSettingsHelper.ReadResetSettingsValue(ApplicationSettingsConstants.AppState);
            if (value == null)
                foregroundAppState = AppState.Unknown;
            else
                foregroundAppState = EnumHelper.Parse<AppState>(value.ToString());
            //if (foregroundAppState != AppState.Suspended)
            //    MessageService.SendMessageToForeground(new AudioTaskStartedMessage());

            ApplicationSettingsHelper.SaveSettingToLocalSettings(ApplicationSettingsConstants.BackgroundTaskState, BackgroundTaskState.Running.ToString());

        }

        private void InitialSTMC()
        {
            if (smtc == null)
            {
                smtc = BackgroundMediaPlayer.Current.SystemMediaTransportControls;
            }
            smtc.ButtonPressed += smtc_ButtonPressed;
            smtc.PropertyChanged += smtc_PropertyChanged;
            smtc.IsEnabled = true;
            smtc.IsPauseEnabled = true;
            smtc.IsPlayEnabled = true;
            smtc.IsNextEnabled = true;
            smtc.IsPreviousEnabled = true;
        }


        private void Shutdown()
        {
            // unsubscribe from list changes
            //if (playbackList != null)
            //{
            //    playbackList.CurrentItemChanged -= PlaybackList_CurrentItemChanged;
            //    playbackList = null;
            //}

            //// unsubscribe event handlers
            //BackgroundMediaPlayer.MessageReceivedFromForeground -= BackgroundMediaPlayer_MessageReceivedFromForeground;
            //smtc.ButtonPressed -= smtc_ButtonPressed;
            //smtc.PropertyChanged -= smtc_PropertyChanged;
            //if (BackgroundMediaPlayer.Current.CurrentState != MediaPlayerState.Closed)
            //{
            //    BackgroundMediaPlayer.Shutdown(); // shutdown media pipeline
            //}
        }

        #region SysteMediaTransportControls related functions and handlers
        /// <summary>
        /// Update Universal Volume Control (UVC) using SystemMediaTransPortControl APIs
        /// </summary>
        private void UpdateUVCOnNewTrack(MediaPlaybackItem item)
        {
            if (item == null)
            {
                smtc.PlaybackStatus = MediaPlaybackStatus.Stopped;
                smtc.DisplayUpdater.MusicProperties.Title = string.Empty;
                smtc.DisplayUpdater.Update();
                return;
            }

            smtc.PlaybackStatus = MediaPlaybackStatus.Playing;
            smtc.DisplayUpdater.Type = MediaPlaybackType.Music;
            smtc.DisplayUpdater.MusicProperties.Title = item.Source.CustomProperties[TaskConstant.TitleKey] as string;

            var albumArtUri = item.Source.CustomProperties[TaskConstant.AlbumArtKey] as Uri;
            if (albumArtUri != null)
                smtc.DisplayUpdater.Thumbnail = RandomAccessStreamReference.CreateFromUri(albumArtUri);
            else
                smtc.DisplayUpdater.Thumbnail = null;

            smtc.DisplayUpdater.Update();
        }

        /// <summary>
        /// Fires when any SystemMediaTransportControl property is changed by system or user
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        void smtc_PropertyChanged(SystemMediaTransportControls sender, SystemMediaTransportControlsPropertyChangedEventArgs args)
        {
            // If soundlevel turns to muted, app can choose to pause the music
        }

        /// <summary>
        /// This function controls the button events from UVC.
        /// This code if not run in background process, will not be able to handle button pressed events when app is suspended.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        private void smtc_ButtonPressed(SystemMediaTransportControls sender, SystemMediaTransportControlsButtonPressedEventArgs args)
        {
            switch (args.Button)
            {
                case SystemMediaTransportControlsButton.Play:
                    Debug.WriteLine("UVC play button pressed");

                    // When the background task has been suspended and the SMTC
                    // starts it again asynchronously, some time is needed to let
                    // the task startup process in Run() complete.

                    // Wait for task to start. 
                    // Once started, this stays signaled until shutdown so it won't wait
                    // again unless it needs to.
                    //bool result = backgroundTaskStarted.WaitOne(5000);
                    //if (!result)
                    //    throw new Exception("Background Task didnt initialize in time");

                    StartPlayback();
                    break;
                case SystemMediaTransportControlsButton.Pause:
                    Debug.WriteLine("UVC pause button pressed");
                    try
                    {
                        BackgroundMediaPlayer.Current.Pause();
                    }
                    catch (Exception ex)
                    {
                        Debug.WriteLine(ex.ToString());
                    }
                    break;
                case SystemMediaTransportControlsButton.Next:
                    Debug.WriteLine("UVC next button pressed");
                    SkipToNext();
                    break;
                case SystemMediaTransportControlsButton.Previous:
                    Debug.WriteLine("UVC previous button pressed");
                    SkipToPrevious();
                    break;
            }
        }



        #endregion

        #region Playlist management functions and handlers
        /// <summary>
        /// Start playlist and change UVC state
        /// </summary>
        private void StartPlayback()
        {
            try
            {
                // If playback was already started once we can just resume playing.
                //if (!playbackStartedPreviously)
                //{
                //    playbackStartedPreviously = true;

                //    // If the task was cancelled we would have saved the current track and its position. We will try playback from there.
                //    var currentTrackId = ApplicationSettingsHelper.ReadResetSettingsValue(ApplicationSettingsConstants.TrackId);
                //    var currentTrackPosition = ApplicationSettingsHelper.ReadResetSettingsValue(ApplicationSettingsConstants.Position);
                //    if (currentTrackId != null)
                //    {
                //        // Find the index of the item by name
                //        var index = playbackList.Items.ToList().FindIndex(item =>
                //            AudioTaskHelper.GetPlaybackListItemTrackId(item).ToString() == (string)currentTrackId);

                //        if (currentTrackPosition == null)
                //        {
                //            // Play from start if we dont have position
                //            Debug.WriteLine("StartPlayback: Switching to track " + index);
                //            playbackList.MoveTo((uint)index);

                //            // Begin playing
                //            BackgroundMediaPlayer.Current.Play();
                //        }
                //        else
                //        {
                //            // Play from exact position otherwise
                //            TypedEventHandler<MediaPlaybackList, CurrentMediaPlaybackItemChangedEventArgs> handler = null;
                //            handler = (MediaPlaybackList list, CurrentMediaPlaybackItemChangedEventArgs args) =>
                //            {
                //                if (args.NewItem == playbackList.Items[index])
                //                {
                //                    // Unsubscribe because this only had to run once for this item
                //                    playbackList.CurrentItemChanged -= handler;

                //                    // Set position
                //                    var position = TimeSpan.Parse((string)currentTrackPosition);
                //                    Debug.WriteLine("StartPlayback: Setting Position " + position);
                //                    BackgroundMediaPlayer.Current.Position = position;

                //                    // Begin playing
                //                    BackgroundMediaPlayer.Current.Play();
                //                }
                //            };
                //            playbackList.CurrentItemChanged += handler;

                //            // Switch to the track which will trigger an item changed event
                //            Debug.WriteLine("StartPlayback: Switching to track " + index);
                //            playbackList.MoveTo((uint)index);
                //        }
                //    }
                //    else
                //    {
                //        // Begin playing
                //        BackgroundMediaPlayer.Current.Play();
                //    }
                //}
                //else
                //{
                //    // Begin playing
                //    BackgroundMediaPlayer.Current.Play();
                //}
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.ToString());
            }
        }

        void PlaybackList_CurrentItemChanged(MediaPlaybackList sender, CurrentMediaPlaybackItemChangedEventArgs args)
        {
            // Get the new item
            var item = args.NewItem;
            Debug.WriteLine("PlaybackList_CurrentItemChanged: " + (item == null ? "null" : AudioTaskHelper.GetPlaybackListItemTrackId(item).ToString()));

            // Update the system view
            UpdateUVCOnNewTrack(item);

            // Get the current track
            Uri currentTrackId = null;
            if (item != null)
                currentTrackId = item.Source.CustomProperties[TaskConstant.TrackIdKey] as Uri;
            else
            {
                return;
            }
            // Notify foreground of change or persist for later
            //if (foregroundAppState == AppState.Active)
            //    MessageService.SendMessageToForeground(new TrackChangedMessage(currentTrackId.AbsoluteUri));
            //else
            //    ApplicationSettingsHelper.SaveSettingToLocalSettings(TaskConstant.TrackIdKey, currentTrackId == null ? null : currentTrackId.ToString());
        }

        private void SkipToPrevious()
        {
            smtc.PlaybackStatus = MediaPlaybackStatus.Changing;
            playbackList.MovePrevious();
        }

        private void SkipToNext()
        {
            smtc.PlaybackStatus = MediaPlaybackStatus.Changing;
            playbackList.MoveNext();
        }
        void CreatePlaybackList(MediaPlaybackList playlist, IEnumerable<Song> tracks)
        {
            if (playbackList != null)
            {
                playbackList.CurrentItemChanged -= PlaybackList_CurrentItemChanged;
            }
            // Make a new list and enable looping
            playbackList = new MediaPlaybackList();
            playbackList.AutoRepeatEnabled = true;

            // Add playback items to the list
            foreach (var track in tracks)
            {
                Uri songUri = new Uri(track.Url);
                var source = MediaSource.CreateFromUri(songUri);
                source.CustomProperties[TaskConstant.TrackIdKey] = songUri;
                source.CustomProperties[TaskConstant.TitleKey] = track.Title;
                source.CustomProperties[TaskConstant.AlbumArtKey] = new Uri(track.PictureUrl);
                playbackList.Items.Add(new MediaPlaybackItem(source));
            }

            // Don't auto start
            BackgroundMediaPlayer.Current.AutoPlay = false;

            // Assign the list to the player
            BackgroundMediaPlayer.Current.Source = playbackList;

            // Add handler for future playlist item changes
            playbackList.CurrentItemChanged += PlaybackList_CurrentItemChanged;
        }

        #endregion

        #region Background Media Player Handlers


        void BackgroundMediaPlayer_MessageReceivedFromForeground(object sender, MediaPlayerDataReceivedEventArgs e)
        {
            //AppSuspendedMessage appSuspendedMessage;
            //if (MessageConstant.TryParseMessage(e.Data, out appSuspendedMessage))
            //{
            //    Debug.WriteLine("App suspending"); // App is suspended, you can save your task state at this point
            //    foregroundAppState = AppState.Suspended;
            //    var currentTrackId = AudioTaskHelper.GetPlayListCurrentTrackId(playbackList);
            //    ApplicationSettingsHelper.SaveSettingToLocalSettings(ApplicationSettingsConstants.TrackId, currentTrackId == null ? null : currentTrackId.ToString());
            //    return;
            //}

            //AppResumedMessage appResumedMessage;
            //if (MessageConstant.TryParseMessage(e.Data, out appResumedMessage))
            //{
            //    Debug.WriteLine("App resuming"); // App is resumed, now subscribe to message channel
            //    foregroundAppState = AppState.Active;
            //    return;
            //}

            //StartPlaybackMessage startPlaybackMessage;
            //if (MessageConstant.TryParseMessage(e.Data, out startPlaybackMessage))
            //{
            //    //Foreground App process has signalled that it is ready for playback
            //    Debug.WriteLine("Starting Playback");
            //    StartPlayback();
            //    return;
            //}

            //SkipNextMessage skipNextMessage;
            //if (MessageConstant.TryParseMessage(e.Data, out skipNextMessage))
            //{
            //    // User has chosen to skip track from app context.
            //    Debug.WriteLine("Skipping to next");
            //    SkipToNext();
            //    return;
            //}

            //SkipPreviousMessage skipPreviousMessage;
            //if (MessageConstant.TryParseMessage(e.Data, out skipPreviousMessage))
            //{
            //    // User has chosen to skip track from app context.
            //    Debug.WriteLine("Skipping to previous");
            //    SkipToPrevious();
            //    return;
            //}

            //TrackChangedMessage trackChangedMessage;
            //if (MessageConstant.TryParseMessage(e.Data, out trackChangedMessage))
            //{
            //    var index = playbackList.Items.ToList().FindIndex(i => (Uri)i.Source.CustomProperties[TaskConstant.TrackIdKey] == new Uri( trackChangedMessage.SongUri));
            //    Debug.WriteLine("Skipping to track " + index);
            //    smtc.PlaybackStatus = MediaPlaybackStatus.Changing;
            //    playbackList.MoveTo((uint)index);
            //    return;
            //}
            //PlayModeChangeMessage playModeChangeMessage;
            //if (MessageConstant.TryParseMessage(e.Data, out playModeChangeMessage))
            //{
            //    //TODO
            //    return;
            //}
            //UpdatePlaylistMessage updatePlaylistMessage;
            //if (MessageConstant.TryParseMessage(e.Data, out updatePlaylistMessage))
            //{
            //    CreatePlaybackList(playbackList, updatePlaylistMessage.Tracks);
            //    return;
            //}
        }

        #endregion

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
                // save state
                ApplicationSettingsHelper.SaveSettingToLocalSettings(ApplicationSettingsConstants.TrackId, AudioTaskHelper.GetPlayListCurrentTrackId(playbackList) == null ? null : AudioTaskHelper.GetPlayListCurrentTrackId(playbackList).ToString());
                ApplicationSettingsHelper.SaveSettingToLocalSettings(ApplicationSettingsConstants.Position, BackgroundMediaPlayer.Current.Position.ToString());
                ApplicationSettingsHelper.SaveSettingToLocalSettings(ApplicationSettingsConstants.BackgroundTaskState, BackgroundTaskState.Canceled.ToString());
                ApplicationSettingsHelper.SaveSettingToLocalSettings(ApplicationSettingsConstants.AppState, Enum.GetName(typeof(AppState), foregroundAppState));

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
