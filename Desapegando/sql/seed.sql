USE [Desapegando]
GO

INSERT INTO [dbo].[Condomino] ([Id],[Nome],[Sobrenome],[Sexo],[Telefone],[Email],[Cpf],[Apartamento],[Ativo],[DataNascimento],[DataRegistro],[ImageFileName]) VALUES (NEWID() ,'João','Alberto',0,'413335265','joao@gmail.com','36298533079','102',0,'1990-06-26',GETDATE(),'condomino1.jpg')
INSERT INTO [dbo].[Condomino] ([Id],[Nome],[Sobrenome],[Sexo],[Telefone],[Email],[Cpf],[Apartamento],[Ativo],[DataNascimento],[DataRegistro],[ImageFileName]) VALUES (NEWID() ,'Pedro','Alberto',0,'413335265','pedro@gmail.com','36298533079','102',0,'1990-06-26',GETDATE(),'condomino2.jpg')
INSERT INTO [dbo].[Condomino] ([Id],[Nome],[Sobrenome],[Sexo],[Telefone],[Email],[Cpf],[Apartamento],[Ativo],[DataNascimento],[DataRegistro],[ImageFileName]) VALUES (NEWID() ,'Carlos','Alberto',0,'413335265','carlos@gmail.com','36298533079','102',0,'1990-06-26',GETDATE(),'condomino3.jpg')
INSERT INTO [dbo].[Condomino] ([Id],[Nome],[Sobrenome],[Sexo],[Telefone],[Email],[Cpf],[Apartamento],[Ativo],[DataNascimento],[DataRegistro],[ImageFileName]) VALUES (NEWID() ,'Leonam','Alberto',0,'413335265','leonam@gmail.com','36298533079','102',0,'1990-06-26',GETDATE(),'condomino1.jpg')
INSERT INTO [dbo].[Condomino] ([Id],[Nome],[Sobrenome],[Sexo],[Telefone],[Email],[Cpf],[Apartamento],[Ativo],[DataNascimento],[DataRegistro],[ImageFileName]) VALUES (NEWID() ,'Vini','Alberto',0,'413335265','vini@gmail.com','36298533079','102',0,'1990-06-26',GETDATE(),'condomino2.jpg')
INSERT INTO [dbo].[Condomino] ([Id],[Nome],[Sobrenome],[Sexo],[Telefone],[Email],[Cpf],[Apartamento],[Ativo],[DataNascimento],[DataRegistro],[ImageFileName]) VALUES (NEWID() ,'Lucas','Alberto',0,'413335265','lucas@gmail.com','36298533079','102',0,'1990-06-26',GETDATE(),'condomino3.jpg')
INSERT INTO [dbo].[Condomino] ([Id],[Nome],[Sobrenome],[Sexo],[Telefone],[Email],[Cpf],[Apartamento],[Ativo],[DataNascimento],[DataRegistro],[ImageFileName]) VALUES (NEWID() ,'Fernando','Alberto',0,'413335265','fernando@gmail.com','36298533079','102',0,'1990-06-26',GETDATE(),'condomino1.jpg')
INSERT INTO [dbo].[Condomino] ([Id],[Nome],[Sobrenome],[Sexo],[Telefone],[Email],[Cpf],[Apartamento],[Ativo],[DataNascimento],[DataRegistro],[ImageFileName]) VALUES (NEWID() ,'Walter','Alberto',0,'413335265','walter@gmail.com','36298533079','102',0,'1990-06-26',GETDATE(),'condomino2.jpg')
INSERT INTO [dbo].[Condomino] ([Id],[Nome],[Sobrenome],[Sexo],[Telefone],[Email],[Cpf],[Apartamento],[Ativo],[DataNascimento],[DataRegistro],[ImageFileName]) VALUES (NEWID() ,'Marcos','Alberto',0,'413335265','marcos@gmail.com','36298533079','102',0,'1990-06-26',GETDATE(),'condomino3.jpg')
INSERT INTO [dbo].[Condomino] ([Id],[Nome],[Sobrenome],[Sexo],[Telefone],[Email],[Cpf],[Apartamento],[Ativo],[DataNascimento],[DataRegistro],[ImageFileName]) VALUES (NEWID() ,'Dagoberto','Alberto',0,'413335265','dagoberto@gmail.com','36298533079','102',0,'1990-06-26',GETDATE(),'condomino1.jpg')
INSERT INTO [dbo].[Condomino] ([Id],[Nome],[Sobrenome],[Sexo],[Telefone],[Email],[Cpf],[Apartamento],[Ativo],[DataNascimento],[DataRegistro],[ImageFileName]) VALUES (NEWID() ,'Ramiro','Alberto',0,'413335265','ramiro@gmail.com','36298533079','102',0,'1990-06-26',GETDATE(),'condomino2.jpg')
INSERT INTO [dbo].[Condomino] ([Id],[Nome],[Sobrenome],[Sexo],[Telefone],[Email],[Cpf],[Apartamento],[Ativo],[DataNascimento],[DataRegistro],[ImageFileName]) VALUES (NEWID() ,'Rui','Alberto',0,'413335265','rui@gmail.com','36298533079','102',0,'1990-06-26',GETDATE(),'condomino3.jpg')
INSERT INTO [dbo].[Condomino] ([Id],[Nome],[Sobrenome],[Sexo],[Telefone],[Email],[Cpf],[Apartamento],[Ativo],[DataNascimento],[DataRegistro],[ImageFileName]) VALUES (NEWID() ,'Ronaldo','Alberto',0,'413335265','ronaldo@gmail.com','36298533079','102',0,'1990-06-26',GETDATE(),'condomino1.jpg')

GO


USE [Desapegando]
GO

