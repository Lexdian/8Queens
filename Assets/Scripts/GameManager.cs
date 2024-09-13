using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class GameManager : MonoBehaviour
{
    public int Size;
    private int queens;
    public float cellsize;
    public GameObject cell, botaoSolucionar, painel;
    public GameObject[,] grid;
    public Text vitoria;

    public InputField input;

    private bool solucionado = false;

    public Camera cam;

    // Start is called before the first frame update
    void Start()
    {
        Cell.mudouState += UpdateGrid;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void GenerateGrid()
    {
        for (int i = 0; i < Size; i++)
        {
            for(int j = 0; j < Size; j++)
            {
                GameObject celula = GameObject.Instantiate(cell, new Vector2((-Size / 2) * cellsize + i * cellsize, (Size / 2) * cellsize - j * cellsize), Quaternion.identity);
                grid[i, j] = celula;
            }
        }
    }

    public void solucionar()
    {
        StartCoroutine(Solucionar(0));
    }

    public void comecar()
    {
        try
        {
            Size = int.Parse(input.text);
            grid = new GameObject[Size, Size];
            queens = Size;
            GenerateGrid();
            painel.SetActive(false);
            botaoSolucionar.SetActive(true);
            cam.orthographicSize = Size * 3 / 4;
        }
        catch
        {
            Debug.Log("Bote um número");
        }
    }

    public IEnumerator Solucionar(int coluna)
    {
        if (coluna == Size)
        {
            solucionado = true;
            StopAllCoroutines();
            yield break;
        }
        for (int i = 0; i < Size; i++)
        {
            if (grid[coluna, i].GetComponent<Cell>().state == Cell.states.blocked)
            {
                continue;
            }
            grid[coluna, i].GetComponent<Cell>().state = Cell.states.queen;
            UpdateGrid();

            // Atraso de 0.5 segundos
            yield return new WaitForSeconds(0.5f);

            yield return StartCoroutine(Solucionar(coluna + 1));
            grid[coluna, i].GetComponent<Cell>().state = Cell.states.empty;
            UpdateGrid();
        }
    }

    private void UpdateGrid()
    {
        queens = 0;
        for (int i = 0; i < Size; i++)
        {
            for (int j = 0; j < Size; j++)
            {
                // Obtém a célula atual
                var currentCell = grid[i, j].GetComponent<Cell>();

                // Variáveis para marcar se encontramos uma rainha na linha, coluna ou diagonais
                bool queenInRow = false;
                bool queenInColumn = false;
                bool queenInDiagonal1 = false;
                bool queenInDiagonal2 = false;

                // Verifica a linha
                for (int x = 0; x < Size; x++)
                {
                    if (grid[i, x].GetComponent<Cell>().state == Cell.states.queen || grid[i, x].GetComponent<Cell>().state == Cell.states.error)
                    {
                        if (grid[i, x] != grid[i, j])
                        {
                            queenInRow = true;
                            break; // Podemos parar de verificar se já encontramos uma rainha
                        }
                    }
                }

                // Verifica a coluna
                for (int y = 0; y < Size; y++)
                {
                    if (grid[y, j].GetComponent<Cell>().state == Cell.states.queen || grid[y, j].GetComponent<Cell>().state == Cell.states.error)
                    {
                        if (grid[y, j] != grid[i, j])
                        {
                            queenInColumn = true;
                            break; // Podemos parar de verificar se já encontramos uma rainha
                        }
                    }
                }

                // Verifica a diagonal principal (\) - da posição atual (i, j)
                for (int d = -Size; d <= Size; d++)
                {
                    int newRow = i + d;
                    int newCol = j + d;

                    if (newRow >= 0 && newRow < Size && newCol >= 0 && newCol < Size)
                    {
                        if (grid[newRow, newCol].GetComponent<Cell>().state == Cell.states.queen || grid[newRow, newCol].GetComponent<Cell>().state == Cell.states.error)
                        {
                            if (grid[newRow, newCol] != grid[i, j])
                            {
                                queenInDiagonal1 = true;
                                break; // Podemos parar de verificar se já encontramos uma rainha
                            }
                        }
                    }
                }

                // Verifica a diagonal secundária (/) - da posição atual (i, j)
                for (int d = -Size; d <= Size; d++)
                {
                    int newRow = i + d;
                    int newCol = j - d;

                    if (newRow >= 0 && newRow < Size && newCol >= 0 && newCol < Size)
                    {
                        if (grid[newRow, newCol].GetComponent<Cell>().state == Cell.states.queen || grid[newRow, newCol].GetComponent<Cell>().state == Cell.states.error)
                        {
                            if (grid[newRow, newCol] != grid[i, j])
                            {
                                queenInDiagonal2 = true;
                                break; // Podemos parar de verificar se já encontramos uma rainha
                            }
                        }
                    }
                }

                // Se encontramos uma rainha na mesma linha, coluna, ou qualquer diagonal, bloqueamos a célula atual
                if (queenInRow || queenInColumn || queenInDiagonal1 || queenInDiagonal2)
                {
                    if (currentCell.state == Cell.states.queen || currentCell.state == Cell.states.error)
                    {
                        currentCell.state = Cell.states.error;
                    }
                    else
                    {
                        currentCell.state = Cell.states.blocked;
                    }
                }
                else
                {
                    if (currentCell.state == Cell.states.queen || currentCell.state == Cell.states.error)
                    {
                        currentCell.state = Cell.states.queen;
                    }
                    else
                    {
                        currentCell.state = Cell.states.empty;
                    }
                }
            }
        }

        for (int i = 0; i < Size; i++) 
        {
            for (int j = 0; j < Size; j++)
            {
                if (grid[i,j].GetComponent<Cell>().state == Cell.states.queen)
                {
                    queens += 1;
                }
            }
        }

        if(queens == Size)
        {
            vitoria.gameObject.SetActive(true);
        }
    }

}
