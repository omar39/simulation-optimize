using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MarbleContainer))]
public class ActorContainer : MonoBehaviour
{
    public ActorBehavior ActorPrefab;
    private MarbleContainer _containerReference;
    private readonly List<ActorBehavior> _actors = new List<ActorBehavior>();

    [SerializeField] private int defaultCapacity = 100;

    void Start()
    {
        _containerReference = this.gameObject.GetComponent<MarbleContainer>();
        for( var i = 0; i < defaultCapacity; i++ )
        {
            var newActor = Instantiate( ActorPrefab );
            newActor.transform.SetParent( this.transform );
            newActor.ContainerReference = _containerReference;
            newActor.transform.position = Random.insideUnitSphere * 100f;
            _actors.Add( newActor );
        }
    }
    public List<ActorBehavior> GetAllActors() => _actors;
}
