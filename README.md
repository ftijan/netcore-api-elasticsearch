# ASP.Net 5.0 API project with Elasticsearch integration

## Using Elasticsearch as a data store
- Based on the following YouTube video tutorial: [link](https://www.youtube.com/watch?v=9tkrDqMbFMg)
- Covers fetching the data from ES and inserting items into it

## Using Elasticsearch as a log store with Serilog
- Based on the following YouTube video tutorial: [link](https://www.youtube.com/watch?v=0acSdHJfk64)

## Elasticsearch Docker setup
- Example assumes `Docker Desktop` is used
- Elasticsearch version used: `7.13.3`
- Get an Elastisearch Docker image:
  - `docker pull elasticsearch:7.13.3`
- Run the image
  - `docker run -p 9200:9200 -p 9300:9300 -e "discovery.type=single-node" elasticsearch:7.13.3`
  
## Checking that Elasticsearch is running:
- Check all indices:
  - `GET http://localhost:9200/_aliases`  
- Inserting data under a particular index:
  - `POST http://localhost:9200/{index-name}/_bulk` with test data
  - Example: `POST http://localhost:9200/employees/_bulk`
  - Inserting test data (invalid JSON, but expected by Elasticsearch):
```json
{"index":{}}
{"id":1,"name":"Joe","description":"developer"}
{"index":{}}
{"id":2,"name":"Bob","description":"accountant"}
{"index":{}}
{"id":3,"name":"Steve","description":"customer care"}

// requires the newline under the rows
```
- Get all data by index name:
  - `GET http://localhost:9200/{index-name}/_search`
  - Example: `GET http://localhost:9200/employees/_search`