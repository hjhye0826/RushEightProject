using RushEightProject;
using System.Diagnostics;

public class Program
{
    public static void Main()
    {
        //Console.WriteLine("Hello World");

        StringTable.InitStringTable();

        var subway = new SubwayManager();
        subway.InitSubwayInfo();

        while (true)
        {
            subway.SearchSubway();

            Console.WriteLine("-----------------------------------------------");
        }
    }
}