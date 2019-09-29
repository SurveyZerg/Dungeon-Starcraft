using System;
using System.Collections.Generic;
using System.IO;

namespace Dungeon_Starcraft
{
    class BattleEventArgs : EventArgs
    {
        public MainHero MainHero;
        public Unit Enemy;
    }
    class Game
    {
        public static bool End {get;set;}

        public BattleEventArgs BattleEventArgs = new BattleEventArgs();

        public event EventHandler<BattleEventArgs> OnBattleEvent;
        //public event EventHandler OnTalkingEvent;

        public Map Map = new Map();

        public void Start(ref MainHero mainHero,ref Boss boss)
        {
            var rnd = new Random();
            Map.Size = rnd.Next(10,20);

            Map[0] = mainHero;
            mainHero.Location = 0;
            Map[Map.Size - 1] = boss;
            boss.Location = Map.Size - 1;

            NewGame();
        }
        private void NewGame()
        {
            List<Unit> unitsList = new List<Unit>();
            List<Loot> lootList = new List<Loot>();

            var rnd = new Random();

            bool loot;

            for (int i = 1; i < Map.Size - 1; i++)
            {
                loot = Convert.ToBoolean(rnd.Next(0, 2));
                if (loot)
                {
                    bool chest = Convert.ToBoolean(rnd.Next(0, 2));
                    if(chest)
                        lootList.Add(new Chest(i));
                    else
                        lootList.Add(new AttackAura(i));
                    Map[i] = lootList[lootList.Count - 1];
                }
                else
                {
                    bool goblin = Convert.ToBoolean(rnd.Next(0, 2));
                    if (goblin)
                        unitsList.Add(new Goblin(i));
                    else
                        unitsList.Add(new Spider(i));
                    Map[i] = unitsList[unitsList.Count - 1];
                }
            }
        }

        public void Load(string savename,ref MainHero mainHero,ref Boss boss)
        {
            using (StreamReader sr = new StreamReader($"saves/{savename}"))
            {
                //Поиск размера
                string buff = sr.ReadLine();
                buff = buff.Remove(0, (buff.IndexOf("-") + 2));
                Map.Size = Convert.ToInt32(buff);
                //

                //Поиск всего, что лежит на карте
                for (int i = 0; !sr.EndOfStream; i++)
                {
                    buff = sr.ReadLine();
                    if (buff.Contains("null"))
                    {
                        Map[i] = null;
                    }
                    else
                    {
                        string typeOfObject_Main = buff.Substring(0, buff.IndexOf("/"));
                        buff = buff.Remove(0, (buff.IndexOf("/") + 1));
                        string typeOfObject_1 = "", typeOfObject_2 = ""; //Если создаются новые ветки наследования объектов, то сюда просто новое
                        if (typeOfObject_Main == "Unit")
                        {
                            typeOfObject_1 = buff.Substring(0, buff.IndexOf("/"));
                            buff = buff.Remove(0, (buff.IndexOf(":") + 2));
                        }
                        else if (typeOfObject_Main == "Loot")
                        {
                            typeOfObject_1 = buff.Substring(0, buff.IndexOf("/"));
                            if (typeOfObject_1 == "Chest")
                            {
                                buff = buff.Remove(0, (buff.IndexOf(":") + 2));
                            }
                            else if (typeOfObject_1 == "Aura")
                            {
                                buff = buff.Remove(0, (buff.IndexOf("/") + 1));
                                typeOfObject_2 = buff.Substring(0, buff.IndexOf("/"));
                                if (typeOfObject_2 == "AttackAura")
                                {
                                    buff = buff.Remove(0, (buff.IndexOf(":") + 2));
                                }
                            }
                        }
                        List<string> attributes = new List<string>();
                        for (int j = 0; buff != ""; j++)
                        {
                            buff = buff.Remove(0, (buff.IndexOf("-") + 2));
                            attributes.Add(buff.Substring(0, buff.IndexOf(",")));
                            buff = buff.Remove(0, (buff.IndexOf(",") + 2));
                        }


                        if (typeOfObject_Main == "Unit")
                        {
                            if (typeOfObject_1 == "MainHero")
                            {
                                MainHero temp = (MainHero)Map[i];
                                temp = new MainHero(attributes[0], Convert.ToInt32(attributes[1]), Convert.ToInt32(attributes[2]), Convert.ToInt32(attributes[3]), Convert.ToInt32(attributes[4]), Convert.ToInt32(attributes[5]), Convert.ToInt32(attributes[6]));
                                Map[i] = temp;
                                mainHero = temp;
                            }
                            else if (typeOfObject_1 == "Boss")
                            {
                                Boss temp = (Boss)Map[i];
                                temp = new Boss(attributes[0], Convert.ToInt32(attributes[1]), Convert.ToInt32(attributes[2]), Convert.ToInt32(attributes[3]), Convert.ToInt32(attributes[4]), Convert.ToInt32(attributes[5]));
                                Map[i] = temp;
                                boss = temp;
                            }
                            else
                            {
                                if (typeOfObject_1 == "Goblin")
                                {
                                    Goblin temp = (Goblin)Map[i];
                                    temp = new Goblin(attributes[0], Convert.ToInt32(attributes[1]), Convert.ToInt32(attributes[2]), Convert.ToInt32(attributes[3]), Convert.ToInt32(attributes[4]), Convert.ToInt32(attributes[5]), Convert.ToInt32(attributes[6]));
                                    Map[i] = temp;
                                }
                                else if (typeOfObject_1 == "Spider")
                                {
                                    Spider temp = (Spider)Map[i];
                                    temp = new Spider(attributes[0], Convert.ToInt32(attributes[1]), Convert.ToInt32(attributes[2]), Convert.ToInt32(attributes[3]), Convert.ToInt32(attributes[4]), Convert.ToInt32(attributes[5]));
                                    Map[i] = temp;
                                }
                            }
                        }
                        else if (typeOfObject_Main == "Loot")
                        {
                            if (typeOfObject_1 == "Chest")
                            {
                                Chest temp = (Chest)Map[i];
                                temp = new Chest(Convert.ToInt32(attributes[0]), Convert.ToInt32(attributes[1]));
                                Map[i] = temp;
                            }
                            else if (typeOfObject_1 == "Aura")
                            {
                                if (typeOfObject_2 == "AttackAura")
                                {
                                    AttackAura temp = (AttackAura)Map[i];
                                    temp = new AttackAura(Convert.ToInt32(attributes[0]), Convert.ToInt32(attributes[1]));
                                    Map[i] = temp;
                                }
                            }
                        }
                    }
                }
            }
        }
        public static string GetSaveName (int saveNumber)
        {
            using (StreamReader sr = new StreamReader("saves/savelog.txt"))
            {
                if (sr.ReadLine() == null)
                {
                    throw new ArgumentException("Сохранений не существует!\nНельзя позволять пользователю загружать игру, если он ни разу не сохранялся\n");
                }
                else
                {
                    sr.DiscardBufferedData();
                    sr.BaseStream.Seek(0, SeekOrigin.Begin);
                    for (int i = 1; i < saveNumber; i++)
                    {
                        sr.ReadLine();
                    }
                    string savename = sr.ReadLine();
                    return savename;
                }
            }
        }