INSERT INTO [dbo].[Produto]([Id],[Nome],[Descricao],[Categoria],[DataPublicacao],[Preco],[Ativo],[Desistencia],[EstadoProduto],[Curtida],[Quantidade],[CondominoId],[DataDesistencia]) VALUES(NEWID(),'Produto Legal','Descrição legal',0,GETDATE(),100,0,1,0,0,1,(SELECT TOP 1 id FROM Condomino),'2023-06-15 10:50:41.653')
INSERT INTO [dbo].[Produto]([Id],[Nome],[Descricao],[Categoria],[DataPublicacao],[Preco],[Ativo],[Desistencia],[EstadoProduto],[Curtida],[Quantidade],[CondominoId],[DataDesistencia]) VALUES(NEWID(),'Produto Legal','Descrição legal',0,GETDATE(),100,1,0,0,0,1,(SELECT TOP 1 id FROM Condomino),null)
INSERT INTO [dbo].[Produto]([Id],[Nome],[Descricao],[Categoria],[DataPublicacao],[Preco],[Ativo],[Desistencia],[EstadoProduto],[Curtida],[Quantidade],[CondominoId],[DataDesistencia]) VALUES(NEWID(),'Produto Legal','Descrição legal',0,GETDATE(),100,0,0,0,0,1,(SELECT TOP 1 id FROM Condomino),'2023-06-15 10:50:41.653')
INSERT INTO [dbo].[Produto]([Id],[Nome],[Descricao],[Categoria],[DataPublicacao],[Preco],[Ativo],[Desistencia],[EstadoProduto],[Curtida],[Quantidade],[CondominoId],[DataDesistencia]) VALUES(NEWID(),'Produto Legal','Descrição legal',0,GETDATE(),100,1,0,0,0,1,(SELECT TOP 1 id FROM Condomino),null)
INSERT INTO [dbo].[Produto]([Id],[Nome],[Descricao],[Categoria],[DataPublicacao],[Preco],[Ativo],[Desistencia],[EstadoProduto],[Curtida],[Quantidade],[CondominoId],[DataDesistencia]) VALUES(NEWID(),'Produto Legal','Descrição legal',0,GETDATE(),100,0,1,0,0,1,(SELECT TOP 1 id FROM Condomino),'2023-06-15 10:50:41.653')
INSERT INTO [dbo].[Produto]([Id],[Nome],[Descricao],[Categoria],[DataPublicacao],[Preco],[Ativo],[Desistencia],[EstadoProduto],[Curtida],[Quantidade],[CondominoId],[DataDesistencia]) VALUES(NEWID(),'Produto Legal','Descrição legal',0,GETDATE(),100,1,0,0,0,1,(SELECT TOP 1 id FROM Condomino),null)
INSERT INTO [dbo].[Produto]([Id],[Nome],[Descricao],[Categoria],[DataPublicacao],[Preco],[Ativo],[Desistencia],[EstadoProduto],[Curtida],[Quantidade],[CondominoId],[DataDesistencia]) VALUES(NEWID(),'Produto Legal','Descrição legal',0,GETDATE(),100,0,0,0,0,1,(SELECT TOP 1 id FROM Condomino),'2023-06-15 10:50:41.653')
INSERT INTO [dbo].[Produto]([Id],[Nome],[Descricao],[Categoria],[DataPublicacao],[Preco],[Ativo],[Desistencia],[EstadoProduto],[Curtida],[Quantidade],[CondominoId],[DataDesistencia]) VALUES(NEWID(),'Produto Legal','Descrição legal',0,GETDATE(),100,1,0,0,0,1,(SELECT TOP 1 id FROM Condomino),null)
INSERT INTO [dbo].[Produto]([Id],[Nome],[Descricao],[Categoria],[DataPublicacao],[Preco],[Ativo],[Desistencia],[EstadoProduto],[Curtida],[Quantidade],[CondominoId],[DataDesistencia]) VALUES(NEWID(),'Produto Legal','Descrição legal',0,GETDATE(),100,0,1,0,0,1,(SELECT TOP 1 id FROM Condomino),'2023-06-15 10:50:41.653')
INSERT INTO [dbo].[Produto]([Id],[Nome],[Descricao],[Categoria],[DataPublicacao],[Preco],[Ativo],[Desistencia],[EstadoProduto],[Curtida],[Quantidade],[CondominoId],[DataDesistencia]) VALUES(NEWID(),'Produto Legal','Descrição legal',0,GETDATE(),100,1,0,0,0,1,(SELECT TOP 1 id FROM Condomino),null)
INSERT INTO [dbo].[Produto]([Id],[Nome],[Descricao],[Categoria],[DataPublicacao],[Preco],[Ativo],[Desistencia],[EstadoProduto],[Curtida],[Quantidade],[CondominoId],[DataDesistencia]) VALUES(NEWID(),'Produto Legal','Descrição legal',0,GETDATE(),100,0,0,0,0,1,(SELECT TOP 1 id FROM Condomino),'2023-06-15 10:50:41.653')
INSERT INTO [dbo].[Produto]([Id],[Nome],[Descricao],[Categoria],[DataPublicacao],[Preco],[Ativo],[Desistencia],[EstadoProduto],[Curtida],[Quantidade],[CondominoId],[DataDesistencia]) VALUES(NEWID(),'Produto Legal','Descrição legal',0,GETDATE(),100,1,0,0,0,1,(SELECT TOP 1 id FROM Condomino),null)
INSERT INTO [dbo].[Produto]([Id],[Nome],[Descricao],[Categoria],[DataPublicacao],[Preco],[Ativo],[Desistencia],[EstadoProduto],[Curtida],[Quantidade],[CondominoId],[DataDesistencia]) VALUES(NEWID(),'Produto Legal','Descrição legal',0,GETDATE(),100,0,1,0,0,1,(SELECT TOP 1 id FROM Condomino),'2023-06-15 10:50:41.653')
INSERT INTO [dbo].[Produto]([Id],[Nome],[Descricao],[Categoria],[DataPublicacao],[Preco],[Ativo],[Desistencia],[EstadoProduto],[Curtida],[Quantidade],[CondominoId],[DataDesistencia]) VALUES(NEWID(),'Produto Legal','Descrição legal',0,GETDATE(),100,1,0,0,0,1,(SELECT TOP 1 id FROM Condomino),null)
INSERT INTO [dbo].[Produto]([Id],[Nome],[Descricao],[Categoria],[DataPublicacao],[Preco],[Ativo],[Desistencia],[EstadoProduto],[Curtida],[Quantidade],[CondominoId],[DataDesistencia]) VALUES(NEWID(),'Produto Legal','Descrição legal',0,GETDATE(),100,0,0,0,0,1,(SELECT TOP 1 id FROM Condomino),'2023-06-15 10:50:41.653')
INSERT INTO [dbo].[Produto]([Id],[Nome],[Descricao],[Categoria],[DataPublicacao],[Preco],[Ativo],[Desistencia],[EstadoProduto],[Curtida],[Quantidade],[CondominoId],[DataDesistencia]) VALUES(NEWID(),'Produto Legal','Descrição legal',0,GETDATE(),100,1,0,0,0,1,(SELECT TOP 1 id FROM Condomino),null)
INSERT INTO [dbo].[Produto]([Id],[Nome],[Descricao],[Categoria],[DataPublicacao],[Preco],[Ativo],[Desistencia],[EstadoProduto],[Curtida],[Quantidade],[CondominoId],[DataDesistencia]) VALUES(NEWID(),'Produto Legal','Descrição legal',0,GETDATE(),100,0,1,0,0,1,(SELECT TOP 1 id FROM Condomino),'2023-06-15 10:50:41.653')
INSERT INTO [dbo].[Produto]([Id],[Nome],[Descricao],[Categoria],[DataPublicacao],[Preco],[Ativo],[Desistencia],[EstadoProduto],[Curtida],[Quantidade],[CondominoId],[DataDesistencia]) VALUES(NEWID(),'Produto Legal','Descrição legal',0,GETDATE(),100,1,0,0,0,1,(SELECT TOP 1 id FROM Condomino),null)
INSERT INTO [dbo].[Produto]([Id],[Nome],[Descricao],[Categoria],[DataPublicacao],[Preco],[Ativo],[Desistencia],[EstadoProduto],[Curtida],[Quantidade],[CondominoId],[DataDesistencia]) VALUES(NEWID(),'Produto Legal','Descrição legal',0,GETDATE(),100,0,0,0,0,1,(SELECT TOP 1 id FROM Condomino),'2023-06-15 10:50:41.653')
INSERT INTO [dbo].[Produto]([Id],[Nome],[Descricao],[Categoria],[DataPublicacao],[Preco],[Ativo],[Desistencia],[EstadoProduto],[Curtida],[Quantidade],[CondominoId],[DataDesistencia]) VALUES(NEWID(),'Produto Legal','Descrição legal',0,GETDATE(),100,1,0,0,0,1,(SELECT TOP 1 id FROM Condomino),null)
INSERT INTO [dbo].[Produto]([Id],[Nome],[Descricao],[Categoria],[DataPublicacao],[Preco],[Ativo],[Desistencia],[EstadoProduto],[Curtida],[Quantidade],[CondominoId],[DataDesistencia]) VALUES(NEWID(),'Produto Legal','Descrição legal',0,GETDATE(),100,0,1,0,0,1,(SELECT TOP 1 id FROM Condomino),'2023-06-15 10:50:41.653')
INSERT INTO [dbo].[Produto]([Id],[Nome],[Descricao],[Categoria],[DataPublicacao],[Preco],[Ativo],[Desistencia],[EstadoProduto],[Curtida],[Quantidade],[CondominoId],[DataDesistencia]) VALUES(NEWID(),'Produto Legal','Descrição legal',0,GETDATE(),100,1,0,0,0,1,(SELECT TOP 1 id FROM Condomino),null)
INSERT INTO [dbo].[Produto]([Id],[Nome],[Descricao],[Categoria],[DataPublicacao],[Preco],[Ativo],[Desistencia],[EstadoProduto],[Curtida],[Quantidade],[CondominoId],[DataDesistencia]) VALUES(NEWID(),'Produto Legal','Descrição legal',0,GETDATE(),100,0,0,0,0,1,(SELECT TOP 1 id FROM Condomino),'2023-06-15 10:50:41.653')

