using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;

namespace Problem_Houses_and_Restaurants
{

    class RectangularComparer : IComparer
    {
        // maintain a reference to the 2-dimensional array being sorted
        private int[,] sortArray;

        // constructor initializes the sortArray reference
        public RectangularComparer(int[,] theArray)
        {
            sortArray = theArray;
        }

        public int Compare(object x, object y)
        {
            // x and y are integer row numbers into the sortArray
            int i1 = (int)x;
            int i2 = (int)y;

            // compare the items in the sortArray
            return sortArray[i1, 2].CompareTo(sortArray[i2, 2]);
        }

        public int[,] sort2DArray(int M)
        {
            int[] tagArray = new int[M];
            int[,] newRoad = new int[M, 3];

            for (int i = 0; i < M; i++)
                tagArray[i] = i;

            Array.Sort(tagArray, this);

            for (int i = 0; i < M; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    newRoad[i, j] = sortArray[tagArray[i], j];
                }

            }

            return newRoad;
        }
    }


    class Program
    {

        static void Main(string[] args)
        {
            Console.Write("Enter number of test cases : ");
            int iTestCases = Convert.ToInt32(Console.ReadLine());
            int[] output = new int[iTestCases];
            for (int iLoop = 0; iLoop < iTestCases; iLoop++)
            {
                output[iLoop]= getInput();
            }

            Console.WriteLine("Output:");
            for (int iLoop = 0; iLoop < iTestCases; iLoop++)
            {
                if (output[iLoop] == -1)
                    Console.WriteLine("Invalid Input");
                else
                    Console.WriteLine(output[iLoop]);
            }

            Console.ReadKey();
        }


        private static int getInput()
        {

            Console.Write("Enter the number of buildings and Roads: ");
            String s = Console.ReadLine();
            string[] input = s.Split(' ');
            int N = Convert.ToInt32(input[0]);
            int M = Convert.ToInt32(input[1]);

            List<char> bType = new List<char>(N);
            Console.Write("Enter type of buidings H/R : ");
            bType = Console.ReadLine().ToList();

            bType.Insert(0, 'E');

            Console.WriteLine("Please provide the road [Src Dest Cost]:");

            int[,] road = new int[M, 3];

            for (int i = 0; i < M; i++)
            {
                string[] map = Console.ReadLine().Split(' ');

                road[i, 0] = Convert.ToInt32(map[0]);
                road[i, 1] = Convert.ToInt32(map[1]);
                road[i, 2] = Convert.ToInt32(map[2]);
            }

            
            //Sort(ref road, 2, "ASC");

            // sort provided road as per their cost
            RectangularComparer reg = new RectangularComparer(road);
            road = reg.sort2DArray(M);
               
           return calcMinPath(bType, road, N);
        }

        private static int calcMinPath(List<char> bType, int[,] road,int N)
        {
            if (!bType.Contains('R'))
                return -1;

            else if (!bType.Contains('H'))
                return 0;

            else
            {
                List<int> restaurant = new List<int>();

                for (int i = 1; i <= N; i++)
                    if (bType[i] == 'R')
                        restaurant.Add(i);

                int minCost = 0, cost = 0;


                foreach (int r in restaurant)
                {
                    cost = getMinCost(r, restaurant, road, N);
                    if (cost < minCost || minCost == 0)
                        minCost = cost;
                }

                return minCost;
            }
        }

        private static int getMinCost(int dest, List<int> restaurant, int[,] road, int buidings)
        {
            List<int> rest = new List<int>();
            rest = restaurant.ToList();
            rest.Remove(dest);

            List<int> path = new List<int>();
            List<int> cost = new List<int>();
            bool flag = false;
            for (int i = 0; i < road.GetLength(0); i++)
            {
                if (path.Count == buidings)
                    break;
                if (!rest.Contains(road[i, 0]) && !rest.Contains(road[i, 1]))
                {
                    flag = false;
                    for (int j = 0; j < 2; j++)
                    {

                        if (!path.Contains(road[i, j]))
                        {
                            path.Add(road[i, j]);
                            flag = true;
                        }

                    }
                    if (flag)
                        cost.Add(road[i, 2]);
                }

            }
            return cost.Sum();
        }


        //private static void Sort(ref int[,] array, int sortCol, string order)
        //{
        //    int colCount = array.GetLength(1), rowCount = array.GetLength(0);
        //    if (sortCol >= colCount || sortCol < 0)
        //        throw new System.ArgumentOutOfRangeException("sortCol", "The column to sort on must be contained within the array bounds.");

        //    DataTable dt = new DataTable();
        //    // Name the columns with the second dimension index values, e.g., "0", "1", etc.
        //    for (int col = 0; col < colCount; col++)
        //    {
        //        DataColumn dc = new DataColumn(col.ToString(), typeof(int));
        //        dt.Columns.Add(dc);
        //    }
        //    // Load data into the data table:
        //    for (int rowindex = 0; rowindex < rowCount; rowindex++)
        //    {
        //        DataRow rowData = dt.NewRow();
        //        for (int col = 0; col < colCount; col++)
        //            rowData[col] = array[rowindex, col];
        //        dt.Rows.Add(rowData);
        //    }
        //    // Sort by using the column index = name + an optional order:
        //    DataRow[] rows = dt.Select("", sortCol.ToString() + " " + order);

        //    for (int row = 0; row <= rows.GetUpperBound(0); row++)
        //    {
        //        DataRow dr = rows[row];
        //        for (int col = 0; col < colCount; col++)
        //        {
        //            array[row, col] = (int)dr[col];
        //        }
        //    }

        //    dt.Dispose();
        //}
    }
}

/*

Example

Input:

3
3 5
HHR
1 2 3
1 2 5
1 3 10
3 2 -1
3 1 7
2 2
RR
1 2 1
2 1 2
3 3
HRR
1 2 1
1 3 2
2 3 3

Output:
2
0
1

*/
