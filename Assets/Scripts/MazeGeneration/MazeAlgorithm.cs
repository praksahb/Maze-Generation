namespace MazeGeneration
{
    public abstract class MazeAlgorithm
    {
        protected MazeCell[,] mazeGrid;
        protected int maxRows, maxColumns;

        protected MazeAlgorithm(MazeCell[,] mazeGrid)
        {
            this.mazeGrid = mazeGrid;
            maxRows = mazeGrid.GetLength(0);
            maxColumns = mazeGrid.GetLength(1);
        }

        public abstract void CreateMaze();
    }
}