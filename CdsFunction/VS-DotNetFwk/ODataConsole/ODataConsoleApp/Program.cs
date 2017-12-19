using Microsoft.OData.SampleService.Models.TripPin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ODataConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            var context = new DefaultContainer(new Uri("http://services.odata.org/v4/(S(lqbvtwide0ngdev54adgc0lu))/TripPinServiceRW/"));

            var people = context.People.Expand(p => p.Trips).Execute();

            foreach (var person in people)
            {
                Console.WriteLine($"{person.FirstName} {person.LastName}");
                foreach (var trip in person.Trips)
                {
                    Console.WriteLine($"\t{trip.Name} {trip.Budget}");
                }
            }
            
            Console.ReadLine();

            Console.WriteLine("--- More complex query ---");
            var petersOrTrippers = from person in context.People
                                       //where person.FirstName == "Scott"
                                   where person.FirstName.EndsWith("Scott")
                                   //|| person.Trips.Count == 2 // Can't do this some error around $count being a single value
                                   || person.Trips.Any(t => t.Budget > 3000)
                                   select person;

            foreach (var person in petersOrTrippers)
            {
                Console.WriteLine($"{person.FirstName} {person.LastName}");
            }

            Console.ReadLine();
        }
    }
}
