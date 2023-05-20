using System.Collections.ObjectModel;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public sealed class ObjectPool : MonoBehaviour
{
    private Collection<GameObject> _pooledObjects = new Collection<GameObject>();

    public Collection<GameObject> PooledObjects { get { return _pooledObjects; } }

    [SerializeField] private int amountToPool = 20;
    [SerializeField] private GameObject[] ballPrefabs;
    [SerializeField] private Image nextBullet;
    [SerializeField] private Image currentBullet;

    public GameObject[] BallPrefabs { get { return ballPrefabs; } }

    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < amountToPool; i++)
        {
            int rand = Random.Range(0, ballPrefabs.Length);
            GameObject obj = Instantiate(ballPrefabs[rand]);
            _pooledObjects.Add(obj);
            obj.transform.SetParent(transform);
            obj.SetActive(false);
        }
        currentBullet.sprite = _pooledObjects.First().GetComponent<SpriteRenderer>().sprite;
        nextBullet.sprite = _pooledObjects.ElementAt(1).GetComponent<SpriteRenderer>().sprite;
    }

    public GameObject GetPooledObject()
    {
        GameObject pooledObj = null;

        for (int i = 0; i < _pooledObjects.Count; i++)
        {
            nextBullet.sprite = _pooledObjects[i].GetComponent<SpriteRenderer>().sprite;
            currentBullet.sprite = _pooledObjects[i + 1].GetComponent<SpriteRenderer>().sprite;

            if (!_pooledObjects[i].activeInHierarchy)
            {
                pooledObj= _pooledObjects[i];
                _pooledObjects.Remove(_pooledObjects[i]);
                break;
            }
        }

        return pooledObj;
    }
}
