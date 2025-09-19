using System;
using System.Data;

public class firstScreen
{

    
    public void display(ref string op )
    {
        
        UI uI1= new UI();
        uI1.ClearScreen();
        uI1.ShowLoadingAnimation("Loading....", 1);uI1.ClearScreen();
        uI1.line();
        uI1.logo();
        uI1.line();
        Console.WriteLine("");Console.WriteLine(""); Console.WriteLine("");Console.WriteLine("");Console.WriteLine(""); Console.WriteLine("");

        List<string> items = new List<string> { "- Add contact     ......................................... 1", 
                                                "- Search contact  ......................................... 2", 
                                                "- View all        ......................................... 3",
                                                "- Modify contact  ......................................... 4",
                                                "- Delete contact  ......................................... 5",
                                                "- Import data     ......................................... 6",
                                                "- Export data     ......................................... 7"
                                                 };
        uI1.ListMaker(7, items);





        
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
          addC addC = new addC();
          addC.display(ref op, ref contacts);
          addC.menu(ref op, ref contacts);
        }
        else if(op=="2"){
            SearchC searchC = new SearchC();
            searchC.display(ref op, ref contacts);
            searchC.menu(ref op, ref contacts);
        }
        else if(op=="3"){
           viewOp viewOp = new viewOp();
             viewOp.display(ref op, ref contacts);
            viewAll viewAll = new viewAll();
            viewAll.display(ref op, ref contacts);
        }
        else if(op=="4"){
          modifyC modifyC = new modifyC();
            modifyC.display(ref op, ref contacts);
            modifyC.menu(ref op, ref contacts); 
        }
        else if(op=="5"){
           deleteC deleteC1 = new deleteC();
            deleteC1.display(ref op, ref contacts);
            deleteC1.menu(ref op, ref contacts);
        }
        else if(op=="6"){
           contacts.load_data_fromtxt();
           firstScreen home = new firstScreen();
            home.display(ref op);
            home.menu(ref op, ref contacts);    
        }
        else if(op=="7"){
           contacts.export();
           firstScreen home = new firstScreen();
            home.display(ref op);
            home.menu(ref op, ref contacts);
            

        }
    }

}