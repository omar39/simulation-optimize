using System;
using System.Collections;

using UnityEngine;
using UnityEngine.Events;

public class MarbleBehavior : MonoBehaviour
{
    public Guid Id { get; set; }
    private bool _wasClaimed;
    public bool WasClaimed
    {
        get
        {
            return _wasClaimed;
        }
        set
        {
            if( !_wasClaimed && value )
            {
                StartCoroutine( DisplayScore() );
            }
            _wasClaimed = value;
        }
    }
    // moving out the destroy behavior to marble container
    public UnityAction<MarbleBehavior> OnClaimed { get; set; }
    public float Value { get; private set; }

    [SerializeField] private Transform _textboxContainer;
    [SerializeField] private TextMesh _textmesh;

    void Start()
    {
        Value = UnityEngine.Random.value * 100f - 25f;
        //_textmesh = this.transform.Find( "TextboxContainer/Textbox/ScoreText" ).gameObject.GetComponent<TextMesh>();
        _textmesh.text = Value.ToString( "##.#" );
        //_textboxContainer = this.transform.Find( "TextboxContainer" );
        _textboxContainer.gameObject.SetActive( false );
        WasClaimed = false;
    }

    private IEnumerator DisplayScore()
    {
        var steps = 60;
        _textboxContainer.localScale = Vector3.zero;
        _textboxContainer.gameObject.SetActive( true );
        for( var i = 0; i < steps; i++ )
        {
            _textboxContainer.localScale += Vector3.one / steps;
            yield return new WaitForEndOfFrame();
        }
        OnClaimed?.Invoke(this);
    }
}
