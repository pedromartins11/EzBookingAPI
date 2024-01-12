# Trabalho Prático II
Integração de Sistemas de Informação 

Licenciatura em Engenharia de Sistemas Informáticos (regime *pós-laboral*) 2023-24

## Constituição do grupo 3
| Número  | Nome                       | Email                  |
| 23527   | Pedro Miguel Gomes Martins | a23527@alunos.ipca.pt  |
| 23528   | Luís Miguel da Costa Anjo  | a23528@alunos.ipca.pt  |
| 23893   | Diogo Gomes Silva          | a23893@alunos.ipca.pt  |

# Título do projeto
EzHouse

# Introdução
No âmbito das unidades curriculares de Projeto Aplicado, Programação de Dispositivos Móveis e Integração de Sistemas de Informação foi proposto o desenvolvimento de uma Aplicação Móvel no qual será possível disponibilizar alojamentos locais para reserva, assim como, efetuar a reserva dos mesmos.

# Breve descrição
A nossa plataforma será utilizada como partilha e reserva de espaços para aluguer, esta permitirá aos utilizadores a reserva de um alojamento ou de um quarto universitário partilhado.
Damos oportunidade também para os utilizadores colocarem a sua residência ou quarto na plataforma para futuros alugueres.





# Getting Started
1. Software dependencies

# Screenshots



# Build

1. Installation process LINUX & MacOSx & Windows
  
   
   2. Logs



# API Commands
1. API Logs
  

# Database

1. Seed Database
cd/EzBooking
dotnet run seeddata

2. Migrations:
Package Manager (Criar Migrations): 
```shell
Add-Migration InitialCreate
```
3. Package Manager (Inserir na BD Migrations):
```shell
 Update-Database
```

4. SQL SERVER:
Sessão:
 - Server: bkbd.database.windows.net
 - Username: rootadmin
 - Password: Root123!

  "ConnectionStrings": {
    "DefaultConnection": "Data Source=bkbd.database.windows.net;Initial Catalog=bookinghouses;User ID=rootadmin;Password=Root123!;Connect Timeout=30;Encrypt=True;Trust Server Certificate=False;Application Intent=ReadWrite;Multi Subnet Failover=False"
  },


5. Run tests
   ```shell
   docker exec -it api-app npm test
   ```

# API - Backend

   http://api.localhost/

   http://api.localhost/docs


# Contribute

- Pedro Martins, nº 23527
- Luís Anjo, nº 23528
- Diogo Silva, nº 23893

# Links

- [BookingItHere.com](https://BookingItHere.com)
- [Visual Studio Code](https://github.com/Microsoft/vscode)
