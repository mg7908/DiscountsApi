using Microsoft.AspNetCore.Mvc.Testing;
using System.Net;
using System.Text;
using System.Text.Json.Nodes;

namespace Discounts.Tests;

public class DiscountEndpointTests(WebApplicationFactory<Program> factory) : IClassFixture<WebApplicationFactory<Program>>
{
    [Fact]
    public async Task CalculateDiscount_ExampleRequest_ReturnsExpectedResponse()
    {
        var client = factory.CreateClient();

        var requestJson = File.ReadAllText("Requests\\ExampleRequest1.json");
        var reponseMessage = await client.PostAsync("/discounts/calculate", new StringContent(requestJson, Encoding.UTF8, "application/json"));

        var actualResponseJson = await reponseMessage.Content.ReadAsStringAsync();
        var expectedResponseJson = File.ReadAllText("Responses\\ExampleResponse1.json");

        Assert.Equal(HttpStatusCode.OK, reponseMessage.StatusCode);

        AssertJsonEqual(expectedResponseJson, actualResponseJson);
    }

    private static void AssertJsonEqual(string expectedJson, string actualJson)
    {
        var expectedNode = JsonNode.Parse(expectedJson);
        var actualNode = JsonNode.Parse(actualJson);

        Assert.True(JsonNode.DeepEquals(expectedNode, actualNode));
    }
}
