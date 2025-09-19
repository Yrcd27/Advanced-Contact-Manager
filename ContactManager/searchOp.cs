using System;
using System.Data;

public class searchOp
{

    
    public void display(ref string op )
    {
        
        UI uI1= new UI();
        uI1.ClearScreen();
        uI1.ShowLoadingAnimation("Loading....", 1 );
        uI1.ClearScreen();
        uI1.line();
        uI1.logo();
        uI1.line();
        Console.WriteLine("");Console.WriteLine(""); Console.WriteLine("");Console.WriteLine("");Console.WriteLine(""); Console.WriteLine("");

        List<string> items = new List<string> { "-Linear Search     ......................................... 1",
                                                "-Binary Search     ......................................... 2",
                                                 };
        uI1.ListMaker(2 , items);





        
        Console.WriteLine("");Console.WriteLine(""); Console.WriteLine("");Console.WriteLine("");Console.WriteLine(""); Console.WriteLine("");
        
        Console.WriteLine("");Console.WriteLine(""); Console.WriteLine("");Console.WriteLine("");Console.WriteLine(""); Console.WriteLine("");

        uI1.line();
        List<string> tems = new List<string> { "Quite    : 0 " };
        uI1.ListBar(1, tems);
        
        uI1.line();
        
     op = Console.ReadLine();
     if(op == "0")
     {
         Environment.Exit(0);
     }

    }

   
}