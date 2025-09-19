using System;
using System.Data;

public class viewOp
{

    
    public void display(ref string op, ref Class2D contacts)  // display the AddCon screen
    {
        
        UI uI1= new UI();
        uI1.ClearScreen();
        uI1.ShowLoadingAnimation("Loading....", 1);uI1.ClearScreen();
        uI1.line();
        uI1.Center("SORT CONTACTS");
        uI1.line();
        Console.WriteLine("");Console.WriteLine(""); Console.WriteLine("");Console.WriteLine("");Console.WriteLine(""); Console.WriteLine("");

        List<string> soulist = new List<string> { "-View unsorted contact list     ......................................... 1",
                                                  "-Sort with bubble sort          ......................................... 2",
                                                  "-Sort with insertion sort       ......................................... 3",
                                                  "-Sort with selection sort       ......................................... 4",
                                                  "-Sort with merge sort           ......................................... 5",
                                                 "-Sort with quick sort           ......................................... 6"
                                                   };
        uI1.ListMaker(6, soulist);


        Console.WriteLine("");Console.WriteLine(""); Console.WriteLine("");Console.WriteLine("");Console.WriteLine(""); Console.WriteLine("");
        
        Console.WriteLine("");Console.WriteLine(""); Console.WriteLine("");Console.WriteLine("");Console.WriteLine(""); Console.WriteLine("");

        uI1.line();
        Console.WriteLine("");
        
        uI1.line();
        
     op = Console.ReadLine();

    }

}

