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
using System.IO;
using Microsoft.Win32;
using System.Windows.Threading;
using System.Windows.Controls.Primitives;
using System.Collections.ObjectModel;
using System.Web.Script.Serialization;

namespace MediaPlayer
{
    /// <summary>
    /// Interaktionslogik für MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
       
        //Init
        #region
        public MainWindow()
        {
            InitializeComponent();
            DispatcherTimer timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromSeconds(1);
            timer.Tick += timer_Tick;
            timer.Start();
            list_Playlist.ItemsSource = playlist;
        }
        ObservableCollection<Media> playlist = new ObservableCollection<Media>();
        private bool mediaPlayerIsPlaying = false;
        private bool userIsDraggingSlider = false;
        string filename;
        string filename_Play;
        string json;
        
        #endregion

        //Progress Bar
        #region
        private void timer_Tick(object sender, EventArgs e)
        {
            if ((MediaPlayer.Source != null) && (MediaPlayer.NaturalDuration.HasTimeSpan) && (!userIsDraggingSlider))
            {
                sli_Progress.Minimum = 0;
                sli_Progress.Maximum = MediaPlayer.NaturalDuration.TimeSpan.TotalSeconds;
                sli_Progress.Value = MediaPlayer.Position.TotalSeconds;
            }
        }
        #endregion

        //Execute Y/N
        private void Open_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }

        //Play
        #region
        private void Play_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = (MediaPlayer != null) && (MediaPlayer.Source != null);
        }
        private void Play_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            MediaPlayer.Play();
            mediaPlayerIsPlaying = true;
        }
        #endregion

        //Pause
        #region
        private void Pause_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = mediaPlayerIsPlaying;
        }
        private void Pause_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            MediaPlayer.Pause();
        }
        #endregion

        //Stop
        #region
        private void Stop_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = mediaPlayerIsPlaying;
        }
        private void Stop_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            MediaPlayer.Stop();
            mediaPlayerIsPlaying = false;
        }
        #endregion

        //SliderControll
        #region
        private void sli_Progress_DragStarted(object sender, DragStartedEventArgs e)
        {
            userIsDraggingSlider = true;
        }
        private void sli_Progress_DragCompleted(object sender, DragCompletedEventArgs e)
        {
            userIsDraggingSlider = false;
            MediaPlayer.Position = TimeSpan.FromSeconds(sli_Progress.Value);
        }
        private void sli_Progress_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            lbl_ProgressStatus.Content = TimeSpan.FromSeconds(sli_Progress.Value).ToString(@"hh\:mm\:ss");
        }
        #endregion

        //Open File
        #region
        private void Open_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Media files (*.mp3;;*.mp4;*.wav)|*.mp3;*.mp4;*.wav";
            if (openFileDialog.ShowDialog() == true)
                MediaPlayer.Source = new Uri(openFileDialog.FileName);
            filename = openFileDialog.FileName;
            lbl_filename.Content = filename;
            //Initialize Media-data
            #region
            MediaPlayer.Play();
            MediaPlayer.Pause();
            #endregion
        }
        #endregion

        //Playlist Control
        #region

        //Add to Playlist
        private void PlaylistAdd(object sender, RoutedEventArgs e)
        {
            if(filename == null)
            {
                MessageBox.Show("No File selected.");
            }
            else if(textBox_name.Text == "")
            {
                MessageBox.Show("Please name your playlist entry.");
            }
            else
            {
                if (MediaPlayer.HasVideo == false)
                {
                    playlist.Add(new Audio(textBox_name.Text, filename, MediaPlayer.NaturalDuration, MediaPlayer.HasVideo));
                }
                else
                    playlist.Add(new Video(textBox_name.Text, filename, MediaPlayer.NaturalDuration, MediaPlayer.HasAudio, Convert.ToString(MediaPlayer.NaturalVideoWidth), Convert.ToString(MediaPlayer.NaturalVideoHeight)));
            }
                        
        }

            //Open From Playlist
        private void list_Playlist_SelectionChanged_1(object sender, SelectionChangedEventArgs e)
        {
            filename_Play = playlist[list_Playlist.SelectedIndex].path;
            MediaPlayer.Source = new Uri(filename_Play);
            MediaPlayer.Play();
            mediaPlayerIsPlaying = true;
            lbl_filename.Content = playlist[list_Playlist.SelectedIndex].path;
        }

        #endregion

        //To Json
        private void btn_save_Click(object sender, RoutedEventArgs e)
        {
            json = new JavaScriptSerializer().Serialize(playlist);
            System.IO.File.WriteAllText(@".\Playlist.json", json);
        }

    }
}
