using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;

namespace Day7
{
    static class Program
    {
        static void Main(string[] args)
        {
            string path = "C:\\Users\\richard\\source\\repos\\AdventOfCode2020\\Day7\\input.txt";
            List<Bag> myBags = CreateBags(path);
            string bagColor = "shiny gold";
            int shinyGoldIndex = myBags.FindIndex(e => e.GetColor() == bagColor);
            List<string> genealogy = AllParentList(myBags, bagColor);
            int sumChildrenBags = CountChildrenBags(myBags, bagColor);
            Console.WriteLine("unique parents : " + genealogy.Distinct().Count());
            Console.WriteLine("Total Children Bags : " + sumChildrenBags);


        }

        private static List<string> AllParentList(List<Bag> myBags,string myBagColor)
        {
            int bagIndex = myBags.FindIndex(e => e.GetColor() == myBagColor);
            List<String> parentList = myBags[bagIndex].GetParents();
            List<String> newParents = new List<string>();
            foreach (string parent in parentList)
            {
                newParents.AddRange(AllParentList(myBags, parent));
            }
            parentList.AddRange(newParents);
            return (parentList);
        }

        private static int CountChildrenBags(List<Bag> myBags, string myBagColor)
        {
            int bagIndex = myBags.FindIndex(e => e.GetColor() == myBagColor);
            List<Tuple<string, int>> childrenList = myBags[bagIndex].GetChildren();
            int childrenBagsCount = 0;
            foreach(Tuple<string, int> child in childrenList)
            {
                childrenBagsCount += child.Item2;
                childrenBagsCount += child.Item2*CountChildrenBags(myBags, child.Item1);
            }
            return childrenBagsCount;
        }
        private static List<Bag> CreateBags(string path)
        {
            IEnumerable<String> rules = File.ReadAllLines(@path);
            List<string[]> parsedRules = new List<string[]>();
            List<Bag> newBags = new List<Bag>();
            foreach (string rule in rules)
            {
                string color = rule.Substring(0, rule.IndexOf(" bags"));
                int bagIndex;
                Bag currentBag = new Bag(color);
                if (!newBags.Contains(currentBag))
                {
                    newBags.Add(currentBag);

                }
                bagIndex = newBags.FindIndex(e => e.GetColor() == currentBag.GetColor());

                string children = rule.Substring(rule.IndexOf("contain") + 8, rule.Length - (rule.IndexOf("contain") + 8));
                foreach (string child in children.Split(", "))
                {
                    int qty = 0;
                    string childcolor = "";
                    if (!child.Contains("no other"))
                    {
                        qty = int.Parse(child.Substring(0, child.IndexOf(' ')));
                        childcolor = child.Substring(child.IndexOf(' ') + 1, child.IndexOf("bag") - 3).Trim();
                        newBags[bagIndex].AddChild(childcolor, qty);
                        Bag childBag = new Bag(childcolor);
                        if (newBags.Contains(childBag))
                        {
                            newBags[newBags.FindIndex(e => e.GetColor() == childcolor)].AddParent(color);
                        }
                        else
                        {
                            newBags.Add(childBag);
                            newBags.Last().AddParent(color);
                        }
                    }
                }


            }
            return newBags;
        }
    }
    public class Bag : IEquatable<Bag>
    {
        private string color;
        private List<string> parentBags;
        private List<Tuple<string, int>> childrenBags;

        public Bag(string color)
        {
            this.color = color;
            childrenBags = new List<Tuple<string, int>>();
            parentBags = new List<string>();
        }
        public override string ToString()
        {
            return color;
        }

        public string GetColor()
        {
            return color;
        }
        public List<Tuple<string,int>> GetChildren()
        {
            return childrenBags;
        }
        public List<string> GetParents()
        {
            return (parentBags);
        }
        public bool Equals(Bag other)
        {
            if (this.color == other.color)
                return true;
            else
                return false;
        }
        public void AddParent(string parentColor)
        {
            if (!parentBags.Contains(parentColor))
            {
                parentBags.Add(parentColor);
            }
        }

        public void AddChild(string color, int qty)
        {
            Tuple<string, int> childBag = new Tuple<string, int>(color, qty);
            if (!childrenBags.Contains(childBag))
            {
                childrenBags.Add(childBag);
            }
        }

    }
}
