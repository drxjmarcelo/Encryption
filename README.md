# 🔐 Sistema de Autenticação (Console App – C#)

Sistema de autenticação desenvolvido em C# com foco em arquitetura, organização de código e evolução progressiva de segurança.
<br>
O projeto começou como uma implementação in-memory para validação de lógica e posteriormente evoluiu para integração com banco de dados e hash de senha.

## 🎯 Objetivo
### Este projeto tem como finalidade:
* Praticar orientação a objetos
* Estruturar um CRUD de usuários
* Implementar fluxo de autenticação
* Trabalhar com persistência de dados
* Aplicar hash seguro de senha (BCrypt)
* Simular cenários reais de evolução de sistema (migração de estrutura)

## 🏗 Arquitetura
### O sistema segue uma separação lógica simples:
* Model → Entidade User
* Service → Regras de negócio (cadastro, login, exclusão, etc.)
* Program → Interface via console
<br>
A persistência já foi implementada com banco relacional, substituindo o armazenamento temporário em memória.

## 🚀 Funcionalidades
✔ Cadastro de usuário
<br>
✔ Login com validação de credenciais
<br>
✔ Hash de senha com BCrypt
<br>
✔ Listagem de usuários
<br>
✔ Exclusão por ID
<br>
✔ Wipe completo da base (ambiente de teste)
<br>
✔ Update de usuário

## 🔐 Segurança
* Senhas não são armazenadas em texto puro
* Utilização de BCrypt para hashing
* Separação entre regra de negócio e persistência

## 🧠 Conceitos Aplicados
* CRUD completo
* Auto-increment / Sequence de banco
* Boas práticas básicas de organização
### Preparação para futura implementação de:
* Controle de papéis (Admin/User)
* Logs de auditoria
* API REST
* Interface web

## 💻 Execução
Projeto executado como Console Application (.NET).

## 👨‍💻 Tecnologias
### C# 🟨
Linguagem principal utilizada no desenvolvimento do sistema.

### NET (.NET / .NET Core) 🟪
Plataforma utilizada para execução da aplicação Console.

### Entity Framework Core 🟩
ORM utilizado para mapeamento objeto-relacional e comunicação com o banco de dados.

### PostgreSQL 🐘
Sistema de gerenciamento de banco de dados relacional.

### BCrypt 🔐
Algoritmo utilizado para hash seguro de senhas.

### DBeaver ⬜
Ferramenta utilizada para administração e consulta do banco de dados.

## 📦 Instalação Dos Pacotes
Devem ser instalados usando: 
* `dotnet add package Microsoft.EntityFrameworkCore`
* `dotnet add package Microsoft.EntityFrameworkCore.Tools`
* `dotnet add package Microsoft.EntityFrameworkCore.Design`
* `dotnet add package Npgsql.EntityFrameworkCore.PostgreSQL`
* `dotnet tool install --global dotnet-ef`
* `dotnet add package BCrypt.Net-Next`

## 📄 Detalhes
Para garantir a segurança, o campo usuário é público no banco de dados,
mas a senha é protegida com hash da seguinte maneira:
### Dentro do `SignIn()`
```
string hashedPassword = BCrypt.Net.BCrypt.HashPassword(password);

    context.Users.Add(new User
    {
        Username = username,
        Password = hashedPassword
    });

    context.SaveChanges();
```
### Dentro do `LogIn()`
```
var user = context.Users.FirstOrDefault(u => u.Username == username);

    if (user != null && BCrypt.Net.BCrypt.Verify(password, user.Password))
    {
        Console.WriteLine("Login realizado com sucesso!");
        Yay.Message();
    }

```

