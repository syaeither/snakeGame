using System;
using System.Threading;

namespace SnakeApp
{
    class Program
    {
        const int Width = 40;
        const int Height = 20;

        static int[] snakeX = new int[100];
        static int[] snakeY = new int[100];
        static int snakeLength = 5;

        static int foodX;
        static int foodY;

        // 0 - up, 1 - right, 2 - down, 3 - left
        static int direction = 1;

        static bool gameOver = false;
        static int score = 0;

        static Random rnd = new Random();

        static void Main(string[] args)
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8; // Для эмодзи

            Console.CursorVisible = false;
            ShowStartScreen();

            InitializeGame();

            while (!gameOver)
            {
                Draw();
                Input();
                Logic();
                Thread.Sleep(100);
            }

            GameOverScreen();
        }

        static void ShowStartScreen()
        {
            Console.Clear();
            Console.WriteLine("🐍 Добро пожаловать в игру Змейка! 🐍 (Автор: syaeither)");
            Console.WriteLine();
            Console.WriteLine("Для управления используйте клавиатуры W: ⬆, A: ⬅, S: ⬇, D: ⮕");
            Console.WriteLine("Нажмите Enter чтобы начать игру");
            Console.WriteLine("Нажмите Esc чтобы выйти");

            while (true)
            {
                var key = Console.ReadKey(true);
                if (key.Key == ConsoleKey.Enter) break;
                if (key.Key == ConsoleKey.Escape) Environment.Exit(0);
            }
        }

        static void InitializeGame()
        {
            int startX = Width / 2;
            int startY = Height / 2;

            for (int i = 0; i < snakeLength; i++)
            {
                snakeX[i] = startX - i;
                snakeY[i] = startY;
            }

            PlaceFood();
            direction = 1;
            score = 0;
            gameOver = false;
        }

        static void PlaceFood()
        {
            bool isOnSnake;
            do
            {
                isOnSnake = false;
                foodX = rnd.Next(0, Width);
                foodY = rnd.Next(0, Height);
                for (int i = 0; i < snakeLength; i++)
                {
                    if (snakeX[i] == foodX && snakeY[i] == foodY)
                    {
                        isOnSnake = true;
                        break;
                    }
                }
            } while (isOnSnake);
        }

        static void Draw()
        {
            Console.SetCursorPosition(0, 0);

            // Верхняя граница
            Console.WriteLine(string.Concat(System.Linq.Enumerable.Repeat("🌳", Width + 2)));

            for (int y = 0; y < Height; y++)
            {
                Console.Write("🌳"); // Левая граница

                for (int x = 0; x < Width; x++)
                {
                    bool printed = false;

                    for (int i = 0; i < snakeLength; i++)
                    {
                        if (snakeX[i] == x && snakeY[i] == y)
                        {
                            // Голова змейки - 🟢, тело - 🟩
                            Console.Write(i == 0 ? "🟢" : "🟩");
                            printed = true;
                            break;
                        }
                    }

                    if (!printed)
                    {
                        if (foodX == x && foodY == y)
                            Console.Write("🍎"); // Еда 
                        else
                            Console.Write("  "); // Пустое пространство (2 пробела для ровности)
                    }
                }

                Console.WriteLine("🌳"); // Правая граница
            }

            // Нижняя граница
            Console.WriteLine(string.Concat(System.Linq.Enumerable.Repeat("🌳", Width + 2)));

            Console.WriteLine($"Очки: {score}    Нажмите Esc для выхода");
        }

        static void Input()
        {
            if (!Console.KeyAvailable) return;

            ConsoleKeyInfo key = Console.ReadKey(true);

            switch (key.Key)
            {
                case ConsoleKey.W:
                    if (direction != 2) direction = 0;
                    break;
                case ConsoleKey.D:
                    if (direction != 3) direction = 1;
                    break;
                case ConsoleKey.S:
                    if (direction != 0) direction = 2;
                    break;
                case ConsoleKey.A:
                    if (direction != 1) direction = 3;
                    break;
                case ConsoleKey.Escape:
                    gameOver = true;
                    break;
            }
        }

        static void Logic()
        {
            // Двигаем тело змейки за головой
            for (int i = snakeLength - 1; i > 0; i--)
            {
                snakeX[i] = snakeX[i - 1];
                snakeY[i] = snakeY[i - 1];
            }

            // Двигаем голову
            switch (direction)
            {
                case 0: snakeY[0]--; break;
                case 1: snakeX[0]++; break;
                case 2: snakeY[0]++; break;
                case 3: snakeX[0]--; break;
            }

            // Проверяем выход за границы
            if (snakeX[0] < 0 || snakeX[0] >= Width || snakeY[0] < 0 || snakeY[0] >= Height)
            {
                gameOver = true;
            }

            // Проверяем столкновение с телом
            for (int i = 1; i < snakeLength; i++)
            {
                if (snakeX[0] == snakeX[i] && snakeY[0] == snakeY[i])
                {
                    gameOver = true;
                    break;
                }
            }

            // Если съела еду
            if (snakeX[0] == foodX && snakeY[0] == foodY)
            {
                score += 10;
                snakeLength++;
                PlaceFood();
            }
        }

        static void GameOverScreen()
        {
            Console.Clear();
            Console.WriteLine("💀 Игра окончена! 💀");
            Console.WriteLine($"Ваш счет: {score}");
            Console.WriteLine("Нажмите Enter чтобы выйти...");
            while (Console.ReadKey(true).Key != ConsoleKey.Enter) { }
        }
    }
}