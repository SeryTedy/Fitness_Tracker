using Fitness_Tracker.Forms;
using Fitness_Tracker.Services;
using Fitness_Tracker.Models;
namespace Fitness_Tracker


{

    /// <summary>
    ///  The main entry point for the application. Shows the login window, then the main window,  
    ///  and loops back to the login window if the user chooses "Log out"
    /// </summary>

    internal static class Program
    {
        [STAThread]

        private static void Main()
        {

            ApplicationConfiguration.Initialize();

            var repository = new UserRepository();
            var auth = new AuthService(repository);

            while (true)
            {
                using var loginForm = new LoginForm(auth);
                if (loginForm.ShowDialog() != DialogResult.OK || loginForm.LoggedInUser == null)
                {
                    break;
                }

                using var mainForm = new MainForm(loginForm.LoggedInUser, auth);
                mainForm.ShowDialog();

                if (!mainForm.LogoutRequested)
                {
                    break;
                }


            }
        }
    }
}

        
       