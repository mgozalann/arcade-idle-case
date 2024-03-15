using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

public class SheepBehaviour : MonoBehaviour
{
    private NavMeshAgent _agent;
    private Animator _animator;

    [SerializeField] private float _radius = 10f;
    
    [SerializeField] private float _minPauseDuration = 2f; 
    [SerializeField] private float _maxPauseDuration = 5f;

    private float _nextMoveTime;
    private static readonly int Speed = Animator.StringToHash("Speed");

    private Vector3 _destinationPos;
    private float _timer;
    private void Start()
    {
        _timer = Random.Range(_minPauseDuration, _maxPauseDuration);
        
        _agent = GetComponent<NavMeshAgent>();
        _animator = GetComponent<Animator>();

        SetRandomDestination();
    }

    private void Update()
    {
        
        if (_agent.remainingDistance < 0.1f)
        {
            
            _animator.SetFloat(Speed, 0, 0.1f, Time.deltaTime); // Burada dördüncü parametreyi kaldırın
            
            float pauseDuration = Random.Range(_minPauseDuration, _maxPauseDuration);
            
            _timer -= Time.deltaTime;

            if (_timer <= 0)
            {
                MoveToNextPoint();
                
                _timer = pauseDuration;
            }
        }
        else
        {
            _animator.SetFloat(Speed, 1,.1f,Time.deltaTime);
        }
    }

    void SetRandomDestination()
    {
        Vector3 randomDirection = Random.insideUnitSphere * _radius;
        randomDirection += transform.position;
        NavMeshHit hit;

        if (NavMesh.SamplePosition(randomDirection, out hit, _radius, 1))
        {
            _destinationPos = hit.position;
        }

        _agent.SetDestination(_destinationPos);
    }
    
    void MoveToNextPoint()
    {
        SetRandomDestination();
    }
}