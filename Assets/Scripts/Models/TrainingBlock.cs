using System;
using System.Collections.Generic;
using UnityEngine;

namespace Models
{
    [Serializable]
    public class TrainingBlock : IModel
    {
        [SerializeField] private string _trainingId;
        [SerializeField] private string _id;
        [SerializeField] private List<ExerciseInBlock> _exercises;
        [SerializeField] private int _approaches;
        [SerializeField] private TimeSpan _approachesTimeSpan;
        [SerializeField] private int _sets;
        [SerializeField] private TimeSpan _restAfterApproachTimeSpan;
        [SerializeField] private TimeSpan _restAfterSetTimeSpan;
        [SerializeField] private TimeSpan _restAfterBlockTimeSpan;

        public string TrainingId
        {
            get => _trainingId;
            private set => _trainingId = value;
        }

        public string Id
        {
            get => _id;
            private set => _id = value;
        }

        public List<ExerciseInBlock> Exercises
        {
            get => _exercises;
            set => _exercises = value;
        }

        public int Approaches
        {
            get => _approaches;
            set => _approaches = value;
        }

        public TimeSpan ApproachesTimeSpan
        {
            get => _approachesTimeSpan;
            set => _approachesTimeSpan = value;
        }

        public int Sets
        {
            get => _sets;
            set => _sets = value;
        }

        public TimeSpan RestAfterApproachTimeSpan
        {
            get => _restAfterApproachTimeSpan;
            set => _restAfterApproachTimeSpan = value;
        }

        public TimeSpan RestAfterSetTimeSpan
        {
            get => _restAfterSetTimeSpan;
            set => _restAfterSetTimeSpan = value;
        }

        public TimeSpan RestAfterBlockTimeSpan
        {
            get => _restAfterBlockTimeSpan;
            set => _restAfterBlockTimeSpan = value;
        }

        public TrainingBlock(string trainingId)
        {
            _trainingId = trainingId;
            _id = Guid.NewGuid().ToString();
            _exercises = new List<ExerciseInBlock>();
        }

        public TrainingBlock(string trainingId, List<ExerciseInBlock> exercises, int approaches, TimeSpan approachesTimeSpan, 
            int sets, TimeSpan restAfterApproachTimeSpan, TimeSpan restAfterSetTimeSpan, 
            TimeSpan restAfterBlockTimeSpan) : this(trainingId)
        {
            _exercises = exercises;
            _approaches = approaches;
            _approachesTimeSpan = approachesTimeSpan;
            _sets = sets;
            _restAfterApproachTimeSpan = restAfterApproachTimeSpan;
            _restAfterSetTimeSpan = restAfterSetTimeSpan;
            _restAfterBlockTimeSpan = restAfterBlockTimeSpan;
        }

        public void AddExercise(ExerciseInBlock exercise)
        {
            _exercises.Add(exercise);
        }

        public void RemoveExercise(string exerciseId)
        {
            _exercises.RemoveAll(r => r.Exercise.Id == exerciseId);
        }
    }
}