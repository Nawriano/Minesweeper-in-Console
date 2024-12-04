
using System.Reflection.Metadata;
using System.Runtime.CompilerServices;
using System.Text;
using System;

namespace MinesweeperConsole
{
    internal class Program
    {
        class Minefield
        {
            public List<List<Spot>> Field = new List<List<Spot>>();
            public List<Spot> Spots = new List<Spot>();
        }
        class Spot
        {
            public char text = '#';
            public int role = 0; //0 = empty, 1 = MIIINEEEEEN
            public int CloseBombCount;
            public bool Visibility = false;
            public int x, y;
            public List<Spot> Neighbours = new List<Spot>();
            public int id;
        }
        class Bomb
        {
            public class Spot;
            public int x;
            public int y;
        }

        private int bomba;
        static void Main(string[] args)
        {
            Gamesense();
        }
        private static void Gamesense()
        {
            Minefield minefield = new Minefield();
            Console.WriteLine("Width: ");
            int x = 0, y = 0, bombCount = 0;
            int num;
            string DumTest = Console.ReadLine();
            bool BoolDumTest = int.TryParse(DumTest, out num);
            if (BoolDumTest == false)
            {
                Console.WriteLine("Bruh, type numbers...");
                Console.ReadLine();
            }
            else
            {
                x = int.Parse(DumTest);
            }
            Console.WriteLine("Height: ");
            DumTest = Console.ReadLine();
            BoolDumTest = int.TryParse(DumTest, out num);
            if (BoolDumTest == false)
            {
                Console.WriteLine("Eh.. try again, but with numbers.");
                Console.ReadLine();
            }
            else
            {
                y = int.Parse(DumTest);
            }
            Console.WriteLine("How many minen?:");
            DumTest = Console.ReadLine();
            BoolDumTest = int.TryParse(DumTest, out num);
            if (BoolDumTest == false)
            {
                Console.WriteLine("Bro.. number!");
                Console.ReadLine();
            }
            else
            {
                bombCount = int.Parse(DumTest);
                
            }

            SetupField(x, y, bombCount, minefield);
            int curPosX = 0;
            int curPosY = 0;
            bool Game = true;
            

            while(Game == true)
            {
                int stage = 0;

                Interface(minefield, x, y, curPosX, curPosY);
                ConsoleKeyInfo KeyInput = Console.ReadKey();
                if (KeyInput.Key == ConsoleKey.UpArrow)
                {
                    if(curPosY == 0)
                    {
                        curPosY = y - 1;
                    }
                    else
                    {
                        curPosY -= 1;
                    }
                }
                else if(KeyInput.Key == ConsoleKey.DownArrow)
                {
                    if(curPosY == y - 1)
                    {
                        curPosY = 0;
                    }
                    else
                    {
                        curPosY += 1;
                    }
                }
                else if (KeyInput.Key == ConsoleKey.LeftArrow)
                {
                    if(curPosX == 0)
                    {
                        curPosX = x - 1;
                    }
                    else
                    {
                        curPosX -= 1;
                    }
                }
                else if (KeyInput.Key == ConsoleKey.RightArrow)
                {
                    if(curPosX == x - 1)
                    {
                        curPosX = 0;
                    }
                    else
                    {
                        curPosX += 1;
                    }
                }
                else if (KeyInput.Key == ConsoleKey.Spacebar)
                {
                    if (minefield.Field[curPosY][curPosX].role == 1)
                    {
                        minefield.Field[curPosY][curPosX].text = 'B';
                        Interface(minefield, x, y, curPosX, curPosY);
                        Console.WriteLine("Game over, you stepped on MINNNEEEN");
                        Console.WriteLine("To play again press R");
                        if(Console.ReadKey().Key == ConsoleKey.R)
                        {
                            Console.WriteLine(); Console.WriteLine();
                            Restart();
                        }
                        else
                        {
                            Console.WriteLine("Program will shut shortly after next intput");
                            Console.Read();
                            return;
                        }
                    }
                    else
                    {
                        if (minefield.Field[curPosY][curPosX].CloseBombCount > 0)
                        {
                            minefield.Field[curPosY][curPosX].Visibility = true;
                            minefield.Field[curPosY][curPosX].text = Convert.ToChar(minefield.Field[curPosY][curPosX].CloseBombCount + 48);
                        }
                        else
                        {   UncoverEmpty(minefield, curPosX, curPosY, x, y);
                            UncoverNum(minefield, x, y);
                            
                            
                        }
                        
                    }
                }
            }

        }

