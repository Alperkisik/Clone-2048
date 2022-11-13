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
    SaveLoadSystem saveLoadSystem;

    private void Awake()
    {
        instance = this;
    }

    void Start()
    {
        Event_Listener();
        Start_Board();
    }

    void Start_Board()
    {
        blocks = new GameObject[boardWidth, boardWidth];
        board_Values = new int[boardWidth, boardWidth];
        GameObject data = GameObject.Find("Datas");
        saveLoadSystem = data.GetComponent<SaveLoadSystem>();

        if (GameMode.instance.new_game) Generate_Board(); else LoadGame();
    }

    void LoadGame()
    {
        //Debug.Log("board manager load game");
        float xPos = -1.8f, yPos = 1.8f, space = 0.2f;
        boardWidth = saveLoadSystem.board_width;
        //Debug.Log("board manager boardWidth : " + boardWidth);
        //Debug.Log("board manager boardWidth : " + level.boardValues_text);

        int index = 0;
        string boardText = saveLoadSystem.board_values_text;
        for (int x = 0; x < boardWidth; x++)
        {
            for (int y = 0; y < boardWidth; y++)
            {
                board_Values[x, y] = ConvertCharToBoardValue(boardText[index]);
                index++;
            }
        }

        for (int y = 0; y < boardWidth; y++)
        {
            xPos = -1.8f;
            for (int x = 0; x < boardWidth; x++)
            {
                GameObject block = Instantiate(blockPrefab, new Vector2(xPos, yPos), Quaternion.identity, blockParentTransform);
                block.name = "Block [" + (x + 1) + "," + (y + 1) + "]";
                block.GetComponent<Block_Manager>().Set_Block_Value(board_Values[x, y]);
                block.GetComponent<Block_Manager>().SetXYpositions(x, y);
                blocks[x, y] = block;

                xPos += 1f + space;
            }
            yPos += -1f - space;
        }

        Increase_Score(saveLoadSystem.score);

        OnBoardGenerationDone?.Invoke(this, System.EventArgs.Empty);
    }

    void Generate_Board()
    {
        saveLoadSystem.ResetData();

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

        SaveGame();
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
        SaveGame();
    }

    private void InputManager_Event_OnRightInputReceived(object sender, System.EventArgs e)
    {
        //Debug.Log("Input Manager right input event triggered");
        MoveBlocks_Right();
        Change_RandomBlock();
        SaveGame();
    }

    private void InputManager_Event_OnUpInputReceived(object sender, System.EventArgs e)
    {
        //Debug.Log("Input Manager up input event triggered");
        MoveBlocks_UP();
        Change_RandomBlock();
        SaveGame();
    }

    private void InputManager_Event_OnDownInputReceived(object sender, System.EventArgs e)
    {
        //Debug.Log("Input Manager down input event triggered");
        MoveBlocks_Down();
        Change_RandomBlock();
        SaveGame();
    }

    private void MoveBlocks_UP()
    {
        //Debug.Log("Move Blocks UP");
        int yPos;
        for (int x = 0; x < boardWidth; x++)
        {
            for (int y = 1; y < boardWidth; y++)
            {
                if (board_Values[x, y] == 0) continue;
                //Debug.Log("Column : " + (x + 1) + ",Row :" + (y + 1));

                // 0 0 0 0
                // 0 2 2 0
                // 0 2 2 0
                // 0 0 0 0

                bool condition;
                yPos = y;

                do
                {
                    if (yPos - 1 < 0) break;

                    if (board_Values[x, yPos - 1] == 0) //switch
                    {
                        board_Values[x, yPos - 1] = board_Values[x, yPos];
                        blocks[x, yPos - 1].GetComponent<Block_Manager>().Set_Block_Value(board_Values[x, yPos - 1]);

                        board_Values[x, yPos] = 0;
                        blocks[x, yPos].GetComponent<Block_Manager>().Set_Block_Value(0);

                        yPos--;
                        condition = true;
                    }
                    else if (board_Values[x, yPos - 1] == board_Values[x, yPos]) //merge
                    {
                        board_Values[x, yPos - 1] += board_Values[x, yPos - 1];
                        blocks[x, yPos - 1].GetComponent<Block_Manager>().Set_Block_Value(board_Values[x, yPos - 1]);

                        Increase_Score(board_Values[x, yPos - 1]);

                        board_Values[x, yPos] = 0;
                        blocks[x, yPos].GetComponent<Block_Manager>().Set_Block_Value(0);

                        break;
                    }
                    else break;
                } while (condition);

                /*
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
                */
            }
        }
    }

    private void MoveBlocks_Down()
    {
        //Debug.Log("Move Blocks Down");
        int yPos;
        for (int x = 0; x < boardWidth; x++)
        {
            for (int y = boardWidth - 2; y >= 0; y--)
            {
                if (board_Values[x, y] == 0) continue;

                bool condition;
                yPos = y;

                do
                {
                    if (yPos + 1 >= boardWidth) break;

                    if (board_Values[x, yPos + 1] == 0) //switch
                    {
                        board_Values[x, yPos + 1] = board_Values[x, yPos];
                        blocks[x, yPos + 1].GetComponent<Block_Manager>().Set_Block_Value(board_Values[x, yPos + 1]);

                        board_Values[x, yPos] = 0;
                        blocks[x, yPos].GetComponent<Block_Manager>().Set_Block_Value(0);

                        yPos++;
                        condition = true;
                    }
                    else if (board_Values[x, yPos + 1] == board_Values[x, yPos]) //merge
                    {
                        board_Values[x, yPos + 1] += board_Values[x, yPos + 1];
                        blocks[x, yPos + 1].GetComponent<Block_Manager>().Set_Block_Value(board_Values[x, yPos + 1]);

                        Increase_Score(board_Values[x, yPos + 1]);

                        board_Values[x, yPos] = 0;
                        blocks[x, yPos].GetComponent<Block_Manager>().Set_Block_Value(0);

                        break;
                    }
                    else break;
                } while (condition);

                //Debug.Log("Column : " + (x + 1) + ",Row :" + (y + 1));
                /*
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
                */
            }
        }
    }

    private void MoveBlocks_Right()
    {
        int xPos;
        for (int y = 0; y < boardWidth; y++)
        {
            for (int x = boardWidth - 2; x >= 0; x--)
            {
                if (board_Values[x, y] == 0) continue;

                bool condition;
                xPos = x;

                do
                {
                    if (xPos + 1 >= boardWidth) break;

                    if (board_Values[xPos + 1, y] == 0) //switch
                    {
                        board_Values[xPos + 1, y] = board_Values[xPos, y];
                        blocks[xPos + 1, y].GetComponent<Block_Manager>().Set_Block_Value(board_Values[xPos + 1, y]);

                        board_Values[xPos, y] = 0;
                        blocks[xPos, y].GetComponent<Block_Manager>().Set_Block_Value(0);

                        xPos++;
                        condition = true;
                    }
                    else if (board_Values[xPos + 1, y] == board_Values[xPos, y]) //merge
                    {
                        board_Values[xPos + 1, y] += board_Values[xPos + 1, y];
                        blocks[xPos + 1, y].GetComponent<Block_Manager>().Set_Block_Value(board_Values[xPos + 1, y]);

                        Increase_Score(board_Values[xPos + 1, y]);

                        board_Values[xPos, y] = 0;
                        blocks[xPos, y].GetComponent<Block_Manager>().Set_Block_Value(0);

                        break;
                    }
                    else break;
                } while (condition);

                /*
                //merge
                for (int i = boardWidth - 1; i >= 1; i--)
                {
                    if (board_Values[i - 1, y] == board_Values[i, y])
                    {
                        board_Values[i - 1, y] += board_Values[i - 1, y];
                        blocks[i - 1, y].GetComponent<Block_Manager>().Set_Block_Value(board_Values[i - 1, y]);

                        Increase_Score(board_Values[i - 1, y]);

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
                }*/
            }
        }
    }

    private void MoveBlocks_Left()
    {
        //Debug.Log("Move Blocks Right");
        int xPos;
        for (int y = 0; y < boardWidth; y++)
        {
            for (int x = 1; x < boardWidth; x++)
            {
                if (board_Values[x, y] == 0) continue;

                bool condition;
                xPos = x;

                do
                {
                    if (xPos - 1 < 0) break;

                    if (board_Values[xPos - 1, y] == 0) //switch
                    {
                        board_Values[xPos - 1, y] = board_Values[xPos, y];
                        blocks[xPos - 1, y].GetComponent<Block_Manager>().Set_Block_Value(board_Values[xPos - 1, y]);

                        board_Values[xPos, y] = 0;
                        blocks[xPos, y].GetComponent<Block_Manager>().Set_Block_Value(0);

                        xPos--;
                        condition = true;
                    }
                    else if (board_Values[xPos - 1, y] == board_Values[xPos, y]) //merge
                    {
                        board_Values[xPos - 1, y] += board_Values[xPos - 1, y];
                        blocks[xPos - 1, y].GetComponent<Block_Manager>().Set_Block_Value(board_Values[xPos - 1, y]);

                        Increase_Score(board_Values[xPos - 1, y]);

                        board_Values[xPos, y] = 0;
                        blocks[xPos, y].GetComponent<Block_Manager>().Set_Block_Value(0);

                        break;
                    }
                    else break;
                } while (condition);

                /*
                //merge
                for (int i = 1; i < boardWidth - 1; i++)
                {
                    if (board_Values[i - 1, y] == board_Values[i, y])
                    {
                        board_Values[i - 1, y] += board_Values[i - 1, y];
                        blocks[i - 1, y].GetComponent<Block_Manager>().Set_Block_Value(board_Values[i - 1, y]);

                        Increase_Score(board_Values[i - 1, y]);

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
                }*/
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

    private void SaveGame()
    {
        saveLoadSystem.score = Level_Manager.instance.GetPlayerScore();
        saveLoadSystem.board_width = boardWidth;

        string boardValues_text = "";
        for (int x = 0; x < boardWidth; x++)
        {
            for (int y = 0; y < boardWidth; y++)
            {
                boardValues_text += ConvertBoardValueToChar(board_Values[x, y]);
            }
        }

        saveLoadSystem.board_values_text = boardValues_text;
        saveLoadSystem.SaveLevel();
    }

    private int ConvertCharToBoardValue(char value)
    {
        //a 0 b 2 c 4 d 8 e 16 f 32 g 64 h 128 i 256 j 512 k 1024 

        if (value == 'b') return 2;
        else if (value == 'c') return 4;
        else if (value == 'd') return 8;
        else if (value == 'e') return 16;
        else if (value == 'f') return 32;
        else if (value == 'g') return 64;
        else if (value == 'h') return 128;
        else if (value == 'i') return 256;
        else if (value == 'j') return 512;
        else if (value == 'k') return 1024;
        else if (value == 'a') return 0;
        else return 0;
    }

    private string ConvertBoardValueToChar(int value)
    {
        //a 0 b 2 c 4 d 8 e 16 f 32 g 64 h 128 i 256 j 512 k 1024 

        if (value == 2) return "b";
        else if (value == 4) return "c";
        else if (value == 8) return "d";
        else if (value == 16) return "e";
        else if (value == 32) return "f";
        else if (value == 64) return "g";
        else if (value == 128) return "h";
        else if (value == 256) return "i";
        else if (value == 512) return "j";
        else if (value == 1024) return "k";
        else if (value == 0) return "a";
        else return "";
    }
}
