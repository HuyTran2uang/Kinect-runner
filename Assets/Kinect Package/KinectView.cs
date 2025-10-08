using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Windows.Kinect;

public class KinectView : MonoBehaviour
{
    // id: {JointType: gameobject}
    private Dictionary<ulong, GameObject> _bodyDict = new Dictionary<ulong, GameObject>();
    private Dictionary<ulong, Dictionary<JointType, GameObject>> _bodyView = new Dictionary<ulong, Dictionary<JointType, GameObject>>();
    [SerializeField] private GameObject _prefab;
    [SerializeField] private KinectManager _kinectManager;

    private void AddBody(Body body)
    {
        GameObject go = new GameObject($"Body {body.TrackingId}");
        _bodyDict.Add(body.TrackingId, go);
        _bodyView.Add(body.TrackingId, new Dictionary<JointType, GameObject>());
        foreach (var part in body.Joints)
        {
            AddPart(part.Key, body.TrackingId, go.transform);   
        }
    }

    private void AddPart(JointType type, ulong bodyIndex, Transform container)
    {
        var part = Instantiate(_prefab);
        part.transform.SetParent(container);
        part.name = $"{type}";
        _bodyView[bodyIndex].Add(type, part);
    }

    private void Update()
    {
        if (_kinectManager == null) return;
        if (_kinectManager.GetData() == null)
        {
            //Debug.Log("Data NULL!!!");
            return;
        }
        foreach (var body in _kinectManager.GetData())
        {
            if(body == null) continue;
            //Debug.Log("body");
            if (!_bodyView.ContainsKey(body.TrackingId))
            {
                AddBody(body);
            }

            if (body.IsTracked)
            {
                //Debug.Log("body tracked");
                UpdateBody(body);
            }
        }
    }

    private static Vector3 GetVector3FromJoint(Windows.Kinect.Joint joint)
    {
        return new Vector3(joint.Position.X, joint.Position.Y, joint.Position.Z);
    }

    private void UpdateBody(Body body)
    {
        ulong id = body.TrackingId;
        foreach (var joint in body.Joints)
        {
            if (_bodyView[id].ContainsKey(joint.Key))
            {
                Vector3 position = GetVector3FromJoint(joint.Value);
                //Debug.Log($"{joint.Key}: {position}");
                _bodyView[id][joint.Key].transform.position = position;
            }
        }
    }
}