        private static void UncoverNum(Minefield minefield, int x, int y)
        {
           foreach(Spot sp in minefield.Spots)
            {
                foreach(Spot ng in sp.Neighbours)
                {
                    if(ng.CloseBombCount == 0 && ng.Visibility == true && ng.role == 0 && sp.role == 0)
                    {
                        sp.Visibility = true;
                        if(sp.CloseBombCount > 0)
                        {
                            sp.text = Convert.ToChar(sp.CloseBombCount + 48);
                        }
                    }
                }
            }
        }

        private static void Interface(Minefield minefield, int x, int y, int curX, int curY)
        {
            
            for(int i = 0; i < 50; i++)
            {
                Console.WriteLine();
            }
            for(int i = 0; i < y; i++)
            {
                for(int j = 0; j < x; j++)
                {
                    if(curX == j && curY == i)
                    {
                        Console.BackgroundColor = ConsoleColor.DarkGray;
                    }
                    if (minefield.Field[i][j].Visibility == true)
                    {
                        if (minefield.Field[i][j].CloseBombCount > 0)
                        {
                            Console.Write(minefield.Field[i][j].text);
                        }
                        else
                        {
                            Console.Write('_');
                        }
                    }
                    else
                    {
                        Console.Write('#');
                    }
                    Console.BackgroundColor = ConsoleColor.Black;
                    Console.Write(' ');
                }
                Console.WriteLine();
            }
            Console.WriteLine();
            Console.WriteLine(curX);
            Console.WriteLine(curY);

        }

        private static void UncoverEmpty(Minefield minefield, int CurX, int CurY, int xMax, int yMax)
        {
            List<Spot> ToGo = new List<Spot>();
            ToGo.Add(minefield.Field[CurX][CurY]);
            bool Start = true;
            while (Start == true)
            {
                List <Spot> Mem = new List <Spot>();
                foreach (Spot sp in ToGo)
                {
                    sp.Visibility = true;
                    sp.text = '_';
                    if (minefield.Field[sp.y][Math.Abs(sp.x - 1)].CloseBombCount == 0)
                    {
                        if(Mem.Contains(minefield.Field[sp.y][Math.Abs(sp.x - 1)]) ==  false && minefield.Field[sp.y][Math.Abs(sp.x - 1)].Visibility == false)
                        {
                            Mem.Add(minefield.Field[sp.y][Math.Abs(sp.x - 1)]);
                        }
                    }
                    if (minefield.Field[Math.Abs(sp.y - 1)][sp.x].CloseBombCount == 0)
                    {
                        if (Mem.Contains(minefield.Field[Math.Abs(sp.y - 1)][sp.x]) == false && minefield.Field[Math.Abs(sp.y - 1)][sp.x].Visibility == false)
                        {
                            Mem.Add(minefield.Field[Math.Abs(sp.y - 1)][sp.x]);
                        }
                    }

                    if (sp.x != xMax - 1){
                        if (minefield.Field[sp.y][sp.x + 1].CloseBombCount == 0)
                        {
                            if (Mem.Contains(minefield.Field[sp.y][sp.x + 1]) == false && minefield.Field[sp.y][sp.x + 1].Visibility == false)
                            {
                                Mem.Add(minefield.Field[sp.y][sp.x + 1]);
                            }
                        } 
                    }
                    if (sp.y != yMax - 1) 
                    {
                        if (minefield.Field[sp.y + 1][sp.x].CloseBombCount == 0)
                        {
                            if (Mem.Contains(minefield.Field[sp.y + 1][sp.x]) == false && minefield.Field[sp.y + 1][sp.x].Visibility == false)
                            {
                                Mem.Add(minefield.Field[sp.y + 1][sp.x]);
                            }
                        }
                    }
                }
                if (Mem.Count != 0)
                {
                    ToGo = Mem;
                }
                else
                {
                    Start = false;
                }
            }
        }

