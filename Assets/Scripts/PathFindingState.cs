using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Classe que será o estado que fará a operação dos algoritmos de busca
/// </summary>
public class PathFindingState : SimulatorState
{
    Cell[] cellmap = new Cell[36];

    /// <summary>
    /// 
    /// </summary>
    /// <param name="cellmapObject"></param>
    public PathFindingState(GeneralController generalController)
    {
        Debug.Log("Estado PathFinding");
        for (int i = 0; i < generalController.cellmap.Length; i++)
        {
            cellmap[i] = generalController.cellmap[i].GetComponent<Cell>() as Cell;
            if (i == 35)
            {
                cellmap[i].endPoint = true;
            }
        }

        //generalController.sucessorFuctionLargura = BuscaLargura(cellmap);
        //generalController.sucessorFuctionProfundidade = BuscaProfundidade(cellmap);
        //generalController.sucessorFuctionGulosa = BuscaGulosa(cellmap);
        generalController.sucessorFuctionAStar = BuscaAStar(cellmap);

    }

    /// <summary>
    /// Algoritmo de busca em largura
    /// </summary>
    /// <param name="cells">Recebe um vetor de celulas do mapa</param>
    /// <returns>Retorna uma lista com as ações tomadas até o objetivo ou nulo caso não encontre</returns>
    List<Cell> BuscaLargura(Cell[] cells)
    {
        Debug.Log("Busca em Lagura inicializada!");
        Queue<Cell> fila = new Queue<Cell>();

        List<Cell> verticesMarcados = new List<Cell>();

        Cell ponteiro, ponteiroAuxiliar;

        Cell[] adjacente = new Cell[4];

        ponteiro = cells[0];

        verticesMarcados.Add(ponteiro);//marca a raiz como visitada

        fila.Enqueue(ponteiro);//coloca raiz na fila

        while (fila.Count != 0)
        {
            ponteiroAuxiliar = fila.Peek();
            //Debug.Log("Ponteiro auxiliar =" + ponteiroAuxiliar.coins);
            if (ponteiroAuxiliar.right != ponteiroAuxiliar)
            { //Caso ele aponte para o anterior ele não vai pra trás novamente
                adjacente[0] = ponteiroAuxiliar.right;//meio que aqui era pra receber a lista de adjacencia
            }
            if (ponteiroAuxiliar.left != ponteiroAuxiliar)
            {
                adjacente[1] = ponteiroAuxiliar.left;
            }
            if (ponteiroAuxiliar.up != ponteiroAuxiliar)
            {
                adjacente[2] = ponteiroAuxiliar.up;
            }
            if (ponteiroAuxiliar.down != ponteiroAuxiliar)
            {
                adjacente[3] = ponteiroAuxiliar.down;
            }

            int i = 0;
            while (i < 4) //percorre os vertices adjacentes
            {
                if (adjacente[i] != null)
                {
                    if (!verticesMarcados.Contains(adjacente[i])) //se nÃo foram percorridos
                    {
                        //Debug.Log("Celula " + adjacente[i].coins + " marcado como percorrido!");
                        verticesMarcados.Add(adjacente[i]); //adiciona na lista de percorridos
                        fila.Enqueue(adjacente[i]); //adiciona a fila 
                    }
                    // if (fila.Contains(adjacente[i]))
                    // {

                    // }
                    if (adjacente[i].endPoint == true)
                    { //se for o nó objetivo
                        Debug.Log("Objeto encontrado");
                        for (int f = 0; f < verticesMarcados.Count; f++)
                        {
                            Debug.Log("Posição percorrida " + f + " celula =" + verticesMarcados[f].coins);
                        }
                        return verticesMarcados;
                    }
                }

                i++;
            }

            fila.Dequeue();
        }

        return null;

    }

    /// <summary>
    /// Função que inicializa o ambiente e variáveis para o uso do algoritmo de busca por profundidade 
    /// </summary>
    /// <param name="cells">O vetor com as celulas do mapa</param>
    /// <returns>Retorna a lista com o caminho das celulas que percorreu</returns>
    List<Cell> BuscaProfundidade(Cell[] cells)
    {
        Debug.Log("Busca em Profundidade inicializada!");
        List<Cell> verticesMarcados = new List<Cell>();
        Cell ponteiro;

        ponteiro = cells[0];
        verticesMarcados.Add(ponteiro);

        DeepFindSearch(ponteiro, verticesMarcados);

        for (int i = 0; i < verticesMarcados.Count; i++)
        {
            Debug.Log("Posição percorrida " + i + " celula =" + verticesMarcados[i].coins);
        }

        return verticesMarcados;
    }

