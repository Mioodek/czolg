using System;

namespace ConsoleGame
{
    class Program
    {
        //szerokość i wysokość ekranu
        const int width = 80;
        const int height = 20;

        //zmienne przechowujące położenie, kierunek i prędkość czołgu
        static int tankX = width / 2;
        static int tankY = height - 1;
        static int tankDir = 0; //kierunek: -1 = lewo, 0 = prosto, 1 = prawo
        static int tankSpeed = 1; //prędkość: 1 = wolno, 2 = szybko

        //położenie, kierunek i prędkość pocisku
        static int bulletX = -1;
        static int bulletY = -1;
        static int bulletDir = 0; //kierunek: -1 = lewo, 0 = prosto, 1 = prawo
        static int bulletSpeed = 2; //prędkość: 2 = wolno, 4 = szybko

        //zmienne przechowujące położenie celu
        static int targetX = 0;
        static int targetY = 0;
        static int targetWith = 3;

        static int scoreToWin;
        static int score = 0;

        //zmienna przechowująca stan gry
        static bool gameOver = false;

        static Random random = new Random();

        static void Main(string[] args)
        {
            Console.BackgroundColor = ConsoleColor.Black;
            Console.ForegroundColor = ConsoleColor.White;

            Console.Title = "Console Game";

           //Console.SetWindowSize(width, height);
           //Console.SetBufferSize(width, height);

            Console.CursorVisible = false;

            Console.WriteLine("Witaj w grze w Czołgi!");
            Console.WriteLine("Steruj czołgiem za pomocą klawiszy strzałek.");
            Console.WriteLine("Strzelaj pociskami za pomocą spacji.");
            Console.WriteLine("Celuj w cel, który pojawia się losowo na ekranie.");
            Console.WriteLine("Za każdy trafiony cel dostajesz 1 punkt.");
            Console.WriteLine("Wpisz do ilu punktów chcesz grać.");
            Console.WriteLine();
            bool parsingResult = int.TryParse(Console.ReadLine(), out scoreToWin);
            if (!parsingResult)
            {
                scoreToWin = 3;
            }
            Console.WriteLine();
            Console.WriteLine($"Gra potrwa do zdobycia {scoreToWin} punktów.");

            Console.ReadKey();

            GenerateTarget();

            while (!gameOver)
            {
                if (Console.KeyAvailable)
                {
                    ConsoleKeyInfo keyInfo = Console.ReadKey();

                    switch (keyInfo.Key)
                    {
                        case ConsoleKey.LeftArrow:
                            tankDir = -1;
                            break;
                        case ConsoleKey.RightArrow:
                            tankDir = 1;
                            break;
                        case ConsoleKey.Spacebar:
                            Shoot();
                            break;
                    }
                }

                UpdateTankPosition();

                UpdateBulletPosition();

                CheckCollision();

                CheckGameOver();

                DrawScreen();

                System.Threading.Thread.Sleep(100);
            }
            Console.WriteLine();
            Console.WriteLine("Koniec gry!");
            Console.WriteLine($"Twój wynik: {score}");
            Console.WriteLine("Naciśnij dowolny klawisz, aby zakończyć program.");

            Console.ReadKey();
            Console.ReadKey();
        }

        static void Shoot()
        {
            //sprawdzenie, czy pocisk jest już na ekranie
            if (bulletX == -1 && bulletY == -1)
            {
                //ustawienie pocisku na pozycji czołgu
                bulletX = tankX;
                bulletY = tankY;

                bulletDir = tankDir;

                bulletSpeed = tankSpeed * 2;
            }
        }

        static void UpdateTankPosition()
        {
            //sprawdzenie, czy czołg się porusza
            if (tankDir != 0)
            {
                int newX = tankX + tankDir * tankSpeed;

                //sprawdzenie, czy nowa pozycja jest w granicach ekranu
                if (newX > 0 && newX < width - 1)
                {

                    tankX = newX;
                }
            }
        }
        static void UpdateBulletPosition()
        {
            //sprawdzenie, czy pocisk jest na ekranie
            if (bulletX != -1 && bulletY != -1)
            {
                int newX = bulletX + bulletDir * bulletSpeed;

                int newY = bulletY - bulletSpeed + 1;

                //sprawdzenie, czy nowa pozycja jest w granicach ekranu
                if (newX > 0 && newX < width - 1 && newY > 0 && newY < height - 1)
                {
                    bulletX = newX;
                    bulletY = newY;
                }
                else
                {
                    //ustawienie pocisku na pozycję poza ekranem
                    bulletX = -1;
                    bulletY = -1;
                }
            }
        }
        static void CheckCollision()
        {
            //sprawdzenie, czy pocisk jest na ekranie
            if (bulletX != -1 && bulletY != -1)
            {
                //sprawdzenie, czy pocisk jest na tej samej pozycji co cel
                if (bulletX >= targetX && bulletX <= targetX + targetWith && bulletY == targetY)
                {
                    score++;

                    GenerateTarget();
                }
            }
        }
        static void CheckGameOver()
        {
            //sprawdzenie, czy pocisk jest na ekranie
            if (score >= scoreToWin)
            {
                Console.Clear();
                gameOver = true;
            }
        }
        static void DrawScreen()
        {
            Console.Clear();

            //narysowanie granicy ekranu
            for (int x = 0; x < width; x++)
            {
                Console.SetCursorPosition(x, 0);
                Console.Write('_');
                Console.SetCursorPosition(x, height - 1);
                Console.Write('_');
            }
            for (int y = 0; y < height; y++)
            {
                Console.SetCursorPosition(0, y);
                Console.Write('|');
                Console.SetCursorPosition(width - 1, y);
                Console.Write('|');
            }

            //narysowanie czołgu
            Console.SetCursorPosition(tankX, tankY);
            Console.Write('C');

            //narysowanie pocisku
            if (bulletX != -1 && bulletY != -1)
            {
                Console.SetCursorPosition(bulletX, bulletY);
                Console.Write('*');
            }

            Console.SetCursorPosition(targetX, targetY);
            Console.Write(new string('O', targetWith));

            Console.SetCursorPosition(1, height - 1);
            Console.WriteLine();
            Console.Write($"Wynik: {score}");
        }
        static void GenerateTarget()
        {
            targetX = random.Next(1, width - 1);

            targetY = random.Next(1, height - 2);
        }
    }
}
