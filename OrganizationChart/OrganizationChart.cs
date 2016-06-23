using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrganizationChart
{
    public class OrganizationChart
    {
        public Employee Manager { get; set; }
        private Queue<Employee> Employees;

        public OrganizationChart()
        {
            Manager = new Employee("Organization Chart");
            Employees = new Queue<Employee>();
        }

        public string PrintTreeView()
        {
            if (Manager == null)
            {
                return string.Empty;
            }

            StringBuilder tree = new StringBuilder();

            Employees.Enqueue(Manager);

            int spacing = 0;
            Employee currentManager = null;

            while (Employees.Count > 0)
            {
                Employee employee = Employees.Dequeue();

                if (employee.Manager != null && !employee.Manager.IsPeer(currentManager))
                {
                    spacing = 0;
                    currentManager = employee.Manager;
                    tree.AppendLine();
                }
                                                          //Remover depois. Efeito cosmetico        
                spacing += CalculateSpacing(employee) / 2 - employee.Name.Length / 2;

                tree.AppendFormat("{0}{1}", MakeIdentation(spacing), employee.Name);
                
                foreach (var subordinate in employee.Subordinates)
                {
                    subordinate.Manager = employee;
                    Employees.Enqueue(subordinate);
                }
                
            }

            return tree.ToString() ;
        }

        private string MakeIdentation(int identationLength)
        {
            string identation = " ";

            for (int i = 0; i < identationLength; i++)
            {
                identation += " ";
            }

            return identation;
        }

        public int CalculateSpacing(Employee employee)
        {
            int subordinateCount = employee.Subordinates.Count;
            int width = employee.Name.Length;

            for (int i = 0; i < subordinateCount; i++)
            {
                var subordinate = employee.Subordinates[i];
                width += CalculateSpacing(subordinate);
            }

            return width < 0 ? 0 : width;
        }

        private string PrintSubTreeView(Employee employee, string identation)
        {
            int subordinateCount = employee.Subordinates.Count;

            StringBuilder subTree = new StringBuilder();

            subTree.AppendLine(identation + employee.Name);

            for (int i = 0; i < subordinateCount; i++)
            {
                var subordinate = employee.Subordinates[i];
                subTree.Append(PrintSubTreeView(subordinate, identation + "   "));
            }

            return subTree.ToString();
        }
    }

    public class Employee
    {
        public Employee(string name)
        {
            this.Name = name;
            this.Subordinates = new List<Employee>();
        }

        public string Name { get; set; }
        public Employee Manager { get; set; }
        public List<Employee> Subordinates { get; set; }
        public bool IsManager { get; private set; }
        public bool HasSubordinates { get { return Subordinates != null && Subordinates.Count > 0; } }

        public void AddSubordinate(Employee employee)
        {
            if(IsManager == false)
            {
                IsManager = true;
            }

            Subordinates.Add(employee);
        }

        public bool IsPeer(Employee employee)
        {
            if(employee == null)
            {
                return false;
            }

            return this.Manager == employee.Manager;
        }

    }

}
