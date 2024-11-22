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
    /// Interaction logic for MemberListView.xaml
    /// </summary>
    public partial class MemberListView : UserControl
    {
        private readonly AuthenticationService _authService;
        public MemberListView()
        {
            InitializeComponent();
            _authService = new AuthenticationService();
            Loaded += async (s, e) =>
            {
                await LoadMemberData();
            };
        }


        private void AddMemberCard(string name, string rank, string number, string status)
        {
            // Create the Border for the card
            Border memberCard = new Border
            {
                Margin = new Thickness(10),
                Padding = new Thickness(10),
                Width = 250,
                Height = 100,
                BorderThickness = new Thickness(1),
                CornerRadius = new CornerRadius(8),
                Background = new SolidColorBrush(Color.FromRgb(53, 53, 53))
            };

            // Set the LinearGradientBrush for the BorderBrush
            LinearGradientBrush gradientBrush = new LinearGradientBrush
            {
                StartPoint = new Point(0, 0),
                EndPoint = new Point(1, 1)
            };
            gradientBrush.GradientStops.Add(new GradientStop(Color.FromRgb(2, 15, 26), 0));
            gradientBrush.GradientStops.Add(new GradientStop(Color.FromRgb(26, 114, 186), 0.6));
            gradientBrush.GradientStops.Add(new GradientStop(Color.FromRgb(22, 32, 41), 0.75));

            memberCard.BorderBrush = gradientBrush;

            // Create the StackPanel to hold the TextBlocks
            StackPanel stackPanel = new StackPanel();

            // Add Name TextBlock
            TextBlock nameBlock = new TextBlock
            {
                Text = name,
                FontWeight = FontWeights.Bold,
                FontSize = 16,
                HorizontalAlignment = HorizontalAlignment.Center,
                Foreground = Brushes.White
            };
            stackPanel.Children.Add(nameBlock);

            // Add Rank TextBlock
            TextBlock rankBlock = new TextBlock
            {
                Text = $"Rank: {rank}",
                FontSize = 14,
                HorizontalAlignment = HorizontalAlignment.Center,
                Foreground = Brushes.White
            };
            stackPanel.Children.Add(rankBlock);

            // Add Number TextBlock
            TextBlock numberBlock = new TextBlock
            {
                Text = $"Number: {number}",
                FontSize = 14,
                HorizontalAlignment = HorizontalAlignment.Center,
                Foreground = Brushes.White
            };
            stackPanel.Children.Add(numberBlock);

            // Add Status TextBlock
            TextBlock statusBlock = new TextBlock
            {
                Text = $"Status: {status}",
                FontSize = 12,
                HorizontalAlignment = HorizontalAlignment.Center,
                Foreground = status.ToLower() == "active" ? Brushes.LightGreen : Brushes.Red
            };
            stackPanel.Children.Add(statusBlock);

            // Add the StackPanel to the Border
            memberCard.Child = stackPanel;

            memberCard.MouseEnter += (s, e) =>
            {
                memberCard.Background = new SolidColorBrush(Color.FromRgb(80, 80, 80)); // Darken the background on hover
            };

            memberCard.MouseLeave += (s, e) =>
            {
                memberCard.Background = new SolidColorBrush(Color.FromRgb(53, 53, 53)); // Reset the background color
            };

            memberCard.MouseLeftButtonDown += (s, e) =>
            {
                ShowMemberDetailsButton(number);
            };

            // Add the Border to the parent container (e.g., a WrapPanel or Grid)
            MemberListPanel.Children.Add(memberCard);
        }


        public async Task LoadMemberData()
        {
            var allMemberData = await _authService.GetAllMemberInfo();
            
            foreach (var member in allMemberData)
            {
                AddMemberCard(member.Name, member.Rank, member.ID, member.Status);
            }
        }
        private void ShowMemberDetailsButton(string num)
        {
            var dialogWin = new Dialog();
            dialogWin.MemberDetailDialog(num);
            dialogWin.ShowDialog();


        }
        private void AddNewMemberButton(object sender, RoutedEventArgs e)
        {
            var dialogWin = new Dialog();
            dialogWin.AddMemberDialog();
            dialogWin.ShowDialog();
        }
    }
}
