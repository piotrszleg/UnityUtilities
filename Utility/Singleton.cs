using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Singleton<T> : MonoBehaviour
{
    public static T instance { get; private set; }

    [Serializable]
    public class MultipleInstancesException : Exception
    {
        public MultipleInstancesException(string message)
        : base(message)
        {

        }
    }

    [Serializable]
    public class IncorrectUsageException : Exception
    {
        public IncorrectUsageException(string message)
        : base(message)
        {

        }
    }

    protected virtual void Awake()
    {
        if (instance != null)
        {
            throw new MultipleInstancesException("There is more than one instance of singleton of type " + GetType().GetGenericArguments()[0]);
        }
        try {
             instance = (T)(object)this;
        } catch(InvalidCastException)
        {
            throw new IncorrectUsageException("Class derieving from Singleton should have signature: \"class DerivingClass : Singleton<DerivingClass>\"");
        }
    }
}
