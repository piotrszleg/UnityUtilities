using System.Collections;

using System.Collections.Generic;
using UnityEngine;
using System;
using System.Reflection;

[AttributeUsage(AttributeTargets.Field)]
public class AutoFieldAttribute : Attribute
{
    public virtual void Execute(object obj, FieldInfo field) { }
}

public class GetComponentAttribute : AutoFieldAttribute
{
    public override void Execute(object obj, FieldInfo field)
    {
        field.SetValue(obj, ((MonoBehaviour)obj).GetComponent(field.FieldType));
    }
}

public class GetComponentInChildrenAttribute : AutoFieldAttribute
{
    public override void Execute(object obj, FieldInfo field)
    {
        field.SetValue(obj, ((MonoBehaviour)obj).GetComponentInChildren(field.FieldType));
    }
}

public class PoolAttribute : AutoFieldAttribute
{
    uint count;
    public PoolAttribute(uint count)
    {
        this.count = count;
    }
    public override void Execute(object obj, FieldInfo field)
    {
        PoolObject fieldValue = (PoolObject)field.GetValue(obj);
        if (fieldValue != null)
        {
            Pool.instance.Register(fieldValue, count);
        }
    }
}

public static class AutoFields
{
    struct AttributeAndField
    {
        public AutoFieldAttribute attribute;
        public FieldInfo field;
    }
    static Dictionary<Type, List<AttributeAndField>> cached = new Dictionary<Type, List<AttributeAndField>>();

    public static void InitializeAll(object obj) => InitializeAll(obj, obj.GetType());

    public static void InitializeAll(object obj, Type type)
    {
        if (cached.ContainsKey(type))
        {
            List<AttributeAndField> typeCache = cached[type];
            foreach (AttributeAndField attributeAndField in typeCache)
            {
                attributeAndField.attribute.Execute(obj, attributeAndField.field);
            }
        }
        else
        {
            List<AttributeAndField> typeCache = new List<AttributeAndField>();
            FieldInfo[] fields = type.GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.FlattenHierarchy);
            foreach (FieldInfo field in fields)
            {
                foreach (AutoFieldAttribute attribute in field.GetCustomAttributes<AutoFieldAttribute>())
                {
                    attribute.Execute(obj, field);
                    typeCache.Add(new AttributeAndField { attribute = attribute, field = field });
                }
            }
            cached.Add(type, typeCache);
        }
    }
}

public abstract class AutoFieldsExecutor : MonoBehaviour
{
    protected virtual void Start()
    {
        AutoFields.InitializeAll(this);
    }
}