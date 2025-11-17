using System;
using System.Collections.Generic;
using UnityEngine;

namespace Models
{
    [Serializable]
    public class Training : IModel
    {
        [field: SerializeField] public string Id { get; private set; }
        [field: SerializeField] public string Name { get; set; }
        [field: SerializeField] public string Description { get; set; }
        [field: SerializeField] public int PrepTimeSeconds { get; set; }
        [field: SerializeField] public List<TrainingBlock> Blocks { get; set; } = new();
        [field: SerializeField] public TimeSpan Duration { get; set; }
        [field: SerializeField] public DateTime LastTime { get; set; }

        public Training()
        {
            Id = Guid.NewGuid().ToString();
            Duration = new TimeSpan(1, 0, 0);
            LastTime = DateTime.Now;
        }

        public Training(string name, string description, int prepTimeSeconds) : this()
        {
            Name = name;
            Description = description;
            PrepTimeSeconds = prepTimeSeconds;
        }

        public void AddOrUpdateBlock(TrainingBlock trainingBlock)
        {
            if (trainingBlock == null) return;

            int index = Blocks.FindIndex(b => b.Id == trainingBlock.Id);
            if (index >= 0)
                Blocks[index] = trainingBlock;
            else
                Blocks.Add(trainingBlock);
        }

        public void RemoveBlock(string blockId)
        {
            Blocks.RemoveAll(b => b.Id == blockId);
        }
    }
}