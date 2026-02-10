using System;
using Grid;
using UnityEngine;

namespace Manager
{
    public static class EventManager
    {
        public static event Action<GridObject, Vector3Int> OnObjectRegister;
        
        public static event Action<GridObject, Vector3Int> OnObjectUnregister;
        
        /// <summary>
        /// Vector3Int => from
        /// Vector3Int => to
        /// </summary>
        public static event Action<GridObject, Vector3Int, Vector3Int> OnObjectMoved;
    }
}
