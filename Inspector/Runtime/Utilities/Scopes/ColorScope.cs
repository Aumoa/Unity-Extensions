using System;
using UnityEngine;

namespace Ayla.Inspector.Runtime.Utilities.Scopes
{
    public readonly struct ColorScope : IDisposable
    {
        private readonly Color previousColor;

        public ColorScope(Color color)
        {
            previousColor = GUI.color;
            GUI.color = color;
        }

        public void Dispose()
        {
            GUI.color = previousColor;
        }
    }
}
