// Skeleton implementation written by Joe Zachary for CS 3500, January 2015.
// Revised for CS 3500 by Joe Zachary, January 29, 2016
// Method and Class implementations by Henry Kucab
// Last Updated 2/11/2016

using System;
using System.Linq;
using System.Collections.Generic;

namespace Dependencies
{
    /// <summary>
    /// A DependencyGraph can be modeled as a set of dependencies, where a dependency is an ordered 
    /// pair of strings.  Two dependencies (s1,t1) and (s2,t2) are considered equal if and only if 
    /// s1 equals s2 and t1 equals t2.
    /// 
    /// Given a DependencyGraph DG:
    /// 
    ///    (1) If s is a string, the set of all strings t such that the dependency (s,t) is in DG 
    ///    is called the dependents of s, which we will denote as dependents(s).
    ///        
    ///    (2) If t is a string, the set of all strings s such that the dependency (s,t) is in DG 
    ///    is called the dependees of t, which we will denote as dependees(t).
    ///    
    /// The notations dependents(s) and dependees(s) are used in the specification of the methods of this class.
    ///
    /// For example, suppose DG = {("a", "b"), ("a", "c"), ("b", "d"), ("d", "d")}
    ///     dependents("a") = {"b", "c"}
    ///     dependents("b") = {"d"}
    ///     dependents("c") = {}
    ///     dependents("d") = {"d"}
    ///     dependees("a") = {}
    ///     dependees("b") = {"a"}
    ///     dependees("c") = {"a"}
    ///     dependees("d") = {"b", "d"}
    ///     
    /// All of the methods below require their string parameters to be non-null.  This means that 
    /// the behavior of the method is undefined when a string parameter is null.  
    ///
    /// IMPORTANT IMPLEMENTATION NOTE
    /// 
    /// The simplest way to describe a DependencyGraph and its methods is as a set of dependencies, 
    /// as discussed above.
    /// 
    /// However, physically representing a DependencyGraph as, say, a set of ordered pairs will not
    /// yield an acceptably efficient representation.  DO NOT USE SUCH A REPRESENTATION.
    /// 
    /// You'll need to be more clever than that.  Design a representation that is both easy to work
    /// with as well acceptably efficient according to the guidelines in the PS3 writeup. Some of
    /// the test cases with which you will be graded will create massive DependencyGraphs.  If you
    /// build an inefficient DependencyGraph this week, you will be regretting it for the next month.
    /// </summary>
    public class DependencyGraph
    {
        /// <summary>
        /// 2 Dictionarys, dependentGraph and dependeeGraph. Each maps strings to hashsets, with the 
        /// hashsets holding the dependents and dependees respectively, under the opposite as a string. 
        /// DependentGraph maps string dependees to Hashsets of their string dependents.
        /// DependeeGraph maps string dependents to Hashsets of their string dependees.
        /// These together form the representation of the DependencyGraph Class
        /// </summary>
        Dictionary<string,HashSet<string>> dependentGraph;
        Dictionary<string, HashSet<string>> dependeeGraph;
        /// <summary>
        /// Creates a DependencyGraph containing no dependencies.
        /// </summary>
        public DependencyGraph()
        {
            dependentGraph = new Dictionary<string, HashSet<String>>();
            dependeeGraph = new Dictionary<string, HashSet<String>>();
        }
        /// <summary>
        /// Copy constructor for dependency graph. Takes in d1 dictionary, returns d2 as an exact copy of d1. 
        /// </summary>
        /// <param name="d1"></param>
        public DependencyGraph(DependencyGraph d1)
        {
            dependentGraph = new Dictionary<string, HashSet<String>>();
            dependeeGraph = new Dictionary<string, HashSet<string>>();
            foreach(KeyValuePair<string,HashSet<string>> current in d1.dependentGraph)
            {
                dependentGraph.Add(current.Key, current.Value);
            }
            foreach (KeyValuePair<string, HashSet<string>> current in d1.dependeeGraph)
            {
                dependeeGraph.Add(current.Key, current.Value);
            }
        }
        /// <summary>
        /// The number of dependencies in the DependencyGraph.
        /// </summary>
        public int Size
        {
            get
            {
                int sum = 0;
                foreach(KeyValuePair<string, HashSet<string>> i in dependentGraph)//Calculates number of dependencies by adding counts of each Hashset in the dictionary.
                {
                    sum += i.Value.Count;
                }
                return sum;
            }
        }
        /// <summary>
        /// Reports whether dependents(s) is non-empty.  Requires s != null.
        /// </summary>
        public bool HasDependents(string s)
        {
            if(s == null)
            {
                throw new ArgumentNullException("s cannot be null");
            }
            if(dependentGraph.ContainsKey(s))//Check that key s is in dictionary
            {
                if (dependentGraph[s].Count > 0)//and that the Hashset is not empty
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Reports whether dependees(s) is non-empty.  Requires s != null.
        /// </summary>
        public bool HasDependees(string s)
        {
            if(s == null)
            {
                throw new ArgumentNullException("s cannot be null");
            }
            if (dependeeGraph.ContainsKey(s))//Check that key s is in dictionary
            {
                if (dependeeGraph[s].Count > 0)//and that the Hashset is not empty
                {
                    return true;
                }
            }
            return false;
        }
        /// <summary>
        /// Enumerates dependents(s).  Requires s != null.
        /// </summary>
        public IEnumerable<string> GetDependents(string s)
        {
            if (s == null)
            {
                throw new ArgumentNullException("s cannot be null");
            }
            if(HasDependents(s) == true)//Ensure the key maps to a hashset
            {
                foreach(string i in dependentGraph[s])//navigate to and return each element in the key's hashset
                {
                    yield return i;
                }
            }
        }

        /// <summary>
        /// Enumerates dependees(s).  Requires s != null.
        /// </summary>
        public IEnumerable<string> GetDependees(string s)
        {
            if (s == null)
            {
                throw new ArgumentNullException("s cannot be null");
            }
            if (HasDependees(s) == true)//Ensure the key maps to a hashset
            {
                foreach (string i in dependeeGraph[s])//navigate to and return each element in the key's hashset
                {
                    yield return i;
                }
            }
        }
        /// <summary>
        /// Adds the dependency (s,t) to this DependencyGraph.
        /// This has no effect if (s,t) already belongs to this DependencyGraph.
        /// Requires s != null and t != null.
        /// </summary>
        public void AddDependency(string s, string t)
        {
            if (s == null || t == null)
            {
                if (s == null)
                {
                    throw new ArgumentNullException("s cannot be null");
                }
                if(t == null)
                {
                    throw new ArgumentNullException("t cannot be null");
                }
            }
            if (dependentGraph.ContainsKey(s) == true)//if the key already exists, just add on the new element to the hashset
            {
                dependentGraph[s].Add(t);
            }
            else//otherwise, create a new key s and a hashset under it. Add element t to hashset
            {
                dependentGraph.Add(s, new HashSet<string>());
                dependentGraph[s].Add(t);
            }
            if (dependeeGraph.ContainsKey(t) == true)//if the key already exists, just add on the new element to the hashset
            {
                dependeeGraph[t].Add(s);
            }
            else//otherwise, create a new key s and a hashset under it. Add element t to hashset
            {
                dependeeGraph.Add(t, new HashSet<string>());
                dependeeGraph[t].Add(s);
            }

        }
        /// <summary>
        /// Removes the dependency (s,t) from this DependencyGraph.
        /// Does nothing if (s,t) doesn't belong to this DependencyGraph.
        /// Requires s != null and t != null.
        /// </summary>
        public void RemoveDependency(string s, string t)
        {
            if (s == null || t == null)
            {
                if (s == null)
                {
                    throw new ArgumentNullException("s cannot be null");
                }
                if (t == null)
                {
                    throw new ArgumentNullException("t cannot be null");
                }
            }
            //if HasDependents == true then inner if
            if(HasDependents(s))
            {
                dependentGraph[s].Remove(t);
            }
            if (HasDependees(t))
            {
                dependeeGraph[t].Remove(s);
            }
        }

        /// <summary>
        /// Removes all existing dependencies of the form (s,r).  Then, for each
        /// t in newDependents, adds the dependency (s,t).
        /// Requires s != null and t != null.
        /// </summary>
        public void ReplaceDependents(string s, IEnumerable<string> newDependents)
        {
            if (s == null || newDependents == null)
            {
                if (s == null)
                {
                    throw new ArgumentNullException("s cannot be null");
                }
                if (newDependents == null)
                {
                    throw new ArgumentNullException("newDependents cannot be null");
                }
            }
            List<string> temp = new List<string>(GetDependents(s));
            foreach(string i in temp)
            {
                RemoveDependency(s,i);
            }
            foreach(string i in newDependents)
            {
                AddDependency(s, i);
            }
        }
        /// <summary>
        /// Removes all existing dependencies of the form (r,t).  Then, for each 
        /// s in newDependees, adds the dependency (s,t).
        /// Requires s != null and t != null.
        /// </summary>
        public void ReplaceDependees(string t, IEnumerable<string> newDependees)
        {
            if (t == null || newDependees == null)
            {
                if (t == null)
                {
                    throw new ArgumentNullException("t cannot be null");
                }
                if (newDependees == null)
                {
                    throw new ArgumentNullException("newDependees cannot be null");
                }
            }
            List<string> temp = new List<string>(GetDependees(t));
            foreach (string i in temp)
            {
                RemoveDependency(i,t);
            }
            foreach (string i in newDependees)
            {
                AddDependency(i,t);
            }
        }
    }
}
