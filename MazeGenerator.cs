using System;
using System.Collections.Generic;
using System.Linq;
// using System.Threading;

namespace Procedural_Maze
{
    public class MazeGenerator
    {
        private enum Orientation
        {
            Horizontal, Vertical
        }

        private static readonly Random Random = new Random();
        
        private Orientation _mazeWallOrientation; // The current orientation of the wall being generated
        
        private bool _orientationVertical = Random.NextDouble() > 0.5; // The current orientation of the wall being generated
        
        private List<int> _columns; // tracks the possible columns walls can be made at
        private List<int> _rows; // tracks the possible rows walls can be made at

        private int _wallLocation; // The index of the wall being generated

        private bool _borderSet; // The status of the outer border

        private const int Top = 1;
        private const int Right = 2;
        private const int Bottom = 3;
        private const int Left = 4;
        
        public static void Main()
        {
            MazeGenerator mazeGenerator = new MazeGenerator();
            int[,] testMaze = mazeGenerator.MakeMaze(mazeGenerator.MazeArray(50,50));
            Console.WriteLine("Maze Complete");
            PrintMaze(testMaze);
        }

        private int[,] MazeArray(int length, int width)
        {
            // Console.WriteLine($"Vertical orientation is {_orientationVertical}");
            _rows = new List<int>();
            _columns = new List<int>();

            for (var index = 0; index < length-1; index++)
            {
                _rows.Add((index + 1) * 2);
            }
            for (var index = 0; index < width-1; index++)
            {
                _columns.Add((index + 1) * 2);
            }
            
            int[,] mazeArray = new int[(length*2)+1, (width*2)+1];
            for (int len = 0; len < mazeArray.GetLength(0); len++)
            {
                for (int wid = 0; wid < mazeArray.GetLength(1); wid++)
                {
                    mazeArray[len, wid] = 0;
                }
            }
            // PrintMaze(mazeArray);
            
            //Start of borderCheck
            // check if the border of the array is set
            // make the borders of the maze by converting all 0s at the ends into 1s
            for (int verticalPosition = 0; verticalPosition < mazeArray.GetLength(0); verticalPosition++)
            {
                if (_borderSet){ Console.WriteLine("Outer Border Already Created");break;}
                
                for (int horizontalPosition = 0; horizontalPosition < mazeArray.GetLength(1); horizontalPosition++)
                {
                    if (!mazeArray[verticalPosition, horizontalPosition].Equals(0)) continue;
                    _borderSet = false;
                    break;
                }
            }
            if (!_borderSet)
            {
                // Console.WriteLine("Border is undefined. Creating Border.....");
                for (int verticalPosition = 0; verticalPosition < mazeArray.GetLength(0); verticalPosition++)
                {
                    for (int horizontalPosition = 0; horizontalPosition < mazeArray.GetLength(1); horizontalPosition++)
                    {
                        if (verticalPosition.Equals(0) || verticalPosition.Equals(mazeArray.GetLength(0)-1) || horizontalPosition.Equals(0) || horizontalPosition.Equals(mazeArray.GetLength(1)-1))
                        {
                            mazeArray[verticalPosition, horizontalPosition] = 1;
                        }
                    }
                }
            }
            //End of borderCheck 
            // PrintMaze(mazeArray);
            return mazeArray;
        }
        
        public int[,] MazeArray()
        {
            int[,] mazeArray = new int[7, 7];
            for (int len = 0; len < mazeArray.GetLength(0); len++)
            {
                for (int wid = 0; wid < mazeArray.GetLength(1); wid++)
                {
                    mazeArray[len, wid] = 0;
                }
            }
            //Start of borderCheck
            // check if the border of the array is set
            // make the borders of the maze by converting all 0s at the ends into 1s
            for (int verticalPosition = 0; verticalPosition < mazeArray.GetLength(0); verticalPosition++)
            {
                if (_borderSet){ /*Console.WriteLine("Outer Border Already Created")*/;break;}
                
                for (int horizontalPosition = 0; horizontalPosition < mazeArray.GetLength(1); horizontalPosition++)
                {
                    if (!mazeArray[verticalPosition, horizontalPosition].Equals(0)) continue;
                    _borderSet = false;
                    break;
                }
            }
            if (!_borderSet)
            {
                // Console.WriteLine("Border is undefined. Creating Border.....");
                for (int verticalPosition = 0; verticalPosition < mazeArray.GetLength(0); verticalPosition++)
                {
                    for (int horizontalPosition = 0; horizontalPosition < mazeArray.GetLength(1); horizontalPosition++)
                    {
                        if (verticalPosition.Equals(0) || verticalPosition.Equals(mazeArray.GetLength(0)-1) || horizontalPosition.Equals(0) || horizontalPosition.Equals(mazeArray.GetLength(1)-1))
                        {
                            mazeArray[verticalPosition, horizontalPosition] = 1;
                        }
                    }
                }
            }
            //End of borderCheck 
            //PrintMaze(mazeArray);
            return mazeArray; 
        }

