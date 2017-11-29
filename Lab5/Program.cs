using System;
using Lab5.Models;
using Microsoft.EntityFrameworkCore;

namespace Lab5
{
    class Program
    {
        private static void Audit(Lab5Context context)
        {
            var User = context.Users.Find(3);

           context.Database.ExecuteSqlCommand("UPDATE Users SET Name = Name + '_DB' WHERE Id = '3'");
 
            // Change the current value in memory
            User.Name = User.Name + "_Memory";

            string value, original;
            original = value = context.Entry(User).Property(m => m.Name).OriginalValue;
            Console.WriteLine(string.Format("Original Value : {0}", value));

            value = context.Entry(User).Property(m => m.Name).CurrentValue;
            Console.WriteLine(string.Format("Current Value : {0}", value));

            value = context.Entry(User).GetDatabaseValues().GetValue<string>("Name");
            Console.WriteLine(string.Format("DB Value : {0}", value));

            // Returning database values to original
            context.Database.ExecuteSqlCommand("UPDATE Users SET Name = '" + original + "' WHERE Id = 3");

            Console.ReadKey();
        }

        static void Main(string[] args)
        {
            using(var context = new Lab5Context())
            {
                DbInitializer.Initialize(context);
                Audit(context);
            }      
        }
    }
}
