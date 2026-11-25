using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[Serializable]
public class InteractionActorEvent : UnityEvent<GameObject>{}

/// <summary>
/// Detects a tagged actor entering, staying inside, or leaving this trigger area.
/// Interaction behavior is connected through the UnityEvents in the Inspector.
/// </summary>
[RequireComponent(typeof(Collider2D))]
public class InteractionSensor2D : MonoBehaviour
{
    [SerializeField] private string actorTag = "Player";
    [SerializeField] private InteractionActorEvent onActorEntered = new InteractionActorEvent();
    [SerializeField] private InteractionActorEvent onActorStayed = new InteractionActorEvent();
    [SerializeField] private InteractionActorEvent onActorExited = new InteractionActorEvent();
    [SerializeField] private bool showDetectionRadius = true;
    [SerializeField] private Color detectionRadiusColor = Color.cyan;
    [SerializeField, Min(12)] private int circleSegments = 48;

    private readonly HashSet<Collider2D> trackedColliders = new HashSet<Collider2D>();
    private readonly Dictionary<GameObject, int> colliderCounts = new Dictionary<GameObject, int>();
    private readonly Dictionary<GameObject, int> lastStayFrame = new Dictionary<GameObject, int>();
    private readonly HashSet<GameObject> actorsBlockedUntilExit = new HashSet<GameObject>();
    private readonly Dictionary<Collider2D, GameObject> blockedColliders = new Dictionary<Collider2D, GameObject>();
    private readonly Dictionary<GameObject, int> blockedColliderCounts = new Dictionary<GameObject, int>();

    private void Awake()
    {
        Collider2D area = GetComponent<Collider2D>();
        if (!area.isTrigger)
            Debug.LogWarning("El collider del InteractionSensor2D debe tener Is Trigger activado.", this);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        GameObject actor = FindActor(other);
        if (actor == null || !trackedColliders.Add(other))
            return;

        if (actorsBlockedUntilExit.Contains(actor))
        {
            blockedColliders[other] = actor;
            blockedColliderCounts.TryGetValue(actor, out int blockedCount);
            blockedColliderCounts[actor] = blockedCount + 1;
            return;
        }

        colliderCounts.TryGetValue(actor, out int count);
        colliderCounts[actor] = count + 1;

        if (count == 0)
            onActorEntered.Invoke(actor);
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        GameObject actor = FindActor(other);
        if (actor == null || !colliderCounts.ContainsKey(actor))
            return;

        if (lastStayFrame.TryGetValue(actor, out int frame) && frame == Time.frameCount)
            return;

        lastStayFrame[actor] = Time.frameCount;
        onActorStayed.Invoke(actor);
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (!trackedColliders.Remove(other))
            return;

        if (blockedColliders.TryGetValue(other, out GameObject blockedActor))
        {
            blockedColliders.Remove(other);
            int coun = blockedColliderCounts[blockedActor] - 1;
            if (coun == 0)
            {
                blockedColliderCounts.Remove(blockedActor);
                actorsBlockedUntilExit.Remove(blockedActor);
            }
            else
            {
                blockedColliderCounts[blockedActor] = coun;
            }

            return;
        }

        GameObject actor = FindActor(other);
        if (actor == null || !colliderCounts.TryGetValue(actor, out int count))
            return;

        if (count > 1)
        {
            colliderCounts[actor] = count - 1;
            return;
        }

        colliderCounts.Remove(actor);
        lastStayFrame.Remove(actor);
        onActorExited.Invoke(actor);
    }

    /// <summary>Ignores this actor until all of its colliders leave this interaction area.</summary>
    public void BlockActorUntilExit(GameObject actor)
    {
        if (actor == null || !actorsBlockedUntilExit.Add(actor))
            return;

        if (!colliderCounts.ContainsKey(actor))
            return;

        onActorExited.Invoke(actor);
        colliderCounts.Remove(actor);
        lastStayFrame.Remove(actor);

        List<Collider2D> actorColliders = new List<Collider2D>();
        foreach (Collider2D trackedCollider in trackedColliders)
        {
            if (trackedCollider != null && FindActor(trackedCollider) == actor)
                actorColliders.Add(trackedCollider);
        }

        blockedColliderCounts[actor] = actorColliders.Count;
        foreach (Collider2D actorCollider in actorColliders)
            blockedColliders[actorCollider] = actor;
    }

    private GameObject FindActor(Collider2D other)
    {
        Rigidbody2D attachedBody = other.attachedRigidbody;
        GameObject actor = attachedBody != null
            ? attachedBody.gameObject
            : other.transform.root.gameObject;

        if (actor.CompareTag(actorTag))
            return actor;

        GameObject root = other.transform.root.gameObject;
        return root.CompareTag(actorTag) ? root : null;
    }

    private void OnDrawGizmos()
    {
        if (!showDetectionRadius)
            return;

        CircleCollider2D circle = GetComponent<CircleCollider2D>();
        if (circle == null)
            return;

        Gizmos.color = detectionRadiusColor;
        int segments = Mathf.Max(12, circleSegments);
        Vector3 previousPoint = circle.transform.TransformPoint(
            circle.offset + new Vector2(circle.radius, 0f));

        for (int i = 1; i <= segments; i++)
        {
            float angle = i * Mathf.PI * 2f / segments;
            Vector2 localPoint = circle.offset + new Vector2(
                Mathf.Cos(angle) * circle.radius,
                Mathf.Sin(angle) * circle.radius);
            Vector3 nextPoint = circle.transform.TransformPoint(localPoint);

            Gizmos.DrawLine(previousPoint, nextPoint);
            previousPoint = nextPoint;
        }
    }

    private void OnDisable()
    {
        foreach (GameObject actor in colliderCounts.Keys)
            onActorExited.Invoke(actor);

        trackedColliders.Clear();
        colliderCounts.Clear();
        lastStayFrame.Clear();
        blockedColliders.Clear();
        blockedColliderCounts.Clear();
        actorsBlockedUntilExit.Clear();
    }
}
