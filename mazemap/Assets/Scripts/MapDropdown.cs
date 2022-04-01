using TMPro;
using UnityEngine;
using System.Collections.Generic;

public class MapDropdown : MonoBehaviour
{
    [SerializeField] private TMP_Dropdown mode_dropdown;
    [SerializeField] private TMP_Dropdown map_dropdown;
    [SerializeField] private TextMeshProUGUI text;
    private string[] maze_list = new string[3] { "Forest Stage1", "Forest Stage2", "Tokyo Stage1" };
    private string[] sullae_list = new string[4] { "apple", "mango", "juice", "pepper" };

    public void OnModeSelect()
    {
        // ���� dropdown�� �ִ� ��� �ɼ��� ����
        map_dropdown.ClearOptions();

        // ���ο� �ɼ� ������ ���� OptionData ����
        List<TMP_Dropdown.OptionData> optionList = new List<TMP_Dropdown.OptionData>();

        if (mode_dropdown.value == 1)
        {
            // sullae_list �迭�� �ִ� ��� ���ڿ� �����͸� �ҷ��ͼ� optionList�� ����
            foreach (string str in sullae_list)
            {
                optionList.Add(new TMP_Dropdown.OptionData(str));
            }
        }
        else
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
        // ������ map �̸��� ������ 
        if (mode_dropdown.value == 1)
        {
            text.text = $"{sullae_list[map_dropdown.value]}";
        } else
        {
            text.text = $"{maze_list[map_dropdown.value]}";
        }
        

    }
}
