USE [Desapegando]
GO

INSERT INTO [dbo].[Condomino] ([Id],[Nome],[Sobrenome],[Sexo],[Telefone],[Email],[Cpf],[Apartamento],[Ativo],[DataNascimento],[DataRegistro]) VALUES (NEWID() ,'João','Alberto',0,'413335265','joao@gmail.com','36298533079','102',0,'1990-06-26',GETDATE())
INSERT INTO [dbo].[Condomino] ([Id],[Nome],[Sobrenome],[Sexo],[Telefone],[Email],[Cpf],[Apartamento],[Ativo],[DataNascimento],[DataRegistro]) VALUES (NEWID() ,'Pedro','Alberto',0,'413335265','pedro@gmail.com','36298533079','102',0,'1990-06-26',GETDATE())
INSERT INTO [dbo].[Condomino] ([Id],[Nome],[Sobrenome],[Sexo],[Telefone],[Email],[Cpf],[Apartamento],[Ativo],[DataNascimento],[DataRegistro]) VALUES (NEWID() ,'Carlos','Alberto',0,'413335265','carlos@gmail.com','36298533079','102',0,'1990-06-26',GETDATE())
INSERT INTO [dbo].[Condomino] ([Id],[Nome],[Sobrenome],[Sexo],[Telefone],[Email],[Cpf],[Apartamento],[Ativo],[DataNascimento],[DataRegistro]) VALUES (NEWID() ,'Leonam','Alberto',0,'413335265','leonam@gmail.com','36298533079','102',0,'1990-06-26',GETDATE())
INSERT INTO [dbo].[Condomino] ([Id],[Nome],[Sobrenome],[Sexo],[Telefone],[Email],[Cpf],[Apartamento],[Ativo],[DataNascimento],[DataRegistro]) VALUES (NEWID() ,'Vini','Alberto',0,'413335265','vini@gmail.com','36298533079','102',0,'1990-06-26',GETDATE())
INSERT INTO [dbo].[Condomino] ([Id],[Nome],[Sobrenome],[Sexo],[Telefone],[Email],[Cpf],[Apartamento],[Ativo],[DataNascimento],[DataRegistro]) VALUES (NEWID() ,'Lucas','Alberto',0,'413335265','lucas@gmail.com','36298533079','102',0,'1990-06-26',GETDATE())
INSERT INTO [dbo].[Condomino] ([Id],[Nome],[Sobrenome],[Sexo],[Telefone],[Email],[Cpf],[Apartamento],[Ativo],[DataNascimento],[DataRegistro]) VALUES (NEWID() ,'Fernando','Alberto',0,'413335265','fernando@gmail.com','36298533079','102',0,'1990-06-26',GETDATE())
INSERT INTO [dbo].[Condomino] ([Id],[Nome],[Sobrenome],[Sexo],[Telefone],[Email],[Cpf],[Apartamento],[Ativo],[DataNascimento],[DataRegistro]) VALUES (NEWID() ,'Walter','Alberto',0,'413335265','walter@gmail.com','36298533079','102',0,'1990-06-26',GETDATE())
INSERT INTO [dbo].[Condomino] ([Id],[Nome],[Sobrenome],[Sexo],[Telefone],[Email],[Cpf],[Apartamento],[Ativo],[DataNascimento],[DataRegistro]) VALUES (NEWID() ,'Marcos','Alberto',0,'413335265','marcos@gmail.com','36298533079','102',0,'1990-06-26',GETDATE())
INSERT INTO [dbo].[Condomino] ([Id],[Nome],[Sobrenome],[Sexo],[Telefone],[Email],[Cpf],[Apartamento],[Ativo],[DataNascimento],[DataRegistro]) VALUES (NEWID() ,'Dagoberto','Alberto',0,'413335265','dagoberto@gmail.com','36298533079','102',0,'1990-06-26',GETDATE())
INSERT INTO [dbo].[Condomino] ([Id],[Nome],[Sobrenome],[Sexo],[Telefone],[Email],[Cpf],[Apartamento],[Ativo],[DataNascimento],[DataRegistro]) VALUES (NEWID() ,'Ramiro','Alberto',0,'413335265','ramiro@gmail.com','36298533079','102',0,'1990-06-26',GETDATE())
INSERT INTO [dbo].[Condomino] ([Id],[Nome],[Sobrenome],[Sexo],[Telefone],[Email],[Cpf],[Apartamento],[Ativo],[DataNascimento],[DataRegistro]) VALUES (NEWID() ,'Rui','Alberto',0,'413335265','rui@gmail.com','36298533079','102',0,'1990-06-26',GETDATE())
INSERT INTO [dbo].[Condomino] ([Id],[Nome],[Sobrenome],[Sexo],[Telefone],[Email],[Cpf],[Apartamento],[Ativo],[DataNascimento],[DataRegistro]) VALUES (NEWID() ,'Ronaldo','Alberto',0,'413335265','ronaldo@gmail.com','36298533079','102',0,'1990-06-26',GETDATE())

