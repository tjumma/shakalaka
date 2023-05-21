using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace Shakalaka
{
    public class MainMenuScope : AppStateScope
    {
        [SerializeField] private MainMenuUI ui;
        
        protected override void Configure(IContainerBuilder builder)
        {
            Debug.Log("MainMenuScope Configure");
            builder.RegisterComponent(ui);
        }
    }
}