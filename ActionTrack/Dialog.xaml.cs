using ActionTrack.Core;
using ActionTrack.MVVM.View;
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
using System.Windows.Shapes;

namespace ActionTrack
{
    /// <summary>
    /// Interaction logic for Dialog.xaml
    /// </summary>
    public partial class Dialog : Window
    {
        private readonly AuthenticationService _authService;
        private string _seat;
        public Dialog()
        {
            InitializeComponent();
            _authService = new AuthenticationService();
        }

        public void AddMemberDialog()
        {
            this.Width = 400;
            this.Height = 520;
            AddMember_Dialog.Visibility = Visibility.Visible;
        }
        public async void MemberDetailDialog(string memberNum)
        {
            this.Width = 400;
            this.Height = 550;
            Console.WriteLine(memberNum);
            LoadingProgressBar.Visibility = Visibility.Visible;

            await InitializeMemberDetails(memberNum);
            await Task.Delay(2000);
            LoadingProgressBar.Visibility = Visibility.Collapsed;
            MemberInfo_Dialog.Visibility = Visibility.Visible;
        }

        private async Task InitializeMemberDetails(string id)
        {
            var memberInfo = await _authService.GetMemberInfoFromNum(id);

            MemberInfo_MemberNumTitle.Text = memberInfo.ID;
            MemberInfo_MemberNameTitle.Text = memberInfo.Name;

            MemberInfo_NumberText.Text = memberInfo.ID;
            MemberInfo_NameText.Text = memberInfo.Name;
            MemberInfo_RankText.Text = memberInfo.Rank;
            MemberInfo_BTText.Text = memberInfo.BloodType;
            MemberInfo_DOBText.Text = memberInfo.DOB.ToString();
            MemberInfo_DprtText.Text = memberInfo.Department;
            MemberInfo_EMText.Text = memberInfo.EmergencyContact;
            MemberInfo_StatusText.Text = memberInfo.Status;

        }

        public void MemberListACDialog(string seat)
        {
            _seat = seat;
            AddMemberCard("Eben Ulrich", "JR Firefighter", "30", "Active");
            MemberListAC_Dialog.Visibility = Visibility.Visible;
        }
        private void DialogExit_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }



        private async void AddMember_Click(object sender, RoutedEventArgs e)
        {
            var inputs = new Dictionary<TextBox, string>
    {
    { AddMember_NameInput, "#313233" },
    { AddMember_NumInput, "#313233" },
    { AddMember_RankInput, "#313233" },
    { AddMember_BTInput, "#313233" },
    { AddMember_DOBInput, "#313233" },
    { AddMember_EMCInput, "#313233" }
    };

            // Check for empty inputs and store them
            var emptyInputs = inputs.Where(kv => string.IsNullOrWhiteSpace(kv.Key.Text)).ToList();

            bool isDobValid = DateTime.TryParse(AddMember_DOBInput.Text, out DateTime parsedDob);

            if (emptyInputs.Any() || !isDobValid)
            {
                var brushConverter = new BrushConverter();
                foreach (var kv in emptyInputs)
                {
                    kv.Key.BorderBrush = Brushes.Red;
                }

                // Highlight DOB input if invalid
                if (!isDobValid)
                {
                    AddMember_DOBInput.BorderBrush = Brushes.Red;
                }

                await Task.Delay(300);

                foreach (var kv in emptyInputs)
                {
                    kv.Key.BorderBrush = (Brush)brushConverter.ConvertFrom(kv.Value);
                }

                if (!isDobValid)
                {
                    AddMember_DOBInput.BorderBrush = (Brush)brushConverter.ConvertFrom(inputs[AddMember_DOBInput]);
                }

                await Task.Delay(300);

                foreach (var kv in emptyInputs)
                {
                    kv.Key.BorderBrush = Brushes.Red;
                }

                if (!isDobValid)
                {
                    AddMember_DOBInput.BorderBrush = Brushes.Red;
                }

                await Task.Delay(500);

                foreach (var kv in emptyInputs)
                {
                    kv.Key.BorderBrush = (Brush)brushConverter.ConvertFrom(kv.Value);
                }

                if (!isDobValid)
                {
                    AddMember_DOBInput.BorderBrush = (Brush)brushConverter.ConvertFrom(inputs[AddMember_DOBInput]);
                }
            }
            else
            {

                AddMember_Dialog.Visibility = Visibility.Collapsed;
                LoadingProgressBar.Visibility = Visibility.Visible;

                var Name = AddMember_NameInput.Text;
                var Num = AddMember_NumInput.Text;
                var Rank = AddMember_RankInput.Text;
                var DOB = parsedDob.ToString("yyyy-MM-dd"); // Format the valid date
                var BT = AddMember_BTInput.Text;
                var EMC = AddMember_EMCInput.Text;

                // Proceed to add the member to the database
                await _authService.AddMemberAsync(Num, Name, "Oakmont Volunteer Fire Department", Rank, BT, parsedDob, EMC, "Active");
                await Task.Delay(5000);

                LoadingProgressBar.Visibility = Visibility.Collapsed;
                this.Close();
            }
        }

