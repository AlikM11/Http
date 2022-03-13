using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace Http_Listener
{
    class Program
    {
        static void Main(string[] args)
        {

            List<User> users = new List<User>()
            {
                new User("Eli","Veliyev"),
                new User("Ilkin","Firengizov"),
                new User("Ceyhun","Abidov"),
                new User("Nureddin","Esgerov"),
                new User("Fizuli","Ehemdov"),
                new User("Mircahan","Ismetov"),
            };

            var listener = new HttpListener();

            listener.Prefixes.Add("http://localhost:45678/");


            listener.Start();
            while (true)
            {
                var context = listener.GetContext();
                var request = context.Request;
                var response = context.Response;

                StreamWriter sw = new StreamWriter(response.OutputStream);

                StreamReader sr = new StreamReader(request.InputStream);

                response.AddHeader("Content-type", "application/json");

                switch (request.HttpMethod)
                {
                    case "GET":
                        var UserList = JsonSerializer.Serialize(users);
                        sw.Write(UserList);
                        sw.Close();
                        break;
                    case "POST":
                        var User = sr.ReadToEnd();
                        var NewUser = JsonSerializer.Deserialize<User>(User);
                        users.Add(NewUser);
                        break;
                    case "PUT":
                        var userinfo = sr.ReadToEnd();
                        var user = JsonSerializer.Deserialize<User>(userinfo);
                        bool ok = false;
                        for (int i = 0; i < users.Count; i++)
                        {
                            if (users[i].ID == user.ID)
                            {
                                ok = true;
                                users[i].Name = user.Name;
                                users[i].Surname = user.Surname;
                                break;
                            }
                            else ok = false;
                        }
                        var str = JsonSerializer.Serialize(ok);
                        sw.Write(str);
                        sw.Close();
                        break;
                    case "DELETE":
                        bool tfdeluserid = false;
                        var feedbackdeluser = sr.ReadToEnd();
                        var deluserid = JsonSerializer.Deserialize<int>(feedbackdeluser);
                        foreach (var user2 in users)
                        {
                            if (user2.ID == deluserid)
                            {
                                users.Remove(user2);
                                tfdeluserid = true;
                                break;
                            }
                            else tfdeluserid = false;
                        }
                        Task.Run(() =>
                        {
                            var str2 = JsonSerializer.Serialize(tfdeluserid);
                            sw.Write(str2);
                            sw.Close();
                        });
                        break;
                    default:
                        break;
                }
            }
        }
    }
}
