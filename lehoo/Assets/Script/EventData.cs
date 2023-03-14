using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventData  //�⺻���� ������ Ǯ���� ������ �̺�Ʈ
{
  public int ID;

  public int PlaceCode = 0; //0:������  1:���� 2:��� 3:������(Ȥ�� ����) 4:ķ�۽�
  public int Season = 0;  //�б�(1,2,3,4)
  public int[] Conditions;  //������ ���� 0:����  1:�� �ʿ�  2:�� �ʿ�   3:��� �ʿ�   4:�� �ʿ�   5:�ٴ� �ʿ�
  public string Description = ""; //���� �ؽ�Ʈ
                                  //�⺻

  //����
  public int ProgressType;        //���� ���� 0:�Ϲ��� 1:���� ����,����(������)  2:���� �Һ�   3:��� ��й�

  //���� 0���� ����ϴ� ����
  public bool IsCheck;      //��� üũ����
  public bool IsTendency;   //���� ����������
  public int[] Tendencies;  //���� ���

  public bool Runnable;     //���� ����?
  public int RunPenalty;    //���� �� �г�Ƽ(ü�� or ���ŷ� or ��)

  public int[] CheckTargets;//�˻� ���
  public int[] CheckLevels; //�˻� ���̵�
  public string[] FailureDialogues; //���н� ��� �ؽ�Ʈ
  public int[] FailurePenalty;      //���н� ����Ƽ ����(ü�� or ���ŷ� or �� or ���� or Ư��)
  public bool ProgressAfterFailure; //���н� ���� ���� ����


  //���� ����   �ִ� 1��
  public bool IsWork;       //���� ��?
  public int WorkID;        //������ ���� ID


  //����        �ִ� 4��?
  public bool IsReward;     //���� ��?
  public int DefaultReward_health, DefaultReward_sanity, DefaultReward_money;
  //�⺻ ���� �� 0:���� 1:���� 2:�߰� 3:����
  public int[] ExtraRewardType;
}
