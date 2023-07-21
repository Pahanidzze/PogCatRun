using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SFML.Learning;
using SFML.Window;

namespace SFMLL
{
    class Program : Game
    {
        static float Px = 385;
        static float Py = 300;
        static int Psize = 56;
        static float Fx;
        static float Fy;
        static int Fsize = 32;
        static bool food = false;
        static bool lose = false;
        static int score = -1;

        static int direction = 0; //1 = UP, 2 = LEFT, 3 = DOWN, 4 = RIGHT
        static float speed = 200;

        static string BGT = LoadTexture("background.png");
        static string PlayerT = LoadTexture("Player.png");
        static string FoodT = LoadTexture("food.png");

        static string BGM = LoadMusic("bg_music.wav");
        static string CatCrashS = LoadSound("cat_crash_sound.wav");
        static string CatEatS = LoadSound("meow_sound.wav");

        static void Move()
        {
            if (GetKey(Keyboard.Key.W) == true && direction != 3) direction = 1;
            if (GetKey(Keyboard.Key.A) == true && direction != 4) direction = 2;
            if (GetKey(Keyboard.Key.S) == true && direction != 1) direction = 3;
            if (GetKey(Keyboard.Key.D) == true && direction != 2) direction = 4;

            if (direction == 1) Py -= speed * DeltaTime;
            if (direction == 2) Px -= speed * DeltaTime;
            if (direction == 3) Py += speed * DeltaTime;
            if (direction == 4) Px += speed * DeltaTime;

            if (Px < 0 || Px + Psize > 800 || Py < 165 || Py + Psize > 600)
            {
                lose = true;
                PlaySound(CatCrashS);
            }
        }

        static void Food()
        {
            if ((Px < Fx + Fsize && Px > Fx) || (Px + Psize < Fx + Fsize && Px + Psize > Fx))
            {
                if (Py < Fy + Fsize && Py + Psize > Fy)
                {
                    food = false;
                    speed = speed + 20;
                    PlaySound(CatEatS, 30);
                }
            }
            if ((Py < Fy + Fsize && Py > Fy) || (Py + Psize < Fy + Fsize && Py + Psize > Fy))
            {
                if (Px < Fx + Fsize && Px + Psize > Fx)
                {
                    food = false;
                    speed = speed + 20;
                    PlaySound(CatEatS, 30);
                }
            }
            if (food == false)
            {
                var rnd = new Random();
                Fx = rnd.Next(0, 800 - Fsize);
                Fy = rnd.Next(165, 600 - Fsize);
                food = true;
                score++;
            }
        }

        static void Main(string[] args)
        {
            InitWindow(800, 600, "Window");


            SetFont("comic.ttf");
            SetFillColor(50, 50, 50);
            PlayMusic(BGM, 70);

            while (true)
            {
                //Расчёт
                DispatchEvents();

                if (lose == false)
                {
                    Move();
                    Food();
                }
                else
                {
                    if (GetKey(Keyboard.Key.R) == true) //рестарт
                    {
                        lose = false;
                        Px = 385;
                        Py = 300;
                        direction = 0;
                        speed = 200;
                        score = 0;
                    }
                }

                //Очистка
                ClearWindow();

                //Отрисовка
                DrawSprite(BGT, 0, 0);
                DrawText(20, 10, "Количество съеденной еды: " + score.ToString());

                DrawSprite(FoodT, Fx, Fy);

                if (lose == true)
                {
                    DrawText(175, 250, "И зачем ты носишся по кухне?", 30);
                    DrawText(240, 300, "Нажмите \"R\", чтобы начать сначала!", 18);
                }
                if (direction == 1)
                {
                    DrawSprite(PlayerT, Px, Py, 64, 64, Psize, Psize);
                }
                else if (direction == 2)
                {
                    DrawSprite(PlayerT, Px, Py, 64, 0, Psize, Psize);
                }
                else if (direction == 4)
                {
                    DrawSprite(PlayerT, Px, Py, 0, 0, Psize, Psize);
                }
                else
                {
                    DrawSprite(PlayerT, Px, Py, 0, 64, Psize, Psize);
                }

                DisplayWindow();

                //Ожидание
                Delay(1);
            }
        }
    }
}
