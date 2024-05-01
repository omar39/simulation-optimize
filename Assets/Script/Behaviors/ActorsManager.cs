using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActorsManager : MonoBehaviour
{
    [SerializeField] private GPUObjectDrawer gpuActors;
    [SerializeField] private MarbleContainer marbleContainer;

    private List<List<ObjectData>> actorsPostions;
    private Dictionary<ObjectData, MarbleBehavior> targets = new Dictionary<ObjectData, MarbleBehavior>();

    private void Start()
    {
        actorsPostions = gpuActors.GetBatches();
        targets = new Dictionary<ObjectData, MarbleBehavior>();
        InvokeRepeating( nameof(UpdateActorsTargets), 0, 0.2f);
    }
    private void UpdateActorsTargets()
    {
        foreach(var batch in actorsPostions)
        {
            AssignMarbleToActor(batch);
        }
    }
    private void Update() 
    {
        foreach(var batch in actorsPostions)
        {
            MoveActorsToTargets(batch);
        }
    }
    private void AssignMarbleToActor(List<ObjectData> batch)
    {
        foreach(var actor in batch)
        {
            if(!targets.ContainsKey(actor) || targets[actor] == null)
            {
                MarbleBehavior target = marbleContainer.GetCloseMarbleToPosition( actor.position );
                targets[actor] = target;
            }
        }
    }
    private void MoveActorsToTargets(List<ObjectData> actors)
    {
        foreach(var actor in actors)
        {
            if(!targets.ContainsKey(actor) || targets[actor] == null) return;

            bool movedToTarget = MoveTo(ref actor.position, targets[actor].transform.position);    
            if(movedToTarget)
            {
                marbleContainer.ClaimMarble(targets[actor]);
                targets[actor] = null;
            }
        }
    }
    private bool MoveTo(ref Vector3 position, Vector3 target)
    {
        position = Vector3.MoveTowards(position, target, 10f * Time.deltaTime);
        return Vector3.Distance(position, target) < 0.1f;
    }

}
