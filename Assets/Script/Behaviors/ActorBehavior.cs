using System.Collections;
using UnityEngine;

public class ActorBehavior : MonoBehaviour
{
    private enum State
    {
        Idle,
        Hunting,
    }

    public MarbleContainer ContainerReference { get; set; }
    private State _currentState;
    private MarbleBehavior _currentTarget;

    void Start()
    {
        //StopAllCoroutines();
        //StartCoroutine( Patrol() );
        // _currentState = State.Idle;
    }
    IEnumerator Patrol()
    {
        while( true )
        {
            yield return StartCoroutine( SearchForMarble() );
            yield return StartCoroutine( Hunt() );

            yield return new WaitForSeconds( Random.Range( 0.5f, 1.5f ) );
        }
    }

    // void Update()
    // {
    //     switch( _currentState )
    //     {
    //         case State.Idle:
    //             UpdateIdle();
    //             break;
    //         case State.Hunting:
    //             UpdateMoving();
    //             break;
    //     }
    // }
    IEnumerator Hunt()
    {
        _currentState = State.Hunting;
        yield return new WaitUntil( () => 
        {
            transform.position = Vector3.MoveTowards(
                transform.position, _currentTarget.transform.position, 10 * Time.deltaTime);

            return Vector3.Distance(_currentTarget.transform.position, transform.position) < 0.1f;
        });

        ClaimMarble();
        
        yield return new WaitForEndOfFrame();
    }
    IEnumerator SearchForMarble()
    {
        _currentState = State.Idle;
        
        _currentTarget = ContainerReference.GetCloseMarbleToPosition( this.transform.position );
        while(_currentTarget == null)
        {
            _currentTarget = ContainerReference.GetCloseMarbleToPosition( this.transform.position );
            yield return new WaitForEndOfFrame();
        }
        StartCoroutine( Hunt() );
    }

    private void UpdateIdle()
    {
        _currentTarget = ContainerReference.GetCloseMarbleToPosition( this.transform.position );
        if( _currentTarget != null )
        {
            _currentState = State.Hunting;
        }
    }

    public void ClaimMarble()
    {
        ContainerReference.ClaimMarble( _currentTarget );
        _currentTarget = null;
        _currentState = State.Idle;
    }

    private void UpdateMoving()
    {
        if( _currentTarget.WasClaimed )
        {
            _currentTarget = null;
            _currentState = State.Idle;
            return;
        }

        var thisToTarget = _currentTarget.transform.position - this.transform.position;
        var thisToTargetDirection = thisToTarget.normalized;
        this.transform.position += thisToTargetDirection *10* Time.deltaTime;

        if( thisToTarget.magnitude < 0.1f )
        {
            ClaimMarble();
        }
    }
}