GO


USE [Desapegando]
GO

INSERT INTO [dbo].[Produto]([Id],[Nome],[Descricao],[Categoria],[DataPublicacao],[Preco],[Ativo],[Desistencia],[EstadoProduto],[Curtida],[Quantidade],[CondominoId],[DataVenda]) VALUES(NEWID(),'Produto Legal','Descrição legal',0,GETDATE(),100,0,1,0,0,1,(SELECT TOP 1 id FROM Condomino),'2023-06-15 10:50:41.653')
INSERT INTO [dbo].[Produto]([Id],[Nome],[Descricao],[Categoria],[DataPublicacao],[Preco],[Ativo],[Desistencia],[EstadoProduto],[Curtida],[Quantidade],[CondominoId],[DataVenda]) VALUES(NEWID(),'Produto Legal','Descrição legal',0,GETDATE(),100,1,0,0,0,1,(SELECT TOP 1 id FROM Condomino),null)
INSERT INTO [dbo].[Produto]([Id],[Nome],[Descricao],[Categoria],[DataPublicacao],[Preco],[Ativo],[Desistencia],[EstadoProduto],[Curtida],[Quantidade],[CondominoId],[DataVenda]) VALUES(NEWID(),'Produto Legal','Descrição legal',0,GETDATE(),100,0,0,0,0,1,(SELECT TOP 1 id FROM Condomino),'2023-06-15 10:50:41.653')
INSERT INTO [dbo].[Produto]([Id],[Nome],[Descricao],[Categoria],[DataPublicacao],[Preco],[Ativo],[Desistencia],[EstadoProduto],[Curtida],[Quantidade],[CondominoId],[DataVenda]) VALUES(NEWID(),'Produto Legal','Descrição legal',0,GETDATE(),100,1,0,0,0,1,(SELECT TOP 1 id FROM Condomino),null)
INSERT INTO [dbo].[Produto]([Id],[Nome],[Descricao],[Categoria],[DataPublicacao],[Preco],[Ativo],[Desistencia],[EstadoProduto],[Curtida],[Quantidade],[CondominoId],[DataVenda]) VALUES(NEWID(),'Produto Legal','Descrição legal',0,GETDATE(),100,0,1,0,0,1,(SELECT TOP 1 id FROM Condomino),'2023-06-15 10:50:41.653')
INSERT INTO [dbo].[Produto]([Id],[Nome],[Descricao],[Categoria],[DataPublicacao],[Preco],[Ativo],[Desistencia],[EstadoProduto],[Curtida],[Quantidade],[CondominoId],[DataVenda]) VALUES(NEWID(),'Produto Legal','Descrição legal',0,GETDATE(),100,1,0,0,0,1,(SELECT TOP 1 id FROM Condomino),null)
INSERT INTO [dbo].[Produto]([Id],[Nome],[Descricao],[Categoria],[DataPublicacao],[Preco],[Ativo],[Desistencia],[EstadoProduto],[Curtida],[Quantidade],[CondominoId],[DataVenda]) VALUES(NEWID(),'Produto Legal','Descrição legal',0,GETDATE(),100,0,0,0,0,1,(SELECT TOP 1 id FROM Condomino),'2023-06-15 10:50:41.653')
INSERT INTO [dbo].[Produto]([Id],[Nome],[Descricao],[Categoria],[DataPublicacao],[Preco],[Ativo],[Desistencia],[EstadoProduto],[Curtida],[Quantidade],[CondominoId],[DataVenda]) VALUES(NEWID(),'Produto Legal','Descrição legal',0,GETDATE(),100,1,0,0,0,1,(SELECT TOP 1 id FROM Condomino),null)
INSERT INTO [dbo].[Produto]([Id],[Nome],[Descricao],[Categoria],[DataPublicacao],[Preco],[Ativo],[Desistencia],[EstadoProduto],[Curtida],[Quantidade],[CondominoId],[DataVenda]) VALUES(NEWID(),'Produto Legal','Descrição legal',0,GETDATE(),100,0,1,0,0,1,(SELECT TOP 1 id FROM Condomino),'2023-06-15 10:50:41.653')
INSERT INTO [dbo].[Produto]([Id],[Nome],[Descricao],[Categoria],[DataPublicacao],[Preco],[Ativo],[Desistencia],[EstadoProduto],[Curtida],[Quantidade],[CondominoId],[DataVenda]) VALUES(NEWID(),'Produto Legal','Descrição legal',0,GETDATE(),100,1,0,0,0,1,(SELECT TOP 1 id FROM Condomino),null)
INSERT INTO [dbo].[Produto]([Id],[Nome],[Descricao],[Categoria],[DataPublicacao],[Preco],[Ativo],[Desistencia],[EstadoProduto],[Curtida],[Quantidade],[CondominoId],[DataVenda]) VALUES(NEWID(),'Produto Legal','Descrição legal',0,GETDATE(),100,0,0,0,0,1,(SELECT TOP 1 id FROM Condomino),'2023-06-15 10:50:41.653')
INSERT INTO [dbo].[Produto]([Id],[Nome],[Descricao],[Categoria],[DataPublicacao],[Preco],[Ativo],[Desistencia],[EstadoProduto],[Curtida],[Quantidade],[CondominoId],[DataVenda]) VALUES(NEWID(),'Produto Legal','Descrição legal',0,GETDATE(),100,1,0,0,0,1,(SELECT TOP 1 id FROM Condomino),null)
INSERT INTO [dbo].[Produto]([Id],[Nome],[Descricao],[Categoria],[DataPublicacao],[Preco],[Ativo],[Desistencia],[EstadoProduto],[Curtida],[Quantidade],[CondominoId],[DataVenda]) VALUES(NEWID(),'Produto Legal','Descrição legal',0,GETDATE(),100,0,1,0,0,1,(SELECT TOP 1 id FROM Condomino),'2023-06-15 10:50:41.653')
INSERT INTO [dbo].[Produto]([Id],[Nome],[Descricao],[Categoria],[DataPublicacao],[Preco],[Ativo],[Desistencia],[EstadoProduto],[Curtida],[Quantidade],[CondominoId],[DataVenda]) VALUES(NEWID(),'Produto Legal','Descrição legal',0,GETDATE(),100,1,0,0,0,1,(SELECT TOP 1 id FROM Condomino),null)
INSERT INTO [dbo].[Produto]([Id],[Nome],[Descricao],[Categoria],[DataPublicacao],[Preco],[Ativo],[Desistencia],[EstadoProduto],[Curtida],[Quantidade],[CondominoId],[DataVenda]) VALUES(NEWID(),'Produto Legal','Descrição legal',0,GETDATE(),100,0,0,0,0,1,(SELECT TOP 1 id FROM Condomino),'2023-06-15 10:50:41.653')
INSERT INTO [dbo].[Produto]([Id],[Nome],[Descricao],[Categoria],[DataPublicacao],[Preco],[Ativo],[Desistencia],[EstadoProduto],[Curtida],[Quantidade],[CondominoId],[DataVenda]) VALUES(NEWID(),'Produto Legal','Descrição legal',0,GETDATE(),100,1,0,0,0,1,(SELECT TOP 1 id FROM Condomino),null)
INSERT INTO [dbo].[Produto]([Id],[Nome],[Descricao],[Categoria],[DataPublicacao],[Preco],[Ativo],[Desistencia],[EstadoProduto],[Curtida],[Quantidade],[CondominoId],[DataVenda]) VALUES(NEWID(),'Produto Legal','Descrição legal',0,GETDATE(),100,0,1,0,0,1,(SELECT TOP 1 id FROM Condomino),'2023-06-15 10:50:41.653')
INSERT INTO [dbo].[Produto]([Id],[Nome],[Descricao],[Categoria],[DataPublicacao],[Preco],[Ativo],[Desistencia],[EstadoProduto],[Curtida],[Quantidade],[CondominoId],[DataVenda]) VALUES(NEWID(),'Produto Legal','Descrição legal',0,GETDATE(),100,1,0,0,0,1,(SELECT TOP 1 id FROM Condomino),null)
INSERT INTO [dbo].[Produto]([Id],[Nome],[Descricao],[Categoria],[DataPublicacao],[Preco],[Ativo],[Desistencia],[EstadoProduto],[Curtida],[Quantidade],[CondominoId],[DataVenda]) VALUES(NEWID(),'Produto Legal','Descrição legal',0,GETDATE(),100,0,0,0,0,1,(SELECT TOP 1 id FROM Condomino),'2023-06-15 10:50:41.653')
INSERT INTO [dbo].[Produto]([Id],[Nome],[Descricao],[Categoria],[DataPublicacao],[Preco],[Ativo],[Desistencia],[EstadoProduto],[Curtida],[Quantidade],[CondominoId],[DataVenda]) VALUES(NEWID(),'Produto Legal','Descrição legal',0,GETDATE(),100,1,0,0,0,1,(SELECT TOP 1 id FROM Condomino),null)
INSERT INTO [dbo].[Produto]([Id],[Nome],[Descricao],[Categoria],[DataPublicacao],[Preco],[Ativo],[Desistencia],[EstadoProduto],[Curtida],[Quantidade],[CondominoId],[DataVenda]) VALUES(NEWID(),'Produto Legal','Descrição legal',0,GETDATE(),100,0,1,0,0,1,(SELECT TOP 1 id FROM Condomino),'2023-06-15 10:50:41.653')
INSERT INTO [dbo].[Produto]([Id],[Nome],[Descricao],[Categoria],[DataPublicacao],[Preco],[Ativo],[Desistencia],[EstadoProduto],[Curtida],[Quantidade],[CondominoId],[DataVenda]) VALUES(NEWID(),'Produto Legal','Descrição legal',0,GETDATE(),100,1,0,0,0,1,(SELECT TOP 1 id FROM Condomino),null)
INSERT INTO [dbo].[Produto]([Id],[Nome],[Descricao],[Categoria],[DataPublicacao],[Preco],[Ativo],[Desistencia],[EstadoProduto],[Curtida],[Quantidade],[CondominoId],[DataVenda]) VALUES(NEWID(),'Produto Legal','Descrição legal',0,GETDATE(),100,0,0,0,0,1,(SELECT TOP 1 id FROM Condomino),'2023-06-15 10:50:41.653')

