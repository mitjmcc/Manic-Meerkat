using UnityEngine;

public class SplineWalker : MonoBehaviour {

	public BezierSpline spline;

	public float duration;

	public bool lookForward;

	[Range(0, 1)]
	public float lookAtPlayerAmount;

	public SplineWalkerMode mode;

	private float progress;

	public GameObject idealTransform;

	public Transform player;

	private void Start () {
		idealTransform.transform.localPosition = transform.localPosition;
		idealTransform.transform.localRotation = transform.localRotation;
	}

	public void Reset () {
		progress = 0;
		idealTransform.transform.position = spline.GetPoint(0);
		transform.position = idealTransform.transform.position;
	}

	private void Update () {
		Vector3 toPlayer = Vector3.ProjectOnPlane (player.position, Vector3.up) - Vector3.ProjectOnPlane (idealTransform.transform.position, Vector3.up);
		Vector3 moveVector = Vector3.Project (toPlayer, idealTransform.transform.forward);
		Vector3 position = spline.GetPoint(progress);

		bool moving = moveVector.magnitude > 12f || moveVector.magnitude < 10f;
		bool movingForward = moveVector.magnitude > 12f;

		int sanity = 0;
		if (moving)
		{
			while ((moveVector.magnitude > 12f || moveVector.magnitude < 10f) && sanity < 10) {
				progress += movingForward ? 0.001f : -0.001f;
				progress = Mathf.Clamp (progress, 0, 1);
				sanity++;
				position = spline.GetPoint (progress);
				idealTransform.transform.localPosition = position;
				toPlayer = Vector3.ProjectOnPlane (player.position, Vector3.up) - Vector3.ProjectOnPlane (idealTransform.transform.position, Vector3.up);
				moveVector = Vector3.Project (toPlayer, idealTransform.transform.forward);
			}
		}

		if (lookForward) {
			Vector3 lookAt = position + Vector3.ProjectOnPlane(spline.GetDirection (progress), Vector3.up);
			idealTransform.transform.LookAt(Vector3.Lerp(lookAt, player.position, lookAtPlayerAmount));
		}

		transform.localPosition = Vector3.Lerp (transform.localPosition, idealTransform.transform.localPosition, Time.deltaTime * 4f);
		transform.localRotation = Quaternion.Lerp (transform.localRotation, idealTransform.transform.localRotation, Time.deltaTime * 4f);
	}
}