        private static void Restart()
        {
            Gamesense();
        }

        private static void SetupField(int x, int y, int bombCount, Minefield field)
        {
            Random rnd = new Random();
           

            for (int i = 0, u = 0; i < y; i++)
            { List<Spot> Mem = new List<Spot>();
                for (int j = 0; j < x; j++, u++)
                {
                    Spot spot = new Spot();
                    spot.x = j; spot.y = i;
                    spot.id = u;
                    field.Spots.Add(spot);
                    Mem.Add(spot);
                }
                field.Field.Add(Mem);
            }
            SetNeighbours(field, x, y);

            for(int i = 0; i < bombCount; i++)
            {
                Bomb boom = new Bomb();
                boom.x = rnd.Next(x - 1);
                boom.y = rnd.Next(y - 1);
                for(int j = 0;  j < bombCount; j++)
                {
                    if (field.Field[boom.y][boom.x].role == 0)
                    {
                        field.Field[boom.y][boom.x].role = 1;
                    }
                }
                Console.WriteLine(boom.x);
            Console.WriteLine(boom.y);
            }
            
            for (int i = 0; i < y; i++)
            {
                for(int j = 0;j < x; j++)
                {
                    if ((i > 0 && j > 0) && (i < y - 1 && j < x - 1))
                    {
                        for (int q = i - 1, p = 0; p < 3; q++, p++)
                        {
                            for (int w = j - 1, o = 0; o < 3; w++, o++)
                            {
                                if (field.Field[q][w].role == 1)
                                {
                                    field.Field[i][j].CloseBombCount++;
                                }
                            }
                        }
                    }
                    else if ((j == 0 && i == 0) || (j == x - 1 && i == y - 1) || (j == 0 && i == y - 1) || (j == x - 1 && i == 0))
                    {
                        if (j == 0 && i == 0)
                        {
                            for (int q = i, p = 0; p < 2; q++, p++)
                            {
                                for (int w = j, o = 0; o < 2; w++, o++)
                                {
                                    if (field.Field[q][w].role == 1)
                                    {
                                        field.Field[i][j].CloseBombCount++;
                                    }
                                }
                            }
                        }
                        else if (j == x - 1 && i == y - 1)
                        {
                            for (int q = i - 1, p = 0; p < 2; q++, p++)
                            {
                                for (int w = j - 1, o = 0; o < 2; w++, o++)
                                {
                                    if (field.Field[q][w].role == 1)
                                    {
                                        field.Field[i][j].CloseBombCount++;
                                    }
                                }
                            }
                        }
                        else if (j == 0 && i == y - 1)
                        {
                            for (int q = i - 1, p = 0; p < 2; q++, p++)
                            {
                                for (int w = j, o = 0; o < 2; w++, o++)
                                {
                                    if (field.Field[q][w].role == 1)
                                    {
                                        field.Field[i][j].CloseBombCount++;
                                    }
                                }
                            }
                        }
                        else if (j == x - 1 && i == 0)
                        {
                            for (int q = i, p = 0; p < 2; q++, p++)
                            {
                                for (int w = j - 1, o = 0; o < 2; w++, o++)
                                {
                                    if (field.Field[q][w].role == 1)
                                    {
                                        field.Field[i][j].CloseBombCount++;
                                    }
                                }
                            }
                        }
                    }
                    else if(j == 0 || j == x - 1)
                    {
                        if(j == 0)
                        {
                            for (int q = i - 1, p = 0; p < 3; q++, p++)
                            {
                                for (int w = j, o = 0; o < 2; w++, o++)
                                {
                                    if (field.Field[q][w].role == 1)
                                    {
                                        field.Field[i][j].CloseBombCount++;
                                    }
                                }
                            }
                        }
                        else
                        {
                            for (int q = i - 1, p = 0; p < 3; q++, p++)
                            {
                                for (int w = j - 1, o = 0; o < 2; w++, o++)
                                {
                                    if (field.Field[q][w].role == 1)
                                    {
                                        field.Field[i][j].CloseBombCount++;
                                    }
                                }
                            }
                        }
                    }
                    else if(i == 0 || i == y - 1)
                    {
                        if (i != 0)
                        {
                            for (int q = i - 1, p = 0; p < 2; q++, p++)
                            {
                                for (int w = j - 1, o = 0; o < 3; w++, o++)
                                {
                                    if (field.Field[q][w].role == 1)
                                    {
                                        field.Field[i][j].CloseBombCount++;
                                    }
                                }
                            }
                        }
                        else
                        {
                            for (int q = i, p = 0; p < 2; q++, p++)
                            {
                                for (int w = j - 1, o = 0; o < 3; w++, o++)
                                {
                                    if (field.Field[q][w].role == 1)
                                    {
                                        field.Field[i][j].CloseBombCount++;
                                    }
                                }
                            }
                        }
                    }
                }
                
            }
        }

