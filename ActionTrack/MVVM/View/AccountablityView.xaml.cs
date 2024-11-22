using ActionTrack.Core;
using ActionTrack.MVVM.ViewModal;
using System;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace ActionTrack.MVVM.View
{
    public partial class AccountablityView : UserControl
    {
        public AccountablityView()
        {
            InitializeComponent();
            DataContext = new AccountabilityViewModal();

            // Initialize asynchronously after the view is loaded
            _ = SetupAccountabilityViewAsync();
        }

        /// <summary>
        /// Initializes the AccountabilityView asynchronously.
        /// </summary>
        private async Task SetupAccountabilityViewAsync()
        {
            try
            {
                var settingsData = await DeviceSettings.GetDeviceSettingsAsync();

                if (settingsData != null)
                {
                    Console.WriteLine($"Device Name: {settingsData.DeviceName}");
                    Console.WriteLine($"Apparatus Name: {settingsData.ApparatusName}");
                    Console.WriteLine($"Apparatus Type: {settingsData.ApparatusType}");

                    // Perform any further actions with the settings data
                    ApperatusName.Text = settingsData.ApparatusName;
                    switch (settingsData.ApparatusType)
                    {
                        case "8seater":
                            EightSeatLayout.Visibility = Visibility.Visible;
                            SixSeatLayout.Visibility = Visibility.Collapsed;
                            FiveSeatLayout.Visibility = Visibility.Collapsed;
                            break;
                        case "6seater":
                            EightSeatLayout.Visibility = Visibility.Collapsed;
                            SixSeatLayout.Visibility = Visibility.Visible;
                            FiveSeatLayout.Visibility = Visibility.Collapsed;
                            break;
                        case "5seater":
                            EightSeatLayout.Visibility = Visibility.Collapsed;
                            SixSeatLayout.Visibility = Visibility.Collapsed;
                            FiveSeatLayout.Visibility = Visibility.Visible;
                            break;
                    }
                }
                else
                {
                    Console.WriteLine("No settings found for the current device.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
            }
        }
        /*
         Eight Seat Layout
        1 = Driver
        2 = Officer
        3 = Behind Driver
        4 = Behind Officer
        5 = Left Outer Forward Face
        6 = Left Center Forward Face
        7 = Right Center Forward Face
        8 = Right Outer Forward Face

        Six Seat Layout
        1 = Driver
        2 = Officer
        3 = Behind Driver
        4 = Behind Officer
        5 = Left Center Forward Face
        6 = Right Center Forward Face
         */
        private void AS_Click(object sender, RoutedEventArgs e)
        {
            RadioButton clickedButton = sender as RadioButton;

            if (clickedButton != null)
            {
                var dialog = new Dialog();
                dialog.MemberListACDialog(clickedButton.Name);
                dialog.ShowDialog();


            }
        }

        public void SetMemberToSeat(string memberID, string seat)
        {
            Console.WriteLine($"Setting Member: {memberID} to {seat}");
            
            switch (seat)
            {
                case "SS_1":
                    Application.Current.Dispatcher.Invoke(() =>
                    {
                        SS_1.Background = Brushes.Black;
                    });

                    break;
            }
        }
       
    }
}
