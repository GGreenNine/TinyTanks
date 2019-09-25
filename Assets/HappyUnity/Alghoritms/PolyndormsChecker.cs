using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HappyUnity.Containers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    class PolydromChecker
    {
        static bool ArePalindromes(String str1, String str2)
        {
            if (str1.Length != str2.Length)
                return false;

            Stack<char> stack1 = StringToCharStack(str1);
            Stack<char> stack2 = StringToCharStack(str2);
            Stack<char> stack3 = new Stack<char>();

            while (!stack1.IsEmpty())
                stack3.Push(stack1.Pop());

            while (!stack2.IsEmpty())
            {
                char tmp1 = stack3.Pop();
                char tmp2 = stack2.Pop();
                if (tmp1 != tmp2)
                    return false;
            }

            return true;
        }

        static Stack<char> StringToCharStack(String str)
        {
            Stack<char> stack = new Stack<char>();
            for (int i = 0; i < str.Length; ++i)
                stack.Push(str[i]);
            return stack;
        }


        static void Main(String[] args)
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine("Enter first string");
                String str1 = Console.ReadLine();
                Console.WriteLine("Enter second string");
                String str2 = Console.ReadLine();
                Console.Write("Strings are palindromes: ");
                Console.WriteLine(ArePalindromes(str1, str2));
                Console.ReadKey();
            }
        }
    }
    public class Stack<E>
    {
        private class Node<T>
        {
            public T Item { get; set; }
            public Node<T> Next { get; set; }

            public Node(T element, Node<T> next)
            {
                Item = element;
                Next = next;
            }
        }

        private Node<E> top;

        public Stack()
        {
        }

        public bool IsEmpty()
        {
            return top == null;
        }

        public void Push(E item)
        {
            Node<E> node = new Node<E>(item, top);
            top = node;
        }

        public E Pop()
        {
            E temp = top.Item;
            top = top.Next;
            return temp;
        }

        public E Peek()
        {
            return top.Item;
        }
    }
}

