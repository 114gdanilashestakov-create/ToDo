using Desktop;
using System.Windows;

namespace Desktop
{
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);


            SQLitePCL.Batteries.Init();

            InitializeDatabase();
        }

        private void InitializeDatabase()
        {
            UserRepository.InitializeDatabase();
        }
    }
}