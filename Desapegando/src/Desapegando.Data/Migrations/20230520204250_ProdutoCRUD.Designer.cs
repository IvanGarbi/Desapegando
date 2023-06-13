﻿// <auto-generated />
using System;
using Desapegando.Data.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace Desapegando.Data.Migrations
{
    [DbContext(typeof(DesapegandoDbContext))]
    [Migration("20230520204250_ProdutoCRUD")]
    partial class ProdutoCRUD
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.15")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder, 1L, 1);

            modelBuilder.Entity("Desapegando.Business.Models.Condomino", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Apartamento")
                        .IsRequired()
                        .HasColumnType("VARCHAR(20)");

                    b.Property<bool>("Ativo")
                        .HasColumnType("BIT");

                    b.Property<string>("Cpf")
                        .IsRequired()
                        .HasColumnType("VARCHAR(11)");

                    b.Property<DateTime>("DataNascimento")
                        .HasColumnType("DATE");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("VARCHAR(100)");

                    b.Property<string>("Nome")
                        .IsRequired()
                        .HasColumnType("VARCHAR(100)");

                    b.Property<int>("Sexo")
                        .HasColumnType("int");

                    b.Property<string>("Sobrenome")
                        .IsRequired()
                        .HasColumnType("VARCHAR(100)");

                    b.Property<string>("Telefone")
                        .IsRequired()
                        .HasColumnType("VARCHAR(20)");

                    b.HasKey("Id");

                    b.ToTable("Condomino", (string)null);
                });

            modelBuilder.Entity("Desapegando.Business.Models.Produto", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<bool>("Ativo")
                        .HasColumnType("BIT");

                    b.Property<int>("Categoria")
                        .HasColumnType("int");

                    b.Property<Guid>("CondominoId")
                        .HasColumnType("UNIQUEIDENTIFIER");

                    b.Property<int>("Curtida")
                        .HasColumnType("INT");

                    b.Property<DateTime>("DataPublicacao")
                        .HasColumnType("DATETIME");

                    b.Property<string>("Descricao")
                        .IsRequired()
                        .HasMaxLength(250)
                        .HasColumnType("NVARCHAR(250)");

                    b.Property<bool>("Desistencia")
                        .HasColumnType("BIT");

                    b.Property<int>("EstadoProduto")
                        .HasColumnType("int");

                    b.Property<string>("Nome")
                        .IsRequired()
                        .HasMaxLength(150)
                        .HasColumnType("NVARCHAR(150)");

                    b.Property<decimal>("Preco")
                        .HasColumnType("DECIMAL");

                    b.Property<int>("Quantidade")
                        .HasColumnType("INT");

                    b.HasKey("Id");

                    b.HasIndex("CondominoId");

                    b.ToTable("Produto", (string)null);
                });

            modelBuilder.Entity("Desapegando.Business.Models.ProdutoImagem", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("FileName")
                        .IsRequired()
                        .HasMaxLength(150)
                        .HasColumnType("NVARCHAR(150)");

                    b.Property<Guid>("ProdutoId")
                        .HasColumnType("UNIQUEIDENTIFIER");

                    b.HasKey("Id");

                    b.HasIndex("ProdutoId");

                    b.ToTable("ProdutoImagem", (string)null);
                });

            modelBuilder.Entity("Desapegando.Business.Models.Produto", b =>
                {
                    b.HasOne("Desapegando.Business.Models.Condomino", "Condomino")
                        .WithMany("Produtos")
                        .HasForeignKey("CondominoId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Condomino");
                });

            modelBuilder.Entity("Desapegando.Business.Models.ProdutoImagem", b =>
                {
                    b.HasOne("Desapegando.Business.Models.Produto", "Produto")
                        .WithMany("ProdutoImagens")
                        .HasForeignKey("ProdutoId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Produto");
                });

            modelBuilder.Entity("Desapegando.Business.Models.Condomino", b =>
                {
                    b.Navigation("Produtos");
                });

            modelBuilder.Entity("Desapegando.Business.Models.Produto", b =>
                {
                    b.Navigation("ProdutoImagens");
                });
#pragma warning restore 612, 618
        }
    }
}