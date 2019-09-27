using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;

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
            Game.OnBattleEvent += Game_OnBattleEvent;


            bool temp_menu = true;
            while (temp_menu)
            {
                Console.Write("1 - Начать новую игру\n2 - Загрузить игру\n3 - Выйти из игры\n");
                int menu = Convert.ToInt32(Console.ReadLine());
                Console.Clear();

                if (menu == 1)
                {
                    Game.Start(MainHero,Boss);
                    temp_menu = false;
                }
                else if (menu == 2)
                {
                    //Game.Load();
                    string savename = "";
                    using (StreamReader sr = new StreamReader("saves/savelog.txt"))
                    {
                        if (sr.ReadLine() == null)
                        {
                            Console.WriteLine("Сохранений не существует!\nВыберите другой пункт меню");
                        }
                        else
                        {
                            sr.DiscardBufferedData();
                            sr.BaseStream.Seek(0, SeekOrigin.Begin);
                            for(int i = 0;!sr.EndOfStream;i++)
                            {
                                Console.WriteLine($"{i+1} - {sr.ReadLine()}");
                            }
                            Console.Write("Выбери номер сохранения - ");
                            int saveNumber = Convert.ToInt32(Console.ReadLine());
                            sr.DiscardBufferedData();
                            sr.BaseStream.Seek(0, SeekOrigin.Begin);
                            for (int i = 1; i < saveNumber;i++)
                            {
                                sr.ReadLine();
                            }
                            savename = sr.ReadLine();
                            
                            temp_menu = false;
                        }
                    }
                    if (!temp_menu)
                    {
                        using (StreamReader sr = new StreamReader($"saves/{savename}"))
                        {
                            //Поиск размера
                            string temp = sr.ReadLine();
                            temp = temp.Remove(0, (temp.IndexOf("-") + 2));
                            Game.Map.Size = Convert.ToInt32(temp);
                            //

                            //Поиск всего, что лежит на карте
                            for (int i = 0; !sr.EndOfStream; i++)
                            {
                                temp = sr.ReadLine();
                                if (temp.Contains("null"))
                                {
                                    Game.Map[i] = null;
                                }
                                else
                                {
                                    string typeOfObject = temp.Substring(0, temp.IndexOf(":"));
                                    if (typeOfObject == "MainHero" || typeOfObject == "Boss" || typeOfObject == "Unit")
                                    {
                                        Unit a = (Unit)Game.Map[i];
                                        List<string> attributes = new List<string>();
                                        for (int j = 0; temp != "";j++)
                                        {
                                            temp = temp.Remove(0, (temp.IndexOf("-") + 2));
                                            attributes.Add(temp.Substring(0, temp.IndexOf(",")));
                                            temp = temp.Remove(0, (temp.IndexOf(",") + 2));
                                            Console.WriteLine(attributes[j]);
                                            Console.WriteLine(temp);
                                        }
                                        a = new Unit(attributes[0], Convert.ToInt32(attributes[1]), Convert.ToInt32(attributes[2]), Convert.ToInt32(attributes[3]), Convert.ToInt32(attributes[4]));
                                        //Спорно, а остается ли та же ссылка
                                        a.Money = Convert.ToInt32(attributes[5]);
                                        a.Location = Convert.ToInt32(attributes[6]);
                                        Game.Map[i] = a;
                                    }
                                    else if (typeOfObject == "Loot")
                                    {
                                        Loot a = (Loot)Game.Map[i];
                                        List<string> attributes = new List<string>();
                                        for (int j = 0; temp != ""; j++)
                                        {
                                            temp = temp.Remove(0, (temp.IndexOf("-") + 2));
                                            attributes.Add(temp.Substring(0, temp.IndexOf(",")));
                                            temp = temp.Remove(0, (temp.IndexOf(",") + 2));
                                            Console.WriteLine(attributes[j]);
                                            Console.WriteLine(temp);
                                        }
                                        a = new Loot(Convert.ToInt32(attributes[0]));
                                        //Спорно, а остается ли та же ссылка
                                        a.Location = Convert.ToInt32(attributes[1]);
                                        Game.Map[i] = a;
                                    }  
                                }
                            }
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

            while (!Game.End)
            {
                MainHero.ShowStatus(true);
                Console.WriteLine("-------------------------------------------");
                if (Game.Map[Game.Map.Size - 2] != MainHero)
                {
                    Console.WriteLine("Ваши действия:\n1 - Идти вперед\n8 - Сохраниться\n9 - Выйти из игры");
                    int turn = Convert.ToInt32(Console.ReadLine());
                    Console.Clear();
                    if (turn == 1)
                    {
                        if (Game.Map[MainHero.Location + 1] is Loot)
                        {
                            Loot temp = (Loot)Game.Map[MainHero.Location + 1];
                            Console.Write($"Вы нашли золото!\n+{temp.Money} золота\n");
                            Console.WriteLine("-------------------------------------------");
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
                                    sw.WriteLine($"MainHero: Name - {temp.Name}, HP - {temp.HP}, Mana - {temp.Mana}, Armor - {temp.Armor}, Damage - {temp.Damage}, Gold - {temp.Money}, Location - {temp.Location}, ");
                                }
                                else if (Game.Map[i] == Boss)
                                {
                                    Unit temp = (Unit)Game.Map[i];
                                    sw.WriteLine($"Boss: Name - {temp.Name}, HP - {temp.HP}, Mana - {temp.Mana}, Armor - {temp.Armor}, Damage - {temp.Damage}, Gold - {temp.Money}, Location - {temp.Location}, ");
                                }
                                else if (Game.Map[i] is Unit)
                                {
                                    Unit temp = (Unit)Game.Map[i];
                                    sw.WriteLine($"Unit: Name - {temp.Name}, HP - {temp.HP}, Mana - {temp.Mana}, Armor - {temp.Armor}, Damage - {temp.Damage}, Gold - {temp.Money}, Location - {temp.Location}, ");
                                }
                                else if (Game.Map[i] is Loot)
                                {
                                    Loot temp = (Loot)Game.Map[i];
                                    sw.WriteLine($"Loot: Gold - {temp.Money}, Location - {temp.Location}, ");
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
                    Console.Clear();
                    if (turn == 2)
                    {
                        var rnd = new Random();
                        int temp = rnd.Next(5, 11);
                        Console.WriteLine($"Вы полностью восстановились, но {Boss.Name} увеличил свою броню на {temp}");
                        Console.WriteLine("-------------------------------------------");
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
                while (e.MainHero.HP > 0 && e.Enemy.HP > 0)
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
                    Console.Clear();
                    Console.WriteLine($"Победа! {e.Enemy.Name} пал...");
                    Console.WriteLine("-------------------------------------------");
                }
                else if (e.MainHero.HP < 1)
                {
                    Console.WriteLine("-------------------------------------------");
                    Console.Clear();
                    Console.WriteLine($"Вы были убиты! {e.Enemy.Name} победил!");
                    Game.End = true;
                }
            }
        }
    }
}
