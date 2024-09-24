using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoMCLib.Tools
{
    public static class Mapper
    {
        public static T Map<T>(object source)
        {
            var target = Activator.CreateInstance(typeof(T));
            Map(source, target);
            return (T)target;
        }

        public static void Map(object source, object target)
        {
            if (source == null || target == null) throw new ArgumentNullException();
            var sourceType = source.GetType();
            var dtoType = target.GetType();

            foreach (var sourceProp in sourceType.GetProperties())
            {
                var AllAttributes = Attribute.GetCustomAttributes(sourceProp, typeof(MapToAttribute));
                var mapToAttribute = Array.Find(AllAttributes, a =>
                {
                    if (a is MapToAttribute m2a)
                    {
                        return m2a.DtoClass == dtoType;
                    }
                    else
                    {
                        return false;
                    }
                }) as MapToAttribute;
                var value = sourceProp.GetValue(source);
                // Проверяем, является ли объектом ссылочного типа
                if (value != null && sourceProp.PropertyType.IsClass && sourceProp.PropertyType != typeof(string) && value is not Array)
                {
                    Map(value, target);
                }
                else
                    if (mapToAttribute != null && mapToAttribute.DtoClass == dtoType)
                {
                    var dtoProp = dtoType.GetProperty(mapToAttribute.DtoField);
                    if (dtoProp.PropertyType != value.GetType()) throw new InvalidCastException($"Типы полей {sourceProp.Name} в исходном классе и {dtoProp.Name} в целевом классе не совпадают");

                    if (dtoProp != null && dtoProp.CanWrite)
                    {
                        // Проверяем, является ли поле массивом или коллекцией
                        if (value is Array array)
                        {
                            var elementType = dtoProp.PropertyType.GetElementType();
                            var mappedArray = Array.CreateInstance(elementType, array.Length);
                            Array.Copy(array, mappedArray, array.Length);

                            dtoProp.SetValue(target, mappedArray);
                        }
                        else
                        if (value is IList list)
                        {
                            var listType = dtoProp.PropertyType.GetGenericArguments()[0];
                            var mappedList = (IList)Activator.CreateInstance(typeof(List<>).MakeGenericType(listType));

                            foreach (var item in list)
                            {
                                mappedList.Add(item);
                            }

                            dtoProp.SetValue(target, mappedList);
                        }
                        else
                        {
                            // Простые типы данных
                            dtoProp.SetValue(target, value);
                        }
                    }
                }
            }

        }
    }

    [AttributeUsage(AttributeTargets.Property, AllowMultiple = true)]
    public class MapToAttribute : Attribute
    {
        public Type DtoClass { get; }
        public string DtoField { get; }

        public MapToAttribute(Type dtoClass, string dtoField)
        {
            DtoClass = dtoClass;
            DtoField = dtoField;
        }
    }


}
