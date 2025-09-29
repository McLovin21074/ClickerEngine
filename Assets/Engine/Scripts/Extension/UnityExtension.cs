using ClickerEngine.EntryPoint;
using UnityEngine;

namespace ClickerEngine.Extension
{
    public static class UnityExtension
    {
        public static IEntryPoint GetEntryPoint<T>() where T : MonoBehaviour, IEntryPoint
        {
            return Object.FindFirstObjectByType<T>();
        }
    }
}