        private async void UpdateMember_Click(object sender, RoutedEventArgs e)
        {
            var inputs = new Dictionary<TextBox, string>
            {
            { MemberInfo_NumberText, "#313233" },
            { MemberInfo_NameText, "#313233" },
            { MemberInfo_RankText, "#313233" },
            { MemberInfo_BTText, "#313233" },
            { MemberInfo_DOBText, "#313233" },
            { MemberInfo_EMText, "#313233" },
            { MemberInfo_DprtText, "#313233" },
            { MemberInfo_StatusText, "#313233" }
            };

            // Check for empty inputs and store them
            var emptyInputs = inputs.Where(kv => string.IsNullOrWhiteSpace(kv.Key.Text)).ToList();

            bool isDobValid = DateTime.TryParse(MemberInfo_DOBText.Text, out DateTime parsedDob);

            if (emptyInputs.Any() || !isDobValid)
            {
                var brushConverter = new BrushConverter();
                foreach (var kv in emptyInputs)
                {
                    kv.Key.BorderBrush = Brushes.Red;
                }

                // Highlight DOB input if invalid
                if (!isDobValid)
                {
                    MemberInfo_DOBText.BorderBrush = Brushes.Red;
                }

                await Task.Delay(300);

                foreach (var kv in emptyInputs)
                {
                    kv.Key.BorderBrush = (Brush)brushConverter.ConvertFrom(kv.Value);
                }

                if (!isDobValid)
                {
                    MemberInfo_DOBText.BorderBrush = (Brush)brushConverter.ConvertFrom(inputs[MemberInfo_DOBText]);
                }

                await Task.Delay(300);

                foreach (var kv in emptyInputs)
                {
                    kv.Key.BorderBrush = Brushes.Red;
                }

                if (!isDobValid)
                {
                    MemberInfo_DOBText.BorderBrush = Brushes.Red;
                }

                await Task.Delay(500);

                foreach (var kv in emptyInputs)
                {
                    kv.Key.BorderBrush = (Brush)brushConverter.ConvertFrom(kv.Value);
                }

                if (!isDobValid)
                {
                    MemberInfo_DOBText.BorderBrush = (Brush)brushConverter.ConvertFrom(inputs[MemberInfo_DOBText]);
                }
            }
            else
            {

                MemberInfo_Dialog.Visibility = Visibility.Collapsed;
                LoadingProgressBar.Visibility = Visibility.Visible;

                var Name = MemberInfo_NameText.Text;
                var newNum = MemberInfo_NumberText.Text;
                var Rank = MemberInfo_RankText.Text;
                var DOB = parsedDob.ToString("yyyy-MM-dd"); // Format the valid date
                var BT = MemberInfo_BTText.Text;
                var EMC = MemberInfo_EMText.Text;
                var Dept = MemberInfo_DprtText.Text;
                var Status = MemberInfo_StatusText.Text;

                // Proceed to add the member to the database
                await _authService.UpdateMemberAsync(MemberInfo_MemberNumTitle.Text, newNum, Name, Dept, Rank, BT, parsedDob, EMC, Status);
                await Task.Delay(5000);

                LoadingProgressBar.Visibility = Visibility.Collapsed;
                this.Close();
            }
        }

        private async void RemoveMember_Click(object sender, RoutedEventArgs e)
        {
            var confirmWin = new Confirmation();
            this.Hide();
            bool? result = confirmWin.ShowDialog();

            if (result == true)
            {
                Console.WriteLine("Confirmed Remove");
                this.Show();
                string memberId = MemberInfo_MemberNumTitle.Text;
                MemberInfo_Dialog.Visibility = Visibility.Collapsed;
                LoadingProgressBar.Visibility = Visibility.Visible;

                await _authService.RemoveMemberData(memberId);
                await Task.Delay(4000);
                LoadingProgressBar.Visibility = Visibility.Collapsed;
                this.Close();

            }
            else
            {
                this.Show();
            }
        }


        private void AddMemberCard(string name, string rank, string number, string status)
        {
            // Create the Border for the card
            Border memberCard = new Border
            {
                Tag = number,
                Margin = new Thickness(10),
                Padding = new Thickness(10),
                Width = 210,
                Height = 90,
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
                FontSize = 15,
                HorizontalAlignment = HorizontalAlignment.Center,
                Foreground = Brushes.White
            };
            stackPanel.Children.Add(nameBlock);

            // Add Rank TextBlock
            TextBlock rankBlock = new TextBlock
            {
                Text = $"Rank: {rank}",
                FontSize = 13,
                HorizontalAlignment = HorizontalAlignment.Center,
                Foreground = Brushes.White
            };
            stackPanel.Children.Add(rankBlock);

            // Add Number TextBlock
            TextBlock numberBlock = new TextBlock
            {
                Text = $"Number: {number}",
                FontSize = 12,
                HorizontalAlignment = HorizontalAlignment.Center,
                Foreground = Brushes.White
            };
            stackPanel.Children.Add(numberBlock);

            // Add Status TextBlock
            TextBlock statusBlock = new TextBlock
            {
                Text = $"Status: {status}",
                FontSize = 10,
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
                                            MemberListMember_Click(s, e);
                                        };

            // Add the Border to the parent container (e.g., a WrapPanel or Grid)
            MemberListPanel.Children.Add(memberCard);
        }

        private void MemberListMember_Click(object sender, EventArgs e)
        {
            // Cast sender to Border
            Border clickedMember = sender as Border;
            if (clickedMember != null)
            {
                // Retrieve the member number from the Border's Tag
                string memberNumber = clickedMember.Tag?.ToString();

                var accountabilityView = LogicalTreeHelper.FindLogicalNode(Application.Current.MainWindow, "AccountablityViewControl") as AccountablityView;

                if (accountabilityView != null)
                {
                    accountabilityView.SetMemberToSeat(memberNumber, _seat);
                }


            }
        }


        public async Task InitializeMemberList()
        {
            var allMemberData = await _authService.GetAllMemberInfo();

            foreach (var member in allMemberData)
            {
                AddMemberCard(member.Name, member.Rank, member.ID, member.Status);
            }
        }


    }
}
