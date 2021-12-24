using UnityEngine;
using UnityEngine.AI;

public class Colonist : MonoBehaviour
{
    private NavMeshAgent _agent;
    private Animator _animator;

    public SurfaceGenerator SurfaceGen;
    public bool BuildAssigned;
    public bool Moving => _agent.remainingDistance > _agent.stoppingDistance;
    public bool Building;
    public float BuildRatio;
    public bool InsideBuildDraft;

    public BuildDraft AssignedBuildDraft = null;

    private ColonistStateMachine _stateMachine;
    private NavMeshPath _pathToDraw;

    public ColonistIdleState IdleState { get; private set; }
    public ColonistWalkState WalkState { get; private set; }
    public ColonistBuildState BuildState { get; private set; }

    private void Awake()
    {
        _agent = GetComponent<NavMeshAgent>();
        //_agent.autoRepath = true;
        _animator = GetComponent<Animator>();
        _stateMachine = new ColonistStateMachine();
        IdleState = new ColonistIdleState(this, _animator, _stateMachine, "idle");
        WalkState = new ColonistWalkState(this, _animator, _stateMachine, "walk");
        BuildState = new ColonistBuildState(this, _animator, _stateMachine, "build");
    }
    private void Start()
    {
        _stateMachine.Initialize(IdleState);
    }
    private void Update()
    {
        _stateMachine.CurrentState.LogicUpdate();
        if (_pathToDraw != null)
            DrawPath(_pathToDraw);
    }
    private void FixedUpdate()
    {
        _stateMachine.CurrentState.PhysicsUpdate();
    }
    public void Move(Vector3 positon)
    {
        _agent.SetDestination(positon);
    }
    public void AssignBuildDraft(BuildDraft buildDraft)
    {
        AssignedBuildDraft = buildDraft;
        AssignedBuildDraft.Assigned = true;
        BuildAssigned = true;
    }
    public void DeassingBuildDraft()
    {
        AssignedBuildDraft.GetComponent<Collider>().enabled = false;
        AssignedBuildDraft.enabled = false;
        AssignedBuildDraft = null;
        BuildAssigned = false;
        Building = false;
    }
    public bool ReachableDestination(Vector3 position)
    {
        NavMeshPath path = new NavMeshPath();
        _agent.CalculatePath(position, path);
        if (path.corners.Length > 0)
        {
            bool validPath = path.status == NavMeshPathStatus.PathComplete && Vector3.Distance(position ,path.corners[path.corners.Length - 1]) <= 1f;
            if (validPath)
            {
                _pathToDraw = path;
            }
            return validPath;
        }
        return false;
    }
    public void RecalculatePath()
    {
        Vector3 destination = _agent.pathEndPosition;
        if (AssignedBuildDraft != null)
        {
            for (int i = 0; i < AssignedBuildDraft.PointsAround.Length; i++)
            {
                Vector3 currentPosition = AssignedBuildDraft.PointsAround[i];
                if (destination != currentPosition)
                {
                    if (ReachableDestination(currentPosition))
                    {
                        Move(currentPosition);
                        return;
                    }
                }
            }
        }
        InsideBuildDraft = false;
    }

    public void DrawPath(NavMeshPath path)
    {
        for (int i = 0; i < path.corners.Length - 1; i++)
            Debug.DrawLine(path.corners[i], path.corners[i + 1], Color.red);
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "BuildDraft")
        {
            InsideBuildDraft = true;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "BuildDraft")
        {
            InsideBuildDraft = false;
        }
    }
}
