using System;
using System.Windows.Controls;
using System.Windows.Shapes;

namespace GreedySnake.components
{
    class UIboard
    {
        private readonly Rectangle[,] rectGrid;
        private readonly WeakReference<Grid> mainGrid;

        public UIboard(uint size, Grid grid, double uiWidth, double uiHeight)
        {
            mainGrid = new WeakReference<Grid>(grid);
            rectGrid = CreateGrid(size, uiWidth, uiHeight);
        }

        public Rectangle[,] CreateGrid(uint size, double uiWidth, double uiHeight)
        {
            var rectGrid = new Rectangle[size, size];

            if (!mainGrid.TryGetTarget(out var grid))
            {
                return rectGrid;
            }

            for (int row = 0; row < size; row++)
                for (int col = 0; col < size; col++)
                {
                    var rect = new Rectangle
                    {
                        Fill = null,
                        StrokeThickness = 1,
                        Stroke = ColorBrushes.whiteStroke,
                    };
                    rect.Width = uiWidth / size;
                    rect.Height = uiHeight / size;
                    grid.Children.Add(rect);
                    Grid.SetRow(rect, col);
                    Grid.SetColumn(rect, row);

                    rectGrid[row, col] = rect;
                }
            return rectGrid;
        }

        public void RenderBlock(Block block)
        {
            var rect = rectGrid[block.coordinate.x, block.coordinate.y];
            rect.Fill = block.fill;
            rect.Stroke = block.stroke;
        }

        public void Reset()
        {
            if (!mainGrid.TryGetTarget(out _))
            {
                return;
            }
            for (int row = 0; row < rectGrid.GetLength(0); row++)
                for (int col = 0; col < rectGrid.GetLength(1); col++)
                {
                    var rect = rectGrid[row, col];
                    rect.Fill = null;
                    rect.Stroke = ColorBrushes.whiteStroke;
                }
        }
    }
}
