using System;
using Interfaces;
using UnityEngine;
using UnityEngine.Events;

namespace Components
{
    public class H2OBehaviour : MonoBehaviour
    {
        [Header("Dependency")]
        [SerializeField]
        private H2OHeatable _heatable;


        public H2OHeatable Heatable => _heatable;

        [Header("Events")]
        public UnityEvent<H2OBehaviour> OnDestroyed = new ();

        private void OnDestroy()
        {
            OnDestroyed.Invoke(this);
            OnDestroyed.RemoveAllListeners();
        }
    }
}