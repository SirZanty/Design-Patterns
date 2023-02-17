using DesignPattersUdemy.Section1;

namespace DesignPattersUdemy;

public class Program
{
    public static void Main(string[] args)
    {
        SingleResponsibility3 singleResponsibility3 = new SingleResponsibility3();
        singleResponsibility3.Run();
        OpenClosed4 openClosed4 = new OpenClosed4();
        openClosed4.Run();        
    }
}