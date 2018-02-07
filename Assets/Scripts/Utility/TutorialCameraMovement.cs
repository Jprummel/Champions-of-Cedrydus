using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Dialogue;

public class TutorialCameraMovement : MonoBehaviour {

    private static Vector3 camStartPos;
    private static int currentTweenID = 0;
    private static bool skippable = false;
    private static TweenCallback EndOfMovementCallback;
    private static TweenCallback StartOfMovementCallback;

    private void OnEnable()
    {
        InputManager.OnAButton += SkipTween;

        EndOfMovementCallback += () => CameraFollowPlayer.FollowTarget = true;
        EndOfMovementCallback += () => DisplayDialogue.CanSkip = true;
        EndOfMovementCallback += () => skippable = false;

        StartOfMovementCallback += () => DisplayDialogue.CanSkip = false;
        StartOfMovementCallback += () => CameraFollowPlayer.FollowTarget = false;
    }

    private void OnDisable()
    {
        InputManager.OnAButton -= SkipTween;
    }

    public static void BossCameraSequence()
    {
        currentTweenID = 5;

        Sequence bossSequence = DOTween.Sequence();
        bossSequence.SetId(5);
        bossSequence.OnStart(StartOfMovementCallback);
        bossSequence.OnComplete(() => returnBossSequence());

        bossSequence.AppendInterval(0.2f);
        bossSequence.AppendCallback(() => skippable = true);
        bossSequence.Join(Camera.main.transform.DOMove(new Vector3(101, -44.5f, -10), 2f).SetEase(Ease.OutQuart));
    }

    private static void returnBossSequence()
    {
        currentTweenID = 6;
        Sequence returnSequence = DOTween.Sequence();
        returnSequence.OnComplete(EndOfMovementCallback);
        returnSequence.SetId(6);

        returnSequence.AppendInterval(0.75f);
        returnSequence.Append(Camera.main.transform.DOMove(camStartPos, 2f).SetEase(Ease.OutQuart));
    }

    public static void MiniBossCameraSequence()
    {
        camStartPos = Camera.main.transform.position;
        MinibossM1();
    }

    private static void MinibossM1()
    {
        currentTweenID = 2;

        Sequence mBoss1Sequence = DOTween.Sequence();
        mBoss1Sequence.SetId(2);
        mBoss1Sequence.OnStart(StartOfMovementCallback);

        Tween mBoss1 = Camera.main.transform.DOMove(new Vector3(6, -125, -10f), 2f).SetEase(Ease.OutQuart);

        mBoss1Sequence.AppendInterval(0.4f);
        mBoss1Sequence.AppendCallback(() => skippable = true);
        mBoss1Sequence.Join(mBoss1);
        mBoss1Sequence.AppendInterval(0.75f);
        mBoss1Sequence.OnComplete(()=>MinibossM2());
    }

    private static void MinibossM2()
    {
        skippable = false;
        currentTweenID = 3;

        Sequence mBoss2Sequence = DOTween.Sequence();
        mBoss2Sequence.SetId(3);

        Tween mBoss2 = Camera.main.transform.DOMove(new Vector3(166, -5, -10f), 2f).SetEase(Ease.OutQuart);

        mBoss2Sequence.AppendInterval(0.4f);
        mBoss2Sequence.AppendCallback(() => skippable = true);
        mBoss2Sequence.Join(mBoss2);
        mBoss2Sequence.AppendInterval(0.75f);
        mBoss2Sequence.OnComplete(()=>MinibossM3());
    }

    private static void MinibossM3()
    {
        skippable = false;
        currentTweenID = 4;

        Sequence mBoss3Sequence = DOTween.Sequence();
        mBoss3Sequence.SetId(4);

        mBoss3Sequence.OnComplete(returnBossSequence);

        Tween mBoss3 = Camera.main.transform.DOMove(new Vector3(171, -125, -10f), 2f).SetEase(Ease.OutQuart);

        mBoss3Sequence.AppendInterval(0.4f);
        mBoss3Sequence.AppendCallback(() => skippable = true);
        mBoss3Sequence.Join(mBoss3);
        mBoss3Sequence.AppendInterval(0.75f);
    }

    private void SkipTween()
    {
        if(skippable)
            DOTween.Kill(currentTweenID, true);
    }
}
