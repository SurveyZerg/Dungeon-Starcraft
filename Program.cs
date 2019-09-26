using System;
using System.Collections.Generic;
using System.IO;

namespace Dungeon_Starcraft
{
    class Program
    {
        static void Main(string[] args)
        {
            var Game = new Game();

            var MainHero = new Unit("MainHero", 100, 0, 2, 20);
            var Boss = new Unit("Boss", 100, 0, 5, 5);

            Game.BattleEventArgs.MainHero = MainHero;

            bool temp_menu = true;
            while (temp_menu)
            {
                Console.Write("1 - Начать новую игру\n2 - Загрузить игру\n3 - Выйти из игры\n");
                int menu = Convert.ToInt32(Console.ReadLine());
                if (menu == 1)
                {
                    Game.Start();

                    Game.Map[0] = MainHero;
                    MainHero.Location = 0;
                    Game.Map[Game.Map.Size - 1] = Boss;
                    Boss.Location = 9;

                    NewGame(Game, MainHero);

                    temp_menu = false;
                }
                else if (menu == 2)
                {
                    using (StreamReader sw = new StreamReader("saves/savelog.txt"))
                    {
                        if (sw.ReadLine() == null)
                        {
                            Console.WriteLine("Сохранений не существует!\nВыберите другой пункт меню");
                        }
                        else
                        {
                            temp_menu = false;
                        }
                    }
                    //Нужно вывести список сейвов и дать выбор пользователю
                    Game.Load("Save#1");
                }
                else if (menu == 3)
                {
                    Environment.Exit(0);
                }
            }

            Game.OnBattleEvent += Game_OnBattleEvent;

            while (!Game.End)
            {
                Console.WriteLine("-------------------------------------------");
                MainHero.ShowStatus(true);
                Console.WriteLine("-------------------------------------------");
                if (Game.Map[Game.Map.Size - 2] != MainHero)
                {
                    Console.WriteLine("Ваши действия:\n1 - Идти вперед\n8 - Сохраниться\n9 - Выйти из игры");
                    int turn = Convert.ToInt32(Console.ReadLine());
                    Console.WriteLine("-------------------------------------------");
                    if (turn == 1)
                    {
                        if (Game.Map[MainHero.Location + 1] is Loot)
                        {
                            Loot temp = (Loot)Game.Map[MainHero.Location + 1];
                            Console.Write($"Вы нашли золото!\n+{temp.Money} золота\n");
                            MainHero.Money += temp.Money;
                            Game.Map[temp.Location] = MainHero;
                            MainHero.Location++;
                        }
                        else if (Game.Map[MainHero.Location + 1] is Unit)
                        {
                            Unit temp = (Unit)Game.Map[MainHero.Location + 1];
                            Console.WriteLine($"Вы встретились с {temp.Name}!");
                            Console.WriteLine("-------------------------------------------");
                            Game.BattleEventArgs.Enemy = temp;
                            Game.Meeting(temp, Game.BattleEventArgs);

                            Game.Map[temp.Location] = MainHero;
                            MainHero.Location++;
                        }
                    }
                    else if (turn == 8)
                    {
                        Console.WriteLine("Выберите название для сохранения");
                        string savename = Console.ReadLine();
                        using (StreamWriter sw = new StreamWriter($"saves/savelog.txt", true))
                        {
                            sw.WriteLine($"{savename}.txt");
                        }
                        using (StreamWriter sw = new StreamWriter($"saves/{savename}.txt"))
                        {
                            sw.WriteLine($"Map Size - {Game.Map.Size}");
                            for (int i = 0; i < MainHero.Location; i++)
                            {
                                sw.WriteLine($"null: Location - {i}");
                            }
                            for (int i = MainHero.Location; i < Game.Map.Size; i++)
                            {
                                if (Game.Map[i] == MainHero)
                                {
                                    Unit temp = (Unit)Game.Map[i];
                                    sw.WriteLine($"MainHero: Name - {temp.Name}, HP - {temp.HP}, Mana - {temp.Mana}, Armor - {temp.Armor}, Damage - {temp.Damage}, Gold - {temp.Money}, Location - {temp.Location}");
                                }
                                else if (Game.Map[i] == Boss)
                                {
                                    Unit temp = (Unit)Game.Map[i];
                                    sw.WriteLine($"Boss: Name - {temp.Name}, HP - {temp.HP}, Mana - {temp.Mana}, Armor - {temp.Armor}, Damage - {temp.Damage}, Gold - {temp.Money}, Location - {temp.Location}");
                                }
                                else if (Game.Map[i] is Unit)
                                {
                                    Unit temp = (Unit)Game.Map[i];
                                    sw.WriteLine($"Unit: Name - {temp.Name}, HP - {temp.HP}, Mana - {temp.Mana}, Armor - {temp.Armor}, Damage - {temp.Damage}, Gold - {temp.Money}, Location - {temp.Location}");
                                }
                                else if (Game.Map[i] is Loot)
                                {
                                    Loot temp = (Loot)Game.Map[i];
                                    sw.WriteLine($"Loot: Gold - {temp.Money}, Location - {temp.Location}");
                                }
                            }
                        }
                    }
                    else if (turn == 9)
                    {
                        Environment.Exit(0);
                    }
                }
                else
                {
                    Console.Write("Впереди босс...\nВаши действия:\n1 - Идти вперед\n2 - Отдохнуть, восстановив здоровье, но дав боссу времени на приготовления\n");
                    int turn = Convert.ToInt32(Console.ReadLine());
                    if (turn == 2)
                    {
                        var rnd = new Random();
                        int temp = rnd.Next(5, 11);
                        Console.WriteLine($"Вы полностью восстановились, но {Boss.Name} увеличил свою броню на {temp}");
                        Boss.Armor += temp;
                        MainHero.HP = 100; //Изменить на максимум
                    }
                    Console.WriteLine($"Вы встретились с {Boss.Name}!");
                    Game.BattleEventArgs.Enemy = Boss;
                    Game.Meeting(Boss, Game.BattleEventArgs);

                    Game.Map[Boss.Location] = MainHero;
                    MainHero.Location++;
                    Game.End = true;
                }

            }
        }

