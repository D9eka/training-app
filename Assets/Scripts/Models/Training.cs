using System;
using System.Collections.Generic;

namespace Models
{
    [Serializable]
    public class Training : IModel
    {
        public string Id { get; private set; }
        public string Name;
        public string Description;
        public float PrepTimeSeconds;
        public List<TrainingBlock> Blocks;

        public Training()
        {
            Id = Guid.NewGuid().ToString();
            Blocks = new List<TrainingBlock>();
        }

        public Training(string name, string description, float prepTimeSeconds) : this()
        {
            Name = name;
            Description = description;
            PrepTimeSeconds = prepTimeSeconds;
        }

        public void AddOrUpdateBlock(TrainingBlock trainingBlock)
        {
            int existingIndex = Blocks.FindIndex(localTrainingBlock => localTrainingBlock.Id == trainingBlock.Id);
            if (existingIndex >= 0)
                Blocks[existingIndex] = trainingBlock;
            else
                Blocks.Add(trainingBlock);
        }

        public void RemoveBlock(string blockId)
        {
            Blocks.RemoveAll(trainingBlock => trainingBlock.Id == blockId);
        }
    }
}