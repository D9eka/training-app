using System;
using System.Collections.Generic;
using Core.Installers;
using Data;
using Models;
using Screens.Factories;
using UnityEngine;

namespace Core
{
    [DefaultExecutionOrder(-1)]
    public class DiContainer : MonoBehaviour
    {
        [SerializeField] private ScreensInstaller _screensInstaller;
        [SerializeField] private UiController _uiController;
        [SerializeField] private TickableManager _tickableManager;
        
        public static DiContainer Instance { get; private set; }

        private readonly Dictionary<Type, object> _services = new();
        private readonly Dictionary<string, GameObject> _named = new();

        private void Awake()
        {
            if (Instance != null)
            {
                Destroy(gameObject); 
                return; 
            }
            Instance = this;
            DontDestroyOnLoad(gameObject);
            
            _screensInstaller.Install(this);
            new DataServiceInstaller().Install(this);
            
            Register(_uiController);
            Register(_tickableManager);
            RegisterScreenFactories();
            
            _uiController.Initialize(Resolve<ViewModelFactory>());
        }

        private void RegisterScreenFactories()
        {
            Register(new MainFactory(Resolve<TrainingDataService>()));
            Register(new CreateEquipmentFactory(Resolve<IDataService<Equipment>>()));
            Register(new CreateExerciseFactory(Resolve<IDataService<Exercise>>(), Resolve<IDataService<Equipment>>()));
            Register(new CreateTrainingBlockFactory(Resolve<TrainingDataService>(), Resolve<IDataService<Exercise>>(), Resolve<IDataService<Equipment>>()));
            Register(new CreateTrainingFactory(Resolve<TrainingDataService>(), Resolve<IDataService<Exercise>>()));
            Register(new SelectExerciseFactory(Resolve<TrainingDataService>(), Resolve<IDataService<Exercise>>(), Resolve<IDataService<Equipment>>()));
            Register(new ViewExerciseFactory(Resolve<TrainingDataService>(), Resolve<IDataService<Exercise>>(), Resolve<IDataService<Equipment>>()));
            Register(new ViewExercisesFactory(Resolve<IDataService<Exercise>>(), Resolve<IDataService<Equipment>>()));
            Register(new ViewTrainingFactory(Resolve<TrainingDataService>(), Resolve<IDataService<Exercise>>()));
            Register(new ViewTrainingsFactory(Resolve<TrainingDataService>()));
            Register(new StartTrainingFactory(Resolve<TrainingDataService>()));
            Register(new TimerFactory(Resolve<TrainingDataService>(), 
                Resolve<IDataService<Exercise>>(), Resolve<IDataService<Equipment>>(), 
                Resolve<UiController>(),Resolve<TickableManager>()));
            
            Register(new ViewModelFactory());
        }

        public void Register<T>(T service)
        {
            if (service == null) throw new ArgumentNullException(nameof(service));
            Type type = typeof(T);
            if (_services.ContainsKey(type)) Debug.LogWarning($"Overriding service {type.Name}");
            _services[type] = service;
        }

        public T Resolve<T>()
        {
            if (_services.TryGetValue(typeof(T), out object s)) return (T)s;
            throw new InvalidOperationException($"Service {typeof(T).Name} not registered. Available: {string.Join(", ", _services.Keys)}");
        }

        public void RegisterNamed(string key, GameObject obj)
        {
            if (string.IsNullOrEmpty(key)) throw new ArgumentNullException(nameof(key));
            if (obj == null) throw new ArgumentNullException(nameof(obj));
            if (_named.ContainsKey(key)) Debug.LogWarning($"Overriding named object '{key}'");
            _named[key] = obj;
        }

        public GameObject ResolveNamed(string key)
        {
            if (string.IsNullOrEmpty(key)) throw new ArgumentNullException(nameof(key));
            if (_named.TryGetValue(key, out GameObject go)) return go;
            Debug.LogError($"Named object '{key}' not found. Available: {string.Join(", ", _named.Keys)}");
            return null;
        }
    }
}
