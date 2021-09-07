# Concorrência entre o Portal de Notícias e Editor

## 1. Editor MicroBlog

#### Diagrama
![Editor Blog](/diagramas/out/editor_blog/Editor%20Blog.png)

#### Persistência no DynamoDB


Campo | Descrição
----- | ---------
**PK**| data do artigo (2021-09-07)
**SK**| titulo-artigo
**Categoria**| Esporte/Geral/
**Url**| caminho S3 para oartigo
**PublicadoEm**| 2021-09-07 15:30 
Pais?| 
Esporte?| (Natação)
Modalidade?| (100m / feminino)
Tags?|[]

?: Campos não obrigatórios


## 2. Página Inicial
#### Diagrama
![Página Inicial](/diagramas/out/pagina_inicial/Pagina%20inicial.png)