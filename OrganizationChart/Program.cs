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

            var nodes = new TreeNode[] { o, e, n, d, m };
            
            foreach (var node in nodes)
            {
                TreeNode lastChild = null;
                foreach (var child in node.Offspring)
                {
                    child.Parent = node;

                    if (lastChild != null)
                    {
                        child.LeftSibling = lastChild;
                        lastChild.RightSibling = child;
                    }

                    lastChild = child;
                }
            }

            if (organograma.TreePosition(o))
            {
                PrintTree(o, Console.WindowWidth / 2);
            }
            else
            {
                Console.WriteLine("Nao Deu certo!");
            }
            
            Console.Read();
        }

        static void PrintTree(TreeNode apexNode, int offset)
        {
            if(apexNode != null)
            {
                //Console.SetCursorPosition((int) Math.Round(apexNode.XCoordinate + offset), (int) Math.Round(apexNode.YCoordinate));
                //Console.Write(apexNode.Info);
                
                Console.WriteLine(apexNode.Info + " (" + apexNode.XCoordinate + ", " + apexNode.YCoordinate + ")");

                if (apexNode.HasRightSibling)
                {
                    PrintTree(apexNode.RightSibling, offset);
                }

                if (apexNode.HasChild)
                {
                    PrintTree(apexNode.FirstChild, offset);
                }
            }
        }
    }
}
