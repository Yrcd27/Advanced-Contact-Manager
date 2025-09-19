using System;
using System.Data;

public class addC
{

    
    public void display(ref string op, ref Class2D contacts)  // display the AddCon screen
    {
        
        UI uI2= new UI();
        uI2.ClearScreen();
        uI2.ShowLoadingAnimation("Loading....", 1);uI2.ClearScreen();
        uI2.line();
        uI2.Center("ADD NEW CONTACTS");
        uI2.line();
        Console.WriteLine("");Console.WriteLine(""); Console.WriteLine("");Console.WriteLine("");Console.WriteLine(""); Console.WriteLine("");

                        Console.Write("-Enter Name           : ");
                        string name = Console.ReadLine();
                        Console.Write("-Enter Phone Number   : ");
                        string phone = Console.ReadLine();
                        Console.Write("-Enter Group          : ");
                        string group = Console.ReadLine();
                        Console.Write("-Enter City           : ");
                        string city = Console.ReadLine();

                        contacts.Add(name, phone, group, city);
                        Console.WriteLine("Contact added successfully!");


        
        Console.WriteLine("");Console.WriteLine(""); Console.WriteLine("");Console.WriteLine("");Console.WriteLine(""); Console.WriteLine("");
        
        Console.WriteLine("");Console.WriteLine(""); Console.WriteLine("");Console.WriteLine("");Console.WriteLine(""); Console.WriteLine("");

        uI2.line();
        List<string> tems = new List<string> {  "Exit : 0"," Add contact : 1", "Save : 2",  "Switch to home page : 3","View all : 4" };
        uI2.ListBar(5, tems);
        
        uI2.line();
        
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
           contacts.export();
           firstScreen home = new firstScreen();
            home.display(ref op);
            home.menu(ref op, ref contacts);
        }
        else if(op=="3"){
           firstScreen home = new firstScreen();
            home.display(ref op);
            home.menu(ref op, ref contacts);
        }
        else if(op=="4"){
           viewOp viewOp = new viewOp();
             viewOp.display(ref op, ref contacts);
            viewAll viewAll = new viewAll();
            viewAll.display(ref op, ref contacts);
        }
    }
}