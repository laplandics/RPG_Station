using UnityEngine;
using UnityEngine.UI;

public class TypeMapDebugUI : MonoBehaviour
{
    private static readonly int TypeMap = Shader.PropertyToID("_TypeMap");
    [SerializeField] private RawImage image;

    private void Start()
    {
        image.texture = Shader.GetGlobalTexture(TypeMap);
    }
}
