using System;
using System.Data;

public class viewAll
{

    
    public void display(ref string op, ref Class2D contacts)  // display the AddCon screen
    {
        
        UI uI1= new UI();
        uI1.ClearScreen();
        uI1.ShowLoadingAnimation("Loading....", 1);uI1.ClearScreen();
        uI1.line();
        uI1.Center("VIEW CONTACTS");
        uI1.line();
        Console.WriteLine("");Console.WriteLine(""); Console.WriteLine("");Console.WriteLine("");Console.WriteLine(""); Console.WriteLine("");

        if(op=="1"){
            contacts.Print();
        }
        else if(op=="2"){
            contacts.BubbleSort();
            contacts.Print();
            
        }
        else if(op=="3"){
            contacts.InsertionSort();
            contacts.Print();
            
        }
        else if(op=="4"){
            contacts.SelectionSort();
            contacts.Print();
        }
        else if(op=="5"){
            contacts.MergeSort();
            contacts.Print();
            
        }
        else if(op=="6"){
            contacts.QuickSort();
            contacts.Print();
            
        }
        else{
            Console.WriteLine("Invalid input");
        }

        Console.WriteLine("");Console.WriteLine(""); Console.WriteLine("");Console.WriteLine("");Console.WriteLine(""); Console.WriteLine("");
        
        Console.WriteLine("");Console.WriteLine(""); Console.WriteLine("");Console.WriteLine("");Console.WriteLine(""); Console.WriteLine("");

        uI1.line();
        List<string> tems = new List<string> { "Search contacts : 1 "  , "Switch to home page : 2 ",  " Exit : 3   " };
        uI1.ListBar(3, tems);
        
        uI1.line();
        
    op = Console.ReadLine();
    viewAll viewAll = new viewAll();
    viewAll.menu(ref op, ref contacts);

    }

    public void menu(ref string op, ref Class2D contacts){
        if(op=="1"){
            searchOp searchOp = new searchOp();
            searchOp.display(ref op);
            SearchC searchC = new SearchC();
            searchC.display(ref op, ref contacts);
            searchC.menu(ref op, ref contacts); 
        }
        else if(op=="2"){
            firstScreen home = new firstScreen();
            home.display(ref op);
            home.menu(ref op, ref contacts);
        }
        else if(op=="3"){
            Environment.Exit(0);
        }
    }
}