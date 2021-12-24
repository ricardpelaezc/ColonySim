using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ColonistManager : MonoBehaviour
{
    public static List<Colonist> Colonists;
    public static List<BuildDraft> BuildDrafts;

    private void Start()
    {
        Colonists = FindObjectsOfType<Colonist>().ToList();
        BuildDrafts = FindObjectsOfType<BuildDraft>().ToList();
    }

    private void Update()
    {
        SetWork();

        if (Input.GetKey(KeyCode.LeftControl))
        {
            Time.timeScale = 4;
        }
        else
        {
            Time.timeScale = 1;
        }
    }

    private void SetWork()
    {
        Colonist[] colonists = NotOccupied();
        foreach (var colonist in colonists)
        {
            Tuple<BuildDraft, Vector3> result = NearestRecheableDeassignedBuildDraft(colonist);
            if (!(result is null))
            {
                if (result.Item1 != default(BuildDraft))
                {
                    SetBuildDraft(colonist, result.Item1, result.Item2);
                }
            }
        }
    }
    private void SetBuildDraft(Colonist colonist, BuildDraft buildDraft, Vector3 position)
    {

        colonist.AssignBuildDraft(buildDraft);
        colonist.Move(position);
        return;
    }
    private Colonist[] NotOccupied()
    {
        return Colonists.Where(x => !x.BuildAssigned).ToArray();
    }
    private BuildDraft[] Deassigned()
    {
        return BuildDrafts.Where(x => !x.Assigned).ToArray();
    }
    private Colonist NearestNotOccupiedColonist(Vector3 position)
    {
        Colonist[] notOccupiedColonist = NotOccupied();

        if (notOccupiedColonist.Length == 0)
        {
            return null;
        }

        Colonist nearestColonist = notOccupiedColonist[0];
        float nearestColonistDistance = Vector3.Distance(nearestColonist.transform.position, position);

        foreach (Colonist c in notOccupiedColonist)
        {
            float currentDistance = Vector3.Distance(c.transform.position, position);
            if (currentDistance < nearestColonistDistance)
            {
                nearestColonist = c;
                nearestColonistDistance = currentDistance;
            }
        }
        return nearestColonist;
    }
    private Tuple<BuildDraft, Vector3> NearestRecheableDeassignedBuildDraft(Colonist colonist)
    {
        BuildDraft[] deassignedBuildDraft = Deassigned();

        if (deassignedBuildDraft.Length == 0)
        {
            return null;
        }

        BuildDraft nearestBuildDraft = default(BuildDraft);
        Vector3 nearestBuildDraftPosition = default(Vector3);
        float nearestBuildDraftDistance = Mathf.Infinity;

        foreach (BuildDraft b in deassignedBuildDraft)
        {
            float currentDistance = Vector3.Distance(b.transform.position, colonist.transform.position);
            if (nearestBuildDraft == default(BuildDraft))
            {
                Vector3 position = NearestRecheablePosition(b, colonist);
                if (position != default(Vector3))
                {
                    nearestBuildDraft = b;
                    nearestBuildDraftPosition = position;
                    nearestBuildDraftDistance = currentDistance;
                }
            }
            else if (currentDistance < nearestBuildDraftDistance)
            {
                Vector3 position = NearestRecheablePosition(b, colonist);
                if (position != default(Vector3))
                {
                    nearestBuildDraft = b;
                    nearestBuildDraftPosition = position;
                    nearestBuildDraftDistance = currentDistance;
                }
            }
        }
        return new Tuple<BuildDraft, Vector3>(nearestBuildDraft, nearestBuildDraftPosition);
    }
    private Vector3 NearestRecheablePosition(BuildDraft buildDraft, Colonist colonist)
    {
        Vector3 nearestPosition = default(Vector3);
        float nearestPositionDistance = Mathf.Infinity;

        foreach (var position in buildDraft.PointsAround)
        {
            float currentDistance = Vector3.Distance(position, colonist.transform.position);
            if (nearestPosition == default(Vector3))
            {
                if (colonist.ReachableDestination(position))
                {
                    nearestPosition = position;
                    nearestPositionDistance = currentDistance;
                }
            }
            else if (currentDistance < nearestPositionDistance)
            {
                nearestPosition = position;
                nearestPositionDistance = currentDistance;
            }

        }
        return nearestPosition;
    }
}
