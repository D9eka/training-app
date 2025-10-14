using System;
using System.Collections.Generic;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Utils
{
    public static class SimplePool
    {
        private static readonly Dictionary<GameObject, Stack<GameObject>> _pools = new();
        
        public static GameObject Get(GameObject prefab, Transform parent)
        {
            if (prefab == null) throw new ArgumentNullException(nameof(prefab));

            if (!_pools.TryGetValue(prefab, out Stack<GameObject> stack) || stack.Count == 0)
            {
                GameObject go = Object.Instantiate(prefab, parent);
                go.SetActive(true);
                return go;
            }

            GameObject obj = stack.Pop();
            obj.transform.SetParent(parent, false);
            obj.SetActive(true);
            return obj;
        }
        
        public static void Return(GameObject obj, GameObject prefab)
        {
            if (obj == null || prefab == null) return;
            obj.SetActive(false);
            if (!_pools.TryGetValue(prefab, out Stack<GameObject> stack))
            {
                stack = new Stack<GameObject>();
                _pools[prefab] = stack;
            }
            stack.Push(obj);
        }
        
        public static void Clear()
        {
            foreach (Stack<GameObject> stack in _pools.Values)
            {
                while (stack.Count > 0)
                {
                    GameObject obj = stack.Pop();
                    if (obj != null) Object.Destroy(obj);
                }
            }
            _pools.Clear();
        }
    }
}