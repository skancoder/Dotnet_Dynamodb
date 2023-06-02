﻿using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.Model;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MovieRank.Integration.Tests.Setup
{
    public class TestDataSetup
    {
        private static readonly IAmazonDynamoDB DynamoDBClient = new AmazonDynamoDBClient(new AmazonDynamoDBConfig
        {
            ServiceURL = "http://localhost:8000"
        });

        public TestDataSetup()
        {
            Task.Run(CreateTable);
        }

        public async Task CreateTable()
        {
            var request = new CreateTableRequest
            {
                AttributeDefinitions = new List<AttributeDefinition>()
                {
                    new AttributeDefinition
                    {
                        AttributeName = "UserId",
                        AttributeType = "N"
                    },
                    new AttributeDefinition
                    {
                        AttributeName = "MovieName",
                        AttributeType = "S"
                    }
                },
                KeySchema = new List<KeySchemaElement>
                {
                    new KeySchemaElement
                    {
                        AttributeName = "UserId",
                        KeyType = "HASH"
                    },
                    new KeySchemaElement
                    {
                        AttributeName = "MovieName",
                        KeyType = "RANGE"
                    }
                },
                ProvisionedThroughput = new ProvisionedThroughput
                {
                    ReadCapacityUnits = 1,
                    WriteCapacityUnits = 1
                },
                TableName = "MovieRank",

                GlobalSecondaryIndexes = new List<GlobalSecondaryIndex>
                {
                    new GlobalSecondaryIndex
                    {
                        IndexName = "MovieName-index",
                        KeySchema = new List<KeySchemaElement>
                        {
                            new KeySchemaElement
                            {
                                AttributeName = "MovieName",
                                KeyType = "HASH"
                            }
                        },
                        ProvisionedThroughput = new ProvisionedThroughput
                        {
                            ReadCapacityUnits = 1,
                            WriteCapacityUnits = 1
                        },
                        Projection = new Projection
                        {
                            ProjectionType = "ALL" // sepcifies the attributes that are copied or projected from table to secondary index. ALL: copy all
                        }
                    }
                }
            };

            await DynamoDBClient.CreateTableAsync(request);
        }

        //create table takes time. so wait till its done
        private static async Task WaitUntilTableActive(string tableName)
        {
            string status = null;
            do
            {
                Thread.Sleep(5000);
                try
                {
                    status = await GetTableStatus(tableName);
                }
                catch (ResourceNotFoundException) //DescribeTable is eventually consistent, it might get this exception
                {
                    // DescribeTable is eventually consistent. So you might
                    // get resource not found. So we handle the potential exception.
                }

            } while (status != "ACTIVE");
        }

        private static async Task<string> GetTableStatus(string tableName)
        {
            var response = await DynamoDBClient.DescribeTableAsync(new DescribeTableRequest
            {
                TableName = tableName
            });

            return response.Table.TableStatus;
        }

        public static void TearDownStore(string tableName)
        {
            var request = new DeleteTableRequest
            {
                TableName = tableName
            };

            DynamoDBClient.DeleteTableAsync(request);
        }
    }
}
