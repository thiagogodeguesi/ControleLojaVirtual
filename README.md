# ControleLojaVirtual
Controle de Loja Virtual - Teste


<b>Pre requisitos </b> <br>
Instalação dos pacotes <br>
Microsoft.AspNetCore.Authentication.JwtBearer<br>
Microsoft.EntityFrameworkCore.InMemory<br>
<p>
<b>Testes realizados via POSTMAN (https://www.postman.com)</b> <br>
<i>*Alterar a porta do endereço abaixo (Porta 51563 para ????)</i>
<p>

<b>BUSCA TOKEN UTILIZANDO USUARIO ABAIXO</b><br>
POST - http://localhost:51563/api/token <br>
{
    "Nome":"Teste",
    "Senha":"1234"
}

<p>
<b>LISTA DE TODOS OS PRODUTOS</b> <br>
GET - http://localhost:51563/api/Produtos

<p>
<b>LISTA DE ESTOQUE</b><br>
GET - http://localhost:51563/api/estoque

<p>
<b>LISTA DE ESTOQUE POR LOJA</b><br>
GET - http://localhost:51563/api/estoque/2

<p>
<b>ADICIONAR PRODUTO A LOJA VIA COMPRA </b><br>
POST - http://localhost:51563/api/Estoque/Compra <br>
{
    "idloja":2,
    "idproduto":2,
    "qtde":100
}
<p>
<b>SUBTRAI PRODUTO DA LOJA VIA VENDA</b><br>
POST - http://localhost:51563/api/Estoque/Venda <br>
{
    "idloja":2,
    "idproduto":2,
    "qtde":100
}

<p>
<b>LISTA DE LOJAS
GET - http://localhost:51563/api/lojas </b><br>
<p>
<b>DADOS DA LOJA POR ID
GET - http://localhost:51563/api/lojas/1 </b><br>
<p>
<b>ADICIONAR UMA NOVA LOJA</b><br>
POST - http://localhost:51563/api/lojas <br>
{
    "Nome":"Loja de Teste 4",
    "Site":"lojavirtual4.com",
    "Endereco":"Rua dos testes 4, 4000 - Vila Debud - SP / 04000-000 "
}
<p>
<b>ATUALIZAR UMA LOJA</b><br>
PUT - http://localhost:51563/api/lojas <br>
{
    "Id":"4",
    "Nome":"Loja de Teste 4 - ATUALIZADO",
    "Site":"lojavirtual4.com - ATUALIZADO",
    "Endereco":"Rua dos testes 4, 4000 - Vila Debud - SP / 04000-000  - ATUALIZADO"
}
<p>
<b>DELETE UMA LOJA</b><br>
DELETE - http://localhost:51563/api/lojas/4
