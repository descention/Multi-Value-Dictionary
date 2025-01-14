﻿using CommandLine;
using dictionary_crud.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;

namespace dictionary_crud_cli
{
    public class Program
    {
        static DictionaryService service = new DictionaryService();

        static void Main(string[] args)
        {
            var types = LoadVerbs();
            Console.WriteLine("Press Ctrl-C to quit. Type Help for a list of commands.");
            
            while (true)
            {
                Console.Write("> ");
                var line = Console.ReadLine();

                // parsing command args with quotes
                // https://stackoverflow.com/a/59638741/935537
                var commandList = Regex.Matches(line, @"[\""].+?[\""]|[^ ]+")
                .Cast<Match>()
                .Select(x => x.Value.Trim('"'))
                .ToArray();

                Parser.Default.ParseArguments(commandList, types)
                    .WithParsed(Parsed);
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
                case UnionOption u:
                    return BuildList(service.Union(u.KeyA, u.KeyB));
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

        // load all types using Reflection
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

