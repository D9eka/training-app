using System;
using System.Collections.Generic;
using UnityEngine;

namespace Models
{
    [Serializable]
    public class Training : IModel
    {
        [SerializeField] private string _id;
        [SerializeField] private string _name;
        [SerializeField] private string _description;
        [SerializeField] private float _prepTimeSeconds;
        [SerializeField] private List<TrainingBlock> _blocks;

        public string Id
        {
            get => _id;
            private set => _id = value;
        }

        public string Name
        {
            get => _name;
            set => _name = value;
        }

        public string Description
        {
            get => _description;
            set => _description = value;
        }

        public float PrepTimeSeconds
        {
            get => _prepTimeSeconds;
            set => _prepTimeSeconds = value;
        }

        public List<TrainingBlock> Blocks
        {
            get => _blocks;
            set => _blocks = value;
        }

        public Training()
        {
            _id = Guid.NewGuid().ToString();
            _blocks = new List<TrainingBlock>();
        }

        public Training(string name, string description, float prepTimeSeconds) : this()
        {
            _name = name;
            _description = description;
            _prepTimeSeconds = prepTimeSeconds;
        }

        public void AddOrUpdateBlock(TrainingBlock trainingBlock)
        {
            int existingIndex = _blocks.FindIndex(localTrainingBlock => localTrainingBlock.Id == trainingBlock.Id);
            if (existingIndex >= 0)
                _blocks[existingIndex] = trainingBlock;
            else
                _blocks.Add(trainingBlock);
        }

        public void RemoveBlock(string blockId)
        {
            _blocks.RemoveAll(trainingBlock => trainingBlock.Id == blockId);
        }
    }
}