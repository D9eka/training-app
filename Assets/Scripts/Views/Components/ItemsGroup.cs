using System.Collections.Generic;
using UnityEngine;
using Utils;

namespace Views.Components
{
    public class ItemsGroup<T>
    where T : MonoBehaviour
    {
        private readonly Transform _contentParent;
        private readonly T _itemPrefab;

        public ItemsGroup(Transform contentParent, T itemPrefab)
        {
            _contentParent = contentParent;
            _itemPrefab = itemPrefab;
        }

        public List<T> Refresh(int itemsCount)
        {
            List<T> items = new List<T>();
            
            foreach (T item in _contentParent.GetComponentsInChildren<T>())
                SimplePool.Return(item.gameObject, _itemPrefab.gameObject);

            for (int i = 0; i < itemsCount; i++)
            {
                GameObject go = SimplePool.Get(_itemPrefab.gameObject, _contentParent);
                items.Add(go.GetComponent<T>());
            }
            return items;
        }
    }
}