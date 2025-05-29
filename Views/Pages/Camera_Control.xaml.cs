using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using Emgu.CV;
using Emgu.CV.Structure;
using Emgu.CV.CvEnum;

namespace 이거인가보오.Views.Pages
{
    /// <summary>
    /// Camera_Control.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class Camera_Control : UserControl
    {
        static readonly HttpClient client = new HttpClient();
        private VideoCapture? camera = null;

        private Mat? frame = null;
        // 카메라 켜져있는지 여부
        private bool isCamera = false;
        // 송신여부
        private bool isStart = true;

        public Camera_Control()
        {
            InitializeComponent();            
        }

        // 카메라 시작
        private async Task StartCameraLoopAsync()
        {
            using var capture = new VideoCapture(0, VideoCapture.API.DShow);

            int frameWidth = (int)capture.Get(CapProp.FrameWidth);
            int frameHeight = (int)capture.Get(CapProp.FrameHeight);

            frame = new Mat();

            isCamera = true;
            while (isCamera)
            {
                await PrintImg(capture);

                if (isStart)
                {   // JPEG로 인코딩
                    byte[] imageBytes = frame.ToImage<Bgr, byte>().ToJpegData();

                    await SendFrameAsync(imageBytes);
                    await Task.Delay(10); // 3fps 대기
                }
            }
        }

        // 웹캠 화면 -> 비트맵 -> 클라이언트 화면
        private async Task PrintImg(VideoCapture? capture)
        {
            if (capture == null || !isCamera) return;

            // 캡처, 이미지 변환 같은 무거운 작업은 별도 쓰레드에서 처리
            var bitmapSource = await Task.Run(() =>
            {
                capture.Read(frame);
                if (frame.IsEmpty) return null;

                using var bitmap = frame.ToBitmap();

                using var memory = new MemoryStream();
                bitmap.Save(memory, System.Drawing.Imaging.ImageFormat.Bmp);
                memory.Position = 0;

                var decoder = new BmpBitmapDecoder(memory, BitmapCreateOptions.PreservePixelFormat, BitmapCacheOption.OnLoad);
                return decoder.Frames[0];
            });

            await Dispatcher.InvokeAsync(() => imageBox.Source = bitmapSource);

            return;
        }

        #region Python 통신 여부 및 Python 송신 메서드
        /// <summary>
        /// Python 송신 시작
        /// </summary>
        public void StartPython()
        {
            isStart = true;
        }
        /// <summary>
        /// Python 송신 끝
        /// </summary>
        public void StopPython()
        {
            isStart = false;
        }
        // 이미지 python 에 전송
        private async Task SendFrameAsync(byte[] imageData)
        {
            using var content = new ByteArrayContent(imageData);
            content.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");

            _ = client.PostAsync("http://localhost:8000/byte/", content);
        }

        #endregion

        #region 카메라 시작 / 종료 메서드
        /// <summary>
        /// WebCam 시작
        /// </summary>
        public void StartCamera()
        {
            if (isCamera) return;

            // 시작
            Task.Run(() => StartCameraLoopAsync());
        }
        /// <summary>
        /// WebCam 끄기
        /// </summary>
        public void StopCamera()
        {
            isCamera = false;
            camera?.Dispose();
        }
        #endregion
    }
}
