using System;
using System.Text;
using UnityEngine;

namespace Utilities
{
    public static class AnimatorExtensions
    {
        // Sometimes not working as expected.
        public static float GetCurrentClipLength(this Animator animator)
        {
            var clipInfo = animator.GetCurrentAnimatorClipInfo(0);
            return clipInfo[0].clip.length;
        }

        public static AnimationClip GetClipFromIndex(this Animator animator,int clipIndex)
        {
            var clips = animator.runtimeAnimatorController.animationClips;
            
            if (clipIndex >= clips.Length) return null;

            return clips[clipIndex];
        }
        
        //index starts from zero.
        public static float GetClipLengthFromClipIndex(this Animator animator, int clipIndex)
        {
            var clips = animator.runtimeAnimatorController.animationClips;

            if (clipIndex >= clips.Length) return 0f;
            
            return clips[clipIndex].length;
        }

        //index starts from zero.
        public static string GetClipNameFromClipIndex(this Animator animator, int clipIndex)
        {
            var clips = animator.runtimeAnimatorController.animationClips;
            
            if (clipIndex >= clips.Length) return "";

            return clips[clipIndex].name;
        }
        
        public static string GetCurrentClipName(this Animator animator)
        {
            var clipInfo = animator.GetCurrentAnimatorClipInfo(0);
            return clipInfo[0].clip.name;
        }

        public static void DebugCurrentClipInfo(this Animator animator)
        {
            var clipInfo = animator.GetCurrentAnimatorClipInfo(0);
            StringBuilder stringBuilder = new StringBuilder();
            foreach (var info in clipInfo)
            {
                stringBuilder.AppendLine($"Clip Name: {info.clip.name}, Clip Length: {info.clip.length}");
            }
            Debug.Log(stringBuilder.ToString());
        }
        
        public static void DebugAllClipInfo(this Animator animator)
        {
            var clips = animator.runtimeAnimatorController.animationClips;
            StringBuilder stringBuilder = new StringBuilder();
            foreach (var clip in clips)
            {
                stringBuilder.AppendLine($"Clip name: {clip.name}, Clip Length: {clip.length}");
            }
            Debug.Log(stringBuilder.ToString());
        }
        
        //you can't use with lambda functions.(i am talking about Action parameter.)
        public static void AddAnimationEvent(this Animator animator, int clipIndex,float time, Action action)
        {
            var clip = animator.GetClipFromIndex(clipIndex);
            var animationEvent = new AnimationEvent
            {
                time = time,
                functionName = action.Method.Name
            };
            clip.AddEvent(animationEvent);
        }


    }
}
