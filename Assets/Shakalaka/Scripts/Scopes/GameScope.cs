using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace Shakalaka
{
    public class GameScope : AppStateScope
    {
        [SerializeField] private Relay relay;
        
        protected override void Configure(IContainerBuilder builder)
        {
            Debug.Log("GameScope Configure");
            builder.RegisterComponent(relay);
        }
    }
}