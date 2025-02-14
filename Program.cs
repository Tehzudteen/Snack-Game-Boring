using System;
using System.Collections.Generic;
using System.Threading;

class Program
{
    static void Main()
    {
        Console.OutputEncoding = System.Text.Encoding.UTF8;
        Console.CursorVisible = false;
        while (true)
        {
            PlayGame();
            Console.Clear();

            string[] gameOverText =
            {
                "  ██████   █████  ███    ███ ███████   ██████  ██    ██ ███████ ██████   ",
                " ██       ██   ██ ████  ████ ██        ██  ██  ██    ██ ██      ██   ██  ",
                " ██   ███ ███████ ██ ████ ██ █████     ██  ██  ██    ██ █████   ██████   ",
                " ██    ██ ██   ██ ██  ██  ██ ██        ██  ██  ██    ██ ██      ██   ██  ",
                "  ██████  ██   ██ ██      ██ ███████   ██████    ██████  ███████ ██   ██  "
            };

            int startX = Math.Max(0, (Console.WindowWidth - gameOverText[0].Length) / 2);
            int startY = Math.Max(0, (Console.WindowHeight - gameOverText.Length) / 2);

            Console.ForegroundColor = ConsoleColor.Red;
            foreach (var line in gameOverText)
            {
                Console.SetCursorPosition(startX, startY++);
                Console.Write(line);
            }
            Console.ResetColor();

            Console.SetCursorPosition(Math.Max(0, Console.WindowWidth / 2 - 10), Math.Max(0, Console.WindowHeight / 2 + 3));
            Console.Write("Press R to Restart or Q to Quit");

            while (true)
            {
                var key = Console.ReadKey(true).Key;
                if (key == ConsoleKey.R)
                    break;
                if (key == ConsoleKey.Q)
                    return;
            }
        }
    }

    static void PlayGame()
    {
        int width = Console.WindowWidth;
        int height = Console.WindowHeight;
        int snakeX = width / 2, snakeY = height / 2;
        int foodX = new Random().Next(1, width - 2), foodY = new Random().Next(1, height - 2);
        int dx = 1, dy = 0, level = 1, score = 0;
        List<(int, int)> snake = new List<(int, int)> { (snakeX, snakeY) };
        List<(int, int)> enemies = new List<(int, int)>();
        ConsoleKeyInfo key;

        void DrawBorders()
        {
            Console.ForegroundColor = ConsoleColor.Blue;
            for (int i = 0; i < width; i++)
            {
                Console.SetCursorPosition(i, 0);
                Console.Write("█");
                Console.SetCursorPosition(i, height - 1);
                Console.Write("█");
            }
            for (int i = 0; i < height; i++)
            {
                Console.SetCursorPosition(0, i);
                Console.Write("█");
                Console.SetCursorPosition(width - 1, i);
                Console.Write("█");
            }
            Console.ResetColor();
        }

        while (true)
        {
            Console.Clear();
            DrawBorders();

            Console.SetCursorPosition(foodX, foodY);
            Console.ForegroundColor = ConsoleColor.Red;
            Console.Write("♥");
            Console.ResetColor();

            foreach (var enemy in enemies)
            {
                Console.SetCursorPosition(enemy.Item1, enemy.Item2);
                Console.ForegroundColor = ConsoleColor.Magenta;
                Console.Write("☠");
                Console.ResetColor();
            }

            foreach (var part in snake)
            {
                Console.SetCursorPosition(part.Item1, part.Item2);
                Console.ForegroundColor = ConsoleColor.Green;
                Console.Write("■");
                Console.ResetColor();
            }

            Console.SetCursorPosition(2, 0);
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.Write($"Score: {score}  Level: {level}");
            Console.ResetColor();

            if (Console.KeyAvailable)
            {
                key = Console.ReadKey(true);
                switch (key.Key)
                {
                    case ConsoleKey.UpArrow:
                        if (dy == 0) { dx = 0; dy = -1; }
                        break;
                    case ConsoleKey.DownArrow:
                        if (dy == 0) { dx = 0; dy = 1; }
                        break;
                    case ConsoleKey.LeftArrow:
                        if (dx == 0) { dx = -1; dy = 0; }
                        break;
                    case ConsoleKey.RightArrow:
                        if (dx == 0) { dx = 1; dy = 0; }
                        break;
                    case ConsoleKey.Q:
                        return;
                }
            }

            snakeX += dx;
            snakeY += dy;

            if (snake.Contains((snakeX, snakeY)))
                break;

            if (snakeX <= 0 || snakeX >= width - 1 || snakeY <= 0 || snakeY >= height - 1)
                break;

            for (int i = 0; i < enemies.Count; i++)
            {
                if (snakeX == enemies[i].Item1 && snakeY == enemies[i].Item2)
                {
                    enemies.RemoveAt(i);
                    score -= 5;
                    if (snake.Count > 1)
                        snake.RemoveAt(0);
                    break;
                }
            }

            if (snakeX == foodX && snakeY == foodY)
            {
                foodX = new Random().Next(1, width - 2);
                foodY = new Random().Next(1, height - 2);
                enemies.Add((new Random().Next(1, width - 2), new Random().Next(1, height - 2)));
                score += 10;
                if (score % 50 == 0) level++;
            }
            else
            {
                snake.RemoveAt(0);
            }

            snake.Add((snakeX, snakeY));

            Thread.Sleep(100 - (level * 5));
        }
    }
}