GO


USE [Desapegando]
GO

INSERT INTO [dbo].[ProdutoImagem]([Id],[FileName],[ProdutoId])VALUES(NEWID(),'Imagem1',(select TOP 1 Id from Produto))
INSERT INTO [dbo].[ProdutoImagem]([Id],[FileName],[ProdutoId])VALUES(NEWID(),'Imagem2',(select TOP 1 Id from Produto))
INSERT INTO [dbo].[ProdutoImagem]([Id],[FileName],[ProdutoId])VALUES(NEWID(),'Imagem3',(select TOP 1 Id from Produto))
INSERT INTO [dbo].[ProdutoImagem]([Id],[FileName],[ProdutoId])VALUES(NEWID(),'Imagem4',(select TOP 1 Id from Produto))
INSERT INTO [dbo].[ProdutoImagem]([Id],[FileName],[ProdutoId])VALUES(NEWID(),'Imagem5',(select TOP 1 Id from Produto))
INSERT INTO [dbo].[ProdutoImagem]([Id],[FileName],[ProdutoId])VALUES(NEWID(),'Imagem6',(select TOP 1 Id from Produto))
INSERT INTO [dbo].[ProdutoImagem]([Id],[FileName],[ProdutoId])VALUES(NEWID(),'Imagem7',(select TOP 1 Id from Produto))
INSERT INTO [dbo].[ProdutoImagem]([Id],[FileName],[ProdutoId])VALUES(NEWID(),'Imagem8',(select TOP 1 Id from Produto))
INSERT INTO [dbo].[ProdutoImagem]([Id],[FileName],[ProdutoId])VALUES(NEWID(),'Imagem9',(select TOP 1 Id from Produto))
INSERT INTO [dbo].[ProdutoImagem]([Id],[FileName],[ProdutoId])VALUES(NEWID(),'Imagem10',(select TOP 1 Id from Produto))
INSERT INTO [dbo].[ProdutoImagem]([Id],[FileName],[ProdutoId])VALUES(NEWID(),'Imagem11',(select TOP 1 Id from Produto))
INSERT INTO [dbo].[ProdutoImagem]([Id],[FileName],[ProdutoId])VALUES(NEWID(),'Imagem12',(select TOP 1 Id from Produto))
INSERT INTO [dbo].[ProdutoImagem]([Id],[FileName],[ProdutoId])VALUES(NEWID(),'Imagem13',(select TOP 1 Id from Produto))
INSERT INTO [dbo].[ProdutoImagem]([Id],[FileName],[ProdutoId])VALUES(NEWID(),'Imagem14',(select TOP 1 Id from Produto))
INSERT INTO [dbo].[ProdutoImagem]([Id],[FileName],[ProdutoId])VALUES(NEWID(),'Imagem15',(select TOP 1 Id from Produto))
INSERT INTO [dbo].[ProdutoImagem]([Id],[FileName],[ProdutoId])VALUES(NEWID(),'Imagem16',(select TOP 1 Id from Produto))
INSERT INTO [dbo].[ProdutoImagem]([Id],[FileName],[ProdutoId])VALUES(NEWID(),'Imagem17',(select TOP 1 Id from Produto))
INSERT INTO [dbo].[ProdutoImagem]([Id],[FileName],[ProdutoId])VALUES(NEWID(),'Imagem18',(select TOP 1 Id from Produto))
INSERT INTO [dbo].[ProdutoImagem]([Id],[FileName],[ProdutoId])VALUES(NEWID(),'Imagem19',(select TOP 1 Id from Produto))
INSERT INTO [dbo].[ProdutoImagem]([Id],[FileName],[ProdutoId])VALUES(NEWID(),'Imagem20',(select TOP 1 Id from Produto))
INSERT INTO [dbo].[ProdutoImagem]([Id],[FileName],[ProdutoId])VALUES(NEWID(),'Imagem21',(select TOP 1 Id from Produto))
INSERT INTO [dbo].[ProdutoImagem]([Id],[FileName],[ProdutoId])VALUES(NEWID(),'Imagem22',(select TOP 1 Id from Produto))
INSERT INTO [dbo].[ProdutoImagem]([Id],[FileName],[ProdutoId])VALUES(NEWID(),'Imagem23',(select TOP 1 Id from Produto))
GO


