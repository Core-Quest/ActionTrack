using ActionTrack.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ActionTrack.MVVM.View
{
    /// <summary>
    /// Interaction logic for SettingsView.xaml
    /// </summary>
    public partial class SettingsView : UserControl
    {
        private readonly AuthenticationService _authService;
        public SettingsView()
        {
            InitializeComponent();
            _authService = new AuthenticationService();
            _ = SetupSettingsViewAsync();
        }

        private async Task SetupSettingsViewAsync()
        {
            try
            {
                var settingsData = await DeviceSettings.GetDeviceSettingsAsync();
                var userData = await _authService.GetUserDataAsync(null); // Call Using Already Set Token

                if (userData != null)
                {
                    Console.WriteLine($"UserName: {userData.Username}");
                    SettingUserName.Text = userData.Username;
                    SettingUserToken.Text = userData.UserToken;
                }

                if (settingsData != null)
                {
                    SettingDeviceNameText.Text = settingsData.DeviceName;
                    SettingApperatusText.Text = settingsData.ApparatusName;
                    switch (settingsData.ApparatusType)
                    {
                        case "8seater":
                            SettingApperatusSeatText.Text = "8 Seater";
                            break;
                        case "6seater":
                            SettingApperatusSeatText.Text = "6 Seater";
                            break;
                        case "5seater":
                            SettingApperatusSeatText.Text = "5 Seater";
                            break;
                    }
                }
                else
                {
                    Console.WriteLine("No settings found for the current device.");
                }
            }
            catch (Exception ex) { Console.WriteLine(ex.Message); }
        }
    }

}
