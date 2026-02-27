using System;
using Encryption.Services;

class Program
{
    public static void Main()
    {
        Auth auth = new Auth();

        int opcao = -1;

        while (opcao != 4)
        {
            Console.WriteLine("\n=== MENU ===");
            Console.WriteLine("1 - Novo cadastro");
            Console.WriteLine("2 - Logar");
            Console.WriteLine("3 - Checar Usuários Cadastrados");
            Console.WriteLine("4 - Sair");

            if (!int.TryParse(Console.ReadLine(), out opcao))
            {
                Auth.InvalidReturn();
                continue;
            }

            switch (opcao)
            {
                case 1:
                    auth.SignUp();
                    break;

                case 2:
                    auth.LogIn();
                    break;

                case 3:
                    auth.List();
                    break;

                case 4:
                    Console.WriteLine("Saindo...");
                    break;

                default:
                    Auth.InvalidReturn();
                    break;
            }
        }
    }
}