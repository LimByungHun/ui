using System.Collections.ObjectModel;
using System.Windows.Media;
using Wpf.Ui.Abstractions.Controls;
using 이거인가보오.Models;

namespace 이거인가보오.ViewModels.Pages
{
    public partial class DataViewModel : ObservableObject, INavigationAware
    {
        #region 기존에 있던거
        private bool _isInitialized = false;

        [ObservableProperty]
        private IEnumerable<DataColor> _colors;

        public Task OnNavigatedToAsync()
        {
            if (!_isInitialized)
                InitializeViewModel();

            return Task.CompletedTask;
        }

        public Task OnNavigatedFromAsync() => Task.CompletedTask;

        private void InitializeViewModel()
        {
            var random = new Random();
            var colorCollection = new List<DataColor>();

            for (int i = 0; i < 8192; i++)
                colorCollection.Add(
                    new DataColor
                    {
                        Color = new SolidColorBrush(
                            Color.FromArgb(
                                (byte)200,
                                (byte)random.Next(0, 250),
                                (byte)random.Next(0, 250),
                                (byte)random.Next(0, 250)
                            )
                        )
                    }
                );

            Colors = colorCollection;

            _isInitialized = true;            
        }
        #endregion

        #region 필터링
        public ObservableCollection<string> ExerciseTypes { get; } = new() { "전체", "스쿼트", "푸쉬업", "런지", "플랭크" };

        [ObservableProperty]
        private string selectedExerciseType = "전체";

        // 전체 데이터와 필터링된 데이터
        public ObservableCollection<ExerciseData> AllData { get; set; } = new();
        public ObservableCollection<ExerciseData> TempData { get; set; } = new();

        // 필터 적용
        partial void OnSelectedExerciseTypeChanged(string value)
        {
            ApplyFilter();
        }

        public void ApplyFilter()
        {
            TempData.Clear();
            var filtered = selectedExerciseType == "전체"
                ? AllData
                : AllData.Where(x => x.ExerciseType.Name == selectedExerciseType);
            foreach (var item in filtered)
                TempData.Add(item);
        }

       public class ExerciseData
        {
            public ExerciseType ExerciseType { get; set; }   // 운동 종류
            public DateTime Date { get; set; }               // 날짜
            public int DurationInSeconds { get; set; }       // 운동 시간(초) 또는 횟수
            public double CaloriesBurned { get; set; }
        }

        public class ExerciseType
        {
            public string Name { get; set; }
        }
        #endregion
    }
}
