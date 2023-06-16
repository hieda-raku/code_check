using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ConsoleApp1
{
    //https://blog.csdn.net/boiled_water123/article/details/83021161
    //https://blog.csdn.net/Elsa15/article/details/104519017?spm=1001.2101.3001.6650.5&utm_medium=distribute.pc_relevant.none-task-blog-2%7Edefault%7ECTRLIST%7ERate-5-104519017-blog-83021161.pc_relevant_aa&depth_1-utm_source=distribute.pc_relevant.none-task-blog-2%7Edefault%7ECTRLIST%7ERate-5-104519017-blog-83021161.pc_relevant_aa&utm_relevant_index=7
    class Program
    {
        //static void Main(string[] args)
        //{
        //    ClassC.Say handler = ClassA.ClassA_Say;
        //    handler += ClassB.ClassB_Say;
        //    //  ClassC.Say_EventHandler handler2 = ClassB.ClassB_Say;
        //    handler();
        //    Console.ReadKey();
        //}


        static void Main(string[] args)
        {
            ClassC.Say_EventHandler += ClassA.ClassA_Say;
            ClassC.Say_EventHandler += ClassB.ClassB_Say;
            // ClassC.Say_EventHandler();
            ClassC classC = new ClassC();
            classC.MyProperty = 1;
            Console.ReadKey();
        }
    }

    class ClassA

    {

        static public void ClassA_Say()

        {

            Console.WriteLine("ClassA_Say");

        }

    }

    class ClassB

    {

        static public void ClassB_Say()

        {

            Console.WriteLine("ClassB_Say");

        }

    }

    class ClassC

    {

        public delegate void Say();

        public static event Say Say_EventHandler;

        private int myVar;//这个字段，只是纯粹用来用用Say_EventHandler用的，没有别的用处。



        public int MyProperty

        {

            get { return myVar; }

            set
            {

                myVar = value;

                if (Say_EventHandler != null)

                {

                    Say_EventHandler();

                }

            }

        }

    }
}
