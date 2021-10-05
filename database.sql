CREATE DATABASE PROJETO_SPRINT_5;

USE PROJETO_SPRINT_5;

CREATE TABLE Cidades (
  Id UNIQUEIDENTIFIER NOT NULL,
  Nome VARCHAR(200) NOT NULL,
  Estado VARCHAR(2) NOT NULL,
  CONSTRAINT PK_Cidades PRIMARY KEY (Id)
);

CREATE TABLE Clientes (
  Id UNIQUEIDENTIFIER NOT NULL,
  CidadeId UNIQUEIDENTIFIER NOT NULL,
  Nome VARCHAR(200) NOT NULL,
  DataNascimento DATE,
  Cep VARCHAR(8),
  Logradouro VARCHAR(200),
  Bairro VARCHAR(200),
  CONSTRAINT PK_Cliente PRIMARY KEY (Id),
  CONSTRAINT FK_Clientes_Cidades_CidadeId FOREIGN KEY (CidadeId) REFERENCES Cidades(Id)
);