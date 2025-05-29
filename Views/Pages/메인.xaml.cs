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
            string videoPath = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Source", "스쿼트.mp4");
            ExerciseVideo.Source = new Uri(videoPath, UriKind.Absolute);
            ExerciseVideo.Play();
           
            Webcam(sender, e);
            Text(sender, e);
        }

        private void Pushup_Click(object sender, RoutedEventArgs e)
        {
            ShowLoading();
            // 시연 영상 자료화면
            ExerciseVideo.Source = null;
            string videoPath = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Source", "푸쉬업.mp4");
            ExerciseVideo.Source = new Uri(videoPath, UriKind.Absolute);
            ExerciseVideo.Play();


            //웹캠 송출
            Webcam(sender, e);
            // 텍스트 출력
            Text(sender, e);
        }

        private void Lunge_Click(object sender, RoutedEventArgs e)
        {
            ShowLoading();
            ExerciseVideo.Source = null;
            string videoPath = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Source", "런지3.mp4");
            ExerciseVideo.Source = new Uri(videoPath, UriKind.Absolute);
            ExerciseVideo.Play();
           
            Webcam(sender, e);
            Text(sender, e);
        }

        private void Plank_Click(object sender, RoutedEventArgs e)
        {
            ShowLoading();
            ExerciseVideo.Source = null;
            string videoPath = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Source", "플랭크3.mp4");
            ExerciseVideo.Source = new Uri(videoPath, UriKind.Absolute);
            ExerciseVideo.Play();

            Webcam(sender, e);            
            Text(sender, e);
        }

        //웹캠 송출
        private void Webcam(object sender, RoutedEventArgs e)
        {
            WebcamControl.Visibility = Visibility.Visible;
            WebcamControl.StartCamera();
            WebcamControl.StartPython();
        }

        // 텍스트 송출
        private void Text(object sender, RoutedEventArgs e)
        {
            ExerciseInfoPanel.Visibility = Visibility.Visible;
            ExerciseCountText.Text = $"운동 횟수: {ViewModel.Counter}";
            AccuracyText.Text = $"정확도: {ViewModel.Accuracy}%";
        }

        private void Close_Click(object sender, RoutedEventArgs e)
        {
            DashboardPage_Unloaded(sender, e);
        }
    }
}


