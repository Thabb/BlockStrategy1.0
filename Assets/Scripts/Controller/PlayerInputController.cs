using System.Collections.Generic;
using Health;
using UnityEngine;
using UnityEngine.UI;

namespace Controller
{
    public class PlayerInputController : MonoBehaviour
    {

        public List<Unit> controlledUnits = new List<Unit>();

        public Image dragBox;
        private Vector3 _leftMouseDownPosition;

        // list of units that exist at that moment
        private List<Unit> _activeUnits = new List<Unit>();

        private Camera _mainCam;

        /// <summary>
        /// Initializes a reference to the main camera for further use.
        /// </summary>
        void Start()
        {
            _mainCam = Camera.main;
        }

        /// <summary>
        /// Calls the main method.
        /// </summary>
        void Update()
        {
            MouseClicks();
        }

        /// <summary>
        /// The main method of the PlayerInputController. It handles left-click, right-click and drag select.
        /// </summary>
        /// <remarks>A single left mouse click marks a single unit.
        /// Pressing down the left mouse marks a corner for the drag box, releasing it marks the other. While the mouse
        /// is being pressed the dragbox changes its visual dimension. This has no effect on the select functionality.
        /// The controller has a list of all currently existing player units and then searches that list for units inside the drag box.
        /// With a right click the controlled units are send somewhere. The controller differentiates between a right-click
        /// on an enemy, an own unit, the enemy base, the own base and a click on open ground and acts accordingly.
        /// There are plenty of comments inside that code that mark the beginnings of the different sections.</remarks>
        private void MouseClicks()
        {
            // left mouse click
            if (Input.GetMouseButtonDown(0))
            {
                // set corner 1 for the dragBox
                _leftMouseDownPosition = Input.mousePosition;
                dragBox.gameObject.SetActive(true);
            }

            if (Input.GetMouseButton(0))
            {
                dragBox.rectTransform.sizeDelta =
                    new Vector2(Mathf.Abs(_leftMouseDownPosition.x - Input.mousePosition.x),
                        Mathf.Abs(_leftMouseDownPosition.y - Input.mousePosition.y));
                dragBox.rectTransform.position = new Vector2((_leftMouseDownPosition.x + Input.mousePosition.x) / 2,
                    (_leftMouseDownPosition.y + Input.mousePosition.y) / 2);
            }

            if (Input.GetMouseButtonUp(0))
            {
                // under this comment is the stuff needed for a one-click
                Ray ray = _mainCam.ScreenPointToRay(Input.mousePosition);

                if (Physics.Raycast(ray, out RaycastHit hit, 200))
                {
                    if (controlledUnits.Count != 0)
                    {
                        foreach (Unit unit in controlledUnits)
                        {
                            if (unit) unit.healthBar.gameObject.SetActive(false);
                        }

                        controlledUnits.Clear();
                    }

                    Unit hitUnit = hit.transform.gameObject.GetComponent<Unit>();
                    if (hit.transform.gameObject.CompareTag("Unit") && hitUnit.team == 1)
                    {
                        controlledUnits.Add(hitUnit);
                        controlledUnits[0].healthBar.gameObject.SetActive(true);
                    }
                }

                // drag select stuff
                // deactivate dragBox
                dragBox.gameObject.SetActive(false);

                Vector2 min = dragBox.rectTransform.anchoredPosition - (dragBox.rectTransform.sizeDelta / 2);
                Vector2 max = dragBox.rectTransform.anchoredPosition + (dragBox.rectTransform.sizeDelta / 2);

                foreach (Unit unit in _activeUnits)
                {
                    Vector3 unitPos = _mainCam.WorldToScreenPoint(unit.transform.position);

                    if (unitPos.x >= min.x && unitPos.x <= max.x && unitPos.y >= min.y && unitPos.y <= max.y)
                    {
                        controlledUnits.Add(unit);
                        unit.healthBar.gameObject.SetActive(true);
                    }
                }
            }

            // right mouse click
            else if (Input.GetMouseButtonDown(1))
            {
                Ray ray = _mainCam.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;

                if (Physics.Raycast(ray, out hit, 200))
                {
                    // transcribe the x and z value of the hit in a new Vector
                    Vector3 destination = hit.point;
                    destination.y = 1;

                    HealthEntity entity = hit.transform.GetComponent<HealthEntity>();

                    // case: clicked on open ground
                    if (hit.transform.gameObject.CompareTag("Ground") && controlledUnits.Count != 0)
                    {
                        foreach (Unit unit in controlledUnits)
                        {
                            unit.destPoint = destination;

                            // get unit out of combat mode
                            unit.isInCombat = false;
                            unit.enemy = null;
                        }
                    }

                    // TODO: case: clicked on friendly unit or structure


                    // case: clicked on enemy unit or structure
                    else if ((hit.transform.gameObject.CompareTag("Unit") ||
                              hit.transform.gameObject.CompareTag("Structure")) &&
                             entity.team != 1 && controlledUnits.Count != 0)
                    {
                        foreach (Unit unit in controlledUnits)
                        {
                            unit.isInCombat = true;
                            unit.enemy = entity;
                        }
                    }
                    // case: clicked on an own unit or structure
                    else if ((hit.transform.gameObject.CompareTag("Unit") ||
                              hit.transform.gameObject.CompareTag("Structure")) &&
                             entity.team == 1 && controlledUnits.Count != 0)
                    {
                        foreach (Unit unit in controlledUnits)
                        {
                            unit.destPoint = destination;
                            
                            // get unit out of combat mode
                            unit.isInCombat = false;
                            unit.enemy = null;
                        }
                    }
                }
            }

        }

        /// <summary>
        /// Adds a unit the list of currently controlled units.
        /// </summary>
        /// <param name="unit">The unit that should be added to the list.</param>
        public void AddActiveUnit(Unit unit)
        {
            _activeUnits.Add(unit);
        }

        /// <summary>
        /// Removes a unit the list of currently controlled units.
        /// </summary>
        /// <param name="unit">The unit that should be removed from the list.</param>
        public void RemoveActiveUnit(Unit unit)
        {
            _activeUnits.Remove(unit);
        }
    }
}
