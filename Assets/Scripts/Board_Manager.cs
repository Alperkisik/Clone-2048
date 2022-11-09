using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Board_Manager : MonoBehaviour
{
    public static Board_Manager instance;

    [SerializeField] GameObject blockPrefab;
    [SerializeField] Transform blockParentTransform;
    [SerializeField] int boardWidth = 4;
    [SerializeField] int startWithXBlock = 1;

    public event System.EventHandler OnBoardGenerationDone;

    int[,] board_Values;
    GameObject[,] blocks;

    private void Awake()
    {
        instance = this;
    }

    void Start()
    {
        blocks = new GameObject[boardWidth, boardWidth];
        board_Values = new int[boardWidth, boardWidth];
        Generate_Board();
        Event_Listener();
    }

    void Generate_Board()
    {
        float xPos = -1.8f, yPos = 1.8f, space = 0.2f;

        for (int y = 0; y < boardWidth; y++)
        {
            xPos = -1.8f;
            for (int x = 0; x < boardWidth; x++)
            {
                GameObject block = Instantiate(blockPrefab, new Vector2(xPos, yPos), Quaternion.identity, blockParentTransform);
                block.name = "Block [" + (x + 1) + "," + (y + 1) + "]";
                block.GetComponent<Block_Manager>().Set_Block_Value(0);
                block.GetComponent<Block_Manager>().SetXYpositions(x, y);
                board_Values[x, y] = 0;
                blocks[x, y] = block;

                xPos += 1f + space;
            }
            yPos += -1f - space;
        }

        for (int i = 0; i < startWithXBlock; i++)
        {
            int x = Random.Range(0, boardWidth - 1), y = Random.Range(0, boardWidth - 1);

            if (blocks[x, y].GetComponent<Block_Manager>().Get_Block_Value() == 0)
            {
                blocks[x, y].GetComponent<Block_Manager>().Set_Block_Value(2);
                board_Values[x, y] = 2;
            }
            else
            {
                i--;
                if (i < 0) i = 0;
            }
        }

        OnBoardGenerationDone?.Invoke(this, System.EventArgs.Empty);
    }

    void Event_Listener()
    {
        Input_Manager.instance.OnDownInputReceived += InputManager_Event_OnDownInputReceived;
        Input_Manager.instance.OnUpInputReceived += InputManager_Event_OnUpInputReceived;
        Input_Manager.instance.OnRightInputReceived += InputManager_Event_OnRightInputReceived;
        Input_Manager.instance.OnLeftInputReceived += InputManager_Event_OnLeftInputReceived;
    }

    private void InputManager_Event_OnLeftInputReceived(object sender, System.EventArgs e)
    {
        //Debug.Log("Input Manager left input event triggered");
        MoveBlocks_Left();
        Change_RandomBlock();
    }

    private void InputManager_Event_OnRightInputReceived(object sender, System.EventArgs e)
    {
        //Debug.Log("Input Manager right input event triggered");
        MoveBlocks_Right();
        Change_RandomBlock();
    }

    private void InputManager_Event_OnUpInputReceived(object sender, System.EventArgs e)
    {
        //Debug.Log("Input Manager up input event triggered");
        MoveBlocks_UP();
        Change_RandomBlock();
    }

    private void InputManager_Event_OnDownInputReceived(object sender, System.EventArgs e)
    {
        //Debug.Log("Input Manager down input event triggered");
        MoveBlocks_Down();
        Change_RandomBlock();
    }

    private void MoveBlocks_UP()
    {
        //Debug.Log("Move Blocks UP");

        for (int x = 0; x < boardWidth; x++)
        {
            for (int y = 1; y < boardWidth; y++)
            {
                if (board_Values[x, y] == 0) continue;
                //Debug.Log("Column : " + (x + 1) + ",Row :" + (y + 1));

                //merge
                for (int i = 1; i < boardWidth - 1; i++)
                {
                    if (board_Values[x, i - 1] == board_Values[x, i])
                    {
                        board_Values[x, i - 1] += board_Values[x, i - 1];
                        blocks[x, i - 1].GetComponent<Block_Manager>().Set_Block_Value(board_Values[x, i - 1]);
                        
                        Increase_Score(board_Values[x, i - 1]);
                        
                        board_Values[x, i] = 0;
                        blocks[x, i].GetComponent<Block_Manager>().Set_Block_Value(0);
                    }
                }

                //switch
                for (int i = 0; i < y; i++)
                {
                    if (board_Values[x, i] == 0)
                    {
                        //Debug.Log("[" + (x + 1) + "," + (i + 1) + "] value = " + board_Values[x, i] + "Changing with ");
                        //Debug.Log("[" + (x + 1) + "," + (y + 1) + "] value = " + board_Values[x, y]);

                        board_Values[x, i] = board_Values[x, y];
                        blocks[x, i].GetComponent<Block_Manager>().Set_Block_Value(board_Values[x, y]);

                        board_Values[x, y] = 0;
                        blocks[x, y].GetComponent<Block_Manager>().Set_Block_Value(0);
                        //Debug.Log("----------Values Changed----------");

                        break;
                    }
                }
            }
        }
    }

    private void MoveBlocks_Down()
    {
        //Debug.Log("Move Blocks Down");

        for (int x = 0; x < boardWidth; x++)
        {
            for (int y = boardWidth - 2; y >= 0; y--)
            {
                if (board_Values[x, y] == 0) continue;
                //Debug.Log("Column : " + (x + 1) + ",Row :" + (y + 1));

                //merge
                for (int i = boardWidth - 1; i >= 1; i--)
                {
                    if (board_Values[x, i - 1] == board_Values[x, i])
                    {
                        board_Values[x, i - 1] += board_Values[x, i - 1];
                        blocks[x, i - 1].GetComponent<Block_Manager>().Set_Block_Value(board_Values[x, i - 1]);
                        
                        Increase_Score(board_Values[x, i - 1]);
                        
                        board_Values[x, i] = 0;
                        blocks[x, i].GetComponent<Block_Manager>().Set_Block_Value(0);
                    }
                }

                //switch
                for (int i = boardWidth - 1; i >= y; i--)
                {
                    if (board_Values[x, i] == 0)
                    {
                        //Debug.Log("[" + (x + 1) + "," + (i + 1) + "] value = " + board_Values[x, i] + "Changing with ");
                        //Debug.Log("[" + (x + 1) + "," + (y + 1) + "] value = " + board_Values[x, y]);

                        board_Values[x, i] = board_Values[x, y];
                        blocks[x, i].GetComponent<Block_Manager>().Set_Block_Value(board_Values[x, y]);

                        board_Values[x, y] = 0;
                        blocks[x, y].GetComponent<Block_Manager>().Set_Block_Value(0);
                        //Debug.Log("----------Values Changed----------");

                        break;
                    }
                }
            }
        }
    }

    private void MoveBlocks_Right()
    {
        for (int y = 0; y < boardWidth; y++)
        {
            for (int x = boardWidth - 2; x >= 0; x--)
            {
                if (board_Values[x, y] == 0) continue;

                //merge
                for (int i = boardWidth - 1; i >= 1; i--)
                {
                    if (board_Values[i - 1, y] == board_Values[i, y])
                    {
                        board_Values[i - 1, y] += board_Values[i - 1, y];
                        blocks[i - 1, y].GetComponent<Block_Manager>().Set_Block_Value(board_Values[i - 1, y]);
                       
                        Increase_Score(board_Values[x, i - 1]);
                        
                        board_Values[i, y] = 0;
                        blocks[i, y].GetComponent<Block_Manager>().Set_Block_Value(0);
                    }
                }

                //switch
                for (int i = boardWidth - 1; i >= x; i--)
                {
                    if (board_Values[i, y] == 0)
                    {
                        //Debug.Log("[" + (x + 1) + "," + (i + 1) + "] value = " + board_Values[x, i] + "Changing with ");
                        //Debug.Log("[" + (x + 1) + "," + (y + 1) + "] value = " + board_Values[x, y]);

                        board_Values[i, y] = board_Values[x, y];
                        blocks[i, y].GetComponent<Block_Manager>().Set_Block_Value(board_Values[x, y]);

                        board_Values[x, y] = 0;
                        blocks[x, y].GetComponent<Block_Manager>().Set_Block_Value(0);
                        //Debug.Log("----------Values Changed----------");

                        break;
                    }
                }
            }
        }
    }

    private void MoveBlocks_Left()
    {
        //Debug.Log("Move Blocks Right");

        for (int y = 0; y < boardWidth; y++)
        {
            for (int x = 1; x < boardWidth; x++)
            {
                if (board_Values[x, y] == 0) continue;

                //merge
                for (int i = 1; i < boardWidth - 1; i++)
                {
                    if (board_Values[i - 1, y] == board_Values[i, y])
                    {
                        board_Values[i - 1, y] += board_Values[i - 1, y];
                        blocks[i - 1, y].GetComponent<Block_Manager>().Set_Block_Value(board_Values[i - 1, y]);
                        
                        Increase_Score(board_Values[x, i - 1]);
                        
                        board_Values[i, y] = 0;
                        blocks[i, y].GetComponent<Block_Manager>().Set_Block_Value(0);
                    }
                }

                //switch
                for (int i = 0; i < x; i++)
                {
                    if (board_Values[i, y] == 0)
                    {
                        //Debug.Log("[" + (x + 1) + "," + (i + 1) + "] value = " + board_Values[x, i] + "Changing with ");
                        //Debug.Log("[" + (x + 1) + "," + (y + 1) + "] value = " + board_Values[x, y]);

                        board_Values[i, y] = board_Values[x, y];
                        blocks[i, y].GetComponent<Block_Manager>().Set_Block_Value(board_Values[x, y]);

                        board_Values[x, y] = 0;
                        blocks[x, y].GetComponent<Block_Manager>().Set_Block_Value(0);
                        //Debug.Log("----------Values Changed----------");

                        break;
                    }
                }
            }
        }
    }

    private void Increase_Score(int value)
    {
        Level_Manager.instance.IncreasePlayerScore(value);
    }

    private void Change_RandomBlock()
    {
        List<string> coordinates = new List<string>();

        for (int x = 0; x < boardWidth; x++)
        {
            for (int y = 0; y < boardWidth; y++)
            {
                if (board_Values[x, y] == 0) coordinates.Add(x.ToString() + y.ToString());
            }
        }

        if (coordinates.Count > 0)
        {
            int rng_Number = Random.Range(0, coordinates.Count);
            string coordinate = coordinates[rng_Number];

            //Debug.Log("Coordinate : " + coordinate);
            //Debug.Log("x:" + coordinate[0] + ",y:" + coordinate[1]);
            
            int xPos = int.Parse(coordinate[0].ToString()), yPos = int.Parse(coordinate[1].ToString());
            
            //Debug.Log(xPos + "," + yPos);

            board_Values[xPos, yPos] = 2;
            blocks[xPos, yPos].GetComponent<Block_Manager>().Set_Block_Value(2);
        }
    }
}