        static void Game_OnBattleEvent(object sender, BattleEventArgs e)
        {
            if (sender is Game)
            {
                e.MainHero.ShowStatus();
                Console.WriteLine("-------------------------------------------");
                e.Enemy.ShowStatus();
                Console.WriteLine("-------------------------------------------");
                while (e.MainHero.HP > 1 && e.Enemy.HP > 1)
                {
                    Console.Write("Ваши действия:\n1 - Атака!\n");
                    int turn = Convert.ToInt32(Console.ReadLine());
                    Console.WriteLine("-------------------------------------------");
                    e.Enemy.HP -= (e.MainHero.Damage - e.Enemy.Armor);
                    Console.Write($"Вы атаковали {e.Enemy.Name} и нанесли {e.MainHero.Damage - e.Enemy.Armor} урона!\n");
                    Console.WriteLine("-------------------------------------------");
                    e.Enemy.ShowStatus();
                    Console.WriteLine("-------------------------------------------");
                    e.MainHero.HP -= (e.Enemy.Damage - e.MainHero.Armor);
                    Console.Write($"Вас атаковал {e.Enemy.Name} и нанес {e.Enemy.Damage - e.MainHero.Armor} урона!\n");
                    Console.WriteLine("-------------------------------------------");
                    e.MainHero.ShowStatus();
                }
                if (e.Enemy.HP < 1)
                {
                    Console.WriteLine("-------------------------------------------");
                    Console.WriteLine($"Победа! {e.Enemy.Name} пал...");
                }
                else if (e.MainHero.HP < 1)
                {
                    Console.WriteLine("-------------------------------------------");
                    Console.WriteLine($"Вы были убиты! {e.Enemy.Name} победил!");
                    Game.End = true;
                }
            }
        }

        static int FindObjectOnMap (Game game,object a)
        {
            int x;
            for (x = 0; x < game.Map.Size; x++)
            {
                if (game.Map[x] == a)
                    break;
            }
            return x;
        }
        static void NewGame(Game game, object mainHero)
        {
            List<Unit> unitsList = new List<Unit>();
            List<Loot> lootList = new List<Loot>();
            var rnd = new Random();
            bool loot;
            for (int i = FindObjectOnMap(game, mainHero) + 1; i < game.Map.Size - 1; i++)
            {
                loot = Convert.ToBoolean(rnd.Next(0, 2));
                if (loot)
                {
                    lootList.Add(new Loot(rnd.Next(0, 501)));
                    game.Map[i] = lootList[lootList.Count - 1];
                    lootList[lootList.Count - 1].Location = i;
                }
                else
                {
                    unitsList.Add(new Unit($"Enemy #{unitsList.Count + 1}", rnd.Next(5, 21), 0, rnd.Next(0, 3), rnd.Next(3, 8)));
                    game.Map[i] = unitsList[unitsList.Count - 1];
                    unitsList[unitsList.Count - 1].Location = i;
                }
            }
        }
    }
}
