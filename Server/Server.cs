using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;

class Server
{
    //Armazenar dados temporariamente no dicionário
    private static Dictionary<string, DadosClient> clientDB = new Dictionary<string, DadosClient>();
    
    //Porta de conexão
    private const int port = 12345;

    static void Main()
    {
        try
        {
            //Criação do socket
            TcpListener listener = new TcpListener(IPAddress.Any, port);
            listener.Start();

            Console.WriteLine("Servidor em operação!! Aguardando conexões...");

            while (true)
            {
                //Aguarda conexões e aceita em seguida
                TcpClient client = listener.AcceptTcpClient();
                Console.WriteLine("Conexão aceita!");

                //Lê requisições
                NetworkStream stream = client.GetStream();
                byte[] buffer = new byte[1024];

                int bytesRead = stream.Read(buffer, 0, buffer.Length);
                string clientMessage = Encoding.ASCII.GetString(buffer, 0, bytesRead);

                string[] request = clientMessage.Split('|');
                string response = ProcessaRequest(request);

                //Envia respostas
                byte[] data = Encoding.ASCII.GetBytes(response);
                stream.Write(data, 0, data.Length);

                //Fecha o socket
                client.Close();
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine("Erro no servidor! " + ex.Message);
        }
    }

    //Processar a requisição do client
    static string ProcessaRequest(string[] request)
    {
        string command = request[0].Trim().ToLower();

        switch (command)
        {
            case "add-client":
                if (request.Length == 4)
                {
                    string nome = request[1].Trim();
                    string cpf = request[2].Trim();
                    string endereco = request[3].Trim();

                    if (!clientDB.ContainsKey(nome))
                    {
                        clientDB[nome] = new DadosClient(cpf, endereco);
                        return "Cliente adicionado com sucesso!";
                    }
                    else
                        return "Cliente já existe!";
                }
                else
                    return "Formato inválido para adicionar cliente!! Correto: add-client|nome|cpf|endereco";

            case "remove-client":
                if (request.Length == 2)
                {
                    string nome = request[1].Trim();

                    if (clientDB.ContainsKey(nome))
                    {
                        clientDB.Remove(nome);
                        return "Cliente removido com sucesso!!";
                    }
                    else
                        return "Cliente não encontrado!!";
                }
                else
                    return "Formato inválido para remover cliente!! Correto: remove-client|nome";

            case "consultar-client":
                if (request.Length == 2)
                {
                    string nome = request[1].Trim();

                    if (clientDB.ContainsKey(nome))
                    {
                        DadosClient client = clientDB[nome];
                        return $"Nome: {nome}, CPF: {client.CPF}, Endereco: {client.Endereco}";
                    }
                    else
                        return "Cliente não encontrado!!";
                }
                else
                    return "Formato inválido para consultar cliente!! Correto: consultar-client|nome";

            default:
                return "Comando inválido!!";
        }
    }
}

class DadosClient
{
    public string CPF { get; }
    public string Endereco { get; }

    public DadosClient(string cpf, string endereco)
    {
        CPF = cpf;
        Endereco = endereco;
    }
}
