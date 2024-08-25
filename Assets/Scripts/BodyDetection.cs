using OpenCVForUnity.CoreModule;
using OpenCVForUnity.UnityUtils.Helper;
using UnityEngine;

public class BodyDetection : MonoBehaviour
{
    public WebCamTextureToMatHelper webCamTextureToMatHelper;
    private Mat rgbaMat;
    private CascadeClassifier bodyCascade;

    void Start()
    {
        webCamTextureToMatHelper.Initialize();
        bodyCascade = new CascadeClassifier(Utils.getFilePath("haarcascade_fullbody.xml")); // Haar Cascade 파일 경로 설정
    }

    void Update()
    {
        if (webCamTextureToMatHelper.IsPlaying() && webCamTextureToMatHelper.DidUpdateThisFrame())
        {
            rgbaMat = webCamTextureToMatHelper.GetMat();
            DetectBody();
        }
    }

    void DetectBody()
    {
        Mat grayMat = new Mat();
        Imgproc.cvtColor(rgbaMat, grayMat, Imgproc.COLOR_RGBA2GRAY);

        MatOfRect bodies = new MatOfRect();
        bodyCascade.detectMultiScale(grayMat, bodies, 1.1, 2, 0, new Size(30, 30), new Size());

        foreach (Rect rect in bodies.toArray())
        {
            Imgproc.rectangle(rgbaMat, rect.tl(), rect.br(), new Scalar(0, 255, 0, 255), 2);
        }
    }

    void OnDestroy()
    {
        webCamTextureToMatHelper.Dispose();
    }
}
