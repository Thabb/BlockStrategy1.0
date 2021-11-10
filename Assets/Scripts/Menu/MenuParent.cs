using UnityEngine;

namespace Menu
{
    /// <summary>
    /// The parent class for any kind of menu.
    /// </summary>
    /// <remarks>
    /// This class is important for the functionality of opening up new menus. Use this class as superclass to every new Menu script that you want to implement.
    /// </remarks>
    public class MenuParent : MonoBehaviour
    {
        internal MenuController MenuController;

        /// <summary>
        /// Initializes the menucontroller for all the subclasses to use.
        /// </summary>
        private void Start()
        {
            MenuController = FindObjectOfType<MenuController>();
        }
    }
}