        public void Save(string savename, MainHero mainHero, Boss boss)
        {
            using (StreamWriter sw = new StreamWriter($"saves/savelog.txt", true))
            {
                sw.WriteLine($"{savename}.txt");
            }
            using (StreamWriter sw = new StreamWriter($"saves/{savename}.txt"))
            {
                sw.WriteLine($"Map Size - {Map.Size}");
                for (int i = 0; i < mainHero.Location; i++)
                {
                    sw.WriteLine($"null: Location - {i}");
                }
                for (int i = mainHero.Location; i < Map.Size; i++)
                {
                    if (Map[i] is Unit)
                    {
                        if(Map[i] is MainHero)
                        {
                            MainHero temp = (MainHero)Map[i];
                            sw.WriteLine($"Unit/MainHero/: Name - {temp.Name}, HP - {temp.HP}, Mana - {temp.Mana}, Armor - {temp.Armor}, Damage - {temp.Damage}, Gold - {temp.Gold}, Location - {temp.Location}, ");
                        }
                        else if (Map[i] is Boss)
                        {
                            Boss temp = (Boss)Map[i];
                            sw.WriteLine($"Unit/Boss/: Name - {temp.Name}, HP - {temp.HP}, Mana - {temp.Mana}, Armor - {temp.Armor}, Damage - {temp.Damage}, Location - {temp.Location}, ");
                        }
                        else
                        {
                            //Все рядовые мобы
                            if (Map[i] is Goblin)
                            {
                                Goblin temp = (Goblin)Map[i];
                                sw.WriteLine($"Unit/Goblin/: Name - {temp.Name}, HP - {temp.HP}, Mana - {temp.Mana}, Armor - {temp.Armor}, Damage - {temp.Damage}, Gold - {temp.Gold}, Location - {temp.Location}, ");
                            }
                            else if (Map[i] is Spider)
                            {
                                Spider temp = (Spider)Map[i];
                                sw.WriteLine($"Unit/Spider/: Name - {temp.Name}, HP - {temp.HP}, Mana - {temp.Mana}, Armor - {temp.Armor}, Damage - {temp.Damage}, Location - {temp.Location}, ");
                            }
                        }
                    }
                    else if (Map[i] is Loot)
                    {
                        if (Map[i] is Chest)
                        {
                            Chest temp = (Chest)Map[i];
                            sw.WriteLine($"Loot/Chest/: Gold - {temp.Gold}, Location - {temp.Location}, ");
                        }
                        else if (Map[i] is Aura)
                        {
                            if (Map[i] is AttackAura)
                            {
                                AttackAura temp = (AttackAura)Map[i];
                                sw.WriteLine($"Loot/Aura/AttackAura/: AttackBuff - {temp.AttackBuff}, Location - {temp.Location}, ");
                            }
                        }
                    }
                }
            }
        }

        public void Meeting(Unit A,BattleEventArgs e)
        {
            //Здесь можно сделать условие на дружественный\нет (При дружеском токинг ивент)
            OnBattleEvent?.Invoke(this,e);
        }

        
        //public int FindObjectOnMap(object a)
        //{
        //    int x;
        //    for (x = 0; x < game.Map.Size; x++)
        //    {
        //        if (game.Map[x] == a)
        //            break;
        //    }
        //    return x;
        //}
    }
}
