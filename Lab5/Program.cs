using System.Configuration;
using Lab5.Models;

namespace Lab5
{
    class Program
    {
        private static void Audit()
        {
           
        }

        static void Main(string[] args)
        {
            using(var context = new Lab5Context())
            {
                DbInitializer.Initialize(context);
                Audit();
            }      
        }
    }
}
