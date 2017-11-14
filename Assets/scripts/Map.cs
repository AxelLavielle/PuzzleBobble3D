using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Map : MonoBehaviour {
    private Ball[][] _mapBall = { new Ball[8], new Ball[7], new Ball[8], new Ball[7], new Ball[8], new Ball[7], new Ball[8], new Ball[7], new Ball[8], new Ball[7], new Ball[8], new Ball[7] };
    private int[][] _map = { new int[8], new int[7], new int[8], new int[7], new int[8], new int[7], new int[8], new int[7], new int[8], new int[7], new int[8], new int[7] };
    private int sum = 0;
    private int ballNumber = 30;
    public int dropping = 0;
    public long score = 0;
    [SerializeField]
    Vector3 offset;
    [SerializeField]
    GameObject ballObject;
    private GameObject upWall;
    private bool hard = false;

    // Initialisation
    private void Start() {
        
        int i = -1;
        int j;
        int[] col = new int[5];

        // Color Init
        while (++i < 5)
        {
            col[i] = (int)(Random.value * 5);
            j = i;
            while (--j >= 0)
                if (col[i] == col[j])
                {
                    col[i] = (int)(Random.value * 5);
                    j = i;
                }
        }
        upWall = GameObject.FindGameObjectWithTag("upWall");

        // Map init
        i = -1;
        while (++i < 12)
        {
            j = -1;
            if (i < 4)
                while (++j < 8 - (i % 2))
                {
                    _map[i][j] = col[(j + i - j % 2) % 5];
                    GameObject tmp = Instantiate(ballObject, new Vector3(0, 0, 0), Quaternion.identity) as GameObject;
                    _mapBall[i][j] = tmp.GetComponent<Ball>();
                    _mapBall[i][j].setColor(_map[i][j]);
                    _mapBall[i][j].transform.parent = gameObject.transform;
                    _mapBall[i][j].transform.localPosition = offset + new Vector3(j + (i % 2) * 0.5f, 0, -i);
                }
            else
                while (++j < 8 - (i % 2))
                    _map[i][j] = -1;
        }
    }

    // Reinitialisation
    public void reset(int _lvl)
    {
        int i = -1;
        int j;
        int[] col = new int[5];

        // color init
        while (++i < 5)
        {
            col[i] = (int)(Random.value * 5);
            j = i;
            while (--j >= 0)
                if (col[i] == col[j])
                {
                    col[i] = (int)(Random.value * 5);
                    j = i;
                }
        }

        upWall.transform.localPosition = new Vector3(0, 0, 8);
        // map init
        i = -1;
        while (++i < 12)
        {
            j = -1;
            if (i < 4)
                while (++j < 8 - (i % 2))
                {
                    if (_map[i][j] != -1)
                        _mapBall[i][j].destruct();
                    _map[i][j] = col[(j + i - j % 2) % 5];
                    GameObject tmp = Instantiate(ballObject, new Vector3(0, 0, 0), Quaternion.identity) as GameObject;
                    _mapBall[i][j] = tmp.GetComponent<Ball>();
                    _mapBall[i][j].setColor(_map[i][j]);
                    _mapBall[i][j].transform.parent = gameObject.transform;
                    _mapBall[i][j].transform.localPosition = offset + new Vector3(j + (i % 2) * 0.5f, 0, -i);
                }
            else
                while (++j < 8 - (i % 2))
                {
                    if (_map[i][j] != -1)
                        _mapBall[i][j].destruct();
                    _map[i][j] = -1;
                }
        }
        ballNumber = 30;
        score = 0;
        if (_lvl == 0)
        {
            transform.GetChild(1).gameObject.SetActive(true);
            transform.GetChild(2).gameObject.SetActive(false);
            transform.GetChild(3).gameObject.SetActive(true);
            transform.GetChild(4).gameObject.SetActive(false);
        }
        else
        {
            transform.GetChild(1).gameObject.SetActive(false);
            transform.GetChild(2).gameObject.SetActive(true);
            transform.GetChild(3).gameObject.SetActive(false);
            transform.GetChild(4).gameObject.SetActive(true);
            transform.GetChild(2).GetComponent<sineMesh>().create();
            transform.GetChild(4).GetComponent<sineMesh>().create();
        }
        hard = false;
        if (_lvl == 2)
            hard = true;
    }

    // get remaining colors
    public int getColor()
    {
        bool[] all_color = { false, false, false, false, false };
        int i = -1;
        int j;
        while (++i < 12)
        {
            j = -1;
            while (++j < 8 - (i % 2))
                if (_map[i][j] != -1)
                    all_color[_map[i][j]] = true;
        }
        int tmp = (int)(Random.value * 5 % 5);
        while (!all_color[tmp])
            tmp = (int)(Random.value * 5 % 5);
        return (tmp);
    }

    // This function search the balls with the same color of the given one, or search all the ball not stuck somewhere and can destroy
    private int[][] searchAndDestroy(int posX, int posY, int color, bool destruct, int[][] mapp)
    {
        if (posY >= 0)
            mapp[posY][posX] = -1;
        sum += 1;
        if (destruct)
        {
            ballNumber--;
            _mapBall[posY][posX].destruct();
        }
        if (posY - 1 >= 0 && posX - (1 - Mathf.Abs(posY) % 2) >= 0 && ((color == -1 && mapp[posY - 1][posX - (1 - Mathf.Abs(posY) % 2)] != color) || (color != -1 && mapp[posY - 1][posX - (1 - Mathf.Abs(posY) % 2)] == color)))
            mapp = searchAndDestroy(posX - (1 - Mathf.Abs(posY) % 2), posY - 1, color, destruct, mapp);
        if (posY - 1 >= 0 && posX + (Mathf.Abs(posY) % 2) < 8 - (1 - Mathf.Abs(posY) % 2) && ((color == -1 && mapp[posY - 1][posX + (Mathf.Abs(posY) % 2)] != color) || (color != -1 && mapp[posY - 1][posX + (Mathf.Abs(posY) % 2)] == color)))
            mapp = searchAndDestroy(posX + (Mathf.Abs(posY) % 2), posY - 1, color, destruct, mapp);
        if (posY >= 0 && posX - 1 >= 0 && ((color == -1 && mapp[posY][posX - 1] != color) || (color != -1 && mapp[posY][posX - 1] == color)))
            mapp = searchAndDestroy(posX - 1, posY, color, destruct, mapp);
        if (posY >= 0 && posX + 1 < 8 - (Mathf.Abs(posY) % 2) && ((color == -1 && mapp[posY][posX + 1] != color) || (color != -1 && mapp[posY][posX + 1] == color)))
            mapp = searchAndDestroy(posX + 1, posY, color, destruct, mapp);
        if (posY + 1 < 12 && posX - (1 - Mathf.Abs(posY) % 2) >= 0 && ((color == -1 && mapp[posY + 1][posX - (1 - Mathf.Abs(posY) % 2)] != color) || (color != -1 && mapp[posY + 1][posX - (1 - Mathf.Abs(posY) % 2)] == color)))
            mapp = searchAndDestroy(posX - (1 - Mathf.Abs(posY) % 2), posY + 1, color, destruct, mapp);
        if (posY + 1 < 12 && posX + (Mathf.Abs(posY) % 2) < 8 - (1 - Mathf.Abs(posY) % 2) && ((color == -1 && mapp[posY + 1][posX + (Mathf.Abs(posY) % 2)] != color) || (color != -1 && mapp[posY + 1][posX + (Mathf.Abs(posY) % 2)] == color)))
            mapp = searchAndDestroy(posX + (Mathf.Abs(posY) % 2), posY + 1, color, destruct, mapp);
        return (mapp);
    }

    // Check if drop is needed, and drop if it's the case
    private void prepareDrop(int[][] mapp)
    {
        int i = -1;
        int j;
        int scoreCounter = 0;

        while (++i < 8)
            mapp = searchAndDestroy(i, -1, -1, false, mapp);
        i = -1;
        while (++i < 12)
        {
            j = -1;
            while (++j < 8 - (i % 2))
                if (mapp[i][j] != -1)
                {
                    _map[i][j] = -1;
                    _mapBall[i][j].drop();
                    dropping++;
                    ballNumber--;
                    scoreCounter++;
                }
        }
        if (scoreCounter > 0)
            score += (long)Mathf.Pow(2, scoreCounter) * 10;
    }

    // check if a lvl is ended
    private bool checkVictory()
    {
        int i = -1;
        int j;
        bool victory = true;

        while (++i < 12)
        {
            j = -1;
            while (++j < 8 - (i % 2))
                if (_map[i][j] != -1)
                    victory = false;
        }
        return (victory);
    }

    // make the upwall go down
    public int goDown(int n)
    {
        int def = 0;
        if (n > 5 - ballNumber / 8)
        {
            upWall.transform.localPosition = new Vector3(0, 0, upWall.transform.localPosition.z - 1);
            if (hard)
            {
                transform.GetChild(2).GetComponent<sineMesh>().invert *= -1;
                transform.GetChild(2).GetComponent<sineMesh>().create();
                transform.GetChild(4).GetComponent<sineMesh>().invert *= -1;
                transform.GetChild(4).GetComponent<sineMesh>().create();
            }
            int i = -1;
            int j;
            while (++i < 12)
            {
                j = -1;
                while (++j < 8 - (i % 2))
                    if (_map[i][j] != -1)
                    {
                        _mapBall[i][j].transform.localPosition = new Vector3(_mapBall[i][j].transform.localPosition.x, _mapBall[i][j].transform.localPosition.y, _mapBall[i][j].transform.localPosition.z - 1);
                        if (i + 8 - upWall.transform.localPosition.z > 11)
                          def = -1;
                    }
            }
            return (def);
        }
        return (n);
    }

    // add a new ball on the map
    public int addBall(int posX, int posY, int color, Ball ball)
    {
        if (posY > 11)
        {
            ball.destruct();
            return (0);
        }
        else
        {
            posY -= 8 - (int)upWall.transform.localPosition.z;
            ballNumber++;
            //modify the old map
            _map[posY][posX] = color;
            _mapBall[posY][posX] = ball;

            //init the new map
            int[][] mapp = { new int[8], new int[7], new int[8], new int[7], new int[8], new int[7], new int[8], new int[7], new int[8], new int[7], new int[8], new int[7] };
            int i = -1;
            int j;
            while (++i < 12)
            {
                j = -1;
                while (++j < 8 - (i % 2))
                  mapp[i][j] = _map[i][j];
            }
            //search and destroy
            sum = 0;
            mapp = searchAndDestroy(posX, posY, color, false, mapp);
            if (sum > 2)
            {
                score += sum * 10;
                _map = searchAndDestroy(posX, posY, color, true, _map);
                prepareDrop(mapp);
                return (checkVictory() ? 1 : -1);
            }
            return (-1);
        }
    }
}
