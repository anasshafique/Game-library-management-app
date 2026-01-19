using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CET2007
{
    public abstract class Person
    {
        private int iPlayerID;
        private string sUsername;

        public int PlayerID
        {
            get { return iPlayerID; }
            set 
            { 
                if (value <= 0)
                    throw new ArgumentException("Player ID must be positive.");
                iPlayerID = value; 
            }
        }

        public String UserName
        {
            get { return sUsername; }
            set 
            { 
                if (string.IsNullOrWhiteSpace(value))
                    throw new ArgumentException("Username cannot be empty.");
                sUsername = value; 
            }
        }

        // For JSON file because it cannot work with parameterized constructors
        public Person() { }

        // Constructor
        public Person(int id, string name) 
        {
            if (id <= 0)
                throw new ArgumentException("Player ID must be positive.");
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("Username cannot be empty.");
                
            iPlayerID = id;
            sUsername = name;
        }

        // Virtual method for polymorphism demonstration
        public override string ToString()
        {
            return $"ID: {iPlayerID}, Username: {sUsername}";
        }
    }
}
