using System.Text.RegularExpressions;

namespace Tutorial1
{

    public class Program
    {
        public static async Task Main(string[] args)
        {
            if (args.Length != 1)
            {
                throw new ArgumentNullException("Parameter 1 was not passed");
            }

            if (!Uri.IsWellFormedUriString(args[0], UriKind.Absolute))
            {
                throw new ArgumentException("Not valid URL");
            }
            
            
            var httpClient = new HttpClient();
           HttpResponseMessage response=  await httpClient.GetAsync(args[0]);
           

            if (response.IsSuccessStatusCode)
            {
                Console.WriteLine(await response.Content.ReadAsStringAsync());
               
                Console.WriteLine("---------------------------------------------------------------");
               
              
               if (Regex.IsMatch(await response.Content.ReadAsStringAsync(), "[A-Za-z0-9+_.-]+@[A-Za-z0-9.-]+"))
               {


                   foreach (string emails in Regex
                                .Matches(await response.Content.ReadAsStringAsync(), "[A-Za-z0-9+_.-]+@[A-Za-z0-9.-]+")
                                .Select(e => e.Value).Distinct().ToList())
                   {
                       Console.WriteLine(emails);
                   }
               }
               else
               {
                   Console.WriteLine("E-mail addresses not found");
               }
            }
            else
            {
               Console.WriteLine("Error while downloading the page"); 
            }
            
            httpClient.Dispose();
        }
    }
}

