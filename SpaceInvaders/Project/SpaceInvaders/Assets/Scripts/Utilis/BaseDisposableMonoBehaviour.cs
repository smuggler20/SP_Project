using System;
using System.Collections.Generic;
using UnityEngine;

namespace SpaceInvaders.Utilis
{
    public class BaseDisposableMonoBehaviour : MonoBehaviour
    {
        private List<IDisposable> _disposable = new List<IDisposable>();

        public List<IDisposable> Disposable
        {
            get { return _disposable; }
            set { _disposable = value; }
        }

        public void DisposeAll()
        {
            foreach (var item in Disposable)
            {
                item.Dispose();
            }
        }

        void OnDestroy()
        {
            foreach (var item in Disposable)
            {
                item.Dispose();
            }
        }
    }
}
