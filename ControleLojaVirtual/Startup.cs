using ControleLojaVirtual.Context;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
//using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ControleLojaVirtual
{
    public class Startup
    {

        public Startup(Microsoft.AspNetCore.Hosting.IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                .AddEnvironmentVariables();
            Configuration = builder.Build();
        }

        public IConfigurationRoot Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<LvContext>(opt => opt.UseInMemoryDatabase("LojaVirtualTeste"));
            services.AddControllers();

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options => {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = "lojavirtual.com",
                    ValidAudience = "lojavirtual.com",
                    IssuerSigningKey = new SymmetricSecurityKey(
                        Encoding.UTF8.GetBytes(Configuration["SecurityKey"]))
                };

                options.Events = new JwtBearerEvents
                {
                    OnAuthenticationFailed = context =>
                    {
                        Console.WriteLine("Token inválido..:. " + context.Exception.Message);
                        return Task.CompletedTask;
                    },
                    OnTokenValidated = context =>
                    {
                        Console.WriteLine("Toekn válido...: " + context.SecurityToken);
                        return Task.CompletedTask;
                    }
                };

            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            
            using (var serviceScope = app.ApplicationServices.CreateScope())
            {
                var context = serviceScope.ServiceProvider.GetService<LvContext>();

                AdicionarDadosTeste(context);
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });


        }

        private static void AdicionarDadosTeste(LvContext context)
       {
            var testeLoja = new Models.Loja
            {
                Nome = "Loja de Teste 1",
                Site = "lojavirtual1.com",
                Endereco = "Rua dos testes 1, 1000 - Vila Debud - SP / 01000-000 "
            };
            context.Lojas.Add(testeLoja);
            testeLoja = new Models.Loja
            {
                Nome = "Loja de Teste 2",
                Site = "lojavirtual2.com",
                Endereco = "Rua dos testes 2, 2000 - Vila Debud - SP / 02000-000 "
            };
            context.Lojas.Add(testeLoja);
            testeLoja = new Models.Loja
            {
                Nome = "Loja de Teste 3",
                Site = "lojavirtual3.com",
                Endereco = "Rua dos testes 3, 3000 - Vila Debud - SP / 03000-000 "
            };
            context.Lojas.Add(testeLoja);

            var testeProduto = new Models.Produto
            {
                Nome = "Produto Teste 1",
                Valor = 10.00,
                Custo = 1.0
            };
            context.Produtos.Add(testeProduto);
            testeProduto = new Models.Produto
            {
                Nome = "Produto Teste 2",
                Valor = 20.00,
                Custo = 10.0
            };
            context.Produtos.Add(testeProduto);
            testeProduto = new Models.Produto
            {
                Nome = "Produto Teste 3",
                Valor = 33.33,
                Custo = 12.33
            };
            context.Produtos.Add(testeProduto);
            testeProduto = new Models.Produto
            {
               Nome = "Produto Teste 4",
                Valor = 44.44,
                Custo = 14.0
            };
            context.Produtos.Add(testeProduto);
            testeProduto = new Models.Produto
            {
                Nome = "Produto Teste 5",
                Valor = 50.00,
                Custo = 15.0
            };
            context.Produtos.Add(testeProduto);

            var testeEstoque = new Models.ItemEstoque
            {
                IdLoja = 1,
                IdProduto = 1,
                Estoque = 10.00,
                EstoqueCompra = 10.00,
                Endereco = "Rua 1"
            };
            context.ItemEstoques.Add(testeEstoque);
            testeEstoque = new Models.ItemEstoque
            {
                IdLoja = 1,
                IdProduto = 2,
                Estoque = 19.00,
                EstoqueCompra = 20.00,
                EstoqueVenda = 1.0,
                Endereco = "Rua 2"
            };
            context.ItemEstoques.Add(testeEstoque);
            testeEstoque = new Models.ItemEstoque
            {
                IdLoja = 2,
                IdProduto = 5,
                Estoque = 510.00,
                EstoqueCompra = 600.00,
                EstoqueVenda = 90.0,
                Endereco = "CORREDOR A PRATELEIRA 10"
            };
            context.ItemEstoques.Add(testeEstoque);
            testeEstoque = new Models.ItemEstoque
            {
                IdLoja = 2,
                IdProduto = 4,
                Estoque = 1000.00,
                EstoqueCompra = 1200.00,
                EstoqueVenda = 200.00,
                Endereco = "CORREDOR B PRATELEIRA 4"
            };
            context.ItemEstoques.Add(testeEstoque);
            testeEstoque = new Models.ItemEstoque
            {
                IdLoja = 2,
                IdProduto = 1,
                Estoque = 99.00,
                EstoqueCompra = 200.00,
                EstoqueVenda = 101.00,
                Endereco = "CORREDOR A PRATELEIRA 2"
            };
            context.ItemEstoques.Add(testeEstoque);
            testeEstoque = new Models.ItemEstoque
            {
                IdLoja = 2,
                IdProduto = 3,
                Estoque = 45.50,
                EstoqueCompra = 200.00,
                EstoqueVenda = 154.50,
                Endereco = "CORREDOR C PRATELEIRA 1"
            };
            context.ItemEstoques.Add(testeEstoque);
            testeEstoque = new Models.ItemEstoque
            {
                IdLoja = 2,
                IdProduto = 1,
                Estoque = 50.00,
                EstoqueCompra = 500.00,
                EstoqueVenda = 450.00,
                Endereco = "CORREDOR C PRATELEIRA 3"
            };
            context.ItemEstoques.Add(testeEstoque);
            testeEstoque = new Models.ItemEstoque
            {
                IdLoja = 3,
                IdProduto = 3,
                Estoque = 125.80,
                EstoqueCompra = 200.00,
                EstoqueVenda = 74.20,
                Endereco = "DEPARTAMENTO X SEÇÃO 8"
            };
            context.ItemEstoques.Add(testeEstoque);
            testeEstoque = new Models.ItemEstoque
            {
                IdLoja = 3,
                IdProduto = 5,
                Estoque = 920.00,
                EstoqueCompra = 4500.00,
                EstoqueVenda = 3580.00,
                Endereco = "DEPARTAMENTO Y SEÇÃO 1"
            };
            context.ItemEstoques.Add(testeEstoque);
            testeEstoque = new Models.ItemEstoque
            {
                IdLoja = 2,
                IdProduto = 1,
                Estoque = 20.00,
                EstoqueCompra = 10333.00,
                EstoqueVenda = 10313.00,
                Endereco = "DEPARTAMENTO W SEÇÃO 2"
            };
            context.ItemEstoques.Add(testeEstoque);
            context.SaveChanges();
        }
    }
}