GO


USE [Desapegando]
GO

INSERT INTO [dbo].[ProdutoImagem]([Id],[FileName],[ProdutoId])VALUES(NEWID(),'produto1.jpg',(SELECT [Id] FROM Produto ORDER BY Id OFFSET 0 ROW FETCH NEXT 1 ROW ONLY))
INSERT INTO [dbo].[ProdutoImagem]([Id],[FileName],[ProdutoId])VALUES(NEWID(),'produto10.jpg',(SELECT [Id] FROM Produto ORDER BY Id OFFSET 1 ROW FETCH NEXT 1 ROW ONLY))
INSERT INTO [dbo].[ProdutoImagem]([Id],[FileName],[ProdutoId])VALUES(NEWID(),'produto2.jpg',(SELECT [Id] FROM Produto ORDER BY Id OFFSET 2 ROW FETCH NEXT 1 ROW ONLY))
INSERT INTO [dbo].[ProdutoImagem]([Id],[FileName],[ProdutoId])VALUES(NEWID(),'produto3.jpg',(SELECT [Id] FROM Produto ORDER BY Id OFFSET 3 ROW FETCH NEXT 1 ROW ONLY))
INSERT INTO [dbo].[ProdutoImagem]([Id],[FileName],[ProdutoId])VALUES(NEWID(),'produto4.jpg',(SELECT [Id] FROM Produto ORDER BY Id OFFSET 4 ROW FETCH NEXT 1 ROW ONLY))
INSERT INTO [dbo].[ProdutoImagem]([Id],[FileName],[ProdutoId])VALUES(NEWID(),'produto5.jpg',(SELECT [Id] FROM Produto ORDER BY Id OFFSET 5 ROW FETCH NEXT 1 ROW ONLY))
INSERT INTO [dbo].[ProdutoImagem]([Id],[FileName],[ProdutoId])VALUES(NEWID(),'produto6.jpg',(SELECT [Id] FROM Produto ORDER BY Id OFFSET 6 ROW FETCH NEXT 1 ROW ONLY))
INSERT INTO [dbo].[ProdutoImagem]([Id],[FileName],[ProdutoId])VALUES(NEWID(),'produto7.jpg',(SELECT [Id] FROM Produto ORDER BY Id OFFSET 7 ROW FETCH NEXT 1 ROW ONLY))
INSERT INTO [dbo].[ProdutoImagem]([Id],[FileName],[ProdutoId])VALUES(NEWID(),'produto8.jpg',(SELECT [Id] FROM Produto ORDER BY Id OFFSET 8 ROW FETCH NEXT 1 ROW ONLY))
INSERT INTO [dbo].[ProdutoImagem]([Id],[FileName],[ProdutoId])VALUES(NEWID(),'produto9.jpg',(SELECT [Id] FROM Produto ORDER BY Id OFFSET 9 ROW FETCH NEXT 1 ROW ONLY))
INSERT INTO [dbo].[ProdutoImagem]([Id],[FileName],[ProdutoId])VALUES(NEWID(),'produto10.jpg',(SELECT [Id] FROM Produto ORDER BY Id OFFSET 10 ROW FETCH NEXT 1 ROW ONLY))
INSERT INTO [dbo].[ProdutoImagem]([Id],[FileName],[ProdutoId])VALUES(NEWID(),'produto11.jpg',(SELECT [Id] FROM Produto ORDER BY Id OFFSET 11 ROW FETCH NEXT 1 ROW ONLY))
INSERT INTO [dbo].[ProdutoImagem]([Id],[FileName],[ProdutoId])VALUES(NEWID(),'produto12.jpg',(SELECT [Id] FROM Produto ORDER BY Id OFFSET 12 ROW FETCH NEXT 1 ROW ONLY))
INSERT INTO [dbo].[ProdutoImagem]([Id],[FileName],[ProdutoId])VALUES(NEWID(),'produto1.jpg',(SELECT [Id] FROM Produto ORDER BY Id OFFSET 13 ROW FETCH NEXT 1 ROW ONLY))
INSERT INTO [dbo].[ProdutoImagem]([Id],[FileName],[ProdutoId])VALUES(NEWID(),'produto2.jpg',(SELECT [Id] FROM Produto ORDER BY Id OFFSET 14 ROW FETCH NEXT 1 ROW ONLY))
INSERT INTO [dbo].[ProdutoImagem]([Id],[FileName],[ProdutoId])VALUES(NEWID(),'produto3.jpg',(SELECT [Id] FROM Produto ORDER BY Id OFFSET 15 ROW FETCH NEXT 1 ROW ONLY))
INSERT INTO [dbo].[ProdutoImagem]([Id],[FileName],[ProdutoId])VALUES(NEWID(),'produto4.jpg',(SELECT [Id] FROM Produto ORDER BY Id OFFSET 16 ROW FETCH NEXT 1 ROW ONLY))
INSERT INTO [dbo].[ProdutoImagem]([Id],[FileName],[ProdutoId])VALUES(NEWID(),'produto5.jpg',(SELECT [Id] FROM Produto ORDER BY Id OFFSET 17 ROW FETCH NEXT 1 ROW ONLY))
INSERT INTO [dbo].[ProdutoImagem]([Id],[FileName],[ProdutoId])VALUES(NEWID(),'produto6.jpg',(SELECT [Id] FROM Produto ORDER BY Id OFFSET 18 ROW FETCH NEXT 1 ROW ONLY))
INSERT INTO [dbo].[ProdutoImagem]([Id],[FileName],[ProdutoId])VALUES(NEWID(),'produto7.jpg',(SELECT [Id] FROM Produto ORDER BY Id OFFSET 19 ROW FETCH NEXT 1 ROW ONLY))
INSERT INTO [dbo].[ProdutoImagem]([Id],[FileName],[ProdutoId])VALUES(NEWID(),'produto8.jpg',(SELECT [Id] FROM Produto ORDER BY Id OFFSET 20 ROW FETCH NEXT 1 ROW ONLY))
INSERT INTO [dbo].[ProdutoImagem]([Id],[FileName],[ProdutoId])VALUES(NEWID(),'produto9.jpg',(SELECT [Id] FROM Produto ORDER BY Id OFFSET 21 ROW FETCH NEXT 1 ROW ONLY))
INSERT INTO [dbo].[ProdutoImagem]([Id],[FileName],[ProdutoId])VALUES(NEWID(),'produto10.jpg',(SELECT [Id] FROM Produto ORDER BY Id OFFSET 22 ROW FETCH NEXT 1 ROW ONLY))
INSERT INTO [dbo].[ProdutoImagem]([Id],[FileName],[ProdutoId])VALUES(NEWID(),'produto11.jpg',(SELECT [Id] FROM Produto ORDER BY Id OFFSET 23 ROW FETCH NEXT 1 ROW ONLY))
GO

