using UnityEngine;

namespace MazeGeneration
{
    public class HuntAndKillAlgorithm : MazeAlgorithm
    {
        private int currentRow;
        private int currentColumn;

        private bool courseComplete;

        private readonly int[] dirX = { -1, 0, 1, 0 };
        private readonly int[] dirY = { 0, 1, 0, -1 };

        public HuntAndKillAlgorithm(MazeCell[,] mazeGrid, int seedVal) : base(mazeGrid)
        {
            // starts from index [0,0]
            currentRow = 0;
            currentColumn = 0;
            courseComplete = false;

            Random.InitState(seedVal);
            Debug.Log("Seed: " + seedVal);
        }

        public override void CreateMaze()
        {
            HuntAndKill();
        }

        private void HuntAndKill()
        {
            mazeGrid[currentRow, currentColumn].visited = true;


            while (!courseComplete)
            {
                Kill();
                Hunt();
            }
        }

        // continue removing walls till we reach the exit condition of no adjacent non-visited walls
        private void Kill()
        {
            while (IsRouteAvailable(currentRow, currentColumn))
            {
                int direction = UnityEngine.Random.Range(0, dirX.Length);
                int new_row = currentRow + dirX[direction];
                int new_col = currentColumn + dirY[direction];

                if (IsCellAvailable(new_row, new_col))
                {
                    DestroyWalls(currentRow, currentColumn, direction);
                    currentRow = new_row;
                    currentColumn = new_col;
                }
                mazeGrid[currentRow, currentColumn].visited = true;
            }
        }

        private void DestroyWalls(int row, int column, int direction)
        {
            if (direction == 0)
            {
                //DestroyWall(mazeCells[row, column].northWall);
                DestroyWall(mazeGrid[row - 1, column].southWall);
            }
            else if (direction == 1)
            {
                DestroyWall(mazeGrid[row, column].eastWall);
                //DestroyWall(mazeCells[row, column + 1].westWall);
            }
            else if (direction == 2)
            {
                DestroyWall(mazeGrid[row, column].southWall);
                //DestroyWall(mazeCells[row + 1, column].northWall);
            }
            else if (direction == 3)
            {
                //DestroyWall(mazeCells[row, column].westWall);
                DestroyWall(mazeGrid[row, column - 1].eastWall);
            }
        }

        private void Hunt()
        {
            courseComplete = true;

            for (int row = 0; row < maxRows; row++)
            {
                for (int col = 0; col < maxColumns; col++)
                {
                    if (IsRouteAvailable(row, col))
                    {
                        courseComplete = false;
                        currentRow = row;
                        currentColumn = col;
                        mazeGrid[currentRow, currentColumn].visited = true;
                        return;
                    }
                }
            }
        }

        // checks if any adjacent un-visited cells
        private bool IsRouteAvailable(int row, int column)
        {
            for (int i = 0; i < dirX.Length; i++)
            {
                int new_row = row + dirX[i];
                int new_col = column + dirY[i];
                if (IsCellAvailable(new_row, new_col))
                {
                    return true;
                }
            }
            return false;
        }

        private bool IsCellAvailable(int row, int column)
        {
            return IsValidCell(row, column) && !mazeGrid[row, column].visited;
        }

        private bool IsValidCell(int row, int column)
        {
            return row >= 0 && column >= 0 && row < maxRows && column < maxColumns;
        }

        private void DestroyWall(GameObject wall)
        {
            if (wall != null)
            {
                GameObject.DestroyImmediate(wall);
            }
        }

    }
}