using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EaseFunctions
{
    public enum Easing
    {
        Linear = 0,
        EaseIn,
        EaseOut,
        EaseInOut
    }
    public enum Axis
    {
        X = 0,
        Y,
        Z,
        XY,
        XZ,
        YZ,
        XYZ
    }
    public enum Format
    {
        Standard,
        Scalar,
        Percentage
    }

    public class EasingFunctions : MonoBehaviour
    {
        #region //Translation
        public static IEnumerator TranslateEase(GameObject Obj, Vector2 start, Vector2 target, float duration, uint power = 2, Easing Ease = Easing.Linear,
                                                 uint delay = 1, uint iterationCount = 1, bool isReturn = false, bool isReverse = false)
        {
            float time = 0f;
            iterationCount = Math.Max(1, iterationCount);
            duration *= (isReturn ? 0.5f : 1f);

            Vector3 startPos = Camera.main.ScreenToWorldPoint(new Vector3(start.x, start.y, Obj.transform.position.z));
            Vector3 targetPos = Camera.main.ScreenToWorldPoint(new Vector3(target.x, target.y, Obj.transform.position.z));

            Vector3 SP = (isReverse ? targetPos : startPos);
            Vector3 TP = (isReverse ? startPos : targetPos);

            while (iterationCount != 0)
            {
                int returnPos = (isReturn ? 1 : -1);
                Obj.transform.position = SP;

                while (returnPos != 0)
                {
                    while (time <= duration)
                    {
                        Vector3 currentPosition = SP;

                        if (Ease == Easing.Linear)
                        {
                            currentPosition.x = Mathf.Lerp(SP.x, TP.x, time / duration);
                            currentPosition.y = Mathf.Lerp(SP.y, TP.y, time / duration);
                        }
                        if (Ease == Easing.EaseIn)
                        {
                            currentPosition.x = Mathf.Lerp(SP.x, TP.x, EaseIn(time / duration, power));
                            currentPosition.y = Mathf.Lerp(SP.y, TP.y, EaseIn(time / duration, power));
                        }
                        if (Ease == Easing.EaseOut)
                        {
                            currentPosition.x = Mathf.Lerp(SP.x, TP.x, EaseOut(time / duration, power));
                            currentPosition.y = Mathf.Lerp(SP.y, TP.y, EaseOut(time / duration, power));
                        }
                        if (Ease == Easing.EaseInOut)
                        {
                            currentPosition.x = Mathf.Lerp(SP.x, TP.x, EaseInOut(time / duration, power));
                            currentPosition.y = Mathf.Lerp(SP.y, TP.y, EaseInOut(time / duration, power));
                        }

                        Obj.transform.position = currentPosition;

                        Vector3 pp = Camera.main.WorldToScreenPoint(new Vector3(SP.x, TP.y, 10));
                        //Debug.Log("Time: " + time);
                        //Debug.Log("Position: (" + pp.x + "," + pp.y + ")");
                        yield return null;
                        time += Time.deltaTime;
                    }
                    time = 0f;
                    returnPos = (returnPos == 1 ? -1 : returnPos + 1);

                    if (returnPos == -1)
                    {
                        Vector3 temp = SP;
                        SP = TP;
                        TP = temp;
                    }
                }
                iterationCount--;
            }
        }

        public static IEnumerator TranslateTo(GameObject Obj, Vector3 target, float duration, uint power = 2, Easing Ease = Easing.Linear)
        {
            float time = 0f;

            Vector3 startPos = new Vector3(Obj.transform.position.x, Obj.transform.position.y, Obj.transform.position.z);
            Vector3 targetPos = new Vector3(target.x, target.y, Obj.transform.position.z);

            Vector3 SP = startPos;
            Vector3 TP = targetPos;

            Obj.transform.position = SP;

            while (time <= duration)
            {
                Vector3 currentPosition = SP;

                if (Ease == Easing.Linear)
                {
                    currentPosition.x = Mathf.Lerp(SP.x, TP.x, time / duration);
                    currentPosition.y = Mathf.Lerp(SP.y, TP.y, time / duration);
                }
                if (Ease == Easing.EaseIn)
                {
                    currentPosition.x = Mathf.Lerp(SP.x, TP.x, EaseIn(time / duration, power));
                    currentPosition.y = Mathf.Lerp(SP.y, TP.y, EaseIn(time / duration, power));
                }
                if (Ease == Easing.EaseOut)
                {
                    currentPosition.x = Mathf.Lerp(SP.x, TP.x, EaseOut(time / duration, power));
                    currentPosition.y = Mathf.Lerp(SP.y, TP.y, EaseOut(time / duration, power));
                }
                if (Ease == Easing.EaseInOut)
                {
                    currentPosition.x = Mathf.Lerp(SP.x, TP.x, EaseInOut(time / duration, power));
                    currentPosition.y = Mathf.Lerp(SP.y, TP.y, EaseInOut(time / duration, power));
                }

                Obj.transform.position = currentPosition;

                //Vector3 pp = Camera.main.WorldToScreenPoint(new Vector3(SP.x, TP.y, 10));
                //Debug.Log("Time: " + time);
                yield return null;
                time += Time.deltaTime;
            }
            Obj.transform.position = new Vector3(TP.x, TP.y, Obj.transform.position.z);
            time = 0f;
        }
        #endregion

        #region //Scale
        public static IEnumerator ScaleEase(GameObject Obj, Vector2 origScale, Vector2 finalScale, float duration, uint power = 2, Easing Ease = Easing.Linear,
                                                   uint delay = 1, uint iterationCount = 1, bool isReturn = false, bool isReverse = false)
        {
            float time = 0f;
            iterationCount = Math.Max(1, iterationCount);
            duration *= (isReturn ? 0.5f : 1f);

            Vector2 origScl = new Vector3(origScale.x, origScale.y);
            Vector2 finalScl = new Vector3(finalScale.x, finalScale.y);

            Vector2 OS = (isReverse ? finalScale : origScale);
            Vector2 FS = (isReverse ? origScale : finalScale);

            while (iterationCount != 0)
            {
                int returnScale = (isReturn ? 1 : -1);
                Obj.transform.localScale = OS;

                while (returnScale != 0)
                {
                    while (time <= duration)
                    {
                        Vector2 currentScale = OS;

                        if (Ease == Easing.Linear)
                        {
                            currentScale.x = Mathf.Lerp(OS.x, FS.x, time / duration);
                            currentScale.y = Mathf.Lerp(OS.y, FS.y, time / duration);
                        }
                        if (Ease == Easing.EaseIn)
                        {
                            currentScale.x = Mathf.Lerp(OS.x, FS.x, EaseIn(time / duration, power));
                            currentScale.y = Mathf.Lerp(OS.y, FS.y, EaseIn(time / duration, power));
                        }
                        if (Ease == Easing.EaseOut)
                        {
                            currentScale.x = Mathf.Lerp(OS.x, FS.x, EaseOut(time / duration, power));
                            currentScale.y = Mathf.Lerp(OS.y, FS.y, EaseOut(time / duration, power));
                        }
                        if (Ease == Easing.EaseInOut)
                        {
                            currentScale.x = Mathf.Lerp(OS.x, FS.x, EaseInOut(time / duration, power));
                            currentScale.y = Mathf.Lerp(OS.y, FS.y, EaseInOut(time / duration, power));
                        }

                        Obj.transform.localScale = currentScale;

                        //Debug.Log("Time: " + time);
                        yield return null;
                        time += Time.deltaTime;
                    }
                    time = 0f;
                    returnScale = (returnScale == 1 ? -1 : returnScale + 1);
                }
                iterationCount--;
            }
        }

        public static IEnumerator ScaleTo(GameObject Obj, Vector2 finalScale, float duration, uint power = 2, Easing Ease = Easing.Linear)
        {
            float time = 0f;

            Vector3 origScl = new Vector3(Obj.transform.localScale.x, Obj.transform.localScale.y, Obj.transform.localScale.z);
            Vector3 finalScl = new Vector3(finalScale.x, finalScale.y, Obj.transform.localScale.z);

            Vector3 OS = origScl;
            Vector3 FS = finalScl;

            Obj.transform.localScale = OS;

            while (time <= duration)
            {
                Vector3 currentScale = OS;

                if (Ease == Easing.Linear)
                {
                    currentScale.x = Mathf.Lerp(OS.x, FS.x, time / duration);
                    currentScale.y = Mathf.Lerp(OS.y, FS.y, time / duration);
                }
                if (Ease == Easing.EaseIn)
                {
                    currentScale.x = Mathf.Lerp(OS.x, FS.x, EaseIn(time / duration, power));
                    currentScale.y = Mathf.Lerp(OS.y, FS.y, EaseIn(time / duration, power));
                }
                if (Ease == Easing.EaseOut)
                {
                    currentScale.x = Mathf.Lerp(OS.x, FS.x, EaseOut(time / duration, power));
                    currentScale.y = Mathf.Lerp(OS.y, FS.y, EaseOut(time / duration, power));
                }
                if (Ease == Easing.EaseInOut)
                {
                    currentScale.x = Mathf.Lerp(OS.x, FS.x, EaseInOut(time / duration, power));
                    currentScale.y = Mathf.Lerp(OS.y, FS.y, EaseInOut(time / duration, power));
                }

                Obj.transform.localScale = new Vector3(currentScale.x, currentScale.y, Obj.transform.localScale.z);

                //Debug.Log("Time: " + time);
                yield return null;
                time += Time.deltaTime;
            }
            Obj.transform.localScale = FS;
            time = 0f;
        }

        public static IEnumerator ScaleXTo(GameObject Obj, float scaleX, float duration, uint power = 2, Easing Ease = Easing.Linear)
        {
            float time = 0f;

            Vector3 OS = Obj.transform.localScale;
            Vector3 FS = new Vector3(scaleX, Obj.transform.localScale.y);

            Obj.transform.localScale = OS;

            while (time <= duration)
            {
                Vector3 currentScale = OS;

                if (Ease == Easing.Linear)
                {
                    currentScale.x = Mathf.Lerp(OS.x, FS.x, time / duration);
                }
                if (Ease == Easing.EaseIn)
                {
                    currentScale.x = Mathf.Lerp(OS.x, FS.x, EaseIn(time / duration, power));
                }
                if (Ease == Easing.EaseOut)
                {
                    currentScale.x = Mathf.Lerp(OS.x, FS.x, EaseOut(time / duration, power));
                }
                if (Ease == Easing.EaseInOut)
                {
                    currentScale.x = Mathf.Lerp(OS.x, FS.x, EaseInOut(time / duration, power));
                }

                Obj.transform.localScale = currentScale;

                yield return null;
                time += Time.deltaTime;
            }
            Obj.transform.localScale = FS;
            time = 0f;
        }
        public static IEnumerator ScaleYTo(GameObject Obj, float scaleY, float duration, uint power = 2, Easing Ease = Easing.Linear)
        {
            float time = 0f;

            Vector3 OS = Obj.transform.localScale;
            Vector3 FS = new Vector3(Obj.transform.localScale.x, scaleY);

            Obj.transform.localScale = OS;

            while (time <= duration)
            {
                Vector3 currentScale = OS;

                if (Ease == Easing.Linear)
                {
                    currentScale.y = Mathf.Lerp(OS.y, FS.y, time / duration);
                }
                if (Ease == Easing.EaseIn)
                {
                    currentScale.y = Mathf.Lerp(OS.y, FS.y, EaseIn(time / duration, power));
                }
                if (Ease == Easing.EaseOut)
                {
                    currentScale.y = Mathf.Lerp(OS.y, FS.y, EaseOut(time / duration, power));
                }
                if (Ease == Easing.EaseInOut)
                {
                    currentScale.y = Mathf.Lerp(OS.y, FS.y, EaseInOut(time / duration, power));
                }

                Obj.transform.localScale = currentScale;

                yield return null;
                time += Time.deltaTime;
            }
            Obj.transform.localScale = FS;
            time = 0f;
        }
        #endregion

        #region //Rotation
        public static IEnumerator RotateTo(GameObject Obj, float angle, Axis Axis, float duration, uint power = 2, Easing Ease = Easing.Linear)
        {
            float time = 0f;

            Vector3 OS = Obj.transform.rotation.eulerAngles;
            Vector3 FS = Vector3.zero;

            switch (Axis)
            {
                case Axis.X:
                    {
                        FS = Vector3.right;
                        break;
                    }
                case Axis.Y:
                    {
                        FS = Vector3.up;
                        break;
                    }
                case Axis.Z:
                    {
                        FS = Vector3.forward;
                        break;
                    }
                case Axis.XY:
                    {
                        FS = new Vector3(1, 1, 0);
                        break;
                    }
                case Axis.XZ:
                    {
                        FS = new Vector3(1, 0, 1);
                        break;
                    }
                case Axis.YZ:
                    {
                        FS = new Vector3(0, 1, 1);
                        break;
                    }
                case Axis.XYZ:
                    {
                        FS = new Vector3(1, 1, 1);
                        break;
                    }
            }
            FS *= angle;
            Obj.transform.rotation = Quaternion.Lerp(Obj.transform.rotation, Quaternion.Euler(OS), 1f);

            while (time <= duration)
            {
                Vector3 currentAngle = OS;

                if (Ease == Easing.Linear)
                {
                    currentAngle.x = Mathf.Lerp(OS.x, FS.x, time / duration);
                    currentAngle.y = Mathf.Lerp(OS.y, FS.y, time / duration);
                    currentAngle.z = Mathf.Lerp(OS.z, FS.z, time / duration);
                    Color FlipColor = new Color(((360 - currentAngle.x) % 360), ((360 - currentAngle.y) % 360), ((360 - currentAngle.z) % 360), 1f);
                    Obj.GetComponent<SpriteRenderer>().color = FlipColor;
                }
                if (Ease == Easing.EaseIn)
                {
                    currentAngle.x = Mathf.Lerp(OS.x, FS.x, EaseIn(time / duration, power));
                    currentAngle.y = Mathf.Lerp(OS.y, FS.y, EaseIn(time / duration, power));
                    currentAngle.z = Mathf.Lerp(OS.z, FS.z, EaseIn(time / duration, power));
                    Color FlipColor = new Color(((360 - currentAngle.x) % 360), ((360 - currentAngle.y) % 360), ((360 - currentAngle.z) % 360), 1f);
                    Obj.GetComponent<SpriteRenderer>().color = FlipColor;
                }
                if (Ease == Easing.EaseOut)
                {
                    currentAngle.x = Mathf.Lerp(OS.x, FS.x, EaseOut(time / duration, power));
                    currentAngle.y = Mathf.Lerp(OS.y, FS.y, EaseOut(time / duration, power));
                    currentAngle.z = Mathf.Lerp(OS.z, FS.z, EaseOut(time / duration, power));
                }
                if (Ease == Easing.EaseInOut)
                {
                    currentAngle.x = Mathf.Lerp(OS.x, FS.x, EaseInOut(time / duration, power));
                    currentAngle.y = Mathf.Lerp(OS.y, FS.y, EaseInOut(time / duration, power));
                    currentAngle.z = Mathf.Lerp(OS.z, FS.z, EaseInOut(time / duration, power));
                }

                Obj.transform.rotation = Quaternion.Lerp(Obj.transform.rotation, Quaternion.Euler(currentAngle), time / duration);

                //Debug.Log("Time: " + time);
                yield return null;
                time += Time.deltaTime;
            }
            Obj.transform.rotation = Quaternion.Euler(FS);
            time = 0f;
        }

        public static IEnumerator RelRotateTo(GameObject Obj, float angle, Axis Axis, float duration, uint power = 2, Easing Ease = Easing.Linear)
        {
            float time = 0f;

            Vector3 OS = Obj.transform.rotation.eulerAngles;
            Vector3 FS = Vector3.zero;

            switch (Axis)
            {
                case Axis.X:
                    {
                        FS = Vector3.right;
                        break;
                    }
                case Axis.Y:
                    {
                        FS = Vector3.up;
                        break;
                    }
                case Axis.Z:
                    {
                        FS = Vector3.forward;
                        break;
                    }
                case Axis.XY:
                    {
                        FS = new Vector3(1, 1, 0);
                        break;
                    }
                case Axis.XZ:
                    {
                        FS = new Vector3(1, 0, 1);
                        break;
                    }
                case Axis.YZ:
                    {
                        FS = new Vector3(0, 1, 1);
                        break;
                    }
                case Axis.XYZ:
                    {
                        FS = new Vector3(1, 1, 1);
                        break;
                    }
            }
            FS *= angle;
            FS += OS;
            Obj.transform.rotation = Quaternion.Lerp(Obj.transform.rotation, Quaternion.Euler(OS), 1f);

            while (time <= duration)
            {
                Vector3 currentAngle = OS;

                if (Ease == Easing.Linear)
                {
                    currentAngle.x = Mathf.Lerp(OS.x, FS.x, time / duration);
                    currentAngle.y = Mathf.Lerp(OS.y, FS.y, time / duration);
                    currentAngle.z = Mathf.Lerp(OS.z, FS.z, time / duration);
                }
                if (Ease == Easing.EaseIn)
                {
                    currentAngle.x = Mathf.Lerp(OS.x, FS.x, EaseIn(time / duration, power));
                    currentAngle.y = Mathf.Lerp(OS.y, FS.y, EaseIn(time / duration, power));
                    currentAngle.z = Mathf.Lerp(OS.z, FS.z, EaseIn(time / duration, power));
                }
                if (Ease == Easing.EaseOut)
                {
                    currentAngle.x = Mathf.Lerp(OS.x, FS.x, EaseOut(time / duration, power));
                    currentAngle.y = Mathf.Lerp(OS.y, FS.y, EaseOut(time / duration, power));
                    currentAngle.z = Mathf.Lerp(OS.z, FS.z, EaseOut(time / duration, power));
                }
                if (Ease == Easing.EaseInOut)
                {
                    currentAngle.x = Mathf.Lerp(OS.x, FS.x, EaseInOut(time / duration, power));
                    currentAngle.y = Mathf.Lerp(OS.y, FS.y, EaseInOut(time / duration, power));
                    currentAngle.z = Mathf.Lerp(OS.z, FS.z, EaseInOut(time / duration, power));
                }

                Obj.transform.rotation = Quaternion.Lerp(Obj.transform.rotation, Quaternion.Euler(currentAngle), time / duration);

                //Debug.Log("Time: " + time);
                yield return null;
                time += Time.deltaTime;
            }
            Obj.transform.rotation = Quaternion.Euler(FS);
            time = 0f;
        }

        public static IEnumerator RelRotateCycles(GameObject Obj, int numOfCycles, Axis Axis, float duration, bool useFlipChannel = false, uint power = 2, Easing Ease = Easing.Linear)
        {
            float time = 0f;
            float angle = (numOfCycles != 0 ? 360f * numOfCycles : 360f);

            Vector3 OS = Obj.transform.rotation.eulerAngles;
            Vector3 FS = Vector3.zero;
            Color FlipColor = new Color();

            switch (Axis)
            {
                case Axis.X:
                    {
                        FS = Vector3.right;
                        break;
                    }
                case Axis.Y:
                    {
                        FS = Vector3.up;
                        break;
                    }
                case Axis.Z:
                    {
                        FS = Vector3.forward;
                        break;
                    }
                case Axis.XY:
                    {
                        FS = new Vector3(1, 1, 0);
                        break;
                    }
                case Axis.XZ:
                    {
                        FS = new Vector3(1, 0, 1);
                        break;
                    }
                case Axis.YZ:
                    {
                        FS = new Vector3(0, 1, 1);
                        break;
                    }
                case Axis.XYZ:
                    {
                        FS = new Vector3(1, 1, 1);
                        break;
                    }
            }
            FS *= angle;
            FS += OS;
            Obj.transform.rotation = Quaternion.Lerp(Obj.transform.rotation, Quaternion.Euler(OS), 1f);
            do
            {
                while (time <= duration)
                {
                    Vector3 currentAngle = OS;

                    if (Ease == Easing.Linear)
                    {
                        currentAngle.x = Mathf.Lerp(OS.x, FS.x, time / duration);
                        currentAngle.y = Mathf.Lerp(OS.y, FS.y, time / duration);
                        currentAngle.z = Mathf.Lerp(OS.z, FS.z, time / duration);
                    }
                    if (Ease == Easing.EaseIn)
                    {
                        currentAngle.x = Mathf.Lerp(OS.x, FS.x, EaseIn(time / duration, power));
                        currentAngle.y = Mathf.Lerp(OS.y, FS.y, EaseIn(time / duration, power));
                        currentAngle.z = Mathf.Lerp(OS.z, FS.z, EaseIn(time / duration, power));
                    }
                    if (Ease == Easing.EaseOut)
                    {
                        currentAngle.x = Mathf.Lerp(OS.x, FS.x, EaseOut(time / duration, power));
                        currentAngle.y = Mathf.Lerp(OS.y, FS.y, EaseOut(time / duration, power));
                        currentAngle.z = Mathf.Lerp(OS.z, FS.z, EaseOut(time / duration, power));
                    }
                    if (Ease == Easing.EaseInOut)
                    {
                        currentAngle.x = Mathf.Lerp(OS.x, FS.x, EaseInOut(time / duration, power));
                        currentAngle.y = Mathf.Lerp(OS.y, FS.y, EaseInOut(time / duration, power));
                        currentAngle.z = Mathf.Lerp(OS.z, FS.z, EaseInOut(time / duration, power));
                    }

                    if (useFlipChannel)
                    {
                        if (Axis == Axis.X || Axis == Axis.Y)
                        {
                            float FlipValue = (Axis == Axis.X ? currentAngle.x : currentAngle.y);
                            FlipColor = new Vector4(Mathf.Abs(90 - (FlipValue % 180)) / 90f,
                                                    Mathf.Abs(90 - (FlipValue % 180)) / 90f,
                                                    Mathf.Abs(90 - (FlipValue % 180)) / 90f,
                                                    1f);
                        }
                        else if (Axis == Axis.XY || Axis == Axis.XZ || Axis == Axis.YZ || Axis == Axis.XYZ)
                        {
                            float FlipValue = (Axis == Axis.XY || Axis == Axis.XZ || Axis == Axis.XYZ ? currentAngle.x : currentAngle.y);
                            FlipColor = new Vector4(Mathf.Abs(90 - (FlipValue % 180)) / 90f,
                                                    Mathf.Abs(90 - (FlipValue % 180)) / 90f,
                                                    Mathf.Abs(90 - (FlipValue % 180)) / 90f,
                                                    1f);
                        }
                        Obj.GetComponent<SpriteRenderer>().color = FlipColor;
                    }

                    Obj.transform.rotation = Quaternion.Lerp(Obj.transform.rotation, Quaternion.Euler(currentAngle), time / duration);
                    
                    yield return null;
                    time += Time.deltaTime;
                }
                Obj.transform.rotation = Quaternion.Euler(FS);
                time = 0f;
            }
            while (numOfCycles == 0);
        }
        #endregion

        #region //Fading
        public static IEnumerator FadeTo(GameObject Obj, float alpha, float duration, uint power = 2, Easing Ease = Easing.Linear)
        {
            float time = 0f;
            alpha = Mathf.Clamp(alpha, 0f, 1f);

            Color OS = Obj.GetComponent<SpriteRenderer>().color;
            Color FS = new Color(Obj.GetComponent<SpriteRenderer>().color.r,
                                Obj.GetComponent<SpriteRenderer>().color.g,
                                Obj.GetComponent<SpriteRenderer>().color.b,
                                alpha);

            Obj.GetComponent<SpriteRenderer>().color = OS;

            while (time <= duration)
            {
                Color currentColor = OS;

                if (Ease == Easing.Linear)
                {
                    currentColor.a = Mathf.Lerp(OS.a, FS.a, time / duration);
                }
                if (Ease == Easing.EaseIn)
                {
                    currentColor.a = Mathf.Lerp(OS.a, FS.a, EaseIn(time / duration, power));
                }
                if (Ease == Easing.EaseOut)
                {
                    currentColor.a = Mathf.Lerp(OS.a, FS.a, EaseOut(time / duration, power));
                }
                if (Ease == Easing.EaseInOut)
                {
                    currentColor.a = Mathf.Lerp(OS.a, FS.a, EaseInOut(time / duration, power));
                }

                Obj.GetComponent<SpriteRenderer>().color = currentColor;

                //Debug.Log("Time: " + time);
                yield return null;
                time += Time.deltaTime;
            }
            Obj.GetComponent<SpriteRenderer>().color = FS;
            time = 0f;
        }
        #endregion

        #region //Color Change
        public static IEnumerator ColorChangeFromRGBA(GameObject Obj, Color color, float duration, Format colorFormat = Format.Standard, uint power = 2, Easing Ease = Easing.Linear)
        {
            float time = 0f;
            int multiplier = 255;

            if (colorFormat == Format.Standard)
            {
                multiplier = 255;
                color = new Color(Mathf.Clamp(color.r, 0f, 255f), Mathf.Clamp(color.g, 0f, 255f), Mathf.Clamp(color.b, 0f, 255f), Mathf.Clamp(color.a, 0f, 255f));
            }
            else if (colorFormat == Format.Scalar)
            {
                multiplier = 1;
                color = new Color(Mathf.Clamp(color.r, 0f, 1), Mathf.Clamp(color.g, 0f, 1f), Mathf.Clamp(color.b, 0f, 1f), Mathf.Clamp(color.a, 0f, 1f));
            }
            else if (colorFormat == Format.Percentage)
            {
                multiplier = 100;
                color = new Color(Mathf.Clamp(color.r, 0f, 100f), Mathf.Clamp(color.g, 0f, 100f), Mathf.Clamp(color.b, 0f, 100f), Mathf.Clamp(color.a, 0f, 100f));
            }

            color.r /= multiplier;
            color.g /= multiplier;
            color.b /= multiplier;
            color.a /= multiplier;
            //Debug.Log("Color: " + color.r + ", " + color.g + ", " + color.b + ", " + color.a);

            Color OS = Obj.GetComponent<SpriteRenderer>().color;
            Color FS = color;

            Obj.GetComponent<SpriteRenderer>().color = OS;

            while (time <= duration)
            {
                Color currentColor = OS;

                if (Ease == Easing.Linear)
                {
                    currentColor.r = Mathf.Lerp(OS.r, FS.r, time / duration);
                    currentColor.g = Mathf.Lerp(OS.g, FS.g, time / duration);
                    currentColor.b = Mathf.Lerp(OS.b, FS.b, time / duration);
                    currentColor.a = Mathf.Lerp(OS.a, FS.a, time / duration);
                }
                if (Ease == Easing.EaseIn)
                {
                    currentColor.r = Mathf.Lerp(OS.r, FS.r, EaseIn(time / duration, power));
                    currentColor.g = Mathf.Lerp(OS.g, FS.g, EaseIn(time / duration, power));
                    currentColor.b = Mathf.Lerp(OS.b, FS.b, EaseIn(time / duration, power));
                    currentColor.a = Mathf.Lerp(OS.a, FS.a, EaseIn(time / duration, power));
                }
                if (Ease == Easing.EaseOut)
                {
                    currentColor.r = Mathf.Lerp(OS.r, FS.r, EaseOut(time / duration, power));
                    currentColor.g = Mathf.Lerp(OS.g, FS.g, EaseOut(time / duration, power));
                    currentColor.b = Mathf.Lerp(OS.b, FS.b, EaseOut(time / duration, power));
                    currentColor.a = Mathf.Lerp(OS.a, FS.a, EaseOut(time / duration, power));
                }
                if (Ease == Easing.EaseInOut)
                {
                    currentColor.r = Mathf.Lerp(OS.r, FS.r, EaseInOut(time / duration, power));
                    currentColor.g = Mathf.Lerp(OS.g, FS.g, EaseInOut(time / duration, power));
                    currentColor.b = Mathf.Lerp(OS.b, FS.b, EaseInOut(time / duration, power));
                    currentColor.a = Mathf.Lerp(OS.a, FS.a, EaseInOut(time / duration, power));
                }

                Obj.GetComponent<SpriteRenderer>().color = currentColor;

                //Debug.Log("Time: " + time);
                yield return null;
                time += Time.deltaTime;
            }
            Obj.GetComponent<SpriteRenderer>().color = FS;
            time = 0f;
        }

        public static IEnumerator ColorChangeFromHex(GameObject Obj, string hexString, float duration, float alpha = 1f, uint power = 2, Easing Ease = Easing.Linear)
        {
            if (!CheckHexFormat(hexString)) { yield break; }
            float time = 0f;

            Color OS = Obj.GetComponent<SpriteRenderer>().color;
            Color FS = ConvertHexToRGB(hexString);
            FS.a = alpha;

            Obj.GetComponent<SpriteRenderer>().color = OS;

            while (time <= duration)
            {
                Color currentColor = OS;

                if (Ease == Easing.Linear)
                {
                    currentColor.r = Mathf.Lerp(OS.r, FS.r, time / duration);
                    currentColor.g = Mathf.Lerp(OS.g, FS.g, time / duration);
                    currentColor.b = Mathf.Lerp(OS.b, FS.b, time / duration);
                    currentColor.a = Mathf.Lerp(OS.a, FS.a, time / duration);
                }
                if (Ease == Easing.EaseIn)
                {
                    currentColor.r = Mathf.Lerp(OS.r, FS.r, EaseIn(time / duration, power));
                    currentColor.g = Mathf.Lerp(OS.g, FS.g, EaseIn(time / duration, power));
                    currentColor.b = Mathf.Lerp(OS.b, FS.b, EaseIn(time / duration, power));
                    currentColor.a = Mathf.Lerp(OS.a, FS.a, EaseIn(time / duration, power));
                }
                if (Ease == Easing.EaseOut)
                {
                    currentColor.r = Mathf.Lerp(OS.r, FS.r, EaseOut(time / duration, power));
                    currentColor.g = Mathf.Lerp(OS.g, FS.g, EaseOut(time / duration, power));
                    currentColor.b = Mathf.Lerp(OS.b, FS.b, EaseOut(time / duration, power));
                    currentColor.a = Mathf.Lerp(OS.a, FS.a, EaseOut(time / duration, power));
                }
                if (Ease == Easing.EaseInOut)
                {
                    currentColor.r = Mathf.Lerp(OS.r, FS.r, EaseInOut(time / duration, power));
                    currentColor.g = Mathf.Lerp(OS.g, FS.g, EaseInOut(time / duration, power));
                    currentColor.b = Mathf.Lerp(OS.b, FS.b, EaseInOut(time / duration, power));
                    currentColor.a = Mathf.Lerp(OS.a, FS.a, EaseInOut(time / duration, power));
                }

                Obj.GetComponent<SpriteRenderer>().color = currentColor;

                //Debug.Log("Time: " + time);
                yield return null;
                time += Time.deltaTime;
            }
            Obj.GetComponent<SpriteRenderer>().color = FS;
            time = 0f;
        }

        private static Color ConvertHexToRGB(string hexCode)
        {
            int rVal = 0;
            int gVal = 0;
            int bVal = 0;

            if (hexCode.StartsWith("#")) { hexCode = hexCode.Remove(0, 1); }

            string rCode = string.Copy(hexCode).Substring(0, 2);
            string gCode = string.Copy(hexCode).Substring(2, 2);
            string bCode = string.Copy(hexCode).Substring(4, 2);

            for (int index = 0, power = 1; index < 2; index++, power--)
            {
                rVal += CalculateIntegerValue(rCode[index], power);
                gVal += CalculateIntegerValue(gCode[index], power);
                bVal += CalculateIntegerValue(bCode[index], power);
            }

            //Debug.Log("Color (R, G, B): " + rVal + ", " + gVal + ", " + bVal);
            Color color = new Color(rVal / 255f, gVal / 255f, bVal / 255f);
            
            return color;
        }

        private static bool CheckHexFormat(string hexString)
        {
            if (hexString.StartsWith("#")) { hexString = hexString.Remove(0, 1); }
            hexString = hexString.ToLower();
            string validChars = "0123456789abcdef";

            if (hexString.Length == 6)
            {
                for (int i = 0; i < hexString.Length; i++)
                {
                    if (validChars.IndexOf(hexString[i]) == -1)
                    {
                        Debug.LogError("ERROR: Hex code is not formatted properly!");
                        return false;
                    }
                }
                return true;
            }

            Debug.LogError("ERROR: Hex code is too long or too short!");
            return false;
        }

        private static int CalculateIntegerValue(char hexLetter, int powerValue)
        {
            hexLetter = char.ToLower(hexLetter);

            if (hexLetter == '0') { return (int)(0 * Math.Pow(16, powerValue)); }
            if (hexLetter == '1') { return (int)(1 * Math.Pow(16, powerValue)); }
            if (hexLetter == '2') { return (int)(2 * Math.Pow(16, powerValue)); }
            if (hexLetter == '3') { return (int)(3 * Math.Pow(16, powerValue)); }
            if (hexLetter == '4') { return (int)(4 * Math.Pow(16, powerValue)); }
            if (hexLetter == '5') { return (int)(5 * Math.Pow(16, powerValue)); }
            if (hexLetter == '6') { return (int)(6 * Math.Pow(16, powerValue)); }
            if (hexLetter == '7') { return (int)(7 * Math.Pow(16, powerValue)); }
            if (hexLetter == '8') { return (int)(8 * Math.Pow(16, powerValue)); }
            if (hexLetter == '9') { return (int)(9 * Math.Pow(16, powerValue)); }
            if (hexLetter == 'a') { return (int)(10 * Math.Pow(16, powerValue)); }
            if (hexLetter == 'b') { return (int)(11 * Math.Pow(16, powerValue)); }
            if (hexLetter == 'c') { return (int)(12 * Math.Pow(16, powerValue)); }
            if (hexLetter == 'd') { return (int)(13 * Math.Pow(16, powerValue)); }
            if (hexLetter == 'e') { return (int)(14 * Math.Pow(16, powerValue)); }
            if (hexLetter == 'f') { return (int)(15 * Math.Pow(16, powerValue)); }
            return '0';
        }
        #endregion

        #region //Easing
        private static float EaseIn(float f, uint powInt)
        {
            float result = 1f;
            while (powInt-- != 0)
            {
                result *= f;
            }
            return result;
        }
        private static float EaseOut(float f, uint powInt) { return (1 - Mathf.Pow((1 - f), powInt)); }
        private static float EaseInOut(float f, uint powInt = 2) { return Mathf.Lerp(EaseIn(f, powInt), EaseOut(f, powInt), f); }
        private static float Spike(float f, uint powInt)
        {
            if (f <= 0.5f) { return EaseIn(f / 0.5f, powInt); }
            return EaseIn((1-f) / 0.5f, powInt);
        }
        #endregion
    }
}