USE [Desapegando]
GO


DECLARE @RandomDays INT;
DECLARE @RandomDateTime DATETIME;

-- Gerar um número aleatório entre 0 e 89
SET @RandomDays = ABS(CHECKSUM(NEWID())) % 90;

-- Calcular a data e hora aleatória
SET @RandomDateTime = DATEADD(DAY, -@RandomDays, GETDATE());
INSERT INTO [dbo].[Compras]([Id],[ProdutoId],[CondominoId],[DataVenda])VALUES(NEWID(),(SELECT [Id] FROM Produto ORDER BY Id OFFSET 1 ROW FETCH NEXT 1 ROW ONLY),(SELECT [Id] FROM Condomino ORDER BY Id OFFSET 1 ROW FETCH NEXT 1 ROW ONLY),@RandomDateTime)
-- Gerar um número aleatório entre 0 e 89
SET @RandomDays = ABS(CHECKSUM(NEWID())) % 90;

-- Calcular a data e hora aleatória
SET @RandomDateTime = DATEADD(DAY, -@RandomDays, GETDATE());
INSERT INTO [dbo].[Compras]([Id],[ProdutoId],[CondominoId],[DataVenda])VALUES(NEWID(),(SELECT [Id] FROM Produto ORDER BY Id OFFSET 2 ROW FETCH NEXT 1 ROW ONLY),(SELECT [Id] FROM Condomino ORDER BY Id OFFSET 2 ROW FETCH NEXT 1 ROW ONLY),@RandomDateTime)
-- Gerar um número aleatório entre 0 e 89
SET @RandomDays = ABS(CHECKSUM(NEWID())) % 90;

-- Calcular a data e hora aleatória
SET @RandomDateTime = DATEADD(DAY, -@RandomDays, GETDATE());
INSERT INTO [dbo].[Compras]([Id],[ProdutoId],[CondominoId],[DataVenda])VALUES(NEWID(),(SELECT [Id] FROM Produto ORDER BY Id OFFSET 3 ROW FETCH NEXT 1 ROW ONLY),(SELECT [Id] FROM Condomino ORDER BY Id OFFSET 3 ROW FETCH NEXT 1 ROW ONLY),@RandomDateTime)
-- Gerar um número aleatório entre 0 e 89
SET @RandomDays = ABS(CHECKSUM(NEWID())) % 90;

-- Calcular a data e hora aleatória
SET @RandomDateTime = DATEADD(DAY, -@RandomDays, GETDATE());
INSERT INTO [dbo].[Compras]([Id],[ProdutoId],[CondominoId],[DataVenda])VALUES(NEWID(),(SELECT [Id] FROM Produto ORDER BY Id OFFSET 4 ROW FETCH NEXT 1 ROW ONLY),(SELECT [Id] FROM Condomino ORDER BY Id OFFSET 4 ROW FETCH NEXT 1 ROW ONLY),@RandomDateTime)
-- Gerar um número aleatório entre 0 e 89
SET @RandomDays = ABS(CHECKSUM(NEWID())) % 90;

