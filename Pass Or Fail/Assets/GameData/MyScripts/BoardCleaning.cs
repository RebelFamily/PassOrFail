using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardCleaning : MonoBehaviour
{
    [SerializeField] private Camera camera;

    [SerializeField] private Texture2D dirtMaskBase, brush;
    private Texture2D _templateDirtMask;

    [SerializeField] private Material material;

    private static readonly int DirtTexture = Shader.PropertyToID("_DirtMask");

    // Start is called before the first frame update
    private void Start()
    {
        CreateTexture();
    }

    // Update is called once per frame
    private void FixedUpdate()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (Physics.Raycast(camera.ScreenPointToRay(Input.mousePosition), out RaycastHit hit))
            {
                var textureCoord = hit.textureCoord;

                var pixelX = (int)(textureCoord.x * _templateDirtMask.width);
                var pixelY = (int)(textureCoord.y * _templateDirtMask.height);
                
                var pixelXOffset = pixelX - (brush.width / 2);
                var pixelYOffset = pixelY - (brush.height / 2);

                for (var x = 0; x < brush.width; x++)
                {
                    for (var y = 0; y < brush.height; y++)
                    {
                        var pixelDirt = brush.GetPixel(x, y);
                        var pixelDirtMask = _templateDirtMask.GetPixel(pixelXOffset + x, pixelYOffset + y);

                        _templateDirtMask.SetPixel(pixelXOffset + x,
                            pixelYOffset + y,
                            new Color(0, pixelDirtMask.g * pixelDirt.g, 0));
                    }
                }

                _templateDirtMask.Apply();
            }
        }
    }

    private void CreateTexture()
    {
        _templateDirtMask = new Texture2D(dirtMaskBase.width, dirtMaskBase.height);
        _templateDirtMask.SetPixels(dirtMaskBase.GetPixels());
        _templateDirtMask.Apply();
        material.SetTexture(DirtTexture, _templateDirtMask);
    }
}
