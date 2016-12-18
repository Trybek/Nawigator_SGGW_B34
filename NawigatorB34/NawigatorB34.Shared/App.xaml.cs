using System;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Windows.ApplicationModel;
using Windows.ApplicationModel.Activation;
using Windows.Storage;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Animation;
using Windows.UI.Xaml.Navigation;

namespace Nawigator_SGGW_B34
{
    public sealed partial class App : Application
    {
#if WINDOWS_PHONE_APP
        private TransitionCollection transitions;
#endif

        public static int IDRoomNotification;
        public static int FontSize;
        public static int TimerNotifications;
        public static string ReadRoomsOnFloor;

        public App()
        {
            this.InitializeComponent();
            this.Suspending += this.OnSuspending;

            var settings = ApplicationData.Current.RoamingSettings;
            if (settings.Values.ContainsKey("FontSize"))
            {
                FontSize = (int)settings.Values["FontSize"];
            }
            else
            {
                FontSize = 20;
            }

            if (settings.Values.ContainsKey("TimerNotifications"))
            {
                TimerNotifications = (int)settings.Values["TimerNotifications"];
            }
            else
            {
                TimerNotifications = 15;
            }

            if (settings.Values.ContainsKey("ReadRoomsOnFloor"))
            {
                ReadRoomsOnFloor = settings.Values["ReadRoomsOnFloor"].ToString();
            }
            else
            {
                ReadRoomsOnFloor = "-1;0;1;2;3";
            }

            if (!CheckFileExists("NawigatorDB.db3").Result)
            {
                CopyDatabase();
            }

        }

        private async Task<bool> CheckFileExists(string fileName)
        {
            try
            {
                var store = await ApplicationData.Current.LocalFolder.GetFileAsync(fileName);
                return true;
            }
            catch
            {
                return false;
            }
        }
        private async void CopyDatabase()
        {
            StorageFile databaseFile = await Package.Current.InstalledLocation.GetFileAsync("NawigatorDB.db3");
            await databaseFile.CopyAsync(ApplicationData.Current.LocalFolder);
        }

        protected override void OnLaunched(LaunchActivatedEventArgs e)
        {
#if DEBUG
            if (System.Diagnostics.Debugger.IsAttached)
            {
                this.DebugSettings.EnableFrameRateCounter = true;
            }
#endif
            if (!string.IsNullOrWhiteSpace(e.Arguments))
            {
                var toastLaunch = Regex.Match(e.Arguments, @"^toast://(?<arguments>.*)$");
                var toastActivationArgs = toastLaunch.Groups["arguments"];
                if (toastActivationArgs.Success)
                {
                    // The app has been activated through a toast notification click.
                    var arguments = toastActivationArgs.Value;
                    IDRoomNotification = int.Parse(arguments);
                }
            }
            else
            {
                IDRoomNotification = -1;
            }

            Frame rootFrame = Window.Current.Content as Frame;

            // Do not repeat app initialization when the Window already has content,
            // just ensure that the window is active
            if (rootFrame == null)
            {
                // Create a Frame to act as the navigation context and navigate to the first page
                rootFrame = new Frame();

                // TODO: change this value to a cache size that is appropriate for your application
                rootFrame.CacheSize = 1;

                if (e.PreviousExecutionState == ApplicationExecutionState.Terminated)
                {
                    // TODO: Load state from previously suspended application
                }

                // Place the frame in the current Window
                Window.Current.Content = rootFrame;
            }

            if (rootFrame.Content == null)
            {
#if WINDOWS_PHONE_APP
                // Removes the turnstile navigation for startup.
                if (rootFrame.ContentTransitions != null)
                {
                    this.transitions = new TransitionCollection();
                    foreach (var c in rootFrame.ContentTransitions)
                    {
                        this.transitions.Add(c);
                    }
                }

                rootFrame.ContentTransitions = null;
                rootFrame.Navigated += this.RootFrame_FirstNavigated;
#endif

                // When the navigation stack isn't restored navigate to the first page,
                // configuring the new page by passing required information as a navigation
                // parameter
                if (!rootFrame.Navigate(typeof(MainPage), e.Arguments))
                {
                    throw new Exception("Failed to create initial page");
                }
            }

            // Ensure the current window is active
            Window.Current.Activate();
        }

#if WINDOWS_PHONE_APP
        /// <summary>
        /// Restores the content transitions after the app has launched.
        /// </summary>
        /// <param name="sender">The object where the handler is attached.</param>
        /// <param name="e">Details about the navigation event.</param>
        private void RootFrame_FirstNavigated(object sender, NavigationEventArgs e)
        {
            var rootFrame = sender as Frame;
            rootFrame.ContentTransitions = this.transitions ?? new TransitionCollection() { new NavigationThemeTransition() };
            rootFrame.Navigated -= this.RootFrame_FirstNavigated;
        }
#endif

        /// <summary>
        /// Invoked when application execution is being suspended.  Application state is saved
        /// without knowing whether the application will be terminated or resumed with the contents
        /// of memory still intact.
        /// </summary>
        /// <param name="sender">The source of the suspend request.</param>
        /// <param name="e">Details about the suspend request.</param>
        private void OnSuspending(object sender, SuspendingEventArgs e)
        {
            var deferral = e.SuspendingOperation.GetDeferral();

            // TODO: Save application state and stop any background activity
            deferral.Complete();
        }
    }
}