-- Calcular a data e hora aleatória
SET @RandomDateTime = DATEADD(DAY, -@RandomDays, GETDATE());
INSERT INTO [dbo].[Compras]([Id],[ProdutoId],[CondominoId],[DataVenda])VALUES(NEWID(),(SELECT [Id] FROM Produto ORDER BY Id OFFSET 5 ROW FETCH NEXT 1 ROW ONLY),(SELECT [Id] FROM Condomino ORDER BY Id OFFSET 5 ROW FETCH NEXT 1 ROW ONLY),@RandomDateTime)
-- Gerar um número aleatório entre 0 e 89
SET @RandomDays = ABS(CHECKSUM(NEWID())) % 90;

-- Calcular a data e hora aleatória
SET @RandomDateTime = DATEADD(DAY, -@RandomDays, GETDATE());
INSERT INTO [dbo].[Compras]([Id],[ProdutoId],[CondominoId],[DataVenda])VALUES(NEWID(),(SELECT [Id] FROM Produto ORDER BY Id OFFSET 6 ROW FETCH NEXT 1 ROW ONLY),(SELECT [Id] FROM Condomino ORDER BY Id OFFSET 6 ROW FETCH NEXT 1 ROW ONLY),@RandomDateTime)
-- Gerar um número aleatório entre 0 e 89
SET @RandomDays = ABS(CHECKSUM(NEWID())) % 90;

-- Calcular a data e hora aleatória
SET @RandomDateTime = DATEADD(DAY, -@RandomDays, GETDATE());
INSERT INTO [dbo].[Compras]([Id],[ProdutoId],[CondominoId],[DataVenda])VALUES(NEWID(),(SELECT [Id] FROM Produto ORDER BY Id OFFSET 7 ROW FETCH NEXT 1 ROW ONLY),(SELECT [Id] FROM Condomino ORDER BY Id OFFSET 7 ROW FETCH NEXT 1 ROW ONLY),@RandomDateTime)
-- Gerar um número aleatório entre 0 e 89
SET @RandomDays = ABS(CHECKSUM(NEWID())) % 90;

-- Calcular a data e hora aleatória
SET @RandomDateTime = DATEADD(DAY, -@RandomDays, GETDATE());
INSERT INTO [dbo].[Compras]([Id],[ProdutoId],[CondominoId],[DataVenda])VALUES(NEWID(),(SELECT [Id] FROM Produto ORDER BY Id OFFSET 8 ROW FETCH NEXT 1 ROW ONLY),(SELECT [Id] FROM Condomino ORDER BY Id OFFSET 8 ROW FETCH NEXT 1 ROW ONLY),@RandomDateTime)
-- Gerar um número aleatório entre 0 e 89
SET @RandomDays = ABS(CHECKSUM(NEWID())) % 90;

-- Calcular a data e hora aleatória
SET @RandomDateTime = DATEADD(DAY, -@RandomDays, GETDATE());
INSERT INTO [dbo].[Compras]([Id],[ProdutoId],[CondominoId],[DataVenda])VALUES(NEWID(),(SELECT [Id] FROM Produto ORDER BY Id OFFSET 9 ROW FETCH NEXT 1 ROW ONLY),(SELECT [Id] FROM Condomino ORDER BY Id OFFSET 9 ROW FETCH NEXT 1 ROW ONLY),@RandomDateTime)
-- Gerar um número aleatório entre 0 e 89
SET @RandomDays = ABS(CHECKSUM(NEWID())) % 90;

-- Calcular a data e hora aleatória
SET @RandomDateTime = DATEADD(DAY, -@RandomDays, GETDATE());
INSERT INTO [dbo].[Compras]([Id],[ProdutoId],[CondominoId],[DataVenda])VALUES(NEWID(),(SELECT [Id] FROM Produto ORDER BY Id OFFSET 10 ROW FETCH NEXT 1 ROW ONLY),(SELECT [Id] FROM Condomino ORDER BY Id OFFSET 10 ROW FETCH NEXT 1 ROW ONLY),@RandomDateTime)
-- Gerar um número aleatório entre 0 e 89
SET @RandomDays = ABS(CHECKSUM(NEWID())) % 90;

-- Calcular a data e hora aleatória
SET @RandomDateTime = DATEADD(DAY, -@RandomDays, GETDATE());
INSERT INTO [dbo].[Compras]([Id],[ProdutoId],[CondominoId],[DataVenda])VALUES(NEWID(),(SELECT [Id] FROM Produto ORDER BY Id OFFSET 11 ROW FETCH NEXT 1 ROW ONLY),(SELECT [Id] FROM Condomino ORDER BY Id OFFSET 11 ROW FETCH NEXT 1 ROW ONLY),@RandomDateTime)
-- Gerar um número aleatório entre 0 e 89
SET @RandomDays = ABS(CHECKSUM(NEWID())) % 90;

-- Calcular a data e hora aleatória
SET @RandomDateTime = DATEADD(DAY, -@RandomDays, GETDATE());
INSERT INTO [dbo].[Compras]([Id],[ProdutoId],[CondominoId],[DataVenda])VALUES(NEWID(),(SELECT [Id] FROM Produto ORDER BY Id OFFSET 12 ROW FETCH NEXT 1 ROW ONLY),(SELECT [Id] FROM Condomino ORDER BY Id OFFSET 12 ROW FETCH NEXT 1 ROW ONLY),@RandomDateTime)
-- Gerar um número aleatório entre 0 e 89
SET @RandomDays = ABS(CHECKSUM(NEWID())) % 90;

-- Calcular a data e hora aleatória
SET @RandomDateTime = DATEADD(DAY, -@RandomDays, GETDATE());
INSERT INTO [dbo].[Compras]([Id],[ProdutoId],[CondominoId],[DataVenda])VALUES(NEWID(),(SELECT [Id] FROM Produto ORDER BY Id OFFSET 13 ROW FETCH NEXT 1 ROW ONLY),(SELECT [Id] FROM Condomino ORDER BY Id OFFSET 13 ROW FETCH NEXT 1 ROW ONLY),@RandomDateTime)
-- Gerar um número aleatório entre 0 e 89
SET @RandomDays = ABS(CHECKSUM(NEWID())) % 90;

