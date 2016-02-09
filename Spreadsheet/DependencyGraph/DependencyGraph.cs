// Skeleton implementation written by Joe Zachary for CS 3500, January 2015.
// Revised for CS 3500 by Joe Zachary, January 29, 2016

using System;
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
        /// Dictionary that maps strings to HashSets of strings, called dgMatrix (dependency graph matrix). 
        /// </summary>
        Dictionary<string,HashSet<string>> dgMatrix;
        /// <summary>
        /// Creates a DependencyGraph containing no dependencies.
        /// </summary>
        public DependencyGraph()
        {
            dgMatrix = new Dictionary<string, HashSet<String>>();
        }
        /// <summary>
        /// Copy constructor for dependency graph. Takes in d1 dictionary, returns d2 dictionary copy of d1
        /// </summary>
        /// <param name="d1"></param>
        public DependencyGraph(DependencyGraph d1)
        {
            Dictionary<string, HashSet<string>> d2 = new Dictionary<string, HashSet<String>>(d1.dgMatrix);
        }
        /// <summary>
        /// The number of dependencies in the DependencyGraph.
        /// </summary>
        public int Size
        {
            get
            {
                int sum = 0;
                foreach(KeyValuePair<string, HashSet<string>> i in dgMatrix)//Calculates number of dependencies by adding counts of each Hashset in the dictionary.
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
            if(dgMatrix.ContainsKey(s))//Check that key s is in dictionary
            {
                if (dgMatrix[s].Count > 0)//and that the Hashset is not empty
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
            foreach(KeyValuePair<string, HashSet<string>> i in dgMatrix)//iterate through each KVP
            {
                if(i.Value.Contains(s)== true)//if they value is found in a hashset, return true
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
                foreach(string i in dgMatrix[s])//navigate to and return each element in the key's hashset
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
            if(HasDependees(s) == true)//Check that there is such a key mapped to s
            {
                foreach(KeyValuePair<string,HashSet<string>> i in dgMatrix)//iterate through KVPs
                {
                    if(i.Value.Contains(s) == true)//if a hashset contains s, return the key containing it
                    {
                        yield return i.Key;
                    }
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
            if (dgMatrix.ContainsKey(s) == true)//if the key already exists, just add on the new element to the hashset
            {
                dgMatrix[s].Add(t);
            }
            else//otherwise, create a new key s and a hashset under it. Add element t to hashset
            {
                dgMatrix.Add(s, new HashSet<string>());
                dgMatrix[s].Add(t);
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
                dgMatrix[s].Remove(t);
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
            if (HasDependents(s) == true)//check key is in dictionary
            {
                //FAILED TO INCLUDE INDEX S WHEN CLEARING
                dgMatrix[s].Clear();//Clear the hashset
            }
            foreach(string t in newDependents)//Create new dependencies in form of s -> t
            {
                AddDependency(s, t);
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
            foreach(string r in GetDependees(t))//Clear all dependencies of under r of t
            {
                RemoveDependency(r,t);
            }
            foreach(string s in newDependees)//Recreate new dependency of t under s (basically, move t from under r to under s; reassigning the dependent to a new dependee)
            {
                AddDependency(s, t);
            }
        }
    }
}
