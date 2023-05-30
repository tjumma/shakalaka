using UnityEngine;
using VContainer.Unity;

namespace Shakalaka
{
    public class SingletonScope<T> : LifetimeScope where T : LifetimeScope
    {
        public static T Instance { get; private set; }
        
        protected override void Awake()
        {
            Debug.Log("SingletonScope Awake");
            Instance ??= this as T;
            
            base.Awake();
        }
    }
}