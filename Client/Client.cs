using System.Net.Sockets;
using System.Text;

class Client
{
    //Endereço ip do servidor
    private const string serverIP = "127.0.0.1";
    //Porta do servidor
    private const int serverPort = 12345;

    static void Main()
    {
        try
        {
            //Efetua a criação do socket
            TcpClient client = new TcpClient(serverIP, serverPort);
            NetworkStream stream = client.GetStream();

            Console.WriteLine("**   Digite um dos comandos abaixo:   **");
            Console.WriteLine("**   add-client|nome|cpf|endereco     **");
            Console.WriteLine("**   remove-client|nome               **");
            Console.WriteLine("**   consultar-client|nome            **");
            Console.WriteLine("**   sair                             **");

            while (true)
            {
                //Lê a opção
                string input = Console.ReadLine();
                if (input.Equals("sair"))
                    break;

                //Envia a requisição
                byte[] data = Encoding.ASCII.GetBytes(input);
                stream.Write(data, 0, data.Length);

                //Recebe a resposta
                byte[] buffer = new byte[1024];
                int bytesRead = stream.Read(buffer, 0, buffer.Length);
                string response = Encoding.ASCII.GetString(buffer, 0, bytesRead);

                Console.WriteLine(response);
            }

            //Fecha o socket
            client.Close();
        }
        catch (Exception ex)
        {
            Console.WriteLine("Erro no cliente!! " + ex.Message);
        }
    }
}