        private int[,] MakeMaze(int[,] mazeArray)
        {
            if (_rows.Count.Equals(0) && _columns.Count.Equals(0))
            {
                // Console.WriteLine("No remaining valid columns or rows can be created");
                CreateMazeEntranceAndExit(mazeArray);
                return mazeArray;
            }

            if (_columns.Count.Equals(0))
            {
               // Console.WriteLine("No remaining valid columns can be created");
                _orientationVertical = false;
            }
            if (_rows.Count.Equals(0))
            {
               // Console.WriteLine("No remaining valid rows can be created");
                _orientationVertical = true; 
            }

            // Console.WriteLine($"Previous Wall Made at: {_wallLocation}");
            _mazeWallOrientation = _orientationVertical ? Orientation.Vertical : Orientation.Horizontal;
            
            _wallLocation = _orientationVertical ? _columns[Random.Next(_columns.Count)] : _rows[Random.Next(_rows.Count)];
            // Console.WriteLine($"Creating a {_mazeWallOrientation.ToString()} wall at position {_wallLocation}");
            
            
            if (_orientationVertical) // if a vertical line is being made
            {
                // Console.WriteLine("MAKING VERTICAL WALL");
                int firstIndex = 0; // The position of the start of the wall
                int secondIndex = 0; // The position of the end of the wall
                
                int endOfSearch = mazeArray.GetLength(0);
                // Console.WriteLine($"end of search set to {endOfSearch}");
                
                for (int index = 0; index < endOfSearch-1; index++)
                {
                    if (mazeArray[index,_wallLocation].Equals(1))
                    {
                        // Console.WriteLine($"A One has been found at {index}");
                        firstIndex = index;
                        // Console.WriteLine($"First index set to {firstIndex}");
                        
                        for (int index2 = firstIndex+1; index2 < mazeArray.GetLength(0) ; index2++)
                        {
                            if (mazeArray[index2,_wallLocation].Equals(1))
                            {
                                // Console.WriteLine($"A One has been found at {index2}");
                                secondIndex = index2;
                                // Console.WriteLine($"Second index set to {secondIndex }");
                                
                                if (secondIndex-firstIndex >= 3)
                                {
                                    // Console.WriteLine($"First index at {firstIndex}. Second index at {secondIndex}");
                                    goto WallCreation;
                                }

                                break;
                            }
                        }
                    }
                }

                
                // Create a wall from the first index to the second
                WallCreation:
                for (int vertPos = firstIndex; vertPos < secondIndex; vertPos++)
                {
                    // Console.WriteLine($"Making a wall from {firstIndex} to {secondIndex }");
                    mazeArray[vertPos,_wallLocation] = 1;
                }
                
                //Thread.Sleep(10);
                int holeIndex = GenerateRandomOddValue(firstIndex, secondIndex);
                // Console.WriteLine($"Making hole at {holeIndex}");
                // Make a hole in the wall at an odd index between the first and second index
                mazeArray[holeIndex,_wallLocation] = 0;
            }
            else // if a horizontal line is being made
            {
                int firstIndex = 0;
                int secondIndex = 0;

                int endOfSearch = mazeArray.GetLength(1);
                
                // Console.WriteLine($"Finding indices...\nEnd of search set to {endOfSearch}");
                for (int index = 0; index < endOfSearch-1; index++)
                {
                    if (mazeArray[_wallLocation,index].Equals(1))
                    {
                        // Console.WriteLine($"A One has been found at {index}");
                        firstIndex = index;
                        // Console.WriteLine($"First index set to {firstIndex}");
                        
                        for (int index2 = firstIndex+1; index2 < mazeArray.GetLength(1) ; index2++)
                        {
                            if (mazeArray[_wallLocation,index2].Equals(1))
                            {
                                // Console.WriteLine($"A One has been found at {index2}");
                                secondIndex = index2;
                                // Console.WriteLine($"Second index set to {secondIndex }");
                                
                                if (secondIndex-firstIndex >= 3)
                                {
                                    // Console.WriteLine($"First index at {firstIndex}. Second index at {secondIndex}");
                                    goto WallCreation; 
                                }
                                break;
                            }
                        }
                    }
                }

                // Create a wall from the first index to the second
                WallCreation:
                for (int horiPos = firstIndex; horiPos  < secondIndex; horiPos++)
                {
                    mazeArray[_wallLocation,horiPos] = 1;
                }

                //Thread.Sleep(10);
                // Make a hole in the wall at an odd index between the first and second index
                int holeIndex = GenerateRandomOddValue(firstIndex, secondIndex);
                // Console.WriteLine($"Making hole at {holeIndex}");
                mazeArray[_wallLocation,holeIndex] = 0;
            }
            // PrintMaze(mazeArray);

            // Update the contents of the column and row arrays
            int endOfRowList = _rows.Last();
            int endOfColumnList = _columns.Last();
            
            for (int rowNumber = 2; rowNumber <= endOfRowList; rowNumber+=2) // rows
            {
                if (!_rows.Contains(rowNumber)) continue;
                // Console.WriteLine($"Checking {Orientation.Horizontal.ToString()} line {rowNumber}");
                UpdateLineStatuses(mazeArray,Orientation.Horizontal,rowNumber);
            }
            for (int columnNumber = 2; columnNumber <= endOfColumnList; columnNumber+=2) //columns
            {
                if (!_columns.Contains(columnNumber)) continue;
                
                // Console.WriteLine($"Checking {Orientation.Vertical.ToString()} line {columnNumber}");
                UpdateLineStatuses(mazeArray,Orientation.Vertical,columnNumber);
            }
            
            
            
            // PrintMaze(mazeArray);
            // Console.WriteLine("ROWS");
            // foreach(var item in _rows){Console.WriteLine(item);}
            // Console.WriteLine("COLUMNS");
            // foreach(var item in _columns){Console.WriteLine(item);}
            // _orientationVertical = !_orientationVertical; // make the next line the opposite orientation
            MakeMaze(mazeArray);
            
            return mazeArray;
        }
        
