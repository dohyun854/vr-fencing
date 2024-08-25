using UnityEngine;
using OpenCvSharp;

public class WebCamCapture : MonoBehaviour
{
    private VideoCapture capture;
    private Mat frame;
    private CascadeClassifier bodyCascade;

    public int width = 640;
    public int height = 480;

    void Start()
    {
        capture = new VideoCapture(0);
        capture.Set(CaptureProperty.FrameWidth, width);
        capture.Set(CaptureProperty.FrameHeight, height);

        frame = new Mat();
        
        // Load Haar Cascade XML for body detection
        bodyCascade = new CascadeClassifier("Assets/haarcascade_fullbody.xml");

        if (bodyCascade.Empty())
        {
            Debug.LogError("Failed to load cascade classifier.");
        }
    }

    void Update()
    {
        if (capture.IsOpened())
        {
            capture.Read(frame);
            if (!frame.Empty())
            {
                DetectBody(frame);
                // Additional code to display or process the frame
            }
        }
    }

    private void DetectBody(Mat image)
    {
        Mat grayImage = new Mat();
        Cv2.CvtColor(image, grayImage, ColorConversionCodes.BGR2GRAY);

        Rect[] bodies = bodyCascade.DetectMultiScale(grayImage, scaleFactor: 1.1, minNeighbors: 3);

        foreach (var body in bodies)
        {
            Cv2.Rectangle(image, body, Scalar.Red);
        }
    }

    void OnDestroy()
    {
        if (capture != null)
        {
            capture.Release();
        }
        if (frame != null)
        {
            frame.Dispose();
        }
    }
}
