using CommandLine;
using System;
using System.Diagnostics;

namespace Sensei.NET
{
    class Program
    {
        public class Options
        {
            [Value(0, MetaName = "crate", Required = true, HelpText = "What crate do you need help with?")]
            public string Name { get; set; }

            [Option('v', "ver", HelpText = "Opens documentation for a specific version.")]
            public string Version { get; set; }

            [Option('q', "query", HelpText = "Specifies query to search documentation.")]
            public string Query { get; set; }
        }

        static void Main(string[] args)
        {
            Crate crate = new Crate();
            Parser.Default.ParseArguments<Options>(args)
                   .WithParsed<Options>(o =>
                   {
                       crate.Name = o.Name;
                       crate.Version = o.Version ?? String.Empty;
                       crate.Query = o.Query ?? String.Empty;

                       if (String.IsNullOrWhiteSpace(crate.Query) && String.IsNullOrWhiteSpace(crate.Version))
                       {
                           OpenUrl($@"https://docs.rs/{crate.Name}", crate);
                       }
                       else if (!String.IsNullOrWhiteSpace(crate.Query) && !String.IsNullOrWhiteSpace(crate.Version))
                       {
                           OpenUrl($@"https://docs.rs/{crate.Name}/{crate.Version}/{crate.Name}/?search={crate.Query}", crate);
                       }
                       else if (!String.IsNullOrWhiteSpace(crate.Version))
                       {
                           OpenUrl($@"https://docs.rs/{crate.Name}/{crate.Version}/{crate.Name}", crate);
                       }
                       else
                       {
                           OpenUrl($@"https://docs.rs/{crate.Name}/?search={crate.Query}", crate);
                       }
                   });
        }

        public class Crate
        {
            public string Name { get; set; }
            public string Version { get; set; }
            public string Query { get; set; }
        }

        public static void OpenUrl(string uri, Crate crate)
        {
            try
            {
                var ps = new ProcessStartInfo(uri)
                {
                    UseShellExecute = true,
                    Verb = "open"
                };
                Process.Start(ps);
                Console.WriteLine($"||| The Book Of {crate.Name.Substring(0, 1).ToUpper() + crate.Name.Substring(1)} {crate.Version ?? String.Empty}|||");
            }
            catch (Exception e)
            {
                Console.WriteLine("Seems like you've lost your way, try again.");
            }
        }
    }
}