        private void UpdateLineStatuses(int[,] mazeArray, Orientation orientation, int lineNumber)
        {
            //Thread.Sleep(10);
            if (orientation.Equals(Orientation.Vertical))
            {
                // for (int i = 0; i < mazeArray.GetLength(0); i++)
                // {
                //     Console.WriteLine(mazeArray[i,lineNumber]);
                // }
                bool lineIsAlive = false; //Whether the line has room for more walls to be added

                int firstIndex = 0;
                
                for (var vertPos = 0; vertPos < mazeArray.GetLength(0); vertPos++)
                {
                    if (mazeArray[vertPos,lineNumber].Equals(1))
                    {
                        firstIndex = vertPos;
                    }
                    
                    int secondIndex = 0;
                    for (int vertPos2 = vertPos + 1; vertPos2 < mazeArray.GetLength(0); vertPos2++)
                    {
                        if (mazeArray[vertPos2,lineNumber].Equals(1))
                        {
                            secondIndex = vertPos2;
                            if (secondIndex-firstIndex > 2)
                            {
                                lineIsAlive = true;
                            }
                            break;
                        }
                    }
                    // Console.WriteLine($"Vertical line at {lineNumber} is {lineIsAlive}");
                    if (lineIsAlive) break;
                }

                //if there is no more room on that line, remove it from the list of valid vertical lines
                if (!lineIsAlive)
                {
                    // Console.WriteLine($" Vertical Line at {lineNumber} is no longer valid");
                    _columns.Remove(lineNumber);
                }
            }
            else
            {
                // for (int i = 0; i < mazeArray.GetLength(1); i++)
                // {
                //     Console.Write($" {mazeArray[lineNumber, i]}");
                // }
                bool lineIsAlive = false; //Whether the line has room for more walls to be added

                int firstIndex = 0;
                for (int horPos = 0; horPos < mazeArray.GetLength(1); horPos++)
                {
                    if (mazeArray[lineNumber,horPos].Equals(1))
                    {
                        firstIndex = horPos;
                    }
                    //Thread.Sleep(10);
                    int secondIndex = 0;
                    for (int horPos2 = horPos + 1; horPos2 < mazeArray.GetLength(1); horPos2++)
                    {
                        if (mazeArray[lineNumber,horPos2].Equals(1))
                        {
                            secondIndex = horPos2;
                            if (secondIndex-firstIndex > 2)
                            {
                                lineIsAlive = true;
                            }

                            break;
                        }
                    }
                    if (lineIsAlive) break;
                }
                // Console.WriteLine($"Horizontal line at {lineNumber} is {lineIsAlive}");
                //if there is no more room on that line, remove it from the list of valid vertical lines
                if (!lineIsAlive)
                {
                    // Console.WriteLine($"Horizontal Line at {lineNumber} is no longer valid");
                    _rows.Remove(lineNumber);
                    // Console.WriteLine("ROWS: ");
                    // foreach (var item in _rows)
                    // {
                    //     Console.Write($" {item}");
                    // }
                }
            }
        }


