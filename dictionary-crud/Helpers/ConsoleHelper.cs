using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace dictionary_crud.Helpers
{
    class ConsoleHelper
    {
        public static bool GenerateMenuForType<T>() where T:class
        {
            List<MethodInfo> methods = new List<MethodInfo>();
            Console.WriteLine("Menu");
            foreach (MethodInfo m in typeof(T).GetMethods().OrderBy(me => me.Name))
            {
                if (m.IsStatic && m.IsPublic)
                {
                    methods.Add(m);
                    var paramString = new StringBuilder();
                    if (m.GetParameters().Count() > 0)
                    {
                        string separator = ", ";
                        foreach (ParameterInfo p in m.GetParameters().OrderBy(pr => pr.Position))
                        {
                            if (paramString.Length > 0)
                                paramString.Append(separator);
                            paramString.Append (p.ParameterType.Name + " " + p.Name);
                        }
                    }
                    Console.WriteLine(methods.IndexOf(m) + ") " + m.Name + "(" + paramString + ")");
                }
            }

            Console.Write("Execute function [EXIT to quit]: ");
            string input = Console.ReadLine();
            if (input.ToUpper() == "EXIT")
                return false;

            else if (string.IsNullOrEmpty(input))
                return false;

            if (Convert.ToInt32(input) > methods.Count() || Convert.ToInt32(input) < 0)
            {
                Console.WriteLine("Index out of range");
                return true;
            }

            MethodInfo method = methods[Convert.ToInt32(input)];
            object[] parameters = null;
            if (method.GetParameters().Count() > 0)
            {
                parameters = new object[method.GetParameters().Count()];
            }

            try
            {
                foreach (ParameterInfo p in method.GetParameters().OrderBy(pr => pr.Position))
                {
                    Console.Write("Input " + p.ParameterType.Name + " for " + p.Name + ": ");
                    string paramInput = Console.ReadLine();
                    switch (p.ParameterType.Name)
                    {
                        case "Type":
                            parameters[p.Position] = Type.GetType(paramInput);
                            break;
                        default:
                            parameters[p.Position] = Convert.ChangeType(paramInput, p.ParameterType);
                            break;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return true;
            }

            Console.WriteLine("== Start " + method.Name + " ==");
            try
            {
                object ret = method.Invoke(null, parameters);
                if (ret != null)
                {
                    Console.WriteLine("Return Value:");
                    Console.WriteLine(ret);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("== ERROR ==");
                Console.WriteLine(ex.Message);
            }
            Console.WriteLine("== END " + method.Name + " ==");
            Console.WriteLine();
            return true;
        }
    }
}
