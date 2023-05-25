using UnityEngine;
using VContainer;

namespace Shakalaka
{
    public class GameScope : AppStateScope
    {
        protected override void Configure(IContainerBuilder builder)
        {
            Debug.Log("GameScope Configure");
        }
    }
}