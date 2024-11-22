using ActionTrack.Core;
using System;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;
using System.Collections.Generic;
using ActionTrack.MVVM.ViewModal;
using System.Runtime.InteropServices;
using System.Runtime;
using System.Windows.Interop;
namespace ActionTrack
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly AuthenticationService _authService;

        public MainWindow()
        {
            InitializeComponent();
            _authService = new AuthenticationService();
            DataContext = new MainViewModal();
            this.MaxHeight = SystemParameters.MaximizedPrimaryScreenHeight;
            /*Loaded += async (s, e) =>
            {
                
                await LoadMemberNames();
            };*/
           

            

        }
        [DllImport("user32.dll")]
        public static extern IntPtr SendMessage(IntPtr hWnd, int wMsg, int wParam, int lParam);

        private void ControlBar_MouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            WindowInteropHelper helper = new WindowInteropHelper(this);
            SendMessage(helper.Handle, 161,2,0);
        }


        public void SetUserToken(string token)
        {
            Console.WriteLine($"User token received: {token}");
            LoadUserData(token);
        }



        private async void LoadUserData(string token)
        {
            try
            {
                var userData = await _authService.GetUserDataAsync(token);

                if (userData != null)
                {
                    DepartmentTitle.Text = userData.Department;
                }
                else
                {
                    DepartmentTitle.Text = "ERROR!";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Failed to load user data: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            } 
            
          
        }

        


        /*private void AddMemberButton(string name) //string commandBinding
        {
            RadioButton newRB = new RadioButton
            {
                Content = name,
                Height = 50,
                Foreground = Brushes.White,
                FontSize = 14,
                IsChecked = false,
                Style = (Style)FindResource("MemberButtonTheme")
            };

            // newRB.SetBinding(RadioButton.CommandProperty, new Binding(commandBinding));
            MemberStackPanel.Children.Add(newRB);
        }
        public async Task LoadMemberNames()
        {
            
            // Calling the method from AuthenticationService to get member names
            var memberNames = await _authService.GetMemberNamesAsync(); // Now using _authService to get names
            MemberStackPanel.Children.Clear(); // Clear existing

            foreach (var name in memberNames)
            {
                Console.WriteLine(name);  // Output each member name
                AddMemberButton(name);    // Add each member name as a button to the StackPanel
            }
        }*/

        private void ExitButton(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }
        private void WinButton(object sender, RoutedEventArgs e)
        {
            if (this.WindowState == WindowState.Normal)
                this.WindowState = WindowState.Maximized;
            else this.WindowState = WindowState.Normal;
            
        }
        private void MinimizeButton(object sender, RoutedEventArgs e)
        {
            this.WindowState = WindowState.Minimized;
        }

        private async void ShowMemberInfo(object sender, RoutedEventArgs e)
        {
            RadioButton rB = sender as RadioButton;

            if (rB != null)
            {
                string membername = rB.Content.ToString();

                var memberData = await _authService.GetMemberInfo(membername);

                if (memberData != null)
                {

                }
            }
        }

        


        /*private void CreateReportButton(object sender, RoutedEventArgs e)
        {
            if (CreateReportBtn.IsVisible) 
            {
                CreateReportBtn.Visibility = Visibility.Collapsed;

                RadioButton rB = new RadioButton
                {
                    Content = "Report",
                    Height = 50,
                    Width = 180,
                    FontSize = 14,
                    Foreground = Brushes.White,
                    IsChecked = true,
                    Style = (Style)FindResource("MemberButtonTheme")
                    
                };
                rB.SetBinding(RadioButton.CommandProperty, new Binding("ReportWriterCommand"));
                WindowBar.Children.Add(rB);
            }
        }*/
    }
}
