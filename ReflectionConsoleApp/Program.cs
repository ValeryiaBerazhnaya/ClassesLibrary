using System;
using System.IO;
using System.Linq;
using System.Reflection;

namespace ReflectionConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                Console.WriteLine("Enter path to DLL:");
                string path = Console.ReadLine();

                if (!File.Exists(path))
                {
                    Console.WriteLine("File not found!");
                    return;
                }

                Assembly asm = Assembly.LoadFrom(path);

                Console.WriteLine("\nClasses in the library:");
                var types = asm.GetTypes().Where(t => t.IsClass).ToList();
                foreach (var type in types)
                {
                    Console.WriteLine($"\nClass: {type.Name}");
                    var props = type.GetProperties(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
                    foreach (var prop in props)
                    {
                        string access = prop.GetMethod?.IsPublic == true ? "public" : "private";
                        Console.WriteLine($"   {access} {prop.PropertyType.Name} {prop.Name}");
                    }
                }

                Console.WriteLine("\nEnter class name:");
                string className = Console.ReadLine();
                var classType = asm.GetType($"ClassesLibrary.{className}");
                if (classType == null)
                {
                    Console.WriteLine("Class not found!");
                    return;
                }

                Console.WriteLine("\nMethods of this class:");
                var methods = classType.GetMethods(BindingFlags.Public | BindingFlags.Static | BindingFlags.Instance | BindingFlags.DeclaredOnly);
                foreach (var m in methods)
                {
                    string methodType = m.IsStatic ? "static" : "instance";
                    Console.WriteLine($"   {methodType} {m.ReturnType.Name} {m.Name}({string.Join(", ", m.GetParameters().Select(p => p.ParameterType.Name + " " + p.Name))})");
                }

                Console.WriteLine("\nEnter method name (e.g., Create or PrintObject):");
                string methodName = Console.ReadLine();
                var method = classType.GetMethod(methodName, BindingFlags.Public | BindingFlags.Static | BindingFlags.Instance);
                if (method == null)
                {
                    Console.WriteLine("Method not found!");
                    return;
                }

                var parameters = method.GetParameters();
                object[] argsForMethod = new object[parameters.Length];

                for (int i = 0; i < parameters.Length; i++)
                {
                    Console.WriteLine($"\nEnter value for parameter {parameters[i].Name} ({parameters[i].ParameterType.Name}):");
                    if (parameters[i].ParameterType.IsEnum)
                    {
                        Console.WriteLine("Available options: " + string.Join(", ", Enum.GetNames(parameters[i].ParameterType)));
                        argsForMethod[i] = Enum.Parse(parameters[i].ParameterType, Console.ReadLine(), true);
                    }
                    else
                    {
                        argsForMethod[i] = Convert.ChangeType(Console.ReadLine(), parameters[i].ParameterType);
                    }
                }

                object instance = null;

                if (method.IsStatic)
                {
                    instance = method.Invoke(null, argsForMethod);
                    Console.WriteLine("\nObject created successfully!");

                    var printMethod = classType.GetMethod("PrintObject");
                    if (printMethod != null)
                    {
                        var output = printMethod.Invoke(instance, null);
                        Console.WriteLine("\nPrintObject result:");
                        Console.WriteLine(output);
                    }
                }
                else
                {
                    Console.WriteLine("\nMethod is not static, skipped.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"\nError: {ex.Message}");
            }
        }
    }
}
