using System;
using System.IO;
using System.Collections.Generic;

namespace Day17
{
    static class Program
    {
        public static HashSet<Cube> activeCubesList = new HashSet<Cube>();
        static void Main(string[] args)
        {
            DateTime time = DateTime.Now;
            string path = "C:\\Users\\richard\\source\\repos\\AdventOfCode2020\\Day17\\input.txt";
            InitializeGrid(path);
            int activeCubes = CycleBoot3D();
            Console.WriteLine("Cubes count after 6 3D cycles " + activeCubes);
            InitializeGrid(path);
            activeCubes = CycleBoot4D();
            Console.WriteLine("Cubes count after 6 4D cycles " + activeCubes);

        }

        private static int CycleBoot4D()
        {
            HashSet<Cube> buffer = new HashSet<Cube>(); ;
            Cube currentCube;

            int w, x, y, z;
            int activeNeighbors = 0;
            // Boot cycle
            for (int cycle = 1; cycle <= 6; cycle++)
            {
                int wmin = 0 - cycle;
                int wmax = 0 + cycle;
                int xmin = 0 - cycle;
                int xmax = 7 + cycle;
                int ymin = 0 - cycle;
                int ymax = 7 + cycle;
                int zmin = 0 - cycle;
                int zmax = 0 + cycle;
                // Let's start the nested loop cycle to check surrounding cubes for each cube

                for (w = wmin; w <= wmax; w++)
                {
                    for (z = zmin; z <= zmax; z++)
                    {
                        for (x = xmin; x <= xmax; x++)
                        {
                            for (y = ymin; y <= ymax; y++)
                            {
                                activeNeighbors = 0;
                                for (int wneighbor = w - 1; wneighbor <= w + 1; wneighbor++)
                                {
                                    for (int xneighbor = x - 1; xneighbor <= x + 1; xneighbor++)
                                    {
                                        for (int yneighbor = y - 1; yneighbor <= y + 1; yneighbor++)
                                        {
                                            for (int zneighbor = z - 1; zneighbor <= z + 1; zneighbor++)
                                            {

                                                if (activeCubesList.Contains(new Cube(wneighbor, xneighbor, yneighbor, zneighbor)) && (w != wneighbor || x != xneighbor || y != yneighbor || z != zneighbor))
                                                {
                                                    activeNeighbors++;
                                                }
                                            }
                                        }
                                    }
                                }
                                currentCube = new Cube(w, x, y, z);
                                if (activeCubesList.Contains(currentCube) && (activeNeighbors == 2 || activeNeighbors == 3))
                                    buffer.Add(currentCube);
                                else if (!activeCubesList.Contains(currentCube) && activeNeighbors == 3)
                                {
                                    buffer.Add(currentCube);
                                }

                            }

                        }
                    }
                }
                activeCubesList = new HashSet<Cube>(buffer);
                buffer.Clear();
            }
            return activeCubesList.Count;
        }

        private static int CycleBoot3D()
        {
            HashSet<Cube> buffer = new HashSet<Cube>();
            Cube currentCube;

            int x, y, z;
            int activeNeighbors = 0;
            // Boot cycle
            for (int cycle = 1; cycle <= 6; cycle++)
            {
                int xmin = 0 - cycle;
                int xmax = 7 + cycle;
                int ymin = 0 - cycle;
                int ymax = 7 + cycle;
                int zmin = 0 - cycle;
                int zmax = 0 + cycle;
                // Let's start the nested loop cycle to check surrounding cubes for each cube
                for (z = zmin; z <= zmax; z++)
                {
                    for (x = xmin; x <= xmax; x++)
                    {
                        for (y = ymin; y <= ymax; y++)
                        {
                            activeNeighbors = 0;
                            for (int xneighbor = x - 1; xneighbor <= x + 1; xneighbor++)
                            {
                                for (int yneighbor = y - 1; yneighbor <= y + 1; yneighbor++)
                                {
                                    for (int zneighbor = z - 1; zneighbor <= z + 1; zneighbor++)
                                    {
                                        if (activeCubesList.Contains(new Cube(0, xneighbor, yneighbor, zneighbor)) && (x != xneighbor || y != yneighbor || z != zneighbor))
                                        {
                                            activeNeighbors++;
                                        }
                                    }
                                }
                            }
                            currentCube = new Cube(0, x, y, z);
                            if (activeCubesList.Contains(currentCube) && (activeNeighbors == 2 || activeNeighbors == 3))
                                buffer.Add(currentCube);
                            else if (!activeCubesList.Contains(currentCube) && activeNeighbors == 3)
                            {
                                buffer.Add(currentCube);
                            }
                        }
                    }

                }

                activeCubesList = new HashSet<Cube>(buffer);
                buffer.Clear();
            }
            return activeCubesList.Count;
        }

        private static void InitializeGrid(string path)
        {
            activeCubesList.Clear();
            int x = 0;
            int y;
            foreach (string line in File.ReadAllLines(@path))
            {
                y = 0;
                char[] values = line.ToCharArray();
                foreach (char value in values)
                {
                    if (value == '#')
                        activeCubesList.Add(new Cube(0,x, y, 0, true));
                    //else activeCubesList.Add(new Cube(x, y, 0, false));
                    y++;
                }
                x++;
            }

        }
    }


}
