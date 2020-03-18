using System;
using System.Collections.Generic;
using System.Linq;

namespace Chat.Common
{
    public class ThreadSafeRoom
    {
        List<Individual> persons;

        private Object thisLock = new Object();

        public ThreadSafeRoom()
        {
            persons = new List<Individual>();
        }

        public List<Individual> GetListExcept(Individual pers)
        {
            lock (thisLock)
            {
                return pers == null
                ? persons
                : persons.Where(p => (p.Name != pers.Name)).ToList();
            }
        }

        public void Join(Individual person)
        {
            lock (thisLock)
            {
                persons.Add(person);
            }
        }

        public void Leave(string personName)
        {
            lock (thisLock)
            {
                var itemToRemove = persons.Single(r => r.Name.Equals(personName));
                this.persons.Remove(itemToRemove);
            }
        }

        public bool IsOnline(string personName)
        {
            lock (thisLock)
            {
                return persons.Exists(x => (x.Name != null && x.Name == personName));
            }
        }

        public int Count()
        {
            lock (thisLock)
            {
                return persons.Count;
            }
        }

        public List<Individual> SortPersonsList()
            => persons.OrderBy(n => n.Name).ToList();

        public string ConvertToString()
        {
            if (persons.Count == 0)
                return "";
            string res = "";
            foreach (Individual pers in SortPersonsList())
                res += pers.Name.Trim() + ",";
            return res.Remove(res.Length - 1, 1);
        }
    }
}