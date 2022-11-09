using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Block_Manager : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI textMeshPro_blockText;
    [SerializeField] List<Color> colors;
    public int block_Value;
    public int xPos, yPos;
    int victory_Score = 2048;

    public void Set_Block_Value(int value)
    {
        block_Value = value;
        textMeshPro_blockText.text = value.ToString();
        Change_Color();
        Check_Value();
    }

    public void SetXYpositions(int x, int y)
    {
        xPos = x;
        yPos = y;
    }

    public int Get_Block_Value()
    {
        return block_Value;
    }

    private void Check_Value()
    {
        if (block_Value == victory_Score) Level_Manager.instance.Trigger_Event_LevelEnd();
    }

    private void Change_Color()
    {
        // 2 4 8 16 32 64 128 256 512 1024 2048 , 11 color
        if (block_Value == 2) GetComponent<SpriteRenderer>().color = colors[0];
        else if (block_Value == 4) GetComponent<SpriteRenderer>().color = colors[1];
        else if (block_Value == 8) GetComponent<SpriteRenderer>().color = colors[2];
        else if (block_Value == 16) GetComponent<SpriteRenderer>().color = colors[3];
        else if (block_Value == 32) GetComponent<SpriteRenderer>().color = colors[4];
        else if (block_Value == 64) GetComponent<SpriteRenderer>().color = colors[5];
        else if (block_Value == 128) GetComponent<SpriteRenderer>().color = colors[6];
        else if (block_Value == 256) GetComponent<SpriteRenderer>().color = colors[7];
        else if (block_Value == 512) GetComponent<SpriteRenderer>().color = colors[8];
        else if (block_Value == 1024) GetComponent<SpriteRenderer>().color = colors[9];
        else if (block_Value == 2048) GetComponent<SpriteRenderer>().color = colors[10];
        else GetComponent<SpriteRenderer>().color = colors[11];
    }
}
