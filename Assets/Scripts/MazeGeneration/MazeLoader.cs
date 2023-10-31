using UnityEngine;

namespace MazeGeneration
{
    public class MazeLoader
    {
        private GameObject wallPrefab;
        private int mazeRows, mazeColumns;
        private float size;
        private MazeCell[,] mazeGrid;
        private MazeAlgorithm mazeAlgorithm;


        public MazeLoader(int mazeRows, int mazeColumns, GameObject wallPrefab)
        {
            this.mazeRows = mazeRows;
            this.mazeColumns = mazeColumns;
            this.wallPrefab = wallPrefab;
        }


        public void SetupMaze(int mazeWidth, int mazeHeight, float wallSize)
        {
            // empty previous maze if any
            if (mazeGrid != null)
            {
                ClearMaze();
            }

            mazeRows = mazeWidth;
            mazeColumns = mazeHeight;
            size = wallSize;
        }

        public void CreateMaze(Transform parentObj, int seedVal)
        {
            InitializeMazeGrid(parentObj);

            // apply maze generation algorithm
            mazeAlgorithm = new HuntAndKillAlgorithm(mazeGrid, seedVal);
            mazeAlgorithm.CreateMaze();
        }

        private void InitializeMazeGrid(Transform parentObj)
        {
            mazeGrid = new MazeCell[mazeRows, mazeColumns];

            for (int row = 0; row < mazeRows; row++)
            {
                for (int col = 0; col < mazeColumns; col++)
                {
                    MazeCell cell = new MazeCell();
                    mazeGrid[row, col] = cell;

                    // wall and floor are using same prefab
                    // x,z Position at Vector2(rowVal * size, col * size) will be the position at center point of a cell.
                    // Rest we are offsetting to get the correct positions at the four corners
                    // Offsets - 
                    // North: -(size/2f) on x(row)
                    // East: +(size / 2f) on z(col)
                    // South: +(size/2f) on x(row)
                    // West: -(size/2f) on z(col)
                    // floor position = rowVal * size, -(size / 2f) , col * size 
                    cell.floor = Object.Instantiate(wallPrefab, new Vector3(row * size, -(size / 2f), col * size), Quaternion.identity, parentObj);

                    cell.floor.transform.Rotate(Vector3.right, 90f);
                    cell.eastWall = Object.Instantiate(wallPrefab, new Vector3(row * size, 0, (col * size) + (size / 2f)), Quaternion.identity, parentObj);
                    cell.southWall = Object.Instantiate(wallPrefab, new Vector3(row * size + (size / 2f), 0, col * size), Quaternion.identity, parentObj);
                    cell.southWall.transform.Rotate(Vector3.up * 90f);

                    if (col == 0)
                    {
                        cell.westWall = Object.Instantiate(wallPrefab, new Vector3(row * size, 0, (col * size) - (size / 2f)), Quaternion.identity, parentObj);
                    }
                    if (row == 0)
                    {
                        cell.northWall = Object.Instantiate(wallPrefab, new Vector3((row * size) - (size / 2f), 0, col * size), Quaternion.identity, parentObj);
                        cell.northWall.transform.Rotate(Vector3.up * 90f);
                    }
                }
            }
        }

        public void ClearMaze()
        {
            for (int row = 0; row < mazeRows; row++)
            {
                for (int col = 0; col < mazeColumns; col++)
                {
                    Object.DestroyImmediate(mazeGrid[row, col].floor);
                    Object.DestroyImmediate(mazeGrid[row, col].eastWall);
                    Object.DestroyImmediate(mazeGrid[row, col].southWall);

                    if (col == 0)
                    {
                        Object.DestroyImmediate(mazeGrid[row, col].westWall);
                    }
                    if (row == 0)
                    {
                        Object.DestroyImmediate(mazeGrid[row, col].northWall);
                    }
                }
            }
        }

    }
}