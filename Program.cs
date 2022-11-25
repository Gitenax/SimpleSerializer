using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;

namespace Serializator
{
    internal class Program
    {
        public static void Main(string[] args)
        {
            List<ListNode> nodes = new List<ListNode>();

            for (var i = 0; i < 10; i++)
                nodes.Add(new ListNode());
            
            for (var i = 0; i < nodes.Count; i++)
            {
                nodes[i].Data = $"Hello I'm node #{i}!";
                nodes[i].Next = i >= nodes.Count - 1 ? null : nodes[i + 1];
                nodes[i].Prev = i ==0 ? null : nodes[i - 1];
            }

            string result = GigachadSerializer.Serialize(nodes[1]);

            ListNode deserialized = GigachadSerializer.Deserialize<ListNode>(result);
            
            Console.ReadLine();
        }
    }


    public static class GigachadSerializer
    {
        public static string Serialize(object target)
        {
            StringBuilder resultData = new StringBuilder();
            resultData.AppendLine("!GIGACHAD SERIALIZER");
            resultData.AppendLine("Version: BFG9000");
            
            Type instanceType = target.GetType();
            
            FieldInfo[] fields = instanceType.GetFields();

            foreach (FieldInfo fieldInfo in fields)
            {
                object value = fieldInfo.GetValue(target);
                resultData.AppendLine($"{fieldInfo.Name}:{value}");
            }

            return resultData.ToString();
        }
        
        public static T Deserialize<T>(string value)
        {
            string[] strings = value.Split(new[] {Environment.NewLine}, StringSplitOptions.RemoveEmptyEntries);
            
            T instance = Activator.CreateInstance<T>();
            Type instanceType = typeof(T);
        
            for (int i = 2; i < strings.Length; i++)
            {
                string[] pair = strings[i].Split(':');
                string fieldName = pair[0];
                string fieldValue = pair[1];
        
                FieldInfo field = instanceType.GetField(fieldName);

                if (TryParseValue(field.FieldType, fieldValue, out object result))
                    field.SetValue(instance, result);
            }

            return instance;
        }

        private static bool TryParseValue(Type fieldType, string value, out object result)
        {
            if (fieldType != typeof(string) && string.IsNullOrEmpty(value))
            {
                result = null;
                return false;
            }
            
            result = fieldType switch
            {
                var t when t == typeof(string) => value,
                var t when t == typeof(int) => int.Parse(value),
                _ => Activator.CreateInstance(fieldType) // По умолчанию.
            };
            
            return result != null;
        }
    }
    
    
    
    
    public sealed class ListNode
    {
        public ListNode Prev;
        public ListNode Next;
        public ListNode Rand; // произвольный элемент внутри списка
        public string Data;
    }


    public sealed class ListRand
    {
        public ListNode Head;
        public ListNode Tail;
        public int Count;

        public void Serialize(FileStream s)
        {
        }

        public void Deserialize(FileStream s)
        {
        }
    }

}
