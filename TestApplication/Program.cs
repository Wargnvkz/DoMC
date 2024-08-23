using System;
using System.Net;
using System.Runtime.InteropServices;

namespace TestApplication
{
    internal class Program
    {
        static void Main(string[] args)
        {
            dynamic myVar = "Hello, World!";
            Console.WriteLine(myVar);  // Выводит: Hello, World!

            // Присваиваем число переменной myVar
            myVar = 42;
            Console.WriteLine(myVar);  // Выводит: 42

            // Теперь myVar ведет себя как число, и мы можем выполнять математические операции
            myVar += 8;
            Console.WriteLine(myVar);  // Выводит: 50

            // Вызов метода, который доступен в текущем типе myVar (число)
            Console.WriteLine(myVar.GetType()); // Выводит: System.Int32

            // Пример неправильного вызова метода (если вызвать несуществующий метод)
            try
            {
                myVar.NonExistentMethod(); // Ошибка на этапе выполнения
            }
            catch (Microsoft.CSharp.RuntimeBinder.RuntimeBinderException ex)
            {
                Console.WriteLine("Runtime error: " + ex.Message);
            }
            Console.ReadKey();
        }
        

    }


}
