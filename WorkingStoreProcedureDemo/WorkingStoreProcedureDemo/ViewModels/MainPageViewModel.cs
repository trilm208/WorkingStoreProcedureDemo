using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Linq;
using DataAccess;

namespace WorkingStoreProcedureDemo.ViewModels
{
    public class MainPageViewModel : BindableBase, INavigationAware
    {
        private bool _isBusy = false;

        public bool IsBusy
        {
            get { return _isBusy; }
            set { SetProperty(ref _isBusy, value, () => RaisePropertyChanged(nameof(IsNotBusy))); }
        }

        public bool IsNotBusy
        {
            get { return !IsBusy; }
        }

        private string _title;
        public string Title
        {
            get { return _title; }
            set { SetProperty(ref _title, value); }
        }


        public DelegateCommand CallStoreProcedureCommand => new DelegateCommand(OnCallStoreProcedureCommandExecute).ObservesProperty(() => IsNotBusy);
       
        private  void OnCallStoreProcedureCommandExecute()
        {
            IsBusy = true;

            #region [CALL SQL STORE PROCEDURE]

            var client = new ClientServices();

            #endregion

            IsBusy = false;
        }

        public MainPageViewModel()
        {

        }

        public void OnNavigatedFrom(NavigationParameters parameters)
        {

        }

        public void OnNavigatingTo(NavigationParameters parameters)
        {

        }

        public void OnNavigatedTo(NavigationParameters parameters)
        {
            if (parameters.ContainsKey("title"))
                Title = (string)parameters["title"] + " and Prism";
        }
    }
}
