using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SimpleRoomButton : MonoBehaviour {
    private void OnEnable() {
        GetComponent<Button>()?.onClick.AddListener(OnButtonClick);
    }
    private void OnDisable() {
        GetComponent<Button>()?.onClick.RemoveListener(OnButtonClick);
    }

    private void OnButtonClick() {
        StartCoroutine(Highlight());
    }

    private IEnumerator Highlight() {
        var image = GetComponent<Button>()?.image;
        if (image) {
            image.color = Color.red;
            while (image.color != Color.white) {
                image.color += 0.1f * new Color(0f, 1f, 1f, 0f);
                yield return new WaitForSeconds(0.1f);
            }
        }
    }
}
