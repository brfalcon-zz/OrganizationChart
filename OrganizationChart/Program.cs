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
            //OrganizationChart chart = new OrganizationChart();

            //Employee nedStark = new Employee("NedStark");
            //Employee johnSnow = new Employee("JohnSnow");
            //Employee sansaStark = new Employee("Sansa");
            //Employee aryaStark = new Employee("Arya");
            //Employee bran = new Employee("Bran");
            //Employee rickon = new Employee("Rickon");
            //Employee hodor = new Employee("Hodor");
            //Employee meera = new Employee("Meera");

            //chart.Manager = nedStark;

            //nedStark.AddSubordinate(bran);
            //nedStark.AddSubordinate(johnSnow);
            //nedStark.AddSubordinate(aryaStark);

            //johnSnow.AddSubordinate(sansaStark);

            //bran.AddSubordinate(rickon);
            //bran.AddSubordinate(meera);
            //bran.AddSubordinate(hodor);


            //Console.WriteLine(chart.ToString());
            //Console.Read();

            Tree organograma = new Tree();

            TreeNode o = new TreeNode("O");
            TreeNode e = new TreeNode("E");
            TreeNode f = new TreeNode("F");
            TreeNode n = new TreeNode("N");
            TreeNode a = new TreeNode("A");
            TreeNode d = new TreeNode("D");
            TreeNode g = new TreeNode("G");
            TreeNode m = new TreeNode("M");
            TreeNode b = new TreeNode("B");
            TreeNode c = new TreeNode("C");
            TreeNode h = new TreeNode("H");
            TreeNode i = new TreeNode("I");
            TreeNode j = new TreeNode("J");
            TreeNode k = new TreeNode("K");
            TreeNode l = new TreeNode("L");

            o.Offspring.Add(e);
            o.Offspring.Add(f);
            o.Offspring.Add(n);

            e.Offspring.Add(a);
            e.Offspring.Add(d);

            n.Offspring.Add(g);
            n.Offspring.Add(m);

            d.Offspring.Add(b);
            d.Offspring.Add(c);

            m.Offspring.Add(h);
            m.Offspring.Add(i);
            m.Offspring.Add(j);
            m.Offspring.Add(k);
            m.Offspring.Add(l);

            foreach(var node in new TreeNode[] { o , e ,n, m })
            {
                TreeNode lastChild = null;
                foreach (var child in node.Offspring)
                {
                    child.Parent = o;

                    if (lastChild != null)
                    {
                        child.LeftSibling = lastChild;
                        lastChild.RightSbling = child;
                    }

                    lastChild = child;
                }
            }

            if (organograma.TreePosition(o))
            {
                Console.WriteLine("Deu certo!");
            }
            else
            {
                Console.WriteLine("Nao Deu certo!");
            }

            Console.Read();
        }
    }
}
