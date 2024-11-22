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
    /// Interaction logic for HomeView.xaml
    /// </summary>
    public partial class HomeView : UserControl
    {
        public HomeView()
        {
            InitializeComponent();
        }
        private void AddInfoText(string str)
        {
            TextBlock newTB = new TextBlock
            {
                Text = str,
                Foreground = Brushes.White,
            };


            MemberInfo.Children.Add(newTB);
        }
        private void SetMemberInfoView(string memberInfo)
        {
            MemberInfo.Children.Clear();

            foreach (var str in memberInfo) {
                AddInfoText(str.ToString());
            }
        }
    }
}
