using MVVMsmoke.Model;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace MVVMsmoke
{
    /// <summary>
    /// Логика взаимодействия для App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            WindowsBuilder.ShowMainWindow();
            InitDB();
            base.OnStartup(e);
        }
        void InitDB()
        {
            try
            {
                using(DbSettings db = new DbSettings())
                {
                    db.Database.Initialize(false);
                    if (db.User.Count() == 0 && db.Game.Count() == 0)
                    {
                        string imagesPath = @"Images\Games\";

                        Game GenshinImpact = new Game(1, "Genshin Impact", 1000, imagesPath + "genshin_impact.jpg");
                        db.Game.Add(GenshinImpact);

                        Game Stalker = new Game(1, "S.T.A.L.K.E.R", 3000, imagesPath + "stalker.jpg");
                        db.Game.Add(Stalker);

                        Game Valorant = new Game(1, "Valorant", 0, imagesPath + "valorant.jpg");
                        db.Game.Add(Stalker);

                        User IBA = new User(1, "IBA", "IBA@gmail.com", "zCATzCATz");
                        db.User.Add(IBA);
                        User VVS = new User(2, "VVS", "VVS@gmail.com", "z123z123z");
                        db.User.Add(VVS);

                        VVS.Games.Add(GenshinImpact);
                        VVS.Games.Add(Stalker);
                        IBA.Games.Add(GenshinImpact);
                        IBA.Games.Add(Valorant);
                        IBA.Games.Add(Stalker);

                        db.SaveChanges();
                    }
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show($"Ошибка иницализации DB: {ex.Message}");
            }
        }
    }
}
