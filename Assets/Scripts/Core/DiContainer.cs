using System;
using System.Collections.Generic;
using Data;
using Models;
using UnityEngine;

namespace Core
{
    [DefaultExecutionOrder(-1)]
    public class DiContainer : MonoBehaviour
    {
        [SerializeField] private ScreensInstaller _screensInstaller;
        [SerializeField] private UiController _uiController;
        
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

            ISaveService saveService = new SaveService(deferSave: true);
            IDataService<Equipment> equipmentDataService = new EquipmentDataService(saveService.EquipmentsCache);
            IDataService<Exercise> exerciseDataService = new ExerciseDataService(saveService.ExercisesCache);
            IDataService<Training> trainingDataService = new TrainingDataService(saveService.TrainingsCache);
            DataService data = new DataService(saveService, equipmentDataService, exerciseDataService, trainingDataService);
            Register(new ViewModelFactory(equipmentDataService, exerciseDataService, trainingDataService));
            Register<ISaveService>(saveService);
            Register<IDataService<Equipment>>(equipmentDataService);
            Register<IDataService<Exercise>>(exerciseDataService);
            Register<IDataService<Training>>(trainingDataService);
            _screensInstaller.Install(this);
            Register(_uiController);
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