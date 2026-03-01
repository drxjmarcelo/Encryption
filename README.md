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
O campo Username não é considerado dado sensível e pode ser armazenado em texto simples.
A senha é protegida utilizando hash com BCrypt.
### Dentro do `SignUp()`
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
⠀⠀└──AppContextDb.cs📄
<br>
├──Migrations📁
<br>
⠀⠀└──[...]
<br>
├──Models📁
<br>
⠀⠀└──User.cs📄
<br>
├──Services📁
<br>
⠀⠀└──Auth.cs📄
<br>
├──Target📁
<br>
⠀⠀└──Yay.cs📄
<br>
├──Program.cs📄
<br>
## Partes Principais
### Program.cs📄
Classe principal que executa e controla todas as outras.

### Models📁/User.cs📄
Classe que cria o objeto **Usuário**.

### Services📁/Auth.cs📄
Classe que cria/manipula dados e acessa o banco de dados.





