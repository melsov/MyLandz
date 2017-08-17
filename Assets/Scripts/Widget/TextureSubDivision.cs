using UnityEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MyLandz.WorkQueue;

public class TextureSubdivision : MonoBehaviour
{

    private Renderer _rendrr;
    private Renderer rendrr {
        get {
            if(!_rendrr) {
                _rendrr = GetComponent<Renderer>();
            }
            return _rendrr;
        }
    }

    private Material mat { get { return rendrr.material; } }

    private Vector4 gridSquare {
        get { return mat.GetVector("_GridSquare"); }
    }

    public int rows {
        get { return Mathf.Max(1, Mathf.RoundToInt(1f / gridSquare.y)); }
    }

    public int columns {
        get { return Mathf.Max(1, Mathf.RoundToInt(1f / gridSquare.x)); }
    }

    [SerializeField]
    private bool animateTransitions;
    [SerializeField]
    private float transitionTotalTimeS = .3f;
    [SerializeField]
    private int transitionFrameCount = 4;

    private WorkQueue _animationQueue;
    private WorkQueue animationQueue {
        get {
            if(!_animationQueue) {
                _animationQueue = ComponentHelper.AddIfNotPresent<WorkQueue>(transform);
                _animationQueue.purgeJobsThreshhold = 5;
            }
            return _animationQueue;
        }
    }

    private Vector4 offsetUV;


    public void setSubdivision(int x, int y) {
        x = x % columns;
        y = y % rows;
        setOffset(x / (float)columns, y / (float)rows);
    }

    public void setOffset(float x, float y) {
        if(animateTransitions) {
            animateTo(x, y);
            return;
        }
        offsetUV.x = x;
        offsetUV.y = y;
        mat.SetVector("_Offset", offsetUV);
    }

    private void animateTo(float x, float y) {
        Vector4 current = offsetUV;
        Vector4 next = new Vector4(x, y, 0f, 0f);
        if(Vector4.SqrMagnitude(current - next) < .001f) { return; }
        float frames = transitionFrameCount;
        float tick = transitionTotalTimeS / frames;
        float lerp = 0f;
        animationQueue.add(() => {
            lerp += 1f / frames;
            current = Vector4.Lerp(offsetUV, next, lerp);
            mat.SetVector("_Offset", current);
        }, (int count) => {
            return count > frames - 1;
        }, tick, () => {
            offsetUV = current;
        });
    }
}