## 💡📋 Separação de Responsábilidades
A arquitetura segue uma estrutura bem simples:
<br>
Encryption📁
<br>
├──Data📁
<br>
└──AppContextDb.cs📄
<br>
├──Migrations📁
<br>
└──[...]
<br>
├──Models📁
<br>
└──User.cs📄
<br>
├──Services📁
<br>
└──Auth.cs📄
<br>
├──Target📁
<br>
└──Yay.cs📄
<br>
├──Program.cs📄
<br>
## Partes Principais
### Program.cs📄
Classe principal que executa e controla todas as outras
<br>
Código:
```
using System;
using Encryption.Services;

class Program
{
    public static void Main()
    {
        Auth auth = new Auth();

        int opcao = -1;

        while (opcao != 7)
        {
            Console.WriteLine("\n=== MENU ===");
            Console.WriteLine("1 - Novo cadastro");
            Console.WriteLine("2 - Logar");
            Console.WriteLine("3 - Checar Usuários Cadastrados");
            Console.WriteLine("4 - Atualizar Usuário");
            Console.WriteLine("5 - Deletar Usuário");
            Console.WriteLine("6 - Redefinir Banco de Dados");
            Console.WriteLine("7 - Sair");

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
                    auth.UpdateUser();
                    break;

                case 5:
                    auth.DeleteUser();
                    break;
                
                case 6:
                    auth.Wipe();
                    break;
                
                case 7:
                    Console.WriteLine("Saindo...");
                    break;

                default:
                    Auth.InvalidReturn();
                    break;
            }
        }
    }
}
```
### Models📁/User.cs📄
Classe que cria o objeto **Usuário**
<br>
Código:
```
using System.ComponentModel.DataAnnotations;

namespace Encryption.Models
{
    public class User
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Username { get; set; }

        [Required]
        public string Password { get; set; }

        // Construtor vazio obrigatório para o EF
        public User()
        {
        }

        // Seu construtor original
        public User(string username, string password)
        {
            Username = username;
            Password = password;
        }
    }
}
```
### Services📁/Auth.cs📄
Classe que cria/manipula dados e acessa o banco de dados
<br>
Código:
```
using System;
using System.Linq;
using Encryption.Models;
using Encryption.Target;
using Encryption.Data;
using BCrypt.Net;

namespace Encryption.Services
{
    public class Auth
    {
        public static void ReturnMenu()
        {
            Console.WriteLine("\n--Pressione ENTER para voltar ao menu--");
            Console.ReadLine();
        }

        public static void InvalidReturn()
        {
            Console.WriteLine("Opção inválida!");
            Console.WriteLine("\n--Pressione ENTER para voltar ao menu--");
            Console.ReadLine();
        }

        public void SignUp()
        {
            using var context = new AppContextDb();

            Console.Write("Digite o username: ");
            string? username = Console.ReadLine();

            Console.Write("Digite a senha: ");
            string? password = Console.ReadLine();

            if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
            {
                Console.WriteLine("Username e senha não podem ser vazios.");
                ReturnMenu();
                return;
            }

            if (context.Users.Any(u => u.Username == username))
            {
                Console.WriteLine("Usuário já existe!");
                ReturnMenu();
                return;
            }

            // 🔐 HASH DA SENHA
            string hashedPassword = BCrypt.Net.BCrypt.HashPassword(password);

            context.Users.Add(new User
            {
                Username = username,
                Password = hashedPassword
            });

            context.SaveChanges();

            Console.WriteLine("Cadastro realizado com sucesso!");
            ReturnMenu();
        }

        public void LogIn()
        {
            using var context = new AppContextDb();

            if (!context.Users.Any())
            {
                Console.WriteLine("Nenhum usuário cadastrado.");
                ReturnMenu();
                return;
            }

            Console.Write("Digite o username: ");
            string? username = Console.ReadLine();

            Console.Write("Digite a senha: ");
            string? password = Console.ReadLine();

            if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
            {
                Console.WriteLine("Dados inválidos.");
                ReturnMenu();
                return;
            }

            var user = context.Users.FirstOrDefault(u => u.Username == username);

            if (user != null && BCrypt.Net.BCrypt.Verify(password, user.Password))
            {
                Console.WriteLine("Login realizado com sucesso!");
                Yay.Message();
            }
            else
            {
                Console.WriteLine("Usuário ou senha incorretos.");
                ReturnMenu();
            }
        }

        public void List()
        {
            using var context = new AppContextDb();

            if (!context.Users.Any())
            {
                Console.WriteLine("Nenhum usuário cadastrado.");
                ReturnMenu();
                return;
            }

            Console.WriteLine("=== Usuários Cadastrados ===");

            foreach (var user in context.Users.ToList())
            {
                Console.WriteLine($"{user.Id} - {user.Username}");
            }

            ReturnMenu();
        }
        public void UpdateUser()
        {
            using var context = new AppContextDb();

            if (!context.Users.Any())
            {
                Console.WriteLine("Nenhum usuário cadastrado.");
                ReturnMenu();
                return;
            }

            Console.Write("Digite o username: ");
            string? username = Console.ReadLine();

            Console.Write("Digite a senha: ");
            string? password = Console.ReadLine();

            if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
            {
                Console.WriteLine("Dados inválidos.");
                ReturnMenu();
                return;
            }

            var loggedUser = context.Users.FirstOrDefault(u => u.Username == username);

            if (loggedUser == null || !BCrypt.Net.BCrypt.Verify(password, loggedUser.Password))
            {
                Console.WriteLine("Usuário ou senha incorretos.");
                ReturnMenu();
                return;
            }

            Console.WriteLine("\n=== Escolha ===");
            Console.WriteLine("[U]- Alterar Nome de Usuário");
            Console.WriteLine("[S]- Alterar Senha");
            string? opcao = Console.ReadLine();

            switch (opcao?.ToUpper())
            {
                case "U":
                    Console.Write("Digite seu novo Nome de Usuário: ");
                    string? newUsername = Console.ReadLine();

                    if (string.IsNullOrWhiteSpace(newUsername))
                    {
                        Console.WriteLine("Usuário inválido.");
                        break;
                    }

                    loggedUser.Username = newUsername;
                    context.SaveChanges();

                    Console.WriteLine("Nome de Usuário atualizado com sucesso!");
                    break;

                case "S":
                    Console.Write("Digite a nova senha: ");
                    string? newPassword = Console.ReadLine();

                    if (string.IsNullOrWhiteSpace(newPassword))
                    {
                        Console.WriteLine("Senha inválida.");
                        break;
                    }

                    loggedUser.Password = BCrypt.Net.BCrypt.HashPassword(newPassword);
                    context.SaveChanges();

                    Console.WriteLine("Senha atualizada com sucesso!");
                    break;

                default:
                    Console.WriteLine("Opção inválida!");
                    break;
            }

            ReturnMenu();
        }
        public void DeleteUser()
        {
            using var context = new AppContextDb();

            if (!context.Users.Any())
            {
                Console.WriteLine("Nenhum usuário cadastrado.");
                ReturnMenu();
                return;
            }

            Console.Write("Digite o username: ");
            string? username = Console.ReadLine();

            Console.Write("Digite a senha: ");
            string? password = Console.ReadLine();

            if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
            {
                Console.WriteLine("Dados inválidos.");
                ReturnMenu();
                return;
            }

            var loggedUser = context.Users.FirstOrDefault(u => u.Username == username);

            if (loggedUser == null || !BCrypt.Net.BCrypt.Verify(password, loggedUser.Password))
            {
                Console.WriteLine("Usuário ou senha incorretos.");
                ReturnMenu();
                return;
            }

            Console.WriteLine("\n=== Usuários Cadastrados ===");

            var users = context.Users.ToList();

            foreach (var u in users)
            {
                Console.WriteLine($"{u.Id} - {u.Username}");
            }

            Console.WriteLine("\nOpções:");
            Console.WriteLine("1 - Deletar por ID");
            Console.WriteLine("2 - Deletar TODOS (exceto você)");
            Console.WriteLine("0 - Cancelar");

            if (!int.TryParse(Console.ReadLine(), out int option))
            {
                InvalidReturn();
                return;
            }

            switch (option)
            {
                case 1:
                    Console.Write("Digite o ID que deseja deletar: ");

                    if (!int.TryParse(Console.ReadLine(), out int id))
                    {
                        InvalidReturn();
                        return;
                    }

                    var userToDelete = context.Users.FirstOrDefault(u => u.Id == id);

                    if (userToDelete == null)
                    {
                        Console.WriteLine("Usuário não encontrado.");
                    }
                    else if (userToDelete.Id == loggedUser.Id)
                    {
                        Console.WriteLine("Você não pode deletar sua própria conta aqui.");
                    }
                    else
                    {
                        context.Users.Remove(userToDelete);
                        context.SaveChanges();
                        Console.WriteLine("Usuário deletado com sucesso!");
                    }
                    break;

                case 2:
                    Console.WriteLine("Tem certeza que deseja deletar TODOS os usuários exceto você? (S/N)");
                    string? confirm = Console.ReadLine();

                    if (confirm?.ToUpper() == "S")
                    {
                        var usersToRemove = context.Users
                            .Where(u => u.Id != loggedUser.Id)
                            .ToList();

                        context.Users.RemoveRange(usersToRemove);
                        context.SaveChanges();

                        Console.WriteLine("Todos os outros usuários foram deletados.");
                    }
                    else
                    {
                        Console.WriteLine("Operação cancelada.");
                    }
                    break;

                case 0:
                    Console.WriteLine("Operação cancelada.");
                    break;

                default:
                    InvalidReturn();
                    return;
            }
            ReturnMenu();
        }
        public void Wipe()
        {
            using var context = new AppContextDb();
            if (!context.Users.Any())
            {
                Console.WriteLine("Não há usuários para deletar.");
                ReturnMenu();
                return;
            }

            Console.WriteLine("===ATENÇÃO!!===");
            Console.WriteLine("TODOS OS USUÁRIOS SERÃO DELETADOS PERMANENTEMENTE.");
            Console.WriteLine("Digite 'CONFIRMAR' para continuar:");

            string? confirm = Console.ReadLine();

            if (confirm != "CONFIRMAR")
            {
                Console.WriteLine("Operação cancelada.");
                ReturnMenu();
                return;
            }

            context.Users.RemoveRange(context.Users);
            context.SaveChanges();

            Console.WriteLine("Banco de usuários limpo com sucesso.");
            ReturnMenu();
        }
    }
}
```
