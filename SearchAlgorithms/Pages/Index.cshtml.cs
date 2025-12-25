// Pages/Index.cshtml.cs
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Collections.Generic;
using System.Linq;
using System;

namespace GoldSearchWebApp.Pages
{
    public class IndexModel : PageModel
    {
        [BindProperty]
        public int GridSize { get; set; } = 5; // Default value
        [BindProperty]
        public string PlayerCoordsInput { get; set; } = "0 0"; // Changed default for better explanation of 0-based index
        [BindProperty]
        public string GoldCoordsInput { get; set; } = "4 4"; // Changed default for better explanation of 0-based index
        [BindProperty]
        public string ObstaclesInput { get; set; } = ""; // For Phase 3: Obstacle coordinates
        [BindProperty]
        public string SelectedAlgorithm { get; set; }

        public Grid CurrentGrid { get; set; }
        public List<Cell> PathFound { get; set; }
        public int StepsCount { get; set; }
        public string ResultMessage { get; set; } // For displaying success/failure messages
        public bool ShowResultSection { get; set; } = false; // To control the display of the results section

        public void OnGet()
        {
            // This method runs when the page is loaded for the first time.
            InitializeGrid();
            ShowResultSection = false; // Hide results section on initial load
        }

        public IActionResult OnPostSearch()
        {
            ShowResultSection = true; // Show results section after search button is clicked

            // First, create the Grid with the new dimensions to perform coordinate validation
            CurrentGrid = new Grid(GridSize);

            // Parsing player input
            string[] playerParts = PlayerCoordsInput.Split(' ', StringSplitOptions.RemoveEmptyEntries); // RemoveEmptyEntries to handle extra spaces
            if (playerParts.Length == 2 && int.TryParse(playerParts[0], out int pX) && int.TryParse(playerParts[1], out int pY))
            {
                if (CurrentGrid.IsValidCoordinate(pX, pY))
                {
                    CurrentGrid.PlacePlayer(pX, pY);
                }
                else
                {
                    ResultMessage = "Error: Player start coordinates are out of grid bounds. (Remember: coordinates start from 0)";
                    return Page();
                }
            }
            else { ResultMessage = "Error: Invalid player coordinates format (Example: 3 4)."; return Page(); }

            // Parsing gold input
            string[] goldParts = GoldCoordsInput.Split(' ', StringSplitOptions.RemoveEmptyEntries);
            if (goldParts.Length == 2 && int.TryParse(goldParts[0], out int gX) && int.TryParse(goldParts[1], out int gY))
            {
                if (CurrentGrid.IsValidCoordinate(gX, gY))
                {
                    CurrentGrid.PlaceGold(gX, gY);
                }
                else
                {
                    ResultMessage = "Error: Gold source coordinates are out of grid bounds. (Remember: coordinates start from 0)";
                    return Page();
                }
            }
            else { ResultMessage = "Error: Invalid gold coordinates format (Example: 1 5)."; return Page(); }

            // Check for player and gold collision
            if (CurrentGrid.PlayerStart.X == CurrentGrid.GoldLocation.X && CurrentGrid.PlayerStart.Y == CurrentGrid.GoldLocation.Y)
            {
                ResultMessage = "Error: Player and gold positions cannot be the same.";
                return Page();
            }

            // Adding obstacles (Phase 3)
            if (!string.IsNullOrWhiteSpace(ObstaclesInput))
            {
                string[] obsCoordPairs = ObstaclesInput.Split(',', StringSplitOptions.RemoveEmptyEntries);
                foreach (var pair in obsCoordPairs)
                {
                    string[] coords = pair.Trim().Split(' ', StringSplitOptions.RemoveEmptyEntries);
                    if (coords.Length == 2 && int.TryParse(coords[0], out int oX) && int.TryParse(coords[1], out int oY))
                    {
                        if (CurrentGrid.IsValidCoordinate(oX, oY))
                        {
                            if (!CurrentGrid.Cells[oX, oY].IsPlayer && !CurrentGrid.Cells[oX, oY].IsGold)
                            {
                                CurrentGrid.PlaceObstacle(oX, oY);
                            }
                            else
                            {
                                ResultMessage = "Warning: Some obstacle coordinates conflict with player or gold and were not added.";
                                // Continue with a warning, as the main message will show the path.
                            }
                        }
                        else
                        {
                            ResultMessage = "Warning: Some obstacle coordinates are invalid and were not added.";
                            // Continue with a warning
                        }
                    }
                    else
                    {
                        ResultMessage = "Warning: Format of some obstacle coordinates is incorrect and they were ignored.";
                        // Continue with a warning
                    }
                }
            }

            (PathFound, StepsCount) = (null, 0); // Reset values

            // Execute the selected algorithm
            if (CurrentGrid.PlayerStart == null || CurrentGrid.GoldLocation == null)
            {
                ResultMessage = "Error: Player start or gold source coordinates are not specified.";
                return Page();
            }

            switch (SelectedAlgorithm)
            {
                case "BFS":
                    (PathFound, StepsCount) = BFS.FindPath(CurrentGrid, CurrentGrid.PlayerStart, CurrentGrid.GoldLocation);
                    break;
                case "DFS":
                    (PathFound, StepsCount) = DFS.FindPath(CurrentGrid, CurrentGrid.PlayerStart, CurrentGrid.GoldLocation);
                    break;
                case "UCS":
                    (PathFound, StepsCount) = UCS.FindPath(CurrentGrid, CurrentGrid.PlayerStart, CurrentGrid.GoldLocation);
                    break;
                case "AStar":
                    (PathFound, StepsCount) = AStar.FindPath(CurrentGrid, CurrentGrid.PlayerStart, CurrentGrid.GoldLocation);
                    break;
                default:
                    ResultMessage = "Error: Please select a search algorithm.";
                    return Page();
            }

            if (PathFound != null)
            {
                ResultMessage = $"Path found! Steps taken: {StepsCount}";
            }
            else
            {
                ResultMessage = "No path to gold source found. (Obstacles might be blocking the path, or no path exists.)";
            }

            return Page(); // Refresh the page to display results
        }

        private void InitializeGrid()
        {
            CurrentGrid = new Grid(GridSize);
        }
    }
}