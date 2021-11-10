using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Menu
{
    /// <summary>
    /// This class is responsible for managing the opening and closing of different menus.
    /// </summary>
    /// <remarks>
    /// Menus get stacked on top of each other on their own as long as you use the Open and Close functions!
    /// Also this class manages the use of the Escape Key in relevance to menus. But thats probably not what you're searching for. If you wanna change that, take a look at the Update function. Cheers.   
    /// </remarks>
    public class MenuController : MonoBehaviour
    {
        public  GameObject mainMenu;

        public List<GameObject> menuList = new List<GameObject>();

        /// <summary>
        /// The Update function manages the opening and closing of the main menu by pressing Esc.
        /// </summary>
        /// <remarks>
        /// This is NOT relevant for someone who wants to implement a new menu. This only used to make the Escape Key work for menus as it should. If you're searching for functionality for new menus take a look at the Open and Close functions below.
        /// </remarks>
        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                if (menuList.Any())
                {
                    Close(menuList.ElementAt(menuList.Count - 1));
                }
                else
                {
                    Open(mainMenu);
                }
            }
        }

        
        // Down here are the two main functions that may interest someone who wants to implement a new menu.
        
        /// <summary>
        /// The main function to open a new menu and append it to the open menus list.
        /// </summary>
        /// <remarks>
        /// Use this function if you wanna open up a new menu. As in you wanna have a button that opens up the next menu or something.
        /// </remarks>
        /// <param name="menu">The gameobject of the menu you want to open.</param>
        public void Open(GameObject menu)
        {
            if (menuList.Any())
            {
                menuList.ElementAt(menuList.Count - 1).SetActive(false);
            }
            
            menuList.Add(menu);
            menu.SetActive(true);
            Time.timeScale = 0;
        }

        /// <summary>
        /// The main function for closing an already open menu.
        /// </summary>
        /// <remarks>
        /// Use this function if you wanna close an already open menu. As in the button that closes a certain menu.
        /// </remarks>
        /// <param name="menu">The gameobject of the menu you want to close.</param>
        public void Close(GameObject menu)
        {
            menu.SetActive(false);
            menuList.Remove(menu);

            if (menuList.Any())
            {
                menuList.ElementAt(menuList.Count - 1).SetActive(true);
            }
            else
            {
                Time.timeScale = 1;
            }
        }
    }
}
