using System;
using Snake.Models;
using Windows.System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Shapes;

namespace Snake
{
    /// <summary>
    /// Hauptseite der Snake-Anwendung, die das Spielfeld und die Spielsteuerung enthält.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        private const int CellSize = 20;

        private readonly SnakeGame _game;
        private readonly DispatcherTimer _timer;

        private readonly SolidColorBrush _headBrush;
        private readonly SolidColorBrush _bodyBrush;
        private readonly SolidColorBrush _foodBrush;

        /// <summary>
        /// Initialisiert eine neue Instanz der <see cref="MainPage"/>-Klasse.
        /// </summary>
        public MainPage()
        {
            this.InitializeComponent();

            _game = new SnakeGame();

            _timer = new DispatcherTimer();
            _timer.Interval = TimeSpan.FromMilliseconds(150);
            _timer.Tick += Timer_Tick;

            _headBrush = new SolidColorBrush(Windows.UI.Color.FromArgb(255, 0, 200, 0));
            _bodyBrush = new SolidColorBrush(Windows.UI.Color.FromArgb(255, 0, 160, 0));
            _foodBrush = new SolidColorBrush(Windows.UI.Color.FromArgb(255, 220, 0, 0));

            Render();
        }

        /// <summary>
        /// Zeichnet das aktuelle Spielfeld auf die Canvas.
        /// </summary>
        private void Render()
        {
            GameArea.Children.Clear();

            Rectangle food = new Rectangle();
            food.Width = CellSize;
            food.Height = CellSize;
            food.Fill = _foodBrush;
            Canvas.SetLeft(food, _game.Food.X * CellSize);
            Canvas.SetTop(food, _game.Food.Y * CellSize);
            GameArea.Children.Add(food);

            for (int i = 0; i < _game.Snake.Count; i++)
            {
                Rectangle segment = new Rectangle();
                segment.Width = CellSize;
                segment.Height = CellSize;
                segment.Fill = (i == 0) ? _headBrush : _bodyBrush;
                Canvas.SetLeft(segment, _game.Snake[i].X * CellSize);
                Canvas.SetTop(segment, _game.Snake[i].Y * CellSize);
                GameArea.Children.Add(segment);
            }
        }

        /// <summary>
        /// Behandelt Tasteneingaben für die Spielsteuerung über Gamepad und Tastatur.
        /// </summary>
        private void Page_KeyDown(object sender, KeyRoutedEventArgs e)
        {
            switch (e.Key)
            {
                case VirtualKey.GamepadDPadUp:
                case VirtualKey.GamepadLeftThumbstickUp:
                case VirtualKey.Up:
                    _game.SetDirection(Direction.Up);
                    break;
                case VirtualKey.GamepadDPadDown:
                case VirtualKey.GamepadLeftThumbstickDown:
                case VirtualKey.Down:
                    _game.SetDirection(Direction.Down);
                    break;
                case VirtualKey.GamepadDPadLeft:
                case VirtualKey.GamepadLeftThumbstickLeft:
                case VirtualKey.Left:
                    _game.SetDirection(Direction.Left);
                    break;
                case VirtualKey.GamepadDPadRight:
                case VirtualKey.GamepadLeftThumbstickRight:
                case VirtualKey.Right:
                    _game.SetDirection(Direction.Right);
                    break;
            }
        }

        /// <summary>
        /// Wird bei jedem Timer-Tick aufgerufen und führt einen Spielschritt aus.
        /// </summary>
        private void Timer_Tick(object sender, object e)
        {
            _game.Update();
            Render();

            ScoreText.Text = "Punkte: " + _game.Score;

            if (_game.IsGameOver)
            {
                _timer.Stop();
                GameOverText.Visibility = Visibility.Visible;
            }
        }

        /// <summary>
        /// Behandelt den Klick auf den Neustart-Button und startet das Spiel neu.
        /// </summary>
        private void StartButton_Click(object sender, RoutedEventArgs e)
        {
            _game.Reset();
            GameOverText.Visibility = Visibility.Collapsed;
            ScoreText.Text = "Punkte: 0";
            Render();
            _timer.Start();
        }
    }
}
