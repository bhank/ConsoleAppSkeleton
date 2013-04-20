using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using NDesk.Options;

namespace ConsoleApp
{
    public class Parameters
    {
        public bool Help { get; private set; }
        public bool Version { get; private set; }

        public static bool TryParse(IList<string> args, out Parameters parameters)
        {
            parameters = null;

            var p = new Parameters
            {
                // Insert default values here
            };

            var optionSet = new OptionSet
                {
                    {"h|?|help", "Show this help.", v => p.Help = (v != null)},
                    {"V|version", "Show the version.", v => p.Version = (v != null)},
                    //{"sp|stringparameter=", "The {VALUE} for a string parameter", v => p.StringParameter = v},
                    //{"ip|intparameter=", "The {VALUE} for an int parameter", v => p.IntParameter = int.Parse(v)},
                    //{"ep|enumparameter=", "The {VALUE} for an enum parameter.", v =>
                    //{"ovsp|optionalvaluestringparameter:", "A parameter that can optionally take a {VALUE}, which will be null otherwise", v => { p.OptionalValueSpecified = true; p.OptionalValue = v } },
                    //    {
                    //            EnumType e;
                    //            Enum.TryParse(v, true, out e);
                    //            p.EnumValue |= e; // You can specify multiple flag values in separate parameters
                    //    }},
                };
            var extraArgs = optionSet.Parse(args);

            string error = null;

            if (extraArgs.Count > 0)
            {
                error = "Unknown parameter: " + extraArgs[0];
            }
            //else if (p.SomethingRequired == null)
            //{
            //    error = "You must specify --somethingrequired";
            //}

            if (p.Help)
            {
                ShowUsage(optionSet);
                return false;
            }

            if (p.Version)
            {
                var assembly = typeof(Parameters).Assembly;
                var name = Path.GetFileNameWithoutExtension(assembly.Location).ToLowerInvariant();
                var version = assembly.GetCustomAttributes(typeof(AssemblyFileVersionAttribute), false).Cast<AssemblyFileVersionAttribute>().Select(a => a.Version).FirstOrDefault();
                Console.Error.WriteLine("{0} version {1}", name, version);
                return false;
            }

            if (error != null)
            {
                Console.Error.WriteLine(error);
                Console.Error.WriteLine("Pass --help for usage information.");
                return false;
            }
            parameters = p;
            return true;
        }

        private static void ShowUsage(OptionSet optionSet)
        {
            optionSet.WriteOptionDescriptions(Console.Error);
            Console.Error.WriteLine(@"

Additional information about usage of this app.

");
        }
    }
}
