using System;
using System.Data;

public class deleteC
{
    public void display(ref string op, ref Class2D contacts)  
    {

        UI uI2 = new UI();
        uI2.ClearScreen();
        uI2.ShowLoadingAnimation("Loading....", 1); uI2.ClearScreen();
        uI2.line();
        uI2.Center("DELETE CONTACT");
        uI2.line();
        Console.WriteLine(""); Console.WriteLine(""); Console.WriteLine(""); Console.WriteLine(""); Console.WriteLine(""); Console.WriteLine("");

                        Console.Write("-Enter NAME        : ");
                        string city = Console.ReadLine();
        
       contacts.delete(city);
       Console.WriteLine("Contact Deleted Successfully");
    

      

        Console.WriteLine(""); Console.WriteLine(""); Console.WriteLine(""); Console.WriteLine(""); Console.WriteLine(""); Console.WriteLine("");

        Console.WriteLine(""); Console.WriteLine(""); Console.WriteLine(""); Console.WriteLine(""); Console.WriteLine(""); Console.WriteLine("");

        uI2.line();
        List<string> tems = new List<string> {  "Exit : 0"," Add contact : 1", "Save : 2",  "Switch to home page : 3" };
        uI2.ListBar(4,   tems);
        
        uI2.line();
        op = Console.ReadLine();

    }

    public void menu(ref string op, ref Class2D contacts)
    {
        if (op == "0")
        {
            Environment.Exit(0);
        }
        else if (op == "1")
        {
            addC addC = new addC();
            addC.display(ref op, ref contacts);
            addC.menu(ref op, ref contacts);
        }
        else if (op == "2")
        {
           contacts.export();
           firstScreen home = new firstScreen();
           home.display(ref op);
              home.menu(ref op, ref contacts);
        }
        else if (op == "3")
        {
            firstScreen home = new firstScreen();
            home.display(ref op);
            home.menu(ref op, ref contacts);
        }
    }

}