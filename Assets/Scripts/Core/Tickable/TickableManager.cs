using System;
using System.Collections.Generic;
using UnityEngine;

namespace Core
{
    public class TickableManager : MonoBehaviour
    {
        private readonly List<ITickable> _tickables = new();

        private void Update()
        {
            foreach (var tickable in _tickables)
            {
                tickable.Tick(Time.deltaTime);
            }
        }

        public void Register(ITickable tickable)
        {
            _tickables.Add(tickable);
        }
    }
}