    /// <summary>
    /// Algoritmo de busca por profundidade
    /// Ele é recursivo
    /// </summary>
    /// <param name="ponteiroAuxiliar">Celula que está explorando</param>
    /// <param name="verticesMarcados">Lista com celulas que já foram visitadas</param>
    /// <returns></returns>
    List<Cell> DeepFindSearch(Cell ponteiroAuxiliar, List<Cell> verticesMarcados)
    {

        Cell[] adjacente = new Cell[4];
        if (ponteiroAuxiliar.right != ponteiroAuxiliar)
        { //Caso ele aponte para o anterior ele não vai pra trás novamente
            adjacente[0] = ponteiroAuxiliar.right;//meio que aqui era pra receber a lista de adjacencia
        }
        if (ponteiroAuxiliar.left != ponteiroAuxiliar)
        {
            adjacente[1] = ponteiroAuxiliar.left;
        }
        if (ponteiroAuxiliar.up != ponteiroAuxiliar)
        {
            adjacente[2] = ponteiroAuxiliar.up;
        }
        if (ponteiroAuxiliar.down != ponteiroAuxiliar)
        {
            adjacente[3] = ponteiroAuxiliar.down;
        }

        if (ponteiroAuxiliar.endPoint == true)
        { //verifica se o cara é o objetivo
            Debug.Log("Objetivo encontrado!" + "Celula " + ponteiroAuxiliar.coins);
            return null;
        }

        for (int i = 0; i < 4; i++)//ele percorre todos as arestas
        {
            if (adjacente[i] != null)
            {
                if (!verticesMarcados.Contains(adjacente[i]) && !ponteiroAuxiliar.endPoint == true)
                {
                    verticesMarcados.Add(adjacente[i]);
                    DeepFindSearch(adjacente[i], verticesMarcados);
                }
            }
        }
        return null;
    }

    List<Cell> BuscaGulosa(Cell[] cells)
    {
        Debug.Log("Busca gulosa inicializada!");
        List<Cell> verticesMarcados = new List<Cell>();
        List<Cell> melhoresValoresHeuristicos = new List<Cell>();

        Cell ponteiro;

        ponteiro = cells[0];
        verticesMarcados.Add(ponteiro);

        BuscaGulosaAlgoritmo(ponteiro, verticesMarcados, melhoresValoresHeuristicos);

        for (int i = 0; i < verticesMarcados.Count; i++)
        {
            Debug.Log("Posição percorrida " + i + " celula =" + verticesMarcados[i].coins);
        }

        return verticesMarcados;
    }


    List<Cell> BuscaGulosaAlgoritmo(Cell cell, List<Cell> verticesMarcados, List<Cell> melhoresValoresHeuristicos)
    {
        Cell[] adjacente = new Cell[4];

        if (cell.right != cell)
        { //Caso ele aponte para o anterior ele não vai pra trás novamente
            adjacente[0] = cell.right;//meio que aqui era pra receber a lista de adjacencia
        }
        if (cell.left != cell)
        {
            adjacente[1] = cell.left;
        }
        if (cell.up != cell)
        {
            adjacente[2] = cell.up;
        }
        if (cell.down != cell)
        {
            adjacente[3] = cell.down;
        }
        //Debug.Log("Ponteiro =" + cell.coins);
        if (cell.endPoint == true)
        { //verifica se o cara é o objetivo
            //Debug.Log("Objetivo encontrado!" + "Celula " + cell.coins);
            return null;
        }

        for (int i = 0; i < 4; i++)//ele percorre todos as arestas
        {

            if (adjacente[i] != null)
            {

                if (!verticesMarcados.Contains(adjacente[i]) && !cell.endPoint == true)
                {
                    verticesMarcados.Add(adjacente[i]);
                    //Debug.Log("Celula percorrida " + adjacente[i].coins);
                    melhoresValoresHeuristicos.Add(adjacente[i]);
                }
            }
        }
        Cell melhorCelula = melhoresValoresHeuristicos[0];
        for (int i = 0; i < melhoresValoresHeuristicos.Count; i++) //isso vai assegurar que ele só vai andar pelo que tem a melhor heurística
        {
            if (Vector3.Distance(melhoresValoresHeuristicos[i].gameObject.transform.position, cellmap[34].gameObject.transform.position) < Vector3.Distance(melhorCelula.gameObject.transform.position, cellmap[34].gameObject.transform.position))
            {
                //Debug.Log("Distancia da celula = " + melhorCelula + " ate o objetivo é = " + Vector3.Distance(melhorCelula.gameObject.transform.position, cellmap[34].gameObject.transform.position));
                melhorCelula = melhoresValoresHeuristicos[i];
                if (Vector3.Distance(melhorCelula.gameObject.transform.position, cellmap[34].gameObject.transform.position) == 0)
                {
                    Debug.Log("Objetivo encontrado!" + "Celula " + cell.coins);
                    return null;
                }
                //Debug.Log("Distancia da celula atual = " + melhorCelula + " ate o objetivo é = " + Vector3.Distance(melhorCelula.gameObject.transform.position, cellmap[34].gameObject.transform.position));
            }

        }

        if (melhorCelula != null)
        {
            BuscaGulosaAlgoritmo(melhorCelula, verticesMarcados, melhoresValoresHeuristicos);
        }


        return null;
    }


