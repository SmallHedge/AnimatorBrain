using UnityEngine;
using System;

public class AnimatorBrain : MonoBehaviour
{
    private readonly static int[] animations =
    {
        //Animator.StringToHash("Change this to the state name");
    };

    private Animator animator;
    private Animations[] currentAnimation;
    private bool[] layerLocked;
    private Action<int> DefaultAnimation;

    protected void Initialize(int layers, Animations startingAnimation, Animator animator, Action<int> DefaultAnimation)
    {
        layerLocked = new bool[layers];
        currentAnimation = new Animations[layers];
        this.animator = animator;
        this.DefaultAnimation = DefaultAnimation;

        for (int i = 0; i < layers; i++)
        {
            layerLocked[i] = false;
            currentAnimation[i] = startingAnimation;
        }
    }

    public Animations GetCurrentAnimation(int layer)
    {
        return currentAnimation[layer];
    }

    public void SetLocked(bool lockLayer, int layer)
    {
        layerLocked[layer] = lockLayer;
    }

    public void Play(Animations animation, int layer, bool lockLayer, bool bypassLock, float crossfade = 0.2f)
    {
        if(animation == Animations.NONE)
        {
            DefaultAnimation(layer);
            return;
        }

        if (layerLocked[layer] && !bypassLock) return;
        layerLocked[layer] = lockLayer;

        if(bypassLock)
            foreach (var item in animator.GetBehaviours<OnExit>())
                if (item.layerIndex == layer)
                    item.cancel = true;

        if (currentAnimation[layer] == animation) return;

        currentAnimation[layer] = animation;
        animator.CrossFade(animations[(int)currentAnimation[layer]], crossfade, layer);
    }
}

public enum Animations
{
    //List Animations here
}
