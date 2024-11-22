using ActionTrack.Core;
using ActionTrack.MVVM.Model.DataAccess;
using Npgsql;
using System.Data;
using System.Windows;
using System;
using System.Windows.Threading;
using System.Threading.Tasks;

namespace ActionTrack
{
    public partial class LoginWindow : Window
    {
        private readonly AuthenticationService _authService;

        public LoginWindow()
        {
            InitializeComponent();
            _authService = new AuthenticationService();
            _ = CheckLoginStatusAsync();
            StartRotatingBorder();
        }

        private async Task CheckLoginStatusAsync()
        {
            var (isLoggedIn, userToken) = await _authService.IsUserLoggedInAsync();

            if (isLoggedIn)
            {
                // Open the login window if not logged in
                var mainwin = new MainWindow();
                mainwin.SetUserToken(userToken);
                mainwin.Show();

                // Close the main window
                this.Close();
            }
            
        }



        private DispatcherTimer rotationTimer;
        private double angle = 0; // Angle in radians for smooth sine-based movement

        private void StartRotatingBorder()
        {
            rotationTimer = new DispatcherTimer
            {
                Interval = TimeSpan.FromMilliseconds(16) // Approximately 60 FPS
            };

            rotationTimer.Tick += (s, e) => RotateBorder();
            rotationTimer.Start();
        }

        private void RotateBorder()
        {
            // Use sine wave for smooth oscillation
            angle += 0.005; // Adjust increment for speed; smaller values = slower motion
            if (angle > 2 * Math.PI) angle -= 2 * Math.PI; // Reset angle to avoid overflow

            // Map sine wave (-1 to 1) to gradient offset range (0 to 1)
            LoginGradientBorderRHighlight.Offset = (Math.Sin(angle) + 1) / 2;
        }


        private void LoginExit_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private async void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string username = UsernameTextBox.Text;
                string password = PasswordTextBox.Password;

                var dbService = new DatabaseService("Host=localhost;Port=5432;Username=postgres;Password=!LoveLilyO23;Database=postgres");

                string query = "SELECT userToken FROM UserData WHERE username = @username AND password = @password";
                var parameters = new[]
                {
            new NpgsqlParameter("@username", DbType.String) { Value = username },
            new NpgsqlParameter("@password", DbType.String) { Value = password }
        };

                object result = await dbService.ExecuteScalarAsync(query, parameters);

                string userToken = result as string;
                if (userToken == null || result == DBNull.Value)
                {
                    MessageBox.Show("Login failed. Please try again.");
                    return;
                }

                if (LoginRememberMe.IsChecked == true)
                {
                    _authService.SaveUserDataAsync(userToken, true);
                }

                Console.WriteLine($"User token: {userToken}");

                var mainWindow = new MainWindow();
                mainWindow.SetUserToken(userToken);
                mainWindow.Show();

                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred: {ex.Message}");
            }
        }


    }
}
