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

            Console.WriteLine("*********************************************");
            Console.WriteLine("**   Instruções:                           **");
            Console.WriteLine("**        a significa adicionar cliente;   **");
            Console.WriteLine("**        r significa remover cliente;     **");
            Console.WriteLine("**        c significa consultar cliente;   **");
            Console.WriteLine("**        s significa sair;                **");
            Console.WriteLine("**   Sintaxe dos comandos:                 **");
            Console.WriteLine("**   a|nome|cpf|endereco                   **");
            Console.WriteLine("**   r|nome (remove cliente específico)    **");
            Console.WriteLine("**   c|nome (consulta cliente específico)  **");
            Console.WriteLine("**   r (remove todos os clientes)          **");
            Console.WriteLine("**   c (consulta todos os clientes)        **");
            Console.WriteLine("**   s                                     **");
            Console.WriteLine("*********************************************");

            while (true)
            {
                //Lê a opção
                string input = Console.ReadLine();
                if (input.Equals("s"))
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
            Console.WriteLine("Erro no cliente!! Exceção: " + ex.Message);
        }
    }
}