        private void CreateMazeEntranceAndExit(int[,] mazeArray)
        {
            // Pick a random side, make a hole at an odd index
                List<int> sides = new List<int>() { 1, 2, 3, 4 };//1- top : 2 - Right : 3 - Bottom : 4 - Left
                int entrance = sides[Random.Next(sides.Count)];
                sides.Remove(entrance);
                int exit = sides[Random.Next(sides.Count)];

                int yPositionEntrance,xPositionEntrance,yPositionExit,xPositionExit;
                switch (entrance)
                {
                    case Top:
                        yPositionEntrance = mazeArray.GetUpperBound(0);
                        mazeArray[yPositionEntrance, GenerateRandomOddValue(0,mazeArray.GetLength(1)-1)] = 0;
                        break;
                    case Right:
                        xPositionEntrance = mazeArray.GetUpperBound(1);
                        mazeArray[GenerateRandomOddValue(0, mazeArray.GetLength(0)-1),xPositionEntrance] = 0;
                        break;
                    case Bottom:
                        yPositionEntrance = mazeArray.GetLowerBound(0);
                        mazeArray[yPositionEntrance, GenerateRandomOddValue(0,mazeArray.GetLength(1)-1)] = 0;
                        break;
                    case Left:
                        xPositionEntrance = mazeArray.GetLowerBound(1);
                        mazeArray[GenerateRandomOddValue(0, mazeArray.GetLength(0)-1),xPositionEntrance] = 0;
                        break;
                }

                switch (exit)
                {
                    case Top:
                        yPositionExit = mazeArray.GetUpperBound(0);
                        mazeArray[yPositionExit, GenerateRandomOddValue(0,mazeArray.GetLength(1)-1)] = 0;
                        break;
                    case Right:
                        xPositionExit= mazeArray.GetUpperBound(1);
                        mazeArray[GenerateRandomOddValue(0, mazeArray.GetLength(0)-1),xPositionExit] = 0;
                        break;
                    case Bottom:
                        yPositionExit = mazeArray.GetLowerBound(0);
                        mazeArray[yPositionExit, GenerateRandomOddValue(0,mazeArray.GetLength(0)-1)] = 0;
                        break;
                    case Left:
                        xPositionExit = mazeArray.GetLowerBound(1);
                        mazeArray[GenerateRandomOddValue(0, mazeArray.GetLength(0)-1),xPositionExit] = 0;
                        break;
                }
        }
        private static void PrintMaze(int[,] mazeArray)
        {
            for (int len = 0; len < mazeArray.GetLength(0); len++)
            {
                for (int wid = 0; wid < mazeArray.GetLength(1); wid++)
                {
                    //Thread.Sleep(10);
                    Console.Write(mazeArray[len,wid] + " ");
                }
                Console.WriteLine();
            }
            Console.WriteLine("---------------------------");
        }
        
        // copied from https://stackoverflow.com/questions/33870759/generate-a-random-even-number-inside-a-range
        private int GenerateRandomEvenValue(int min, int max)
        {
            if (min.Equals(0)) min += 2;
            return min + Random.Next((max-min)/2)*2;
        }
        
        // Generates a random odd number between the minimum and maximum values
        private int GenerateRandomOddValue(int min, int max)
        {
            if (max%2 == 0) max--;
            // ReSharper disable once SuggestVarOrType_BuiltInTypes
            int value = Random.Next(min, max + 1);
            value += (value % 2 == 0 ? 1 : 0);
            return value;
        }
    }
}