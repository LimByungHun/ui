using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using Wpf.Ui.Abstractions.Controls;
using 이거인가보오.ViewModels.Pages;

namespace 이거인가보오.Views.Pages
{
    public partial class DataPage : INavigableView<DataViewModel>
    {
        public DataViewModel ViewModel { get; }

        public DataPage(DataViewModel viewModel)
        {
            ViewModel = viewModel;

            DataContext = ViewModel;
            InitializeComponent();                        



            ViewModel.ApplyFilter();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            
        }
    }
}
