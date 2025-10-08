using System.Collections.Generic;
using UnityEngine;

public class Spawner<T> : MonoBehaviourSingleton<Spawner<T>> where T : MonoBehaviour
{
    [SerializeField] protected T _prefab;
    [SerializeField] protected Transform _container;
    protected Queue<T> _pool = new Queue<T>();
    protected List<T> _used = new List<T>();
    protected Transform Container
    {
        get
        {
            if (_container == null)
            {
                _container = GetComponent<Transform>();
            }
            return _container;
        }
    }

    public T Spawn()
    {
        if(_pool.Count > 0)
        {
            T item = _pool.Dequeue();
            _used.Add(item);
            return item;
        }
        else
        {
            T item = Instantiate(_prefab, Container);
            _used.Add(item);
            return item;
        }
    }

    public void AddToPool(T item)
    {
        _used.Remove(item);
        _pool.Enqueue(item);
    }

    //public void Clear()
    //{
    //    _used.ForEach(i => i.gameObject.SetActive(false));
    //}

    public void Clear()
    {
        // copy danh sách trước khi disable
        var tempList = new List<T>(_used);

        foreach (var i in tempList)
        {
            i.gameObject.SetActive(false);
            //Destroy(i);
        }
    }

}
