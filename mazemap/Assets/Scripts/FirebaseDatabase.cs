using System.Runtime.InteropServices;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;


namespace FirebaseWebGL.Scripts.FirebaseBridge
{
    public static class FirebaseDatabase
    {
        //�г��� �ߺ��˻�
        [DllImport("__Internal")]
        public static extern void CheckNickname(string name);

        //��ŷ�������� ��� �ø���
        [DllImport("__Internal")]
        public static extern void PostGameRecord(string json);

        //��ŷ�������� ��� �޾ƿ���
        [DllImport("__Internal")]
        public static extern void SetGameRecord();

        //��ŷ������ �� Ŭ��(���, �ʺ� ������)
        [DllImport("__Internal")]
        public static extern void SetByInfo(int mode, string map);
    }
}