Prós:

Salário bom.
Ambiente de trabalho agradável.
Colegas de trabalho solicítos.
Qualidade de vida.
Possibilidade de trabalho remoto.
Pacote de benefícios (VA, VR, Plano saúde, plano de previdência, PLR).
Possibilidade de melhorar o inglês, pois é necessário o contato com pessoas dos EUA e Índia. Existe possibilidade de aula de inglês.
Sem cobranças desnecessárias.

Contra:

45 horas semanais (sem necessidade).
PLR irrisório (menos de 2 salários, apesar de ser melhor que nada).
A comida no refeitório da empresa é terrível.
Reuniões 1:1 com o gestor perguntando sobre sua vida pessoal. O propósito dessa reunião é falar sobre o trabalho, não da minha vida e dos meus parentes. Outra coisa, muitos pontos irrelevantes discutidos nas 1:1, por exemplo: "Fulano na sua equipe me informou que você estava sem câmera aberta na reunião, pq isso? Sua resposta: pq o teams estava bugado". É um ponto tão irrelevante pra levar pra 1:1 que no fundo você acaba tendo uma ideia do que as outras pessoas acabam falando sobre você para o gestor, levando você a ter uma visão negativa sobre seu colega de equipe, pois ele pode estar tentando te queimar na empresa.
Alta rotatividade de desenvolvedores (abaixo os motivos).
Para desenvolvedores alguns pontos são importantes de destacar: 
	- Sistema legado e muito grande (mais de 180 projetos na solução; pouca documentação).
	- Tecnologia atrasada (.NET Framework).
	- Regra de negócio absurdamente complexa (o que dificulta muito o trabalho).
	- Área de tecnologia pouco estruturada, onde não existe divisão de tarefas por senioridade, pouca autonomia em relação ao projeto (qualquer mudança pode quebrar alguma funcionalidade e é difícil de testar).
	- Trabalhar em um único projeto, o que chega a ser entediante.
	- ZERO aprendizado em programação e tecnologia. Só se aprende a regra de negócio da empresa.
	- Épocas do ano em que não se faz absolutamente nada (mais de 1 mês sem fazer nada, o que gera bastante ansiedade pra mim, mas é possível realizar cursos online).
	- Período de Hardening (resolução de bugs) é normal o Dev arrumar bugs de sistemas que nunca teve contato prévio (nunca ter realizado alguma feature ou outro tipo de trabalho e apresentação do sistema).

