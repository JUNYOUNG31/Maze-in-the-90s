using TMPro;
using UnityEngine;
using System.Collections.Generic;

public class MapDropdown : MonoBehaviour
{
    [SerializeField] private TMP_Dropdown mode_dropdown;
    [SerializeField] private TMP_Dropdown map_dropdown;
    [SerializeField] private TextMeshProUGUI text;
    private string[] maze_list = new string[4] { "-", "Forest Stage1", "Forest Stage2", "Tokyo Stage1" };
    private string[] sullae_list = new string[5] { "-", "apple", "mango", "juice", "pepper" };

    public void OnModeSelect()
    {
        // ���� dropdown�� �ִ� ��� �ɼ��� ����
        map_dropdown.ClearOptions();

        // ���ο� �ɼ� ������ ���� OptionData ����
        List<TMP_Dropdown.OptionData> optionList = new List<TMP_Dropdown.OptionData>();

        if (mode_dropdown.value == 2)
        {
            // sullae_list �迭�� �ִ� ��� ���ڿ� �����͸� �ҷ��ͼ� optionList�� ����
            foreach (string str in sullae_list)
            {
                optionList.Add(new TMP_Dropdown.OptionData(str));
            }
        }
        else if (mode_dropdown.value == 1)
        {
            // maze_list �迭�� �ִ� ��� ���ڿ� �����͸� �ҷ��ͼ� optionList�� ����
            foreach (string str in maze_list)
            {
                optionList.Add(new TMP_Dropdown.OptionData(str));
            }
        }

        // ������ ������ optionList�� dropdown�� �ɼ� ���� �߰�
        map_dropdown.AddOptions(optionList);

        // ���� dropdown�� ���õ� �ɼ��� 0������ ����
        map_dropdown.value = 0;
    }

    
    public void OnDropdownEvent(int index)
    {
        // ������ map �̸��� ������ (1 -> �̷� 2-> ����)
        if (mode_dropdown.value == 2)
        {
            text.text = $"{sullae_list[map_dropdown.value]}";
            Debug.Log(mode_dropdown.value);
            Debug.Log(text.text);

        } else if (mode_dropdown.value == 1)
        {
            text.text = $"{maze_list[map_dropdown.value]}";
            Debug.Log(mode_dropdown.value);
            Debug.Log(text.text);
        }

        //��� �޾ƿ� ��û ����
        RankingHandler.instance.ChangeTab(mode_dropdown.value, text.text);

    }
}
