using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace DynamicGenerateProperties
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            Init();
        }

        private void Init()
        {
            // Generate properties dynamically
            Type dynamicType = GenerateDynamicClass(100); // Generate properties A to AA

            // Create an instance of the dynamic class
            dynamic instance = Activator.CreateInstance(dynamicType);

            // Set values to the dynamic properties
            PropertyInfo propertyA = dynamicType.GetProperty("A");
            propertyA.SetValue(instance, 10);

            PropertyInfo propertyB = dynamicType.GetProperty("B");
            propertyB.SetValue(instance, 20);

            PropertyInfo propertyAA = dynamicType.GetProperty("AA");
            propertyAA.SetValue(instance, 30);

            // Get values from the dynamic properties
            int valueA = (int)propertyA.GetValue(instance);
            int valueB = (int)propertyB.GetValue(instance);
            int valueAA = (int)propertyAA.GetValue(instance);

            // Print the values
            Console.WriteLine("A: " + valueA);
            Console.WriteLine("B: " + valueB);
            Console.WriteLine("AA: " + valueAA);
        }

        Type GenerateDynamicClass(int propertyCount)
        {
            AssemblyName assemblyName = new AssemblyName("DynamicAssembly");
            AssemblyBuilder assemblyBuilder = AssemblyBuilder.DefineDynamicAssembly(assemblyName, AssemblyBuilderAccess.Run);
            ModuleBuilder moduleBuilder = assemblyBuilder.DefineDynamicModule("DynamicModule");
            TypeBuilder typeBuilder = moduleBuilder.DefineType("DynamicClass", TypeAttributes.Public | TypeAttributes.Class);

            // Generate properties dynamically
            for (int i = 0; i < propertyCount; i++)
            {
                string propertyName = GetPropertyName(i);
                FieldBuilder fieldBuilder = typeBuilder.DefineField("_" + propertyName, typeof(int), FieldAttributes.Private);

                PropertyBuilder propertyBuilder = typeBuilder.DefineProperty(propertyName, PropertyAttributes.HasDefault, typeof(int), null);
                MethodAttributes getSetAttributes = MethodAttributes.Public | MethodAttributes.SpecialName | MethodAttributes.HideBySig;

                MethodBuilder getMethodBuilder = typeBuilder.DefineMethod("get_" + propertyName, getSetAttributes, typeof(int), Type.EmptyTypes);
                ILGenerator getIlGenerator = getMethodBuilder.GetILGenerator();
                getIlGenerator.Emit(OpCodes.Ldarg_0);
                getIlGenerator.Emit(OpCodes.Ldfld, fieldBuilder);
                getIlGenerator.Emit(OpCodes.Ret);

                MethodBuilder setMethodBuilder = typeBuilder.DefineMethod("set_" + propertyName, getSetAttributes, null, new Type[] { typeof(int) });
                ILGenerator setIlGenerator = setMethodBuilder.GetILGenerator();
                setIlGenerator.Emit(OpCodes.Ldarg_0);
                setIlGenerator.Emit(OpCodes.Ldarg_1);
                setIlGenerator.Emit(OpCodes.Stfld, fieldBuilder);
                setIlGenerator.Emit(OpCodes.Ret);

                propertyBuilder.SetGetMethod(getMethodBuilder);
                propertyBuilder.SetSetMethod(setMethodBuilder);
            }

            // Create the dynamic class type
            Type dynamicType = typeBuilder.CreateType();

            return dynamicType;
        }

        string GetPropertyName(int propertyIndex)
        {
            int dividend = propertyIndex + 1;
            string propertyName = string.Empty;

            while (dividend > 0)
            {
                int modulo = (dividend - 1) % 26;
                propertyName = Convert.ToChar(65 + modulo) + propertyName;
                dividend = (dividend - modulo) / 26;
            }

            return propertyName;
        }
    }
}