-- Calcular a data e hora aleatória
SET @RandomDateTime = DATEADD(DAY, -@RandomDays, GETDATE());
INSERT INTO [dbo].[Compras]([Id],[ProdutoId],[CondominoId],[DataVenda])VALUES(NEWID(),(SELECT [Id] FROM Produto ORDER BY Id OFFSET 14 ROW FETCH NEXT 1 ROW ONLY),(SELECT [Id] FROM Condomino ORDER BY Id OFFSET 14 ROW FETCH NEXT 1 ROW ONLY),@RandomDateTime)
-- Gerar um número aleatório entre 0 e 89
SET @RandomDays = ABS(CHECKSUM(NEWID())) % 90;

-- Calcular a data e hora aleatória
SET @RandomDateTime = DATEADD(DAY, -@RandomDays, GETDATE());
INSERT INTO [dbo].[Compras]([Id],[ProdutoId],[CondominoId],[DataVenda])VALUES(NEWID(),(SELECT [Id] FROM Produto ORDER BY Id OFFSET 15 ROW FETCH NEXT 1 ROW ONLY),(SELECT [Id] FROM Condomino ORDER BY Id OFFSET 15 ROW FETCH NEXT 1 ROW ONLY),@RandomDateTime)
-- Gerar um número aleatório entre 0 e 89
SET @RandomDays = ABS(CHECKSUM(NEWID())) % 90;

-- Calcular a data e hora aleatória
SET @RandomDateTime = DATEADD(DAY, -@RandomDays, GETDATE());
INSERT INTO [dbo].[Compras]([Id],[ProdutoId],[CondominoId],[DataVenda])VALUES(NEWID(),(SELECT [Id] FROM Produto ORDER BY Id OFFSET 16 ROW FETCH NEXT 1 ROW ONLY),(SELECT [Id] FROM Condomino ORDER BY Id OFFSET 16 ROW FETCH NEXT 1 ROW ONLY),@RandomDateTime)
-- Gerar um número aleatório entre 0 e 89
SET @RandomDays = ABS(CHECKSUM(NEWID())) % 90;

-- Calcular a data e hora aleatória
SET @RandomDateTime = DATEADD(DAY, -@RandomDays, GETDATE());
INSERT INTO [dbo].[Compras]([Id],[ProdutoId],[CondominoId],[DataVenda])VALUES(NEWID(),(SELECT [Id] FROM Produto ORDER BY Id OFFSET 17 ROW FETCH NEXT 1 ROW ONLY),(SELECT [Id] FROM Condomino ORDER BY Id OFFSET 17 ROW FETCH NEXT 1 ROW ONLY),@RandomDateTime)
-- Gerar um número aleatório entre 0 e 89
SET @RandomDays = ABS(CHECKSUM(NEWID())) % 90;

-- Calcular a data e hora aleatória
SET @RandomDateTime = DATEADD(DAY, -@RandomDays, GETDATE());
INSERT INTO [dbo].[Compras]([Id],[ProdutoId],[CondominoId],[DataVenda])VALUES(NEWID(),(SELECT [Id] FROM Produto ORDER BY Id OFFSET 18 ROW FETCH NEXT 1 ROW ONLY),(SELECT [Id] FROM Condomino ORDER BY Id OFFSET 18 ROW FETCH NEXT 1 ROW ONLY),@RandomDateTime)

GO



USE [Desapegando]
GO

