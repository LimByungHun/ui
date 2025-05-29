using Wpf.Ui.Abstractions.Controls;
using 이거인가보오.ViewModels.Pages;

namespace 이거인가보오.Views.Pages
{
    public partial class DashboardPage : INavigableView<DashboardViewModel>
    {
        public DashboardViewModel ViewModel { get; }

        public DashboardPage(DashboardViewModel viewModel)
        {
            ViewModel = viewModel;
            DataContext = this;

            InitializeComponent();
            this.Unloaded += DashboardPage_Unloaded;
            ExerciseVideo.MediaOpened += ExerciseVideo_MediaOpened;
            ExerciseVideo.MediaEnded += ExerciseVideo_MediaEnded;            
        }

        private void ShowLoading()
        {
            LoadingOverlay.Visibility = Visibility.Visible;
            WebcamLoadingOverlay.Visibility = Visibility.Visible;
        }
        private void HideLoading()
        {
            LoadingOverlay.Visibility = Visibility.Collapsed;
            WebcamLoadingOverlay.Visibility = Visibility.Collapsed;
        }

        private void ExerciseVideo_MediaOpened(object sender, RoutedEventArgs e)
        {
            HideLoading();
        }

        private void ExerciseVideo_MediaEnded(object sender, RoutedEventArgs e)
        {
            HideLoading();
            ExerciseVideo.Position = TimeSpan.Zero;
            ExerciseVideo.Play();            
        }

        private void DashboardPage_Unloaded(object sender, RoutedEventArgs e)
        {
            ExerciseVideo.Stop();
            ExerciseVideo.Source = null;                                   

            WebcamControl.StopCamera();            
            WebcamControl.Visibility = Visibility.Collapsed;
        }

        private void Squat_Click(object sender, RoutedEventArgs e)
        {
            ShowLoading();
            ExerciseVideo.Source = null;
            ExerciseVideo.Source = new Uri("C:/Users/user/Desktop/영상자료/스쿼트.mp4", UriKind.Absolute);
            ExerciseVideo.Play();

            WebcamControl.Visibility = Visibility.Visible;
            WebcamControl.StartCamera();
            WebcamControl.StartPython();
        }

        private void Pushup_Click(object sender, RoutedEventArgs e)
        {
            ShowLoading();
            ExerciseVideo.Source = null;
            ExerciseVideo.Source = new Uri("C:/Users/user/Desktop/영상자료/푸쉬업.mp4", UriKind.Absolute);
            ExerciseVideo.Play();

            WebcamControl.Visibility = Visibility.Visible;
            WebcamControl.StartCamera();
            WebcamControl.StartPython();
        }

        private void Lunge_Click(object sender, RoutedEventArgs e)
        {
            ShowLoading();
            ExerciseVideo.Source = null;
            ExerciseVideo.Source = new Uri("C:/Users/user/Desktop/영상자료/런지.mp4", UriKind.Absolute);
            ExerciseVideo.Play();

            WebcamControl.Visibility = Visibility.Visible;
            WebcamControl.StartCamera();
            WebcamControl.StartPython();
        }

        private void Plank_Click(object sender, RoutedEventArgs e)
        {
            ShowLoading();
            ExerciseVideo.Source = null;
            ExerciseVideo.Source = new Uri("C:/Users/user/Desktop/영상자료/플랭크.mp4", UriKind.Absolute);
            ExerciseVideo.Play();

            WebcamControl.Visibility = Visibility.Visible;
            WebcamControl.StartCamera();
            WebcamControl.StartPython();
        }

        private void Close_Click(object sender, RoutedEventArgs e)
        {
            DashboardPage_Unloaded(sender, e);
        }
    }
}


