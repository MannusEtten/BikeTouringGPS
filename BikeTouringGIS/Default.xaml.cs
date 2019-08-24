﻿using BikeTouringGIS.ViewModels;
using log4net;
using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;
using Newtonsoft.Json;
using Squirrel;
using System;
using System.IO;
using System.IO.IsolatedStorage;
using System.Reflection;
using System.Windows;

namespace BikeTouringGIS
{
    /// <summary>
    /// Interaction logic for Default.xaml
    /// </summary>
    public partial class Default : MetroWindow
    {
        private static readonly ILog _log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        public Default()
        {
            InitializeComponent();
        }

        private async void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                using (var mgr = await UpdateManager.GitHubUpdateManager("https://github.com/MannusEtten/BikeTouringGIS"))
                {
                    var updateInfo = await mgr.CheckForUpdate();
                    var currentVersion = updateInfo.CurrentlyInstalledVersion?.Version;
                    var futureVersion = updateInfo.FutureReleaseEntry.Version;
                    if (currentVersion != futureVersion)
                    {
                        var window = Application.Current.MainWindow as MetroWindow;
                        var controller = await window.ShowProgressAsync("Please wait...", "Updating application");
                        await mgr.UpdateApp();
                        await controller.CloseAsync();
                    }
                }
            }
            catch (Exception ex)
            {
                _log.Error(ex);
            }
            var context = (BikeTouringGISViewModel)DataContext;
            try
            {
                IsolatedStorageFile isolatedStorage = IsolatedStorageFile.GetUserStoreForAssembly();
                StreamReader srReader = new StreamReader(new IsolatedStorageFileStream("settings", FileMode.OpenOrCreate, isolatedStorage));
                if (srReader != null)
                {
                    string data = srReader.ReadToEnd();
                    if (string.IsNullOrEmpty(data))
                    {
                        context.SplitLength = 100;
                        context.ConvertTracksToRoutesAutomatically = false;
                    }
                    else
                    {
                        dynamic jsonObject = JsonConvert.DeserializeObject(data);
                        context.SplitLength = jsonObject.SplitLength;
                        context.ConvertTracksToRoutesAutomatically = jsonObject.ConvertTracksToRoutesAutomatically;
                    }
                    srReader.Close();
                }
            }
            catch (Exception ex)
            {
                context.SplitLength = 100;
                _log.Error(ex);
            }
        }
    }
}