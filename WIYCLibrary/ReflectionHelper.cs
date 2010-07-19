namespace WIYCLibrary
{
    using System;
    using System.Reflection;

    public class ReflectionHelper
    {
        public static object GetFieldValue(object obj, string fieldName)
        {
            if (obj == null)
            {
                throw new ArgumentNullException("object cannot be null");
            }
            if (string.IsNullOrEmpty(fieldName))
            {
                throw new ArgumentNullException("FieldName cannot be null or empty");
            }
            FieldInfo field = obj.GetType().GetField(fieldName, BindingFlags.GetProperty | BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance);
            if (field == null)
            {
                throw new MissingFieldException(obj.GetType().Name + " does not contain the field " + fieldName);
            }
            return field.GetValue(obj);
        }

        public static object GetPropertyValue(object obj, string propertyName)
        {
            if (obj == null)
            {
                throw new ArgumentNullException("object cannot be null");
            }
            if (string.IsNullOrEmpty(propertyName))
            {
                throw new ArgumentNullException("PropertyName cannot be null or empty");
            }
            PropertyInfo property = obj.GetType().GetProperty(propertyName, BindingFlags.GetProperty | BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance);
            if (property == null)
            {
                throw new MissingMemberException(obj.GetType().Name + " does not contain the field " + propertyName);
            }
            return property.GetValue(obj, null);
        }

        public static object InvokeMethod(object obj, string key, string methodName, object[] parameters)
        {
            if (obj == null)
            {
                throw new ArgumentNullException("obj");
            }
            if (string.IsNullOrEmpty(methodName))
            {
                throw new ArgumentNullException("MethodName");
            }
            MethodInfo method = obj.GetType().GetMethod(methodName, BindingFlags.GetProperty | BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance);
            if (method == null)
            {
                throw new MissingMethodException(obj.GetType().Name + " does not contain the method " + methodName);
            }
            return method.Invoke(obj, parameters);
        }
    }
}

