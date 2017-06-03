using UnityEngine;
using System.Collections;

/// <summary>
/// 이동 관련 유틸리티 클래스
/// </summary>
public class MoveUtil
{
    /// <summary>
    /// 지정한 캐릭터컨트롤러를 가지고 목적지까지 이동함
    /// </summary>
    /// <param name="cc">캐릭터컨트롤러</param>
    /// <param name="target">목적지 트랜스폼</param>
    /// <param name="moveSpeed">이동 속도</param>
    /// <param name="turnSpeed">회전 속도</param>
    /// <returns>목적지까지 남은 거리</returns>
	public static float MoveByFrame(CharacterController cc, Transform target, float moveSpeed, float turnSpeed)
    {
        //이동하는 물체의 트랜스폼
    	Transform self = cc.transform;

        //목적지 위치
        Vector3 targetPos = target.position;
        //목적지 위치를 자기 높이와 맞춤
        targetPos.y = self.position.y;
        //목적지를 벗어나지 않도록 프레임별로 확인함
        Vector3 framePos = Vector3.MoveTowards(self.position, targetPos, moveSpeed * Time.deltaTime);

        //한프레임 이동벡터
        Vector3 movement = framePos - self.position;
        //캐릭터 컨트롤러로 이동 시킴, 중력도 처리함
        cc.Move(movement + Physics.gravity);

        //목적지로 회전함
        RotateToDir(self, target, turnSpeed);

        //남은거리를 리턴함
        return Vector3.Distance(framePos, targetPos);
    }

    /// <summary>
    /// 목적지까지 회전 시켜줌
    /// </summary>
    /// <param name="self">이동 트랜스폼</param>
    /// <param name="target">목적지</param>
    /// <param name="turnSpeed">회전속도</param>
    public static void RotateToDir(Transform self, Transform target, float turnSpeed)
    {
        //회전 방향벡터
        Vector3 rotDir = target.position - self.position;
        //높이는 제외함
        rotDir.y = 0f;

        //회전량이 없음 회전하지 않음
        if (rotDir == Vector3.zero)
            return;

        //최종 회전 쿼터니언
        Quaternion targetRot = Quaternion.LookRotation(rotDir);
        //프래임별 회전량 
        Quaternion frameRot = Quaternion.RotateTowards(
        	self.rotation, targetRot, turnSpeed * Time.deltaTime);
        //회전시킴
        self.rotation = frameRot;
    }

    /// <summary>
    /// 한방에 목적지로 회전함
    /// </summary>
    /// <param name="self"></param>
    /// <param name="target"></param>
    public static void RotateBurst(Transform self, Transform target)
    {
        Vector3 rotDir = target.position - self.position;
        rotDir.y = 0f;

        if (rotDir == Vector3.zero)
            return;

        self.rotation = Quaternion.LookRotation(rotDir);
    }
}