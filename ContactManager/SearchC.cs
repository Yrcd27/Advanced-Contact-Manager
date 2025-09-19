using System;
using System.Data;

public class SearchC
{

    
    public void display(ref string op,ref Class2D contacts )
    {
        
        UI uI1= new UI();
        uI1.ClearScreen();
        //uI1.ShowLoadingAnimation("Loading....", 2);uI1.ClearScreen();
        uI1.line();
        uI1.Center("SEARCH CONTACT");
        uI1.line();
        Console.WriteLine("");Console.WriteLine(""); Console.WriteLine("");Console.WriteLine("");Console.WriteLine(""); Console.WriteLine("");

        List<string> items = new List<string> { "- Search by name         ......................................... 1", 
                                                "- Search by number       ......................................... 2", 
                                                "- Search by group        ......................................... 3",
                                                "- Search by city         ......................................... 4",
                                                
                                                 };
        uI1.ListMaker(4, items);


        
        Console.WriteLine("");Console.WriteLine(""); Console.WriteLine("");Console.WriteLine("");Console.WriteLine(""); Console.WriteLine("");
        
        Console.WriteLine("");Console.WriteLine(""); Console.WriteLine("");Console.WriteLine("");Console.WriteLine(""); Console.WriteLine("");

        uI1.line();
        List<string> tems = new List<string> { "Quite    : 0 " };
        uI1.ListBar(1, tems);
        
        uI1.line();
        
     op = Console.ReadLine();

    }

    public void menu(ref string op, ref Class2D contacts){
        if(op=="0"){
            Environment.Exit(0);
        }
        else if(op=="1"){
            searchOp searchOp = new searchOp();
            searchOp.display(ref op);
            SearchByName searchByName = new SearchByName();
            searchByName.display(ref op, ref contacts);
            searchByName.menu(ref op, ref contacts);
            
        }
        else if(op=="2"){
            SearchByNumber searchByNumber = new SearchByNumber();
            searchByNumber.display(ref op, ref contacts);
            searchByNumber.menu(ref op, ref contacts);
        }
        else if(op=="3"){
            searchOp searchOp = new searchOp();
            searchOp.display(ref op);
            SearchByGroup searchByGroup = new SearchByGroup();
            searchByGroup.display(ref op, ref contacts);
            searchByGroup.menu(ref op, ref contacts);
            
        }
        else if(op=="4"){
            searchOp searchOp = new searchOp();
            searchOp.display(ref op);
            SearchByCity searchByCity = new SearchByCity();
            searchByCity.display(ref op, ref contacts);
            searchByCity.menu(ref op, ref contacts);
          
        }
    }
}