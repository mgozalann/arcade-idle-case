using UnityEngine;
using UnityEngine.AI;

public class AIMovement : MonoBehaviour
{
    [SerializeField] private NavMeshAgent _agent;
    [SerializeField] private Animator _animator;
    
    [SerializeField] private Collectable _targetCollectable;
    [SerializeField] private Dropable _targetDropable;
    [SerializeField] private ObjectCollection _objectCollection;
    
    private static readonly int Speed = Animator.StringToHash("Speed");

    private void Start()
    {
        SetCollectDestination();
        
        _objectCollection.OnInventoryFull += SetDropDestination;
        _objectCollection.OnInventoryEmpty += SetCollectDestination;
    }

    private void OnDisable()
    {
        _objectCollection.OnInventoryFull -= SetDropDestination;
        _objectCollection.OnInventoryEmpty -= SetCollectDestination;
    }

    private void Update()
    {
        AnimationControl();
    }

    private void AnimationControl()
    {
        _animator.SetFloat(Speed, _agent.remainingDistance < 0.1f ? 0 : 1, .1f, Time.deltaTime);
    }

    private void GoToDestination(Transform destination)
    {
        _agent.SetDestination(destination.position);
    }

    private void SetCollectDestination()
    {
        GoToDestination(_targetCollectable.transform);
    }
    
    private void SetDropDestination()
    {
        GoToDestination(_targetDropable.transform);
    }
}