INSERT INTO [dbo].[Campanha]([Id],[Nome],[NomeInstituicao],[Descricao],[NomeResponsavel],[EmailResponsavel],[TelefoneResponsavel],[LocalDeEncontro],[DataInicio],[DataFinal],[Ativo],[CondominoId])VALUES(NEWID(),'Doação 1','Instituição 1','Doação Legal','João Carlos','joao.carlos@gmail.com','41999963254','Térreo',GETDATE(),(SELECT DATEADD(DAY, 7, GETDATE())),0,(SELECT [Id] FROM Condomino ORDER BY Id OFFSET 1 ROW FETCH NEXT 1 ROW ONLY))
INSERT INTO [dbo].[Campanha]([Id],[Nome],[NomeInstituicao],[Descricao],[NomeResponsavel],[EmailResponsavel],[TelefoneResponsavel],[LocalDeEncontro],[DataInicio],[DataFinal],[Ativo],[CondominoId])VALUES(NEWID(),'Doação 1','Instituição 1','Doação Legal','João Carlos','joao.carlos@gmail.com','41999963254','Térreo',GETDATE(),(SELECT DATEADD(DAY, 7, GETDATE())),0,(SELECT [Id] FROM Condomino ORDER BY Id OFFSET 2 ROW FETCH NEXT 1 ROW ONLY))
INSERT INTO [dbo].[Campanha]([Id],[Nome],[NomeInstituicao],[Descricao],[NomeResponsavel],[EmailResponsavel],[TelefoneResponsavel],[LocalDeEncontro],[DataInicio],[DataFinal],[Ativo],[CondominoId])VALUES(NEWID(),'Doação 1','Instituição 1','Doação Legal','João Carlos','joao.carlos@gmail.com','41999963254','Térreo',GETDATE(),(SELECT DATEADD(DAY, 7, GETDATE())),0,(SELECT [Id] FROM Condomino ORDER BY Id OFFSET 3 ROW FETCH NEXT 1 ROW ONLY))
INSERT INTO [dbo].[Campanha]([Id],[Nome],[NomeInstituicao],[Descricao],[NomeResponsavel],[EmailResponsavel],[TelefoneResponsavel],[LocalDeEncontro],[DataInicio],[DataFinal],[Ativo],[CondominoId])VALUES(NEWID(),'Doação 1','Instituição 1','Doação Legal','João Carlos','joao.carlos@gmail.com','41999963254','Térreo',GETDATE(),(SELECT DATEADD(DAY, 7, GETDATE())),0,(SELECT [Id] FROM Condomino ORDER BY Id OFFSET 4 ROW FETCH NEXT 1 ROW ONLY))
INSERT INTO [dbo].[Campanha]([Id],[Nome],[NomeInstituicao],[Descricao],[NomeResponsavel],[EmailResponsavel],[TelefoneResponsavel],[LocalDeEncontro],[DataInicio],[DataFinal],[Ativo],[CondominoId])VALUES(NEWID(),'Doação 1','Instituição 1','Doação Legal','João Carlos','joao.carlos@gmail.com','41999963254','Térreo',GETDATE(),(SELECT DATEADD(DAY, 7, GETDATE())),0,(SELECT [Id] FROM Condomino ORDER BY Id OFFSET 5 ROW FETCH NEXT 1 ROW ONLY))
INSERT INTO [dbo].[Campanha]([Id],[Nome],[NomeInstituicao],[Descricao],[NomeResponsavel],[EmailResponsavel],[TelefoneResponsavel],[LocalDeEncontro],[DataInicio],[DataFinal],[Ativo],[CondominoId])VALUES(NEWID(),'Doação 1','Instituição 1','Doação Legal','João Carlos','joao.carlos@gmail.com','41999963254','Térreo',GETDATE(),(SELECT DATEADD(DAY, 7, GETDATE())),0,(SELECT [Id] FROM Condomino ORDER BY Id OFFSET 6 ROW FETCH NEXT 1 ROW ONLY))
INSERT INTO [dbo].[Campanha]([Id],[Nome],[NomeInstituicao],[Descricao],[NomeResponsavel],[EmailResponsavel],[TelefoneResponsavel],[LocalDeEncontro],[DataInicio],[DataFinal],[Ativo],[CondominoId])VALUES(NEWID(),'Doação 1','Instituição 1','Doação Legal','João Carlos','joao.carlos@gmail.com','41999963254','Térreo',GETDATE(),(SELECT DATEADD(DAY, 7, GETDATE())),0,(SELECT [Id] FROM Condomino ORDER BY Id OFFSET 7 ROW FETCH NEXT 1 ROW ONLY))
INSERT INTO [dbo].[Campanha]([Id],[Nome],[NomeInstituicao],[Descricao],[NomeResponsavel],[EmailResponsavel],[TelefoneResponsavel],[LocalDeEncontro],[DataInicio],[DataFinal],[Ativo],[CondominoId])VALUES(NEWID(),'Doação 1','Instituição 1','Doação Legal','João Carlos','joao.carlos@gmail.com','41999963254','Térreo',GETDATE(),(SELECT DATEADD(DAY, 7, GETDATE())),0,(SELECT [Id] FROM Condomino ORDER BY Id OFFSET 1 ROW FETCH NEXT 1 ROW ONLY))
INSERT INTO [dbo].[Campanha]([Id],[Nome],[NomeInstituicao],[Descricao],[NomeResponsavel],[EmailResponsavel],[TelefoneResponsavel],[LocalDeEncontro],[DataInicio],[DataFinal],[Ativo],[CondominoId])VALUES(NEWID(),'Doação 1','Instituição 1','Doação Legal','João Carlos','joao.carlos@gmail.com','41999963254','Térreo',GETDATE(),(SELECT DATEADD(DAY, 7, GETDATE())),0,(SELECT [Id] FROM Condomino ORDER BY Id OFFSET 2 ROW FETCH NEXT 1 ROW ONLY))
INSERT INTO [dbo].[Campanha]([Id],[Nome],[NomeInstituicao],[Descricao],[NomeResponsavel],[EmailResponsavel],[TelefoneResponsavel],[LocalDeEncontro],[DataInicio],[DataFinal],[Ativo],[CondominoId])VALUES(NEWID(),'Doação 1','Instituição 1','Doação Legal','João Carlos','joao.carlos@gmail.com','41999963254','Térreo',GETDATE(),(SELECT DATEADD(DAY, 7, GETDATE())),0,(SELECT [Id] FROM Condomino ORDER BY Id OFFSET 3 ROW FETCH NEXT 1 ROW ONLY))
INSERT INTO [dbo].[Campanha]([Id],[Nome],[NomeInstituicao],[Descricao],[NomeResponsavel],[EmailResponsavel],[TelefoneResponsavel],[LocalDeEncontro],[DataInicio],[DataFinal],[Ativo],[CondominoId])VALUES(NEWID(),'Doação 1','Instituição 1','Doação Legal','João Carlos','joao.carlos@gmail.com','41999963254','Térreo',GETDATE(),(SELECT DATEADD(DAY, 7, GETDATE())),0,(SELECT [Id] FROM Condomino ORDER BY Id OFFSET 4 ROW FETCH NEXT 1 ROW ONLY))
INSERT INTO [dbo].[Campanha]([Id],[Nome],[NomeInstituicao],[Descricao],[NomeResponsavel],[EmailResponsavel],[TelefoneResponsavel],[LocalDeEncontro],[DataInicio],[DataFinal],[Ativo],[CondominoId])VALUES(NEWID(),'Doação 1','Instituição 1','Doação Legal','João Carlos','joao.carlos@gmail.com','41999963254','Térreo',GETDATE(),(SELECT DATEADD(DAY, 7, GETDATE())),0,(SELECT [Id] FROM Condomino ORDER BY Id OFFSET 5 ROW FETCH NEXT 1 ROW ONLY))
INSERT INTO [dbo].[Campanha]([Id],[Nome],[NomeInstituicao],[Descricao],[NomeResponsavel],[EmailResponsavel],[TelefoneResponsavel],[LocalDeEncontro],[DataInicio],[DataFinal],[Ativo],[CondominoId])VALUES(NEWID(),'Doação 1','Instituição 1','Doação Legal','João Carlos','joao.carlos@gmail.com','41999963254','Térreo',GETDATE(),(SELECT DATEADD(DAY, 7, GETDATE())),0,(SELECT [Id] FROM Condomino ORDER BY Id OFFSET 6 ROW FETCH NEXT 1 ROW ONLY))
INSERT INTO [dbo].[Campanha]([Id],[Nome],[NomeInstituicao],[Descricao],[NomeResponsavel],[EmailResponsavel],[TelefoneResponsavel],[LocalDeEncontro],[DataInicio],[DataFinal],[Ativo],[CondominoId])VALUES(NEWID(),'Doação 1','Instituição 1','Doação Legal','João Carlos','joao.carlos@gmail.com','41999963254','Térreo',GETDATE(),(SELECT DATEADD(DAY, 7, GETDATE())),0,(SELECT [Id] FROM Condomino ORDER BY Id OFFSET 7 ROW FETCH NEXT 1 ROW ONLY))
INSERT INTO [dbo].[Campanha]([Id],[Nome],[NomeInstituicao],[Descricao],[NomeResponsavel],[EmailResponsavel],[TelefoneResponsavel],[LocalDeEncontro],[DataInicio],[DataFinal],[Ativo],[CondominoId])VALUES(NEWID(),'Doação 1','Instituição 1','Doação Legal','João Carlos','joao.carlos@gmail.com','41999963254','Térreo',GETDATE(),(SELECT DATEADD(DAY, 7, GETDATE())),0,(SELECT [Id] FROM Condomino ORDER BY Id OFFSET 1 ROW FETCH NEXT 1 ROW ONLY))
INSERT INTO [dbo].[Campanha]([Id],[Nome],[NomeInstituicao],[Descricao],[NomeResponsavel],[EmailResponsavel],[TelefoneResponsavel],[LocalDeEncontro],[DataInicio],[DataFinal],[Ativo],[CondominoId])VALUES(NEWID(),'Doação 1','Instituição 1','Doação Legal','João Carlos','joao.carlos@gmail.com','41999963254','Térreo',GETDATE(),(SELECT DATEADD(DAY, 7, GETDATE())),0,(SELECT [Id] FROM Condomino ORDER BY Id OFFSET 2 ROW FETCH NEXT 1 ROW ONLY))
GO