    List<Cell> BuscaAStar(Cell[] cells)
    {
        Debug.Log("Busca AStar inicializada!");
        List<Cell> verticesMarcados = new List<Cell>();
        List<Cell> melhoresValoresHeuristicos = new List<Cell>();

        Cell ponteiro;

        ponteiro = cells[0];
        verticesMarcados.Add(ponteiro);

        AStarSearch(ponteiro, verticesMarcados, melhoresValoresHeuristicos);

        float custo = 0;
        for (int i = 0; i < verticesMarcados.Count; i++)
        {
            Debug.Log("Posição percorrida " + i + " celula =" + verticesMarcados[i].coins);
            custo += ((float)verticesMarcados[i].ambientType);
        }

        Debug.Log("Custo final " + custo);

        return verticesMarcados;
    }

    List<Cell> AStarSearch(Cell cell, List<Cell> verticesMarcados, List<Cell> melhoresValoresHeuristicos)
    {

        Cell[] adjacente = new Cell[4];

        if (cell.right != cell)
        { //Caso ele aponte para o anterior ele não vai pra trás novamente
            adjacente[0] = cell.right;//meio que aqui era pra receber a lista de adjacencia
        }
        if (cell.left != cell)
        {
            adjacente[1] = cell.left;
        }
        if (cell.up != cell)
        {
            adjacente[2] = cell.up;
        }
        if (cell.down != cell)
        {
            adjacente[3] = cell.down;
        }
        //Debug.Log("Ponteiro =" + cell.coins);
        if (cell.endPoint == true)
        { //verifica se o cara é o objetivo
            //Debug.Log("Objetivo encontrado!" + "Celula " + cell.coins);
            return null;
        }

        for (int i = 0; i < 4; i++)//ele percorre todos as arestas
        {

            if (adjacente[i] != null)
            {

                if (!verticesMarcados.Contains(adjacente[i]) && !cell.endPoint == true)
                {
                    verticesMarcados.Add(adjacente[i]);
                    //Debug.Log("Celula percorrida " + adjacente[i].coins);
                    melhoresValoresHeuristicos.Add(adjacente[i]);
                }
            }
        }
        Cell melhorCelula = melhoresValoresHeuristicos[0];
        for (int i = 0; i < melhoresValoresHeuristicos.Count; i++) //isso vai assegurar que ele só vai andar pelo que tem a melhor heurística
        {
            if (Vector3.Distance(melhoresValoresHeuristicos[i].gameObject.transform.position, cellmap[34].gameObject.transform.position) + ((float)melhoresValoresHeuristicos[i].ambientType) < Vector3.Distance(melhorCelula.gameObject.transform.position, cellmap[34].gameObject.transform.position) + ((float)melhorCelula.ambientType))
            {
                //Debug.Log("Distancia da celula = " + melhorCelula + " ate o objetivo é = " + Vector3.Distance(melhorCelula.gameObject.transform.position, cellmap[34].gameObject.transform.position));
                melhorCelula = melhoresValoresHeuristicos[i];
                if (Vector3.Distance(melhorCelula.gameObject.transform.position, cellmap[34].gameObject.transform.position) == 0)
                {
                    Debug.Log("Objetivo encontrado!" + "Celula " + cell.coins);
                    return null;
                }
                //Debug.Log("Distancia da celula atual = " + melhorCelula + " ate o objetivo é = " + Vector3.Distance(melhorCelula.gameObject.transform.position, cellmap[34].gameObject.transform.position));
            }

        }

        if (melhorCelula != null)
        {
            BuscaGulosaAlgoritmo(melhorCelula, verticesMarcados, melhoresValoresHeuristicos);
        }


        return null;
    }
}
