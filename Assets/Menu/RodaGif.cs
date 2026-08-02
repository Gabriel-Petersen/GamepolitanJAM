using UnityEngine;
using UnityEngine.UI;

public class RodaGif : MonoBehaviour
{
    public TextAsset gifAsset;
    public RawImage rawImage;

    void Start()
    {
        StartCoroutine(UniGif.GetTextureListCoroutine(gifAsset.bytes, (gifTexList, loopCount, width, height) => {
            if (gifTexList != null && gifTexList.Count > 0)
            {
                StartCoroutine(PlayGif(gifTexList));
            }
        }));
    }

    System.Collections.IEnumerator PlayGif(System.Collections.Generic.List<UniGif.GifTexture> gifTexList)
    {
        int index = 0;
        while (true)
        {
            rawImage.texture = gifTexList[index].m_texture2d;
            yield return new WaitForSeconds(gifTexList[index].m_delaySec);
            index = (index + 1) % gifTexList.Count;
        }
    }
}