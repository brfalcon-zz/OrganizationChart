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

            Employee felix = new Employee("aaaaa");
            Employee x = new Employee("bbbbb");
            Employee x1 = new Employee("ccccc");
            Employee x2 = new Employee("ddddd");
            Employee rubiana = new Employee("eeeee");
            Employee vinicius = new Employee("fffff");
            Employee vinicius1 = new Employee("ggggg");
            Employee iury = new Employee("hhhhh");

            chart.Manager = felix;

            felix.AddSubordinate(rubiana);
            felix.AddSubordinate(x);
            x.AddSubordinate(x1);
            x.AddSubordinate(x2);
            rubiana.AddSubordinate(vinicius);
            rubiana.AddSubordinate(iury);
            vinicius.AddSubordinate(vinicius1);
            vinicius.AddSubordinate(vinicius1);

            Console.WriteLine(chart.PrintTreeView());
            Console.Read();
        }
    }
}
