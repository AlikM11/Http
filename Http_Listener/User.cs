using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Http_Listener
{
    public class User
    {
        public int ID { get; set; }

        public static int _id { get; set; } = 1;

        public string Name { get; set; }

        public string Surname { get; set; }


        public User()
        {
            ID = _id;
            _id++;
        }
        public User(string name, string surname)
        {
            ID = _id;
            _id++;
            Name = name;
            Surname = surname;
        }

    }
}
