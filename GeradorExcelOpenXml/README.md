# GeradorExcelOpenXml
Gerar excel via OpenXml com base em um arquivo modelo

# Como utilizar
Utilizar a chamada do método GerarExcelOpenXml como exemplo abaixo passando os parâmetros para geração da planilha

# Exemplo
    GerarExcel.GerarExcelOpenXml(listBase, pathExcelFile, sheetName, colunaDefault, rowDefault, larguraDefault);

# Parametros
* listBase: Lista base com os dados que serão gerados
* pathExcelFile: Local do arquivo que será utilizado para reescrever
* sheetName: Nome da planilha que será aberta para escrita
* colunaDefault: Coluna que contem a célula de estilo para as demais células
* rowDefault: Linha que contem o estilo da para as demais linhas
* larguraDefault: Largura que será aplicada a todas as colunas

# Dicas
* public decimal Id { get; set; } - Caso as propriedades da classe não possuam um DataAnnotations, será utilizada o nome da propriedade para escrever o titulo das colunas no excel (Coluna será gerado com o Título Id)

* [Display(Name = "Id do Usuário")] public decimal Id { get; set; } - Caso as propriedades da classe possuam um DataAnnotations, será utilizada a descrição do DataAnnotations para escrever o titulo das colunas no excel  (Coluna será gerado com o Título "Id do Usuário")

# DataAnnotations 
https://docs.microsoft.com/pt-br/dotnet/api/system.componentmodel.dataannotations?view=netframework-4.7.2

# Pacote Nuget
https://www.nuget.org/packages/GeradorExcelOpenXml/   

# Exemplo prático
            #Classe Base
            public class listBase
            {
                [Display(Name = "Nome do Usuário")]
                public string NomeUsuario { get; set; }
                [Display(Name = "Endereço do Usuário")]
                public string Endereco { get; set; }
            }
            
            #Usando a classe
            var listUsuarios = new List<listBase>();
            listUsuarios.Add(usuario);
            
            #Gerando o Excel passando o obj listUsuarios como referencia
            GerarExcel.GerarExcelOpenXml(listUsuarios, "c:\excel\excelDefault.xlsx", "Usuarios", "A", 1, 25);