USE [Desapegando]
GO

INSERT INTO [dbo].[CampanhaImagem]([Id],[FileName],[CampanhaId])VALUES(NEWID(),'campanha1.png',(SELECT [Id] FROM Campanha ORDER BY Id OFFSET 0 ROW FETCH NEXT 1 ROW ONLY))
INSERT INTO [dbo].[CampanhaImagem]([Id],[FileName],[CampanhaId])VALUES(NEWID(),'campanha2.jpg',(SELECT [Id] FROM Campanha ORDER BY Id OFFSET 1 ROW FETCH NEXT 1 ROW ONLY))
INSERT INTO [dbo].[CampanhaImagem]([Id],[FileName],[CampanhaId])VALUES(NEWID(),'campanha1.png',(SELECT [Id] FROM Campanha ORDER BY Id OFFSET 2 ROW FETCH NEXT 1 ROW ONLY))
INSERT INTO [dbo].[CampanhaImagem]([Id],[FileName],[CampanhaId])VALUES(NEWID(),'campanha2.jpg',(SELECT [Id] FROM Campanha ORDER BY Id OFFSET 3 ROW FETCH NEXT 1 ROW ONLY))
INSERT INTO [dbo].[CampanhaImagem]([Id],[FileName],[CampanhaId])VALUES(NEWID(),'campanha1.png',(SELECT [Id] FROM Campanha ORDER BY Id OFFSET 4 ROW FETCH NEXT 1 ROW ONLY))
INSERT INTO [dbo].[CampanhaImagem]([Id],[FileName],[CampanhaId])VALUES(NEWID(),'campanha2.jpg',(SELECT [Id] FROM Campanha ORDER BY Id OFFSET 5 ROW FETCH NEXT 1 ROW ONLY))
INSERT INTO [dbo].[CampanhaImagem]([Id],[FileName],[CampanhaId])VALUES(NEWID(),'campanha1.png',(SELECT [Id] FROM Campanha ORDER BY Id OFFSET 6 ROW FETCH NEXT 1 ROW ONLY))
INSERT INTO [dbo].[CampanhaImagem]([Id],[FileName],[CampanhaId])VALUES(NEWID(),'campanha2.jpg',(SELECT [Id] FROM Campanha ORDER BY Id OFFSET 7 ROW FETCH NEXT 1 ROW ONLY))
INSERT INTO [dbo].[CampanhaImagem]([Id],[FileName],[CampanhaId])VALUES(NEWID(),'campanha1.png',(SELECT [Id] FROM Campanha ORDER BY Id OFFSET 8 ROW FETCH NEXT 1 ROW ONLY))
INSERT INTO [dbo].[CampanhaImagem]([Id],[FileName],[CampanhaId])VALUES(NEWID(),'campanha2.jpg',(SELECT [Id] FROM Campanha ORDER BY Id OFFSET 9 ROW FETCH NEXT 1 ROW ONLY))
INSERT INTO [dbo].[CampanhaImagem]([Id],[FileName],[CampanhaId])VALUES(NEWID(),'campanha1.png',(SELECT [Id] FROM Campanha ORDER BY Id OFFSET 10 ROW FETCH NEXT 1 ROW ONLY))
INSERT INTO [dbo].[CampanhaImagem]([Id],[FileName],[CampanhaId])VALUES(NEWID(),'campanha2.jpg',(SELECT [Id] FROM Campanha ORDER BY Id OFFSET 11 ROW FETCH NEXT 1 ROW ONLY))
INSERT INTO [dbo].[CampanhaImagem]([Id],[FileName],[CampanhaId])VALUES(NEWID(),'campanha1.png',(SELECT [Id] FROM Campanha ORDER BY Id OFFSET 12 ROW FETCH NEXT 1 ROW ONLY))
INSERT INTO [dbo].[CampanhaImagem]([Id],[FileName],[CampanhaId])VALUES(NEWID(),'campanha2.jpg',(SELECT [Id] FROM Campanha ORDER BY Id OFFSET 13 ROW FETCH NEXT 1 ROW ONLY))
INSERT INTO [dbo].[CampanhaImagem]([Id],[FileName],[CampanhaId])VALUES(NEWID(),'campanha1.png',(SELECT [Id] FROM Campanha ORDER BY Id OFFSET 13 ROW FETCH NEXT 1 ROW ONLY))
INSERT INTO [dbo].[CampanhaImagem]([Id],[FileName],[CampanhaId])VALUES(NEWID(),'campanha2.jpg',(SELECT [Id] FROM Campanha ORDER BY Id OFFSET 14 ROW FETCH NEXT 1 ROW ONLY))
INSERT INTO [dbo].[CampanhaImagem]([Id],[FileName],[CampanhaId])VALUES(NEWID(),'campanha1.png',(SELECT [Id] FROM Campanha ORDER BY Id OFFSET 15 ROW FETCH NEXT 1 ROW ONLY))
INSERT INTO [dbo].[CampanhaImagem]([Id],[FileName],[CampanhaId])VALUES(NEWID(),'campanha2.jpg',(SELECT [Id] FROM Campanha ORDER BY Id OFFSET 16 ROW FETCH NEXT 1 ROW ONLY))

GO