using System.Media;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace SoundPlayDemo
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            _ = TestSounderPlay();
        }

        private async Task TestSounderPlay()
        {
            Console.WriteLine("play media by MediaElement");
            soundPlay.Play();

            await Task.Run(() =>
            {
                Thread.Sleep(3000);
                PlaySoundByMediaPlayer();
            });

            await Task.Run(() => {
                Thread.Sleep(3000);
                PlaySoundBySoundPlayer();
            });
          
        }

        private void PlaySoundByMediaPlayer()
        {
            Console.WriteLine("play media by MediaPlayer");
            MediaPlayer mediaPlayer = new MediaPlayer();
            mediaPlayer.Open(new Uri("trap-beat-2-with-stems-140bpm-159341.mp3", UriKind.RelativeOrAbsolute));

            mediaPlayer.Play();
        }

        private void PlaySoundBySoundPlayer()
        {
            Console.WriteLine("play media by MediaPlayer");
            SoundPlayer soundPlayer = new SoundPlayer("不如不见.wav");
            soundPlayer.Play();
        }
    }
}