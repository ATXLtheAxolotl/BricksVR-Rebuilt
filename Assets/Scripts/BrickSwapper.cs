﻿using UnityEngine;
using System.Linq;

public static class BrickSwapper
{
    public static GameObject SwapToRealBrick(GameObject brick)
    {
        BrickAttach brickAttach = brick.GetComponent<BrickAttach>();
        if (!brickAttach)
            return null;

        Transform t = brick.transform;

        // NormcoreRPC.Brick serializedBrickObject = new NormcoreRPC.Brick()
        // {
        //     uuid = brickAttach.GetUuid(),
        // };
        // DestroyBrickOverRPC(serializedBrickObject);

        // Remove the brick from the datastore since the datastore specifically stores PLACED bricks
        //BrickServerInterface.GetInstance().RemoveBrick(brickAttach.GetUuid());

        GameObject newBrick = GameObject.Instantiate(
            Resources.Load<GameObject>(brickAttach.swapPrefab),
            t.position,
            t.rotation
        );

        BrickAttach attach = newBrick.GetComponent<BrickAttach>();
        attach.Color = (brickAttach.Color);
        attach.SetUuid(brickAttach.GetUuid());

        newBrick.GetComponent<BrickAttach>().texOffset = brickAttach.texOffset;

        //brickAttach.DelayedDestroy();
        BrickDestroyer.GetInstance().DelayedDestroy(brick);

        return newBrick;
    }

    public static GameObject SwapToFakeBrick(GameObject brick, string headClientId = null, AvatarManager avatarManager = null, Session session = null) {
        BrickAttach brickAttach = brick.GetComponent<BrickAttach>();
        session = session ?? Session.GetInstance();

        headClientId = headClientId ?? session.ClientID;

        BrickData.LocalBrickData serializedBrickObject = new BrickData.LocalBrickData() {
            color = ColorInt.ColorToInt(brickAttach.Color),
            type = brickAttach.normalPrefabName,
            //uuid = brickAttach.GetUuid(),
            pos = BrickData.CustomVec3.From(brick.transform.position),
            rot = BrickData.CustomQuaternion.From(brick.transform.rotation),
            //usingNewColor = true,
            //headClientId = headClientId,
            //usingHeadStuff = true,
        };

        // If this brick is on a head, send the relative position/rotation instead of the world position/rotation
        if (headClientId != session.ClientID){
            brick.transform.parent = avatarManager.GetAvatar(headClientId).head;
            serializedBrickObject.pos = BrickData.CustomVec3.From(brick.transform.localPosition);
            serializedBrickObject.rot = BrickData.CustomQuaternion.From(brick.transform.localRotation);
        }

        if (!TutorialManager.GetInstance().IsTutorialRunning()) {
            //BrickServerInterface.GetInstance().SendBrick(serializedBrickObject);
        }

        GameObject newBrick = PlacedBrickCreator.CreateFromBrickObject(serializedBrickObject);
        newBrick.GetComponent<BrickAttach>().texOffset = brickAttach.texOffset;

        brickAttach.DelayedDestroy();

        return newBrick;
    }
}
