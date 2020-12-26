using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Utility {


    public static Bounds GetRenderersBounds(GameObject gameObject)
    {
        Renderer[] renderers = gameObject.GetComponentsInChildren<Renderer>();
        Bounds? bounds = null;
        foreach (Renderer renderer in renderers)
        {
            if (bounds==null)
                bounds = renderer.bounds;
            else
                bounds.Value.Encapsulate(renderer.bounds);
        }

        return bounds.Value;
    }

    public static void PlaceOnTheGround(Transform transform)
    {
        RaycastHit hit;
        if(Physics.Raycast(transform.position, Vector3.down, out hit))
        {
            Bounds bounds = GetRenderersBounds(transform.gameObject);
            float verticalOffset = transform.position.y - bounds.min.y;
            transform.position = hit.point + Vector3.up*verticalOffset;
        }
    }

    public static T RandomChoice<T>(T[] array)
    {
        if (array.Length == 0) return default(T);
        else return array[(uint)Random.Range(0, array.Length)];
    }

    public static float PlanarDistance(Vector3 a, Vector3 b)
    {
        float dx = b.x - a.x;
        float dz = b.z - a.z;
        return Mathf.Sqrt(dx * dx + dz * dz);
    }

    public static Transform FindTransformWithTag(string tag)
    {
        GameObject tagged = GameObject.FindGameObjectWithTag(tag);
        if (tagged != null)
        {
            return tagged.transform;
        }
        else
        {
            return null;
        }
    }

    public interface IProbability
    {
        float Probability { get; }
    }

    public static T SelectBasedOnProbabilities<T>(IEnumerable<T> list) where T : IProbability
    {
        return SelectBasedOnProbabilities(list, (T item) => item.Probability);
    }

    public static T SelectBasedOnProbabilities<T>(IEnumerable<T> list, System.Func<T, float> probabilityFunction)
    {
        float probabilitiesSum = 0f;
        foreach (T item in list)
        {
            probabilitiesSum += probabilityFunction(item);
        }
        float random = Random.value * probabilitiesSum;
        float currentSum = 0;
        foreach (T item in list)
        {
            currentSum += probabilityFunction(item);
            if (currentSum >= random)
            {
                return item;
            }
        }
        return default(T);
    }

    [System.Serializable]
    public class Range
    {
        public float min;
        public float max;
        public bool Contains(float value)
        {
            return value >= min && value <= min;
        }
        public float Lerp(float t)
        {
            return Mathf.Lerp(min, max, t);
        }
        public float RandomValue()
        {
            return Random.Range(min, max);
        }
        public Range(float min, float max)
        {
            this.min = min;
            this.max = max;
        }
    }
}