Plano de carreira basicamente inexistente. Você realiza os PDI e bate as metas, mas tudo fica a critério do gestor e da sua avaliação discricionária. Existe algumas métricas, mas não tem como mensurá-las, então o gestor as inventa, ficando explícito nas suas falas: "eu acho...; na minha visão...; você poderia..., etc".
 
Qualquer ponto de feedback debatido com o gestor ele fica na defensiva sobre seu ponto de vista e é possível verificar certa confiança em somente algumas pessoas com mais tempo de casa.

Diferença gritante de VR entre funcionários remotos (ganham mais) e híbridos.

Tech Lead não é um cargo, somente uma função a mais para o DEV ou QA.

Pouca autonomia para os Tech Leads, tudo fica concentrado no gestor da área.

Diferença salarial muito grande entre pessoas da mesma senioridade e experiência.

No fundo uma pessoa contratada Senior faz as mesmas atividades que um Júnior, porém recebe muito mais por isso. (A maior dificuldade é a regra de negócio e não a programação em si, portanto o que um senior faz um júnior já consegue fazer).

Atualmente existe um time atualizando o projeto para novas versões do .NET, mas ainda tenho minhas dúvidas sobre a qualidade final, pois o projeto é muuuito grande e já deveria ter sido refeito há muito tempo e várias vezes.

Só recomendo a empresa para desenvolvedores REMOTOS que não queiram aprender mais nada sobre tecnologia, e queiram fazer carreira em uma empresa que proporciona qualidade de vida.

Empresa cada vez mais apertando o cerco para os funcionários.

Desvio de funções... Júnior realizando atividades de Tech Lead, Pleno e Senior. Mais um pouco o Júnior pega atividades do Diretor.

Empresa disponibiliza horário flexível de 1 hora, porém algumas reuniões são feitas nesse ínterim. O que não faz sentido nenhum.

