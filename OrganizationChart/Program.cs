using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrganizationChart
{
    class Program
    {
        static void Main(string[] args)
        {
            OrganizationChart chart = new OrganizationChart();

            Employee nedStark = new Employee("NedStark");
            Employee johnSnow = new Employee("JohnSnow");
            Employee sansaStark = new Employee("Sansa");
            Employee aryaStark = new Employee("Arya");
            Employee bran = new Employee("Bran");
            Employee rickon = new Employee("Rickon");
            Employee hodor = new Employee("Hodor");
            Employee meera = new Employee("Meera");

            chart.Manager = nedStark;

            nedStark.AddSubordinate(bran);
            nedStark.AddSubordinate(johnSnow);
            nedStark.AddSubordinate(aryaStark);

            johnSnow.AddSubordinate(sansaStark);
            
            bran.AddSubordinate(rickon);
            bran.AddSubordinate(meera);
            bran.AddSubordinate(hodor);
            

            Console.WriteLine(chart.ToString());
            Console.Read();
        }
    }
}
