using System;
using System.Collections.Generic;

public class ViewExercisesViewModel
{
    public event Action ExercisesChanged;

    public List<Exercise> Exercises { get; private set; }

    private readonly DataService _dataService;

    public ViewExercisesViewModel(DataService dataService)
    {
        _dataService = dataService;
        Load();
    }

    public void Load()
    {
        var data = _dataService.Load();
        Exercises = data.Exercises;
        ExercisesChanged?.Invoke();
    }
}