        private static void SetNeighbours(Minefield field, int x, int y)
        {
            for (int i = 0; i < y; i++)
            {
                for (int j = 0; j < x; j++)
                {
                    if(i != y - 1 && j != x - 1)
                    {
                        for(int k = Math.Abs(i - 1), q = 0; q < 3; q++, k++)
                        {
                            for(int l = Math.Abs(j - 1), r = 0; r < 3; r++, l++)
                            {
                                if (field.Field[i][j].Neighbours.Contains(field.Field[k][l]) == false && field.Field[i][j] != field.Field[k][l])
                                {
                                    field.Field[i][j].Neighbours.Add(field.Field[k][l]);
                                }
                            }
                        }
                    }
                    if(i == y - 1 && j != x - 1)
                    {
                        for (int k = Math.Abs(i - 1), q = 0; q < 2; q++, k++)
                        {
                            for (int l = Math.Abs(j - 1), r = 0; r < 3; r++, l++)
                            {
                                if (field.Field[i][j].Neighbours.Contains(field.Field[k][l]) == false && field.Field[i][j] != field.Field[k][l])
                                {
                                    field.Field[i][j].Neighbours.Add(field.Field[k][l]);
                                }
                            }
                        }
                    }
                    if(j == x - 1 && i != y - 1)
                    {
                        for (int k = Math.Abs(i - 1), q = 0; q < 3; q++, k++)
                        {
                            for (int l = Math.Abs(j - 1), r = 0; r < 2; r++, l++)
                            {
                                if (field.Field[i][j].Neighbours.Contains(field.Field[k][l]) == false && field.Field[i][j] != field.Field[k][l])
                                {
                                    field.Field[i][j].Neighbours.Add(field.Field[k][l]);
                                }
                            }
                        }
                    }
                    if(j == y - 1 && i == x - 1)
                    {

                        for (int k = Math.Abs(i - 1), q = 0; q < 2; q++, k++)
                        {
                            for (int l = Math.Abs(j - 1), r = 0; r < 2; r++, l++)
                            {
                                if (field.Field[i][j].Neighbours.Contains(field.Field[k][l]) == false && field.Field[i][j] != field.Field[k][l])
                                {
                                    field.Field[i][j].Neighbours.Add(field.Field[k][l]);
                                }
                            }
                        }
                    }
                }
            }
        }
    }
}
