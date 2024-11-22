using ActionTrack.Core;
using ActionTrack.MVVM.View;
using ActionTrack.MVVM.ViewModal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ActionTrack.MVVM.ViewModal
{
    class MainViewModal : ObservableObject
    {

        
        public RelayCommand AccountabilityCommand { get; set; }
        public RelayCommand HomeViewCommand { get; set; }
        public RelayCommand SettingsViewCommand { get; set; }
        public RelayCommand ReportWriterCommand { get; set; }
        public RelayCommand MemberListCommand { get; set; }

        public AccountabilityViewModal AcVM { get; set; }
        public HomeViewModal HomeVM { get; set; }
        public SettingsViewModal SettingsVM { get; set; }
        public ReportWriterViewModal RWVM { get; set; }
        public MemberListViewModal MLVM { get; set; }
        private object _currentView;

        public object CurrentView
        {
            get { return _currentView; }
            set
            {
                _currentView = value;
                OnProperyChanged();
            }
        }
        public MainViewModal() 
        {
            AcVM = new AccountabilityViewModal();
            HomeVM = new HomeViewModal();
            SettingsVM = new SettingsViewModal();
            RWVM = new ReportWriterViewModal();
            MLVM = new MemberListViewModal();
            CurrentView = AcVM;

            AccountabilityCommand = new RelayCommand(o =>
            {
                CurrentView = AcVM;
            });

            HomeViewCommand = new RelayCommand(o =>
            {
                CurrentView = HomeVM;
            });

            SettingsViewCommand = new RelayCommand(o =>
            {
                CurrentView = SettingsVM;
            });
            ReportWriterCommand = new RelayCommand(o =>
            {
                CurrentView = RWVM;
            });
            MemberListCommand = new RelayCommand(o =>
            {
                CurrentView = MLVM;
            });
        }

        
    }
}
