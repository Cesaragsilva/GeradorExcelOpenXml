# GeradorExcelOpenXml
Gerar excel via OpenXml com base em um arquivo modelo

# Como utilizar
Utilizar a chamada do método GerarExcelOpenXml como exemplo abaixo.

# Exemplo
    GerarExcel.GerarExcelOpenXml(listBase, pathExcelFile, sheetName, colunaDefault, rowDefault, larguraDefault);

# Parametros
* listBase: Lista base com os dados que serão gerados
* pathExcelFile: Local do arquivo que será utilizado para reescrever
* sheetName: Nome da planilha que será aberta para escrita
* colunaDefault: Coluna que contem a célula de estilo para as demais células
* rowDefault: Linha que contem o estilo da para as demais linhas
* larguraDefault: Largura que será aplicada a todas as colunas
