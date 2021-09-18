using CommandLine;
using dictionary_crud.Business;
using dictionary_crud.Services;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace dictionary_crud
{
    public class Program
    {
        static DictionaryService service = new DictionaryService();

        static void Main(string[] args)
        {
            var types = LoadVerbs();
            bool processing = true;
            //while (Helpers.ConsoleHelper.GenerateMenuForType<DictionaryService>()) { }
            while(processing){
                Console.Write("> ");
                var line = Console.ReadLine().Split(' ', StringSplitOptions.RemoveEmptyEntries);

                Parser.Default.ParseArguments(line, types)
                    .WithParsed(Parsed)
                    .WithNotParsed(errors => errors.Output().WriteLine("Error"));
            }
        }

        private static void Parsed(object obj)
        {
            try
            {
                var ret = Run(obj);
                if (!string.IsNullOrWhiteSpace(ret))
                    Console.WriteLine(ret);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"ERROR, {ex.Message}");
            }
        }

        private static string Run(object obj)
        {
            switch (obj)
            {
                case KeysOption k:
                    return BuildList(service.Keys());
                case MembersOption m:
                    return BuildList(service.Members(m.Key));
                case AddOption a:
                    service.Add(a.Key, a.Member);
                    return "Added";
                case RemoveOption r:
                    service.Remove(r.Key, r.Member);
                    return "Removed";
                case RemoveAllOption ra:
                    service.RemoveAll(ra.Key);
                    return "Removed";
                case ClearOption c:
                    service.Clear();
                    return "Cleared";
                case KeyExistsOption ke:
                    return service.KeyExists(ke.Key).ToString();
                case MemberExistsOption me:
                    return service.MemberExists(me.Key, me.Member).ToString();
                case AllMembersOption am:
                    return BuildList(service.AllMembers());
                case ItemsOption i:
                    return BuildList(service.Items().Select(item => $"{item.key}: {item.member}"));
                default:
                    return "unknown command";
            }
        }
        
        private static string BuildList<T>(IEnumerable<T> list)
        {
            var sb = new StringBuilder();
            if (!list.Any())
                sb.Append("(empty set)");
            else
                for (int i = 0; i < list.Count(); ++i)
                    sb.AppendLine($"{i + 1}) {list.ElementAt(i)}");
            return sb.ToString();
        }

        //load all types using Reflection
        // https://github.com/commandlineparser/commandline/wiki/Verbs
        private static Type[] LoadVerbs()
        {
            return Assembly.GetExecutingAssembly()
                .GetTypes()
                .Where(t => t.GetCustomAttribute<VerbAttribute>() != null)
                .ToArray();
        }
}
}
