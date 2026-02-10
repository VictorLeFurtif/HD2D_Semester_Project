using System;
using System.Collections.Generic;
using Manager;
using UnityEngine;

namespace Grid
{
    public class GridSystem : MonoBehaviour
    {
        #region Variables

        Dictionary<Vector3Int, GridObject> dictionnaryGridObjects = new Dictionary<Vector3Int, GridObject>();
        
        [SerializeField] private float cellSize = 1f;



        #endregion

        #region Unity Lifecycle

        private void OnEnable()
        {
            SetupSub();
        }

        private void OnDisable()
        {
            CleanSub();
        }

        #endregion

        #region Setup & Clean Methods

        private void SetupSub()
        {
            /*
            EventManager.OnObjectRegister += 
            EventManager.OnObjectUnregister += 
            EventManager.OnObjectMoved += 
            */
        }

        private void CleanSub()
        {
            /*
            EventManager.OnObjectRegister -=
            EventManager.OnObjectUnregister -=
            EventManager.OnObjectMoved -=
            */
        }

        #endregion

        #region Grid Methods

        private void RegisterObject(GridObject gridObject, Vector3Int position)
        {
            
        }

        private void UnregisterObject(GridObject gridObject, Vector3Int position)
        {
            
        }

        private void MoveObject(GridObject gridObject, Vector3Int fromPosition, Vector3Int toPosition)
        {
            
        }

        private bool TryMoveObject(Vector3Int toPosition)
        {
            
        }

        private Vector3Int WorldToGrid(Vector3 worldPosition)
        {
            
        }

        private Vector3 GridToWorld(Vector3Int gridPosition)
        {
            
        }

        private GridObject GetGridObjectAt(Vector3Int gridPosition)
        {
            
        }

        private void IsPositionOccupied(Vector3Int gridPosition)
        {
            
        }
        
        #endregion
    }
}
