# RequisicoesApiBase
Efetuar requisições (GET,POST,PUT,DELETE) com autorização do tipo Bearer (pode ser obtida via a interface IServicoRequisicaoBaseToken)

# Como utilizar
Via injeção de dependência utilizar as interfaces IServicoRequisicaoBaseHttpMetodos e IServicoRequisicaoBaseToken para utilização de requisições e geração de Tokens de acordo com o IdentityProvider enviado

# IdentityServer 
http://docs.identityserver.io/en/latest/
# DependencyInjection
https://github.com/autofac/Autofac
https://docs.microsoft.com/en-us/dotnet/api/microsoft.extensions.dependencyinjection?view=aspnetcore-2.2
https://docs.microsoft.com/en-us/dotnet/api/microsoft.extensions.dependencyinjection.iservicecollection?view=aspnetcore-2.2

# Pacote Nuget
https://www.nuget.org/packages/   

# Registro via Injeção de Dependência
Na classe Startup do projeto, no método ConfigureServices(IServiceCollection services) configure a Injeção de Dependência
services.AddSingleton<IServicoRequisicaoBaseHttpMetodos, ServicoRequisicaoBaseHttpMetodos>();
services.AddSingleton<IServicoRequisicaoBaseToken, ServicoRequisicaoBaseToken>();

ou utilize o Autofac(Ou qualquer outro para ID)
		
		#Via Autofac
		builder.RegisterType<ServicoRequisicaoBaseHttpMetodos>().As<IServicoRequisicaoBaseHttpMetodos>();

		#ID Default .NET Core
		public IServiceProvider ConfigureServices(IServiceCollection services)
        	{
		    services.AddSingleton<IServicoRequisicaoBaseHttpMetodos, ServicoRequisicaoBaseHttpMetodos>();
		    services.AddSingleton<IServicoRequisicaoBaseToken, ServicoRequisicaoBaseToken>();
		 }

# Exemplo de utilização de requisicoes base (GET,POST,PUT,DELETE)
			#Utilização da Interface
			private readonly IServicoRequisicaoBaseHttpMetodos _serverBaseHttpMetodos;
			public meuConstrutor(IServicoRequisicaoBaseHttpMetodos serverBaseHttpMetodos){
				_serverBaseHttpMetodos = serverBaseHttpMetodos;
			}

			#Utilização de request
			var usuarios = _serverBaseHttpMetodos.Get<string>("https://localhost:8100/usuarios");

			#Utilização de request com bearer Token
			_serverBaseHttpMetodos.TokenBearer = "access_token_key";
			var usuarios = _serverBaseHttpMetodos.Get<string>("https://localhost:8100/usuarios");

# Exemplo de utilização de requisicoes de token
* hostIdentityServer: Url do IdentityServer (Apenas a url, pois o discovery será executado internamente para obter a url do token)
* clientId: Id de identificação do client registrado no identityServer
* clientSecret: Secret do clientId (não obrigatório na request, pois depende do tipo de configuração do clientId)
* scope: Scopes permitidos pelo clientId (não obrigatório, assim solicito todos os escopes)
* username: login do usuário para identificação quando solicitado o token com o flow ResourceOwnerPassword
* password: password do usuário para identificação quando solicitado o token com o flow ResourceOwnerPassword

		#Utilização da Interface
			private readonly IServicoRequisicaoBaseToken _serverToken;
			public meuConstrutor(IServicoRequisicaoBaseToken serverToken){
				_serverToken = serverToken;
			}

			#Solicitação do Token (ClientCredentians e ClientPassword)
			var token = _serverToken.GerarTokenClientPasswordAsync("https://localhost:44300", "client_id", "username", "password");
