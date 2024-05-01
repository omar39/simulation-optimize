using System;
using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;
using UnityEngine.Pool;

public class MarbleContainer : MonoBehaviour
{
    public MarbleBehavior MarblePrefab;
    private KDTree _marblesTree;
    private ObjectPool<MarbleBehavior> _objectPool;
    [SerializeField] private int defaultCapacity = 100;
    [SerializeField] private int maxCapacity = 200;
    [SerializeField] private bool _usePooling = true;
    
    private int _marblesCurrentCount = 0;

    void Start()
    {
        // create a KDTree
        _marblesTree = new KDTree();
        
        _objectPool = new ObjectPool<MarbleBehavior>(
            () => {return Instantiate(MarblePrefab);},
            m => {m.gameObject.SetActive(true);},
            m => {m.gameObject.SetActive(false);},
            m => {Destroy(m.gameObject);},
            false,defaultCapacity, maxCapacity 
        );

        StopAllCoroutines();
        for( var i = 0; i < 500; i++ )
        {
            GenerateMarble();
        }

        StartCoroutine( SpawnMarbles() );
    }

    IEnumerator SpawnMarbles()
    {
        while( true )
        {
            if( _marblesCurrentCount < defaultCapacity )
            {
                for( var i = 0; i < 25; i++ )
                {
                   GenerateMarble();
                }
            }
            yield return new WaitForEndOfFrame();
            //yield return new WaitForSeconds( 0.5f );
        }
    }

    private void GenerateMarble()
    {
        Vector3 randomPosition = new Vector3( Random.value, Random.value, Random.value );
        var newMarble = _usePooling ? _objectPool.Get() : Instantiate( MarblePrefab );
        
        newMarble.transform.SetPositionAndRotation(randomPosition, Quaternion.identity);
        
        newMarble.OnClaimed += RemoveMarble;

        newMarble.Id = Guid.NewGuid();
        newMarble.transform.parent = this.transform;
        newMarble.transform.position = Random.insideUnitSphere * 100f;
        _marblesCurrentCount ++;

        _marblesTree.AddNode(newMarble.gameObject);
    }

    public void ClaimMarble( MarbleBehavior marble )
    {
        _marblesTree.RemoveNode(marble.gameObject);
        _marblesCurrentCount --;

        marble.WasClaimed = true;
    }
    // add the remove behavior to container to control pool
    private void RemoveMarble(MarbleBehavior marble)
    {
        if(_usePooling)
            _objectPool.Release( marble );
        else 
            Destroy( marble.gameObject );
    }

    public MarbleBehavior GetCloseMarbleToPosition( Vector3 position )
    {
        return _marblesTree.FindNearestNeighbor( position ).Value.GetComponent<MarbleBehavior>();
    }
}