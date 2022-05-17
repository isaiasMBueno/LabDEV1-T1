# LabDEV1-T1
Trabalho T1 - Lab. Desenvolvimento 1 - IFSP Catanduva: API DriveThru

<!---Esses são exemplos. Veja https://shields.io para outras pessoas ou para personalizar este conjunto de escudos. Você pode querer incluir dependências, status do projeto e informações de licença aqui--->

![GitHub IFSP](https://ctd.ifsp.edu.br/images/IFSP-CTD2.png)
![Github IFSP](https://img.shields.io/badge/IFSP-5%C2%BA%20Semestre%20ADS-green?style=plastic)
![Github ADS](https://img.shields.io/badge/ADS-Lab.%20Dev.%20l-green?style=plastic&logo=superuser)
![Github C#](https://img.shields.io/badge/C%23-.NET%20Core-blue?style=plastic&logo=gitlab)
![Github API](https://img.shields.io/badge/API-Rest-brightgreen?style=plastic&logo=stackexchange)

<img src="http://www.joaoalberto.com/wp-content/uploads/2020/05/09/DriveThru.jpg" alt="DriveThru">

> Este projeto tem por finalidade a construção de uma API(Application Programming Interface) que simule o funcionamento de um DriveThru com base em um CRUD simples do padrão REST. De forma que seja possível fazer, alterar pedidos, além de produzi-los, entrega-los e retira-los.

### Ajustes e melhorias

O projeto ainda está em desenvolvimento e as próximas atualizações serão voltadas nas seguintes tarefas:

- [x] Criação dos endpoints
- [x] Tratamento de erros
- [x] Tratamento de Requisições
- [ ] Padronização de URLs
- [ ] Implementação de Testes Unitários
- [ ] Revisão do código e boas práticas

## 💻 Pré-requisitos

Antes de começar, verifique se você atendeu aos seguintes requisitos:
* Instalar o Framework `< .NET Core 5.0 (ou superior) >`
* Instalar um IDE para a linguagem C# `< VSCode / VisualStudio >`
* Instalar uma ferramenta para testes na API `< Postman / Insomnia >`
* Uma máquina com pelo menos `<8GB RAM (recomendável)>`.
* Conhecer `<C# / .NET Core / padrão REST>`.

## 🚀 Iniciando o projeto

Para executar o projeto, siga estas etapas:

🟦🟦VSCode:
```
Com a pasta do projeto aberta, abra um novo terminal e digite o comando: "dotnet run"
```

🟪🟪VisualStudio:
```
Abra o projeto pelo VisualStudio e execute em modo Debug (F5) ou sem Debug(Ctrl + F5)
```

## ☢️Observação☢️
```
Nos arquivos do projeto, está incluso o arquivo <Trabalhov3.postman_collection>.
Basta importá-lo para utilizar as rotas já configuradas no Postman e fazer os testes.
```

## ⚠️⚠️IMPORTANTE!!⚠️⚠️
```
Durante os testes da API, os valores retornados são inteiros, correspondentes a uma posição do Enum.
```
📌Origem do Pedido
<table border="1">
    <tr>
        <td>Nº</td>
        <td>Origem</td>
    </tr>
    <tr>
        <td>0</td>
        <td>Balcão</td>
    </tr>
    <tr>
        <td>1</td>
        <td>Delivery</td>
    </tr>
    <tr>
        <td>2</td>
        <td>DriveThru</td>
    </tr>
</table>

📌Status do Pedido
<table border="1">
    <tr>
        <td>Nº</td>
        <td>Status</td>
    </tr>
    <tr>
        <td>0</td>
        <td>Aguardando</td>
    </tr>
    <tr>
        <td>1</td>
        <td>Fazendo</td>
    </tr>
    <tr>
        <td>2</td>
        <td>Pronto</td>
    </tr>
</table>


## 📫 Contribuindo para o projeto
Para contribuir com o projeto, siga estas etapas:

1. Bifurque este repositório.
2. Crie um branch: `git checkout -b <nome_branch>`.
3. Faça suas alterações e confirme-as: `git commit -m '<mensagem_commit>'`
4. Envie para o branch original: `git push origin <nome_do_projeto> / <local>`
5. Crie a solicitação de pull.

Como alternativa, consulte a documentação do GitHub em [como criar uma solicitação pull](https://help.github.com/en/github/collaborating-with-issues-and-pull-requests/creating-a-pull-request).

## 🤝 Colaboradores

O projeto foi desenvolvido pelos alunos Isaías M. Bueno (CT3006395) e Pedro H. Guzzo (CT3007324), do IFSP Catanduva.

<table>
  <tr>
    <td align="center">
      <a href="https://www.linkedin.com/in/isaías-bueno-80a0ba157">
        <img src="https://media-exp1.licdn.com/dms/image/C4D03AQHUINi-Lf1_tg/profile-displayphoto-shrink_200_200/0/1585184845908?e=1652918400&v=beta&t=GLaQDXE09vJbt2hr3afBD-6BLd0u3IYf7jh2VQVQhvg" target="_blank" width="100px;" alt="Foto do Isaías Bueno no GitHub"/><br>
        <sub>
          <b>Isaías Bueno</b>
        </sub>
      </a>
    </td>
    <td align="center">
      <a href="https://www.linkedin.com/in/pedro-guzzo-426439207/">
        <img src="https://media-exp1.licdn.com/dms/image/D4E35AQEW-Sm_wsrasQ/profile-framedphoto-shrink_800_800/0/1623860914006?e=1652274000&v=beta&t=Q8DFALo-vbBd6ig5oH7wXjJK1i6PWbCu_pojAvJuu74" target="_blank" width="100px;" alt="Foto do Pedro Guzzo"/><br>
        <sub>
          <b>Pedro Guzzo</b>
        </sub>
      </a>
    </td>
  </tr>
</table>


## 😄 Seja um dos colaboradores<br>

Quer aprender mais sobre C#? Clique [AQUI](https://www.macoratti.net) e aprenda com um dos melhores professores gratuitamente.

## 📝 Licença

Esse projeto não tem fins lucrativos ou comerciais. Sendo assim, é livre para qualquer pessoa utilizar para estudo.

[⬆ Voltar ao topo](#LabDEV1-T1)<br>