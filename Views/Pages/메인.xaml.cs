using System.Threading.Tasks;
using Wpf.Ui.Abstractions.Controls;
using 이거인가보오.ViewModels.Pages;

namespace 이거인가보오.Views.Pages
{
    public partial class DashboardPage : INavigableView<DashboardViewModel>
    {
        public DashboardViewModel ViewModel { get; }

        private int _lastRestCounter = 0;

        public DashboardPage(DashboardViewModel viewModel)
        {
            ViewModel = viewModel;
            DataContext = this;

            InitializeComponent();
            this.Unloaded += DashboardPage_Unloaded;
            ExerciseVideo.MediaOpened += ExerciseVideo_MediaOpened;
            ExerciseVideo.MediaEnded += ExerciseVideo_MediaEnded;
            ViewModel.PropertyChanged += ViewModel_PropertyChanged;
        }

        private void ShowLoading()
        {
            GlobalLoadingOverlay.Visibility = Visibility.Visible;
        }
        private void HideLoading()
        {
            GlobalLoadingOverlay.Visibility = Visibility.Collapsed;
        }

        private async void ExerciseVideo_MediaOpened(object sender, RoutedEventArgs e)
        {
            HideLoading();
            reset();
            _ = Webcam();
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
            WebcamControl.StopPython();
            WebcamControl.Visibility = Visibility.Collapsed;

            ExerciseInfoPanel.Visibility = Visibility.Collapsed;
        }

        private void Squat_Click(object sender, RoutedEventArgs e)
        {
            ShowLoading();
            ExerciseVideo.Source = null;
            string videoPath = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Source", "스쿼트.mp4");
            ExerciseVideo.Source = new Uri(videoPath, UriKind.Absolute);
            ExerciseVideo.Play();
        }

        private void Pushup_Click(object sender, RoutedEventArgs e)
        {
            ShowLoading();
            // 시연 영상 자료화면
            ExerciseVideo.Source = null;
            string videoPath = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Source", "푸쉬업.mp4");
            ExerciseVideo.Source = new Uri(videoPath, UriKind.Absolute);
            ExerciseVideo.Play();

        }

        private void Lunge_Click(object sender, RoutedEventArgs e)
        {
            ShowLoading();
            ExerciseVideo.Source = null;
            string videoPath = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Source", "런지.mp4");
            ExerciseVideo.Source = new Uri(videoPath, UriKind.Absolute);
            ExerciseVideo.Play();

        }

        private void Stretching_Click(object sender, RoutedEventArgs e)
        {
            ShowLoading();
            ExerciseVideo.Source = null;
            string videoPath = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Source", "스트레칭.mp4");
            ExerciseVideo.Source = new Uri(videoPath, UriKind.Absolute);
            ExerciseVideo.Play();
        }

        //웹캠 송출
        private async Task Webcam()
        {
            WebcamControl.Visibility = Visibility.Visible;
            WebcamControl.StartCamera();
            WebcamControl.StartPython();

            ExerciseInfoPanel.Visibility = Visibility.Visible;

            //3초 딜레이
            for (int i = 3; i > 0; i--)
            {
                ExerciseCountText.Text = $"{i}초 뒤 운동 시작.";
                AccuracyText.Text = "";
                await Task.Delay(1000);
            }
            ExerciseCountText.Text = "운동 시작!";
            await Task.Delay(1000);

            ExerciseCountText.Text = $"운동 횟수: {ViewModel.Counter}";
            AccuracyText.Text = $"정확도: {ViewModel.Accuracy}%";
        }

        // 텍스트 송출
        private void Text()
        {
            ExerciseInfoPanel.Visibility = Visibility.Visible;
            ExerciseCountText.Text = $"운동 횟수: {ViewModel.Counter}";
            AccuracyText.Text = $"정확도: {ViewModel.Accuracy}%";
        }

        //초기화
        private void reset()
        {
            ViewModel.Counter = 0;
            ViewModel.Accuracy = 0.0;
        }

        // 휴식 시간
        private async void ViewModel_PropertyChanged(object? sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(ViewModel.Counter))
            {
                // 예: 10회마다 30초 쉬기
                if (ViewModel.Counter > 0 && ViewModel.Counter % 10 == 0 && _lastRestCounter != ViewModel.Counter)
                {
                    _lastRestCounter = ViewModel.Counter;

                    ExerciseVideo.Visibility = Visibility.Collapsed;
                    ExerciseVideo.Stop();
                    WebcamControl.Visibility = Visibility.Collapsed;
                    WebcamControl.StopCamera();
                    WebcamControl.StopPython();

                    ExerciseCountText.Text = "휴식 시간: 30초";
                    for (int i = 30; i > 0; i--)
                    {
                        ExerciseCountText.Text = $"휴식 시간: {i}초";
                        await Task.Delay(1000);
                    }
                    ExerciseVideo.Visibility = Visibility.Visible;
                    ExerciseVideo.Play();
                    WebcamControl.Visibility = Visibility.Visible;
                    WebcamControl.StartCamera();
                    WebcamControl.StartPython();

                    Text();
                }
            }
        }

        private void Close_Click(object sender, RoutedEventArgs e)
        {
            DashboardPage_Unloaded(sender, e);
        }
    }
}


