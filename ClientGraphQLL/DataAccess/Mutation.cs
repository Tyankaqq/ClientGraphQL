﻿using GraphQL.Client.Http;
using GraphQL.Client.Serializer.Newtonsoft;
using GraphQL;
using ModernHttpClient;
using Newtonsoft.Json;

namespace ClientGraphQL.DataAccess
{
    public class Mutation
    {
        private static GraphQLHttpClient graphQLHttpClient;

        static Mutation()
        {
            var uri = new Uri("http://localhost:5000/graphql/"); // Убедитесь, что URL правильный
            var graphQLOptions = new GraphQLHttpClientOptions
            {
                EndPoint = uri,
                HttpMessageHandler = new NativeMessageHandler(),
            };
            graphQLHttpClient = new GraphQLHttpClient(graphQLOptions, new NewtonsoftJsonSerializer());
        }

        public static async Task<T> ExecuteMutationAsync<T>(string graphQLQueryType, string completeQueryString)
        {
            try
            {
                var request = new GraphQLRequest
                {
                    Query = completeQueryString
                };

                var response = await graphQLHttpClient.SendMutationAsync<object>(request);
                var stringResult = response.Data.ToString();

                stringResult = stringResult!.Replace($"\"{graphQLQueryType}\":", string.Empty);
                stringResult = stringResult.Remove(0, 1);
                stringResult = stringResult.Remove(stringResult.Length - 1, 1);

                var result = JsonConvert.DeserializeObject<T>(stringResult);
                return result!;
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}
