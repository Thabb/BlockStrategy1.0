using UnityEngine;

namespace Menu
{
    /// <summary>
    /// The Main menu that is opened by pressing Esc.
    /// </summary>
    /// <remarks>
    /// The main menu is responsible for the exiting of the game as well as different kind of option menus.
    /// </remarks>
    public class MainMenu : MenuParent
    {
        /// <summary>
        /// Closes the menu and resumes the game.
        /// </summary>
        public void ResumeButton()
        {
            MenuController.Close(gameObject);
        }

        public void ExitButton()
        {
            Application.Quit();
        }
    }
}