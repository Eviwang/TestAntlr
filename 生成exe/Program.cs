using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

namespace 生成exe
{
    class Program
    {
        //bin 目录下生成了一个Program.exe
        static void Main(string[] args)
        {
            var assemblyBuilder = AppDomain.CurrentDomain.DefineDynamicAssembly(new AssemblyName("Assembly"), AssemblyBuilderAccess.RunAndSave);
            var moduleBuilder = assemblyBuilder.DefineDynamicModule("Program", "Program.exe", true);

            var typeBuilder = moduleBuilder.DefineType("Foo", TypeAttributes.Public | TypeAttributes.Class);
            
            var method = typeBuilder.DefineMethod("Main", MethodAttributes.Public | MethodAttributes.Static, typeof(void), Type.EmptyTypes);

            ILGenerator methodIL = method.GetILGenerator();
            methodIL.Emit(OpCodes.Ldstr,"hello world!");
            methodIL.Emit(OpCodes.Call,typeof(Console).GetMethod(nameof(Console.WriteLine), new[] { typeof(string) }));
            //methodIL.Emit(OpCodes.Call, typeof(Console).GetMethod(nameof(Console.ReadLine)));
            methodIL.Emit(OpCodes.Ret);
            assemblyBuilder.SetEntryPoint(method, PEFileKinds.ConsoleApplication);
            typeBuilder.CreateType();
            assemblyBuilder.Save("Program.exe");
        }
    }
}
