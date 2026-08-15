using System;
using System.Collections.Generic;

namespace Snake.Models
{
    /// <summary>
    /// Repräsentiert einen Punkt auf dem Spielfeld mit X- und Y-Koordinaten.
    /// </summary>
    public struct Point
    {
        public int X;
        public int Y;

        public Point(int x, int y)
        {
            X = x;
            Y = y;
        }
    }

    /// <summary>
    /// Definiert die Bewegungsrichtung der Schlange.
    /// </summary>
    public enum Direction
    {
        Up,
        Down,
        Left,
        Right
    }

    /// <summary>
    /// Enthält die Spiellogik für das Snake-Spiel.
    /// </summary>
    public sealed class SnakeGame
    {
        /// <summary>
        /// Die Breite des Spielfelds in Zellen.
        /// </summary>
        public const int GridWidth = 20;

        /// <summary>
        /// Die Höhe des Spielfelds in Zellen.
        /// </summary>
        public const int GridHeight = 20;

        private static readonly Random _random = new Random();

        /// <summary>
        /// Ruft den Körper der Schlange als Liste von Punkten ab.
        /// Der Kopf ist das erste Element.
        /// </summary>
        public List<Point> Snake { get; private set; }

        /// <summary>
        /// Ruft die aktuelle Bewegungsrichtung der Schlange ab.
        /// </summary>
        public Direction CurrentDirection { get; private set; }

        /// <summary>
        /// Ruft die Position des aktuellen Futters ab.
        /// </summary>
        public Point Food { get; private set; }

        /// <summary>
        /// Ruft die aktuelle Punktzahl ab.
        /// </summary>
        public int Score { get; private set; }

        /// <summary>
        /// Ruft einen Wert ab, der angibt, ob das Spiel beendet ist.
        /// </summary>
        public bool IsGameOver { get; private set; }

        /// <summary>
        /// Ruft einen Wert ab, der angibt, ob das Spiel pausiert ist.
        /// </summary>
        public bool IsPaused { get; set; }

        /// <summary>
        /// Initialisiert eine neue Instanz der <see cref="SnakeGame"/>-Klasse.
        /// </summary>
        public SnakeGame()
        {
            Snake = new List<Point>();
            Reset();
        }

        /// <summary>
        /// Setzt das Spiel in den Ausgangszustand zurück.
        /// </summary>
        public void Reset()
        {
            Snake.Clear();
            int centerX = GridWidth / 2;
            int centerY = GridHeight / 2;
            Snake.Add(new Point(centerX, centerY));
            Snake.Add(new Point(centerX - 1, centerY));
            Snake.Add(new Point(centerX - 2, centerY));

            CurrentDirection = Direction.Right;
            Score = 0;
            IsGameOver = false;
            IsPaused = false;
            SpawnFood();
        }

        /// <summary>
        /// Ändert die Bewegungsrichtung der Schlange, sofern dies nicht
        /// einer Umkehrung der aktuellen Richtung entspricht.
        /// </summary>
        /// <param name="newDirection">Die neue Bewegungsrichtung.</param>
        public void SetDirection(Direction newDirection)
        {
            if (IsGameOver || IsPaused)
                return;

            if (CurrentDirection == Direction.Up && newDirection == Direction.Down)
                return;
            if (CurrentDirection == Direction.Down && newDirection == Direction.Up)
                return;
            if (CurrentDirection == Direction.Left && newDirection == Direction.Right)
                return;
            if (CurrentDirection == Direction.Right && newDirection == Direction.Left)
                return;

            CurrentDirection = newDirection;
        }

        /// <summary>
        /// Führt einen Spielschritt aus: bewegt die Schlange, prüft Kollisionen
        /// und verarbeitet das Fressen von Nahrung.
        /// </summary>
        public void Update()
        {
            if (IsGameOver || IsPaused)
                return;

            Point head = Snake[0];
            Point newHead;

            switch (CurrentDirection)
            {
                case Direction.Up:
                    newHead = new Point(head.X, head.Y - 1);
                    break;
                case Direction.Down:
                    newHead = new Point(head.X, head.Y + 1);
                    break;
                case Direction.Left:
                    newHead = new Point(head.X - 1, head.Y);
                    break;
                case Direction.Right:
                    newHead = new Point(head.X + 1, head.Y);
                    break;
                default:
                    return;
            }

            if (newHead.X < 0 || newHead.X >= GridWidth ||
                newHead.Y < 0 || newHead.Y >= GridHeight)
            {
                IsGameOver = true;
                return;
            }

            for (int i = 0; i < Snake.Count; i++)
            {
                if (Snake[i].X == newHead.X && Snake[i].Y == newHead.Y)
                {
                    IsGameOver = true;
                    return;
                }
            }

            Snake.Insert(0, newHead);

            if (newHead.X == Food.X && newHead.Y == Food.Y)
            {
                Score++;
                SpawnFood();
            }
            else
            {
                Snake.RemoveAt(Snake.Count - 1);
            }
        }

        /// <summary>
        /// Platziert Nahrung an einer zufälligen Position, die nicht
        /// von der Schlange belegt ist.
        /// </summary>
        private void SpawnFood()
        {
            List<Point> available = new List<Point>();

            for (int x = 0; x < GridWidth; x++)
            {
                for (int y = 0; y < GridHeight; y++)
                {
                    bool occupied = false;
                    for (int i = 0; i < Snake.Count; i++)
                    {
                        if (Snake[i].X == x && Snake[i].Y == y)
                        {
                            occupied = true;
                            break;
                        }
                    }
                    if (!occupied)
                        available.Add(new Point(x, y));
                }
            }

            if (available.Count > 0)
            {
                Food = available[_random.Next(available.Count)];
            }
        }
    }
}
