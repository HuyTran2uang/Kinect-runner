using UnityEngine;
using Windows.Kinect;

public class KinectManager : MonoBehaviour
{
    private KinectSensor _sensor;
    private BodyFrameReader _reader;
    private Body[] _data = null;

    public Body[] GetData() => _data;

    private void Start()
    {
        _sensor = KinectSensor.GetDefault();

        if (_sensor != null)
        {
            _reader = _sensor.BodyFrameSource.OpenReader();
            if (!_sensor.IsOpen)
            {
                _sensor.Open();
            }
            Debug.Log($"Kinect is opening: " + _sensor.IsOpen);
        }
    }

    void Update()
    {
        if (_reader != null)
        {
            //Debug.Log("Reading...");
            var frame = _reader.AcquireLatestFrame();
            if (frame != null)
            {
                    //Debug.Log("frame not null...");
                if (_data == null)
                {
                    //Debug.Log("data not null...");
                    _data = new Body[_sensor.BodyFrameSource.BodyCount];
                }

                frame.GetAndRefreshBodyData(_data);
                frame.Dispose();
                frame = null;
            }
        }
    }

    void OnApplicationQuit()
    {
        if (_reader != null)
        {
            _reader.Dispose();
            _reader = null;
        }

        if (_sensor != null)
        {
            if (_sensor.IsOpen)
            {
                _sensor.Close();
            }

            _sensor = null;
        }
    }
}
