using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Http_Client
{


    class Program
    {
        static void Main()
        {
            HttpClient client = new HttpClient();

            do
            {
                Console.WriteLine($"1 GET()\n2 POST()\n3 PUT()\n4 DELETE()");
                int input = int.Parse(Console.ReadLine());


                switch (input)
                {
                    case 1:
                        client.GetAsync("http://localhost:45678/");
                        Task.Run(async () =>
                        {
                            HttpResponseMessage response = await client.GetAsync("http://localhost:45678/");

                            var json = await response.Content.ReadAsStringAsync();

                            var Users = JsonSerializer.Deserialize<List<User>>(json);

                            foreach (var user in Users)
                            {
                                Console.WriteLine(user.ID);
                                Console.WriteLine(user.Name);
                                Console.WriteLine(user.Surname);
                                Console.WriteLine();
                            }
                        });

                        break;

                    case 2:
                        Console.Clear();
                        Console.WriteLine("Enter Name: ");
                        string Name = Console.ReadLine();
                        Console.WriteLine("Enter Surame: ");
                        string Surname = Console.ReadLine();
                        User user = new User(Name, Surname);

                        var UserJsn = JsonSerializer.Serialize(user);

                        var request = new HttpRequestMessage
                        {
                            Method = HttpMethod.Post,
                            RequestUri = new Uri("http://localhost:45678/"),
                            Content = new StringContent(UserJsn, Encoding.UTF8, "application/json")
                        };
                        Task.Run(async () =>
                        {
                            await client.SendAsync(request).ConfigureAwait(false);
                            Console.WriteLine("Great User Successfuly Addedd!!");
                        });
                        break;

                    case 3:
                        Console.Clear();
                        Console.WriteLine("Please Enter UserID: ");
                        int Id = int.Parse(Console.ReadLine());
                        Console.WriteLine("Enter Name: ");
                        string Name2 = Console.ReadLine();
                        Console.WriteLine("Enter Surame: ");
                        string Surname2 = Console.ReadLine();
                        var user2 = new User()
                        {
                            ID = Id,
                            Name = Name2,
                            Surname = Surname2
                        };

                        var Jsonuser = JsonSerializer.Serialize(user2);

                        var request2 = new HttpRequestMessage
                        {
                            Method = HttpMethod.Put,
                            RequestUri = new Uri("http://localhost:45678/"),
                            Content = new StringContent(Jsonuser, Encoding.UTF8, "application/json")
                        };
                        Task.Run(async () =>
                        {
                            await client.SendAsync(request2).ConfigureAwait(false);
                            Console.WriteLine("Great User Successfuly Addedd!!");
                        });


                        Task.Run(async () =>
                        {
                            HttpResponseMessage response = await client.GetAsync("http://localhost:45678/");

                            var json = await response.Content.ReadAsStringAsync();

                            var Usertf = JsonSerializer.Deserialize<bool>(json);

                            if (Usertf == true) Console.WriteLine("Great Succesfully Exchanged User Informations!!!");
                            else Console.WriteLine("User Not Found!!!Please Try Again!!!");
                        });

                        break;

                    case 4:
                        Console.Clear();
                        Console.WriteLine("Please Enter Car's Id: ");

                        var delcar = int.Parse(Console.ReadLine());

                        var cardel = JsonSerializer.Serialize(delcar);

                        var request3 = new HttpRequestMessage
                        {
                            Method = HttpMethod.Delete,
                            RequestUri = new Uri("http://localhost:45678/"),
                            Content = new StringContent(cardel, Encoding.UTF8, "application/json")
                        };
                        Task.Run(async () =>
                        {
                            await client.SendAsync(request3).ConfigureAwait(false);
                        });
                        var deluser = false;
                        Task.Run(async () =>
                        {
                            HttpResponseMessage response = await client.GetAsync("http://localhost:45678/");

                            var json = await response.Content.ReadAsStringAsync();

                            deluser = JsonSerializer.Deserialize<bool>(json);

                        });
                        Console.WriteLine(deluser);
                        if (deluser != true) { Console.WriteLine("Car deleted!!!"); }
                        else Console.WriteLine("Car Not Found!!!");
                        break;


                    default:
                        Console.WriteLine("Please write correct info!!!");
                        break;
                }


            } while (true);


        }
    }
}
