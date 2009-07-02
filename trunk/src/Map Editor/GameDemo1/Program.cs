using System;
using System.Windows.Forms;

namespace GameDemo1
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main(string[] args)
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new frmMain());
            if (frmMain.EditMode == true)
                using (CreateTerrains game = new CreateTerrains()) { game.Run(); }
        }
    }
}

