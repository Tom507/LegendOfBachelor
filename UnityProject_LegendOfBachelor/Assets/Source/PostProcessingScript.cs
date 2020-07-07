using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

public class PostProcessingScript : MonoBehaviour
{
    Player player;
    Camera postCamera;
    PostProcessVolume ppVolume;

    DepthOfField depthOfField;
    public float focalLengthPow = 1.3f;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
        postCamera = gameObject.GetComponent<Camera>();
        ppVolume = postCamera.gameObject.GetComponent<PostProcessVolume>();

        ppVolume.profile.TryGetSettings<DepthOfField>(out depthOfField);
    }

    // Update is called once per frame
    void Update()
    {
        float distanceCameraPlayer = (player.transform.position - postCamera.gameObject.transform.position).magnitude;
        depthOfField.focusDistance.Override( distanceCameraPlayer);

        depthOfField.focalLength.Override(Mathf.Clamp(Mathf.Pow(distanceCameraPlayer, focalLengthPow), 61f, 300f ));
    }
}
