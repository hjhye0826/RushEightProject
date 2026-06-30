using RushEightProject;

public class Program
{
    public static void Main()
    {
        //Console.WriteLine("Hello World");

        var subway = new SubwayManager();
        subway.InitSubwayInfo();
        subway.SearchSubway();
    }
}