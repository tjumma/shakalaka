using UnityEngine;

namespace Shakalaka
{
    public class SingletonMonoBehaviour<T> : MonoBehaviour where T : MonoBehaviour
    {
        public static T Instance { get; private set; }
        
        protected void Awake()
        {
            Instance ??= this as T;
        }
    }
}