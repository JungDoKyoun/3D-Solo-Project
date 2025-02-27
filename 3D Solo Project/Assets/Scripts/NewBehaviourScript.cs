using UnityEngine;

public class ScreenshotCapture : MonoBehaviour
{
    public Camera screenshotCamera;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))  // P키를 누르면 스크린샷
        {
            CaptureScreenshot();
            Debug.Log("찍힘");
        }
    }

    void CaptureScreenshot()
    {
        // 정사각형 크기로 RenderTexture 설정
        RenderTexture rt = new RenderTexture(512, 512, 24);  // 512x512 크기
        screenshotCamera.targetTexture = rt;
        screenshotCamera.Render();
        RenderTexture.active = rt;

        // 캡처할 텍스처 크기 설정 (512x512)
        Texture2D screenshot = new Texture2D(512, 512, TextureFormat.RGB24, false);
        screenshot.ReadPixels(new Rect(0, 0, 512, 512), 0, 0);
        screenshot.Apply();

        // 이미지 저장
        byte[] bytes = screenshot.EncodeToPNG();
        System.IO.File.WriteAllBytes("Assets/ItemScreenshot.png", bytes);

        // 정리
        screenshotCamera.targetTexture = null;
        RenderTexture.active = null;
        Destroy(rt);
    }
}